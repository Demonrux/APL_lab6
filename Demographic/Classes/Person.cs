using Demographic.Models;

namespace Demographic.Classes
{
    public class Person
    {
        public int Age { get; private set; }
        public Gender Gender { get; private set; }
        public bool IsAlive { get; private set; } = true;
        public int? DeathYear { get; private set; }

        public event Action<Person, ChildBirthEventArgs> ChildBirth;

        public Person(int age, Gender gender)
        {
            Age = age;
            Gender = gender;
        }

        public void ProcessYear(int currentYear, DeathRules deathRules)
        {
            if (!IsAlive) return;

            double deathProbability = deathRules.GetDeathProbability(Age, Gender);

            if (ProbabilityCalculator.IsEventHappened(deathProbability))
            {
                IsAlive = false;
                DeathYear = currentYear;
                return;
            }

            Age++;

            if (IsAlive && Gender == Gender.Female &&Age >= Constants.MIN_CHILDBEARING_AGE && Age <= Constants.MAX_CHILDBEARING_AGE)
            {
                if (ProbabilityCalculator.IsEventHappened(Constants.BIRTH_PROBABILITY))
                    OnChildBirth(currentYear);
            }
        }

        protected virtual void OnChildBirth(int currentYear)
        {
            var childGender = ProbabilityCalculator.IsEventHappened(Constants.FEMALE_BIRTH_PROBABILITY)? Gender.Female : Gender.Male;
            ChildBirth?.Invoke(this, new ChildBirthEventArgs(childGender, currentYear));
        }
    }
}
