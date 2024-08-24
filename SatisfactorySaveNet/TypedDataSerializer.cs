using SatisfactorySaveNet.Abstracts;
using SatisfactorySaveNet.Abstracts.Model;
using SatisfactorySaveNet.Abstracts.Model.Properties;
using SatisfactorySaveNet.Abstracts.Model.Typed;
using System;
using System.Collections.Frozen;
using System.IO;
using System.Linq;
using DateTime = SatisfactorySaveNet.Abstracts.Model.Typed.DateTime;
using Guid = SatisfactorySaveNet.Abstracts.Model.Typed.Guid;

namespace SatisfactorySaveNet;

public class TypedDataSerializer : ITypedDataSerializer
{
    public static readonly ITypedDataSerializer Instance = new TypedDataSerializer(VectorSerializer.Instance, StringSerializer.Instance, ObjectReferenceSerializer.Instance, HexSerializer.Instance, SoftObjectReferenceSerializer.Instance);

    private readonly IVectorSerializer _vectorSerializer;
    private readonly IStringSerializer _stringSerializer;
    private readonly IPropertySerializer _propertySerializer;
    private readonly IHexSerializer _hexSerializer;
    private readonly IObjectReferenceSerializer _objectReferenceSerializer;

    internal TypedDataSerializer(IVectorSerializer vectorSerializer, IStringSerializer stringSerializer, IPropertySerializer propertySerializer, IHexSerializer hexSerializer, IObjectReferenceSerializer objectReferenceSerializer)
    {
        _vectorSerializer = vectorSerializer;
        _stringSerializer = stringSerializer;
        _propertySerializer = propertySerializer;
        _hexSerializer = hexSerializer;
        _objectReferenceSerializer = objectReferenceSerializer;
    }

    public TypedDataSerializer(IVectorSerializer vectorSerializer, IStringSerializer stringSerializer, IObjectReferenceSerializer objectReferenceSerializer, IHexSerializer hexSerializer, ISoftObjectReferenceSerializer softObjectReferenceSerializer)
    {
        _vectorSerializer = vectorSerializer;
        _stringSerializer = stringSerializer;
        _propertySerializer = new PropertySerializer(stringSerializer, objectReferenceSerializer, this, hexSerializer, vectorSerializer, softObjectReferenceSerializer);
        _hexSerializer = hexSerializer;
        _objectReferenceSerializer = objectReferenceSerializer;
    }

    public TypedData Deserialize(BinaryReader reader, Header header, string type, bool isArrayProperty)
    {
        return type switch
        {
            nameof(Color) => DeserializeColor(reader),
            nameof(LinearColor) => DeserializeLinearColor(reader),
            nameof(Vector) => DeserializeVector(reader, header),
            nameof(Rotator) => DeserializeRotator(reader, header, type),
            nameof(Vector2D) => DeserializeVector2D(reader, header),
            nameof(Quat) => DeserializeQuat(reader, header),
            nameof(Vector4) => DeserializeVector4(reader, header),
            nameof(Box) => DeserializeBox(reader, header),
            nameof(RailroadTrackPosition) => DeserializeRailroadTrackPosition(reader),
            nameof(TimerHandle) => DeserializeTimerHandle(reader),
            nameof(Guid) => DeserializeGuid(reader),
            nameof(InventoryItem) => DeserializeInventoryItem(reader, header, isArrayProperty),
            nameof(FluidBox) => DeserializeFluidBox(reader),
            nameof(SlateBrush) => DeserializeSlateBrush(reader),
            nameof(DateTime) => DeserializeDateTime(reader),
            nameof(FINNetworkTrace) => DeserializeFINNetworkTrace(reader),
            nameof(FINLuaProcessorStateStorage) => DeserializeFINLuaProcessorStateStorage(reader, header),
            nameof(FICFrameRange) => DeserializeFICFrameRange(reader),
            nameof(IntPoint) => DeserializeIntPoint(reader),
            nameof(FINGPUT1BufferPixel) => DeserializeFINGPUT1BufferPixel(reader),
            //ToDo: All implemented?

            //nameof(InventoryStack) => DeserializeInventoryStack(reader, header),
            //nameof(SpawnData) => DeserializeSpawnData(reader), False?
            //nameof(FactoryCustomizationColorSlot) => DeserializeFactoryCustomizationColorSlot(reader, header, endPosition), False?

            //nameof(PhaseCost) => DeserializePhaseCost(reader),
            //"" => DeserializeProperty(reader),
            _ => DeserializeArrayProperties(reader, header, type)
        };
    }

    //private ITypedData DeserializeProperty(BinaryReader reader)
    //{
    //    var value = _propertySerializer.DeserializeProperty(reader);
    //
    //    return new PropertyData
    //    {
    //        Value = value
    //    };
    //}

