using Microsoft.Extensions.Logging.Abstractions;
using SatisfactorySaveNet.Abstracts;
using SatisfactorySaveNet.Abstracts.Exceptions;
using SatisfactorySaveNet.Abstracts.Model;
using SatisfactorySaveNet.Abstracts.Model.Properties;
using SatisfactorySaveNet.Abstracts.Model.Typed;
using System;
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
        _propertySerializer = new PropertySerializer(stringSerializer, objectReferenceSerializer, this, hexSerializer, vectorSerializer, softObjectReferenceSerializer, NullLoggerFactory.Instance);
        _hexSerializer = hexSerializer;
        _objectReferenceSerializer = objectReferenceSerializer;
    }

    public TypedData Deserialize(BinaryReader reader, Header header, string type, bool isArrayProperty, int binarySize)
    {
        return type switch
        {
            nameof(Color) => DeserializeColor(reader),
            nameof(LinearColor) => DeserializeLinearColor(reader),
            nameof(Vector) => DeserializeVector(reader, header, binarySize),
            nameof(Rotator) => DeserializeRotator(reader, header, type),
            nameof(Vector2D) => DeserializeVector2D(reader, header),
            nameof(Quat) => DeserializeQuat(reader, header),
            nameof(IntVector4) => DeserializeVector4I(reader),
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
            "FIRTrace" => DeserializeFINNetworkTrace(reader),
            nameof(FINLuaProcessorStateStorage) => DeserializeFINLuaProcessorStateStorage(reader, header),
            "FINLuaRuntimePersistenceState" => DeserializeFINLuaProcessorStateStorage(reader, header),
            nameof(FIRExecutionContext) => DeserializeFIRExecutionContext(reader),
            nameof(FICFrameRange) => DeserializeFICFrameRange(reader),
            nameof(IntPoint) => DeserializeIntPoint(reader),
            nameof(FINGPUT1BufferPixel) => DeserializeFINGPUT1BufferPixel(reader),
            nameof(FINDynamicStructHolder) => DeserializeFINDynamicStructHolder(reader, header),
            "FIRInstancedStruct" => DeserializeFINDynamicStructHolder(reader, header),
            nameof(ClientIdentityInfo) => DeserializeClientIdentityInfo(reader, binarySize),
            nameof(FIRAnyValue) => DeserializeFIRAnyValue(reader, header),

            nameof(InventoryStack) => DeserializeArrayProperties(reader, header, type),
            nameof(SpawnData) => DeserializeArrayProperties(reader, header, type),
            "FactoryCustomizationColorSlot" => DeserializeArrayProperties(reader, header, type),
            "PhaseCost" => DeserializeArrayProperties(reader, header, type),
            "GlobalColorPreset" => DeserializeArrayProperties(reader, header, type),
            "MiniGameResult" => DeserializeArrayProperties(reader, header, type),
            "ItemAmount" => DeserializeArrayProperties(reader, header, type),
            "MapMarker" => DeserializeArrayProperties(reader, header, type),
            "ScannableResourcePair" => DeserializeArrayProperties(reader, header, type),
            "SwatchGroupData" => DeserializeArrayProperties(reader, header, type),
            "Vector_NetQuantize" => DeserializeArrayProperties(reader, header, type),
            "ItemFoundData" => DeserializeArrayProperties(reader, header, type),
            "Transform" => DeserializeArrayProperties(reader, header, type),
            "RemovedInstanceArray" => DeserializeArrayProperties(reader, header, type),
            "ResearchCost" => DeserializeArrayProperties(reader, header, type),
            "TimeTableStop" => DeserializeArrayProperties(reader, header, type),
            "ScannableObjectData" => DeserializeArrayProperties(reader, header, type),
            "HighlightedMarkerPair" => DeserializeArrayProperties(reader, header, type),
            "TrainDockingRuleSet" => DeserializeArrayProperties(reader, header, type),
            "RemovedInstance" => DeserializeArrayProperties(reader, header, type),
            "CompletedResearch" => DeserializeArrayProperties(reader, header, type),
            "FactoryCustomizationData" => DeserializeArrayProperties(reader, header, type),
            "TrainSimulationData" => DeserializeArrayProperties(reader, header, type),
            "ResourceSinkHistory" => DeserializeArrayProperties(reader, header, type),
            "RecipeAmountStruct" => DeserializeArrayProperties(reader, header, type),
            "ResearchData" => DeserializeArrayProperties(reader, header, type),
            "Hotbar" => DeserializeArrayProperties(reader, header, type),
            "GCheckmarkUnlockData" => DeserializeArrayProperties(reader, header, type),
            "BlueprintCategoryRecord" => DeserializeArrayProperties(reader, header, type),
            "BlueprintSubCategoryRecord" => DeserializeArrayProperties(reader, header, type),
            "SplinePointData" => DeserializeArrayProperties(reader, header, type),
            "LampGroup" => DeserializeArrayProperties(reader, header, type),
            "MessageData" => DeserializeArrayProperties(reader, header, type),
            "SplitterSortRule" => DeserializeArrayProperties(reader, header, type),
            "SubCategoryMaterialDefault" => DeserializeArrayProperties(reader, header, type),
            "FeetOffset" => DeserializeArrayProperties(reader, header, type),
            "WireInstance" => DeserializeArrayProperties(reader, header, type),
            "DroneTripInformation" => DeserializeArrayProperties(reader, header, type),
            "LightSourceControlData" => DeserializeArrayProperties(reader, header, type),
            "PrefabTextElementSaveData" => DeserializeArrayProperties(reader, header, type),
            "PlayerRules" => DeserializeArrayProperties(reader, header, type),
            "PrefabIconElementSaveData" => DeserializeArrayProperties(reader, header, type),
            "BoomBoxPlayerState" => DeserializeArrayProperties(reader, header, type),
            "ShoppingListRecipeEntry" => DeserializeArrayProperties(reader, header, type),
            "BlueprintRecord" => DeserializeArrayProperties(reader, header, type),
            "DroneDockingStateInfo" => DeserializeArrayProperties(reader, header, type),
            "FGDroneFuelRuntimeData" => DeserializeArrayProperties(reader, header, type),
            "ShoppingListBlueprintEntry" => DeserializeArrayProperties(reader, header, type),
            "FICAttributeBool" => DeserializeArrayProperties(reader, header, type),
            "PlayerCustomizationData" => DeserializeArrayProperties(reader, header, type),
            "FICAttributePosition" => DeserializeArrayProperties(reader, header, type),
            "FICFloatAttribute" => DeserializeArrayProperties(reader, header, type),
            "FICAttributeRotation" => DeserializeArrayProperties(reader, header, type),

            "HardDriveData" => DeserializeArrayProperties(reader, header, type),
            "SchematicCost" => DeserializeArrayProperties(reader, header, type),
            "FGPlayerPortalData" => DeserializeArrayProperties(reader, header, type),
            "ShoppingListClassEntry" => DeserializeArrayProperties(reader, header, type),
            "FGPortalCachedFactoryTickData" => DeserializeArrayProperties(reader, header, type),

            _ => DeserializeArrayProperties(reader, header, type)
        };
    }

    private FIRExecutionContext DeserializeFIRExecutionContext(BinaryReader reader)
    {
        var unknown1 = reader.ReadInt32();
        var trace = DeserializeFINNetworkTrace(reader);

        return new FIRExecutionContext
        {
            Unknown1 = unknown1,
            Trace = trace
        };
    }

    private FIRAnyValue DeserializeFIRAnyValue(BinaryReader reader, Header header)
    {
        var valueType = reader.ReadSByte();

        return valueType switch
        {
            4 => new FIRStringValue { Value = _stringSerializer.Deserialize(reader), ValueType = valueType },
            8 => new FIRFINDynamicStructHolderValue
            {
                Value = DeserializeFINDynamicStructHolder(reader, header), ValueType = valueType
            },
            _ => throw new NotSupportedException($"Unimplemented type '{valueType}' in ReadFIRAnyValue.")
        };
    }

    private FINDynamicStructHolder DeserializeFINDynamicStructHolder(BinaryReader reader, Header header)
    {
        var unknown1 = reader.ReadInt32();
        var type = _stringSerializer.Deserialize(reader);
        return MapNetwork(reader, header, unknown1, type);
    }

    private FINDynamicStructHolder MapNetwork(BinaryReader reader, Header header, int unknown1, string type)
    {
        Property[] properties;

        switch (type)
        {
            case "/Script/FicsItNetworks.FINGPUT2DC_Box":
            case "/Script/FicsItNetworksComputer.FINGPUT2DC_Box":
            case "/Script/FicsItNetworksComputer.FINGPUT2DC_Text":
            case "/Script/FicsItNetworksComputer.FINGPUT2DC_PushClipRect":
            case "/Script/FicsItNetworksComputer.FINGPUT2DC_PopClip":
            case "/Script/FicsItNetworksLua.FINEventFilter":
            case "/Script/FactoryGame.PrefabSignData":
                properties = [.. _propertySerializer.DeserializeProperties(reader, header, type)];
                return new FINGPUT2DC_Box
                {
                    Unknown1 = unknown1,
                    TypeName = type,
                    Properties = properties
                };
            case "/Script/FicsItNetworks.FINGPUT2DC_Lines":
                var unknown2 = _stringSerializer.Deserialize(reader);
                var unknown3 = _stringSerializer.Deserialize(reader);
                var unknown4 = reader.ReadInt32();
                var unknown5 = reader.ReadInt32();
                var unknown6 = _stringSerializer.Deserialize(reader);
                var unknown7 = reader.ReadByte();
                var unknown8 = reader.ReadInt32();
                var unknown9 = _propertySerializer.DeserializeProperty(reader, header) ?? throw new BadReadException("Expected property to be read");
                var unknown10 = new Abstracts.Maths.Vector.Vector2D[unknown8 - 1];
                for (var x = 0; x < unknown8 - 1; x++)
                {
                    unknown10[x] = _vectorSerializer.DeserializeVec2D(reader);
                }
                properties = [.. _propertySerializer.DeserializeProperties(reader, header, type)];
                return new FINGPUT2DC_Lines
                {
                    Unknown1 = unknown1,
                    Unknown2 = unknown2,
                    Unknown3 = unknown3,
                    Unknown4 = unknown4,
                    Unknown5 = unknown5,
                    Unknown6 = unknown6,
                    Unknown7 = unknown7,
                    Unknown8 = unknown8,
                    Unknown9 = unknown9,
                    Unknown10 = unknown10,
                    TypeName = type,
                    Properties = properties
                };
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

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

    private ClientIdentityInfo DeserializeClientIdentityInfo(BinaryReader reader, int binarySize)
    {
        var value = _stringSerializer.Deserialize(reader);

        if (binarySize - 4 - value.Length > 0)
        {
            _ = reader.ReadBytes(binarySize - 4 - value.Length - 1);
        }

        return new ClientIdentityInfo
        {
            Value = value
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
        var typedData = new TypedData?[nrTypedData];
        for (var x = 0; x < nrTypedData; x++)
        {
            var unknown1 = reader.ReadInt32();
            var objectPath = _stringSerializer.Deserialize(reader);

            TypedData? data = null;
            switch (objectPath) {
                case "/Script/CoreUObject.Vector":
                    data = DeserializeVector(reader);
                    break;
                case "/Script/CoreUObject.Vector2D":
                    data = DeserializeVector2D(reader);
                    break;
                case "/Script/CoreUObject.LinearColor":
                    data = DeserializeLinearColor(reader);
                    break;
                case "/Script/FactoryGame.InventoryStack":
                    data = DeserializeInventoryStack(reader, header, objectPath);
                    break;
                case "/Script/FactoryGame.ItemAmount":
                    data = DeserializeItemAmount(reader);
                    break;
                case "/Script/FicsItNetworks.FINTrackGraph":
                    data = DeserializeFINNetworkTrace(reader);
                    break;
                case "/Script/FicsItNetworks.FINGPUT1Buffer":
                    data = DeserializeFINGPUT1Buffer(reader);
                    break;
                case "/Script/FicsItNetworksLua.FINLuaEventRegistry":
                case "/Script/FicsItNetworksMisc.FINFutureReflection":
                case "/Script/FactoryGame.PrefabSignData":
                    if (header.SaveVersion >= 46)
                    {
                        var properties = _propertySerializer.DeserializeProperties(reader, header).ToArray();
                        data = new TypedDataCollection
                        {
                            Properties = properties
                        };
                    }
                    break;
                case "/Script/FactoryGame.InventoryItem":
                    if (header.SaveVersion >= 46)
                    {
                        data = DeserializeInventoryItem(reader, header, false);
                    }
                    break;
                case "/Script/FicsItNetworks.FINInternetCardHttpRequestFuture":
                case "/Script/FicsItNetworksComputer.FINInternetCardHttpRequestFuture":
                case "/Script/FicsItNetworks.FINRailroadSignalBlock":
                    continue;
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
        var unknown1 = _objectReferenceSerializer.Deserialize(reader);
        var unknown2 = reader.ReadInt32();

        return new ItemAmount
        {
            Unknown1 = unknown1,
            Unknown2 = unknown2,
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

    private Vector4 DeserializeVector4I(BinaryReader reader)
    {
        return new Vector4 { Value = _vectorSerializer.DeserializeVec4(reader) };
    }

    private TypedData DeserializeVector4(BinaryReader reader, Header header)
    {
        return header.SaveVersion >= 41
            ? new Vector4D { Value = _vectorSerializer.DeserializeVec4D(reader) }
            : new Vector4 { Value = _vectorSerializer.DeserializeVec4(reader) };
    }

    private Vector2D DeserializeVector2D(BinaryReader reader)
    {
        return new Vector2D { Value = _vectorSerializer.DeserializeVec2D(reader) };
    }

    private Vector DeserializeVector(BinaryReader reader)
    {
        return new Vector { Value = _vectorSerializer.DeserializeVec3(reader) };
    }

    private TypedData DeserializeVector(BinaryReader reader, Header header, int binarySize)
    {
        return header.SaveVersion >= 41 && binarySize != 12
            ? new VectorD { Value = _vectorSerializer.DeserializeVec3D(reader) }
            : new Vector { Value = _vectorSerializer.DeserializeVec3(reader) };
    }

    private TypedData DeserializeRotator(BinaryReader reader, Header header, string typeName)
    {
        return header.SaveVersion >= 41 && !typeName.Equals("SpawnData", StringComparison.Ordinal)
            ? new RotatorD { Value = _vectorSerializer.DeserializeVec3D(reader) }
            : new Rotator { Value = _vectorSerializer.DeserializeVec3(reader) };
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
        var color = _vectorSerializer.DeserializeVec4BAs4I(reader);

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
    private TypedData DeserializeInventoryStack(BinaryReader reader, Header header, string type)
    {
        if (header.SaveVersion >= 42)
        {
            var unknown1 = _objectReferenceSerializer.Deserialize(reader);
            var unknown2 = reader.ReadInt32();
            var unknown3 = reader.ReadInt32();
            var unknown4 = _propertySerializer.DeserializeProperty(reader, header, type);
            var unknown5 = _stringSerializer.Deserialize(reader);

            return new InventoryStackV1
            {
                Unknown1 = unknown1,
                Unknown2 = unknown2,
                Unknown3 = unknown3,
                Unknown4 = unknown4,
                Unknown5 = unknown5
            };
        }
        else
        {
            var unknown1 = _objectReferenceSerializer.Deserialize(reader);
            var unknown2 = reader.ReadInt32();
            var unknown3 = reader.ReadInt32();
            var unknown4 = reader.ReadInt32();

            return new InventoryStack
            {
                Unknown1 = unknown1,
                Unknown2 = unknown2,
                Unknown3 = unknown3,
                Unknown4 = unknown4
            };
        }
    }

    private InventoryItem DeserializeInventoryItem(BinaryReader reader, Header header, bool isArrayProperty)
    {
        _ = reader.ReadInt32();
        var itemType = _stringSerializer.Deserialize(reader);
        ObjectReference? itemState = null;
        var state = 0;

        if (header.SaveVersion < 44)
            itemState = _objectReferenceSerializer.Deserialize(reader);
        else
            state = reader.ReadInt32();

        Property? property = null;

        if (header.SaveVersion >= 44 && KnownConstants.StatefulInventoryItems.Contains(itemType) && state != 0)
        {
            _ = reader.ReadInt32();
            var scriptName = _stringSerializer.Deserialize(reader);

            //ToDo: Create reusable method for read item, see TypedDataSerializer and ExtraDataSerializer
            if (string.Equals(itemState?.PathName, "/Script/FicsItNetworksComputer.FINItemStateFileSystem"))
            {
                var length = reader.ReadInt32();

                var fINItemStateFileSystem = _hexSerializer.Deserialize(reader, length);

                return new StatefulInventoryItem
                {
                    State = state,
                    ItemType = itemType,
                    ItemState = itemState,
                    ExtraProperty = property,
                    ScriptName = scriptName,
                    FINItemStateFileSystem = fINItemStateFileSystem
                };
            }
            else
            {
                var unknown = reader.ReadInt32();
                var properties = _propertySerializer.DeserializeProperties(reader, header).ToArray();

                if (!isArrayProperty)
                    property = _propertySerializer.DeserializeProperty(reader, header);

                return new StatefulInventoryItem
                {
                    State = state,
                    ItemType = itemType,
                    ItemState = itemState,
                    ExtraProperty = property,
                    Properties = properties,
                    ScriptName = scriptName,
                    Unknown1 = unknown
                };
            }
        }

        if (header.SaveVersion >= 46 && reader.ReadInt32() != 0)
            reader.BaseStream.Seek(-4, SeekOrigin.Current);

        if (!isArrayProperty)
            property = _propertySerializer.DeserializeProperty(reader, header);

        return new InventoryItem
        {
            ItemType = itemType,
            ItemState = itemState,
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
