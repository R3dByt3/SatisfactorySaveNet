using System;
using System.Collections.Frozen;
using System.Linq;

namespace SatisfactorySaveNet;

public static class KnownConstants
{
    public static readonly FrozenSet<string> ConveyorBelts = new[]
    {
        "/Game/FactoryGame/Buildable/Factory/ConveyorBeltMk1/Build_ConveyorBeltMk1.Build_ConveyorBeltMk1_C",
        "/Game/FactoryGame/Buildable/Factory/ConveyorBeltMk2/Build_ConveyorBeltMk2.Build_ConveyorBeltMk2_C",
        "/Game/FactoryGame/Buildable/Factory/ConveyorBeltMk3/Build_ConveyorBeltMk3.Build_ConveyorBeltMk3_C",
        "/Game/FactoryGame/Buildable/Factory/ConveyorBeltMk4/Build_ConveyorBeltMk4.Build_ConveyorBeltMk4_C",
        "/Game/FactoryGame/Buildable/Factory/ConveyorBeltMk5/Build_ConveyorBeltMk5.Build_ConveyorBeltMk5_C",
        "/Game/FactoryGame/Buildable/Factory/ConveyorBeltMk6/Build_ConveyorBeltMk6.Build_ConveyorBeltMk6_C"
    }.ToFrozenSet(StringComparer.Ordinal);

    public static readonly string[] ModConveyorBelts =
    [
        "/Conveyors_Mod/Build_BeltMk",
        "/Game/Conveyors_Mod/Build_BeltMk",
        "/UltraFastLogistics/Buildable/build_conveyorbeltMK",
        "/FlexSplines/Conveyor/Build_Belt",
        "/conveyorbeltmod/Belt/mk",
        "/minerplus/content/buildable/Factory/belt_",
        "/bamfp/content/buildable/Factory/belt_"
    ];

    public static readonly FrozenSet<string> ConveyorLifts = new[]
    {
        "/Game/FactoryGame/Buildable/Factory/ConveyorLiftMk1/Build_ConveyorLiftMk1.Build_ConveyorLiftMk1_C",
        "/Game/FactoryGame/Buildable/Factory/ConveyorLiftMk2/Build_ConveyorLiftMk2.Build_ConveyorLiftMk2_C",
        "/Game/FactoryGame/Buildable/Factory/ConveyorLiftMk3/Build_ConveyorLiftMk3.Build_ConveyorLiftMk3_C",
        "/Game/FactoryGame/Buildable/Factory/ConveyorLiftMk4/Build_ConveyorLiftMk4.Build_ConveyorLiftMk4_C",
        "/Game/FactoryGame/Buildable/Factory/ConveyorLiftMk5/Build_ConveyorLiftMk5.Build_ConveyorLiftMk5_C",
        "/Game/FactoryGame/Buildable/Factory/ConveyorLiftMk6/Build_ConveyorLiftMk6.Build_ConveyorLiftMk6_C"
    }.ToFrozenSet(StringComparer.Ordinal);

    public static readonly FrozenSet<string> AvailableConveyorChainActor = new[]
    {
        "/Script/FactoryGame.FGConveyorChainActor",
        "/Script/FactoryGame.FGConveyorChainActor_RepSizeMedium",
        "/Script/FactoryGame.FGConveyorChainActor_RepSizeLarge",
        "/Script/FactoryGame.FGConveyorChainActor_RepSizeHuge",
        "/Script/FactoryGame.FGConveyorChainActor_RepSizeNoCull"
    }.ToFrozenSet(StringComparer.Ordinal);

    public static readonly string[] ModConveyorLifts =
    [
        "/minerplus/content/buildable/Factory/lift",
        "/bamfp/content/buildable/Factory/lift",
        "/Game/Conveyors_Mod/Build_LiftMk",
        "/Conveyors_Mod/Build_LiftMk",
        "/Game/CoveredConveyor",
        "/CoveredConveyor",
        "/conveyorbeltmod/lift/"
    ];

    public static readonly FrozenSet<string> PowerLines = new[]
    {
        "/Game/FactoryGame/Buildable/Factory/PowerLine/Build_PowerLine.Build_PowerLine_C",
        "/Game/FactoryGame/Events/Christmas/Buildings/PowerLineLights/Build_XmassLightsLine.Build_XmassLightsLine_C"
    }.ToFrozenSet(StringComparer.Ordinal);

    public static readonly string[] ModPowerLines =
    [
        "/FlexSplines/PowerLine/Build_FlexPowerline.Build_FlexPowerline_C",
        "/AB_CableMod/Visuals1/Build_AB-PLCopper.Build_AB-PLCopper_C",
        "/AB_CableMod/Visuals1/Build_AB-PLCaterium.Build_AB-PLCaterium_C",
        "/AB_CableMod/Visuals3/Build_AB-PLHeavy.Build_AB-PLHeavy_C",
        "/AB_CableMod/Visuals4/Build_AB-SPLight.Build_AB-SPLight_C",
        "/AB_CableMod/Visuals3/Build_AB-PLPaintable.Build_AB-PLPaintable_C",
        "/AB_CableMod/Cables_Heavy/Build_AB-PLHeavy-Cu.Build_AB-PLHeavy-Cu_C",
        "/AB_CableMod/Cables_Standard/Build_AB-PLStandard-Cu.Build_AB-PLStandard-Cu_C",
        "/AB_CableMod/Cables_Wire/Build_AB-PLWire-Si.Build_AB-PLWire-Si_C",
        "/AB_CableMod/Cables_Wire/Build_AB-PLWire-Au.Build_AB-PLWire-Au_C"
    ];