    private ArrayProperties DeserializeArrayProperties(BinaryReader reader, Header header, string type)
    {
        var values = _propertySerializer.DeserializeProperties(reader, header).ToArray();

        return new ArrayProperties
        {
            TypeName = type,
            Values = values
        };
    }

    private FINGPUT1BufferPixel DeserializeFINGPUT1BufferPixel(BinaryReader reader)
    {
        var character = _hexSerializer.Deserialize(reader, 2);
        var foregroundColor = _vectorSerializer.DeserializeVec4(reader);
        var backgroundColor = _vectorSerializer.DeserializeVec4(reader);

        return new FINGPUT1BufferPixel
        {
            Character = character,
            ForegroundColor = foregroundColor,
            BackgroundColor = backgroundColor
        };
    }

    private IntPoint DeserializeIntPoint(BinaryReader reader)
    {
        var value = _vectorSerializer.DeserializeVec2I(reader);

        return new IntPoint
        {
            Value = value
        };
    }

    private static FICFrameRange DeserializeFICFrameRange(BinaryReader reader)
    {
        var begin = reader.ReadInt64();
        var end = reader.ReadInt64();

        return new FICFrameRange
        {
            Begin = begin,
            End = end
        };
    }

    private FINLuaProcessorStateStorage DeserializeFINLuaProcessorStateStorage(BinaryReader reader, Header header)
    {
        var nrTraces = reader.ReadInt32();
        var traces = new FINNetworkTrace[nrTraces];
        
        for (var x = 0; x < nrTraces; x++)
            traces[x] = DeserializeFINNetworkTrace(reader);

        var nrObjectReferences = reader.ReadInt32();
        var objectReferences = new ObjectReference[nrObjectReferences];

        for (var x = 0; x < nrObjectReferences; x++)
            objectReferences[x] = _objectReferenceSerializer.Deserialize(reader);

        var thread = _stringSerializer.Deserialize(reader);
        var globals = _stringSerializer.Deserialize(reader);

        var nrTypedData = reader.ReadInt32();
        var typedData = new TypedData[nrTypedData];
        for (var x = 0; x < nrTypedData; x++)
        {
            var unknown1 = reader.ReadInt32();
            var objectPath = _stringSerializer.Deserialize(reader);

            TypedData data;
            switch (objectPath) {
                case "/Script/CoreUObject.Vector":
                    data = DeserializeVector(reader);
                    break;
                case "/Script/CoreUObject.LinearColor":
                    data = DeserializeLinearColor(reader);
                    break;
                case "/Script/FactoryGame.InventoryStack":
                    data = DeserializeInventoryStack(reader, header);
                    break;
                case "/Script/FactoryGame.ItemAmount":
                    data = DeserializeItemAmount(reader);
                    break;
                case "/Script/FicsItNetworks.FINTrackGraph":
                    data = DeserializeFINNetworkTrace(reader);
                    break;
                case "/Script/FactoryGame.PrefabSignData":
                case "/Script/FicsItNetworks.FINInternetCardHttpRequestFuture":
                case "/Script/FactoryGame.InventoryItem":
                case "/Script/FicsItNetworks.FINRailroadSignalBlock":
                    continue;
                case "/Script/FicsItNetworks.FINGPUT1Buffer":
                    data = DeserializeFINGPUT1Buffer(reader);
                    break;
                default:
                    throw new InvalidDataException($"Unknown object path for {nameof(FINLuaProcessorStateStorage)}");
            }
            typedData[x] = data;
        }

        return new FINLuaProcessorStateStorage
        {
            Traces = traces,
            ObjectReferences = objectReferences,
            Thread = thread,
            Globals = globals,
            TypedData = typedData
        };
    }

    private FINGPUT1Buffer DeserializeFINGPUT1Buffer(BinaryReader reader)
    {
        var vector = _vectorSerializer.DeserializeVec2I(reader);
        var size = reader.ReadInt32();
        var name = _stringSerializer.Deserialize(reader);
        var typeName = _stringSerializer.Deserialize(reader);
        var length = reader.ReadInt32();
        var buffer = new FINGPUT1BufferPixel[size];

        for (var x = 0; x < size; x++)
            buffer[x] = DeserializeFINGPUT1BufferPixel(reader);

        var unknown = _hexSerializer.Deserialize(reader, 45);

        return new FINGPUT1Buffer
        {
            Vector = vector,
            Name = name,
            TypeName = typeName,
            Length = length,
            Buffer = buffer,
            Unknown = unknown
        };
    }

