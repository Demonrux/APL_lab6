using System.Collections.Generic;
using Demographic.Models;

namespace Demographic.FileOperations.Interfaces
{
    public interface IInitialAgeReader
    {
        Dictionary<int, double> ReadAgeDistribution(string filePath);
    }
}