using SatisfactorySaveNet.Abstracts;
using SatisfactorySaveNet.Abstracts.Extra;
using SatisfactorySaveNet.Abstracts.Maths.Vector;
using SatisfactorySaveNet.Abstracts.Model;
using SatisfactorySaveNet.Abstracts.Model.Extra;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace SatisfactorySaveNet;

public class ExtraDataSerializer : IExtraDataSerializer
{
    public static readonly IExtraDataSerializer Instance = new ExtraDataSerializer(StringSerializer.Instance, ObjectReferenceSerializer.Instance, VectorSerializer.Instance, HexSerializer.Instance, PropertySerializer.Instance);

    private readonly IStringSerializer _stringSerializer;
    private readonly IObjectReferenceSerializer _objectReferenceSerializer;
    private readonly IVectorSerializer _vectorSerializer;
    private readonly IHexSerializer _hexSerializer;
    private readonly IPropertySerializer _propertySerializer;

    public ExtraDataSerializer(IStringSerializer stringSerializer, IObjectReferenceSerializer objectReferenceSerializer, IVectorSerializer vectorSerializer, IHexSerializer hexSerializer, IPropertySerializer propertySerializer)
    {
        _stringSerializer = stringSerializer;
        _objectReferenceSerializer = objectReferenceSerializer;
        _vectorSerializer = vectorSerializer;
        _hexSerializer = hexSerializer;
        _propertySerializer = propertySerializer;
    }

    public ExtraData? Deserialize(BinaryReader reader, string typePath, Header header, long expectedPosition)
    {
        if (KnownConstants.IsConveyor(typePath))
            return DeserializeConveyor(reader);
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
            _ => DeserializeUnknownData(reader, header, typePath, expectedPosition),
        };
    }

    private UnknownExtraData? DeserializeUnknownData(BinaryReader reader, Header header, string typePath, long expectedPosition)
    {
        var bytesCount = expectedPosition - reader.BaseStream.Position;
        
        if (bytesCount > 4)
        {
            if (header.SaveVersion >= 41 && typePath.StartsWith("/Script/FactoryGame.FG", StringComparison.Ordinal))
                reader.BaseStream.Seek(8, SeekOrigin.Current);
            else
            {
                var missing = _hexSerializer.Deserialize(reader, (int) bytesCount);

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
        var missing = _hexSerializer.Deserialize(reader, (int)bytesCount);

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
            var missing = _hexSerializer.Deserialize(reader, (int)bytesCount);
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

                    playerData.EpicOnlineServicesId = sb1.ToString().TrimStart('0'); //.Substring(1, 32);
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

    private ConveyorData DeserializeConveyor(BinaryReader reader)
    {
        var count = reader.ReadInt32();
        var nrElements = reader.ReadInt32();

        var items = new Item[nrElements];

        for (var x = 0; x < nrElements; x++)
        {
            var length = reader.ReadInt32();
            var name = _stringSerializer.Deserialize(reader);
            var objectReference = _objectReferenceSerializer.Deserialize(reader);
            var position = VectorSerializer.Instance.DeserializeVec4B(reader);

            items[x] = new Item
            {
                Name = name,
                ObjectReference = objectReference,
                Position = position,
                Length = length
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