    private ItemAmount DeserializeItemAmount(BinaryReader reader)
    {
        var unknown1 = reader.ReadInt32();
        var unknown2 = _stringSerializer.Deserialize(reader);
        var unknown3 = reader.ReadInt32();

        return new ItemAmount
        {
            Unknown1 = unknown1,
            Unknown2 = unknown2,
            Unknown3 = unknown3
        };
    }

    private FINNetworkTrace DeserializeFINNetworkTrace(BinaryReader reader)
    {
        var values = _propertySerializer.DeserializeProperties(reader, type: nameof(FINNetworkProperty)).OfType<FINNetworkProperty>().ToArray();

        return new FINNetworkTrace
        {
            Values = values
        };
    }

    private static DateTime DeserializeDateTime(BinaryReader reader)
    {
        var value = reader.ReadInt64();

        return new DateTime
        {
            Value = new System.DateTime(value)
        };
    }

    private SlateBrush DeserializeSlateBrush(BinaryReader reader)
    {
        var value = _stringSerializer.Deserialize(reader);

        return new SlateBrush
        {
            Unknown = value
        };
    }

    private Guid DeserializeGuid(BinaryReader reader)
    {
        var value = _hexSerializer.Deserialize(reader, 16);

        return new Guid
        {
            Value = value
        };
    }

    private TimerHandle DeserializeTimerHandle(BinaryReader reader)
    {
        var value = _stringSerializer.Deserialize(reader);

        return new TimerHandle
        {
            Value = value
        };
    }

    private TypedData DeserializeVector2D(BinaryReader reader, Header header)
    {
        return header.SaveVersion >= 41
            ? new Vector2D { Value = _vectorSerializer.DeserializeVec2D(reader) }
            : new Vector2 { Value = _vectorSerializer.DeserializeVec2(reader) };
    }
    
    private TypedData DeserializeVector4(BinaryReader reader, Header header)
    {
        return header.SaveVersion >= 41
            ? new Vector4D { Value = _vectorSerializer.DeserializeVec4D(reader) }
            : new Vector4 { Value = _vectorSerializer.DeserializeVec4(reader) };
    }

    private Vector DeserializeVector(BinaryReader reader)
    {
        return new Vector { Value = _vectorSerializer.DeserializeVec3(reader) };
    }

    private TypedData DeserializeVector(BinaryReader reader, Header header)
    {
        return header.SaveVersion >= 41
            ? new VectorD { Value = _vectorSerializer.DeserializeVec3D(reader) }
            : new Vector { Value = _vectorSerializer.DeserializeVec3(reader) };
    }

    private TypedData DeserializeRotator(BinaryReader reader, Header header, string typeName)
    {
        return header.SaveVersion >= 41 && !typeName.Equals("SpawnData", StringComparison.Ordinal)
            ? new RotatorD { Value = _vectorSerializer.DeserializeVec3D(reader) }
            : new Rotator { Value = _vectorSerializer.DeserializeVec3(reader) };
    }

    private SpawnData DeserializeSpawnData(BinaryReader reader)
    {
        var properties = _propertySerializer.DeserializeProperties(reader).ToArray();

        return new SpawnData
        {
            Properties = properties
        };
    }

    private RailroadTrackPosition DeserializeRailroadTrackPosition(BinaryReader reader)
    {
        var levelName = _stringSerializer.Deserialize(reader); //ToDo: ObjectReference
        var pathName = _stringSerializer.Deserialize(reader);
        var offset = reader.ReadSingle();
        var forward = reader.ReadSingle();

        return new RailroadTrackPosition
        {
            LevelName = levelName,
            PathName = pathName,
            Offset = offset,
            Forward = forward
        };
    }

    private TypedData DeserializeQuat(BinaryReader reader, Header header)
    {
        if (header.SaveVersion >= 41)
        {
            var value = _vectorSerializer.DeserializeQuaternionD(reader);

            return new QuatD
            {
                Value = value
            };
        }
        else
        {
            var value = _vectorSerializer.DeserializeQuaternion(reader);

            return new Quat
            {
                Value = value
            };
        }
    }

    private Color DeserializeColor(BinaryReader reader)
    {
        var color = _vectorSerializer.DeserializeVec4B(reader);

        return new Color
        {
            Value = color
        };
    }

    private LinearColor DeserializeLinearColor(BinaryReader reader)
    {
        var color = _vectorSerializer.DeserializeVec4(reader);

        return new LinearColor
        {
            Color = color
        };
    }

