using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SatisfactorySaveNet.Abstracts;
using SatisfactorySaveNet.Abstracts.Extra;
using SatisfactorySaveNet.Abstracts.Maths.Vector;
using SatisfactorySaveNet.Abstracts.Model;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace SatisfactorySaveNet;

public class ExtraDataSerializer : IExtraDataSerializer
{
    public static readonly IExtraDataSerializer Instance = new ExtraDataSerializer(NullLoggerFactory.Instance, StringSerializer.Instance, ObjectReferenceSerializer.Instance, VectorSerializer.Instance, HexSerializer.Instance, PropertySerializer.Instance);

    private readonly IStringSerializer _stringSerializer;
    private readonly IObjectReferenceSerializer _objectReferenceSerializer;
    private readonly IVectorSerializer _vectorSerializer;
    private readonly IHexSerializer _hexSerializer;
    private readonly IPropertySerializer _propertySerializer;
    private readonly ILogger<ExtraDataSerializer> _logger;

    public ExtraDataSerializer(ILoggerFactory loggerFactory, IStringSerializer stringSerializer, IObjectReferenceSerializer objectReferenceSerializer, IVectorSerializer vectorSerializer, IHexSerializer hexSerializer, IPropertySerializer propertySerializer)
    {
        _stringSerializer = stringSerializer;
        _objectReferenceSerializer = objectReferenceSerializer;
        _vectorSerializer = vectorSerializer;
        _hexSerializer = hexSerializer;
        _propertySerializer = propertySerializer;
        _logger = loggerFactory.CreateLogger<ExtraDataSerializer>();
    }

    public ExtraData? Deserialize(BinaryReader reader, string typePath, Header header, long expectedPosition)
    {
        if (KnownConstants.IsConveyor(typePath))
            return DeserializeConveyor(reader, header);
        if (KnownConstants.IsConveyorActor(typePath))
            return DeserializeConveyorChainActor(reader, header);
        if (KnownConstants.IsPowerLine(typePath))
            return DeserializePowerLine(reader, header);
        if (KnownConstants.IsVehicle(typePath))
            return DeserializeVehicle(reader, header);
        if (KnownConstants.IsLocomotive(typePath) || KnownConstants.IsFreightWagon(typePath))
            return DeserializeLocomotiveData(reader, header);

        return typePath switch
        {
            "/Game/FactoryGame/-Shared/Blueprint/BP_GameState.BP_GameState_C" or "/Game/FactoryGame/-Shared/Blueprint/BP_GameMode.BP_GameMode_C" => DeserializeBlueprint(reader),
            "/Game/FactoryGame/Character/Player/BP_PlayerState.BP_PlayerState_C" => DeserializePlayerData(reader, expectedPosition),
            "/Game/FactoryGame/Buildable/Factory/DroneStation/BP_DroneTransport.BP_DroneTransport_C" => DeserializeDroneStation(reader, header, expectedPosition),
            "/Game/FactoryGame/-Shared/Blueprint/BP_CircuitSubsystem.BP_CircuitSubsystem_C" => DeserializeCircuitData(reader),
            "/Script/FactoryGame.FGLightweightBuildableSubsystem" => DeserializeLightweightBuildableSubsystem(reader, header),
            _ => DeserializeUnknownData(reader, header, typePath, expectedPosition),
        };
    }

    private UnknownExtraData? DeserializeUnknownData(BinaryReader reader, Header header, string typePath, long expectedPosition)
    {
        var bytesCount = expectedPosition - reader.BaseStream.Position;
        
        if (bytesCount > 4)
        {
            if (header.SaveVersion >= 41 && (
                typePath.StartsWith("/Script/FactoryGame.FG", StringComparison.Ordinal) ||
                typePath.StartsWith("/Script/FicsitFarming.", StringComparison.Ordinal) ||
                typePath.StartsWith("/Script/RefinedRDLib.", StringComparison.Ordinal) ||
                typePath.StartsWith("/Script/DigitalStorage.", StringComparison.Ordinal) ||
                typePath.StartsWith("/Script/FicsIt", StringComparison.Ordinal)
                ))
                reader.BaseStream.Seek(8, SeekOrigin.Current);
            else
            {
                var missing = _hexSerializer.Deserialize(reader, bytesCount.ToInt());
                _logger.LogWarning("Extra missing bytes: {MissingBytes}", missing);

                return new UnknownExtraData
                {
                    Missing = missing,
                };
            }
        }
        else
        {
            reader.BaseStream.Seek(4, SeekOrigin.Current);
        }

        return null;
    }

