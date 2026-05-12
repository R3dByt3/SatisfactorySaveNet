# TODO — SatisfactorySaveNet

Tracked follow-ups for architectural cleanup, known production-code quirks, and
test-coverage stretch goals. Bigger items first.

---

## 1. Separate vanilla and mod-specific types into different projects

**Observation.** The parser currently bundles vanilla-game type handling and a
growing list of mod-specific format readers into the same assembly. Users of
the library may have any subset of these mods installed (or none) — but
everyone pays the dependency and surface-area cost of every supported mod.

**Mod families currently mixed into the core parser:**

| Mod | Type-name prefix(es) | Surface |
|---|---|---|
| **FicsItNetworks** (incl. Computer, Misc, Lua) | `/Script/FicsItNetworks*`, type names `FIN*` / `FIR*` | Largest. `FINNetworkProperty`, `FINNetworkUnion`, `FINNetworkTrace`, `FINLuaProcessorStateStorage`, `FIRExecutionContext`, `FIRAnyValue`, `FINGPUT1Buffer`, `FINGPUT1BufferPixel`, `FINDynamicStructHolder`, `FIRInstancedStruct`, `FINItemStateFileSystem`. Special-cased in `PropertySerializer`, `TypedDataSerializer`, `ExtraDataSerializer` |
| **FicsItFarming** | `/Script/FicsitFarming.` | `ExtraDataSerializer.cs` script-prefix list |
| **RefinedRDLib** | `/Script/RefinedRDLib.` | `ExtraDataSerializer.cs` script-prefix list |
| **DigitalStorage** | `/Script/DigitalStorage.` | `ExtraDataSerializer.cs` script-prefix list |
| **Conveyor mods** (various authors) | `/Conveyors_Mod/Build_BeltMk` etc. | `KnownConstants.ModConveyorBelts` / `ModConveyorLifts` string-prefix arrays |
| **Vehicle mods** (X3 Truck, DI Locomotive, …) | various `/x3_mavegrag/`, `/DI_Transportation_*/` | `KnownConstants.ModVehicles` / `ModLocomotives` arrays |
| **FlexSplines** | `/FlexSplines/PowerLine/` | `KnownConstants.ModPowerLines` array |

**Target shape (sketch).**

```
SatisfactorySaveNet.Core              ← parser engine + vanilla types only
SatisfactorySaveNet.Abstracts         ← unchanged (POCO models + interfaces)
SatisfactorySaveNet.Mods.FicsItNetworks
SatisfactorySaveNet.Mods.FicsItFarming
SatisfactorySaveNet.Mods.RefinedRDLib
SatisfactorySaveNet.Mods.DigitalStorage
SatisfactorySaveNet.Mods.Conveyors    ← consolidates the various belt/lift mods
SatisfactorySaveNet.Mods.Vehicles     ← consolidates the various vehicle/loco mods
```

Each mod package registers its custom types and Type-Path matchers with the
core engine via a plugin interface — e.g. an `ICustomTypeReaderRegistry` the
caller pre-populates before invoking `SaveFileSerializer.Deserialize`. Core
ships without any mod opt-ins; consumers add the packages that match their
savegame's mod set.

**Why this matters beyond cleanliness:**
- Mod readers can ship and version on the mod's release cadence, decoupled
  from the core parser.
- Third-party mod authors can publish their own reader packages without
  upstreaming PRs.
- Test isolation: vanilla coverage becomes interpretable on its own (today
  some of the unvisited `TypedDataSerializer` lines are mod-only struct
  readers that drag the aggregate down — see also item 4 below).
- Smaller core assembly for consumers who only deal with vanilla saves.

**Migration considerations:**
- The current `static readonly Instance` singletons in
  `SatisfactorySaveNet.Core` would need a small constructor-injectable
  override path so a mod package can register before the singleton is read.
- `KnownConstants.IsConveyor` / `IsPowerLine` / etc. would consult the
  registry rather than hardcoded `Mod*` arrays.
- Existing consumers who pull `SatisfactorySaveNet` (the big assembly) keep
  working via a metapackage that depends on Core + all mod packages.

**Non-goals:**
- Don't try to support arbitrary runtime plugin discovery (DLL scanning,
  reflection). The mod packs are NuGet references like any other.

---

## 2. Add a real Satisfactory v1.1 fixture for compatibility testing

