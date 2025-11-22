using Demographic.Models;
using Demographic.Interfaces;

namespace Demographic.Classes
{
    public class Engine : IEngine
    {
        public event Action<int, DeathRules> YearTick;

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
                    var gender = ProbabilityCalculator.IsEventHappened(Constants.DEFAULT_GENDER_PROBABILITY)
                        ? Gender.Male
                        : Gender.Female;
                    var person = new Person(age, gender);
                    person.ChildBirth += OnChildBirth;

                    YearTick += person.ProcessYear;

                    _persons.Add(person);
                }
            }
        }

        private void OnChildBirth(Gender childGender)
        {
            var child = new Person(0, childGender);
            child.ChildBirth += OnChildBirth;

            YearTick += child.ProcessYear;

            _persons.Add(child);
        }

        public void RunSimulation(int endYear)
        {
            while (_currentYear <= endYear)
            {
                YearTick?.Invoke(_currentYear, DeathRules);

                var deadPersons = _persons.Where(persons => !persons.IsAlive).ToList();
                foreach (var deadPerson in deadPersons)
                {
                    YearTick -= deadPerson.ProcessYear; 
                    deadPerson.ChildBirth -= OnChildBirth;
                }

                SaveYearlyStats();
                _currentYear++;
            }
        }

        private void SaveYearlyStats()
        {

            var stats = new DemographicStats
            {
                Year = _currentYear,
                TotalPopulation = _persons.Count * (int)Constants.INITIAL_POPULATION_SCALE,
                MalePopulation = _persons.Count(p => p.Gender == Gender.Male) * (int)Constants.INITIAL_POPULATION_SCALE,
                FemalePopulation = _persons.Count(p => p.Gender == Gender.Female) * (int)Constants.INITIAL_POPULATION_SCALE
            };

            _simulationResult.YearlyStatistics.Add(stats);
        }

        public List<Person> GetPopulation() => _persons;
        public SimulationResult GetSimulationResult() => _simulationResult;
    }
}
