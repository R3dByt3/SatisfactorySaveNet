namespace SatisfactorySaveNet.Tests.Analysis;

public sealed record SaveStatistics(
    int TotalObjects,
    int FactoryBuildables,
    int Vehicles,
    int Locomotives,
    int FreightWagons,
    int ConveyorBelts,
    int ConveyorLifts,
    int PowerLines,
    int DroneStations,
    int PlayerStates,
    int GameStateActors);
