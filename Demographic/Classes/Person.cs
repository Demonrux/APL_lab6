using Demographic.Interfaces;
using Demographic.Models;

namespace Demographic.Classes
{
    public class Person
    {
        public int Age { get; private set; }
        public Gender Gender { get; private set; }
        public bool IsAlive { get; private set; } = true;
        public int? DeathYear { get; private set; }

        public event EventHandler<ChildBirthEventArgs> ChildBirth;

        public Person(int age, Gender gender, IEngine engine)
        {
            Age = age;
            Gender = gender;
            engine.YearTick += OnYearTick;
        }

        private void OnYearTick(object sender, int currentYear)
        {
            if (!IsAlive) return;

            var engine = (IEngine)sender;
            double deathProbability = engine.DeathRules.GetDeathProbability(Age, Gender);

            if (ProbabilityCalculator.IsEventHappened(deathProbability))
            {
                IsAlive = false;
                DeathYear = currentYear;
                return;
            }

            Age++;

            if (IsAlive && Gender == Gender.Female && Age >= 18 && Age <= 45)
            {
                double birthProbability = 0.151;
                if (ProbabilityCalculator.IsEventHappened(birthProbability))
                {
                    OnChildBirth(currentYear);
                }
            }
        }

        protected virtual void OnChildBirth(int currentYear)
        {
            var childGender = ProbabilityCalculator.IsEventHappened(0.55) ? Gender.Female : Gender.Male;
            ChildBirth?.Invoke(this, new ChildBirthEventArgs(childGender, currentYear));
        }
    }
}