    //Seems to be dead code
    private TypedData DeserializeInventoryStack(BinaryReader reader, Header header)
    {
        if (header.SaveVersion >= 42)
        {
            var unknown1 = _stringSerializer.Deserialize(reader);
            var unknown2 = _stringSerializer.Deserialize(reader);
            var unknown3 = reader.ReadInt32();
            var unknown4 = reader.ReadInt32();
            var unknown5 = _propertySerializer.DeserializeProperty(reader, header, unknown2);
            var unknown6 = _stringSerializer.Deserialize(reader);

            return new InventoryStackV1
            {
                Unknown1 = unknown1,
                Unknown2 = unknown2,
                Unknown3 = unknown3,
                Unknown4 = unknown4,
                Unknown5 = unknown5,
                Unknown6 = unknown6,
            };
        }
        else
        {
            var unknown1 = reader.ReadInt32();
            var unknown2 = _stringSerializer.Deserialize(reader);
            var unknown3 = reader.ReadInt32();
            var unknown4 = reader.ReadInt32();
            var unknown5 = reader.ReadInt32();

            return new InventoryStack
            {
                Unknown1 = unknown1,
                Unknown2 = unknown2,
                Unknown3 = unknown3,
                Unknown4 = unknown4,
                Unknown5 = unknown5,
            };
        }
    }

    private static readonly FrozenSet<string> FuelContainingItems = new string[]
    {
        "/Game/FactoryGame/Equipment/Chainsaw/Desc_Chainsaw.Desc_Chainsaw_C",
        "/Game/FactoryGame/Resource/Equipment/JetPack/BP_EquipmentDescriptorJetPack.BP_EquipmentDescriptorJetPack_C",
        "/Game/FactoryGame/Resource/Equipment/NailGun/Desc_RebarGunProjectile.Desc_RebarGunProjectile_C",
        "/Game/FactoryGame/Resource/Equipment/Rifle/BP_EquipmentDescriptorRifle.BP_EquipmentDescriptorRifle_C",
        "/Game/FactoryGame/Resource/Equipment/NobeliskDetonator/BP_EquipmentDescriptorNobeliskDetonator.BP_EquipmentDescriptorNobeliskDetonator_C"
    }.ToFrozenSet(StringComparer.Ordinal);

    private InventoryItem DeserializeInventoryItem(BinaryReader reader, Header header, bool isArrayProperty)
    {
        var padding1 = reader.ReadInt32();
        var itemType = _stringSerializer.Deserialize(reader);
        ObjectReference? objectReference = null;
        var hasState = false;

        if (header.SaveVersion < 44)
            objectReference = _objectReferenceSerializer.Deserialize(reader);
        else
            hasState = reader.ReadInt32() != 0;

        Property? property = null;

        if (header.SaveVersion >= 44 && FuelContainingItems.Contains(itemType) && hasState)
        {
            var padding2 = reader.ReadInt32();
            var scriptName = _stringSerializer.Deserialize(reader);
            var unknown = reader.ReadInt32();
            var properties = _propertySerializer.DeserializeProperties(reader, header).ToArray();

            if (!isArrayProperty)
                property = _propertySerializer.DeserializeProperty(reader, header);

            return new FueledInventoryItem
            {
                ItemType = itemType,
                ObjectReference = objectReference,
                ExtraProperty = property,
                Properties = properties,
                ScriptName = scriptName,
                Unknown1 = unknown
            };
        }

        if (!isArrayProperty)
            property = _propertySerializer.DeserializeProperty(reader, header);

        return new InventoryItem
        {
            ItemType = itemType,
            ObjectReference = objectReference,
            ExtraProperty = property
        };
    }

    private static FluidBox DeserializeFluidBox(BinaryReader reader)
    {
        var value = reader.ReadSingle();

        return new FluidBox
        {
            Value = value
        };
    }

    //private FactoryCustomizationColorSlot DeserializeFactoryCustomizationColorSlot(BinaryReader reader, Header header, long endPosition)
    //{
    //    var properties = _propertySerializer.DeserializeProperties(reader, header).ToArray();
    //
    //    return new FactoryCustomizationColorSlot
    //    {
    //        Properties = properties
    //    };
    //}

    private TypedData DeserializeBox(BinaryReader reader, Header header)
    {
        if (header.SaveVersion >= 41)
        {
            var min = _vectorSerializer.DeserializeVec3D(reader);
            var max = _vectorSerializer.DeserializeVec3D(reader);
            var isValid = reader.ReadSByte();

            return new BoxD
            {
                Min = min,
                Max = max,
                IsValid = isValid
            };
        }
        else
        {
            var min = _vectorSerializer.DeserializeVec3(reader);
            var max = _vectorSerializer.DeserializeVec3(reader);
            var isValid = reader.ReadSByte();

            return new Box
            {
                Min = min,
                Max = max,
                IsValid = isValid
            };
        }
    }
}
