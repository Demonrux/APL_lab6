using Demographic.Models;

namespace Demographic.FileOperations.Interfaces
{
    public interface IDeathRulesReader
    {
        DeathRules ReadDeathRules(string filePath);
    }
}
