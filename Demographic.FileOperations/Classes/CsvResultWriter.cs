using Demographic.Classes;
using Demographic.FileOperations.Interfaces;
using Demographic.Models;
using System;

namespace Demographic.FileOperations.Classes
{
    public class CsvResultWriter : IResultWriter
    {
        public void WritePopulationData(List<DemographicStats> yearlyStats, string filePath)
        {
            try
            {
                Console.WriteLine($"Путь: {filePath}");
                Console.WriteLine($"Статистика: {yearlyStats?.Count ?? 0} записей");
                
                var lines = new List<string>
                {
                    "year,total_population,male_population,female_population"
                };

                foreach (var stats in yearlyStats)
                {
                    var line = $"{stats.Year},{stats.TotalPopulation:F0},{stats.MalePopulation:F0},{stats.FemalePopulation:F0}";
                    lines.Add(line);
                }

                File.WriteAllLines(filePath, lines);
                Console.WriteLine($"Данные о населении сохранены в: {filePath}");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка записи в файл {filePath}", ex);
            }
        }

        public void WritePeopleData(List<Person> people, string filePath)
        {
            try
            {
                Console.WriteLine($"Запись {people.Count} людей...");
                var lines = new List<string>
                {
                    "Age,Gender,IsAlive,DeathYear"
                };

                foreach (var person in people)
                {
                    var line = $"{person.Age},{person.Gender},{person.IsAlive}, {person.DeathYear}";
                    lines.Add(line);
                }

                File.WriteAllLines(filePath, lines);
                Console.WriteLine($"Данные об отдельных людях сохранены в: {filePath}");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка записи данных о людях в файл {filePath}", ex);
            }
        }
    }
}