    private LightweightBuildableSubsystem DeserializeLightweightBuildableSubsystem(BinaryReader reader, Header header)
    {
        var unknownRoot1 = reader.ReadInt32();

        int? lightWeightVersion = null;
        if (header.HeaderVersion >= 14)
            lightWeightVersion = reader.ReadInt32();

        var objectCount = reader.ReadInt32();
        var objects = new ExtraObject[objectCount];

        for (var x = 0; x < objectCount; x++)
        {
            var unknown1 = reader.ReadInt32();
            var className = _stringSerializer.Deserialize(reader);
            var instanceCount = reader.ReadInt32();

            var instances = new ExtraInstance[instanceCount];

            for (var y = 0; y < instanceCount; y++)
            {
                var pathName = GenerateFastUniquePathName("LightweightBuildable_" + className.Split("/")[^1] + "_");

                var rotation = _vectorSerializer.DeserializeVec4D(reader);
                var translation = _vectorSerializer.DeserializeVec3D(reader);
                var scale = _vectorSerializer.DeserializeVec3D(reader);

                var swatchDescription = _objectReferenceSerializer.Deserialize(reader);
                var materialDescription = _objectReferenceSerializer.Deserialize(reader);
                var patternDescription = _objectReferenceSerializer.Deserialize(reader);
                var skinDescription = _objectReferenceSerializer.Deserialize(reader);

                var primaryColor = _vectorSerializer.DeserializeColor4(reader);
                var secondaryColor = _vectorSerializer.DeserializeColor4(reader);
                var paintFinish = _objectReferenceSerializer.Deserialize(reader);
                var patternRotation = reader.ReadSByte();

                var buildWithRecipe = _objectReferenceSerializer.Deserialize(reader);
                var blueprintProxy = _objectReferenceSerializer.Deserialize(reader);

                TypeSpecificData? typeSpecificData = null;

                if (header.HeaderVersion >= 14)
                {
                    var specificationDataFlag = reader.ReadInt32();
                    var hasSpecificationData = specificationDataFlag == 1;

                    if (hasSpecificationData)
                    {
                        var objectReference = _objectReferenceSerializer.Deserialize(reader);

                        _ = reader.ReadInt32();

                        var properties = _propertySerializer.DeserializeProperties(reader, header).ToArray();

                        typeSpecificData = new TypeSpecificData
                        {
                            ObjectReference = objectReference,
                            Properties = properties,
                        };
                    }
                }

                instances[y] = new ExtraInstance
                {
                    PathName = pathName,
                    Rotation = rotation,
                    Translation = translation,
                    Scale = scale,
                    SwatchDescription = swatchDescription,
                    MaterialDescription = materialDescription,
                    PatternDescription = patternDescription,
                    SkinDescription = skinDescription,
                    PrimaryColor = primaryColor,
                    SecondaryColor = secondaryColor,
                    PaintFinish = paintFinish,
                    PatternRotation = patternRotation,
                    BuildWithRecipe = buildWithRecipe,
                    BlueprintProxy = blueprintProxy,
                    TypeSpecificData = typeSpecificData
                };
            }

            objects[x] = new ExtraObject
            {
                Unknown1 = unknown1,
                ClassName = className,
                Instances = instances
            };
        }

        var lightweightBuildableSubsystem = new LightweightBuildableSubsystem
        {
            Unknown1 = unknownRoot1,
            Objects = objects,
            LightWeightVersion = lightWeightVersion
        };

        return lightweightBuildableSubsystem;
    }

    private static string GenerateFastUniquePathName(string path)
    {
        var parts = path.Split('_').SkipLast(1).Append(Ulid.NewUlid().ToString());

        var result = string.Join("_", parts);

        return result;
    }

    private CircuitData DeserializeCircuitData(BinaryReader reader)
    {
        var count = reader.ReadInt32();
        var nrElements = reader.ReadInt32();
        var circuits = new Circuit[nrElements];

        for (var x = 0; x < nrElements; x++)
        {
            var circuitId = reader.ReadInt32();
            var objectReference = _objectReferenceSerializer.Deserialize(reader);

            circuits[x] = new Circuit
            {
                CircuitId = circuitId,
                ObjectReference = objectReference
            };
        }

        return new CircuitData
        {
            Count = count,
            Circuits = circuits
        };
    }

