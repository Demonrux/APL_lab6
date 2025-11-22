using Demographic.Interfaces;
using Demographic.Models;

namespace Demographic.Classes
{
    /// @ingroup core_system
    public class Person
    {
        public int Age { get; private set; }
        public Gender Gender { get; private set; }
        public bool IsAlive { get; private set; } = true;
        public int? DeathYear { get; private set; }

        private readonly DeathRules _deathRules;
        private readonly IEngine _engine;

        public event Action<ChildBirthEventArgs> ChildBirth;

        public Person(int age, Gender gender, DeathRules deathRules, IEngine engine)
        {
            Age = age;
            Gender = gender;
            _deathRules = deathRules;
            _engine = engine;

            _engine.YearTick += ProcessYear;
        }

        public void ProcessYear(int currentYear)
        {
            if (!IsAlive) return;

            double deathProbability = _deathRules.GetDeathProbability(Age, Gender);

            if (ProbabilityCalculator.IsEventHappened(deathProbability))
            {
                IsAlive = false;
                DeathYear = currentYear;
                _engine.YearTick -= ProcessYear;
                return;
            }

            Age++;

            if (IsAlive && Gender == Gender.Female &&
                Age >= Constants.MIN_CHILDBEARING_AGE &&
                Age <= Constants.MAX_CHILDBEARING_AGE)
            {
                if (ProbabilityCalculator.IsEventHappened(Constants.BIRTH_PROBABILITY))
                {
                    OnChildBirth(currentYear);
                }
            }
        }

        protected virtual void OnChildBirth(int currentYear)
        {
            var childGender = ProbabilityCalculator.IsEventHappened(Constants.FEMALE_BIRTH_PROBABILITY)
                ? Gender.Female
                : Gender.Male;
            ChildBirth?.Invoke(new ChildBirthEventArgs(childGender, currentYear));
        }
    }
}