﻿using Demographic.Models;
using Demographic.Interfaces;

namespace Demographic.Classes
{
    public class Engine : IEngine
    {
        public event EventHandler<int> YearTick;
        public DeathRules DeathRules { get; private set; }
        private List<Person> _persons;
        private int _currentYear;
        private SimulationResult _simulationResult;

        public void Initialize(double totalPopulation, int startYear, InitialAgeData ageData, DeathRules deathRules)
        {
            double totalPopulationInObjects = totalPopulation / 1000.0;
            DeathRules = deathRules;
            _persons = new List<Person>();
            _currentYear = startYear;
            _simulationResult = new SimulationResult();

            InitializeInitialPopulation(totalPopulationInObjects, ageData);
        }

        private void InitializeInitialPopulation(double totalPopulation, InitialAgeData ageData)
        {
            var random = new Random();

            foreach (var (age, percentage) in ageData.AgePercentages)
            {
                int countForThisAge = (int)(totalPopulation * percentage);
                if (countForThisAge < 1 && totalPopulation > 0)
                    countForThisAge = 1;

                for (int i = 0; i < countForThisAge; i++)
                {
                    var gender = random.Next(2) == 0 ? Gender.Male : Gender.Female;
                    var person = new Person(age, gender, this);
                    person.ChildBirth += OnChildBirth;
                    _persons.Add(person);
                }
            }
        }

        private void OnChildBirth(object sender, ChildBirthEventArgs e)
        {
            var child = new Person(0, e.ChildGender, this);
            child.ChildBirth += OnChildBirth;
            _persons.Add(child);
        }

        public void RunSimulation(int endYear)
        {
            while (_currentYear <= endYear)
            {
                // Вызываем событие - все люди автоматически обработаются
                YearTick?.Invoke(this, _currentYear);

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
                TotalPopulation = alivePersons.Count,
                MalePopulation = alivePersons.Count(p => p.Gender == Gender.Male),
                FemalePopulation = alivePersons.Count(p => p.Gender == Gender.Female)
            };

            _simulationResult.YearlyStatistics.Add(stats);
        }

        public List<Person> GetPopulation() => _persons;
        public SimulationResult GetSimulationResult() => _simulationResult;
    }
}