Today the v1.1 (stable branch) parsing surface is locked in by
synthesised-binary tests with `saveVersion < 53`
(`PropertySerializerLegacyTests`, `ExtraDataSerializerLegacyTests`, the legacy
sides of `Compat/VersionCompatibilityTests`). A real v1.1 `.sav` fixture would
give us end-to-end proof rather than per-branch proof. Requires switching
Satisfactory to the stable branch in-game and saving a `Pedestal`-equivalent
fixture under the existing `SatisfactorySaveNet - v1.1 - *.sav` naming
convention — `Update-Fixtures.ps1` would pick it up automatically after a
`ExpectedSessionName` constant bump in the fixture test class.

---

## 3. `BoolProperty` flag-bit value unreachable when `binarySize == 0`

`PropertySerializer.cs:117` gates `TryParseKnownValue` behind
`if (binarySize > 0)`, but the `BoolProperty` branch inside that method
documents itself as having no value bytes (the bool lives in flag bit `0x10`).
If a real save ever emits a `BoolProperty` tag with `binarySize == 0` —
which the comment suggests is the actual wire format — `RawProperty.BoolValue`
stays null forever.

Currently pinned as a regression marker by
`DeserializeProperty_AtV12_BoolProperty_WithZeroBinarySize_LeavesBoolValueNull`.
**Decision needed:** move the `BoolProperty` special case before the
`binarySize > 0` gate, OR confirm via real saves that `binarySize` is always
positive on `BoolProperty` tags and tighten the regression test accordingly.

---

## 4. Push `SatisfactorySaveNet` coverage from ~63% toward 80%

Current per-class sequence coverage is in `SatisfactorySaveNet.Tests/README.md`.
The shortfall lives entirely in three files:

- `TypedDataSerializer.cs` (34%) — ~30 type-specific deserializers, most of
  which are mod-only (FicsItNetworks family). Resolved partly by item 1 above
  — moving mod readers out of the core lifts the vanilla-only aggregate
  without writing busywork tests.
- `PropertySerializer.cs` (63%) — `MapProperty` (polymorphic key/value),
  `SetProperty` + `DeserializeUnions` (6 union types with TypePath
  discriminators), `TextProperty` history-types 1/3/10/255, `ArrayStructProperty`.
  Each of these is genuine vanilla parsing logic worth testing; deferred
  because the wire format is fiddly and tests would be substantial. A real
  `Pedestal + <building-with-inventory>` fixture would exercise most of the
  Map paths transitively.
- `ExtraDataSerializer.cs` (52%) — `ConveyorChainActor` non-empty item loop,
  `LightweightBuildableSubsystem` instance loop, `DroneStation` non-empty
  action queues, `PlayerData` modes 241/17/25/29 (variable-length hex/SteamId
  parsing). All best exercised by per-feature `Pedestal + <building>`
  fixtures rather than synthesised tests.

**Recommended path:** capture 3-4 more curated fixtures
(`Pedestal + Conveyor Belt with Item`, `Pedestal + Power Line`,
`Pedestal + Drone Station`, `Pedestal + Lightweight Buildable`) rather than
adding ~30 brittle synthesised tests. Fixture-driven coverage is more
meaningful and doesn't drift when the wire format changes.

---

## 5. Cover the still-untested v1.2+ ExtraData paths

`ObjectSerializer.cs:130-132` shortlists Conveyor / PowerLine /
CircuitSubsystem as the v1.2-ported ExtraData branches. Other actor classes
silently skip ExtraData at v1.2+. As the rest of the v1.2 ExtraData formats
are ported (Vehicle, Locomotive, DroneStation, Blueprint, PlayerData,
LightweightBuildableSubsystem), add them to the `extraDataPortedAtV12` check
AND add a synthesised-binary test for the v1.2 wire shape in
`Serializers/ExtraDataSerializerV12Tests.cs` (new file, mirrors the existing
`*LegacyTests.cs`).

---

## 6. Add typed property accessor helpers on `ComponentObject`

**Problem.** Downstream consumers typically need a handful of typed properties
off each actor (e.g. `mPurity` on a resource node, `mCurrentRecipe` on a
producer, `mExtractResourceNode` on a miner, `mCurrentPotential` for clock
speed). Today that means walking `ComponentObject.Properties` and
pattern-matching against the concrete `Property` subclasses for every actor
type in every adapter:

