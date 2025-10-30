

namespace Demographic.Classes
{
    public static class ProbabilityCalculator
    {
        private static readonly Random _random = new Random();

        public static bool IsEventHappened(double eventProbability)
        {
            return _random.NextDouble() < eventProbability;
        }
    }
}
