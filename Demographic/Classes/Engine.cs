using Demographic.Models;
using Demographic.Interfaces;

namespace Demographic.Classes
{
    /// @ingroup core_system
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
                    var person = new Person(age, gender, DeathRules, this);
                    person.ChildBirth += OnChildBirth;
                    _persons.Add(person);
                }
            }
        }

        private void OnChildBirth(ChildBirthEventArgs e)
        {
            var child = new Person(0, e.ChildGender, DeathRules, this);
            child.ChildBirth += OnChildBirth;
            _persons.Add(child);
        }

        public void RunSimulation(int endYear)
        {
            while (_currentYear <= endYear)
            {
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
                MalePopulation = alivePersons.Count(persons => persons.Gender == Gender.Male) * (int)Constants.INITIAL_POPULATION_SCALE,
                FemalePopulation = alivePersons.Count(persons => persons.Gender == Gender.Female) * (int)Constants.INITIAL_POPULATION_SCALE
            };

            _simulationResult.YearlyStatistics.Add(stats);
        }

        public List<Person> GetPopulation() => _persons;
        public SimulationResult GetSimulationResult() => _simulationResult;
    }
}