```csharp
var purityProp = actor.Properties
    .OfType<EnumProperty>()
    .FirstOrDefault(p => p.Name == "mPurity");
if (purityProp is not null && Enum.TryParse<NodePurity>(StripPrefix(purityProp.Value), out var purity)) { ... }
```

…repeated for every property type and every actor type. The ERP planner
consuming this library hits the boilerplate hard — see the open task
"Surface NodePurity, ClockSpeed, RecipeId, and miner→ResourceNode binding
on LiveFactoryState" in the consuming repo
(`ChrisonSimtian/ERP.Satisfactory` issue #35).

**Proposed addition.** Convenience accessors on `ComponentObject` that return
`null` / `false` when the property is absent or the wrong type — without
throwing:

```csharp
public bool TryGetEnumValue(string name, out string? value);
public bool TryGetFloatValue(string name, out float value);
public bool TryGetIntValue(string name, out int value);
public bool TryGetBoolValue(string name, out bool value);
public bool TryGetStringValue(string name, out string? value);
public bool TryGetObjectReference(string name, out ObjectReference? reference);
public bool TryGetSoftObjectReference(string name, out SoftObjectReference? reference);
```

Or an indexer-style API if that fits the existing surface better:
`T? GetPropertyValue<T>(string name)`.

**Impact.**
- Adapters reading typed properties become one-liners instead of
  pattern-match ladders.
- Consumers' code becomes resilient to minor wire-format changes (a property
  going from `FloatProperty` to `DoubleProperty` upstream, say).
- No behavior change for existing consumers — purely additive.

**Out of scope.** Auto-deserializing into POCOs (different abstraction).
Collection-shaped properties (`MapProperty`, `SetProperty`, `ArrayProperty`)
probably warrant their own accessors but can land separately.

---

## 7. Bundle a coordinate→resource lookup table for vanilla resource nodes

**Problem.** `BP_ResourceNode_C` actors (the static mining nodes Coffee Stain
placed in the world — Iron / Copper / Coal / Limestone / Oil / Caterium /
Sulfur / Quartz / Bauxite / Uranium / SAM) carry only `mResourcesLeft` in
the save file. The resource type (`mResourceClass`) and purity (`mPurity`)
are **blueprint class defaults**, not instance-serialized properties — they
live in the game's `.pak` assets, not in the `.sav`.

Same story for `BP_FrackingCore_C` (oil/water/nitrogen wells), and to a lesser
extent for `BP_ResourceNodeGeyser_C` (always geothermal, so trivially
inferable from the BP type).

Today every downstream consumer that wants per-node resource info has to
either ship their own coordinate→resource map or skip the feature. The data
itself is well-known (community-maintained on the Satisfactory wiki,
satisfactory-calculator.com, etc.) and stable across game patches — node
placements have barely moved since v1.0.

**Proposed addition.** Bundle a curated dataset alongside the parser:

```
SatisfactorySaveNet.Resources/
  known-resource-nodes.json        # public-data, CC-BY-SA-sourced
  KnownResourceNodes.cs            # static API: Lookup(position) → (resource, purity)
```

```csharp
public static class KnownResourceNodes
{
    public static KnownNode? Lookup(Vector3 position, float tolerance = 500f);
    public static IReadOnlyList<KnownNode> All { get; }
}

public sealed record KnownNode(string ResourceClass, NodePurity Purity, Vector3 Position);
```

Tolerance lets us match nodes that have drifted slightly between save versions
or via mods. ~500 cm is plenty — vanilla nodes are tens of metres apart.

**Impact.**
- Consumers get per-node resource info without each having to ship the data.
- Centralised maintenance: one PR updates the table when Coffee Stain adds
  a new node (rare but happens).
- Decouples parser code from world data (data file, not C# code).

**Out of scope.**
- Modded world support (modded maps would need their own lookup). Could ship
  a "register custom map" hook later.
- Resource amounts / extraction rates per purity — derivable from existing
  game data, but consumer-side concern.

**Sourcing the data.** Public Satisfactory wiki (CC-BY-SA 3.0) or community
JSON dumps. The list is ~600 entries total (all node types combined).
Until the upstream lib bundles this, the ERP planner ships its own seed
table under `src/Satisfactory/Save/Data/`.

Tracked as a consumer-side need in
`ChrisonSimtian/ERP.Satisfactory` (resource node identification work).
