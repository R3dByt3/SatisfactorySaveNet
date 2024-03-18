using System;
using System.Collections.Generic;

namespace SatisfactorySaveNet;

public static class KnownConstants
{
    public static readonly IReadOnlySet<string> ConveyorBelts = new HashSet<string>(StringComparer.Ordinal)
    {
        "/Game/FactoryGame/Buildable/Factory/ConveyorBeltMk1/Build_ConveyorBeltMk1.Build_ConveyorBeltMk1_C",
        "/Game/FactoryGame/Buildable/Factory/ConveyorBeltMk2/Build_ConveyorBeltMk2.Build_ConveyorBeltMk2_C",
        "/Game/FactoryGame/Buildable/Factory/ConveyorBeltMk3/Build_ConveyorBeltMk3.Build_ConveyorBeltMk3_C",
        "/Game/FactoryGame/Buildable/Factory/ConveyorBeltMk4/Build_ConveyorBeltMk4.Build_ConveyorBeltMk4_C",
        "/Game/FactoryGame/Buildable/Factory/ConveyorBeltMk5/Build_ConveyorBeltMk5.Build_ConveyorBeltMk5_C",
        "/Conveyors_Mod/Build_BeltMk",
        "/Game/Conveyors_Mod/Build_BeltMk",
        "/UltraFastLogistics/Buildable/build_conveyorbeltMK",
        "/FlexSplines/Conveyor/Build_Belt",
        "/conveyorbeltmod/Belt/mk",
        "/minerplus/content/buildable/Factory/belt_",
        "/bamfp/content/buildable/Factory/belt_"
    };

    public static readonly IReadOnlySet<string> ConveyorLifts = new HashSet<string>(StringComparer.Ordinal)
    {
        "/Game/FactoryGame/Buildable/Factory/ConveyorLiftMk1/Build_ConveyorLiftMk1.Build_ConveyorLiftMk1_C",
        "/Game/FactoryGame/Buildable/Factory/ConveyorLiftMk2/Build_ConveyorLiftMk2.Build_ConveyorLiftMk2_C",
        "/Game/FactoryGame/Buildable/Factory/ConveyorLiftMk3/Build_ConveyorLiftMk3.Build_ConveyorLiftMk3_C",
        "/Game/FactoryGame/Buildable/Factory/ConveyorLiftMk4/Build_ConveyorLiftMk4.Build_ConveyorLiftMk4_C",
        "/Game/FactoryGame/Buildable/Factory/ConveyorLiftMk5/Build_ConveyorLiftMk5.Build_ConveyorLiftMk5_C",
        "/minerplus/content/buildable/Factory/lift",
        "/bamfp/content/buildable/Factory/lift",
        "/Game/Conveyors_Mod/Build_LiftMk",
        "/Conveyors_Mod/Build_LiftMk",
        "/Game/CoveredConveyor",
        "/CoveredConveyor",
        "/conveyorbeltmod/lift/"
    };

    public static bool IsConveyorLift(string path)
    {
        return ConveyorLifts.Contains(path);
    }

    public static bool IsConveyorBelt(string path)
    {
        return ConveyorBelts.Contains(path);
    }

    public static bool IsConveyor(string path)
    {
        return IsConveyorLift(path) || IsConveyorBelt(path);
    }
}