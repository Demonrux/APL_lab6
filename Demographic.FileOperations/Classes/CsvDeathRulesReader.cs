using System.Globalization;
using Demographic.FileOperations.Interfaces;
using Demographic.Models;

namespace Demographic.FileOperations.Classes
{
    public class CsvDeathRulesReader : IDeathRulesReader
    {
        public DeathRules ReadDeathRules(string filePath)
        {
            var deathRules = new DeathRules();

            try
            {
                var lines = File.ReadAllLines(filePath);

                foreach (var line in lines.Skip(1))
                {
                    var parts = line.Split(',');

                    if (parts.Length >= 4)
                    {
                        var rule = new DeathRule
                        {
                            StartAge = int.Parse(parts[0].Trim()),
                            EndAge = int.Parse(parts[1].Trim()),
                            DeathProbabilityMale = double.Parse(parts[2].Trim(), CultureInfo.InvariantCulture),
                            DeathProbabilityFemale = double.Parse(parts[3].Trim(), CultureInfo.InvariantCulture)
                        };

                        deathRules.AddRule(rule);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка чтения файла правил смертности {filePath}", ex);
            }

            return deathRules;
        }
    }
}