    private DroneStationData DeserializeDroneStation(BinaryReader reader, Header header, long expectedPosition)
    {
        if (header.SaveVersion >= 41)
        {
            var unknown1 = reader.ReadInt32();
            var unknown2 = reader.ReadInt32();

            var nrActiveActions = reader.ReadInt32();
            var activeActions = new DroneStationAction[nrActiveActions];

            for (var x = 0; x < nrActiveActions; x++)
            {
                var name = _stringSerializer.Deserialize(reader);
                var properties = _propertySerializer.DeserializeProperties(reader, header).ToArray();

                activeActions[x] = new DroneStationAction
                {
                    Properties = properties,
                    Name = name
                };
            }

            var nrQueuedActions = reader.ReadInt32();
            var queuedActions = new DroneStationAction[nrQueuedActions];

            for (var x = 0; x < nrQueuedActions; x++)
            {
                var name = _stringSerializer.Deserialize(reader);
                var properties = _propertySerializer.DeserializeProperties(reader, header).ToArray();

                queuedActions[x] = new DroneStationAction
                {
                    Properties = properties,
                    Name = name
                };
            }

            return new DroneStationData
            {
                ActionQueue = queuedActions,
                ActiveActions = activeActions,
                Unknown1 = unknown1,
                Unknown2 = unknown2,
            };
        }

        var bytesCount = expectedPosition - reader.BaseStream.Position;
        var missing = _hexSerializer.Deserialize(reader, bytesCount.ToInt());
        _logger.LogWarning("Drone station missing bytes: {MissingBytes}", missing);

        return new DroneStationData
        {
            Missing = missing
        };
    }

    private PlayerData? DeserializePlayerData(BinaryReader reader, long expectedPosition)
    {
        var bytesCount = expectedPosition - reader.BaseStream.Position;
        var playerData = new PlayerData();

        if (bytesCount > 0)
        {
            var missing = _hexSerializer.Deserialize(reader, bytesCount.ToInt());
            reader.BaseStream.Seek(-bytesCount, SeekOrigin.Current);
            _ = reader.ReadInt32();
            var mode = reader.ReadByte();

            playerData.PlayerType = mode;

            switch (mode)
            {
                case 241:
                    _ = reader.ReadByte();

                    var nrElements1 = reader.ReadInt32();
                    var sb1 = new StringBuilder();

                    for (var x = 0; x < nrElements1; x++)
                    {
                        sb1.Append(reader.ReadByte().ToString("X2"));
                    }

                    playerData.EpicOnlineServicesId = new string([.. sb1.ToString().TrimStart('0').Skip(1).Take(32)]);
                    return playerData;
                case 248:
                    var value = _stringSerializer.Deserialize(reader);
                    playerData.EpicOnlineServicesId = value.Split("|")[0];
                    return playerData;
                case 249:
                    _ = _stringSerializer.Deserialize(reader);
                    break;
                case 17:
                    var nrElements2 = reader.ReadByte();
                    var sb2 = new StringBuilder();

                    for (var x = 0; x < nrElements2; x++)
                    {
                        sb2.Append(reader.ReadByte().ToString("X2"));
                    }

                    playerData.EpicOnlineServicesId = sb2.ToString().TrimStart('0');
                    return playerData;
                case 25:
                case 29:
                    var nrElements3 = reader.ReadByte();
                    var sb3 = new StringBuilder();

                    for (var x = 0; x < nrElements3; x++)
                    {
                        sb3.Append(reader.ReadByte().ToString("X2"));
                    }

                    playerData.SteamId = sb3.ToString().TrimStart('0');
                    return playerData;
                case 8:
                    playerData.PlatformId = _stringSerializer.Deserialize(reader);
                    return playerData;
                default:
                    playerData.Missing = missing;
                    _logger.LogWarning("PlayerData missing bytes: {MissingBytes}", missing);
                    reader.BaseStream.Seek(-5, SeekOrigin.Current);
                    break;
            }

            return playerData;
        }

        return null;
    }

    private BlueprintData DeserializeBlueprint(BinaryReader reader)
    {
        var count = reader.ReadInt32();
        var nrElements = reader.ReadInt32();
        var objectReferences = new ObjectReference[nrElements];

        for (var x = 0; x < nrElements; x++)
        {
            objectReferences[x] = _objectReferenceSerializer.Deserialize(reader);
        }

        return new BlueprintData
        {
            Count = count,
            Objects = objectReferences
        };
    }

