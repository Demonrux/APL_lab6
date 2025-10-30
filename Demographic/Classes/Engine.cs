using Demographic.Models;
using Demographic.Interfaces;

namespace Demographic.Classes
{
    public class Engine : IEngine
    {
        private List<Person> _persons;
        private DeathRules _deathRules;
        private int _currentYear;
        private SimulationResult _simulationResult;
        private int _birthsThisYear;
        private int _deathsThisYear;

        public event EventHandler<int> YearTick;

        public void Initialize(double totalPopulation, int startYear, InitialAgeData ageData, DeathRules deathRules)
        {
            _persons = new List<Person>();
            _deathRules = deathRules;
            _currentYear = startYear;
            _simulationResult = new SimulationResult();

            InitializeInitialPopulation(totalPopulation, ageData);
        }

        private void InitializeInitialPopulation(double totalPopulation, InitialAgeData ageData)
        {
            var random = new Random();
            int totalPersonObjects = (int)(totalPopulation / 1000);

            foreach (var (age, percentage) in ageData.AgePercentages)
            {
                int countForThisAge = (int)(totalPersonObjects * percentage);

                for (int i = 0; i < countForThisAge; i++)
                {
                    var gender = random.Next(2) == 0 ? Gender.Male : Gender.Female;
                    var person = new Person(age, gender);
                    person.ChildBirth += OnChildBirth;
                    _persons.Add(person);
                }
            }
        }

        private void OnChildBirth(object sender, ChildBirthEventArgs e)
        {
            var child = new Person(0, e.ChildGender);
            child.ChildBirth += OnChildBirth;
            _persons.Add(child);
            _birthsThisYear++;
        }

        public void RunSimulation(int endYear)
        {
            while (_currentYear <= endYear)
            {
                _birthsThisYear = 0;
                _deathsThisYear = 0;

                var alivePersons = _persons.Where(p => p.IsAlive).ToList();

                foreach (var person in alivePersons)
                {
                    bool wasAlive = person.IsAlive;
                    person.ProcessYear(_currentYear, _deathRules);
                    if (wasAlive && !person.IsAlive)
                    {
                        _deathsThisYear++;
                    }
                }

                SaveYearlyStats();

                YearTick?.Invoke(this, _currentYear);
                _currentYear++;
            }
        }

        private void SaveYearlyStats()
        {
            var alivePersons = _persons.Where(p => p.IsAlive).ToList();

            var stats = new DemographicStats
            {
                Year = _currentYear,
                TotalPopulation = alivePersons.Count * 1000,
                MalePopulation = alivePersons.Count(p => p.Gender == Gender.Male) * 1000,
                FemalePopulation = alivePersons.Count(p => p.Gender == Gender.Female) * 1000
            };

            _simulationResult.YearlyStatistics.Add(stats);
        }

        public List<Person> GetPopulation() => _persons;

        public SimulationResult GetSimulationResult() => _simulationResult;
    }
}