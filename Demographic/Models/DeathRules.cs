namespace Demographic.Models
{
    public class DeathRules
    {
        public List<DeathRule> Rules { get; set; } = new List<DeathRule>();

        public void AddRule(DeathRule rule)
        {
            Rules.Add(rule);
        }

        public double GetDeathProbability(int age, Gender gender)
        {
            var rule = Rules.FirstOrDefault(r => age >= r.StartAge && age <= r.EndAge);
            if (rule != null)
            {
                return gender == Gender.Male ? rule.DeathProbabilityMale : rule.DeathProbabilityFemale;
            }

            return Constants.DEFAULT_DEATH_PROBABILITY;
        }
    }

    public class DeathRule
    {
        public int StartAge { get; set; }
        public int EndAge { get; set; }
        public double DeathProbabilityMale { get; set; }
        public double DeathProbabilityFemale { get; set; }
    }
}

