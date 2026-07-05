namespace SatisfactorySaveNet.Tests.Analysis;

public static class SaveAnalysis
{
    private const string FactoryBuildablePrefix = "/Game/FactoryGame/Buildable/Factory/";
    private const string DroneStationPath = "/Game/FactoryGame/Buildable/Factory/DroneStation/BP_DroneTransport.BP_DroneTransport_C";
    private const string PlayerStatePath = "/Game/FactoryGame/Character/Player/BP_PlayerState.BP_PlayerState_C";
    private const string GameStatePath = "/Game/FactoryGame/-Shared/Blueprint/BP_GameState.BP_GameState_C";

    public static SaveStatistics Analyze(SatisfactorySave save)
    {
        var stats = new MutableStats();

        foreach (var obj in EnumerateObjects(save))
        {
            stats.TotalObjects++;

            if (IsFactoryBuildable(obj.TypePath))
            {
                stats.FactoryBuildables++;
            }

            if (KnownConstants.IsVehicle(obj.TypePath))
            {
                stats.Vehicles++;
            }
            else if (KnownConstants.IsLocomotive(obj.TypePath))
            {
                stats.Locomotives++;
            }
            else if (KnownConstants.IsFreightWagon(obj.TypePath))
            {
                stats.FreightWagons++;
            }

            if (KnownConstants.IsConveyorBelt(obj.TypePath))
            {
                stats.ConveyorBelts++;
            }
            else if (KnownConstants.IsConveyorLift(obj.TypePath))
            {
                stats.ConveyorLifts++;
            }

            if (KnownConstants.IsPowerLine(obj.TypePath))
            {
                stats.PowerLines++;
            }

            if (obj.TypePath == DroneStationPath)
            {
                stats.DroneStations++;
            }

            if (obj.TypePath == PlayerStatePath)
            {
                stats.PlayerStates++;
            }

            if (obj.TypePath == GameStatePath)
            {
                stats.GameStateActors++;
            }
        }

        return stats.ToRecord();
    }

    public static IEnumerable<ComponentObject> EnumerateObjects(SatisfactorySave save)
    {
        if (save.Body is BodyV8 bodyV8)
        {
            foreach (var level in bodyV8.Levels)
            {
                foreach (var obj in level.Objects)
                {
                    yield return obj;
                }
            }

            yield break;
        }

#pragma warning disable CS0618
        if (save.Body is BodyPreV8 bodyPreV8)
#pragma warning restore CS0618
        {
            foreach (var obj in bodyPreV8.Objects)
            {
                yield return obj;
            }
        }
    }

    private static bool IsFactoryBuildable(string typePath)
    {
        if (!typePath.StartsWith(FactoryBuildablePrefix, StringComparison.Ordinal))
        {
            return false;
        }

        if (KnownConstants.IsConveyor(typePath) || KnownConstants.IsPowerLine(typePath))
        {
            return false;
        }

        return true;
    }

    private sealed class MutableStats
    {
        public int TotalObjects;
        public int FactoryBuildables;
        public int Vehicles;
        public int Locomotives;
        public int FreightWagons;
        public int ConveyorBelts;
        public int ConveyorLifts;
        public int PowerLines;
        public int DroneStations;
        public int PlayerStates;
        public int GameStateActors;

        public SaveStatistics ToRecord() => new(
            TotalObjects,
            FactoryBuildables,
            Vehicles,
            Locomotives,
            FreightWagons,
            ConveyorBelts,
            ConveyorLifts,
            PowerLines,
            DroneStations,
            PlayerStates,
            GameStateActors);
    }
}
