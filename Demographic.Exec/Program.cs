using Demographic.Classes;
using Demographic.FileOperations.Classes;
using Demographic.FileOperations.Interfaces;
using Demographic.Models;

namespace Demographic.Exec
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string initialAgeFilePath = "C:\\Users\\Пользователь\\source\\repos\\DemographicProject\\Demographic.Exec\\Files\\InitialAge.csv";
                string deathRulesFilePath = "C:\\Users\\Пользователь\\source\\repos\\DemographicProject\\Demographic.Exec\\Files\\DeathRules.csv";
                string outputPopulationPath = "C:\\Users\\Пользователь\\source\\repos\\DemographicProject\\Demographic.Exec\\Files\\PopulationData.csv";
                string outputPeoplePath = "C:\\Users\\Пользователь\\source\\repos\\DemographicProject\\Demographic.Exec\\Files\\PeopleData.csv";
                int startYear = 1970;
                int endYear = 2022;
                int totalPopulation = 13000000;

                if (args.Length >= 1) initialAgeFilePath = args[0];
               if (args.Length >= 2) deathRulesFilePath = args[1];
               if (args.Length >= 3 && !int.TryParse(args[2], out startYear))
                   throw new ArgumentException("Некорректный начальный год");
               if (args.Length >= 4 && !int.TryParse(args[3], out endYear))
                   throw new ArgumentException("Некорректный конечный год");
               if (args.Length >= 5 && !int.TryParse(args[4], out totalPopulation))
                   throw new ArgumentException("Некорректный размер популяции");
               if (args.Length >= 6) outputPopulationPath = args[5];
               if (args.Length >= 7) outputPeoplePath = args[6];

               if (!File.Exists(initialAgeFilePath))
                   throw new FileNotFoundException($"Файл не найден: {initialAgeFilePath}");
               if (!File.Exists(deathRulesFilePath))
                   throw new FileNotFoundException($"Файл не найден: {deathRulesFilePath}");
               if (startYear >= endYear)
                   throw new ArgumentException("Начальный год должен быть меньше конечного");
               if (totalPopulation <= 0)
                   throw new ArgumentException("Размер популяции должен быть положительным");

                Console.WriteLine("Демографическое моделирование");
                Console.WriteLine();

                IInitialAgeReader ageReader = new CsvInitialAgeReader();
                IDeathRulesReader deathReader = new CsvDeathRulesReader();
                IResultWriter resultWriter = new CsvResultWriter();

                var rawAgeDistribution = ageReader.ReadAgeDistribution(initialAgeFilePath);
                var ageData = new InitialAgeData();
                ageData.CalculatePercentages(rawAgeDistribution);

                var deathRules = deathReader.ReadDeathRules(deathRulesFilePath);

                var engine = new Engine();
                engine.Initialize(totalPopulation, startYear, ageData, deathRules);
                var initialPopulation = engine.GetPopulation();
                Console.WriteLine($"После инициализации: {initialPopulation.Count} объектов Person");

                Console.WriteLine("Запуск...");
                engine.YearTick += (year) => Console.WriteLine($"Обработан год: {year}");

                engine.RunSimulation(endYear);

                var result = engine.GetSimulationResult();
                var population = engine.GetPopulation();

                resultWriter.WritePopulationData(result.YearlyStatistics, outputPopulationPath);
                resultWriter.WritePeopleData(population, outputPeoplePath);

            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка в параметрах: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
            }
        }        
    }
}
