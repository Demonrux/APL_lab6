using Demographic.Models;

namespace Demographic.Classes
{
    public class Person
    {
        public int Age { get; private set; }
        public Gender Gender { get; private set; }
        public bool IsAlive { get; private set; } = true;
        public int? DeathYear { get; private set; }

        public Person(int age, Gender gender)
        {
            Age = age;
            Gender = gender;
        }

        public bool ProcessYear(int currentYear, DeathRules deathRules)
        {
            if (!IsAlive) return false;

            double deathProbability = deathRules.GetDeathProbability(Age, Gender);

            if (ProbabilityCalculator.IsEventHappened(deathProbability))
            {
                IsAlive = false;
                DeathYear = currentYear;
                return false;
            }

            Age++;

            bool childBorn = false;
            if (IsAlive && Gender == Gender.Female &&
                Age >= Constants.MIN_CHILDBEARING_AGE &&
                Age <= Constants.MAX_CHILDBEARING_AGE)
            {
                if (ProbabilityCalculator.IsEventHappened(Constants.BIRTH_PROBABILITY))
                {
                    childBorn = true;
                }
            }

            return childBorn;
        }
    }
}