    public static readonly FrozenSet<string> Vehicles = new[]
    {
        "/Game/FactoryGame/Buildable/Vehicle/Tractor/BP_Tractor.BP_Tractor_C",
        "/Game/FactoryGame/Buildable/Vehicle/Truck/BP_Truck.BP_Truck_C",
        "/Game/FactoryGame/Buildable/Vehicle/Explorer/BP_Explorer.BP_Explorer_C",
        "/Game/FactoryGame/Buildable/Vehicle/Cyberwagon/Testa_BP_WB.Testa_BP_WB_C",
        "/Game/FactoryGame/Buildable/Vehicle/Golfcart/BP_Golfcart.BP_Golfcart_C",
        "/Game/FactoryGame/Buildable/Vehicle/Golfcart/BP_GolfcartGold.BP_GolfcartGold_C"
    }.ToFrozenSet(StringComparer.Ordinal);

    public static readonly string[] ModVehicles =
    [
        "/x3_mavegrag/Vehicles/Trucks/TruckMk1/BP_X3Truck_Mk1.BP_X3Truck_Mk1_C"
    ];

    public static readonly FrozenSet<string> Locomotives = new[]
    {
        "/Game/FactoryGame/Buildable/Vehicle/Train/Locomotive/BP_Locomotive.BP_Locomotive_C"
    }.ToFrozenSet(StringComparer.Ordinal);

    public static readonly string[] ModLocomotives =
    [
        "/x3_mavegrag/Vehicles/Trains/Locomotive_Mk1/BP_X3Locomotive_Mk1.BP_X3Locomotive_Mk1_C",
        "/DI_Transportation_Darkplate/Trains/Locomotive/DI_Locomotive_400/Build_DI_Locomotive_400.Build_DI_Locomotive_400_C",
        "/MkPlus/Buildables/Train/BP_Locomotive_Mk2.BP_Locomotive_Mk2_C"
    ];

    public static readonly FrozenSet<string> FreightWagon = new[]
    {
        "/Game/FactoryGame/Buildable/Vehicle/Train/Wagon/BP_FreightWagon.BP_FreightWagon_C"
    }.ToFrozenSet(StringComparer.Ordinal);

    public static readonly string[] ModFreightWagon =
    [
        "/x3_mavegrag/Vehicles/Trains/CargoWagon_Mk1/BP_X3CargoWagon_Mk1.BP_X3CargoWagon_Mk1_C",
        "/DI_Transportation_Darkplate/Trains/Wagon/DI_Wagon_512/Build_DI_FrieghtWagon512.Build_DI_FrieghtWagon512_C",
        "/MkPlus/Buildables/Train/BP_FreightWagon_Mk2.BP_FreightWagon_Mk2_C"
    ];

    public static readonly FrozenSet<string> StatefulInventoryItems = new string[]
    {
        "/Game/FactoryGame/Equipment/Chainsaw/Desc_Chainsaw.Desc_Chainsaw_C",
        "/Game/FactoryGame/Resource/Equipment/JetPack/BP_EquipmentDescriptorJetPack.BP_EquipmentDescriptorJetPack_C",
        "/Game/FactoryGame/Resource/Equipment/NailGun/Desc_RebarGunProjectile.Desc_RebarGunProjectile_C",
        "/Game/FactoryGame/Resource/Equipment/Rifle/BP_EquipmentDescriptorRifle.BP_EquipmentDescriptorRifle_C",
        "/Game/FactoryGame/Resource/Equipment/NobeliskDetonator/BP_EquipmentDescriptorNobeliskDetonator.BP_EquipmentDescriptorNobeliskDetonator_C",
        "/Game/FactoryGame/Resource/Equipment/GemstoneScanner/BP_EquipmentDescriptorObjectScanner.BP_EquipmentDescriptorObjectScanner_C",
        "/Game/FactoryGame/Resource/Equipment/GasMask/BP_EquipmentDescriptorGasmask.BP_EquipmentDescriptorGasmask_C"
    }.ToFrozenSet(StringComparer.Ordinal);

    public static bool IsConveyorLift(string path)
    {
        return ConveyorLifts.Contains(path) || ModConveyorLifts.Any(x => x.StartsWith(path, StringComparison.Ordinal));
    }

    public static bool IsConveyorBelt(string path)
    {
        return ConveyorBelts.Contains(path) || ModConveyorBelts.Any(x => x.StartsWith(path, StringComparison.Ordinal));
    }

    public static bool IsConveyorActor(string path)
    {
        return AvailableConveyorChainActor.Contains(path);
    }

    public static bool IsConveyor(string path)
    {
        return IsConveyorLift(path) || IsConveyorBelt(path);
    }

    public static bool IsPowerLine(string path)
    {
        return PowerLines.Contains(path) || ModPowerLines.Any(x => x.StartsWith(path, StringComparison.Ordinal));
    }

    public static bool IsVehicle(string path)
    {
        return Vehicles.Contains(path) || ModVehicles.Any(x => x.StartsWith(path, StringComparison.Ordinal));
    }

    public static bool IsLocomotive(string path)
    {
        return Locomotives.Contains(path) || ModLocomotives.Any(x => x.StartsWith(path, StringComparison.Ordinal));
    }

    public static bool IsFreightWagon(string path)
    {
        return FreightWagon.Contains(path) || ModFreightWagon.Any(x => x.StartsWith(path, StringComparison.Ordinal));
    }
}