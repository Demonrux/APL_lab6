using System.Globalization;
using Demographic.FileOperations.Interfaces;

namespace Demographic.FileOperations.Classes
{
    public class CsvInitialAgeReader : IInitialAgeReader
    {
        public Dictionary<int, double> ReadAgeDistribution(string filePath)
        {
            var distribution = new Dictionary<int, double>();

            try
            {
                var lines = File.ReadAllLines(filePath);

                foreach (var line in lines.Skip(1))
                {
                    var parts = line.Split(',');

                    if (parts.Length >= 2)
                    {
                        int age = int.Parse(parts[0].Trim());
                        double countPer1000 = double.Parse(parts[1].Trim(), CultureInfo.InvariantCulture);

                        distribution[age] = countPer1000 / 1000.0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка чтения файла {filePath}", ex);
            }

            return distribution;
        }
    }
}