namespace Demographic.Models
{
    public class InitialAgeData
    {
        public Dictionary<int, double> AgePercentages { get; private set; }

        public InitialAgeData()
        {
            AgePercentages = new Dictionary<int, double>();
        }

        public void CalculatePercentages(Dictionary<int, double> ageDistribution)
        {
            AgePercentages.Clear();
            double totalPer1000 = ageDistribution.Values.Sum();

            foreach (var (age, countPer1000) in ageDistribution)
            {
                double percentage = countPer1000 / totalPer1000;
                AgePercentages[age] = percentage;
            }
        }

        public double GetPercentageForAge(int age)
        {
            return AgePercentages.ContainsKey(age) ? AgePercentages[age] : 0;
        }
    }
}