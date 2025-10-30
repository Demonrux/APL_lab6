using Demographic.Models;
using Demographic.Classes;


namespace Demographic.Interfaces
{
    public interface IEngine
    {
        event EventHandler<int> YearTick;
        DeathRules DeathRules { get; }
        void Initialize(double totalPopulation, int startYear, InitialAgeData ageData, DeathRules deathRules);
        void RunSimulation(int endYear);
        List<Person> GetPopulation();
        SimulationResult GetSimulationResult();
    }
}