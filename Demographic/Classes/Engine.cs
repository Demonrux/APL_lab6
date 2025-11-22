using Demographic.Models;
using Demographic.Interfaces;
using System;

namespace Demographic.Classes
{
    public class Engine : IEngine
    {
        public event Action<int> YearTick;
        public DeathRules DeathRules { get; private set; }
        private List<Person> _persons;
        private int _currentYear;
        private SimulationResult _simulationResult;

        public void Initialize(double totalPopulation, int startYear, InitialAgeData ageData, DeathRules deathRules)
        {
            double totalPopulationInObjects = totalPopulation / Constants.INITIAL_POPULATION_SCALE;
            DeathRules = deathRules;
            _persons = new List<Person>();
            _currentYear = startYear;
            _simulationResult = new SimulationResult();

            InitializeInitialPopulation(totalPopulationInObjects, ageData);
        }

        private void InitializeInitialPopulation(double totalPopulation, InitialAgeData ageData)
        {
            foreach (var (age, percentage) in ageData.AgePercentages)
            {
                int countForThisAge = (int)(totalPopulation * percentage);
                if (countForThisAge < 1 && totalPopulation > 0)
                    countForThisAge = 1;

                for (int i = 0; i < countForThisAge; i++)
                {
                    var gender = ProbabilityCalculator.IsEventHappened(Constants.DEFAULT_GENDER_PROBABILITY) ? Gender.Male : Gender.Female;
                    var person = new Person(age, gender);
                    _persons.Add(person);
                }
            }
        }

        public void RunSimulation(int endYear)
        {
            while (_currentYear <= endYear)
            {
                var newChildren = new List<Person>();

                foreach (var person in _persons.ToList())
                {
                    bool childBorn = person.ProcessYear(_currentYear, DeathRules);

                    if (childBorn)
                    {
                        var childGender = ProbabilityCalculator.IsEventHappened(Constants.FEMALE_BIRTH_PROBABILITY)
                            ? Gender.Female
                            : Gender.Male;
                        var child = new Person(0, childGender);
                        newChildren.Add(child);
                    }
                }

                _persons.AddRange(newChildren);

                YearTick?.Invoke(_currentYear);
                SaveYearlyStats();
                _currentYear++;
            }
        }

        private void SaveYearlyStats()
        {
            var alivePersons = _persons.Where(p => p.IsAlive).ToList();

            var stats = new DemographicStats
            {
                Year = _currentYear,
                TotalPopulation = alivePersons.Count * (int)Constants.INITIAL_POPULATION_SCALE,
                MalePopulation = alivePersons.Count(p => p.Gender == Gender.Male) * (int)Constants.INITIAL_POPULATION_SCALE,
                FemalePopulation = alivePersons.Count(p => p.Gender == Gender.Female) * (int)Constants.INITIAL_POPULATION_SCALE
            };

            _simulationResult.YearlyStatistics.Add(stats);
        }

        public List<Person> GetPopulation() => _persons;
        public SimulationResult GetSimulationResult() => _simulationResult;
    }
}
