using Demographic.Classes;
using Demographic.Models;

namespace Demographic.FileOperations.Interfaces
{
    public interface IResultWriter
    {
        void WritePopulationData(List<DemographicStats> yearlyStats, string filePath);
        void WritePeopleData(List<Person> people, string filePath);
    }
}