    private ConveyorChainActor DeserializeConveyorChainActor(BinaryReader reader, Header header)
    {
        var count = reader.ReadInt32();

        var unknownRoot1 = _objectReferenceSerializer.Deserialize(reader);
        var unknownRoot2 = _objectReferenceSerializer.Deserialize(reader);

        var conveyorCount = reader.ReadInt32();
        var conveyors = new ConveyorActor[conveyorCount];

        for (var x = 0; x < conveyorCount; x++)
        {
            var unknown1 = _objectReferenceSerializer.Deserialize(reader);
            var conveyorBase = _objectReferenceSerializer.Deserialize(reader);

            var splinesCount = reader.ReadInt32();
            var splines = new Spline[splinesCount];

            for (var y = 0; y < splinesCount; y++)
            {
                var location = _vectorSerializer.DeserializeVec3D(reader);
                var arriveTangent = _vectorSerializer.DeserializeVec3D(reader);
                var leaveTangent = _vectorSerializer.DeserializeVec3D(reader);

                splines[y] = new Spline
                {
                    Location = location,
                    ArriveTangent = arriveTangent,
                    LeaveTangent = leaveTangent
                };
            }

            var offsetAtStart = reader.ReadSingle();
            var startsAtLength = reader.ReadSingle();
            var endsAtLength = reader.ReadSingle();
            var firstItemIndex = reader.ReadInt32();
            var lastItemIndex = reader.ReadInt32();
            var indexInChainArray = reader.ReadInt32();

            conveyors[x] = new ConveyorActor
            {
                Unknown1 = unknown1,
                ConveyorBase = conveyorBase,
                Splines = splines,
                OffsetAtStart = offsetAtStart,
                StartsAtLength = startsAtLength,
                EndsAtLength = endsAtLength,
                FirstItemIndex = firstItemIndex,
                LastItemIndex = lastItemIndex,
                IndexInChainArray = indexInChainArray
            };
        }

        var totalLength = reader.ReadSingle();
        var numberItems = reader.ReadInt32();
        var headItemIndex = reader.ReadInt32();
        var tailItemIndex = reader.ReadInt32();

        var itemCount = reader.ReadInt32();
        var items = new Item[itemCount];

        for (var x = 0; x < itemCount; x++)
        {
            var name = _objectReferenceSerializer.Deserialize(reader);
            var state = reader.ReadInt32();
            Vector4I position;
            ObjectReference? itemState = null;

            if (state != 0)
            {
                itemState = _objectReferenceSerializer.Deserialize(reader);
                var binarySizeProperties = reader.ReadInt32();
                var expectedPosition = reader.BaseStream.Position + binarySizeProperties;

                if (string.Equals(itemState?.PathName, "/Script/FicsItNetworksComputer.FINItemStateFileSystem"))
                {
                    var length = reader.ReadInt32();
                    var fINItemStateFileSystem = _hexSerializer.Deserialize(reader, length);

                    position = _vectorSerializer.DeserializeVec4BAs4I(reader);

                    items[x] = new StatefulItem
                    {
                        Name = name,
                        State = state,
                        ItemState = itemState,
                        Position = position,
                        FINItemStateFileSystem = fINItemStateFileSystem,
                    };
                }
                else
                {
                    var properties = _propertySerializer.DeserializeProperties(reader, header, expectedPosition: expectedPosition).ToArray();

                    position = _vectorSerializer.DeserializeVec4BAs4I(reader);

                    items[x] = new StatefulItem
                    {
                        Name = name,
                        State = state,
                        ItemState = itemState,
                        Position = position,
                        Properties = properties
                    };
                }

                break;
            }

            position = _vectorSerializer.DeserializeVec4BAs4I(reader);

            items[x] = new Item
            {
                Name = name,
                ItemState = itemState,
                Position = position,
            };
        }

        var conveyorChainActor = new ConveyorChainActor
        {
            ConveyorActors = conveyors,
            Count = count,
            HeadItemIndex = headItemIndex,
            TailItemIndex = tailItemIndex,
            NumberItems = numberItems,
            Items = items,
            TotalLength = totalLength,
            Unknown1 = unknownRoot1,
            Unknown2 = unknownRoot2
        };

        return conveyorChainActor;
    }

    private ConveyorData DeserializeConveyor(BinaryReader reader, Header header)
    {
        var count = reader.ReadInt32();
        var nrElements = reader.ReadInt32();

        var items = new Item[nrElements];

        for (var x = 0; x < nrElements; x++)
        {
            var name = _objectReferenceSerializer.Deserialize(reader);
            Vector4I position;
            ObjectReference? itemState = null;

            if (header.SaveVersion >= 44)
            {
                var state = reader.ReadInt32();
                if (state != 0)
                {
                    itemState = _objectReferenceSerializer.Deserialize(reader);

                    if (string.Equals(itemState?.PathName, "/Script/FicsItNetworksComputer.FINItemStateFileSystem"))
                    {
                        var length = reader.ReadInt32();
                        var fINItemStateFileSystem = _hexSerializer.Deserialize(reader, length);

                        position = _vectorSerializer.DeserializeVec4BAs4I(reader);

                        items[x] = new StatefulItem
                        {
                            Name = name,
                            ItemState = itemState,
                            Position = position,
                            State = state,
                            FINItemStateFileSystem = fINItemStateFileSystem
                        };
                        break;
                    }
                    else
                    {
                        var properties = _propertySerializer.DeserializeProperties(reader, header).ToArray();

                        position = _vectorSerializer.DeserializeVec4BAs4I(reader);

                        items[x] = new StatefulItem
                        {
                            Name = name,
                            ItemState = itemState,
                            Position = position,
                            State = state,
                            Properties = properties
                        };
                        break;
                    }
                }
            }
            else
            {
                itemState = _objectReferenceSerializer.Deserialize(reader);
            }

            position = _vectorSerializer.DeserializeVec4BAs4I(reader);

            items[x] = new Item
            {
                Name = name,
                ItemState = itemState,
                Position = position
            };
        }

        return new ConveyorData
        {
            Items = items,
            Count = count
        };
    }

    private LocomotiveData DeserializeLocomotiveData(BinaryReader reader, Header header)
    {
        var count = reader.ReadInt32();
        var nrElements = reader.ReadInt32();
        var vehicleObjects = new CargoObject[nrElements];

        var unknownSize = header.SaveVersion >= 41 ? 105 : 53;

        for (var x = 0; x < nrElements; x++)
        {
            var name = _stringSerializer.Deserialize(reader);
            var unknown = _hexSerializer.Deserialize(reader, unknownSize);

            vehicleObjects[x] = new CargoObject
            {
                Name = name,
                Unknown = unknown,
            };
        }

        var previous = _objectReferenceSerializer.Deserialize(reader);
        var next = _objectReferenceSerializer.Deserialize(reader);

        return new LocomotiveData
        {
            Count = count,
            CargoObjects = vehicleObjects,
            Previous = previous,
            Next = next
        };
    }

    private PowerLineData DeserializePowerLine(BinaryReader reader, Header header)
    {
        var count = reader.ReadInt32();
        var source = _objectReferenceSerializer.Deserialize(reader);
        var target = _objectReferenceSerializer.Deserialize(reader);
        Vector3? sourceTranslation = null;
        Vector3? targetTranslation = null;

        if (header.SaveVersion >= 33 && header.SaveVersion < 41)
        {
            sourceTranslation = _vectorSerializer.DeserializeVec3(reader);
            targetTranslation = _vectorSerializer.DeserializeVec3(reader);
        }

        return new PowerLineData
        {
            Count = count,
            Source = source,
            Target = target,
            SourceTranslation = sourceTranslation,
            TargetTranslation = targetTranslation
        };
    }

    private VehicleData DeserializeVehicle(BinaryReader reader, Header header)
    {
        var count = reader.ReadInt32();
        var nrElements = reader.ReadInt32();
        var vehicleObjects = new CargoObject[nrElements];

        var unknownSize = header.SaveVersion >= 41 ? 105 : 53;

        for (var x = 0; x < nrElements; x++)
        {
            var name = _stringSerializer.Deserialize(reader);
            var unknown = _hexSerializer.Deserialize(reader, unknownSize);

            vehicleObjects[x] = new CargoObject
            {
                Name = name,
                Unknown = unknown,
            };
        }

        return new VehicleData
        {
            Count = count,
            CargoObjects = vehicleObjects
        };
    }
}