using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SatisfactorySaveNet.Abstracts;
using SatisfactorySaveNet.Abstracts.Model;
using SatisfactorySaveNet.Abstracts.Model.Properties;
using SatisfactorySaveNet.Abstracts.Model.Typed;
using SatisfactorySaveNet.Abstracts.Model.Union;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace SatisfactorySaveNet;

public class PropertySerializer : IPropertySerializer
{
    private const int SerializeDataPackageVersionAndCustomVersions = 53;

    public static readonly IPropertySerializer Instance = new PropertySerializer(StringSerializer.Instance, ObjectReferenceSerializer.Instance, VectorSerializer.Instance, HexSerializer.Instance, SoftObjectReferenceSerializer.Instance, NullLoggerFactory.Instance);

    private readonly IStringSerializer _stringSerializer;
    private readonly IObjectReferenceSerializer _objectReferenceSerializer;
    private readonly ISoftObjectReferenceSerializer _softObjectReferenceSerializer;
    private readonly ITypedDataSerializer _typedDataSerializer;
    private readonly IHexSerializer _hexSerializer;
    private readonly IVectorSerializer _vectorSerializer;
    private readonly ILogger _logger;

    internal PropertySerializer(IStringSerializer stringSerializer, IObjectReferenceSerializer objectReferenceSerializer, ITypedDataSerializer typedDataSerializer, IHexSerializer hexSerializer, IVectorSerializer vectorSerializer, ISoftObjectReferenceSerializer softObjectReferenceSerializer, ILoggerFactory loggerFactory)
    {
        _stringSerializer = stringSerializer;
        _objectReferenceSerializer = objectReferenceSerializer;
        _typedDataSerializer = typedDataSerializer;
        _hexSerializer = hexSerializer;
        _vectorSerializer = vectorSerializer;
        _softObjectReferenceSerializer = softObjectReferenceSerializer;
        _logger = loggerFactory.CreateLogger<PropertySerializer>() ?? NullLogger<PropertySerializer>.Instance;
    }

    public PropertySerializer(IStringSerializer stringSerializer, IObjectReferenceSerializer objectReferenceSerializer, IVectorSerializer vectorSerializer, IHexSerializer hexSerializer, ISoftObjectReferenceSerializer softObjectReferenceSerializer, ILoggerFactory loggerFactory)
    {
        _stringSerializer = stringSerializer;
        _objectReferenceSerializer = objectReferenceSerializer;
        _typedDataSerializer = new TypedDataSerializer(vectorSerializer, stringSerializer, this, hexSerializer, objectReferenceSerializer);
        _hexSerializer = hexSerializer;
        _vectorSerializer = vectorSerializer;
        _softObjectReferenceSerializer = softObjectReferenceSerializer;
        _logger = loggerFactory.CreateLogger<PropertySerializer>() ?? NullLogger<PropertySerializer>.Instance;
    }

    public IEnumerable<Property> DeserializeProperties(BinaryReader reader, Header? header = null, string? type = null, long? expectedPosition = null, int? saveVersion = null)
    {
        if (saveVersion >= SerializeDataPackageVersionAndCustomVersions && type == null)
        {
            _ = reader.ReadByte();
        }

        if (expectedPosition != null && expectedPosition < reader.BaseStream.Position)
        {
            yield break;
        }

        Property? property;
        while (expectedPosition == null || reader.BaseStream.Position < expectedPosition)
        {
            property = DeserializeProperty(reader, header, type, saveVersion);
            if (property == null)
            {
                break;
            }

            yield return property;

            if (expectedPosition != null && expectedPosition < reader.BaseStream.Position)
            {
                yield break;
            }
        }
    }

    [return: NotNullIfNotNull(nameof(type))]
    public Property? DeserializeProperty(BinaryReader reader, Header? header = null, string? type = null, int? saveVersion = null)
    {
        var propertyName = string.Empty;

        if (type == null)
        {
            propertyName = _stringSerializer.Deserialize(reader);

            if (string.Equals(propertyName, "None", StringComparison.Ordinal) || string.IsNullOrWhiteSpace(propertyName))
                return null;

            if (saveVersion >= SerializeDataPackageVersionAndCustomVersions)
                return DeserializePropertyV12(reader, propertyName);

            type = _stringSerializer.Deserialize(reader);
        }

        if (header != null && saveVersion == null && header.ParseState.UseUe5PropertyFormat(header.SaveVersion))
            return DeserializePropertyUe5(reader, header, type, propertyName);

        Property property = type switch
        {
            nameof(ArrayProperty) => header == null ? throw new ArgumentNullException(nameof(header)) : DeserializeArrayProperty(reader, header),
            nameof(BoolProperty) => DeserializeBoolProperty(reader),
            nameof(ByteProperty) => DeserializeByteProperty(reader),
            nameof(EnumProperty) => DeserializeEnumProperty(reader),
            nameof(FloatProperty) => DeserializeFloatProperty(reader),
            nameof(IntProperty) => DeserializeIntProperty(reader),
            nameof(Int64Property) => DeserializeInt64Property(reader),
            nameof(UInt64Property) => DeserializeUInt64Property(reader),
            nameof(MapProperty) => header == null ? throw new ArgumentNullException(nameof(header)) : DeserializeMapProperty(reader, header, propertyName, type),
            nameof(NameProperty) => DeserializeNameProperty(reader),
            nameof(ObjectProperty) => DeserializeObjectProperty(reader),
            nameof(SoftObjectProperty) => DeserializeSoftObjectProperty(reader),
            nameof(SetProperty) => DeserializeSetProperty(reader, propertyName),
            nameof(StrProperty) => DeserializeStrProperty(reader),
            nameof(StructProperty) => header == null ? throw new ArgumentNullException(nameof(header)) : DeserializeStructProperty(reader, header),
            nameof(TextProperty) => header == null ? throw new ArgumentNullException(nameof(header)) : DeserializeTextProperty(reader, header),
            nameof(UInt32Property) => DeserializeUInt32Property(reader),
            nameof(Int8Property) => DeserializeInt8Property(reader),
            nameof(DoubleProperty) => DeserializeDoubleProperty(reader),
            nameof(FINNetworkProperty) => DeserializeFINNetworkProperty(reader),

            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        property.Name = propertyName;
        return property;
    }

    private ArrayPropertyBase DeserializeArrayProperty(BinaryReader reader, Header header, string type, int count)
    {
        return type switch
        {
            nameof(ByteProperty) => DeserializeArrayByteProperty(reader, count, type),
            nameof(BoolProperty) => DeserializeBoolArrayProperty(reader, count),
            nameof(IntProperty) => DeserializeArrayIntProperty(reader, count),
            nameof(Int64Property) => DeserializeArrayInt64Property(reader, count),
            nameof(UInt64Property) => DeserializeArrayUInt64Property(reader, count),
            nameof(DoubleProperty) => DeserializeArrayDoubleProperty(reader, count),
            nameof(FloatProperty) => DeserializeArrayFloatProperty(reader, count),
            nameof(EnumProperty) => DeserializeArrayEnumProperty(reader, count),
            nameof(StrProperty) => DeserializeArrayStrProperty(reader, count),
            nameof(TextProperty) => DeserializeArrayTextProperty(reader, header, count),
            nameof(SoftObjectProperty) => DeserializeArraySoftObjectProperty(reader, count),
            nameof(ObjectProperty) => DeserializeArrayObjectProperty(reader, count),
            nameof(InterfaceProperty) => DeserializeArrayInterfaceProperty(reader, count),
            nameof(StructProperty) => DeserializeArrayStructProperty(reader, header, count),
            //ToDo: All implemented?

            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    private ArrayTextProperty DeserializeArrayTextProperty(BinaryReader reader, Header header, int count, bool ue5Format = false, bool ue5IncludePropertyGuid = false)
    {
        var values = new TextProperty[count];

        for (var x = 0; x < count; x++)
        {
            values[x] = DeserializeTextProperty(reader, header, ue5Format: ue5Format, ue5IncludePropertyGuid: ue5IncludePropertyGuid);
        }

        return new ArrayTextProperty
        {
            Values = values
        };
    }

    private ArrayStrProperty DeserializeArrayStrProperty(BinaryReader reader, int count)
    {
        var values = new string[count];

        for (var x = 0; x < count; x++)
        {
            values[x] = _stringSerializer.Deserialize(reader);
        }

        return new ArrayStrProperty
        {
            Values = values
        };
    }

    private ArrayEnumProperty DeserializeArrayEnumProperty(BinaryReader reader, int count)
    {
        var values = new string[count];

        for (var x = 0; x < count; x++)
        {
            values[x] = _stringSerializer.Deserialize(reader);
        }

        return new ArrayEnumProperty
        {
            Values = values
        };
    }

    private static ArrayFloatProperty DeserializeArrayFloatProperty(BinaryReader reader, int count)
    {
        var values = new float[count];

        for (var x = 0; x < count; x++)
        {
            values[x] = reader.ReadSingle();
        }

        return new ArrayFloatProperty
        {
            Values = values
        };
    }

    private static ArrayInt64Property DeserializeArrayInt64Property(BinaryReader reader, int count)
    {
        var values = new long[count];

        for (var x = 0; x < count; x++)
        {
            values[x] = reader.ReadInt64();
        }

        return new ArrayInt64Property
        {
            Values = values
        };
    }

    private static ArrayUInt64Property DeserializeArrayUInt64Property(BinaryReader reader, int count)
    {
        var values = new ulong[count];

        for (var x = 0; x < count; x++)
        {
            values[x] = reader.ReadUInt64();
        }

        return new ArrayUInt64Property
        {
            Values = values
        };
    }

    private static ArrayDoubleProperty DeserializeArrayDoubleProperty(BinaryReader reader, int count)
    {
        var values = new double[count];

        for (var x = 0; x < count; x++)
        {
            values[x] = reader.ReadDouble();
        }

        return new ArrayDoubleProperty
        {
            Values = values
        };
    }

    private static ArrayIntProperty DeserializeArrayIntProperty(BinaryReader reader, int count)
    {
        var values = new int[count];

        for (var x = 0; x < count; x++)
        {
            values[x] = reader.ReadInt32();
        }

        return new ArrayIntProperty
        {
            Values = values
        };
    }

    private ArrayInterfaceProperty DeserializeArrayInterfaceProperty(BinaryReader reader, int count)
    {
        var values = new ObjectReference[count];

        for (var x = 0; x < count; x++)
        {
            values[x] = _objectReferenceSerializer.Deserialize(reader);
        }

        return new ArrayInterfaceProperty
        {
            Values = values
        };
    }

    private static ArrayBoolProperty DeserializeBoolArrayProperty(BinaryReader reader, int count)
    {
        var values = new sbyte[count];

        for (var x = 0; x < count; x++)
        {
            values[x] = reader.ReadSByte();
        }

        return new ArrayBoolProperty
        {
            Values = values
        };
    }

    private static ArrayByteProperty DeserializeArrayByteProperty(BinaryReader reader, int count, string propertyName)
    {
        sbyte[] values;

        if (propertyName.Equals("mFogOfWarRawData", StringComparison.Ordinal))
        {
            var newCount = count / 4;
            values = new sbyte[newCount];
            for (var x = 0; x < newCount; x++)
            {
                var unknown1 = reader.ReadSByte();
                var unknown2 = reader.ReadSByte();
                values[x] = reader.ReadSByte();
                var unknown3 = reader.ReadSByte();
            }
        }
        else
        {
            values = new sbyte[count];
            for (var x = 0; x < count; x++)
            {
                values[x] = reader.ReadSByte();
            }
        }

        return new ArrayByteProperty
        {
            Values = values
        };
    }

    private ArrayObjectProperty DeserializeArrayObjectProperty(BinaryReader reader, int count)
    {
        var values = new ObjectReference[count];

        for (var x = 0; x < count; x++)
        {
            values[x] = _objectReferenceSerializer.Deserialize(reader);
        }

        return new ArrayObjectProperty
        {
            Values = values
        };
    }

    private ArraySoftObjectProperty DeserializeArraySoftObjectProperty(BinaryReader reader, int count)
    {
        var values = new SoftObjectReference[count];

        for (var x = 0; x < count; x++)
        {
            values[x] = _softObjectReferenceSerializer.Deserialize(reader);
        }

        return new ArraySoftObjectProperty
        {
            Values = values
        };
    }

    private ArrayProperty DeserializeArrayProperty(BinaryReader reader, Header header)
    {
        var binarySize = reader.ReadInt32();
        var index = reader.ReadInt32();
        var type = _stringSerializer.Deserialize(reader);
        _ = reader.ReadSByte();
        var length = reader.ReadInt32();

        var property = DeserializeArrayProperty(reader, header, type, length);

        return new ArrayProperty
        {
            Index = index,
            Type = type,
            Property = property
        };
    }

    private TextProperty DeserializeTextProperty(BinaryReader reader, Header header, int? binarySize = null, int? index = null, sbyte? padding = null, bool ue5Format = false, bool ue5IncludePropertyGuid = false)
    {
        if (!ue5Format)
        {
            binarySize ??= reader.ReadInt32();
            index ??= reader.ReadInt32();
            padding ??= reader.ReadSByte();
        }
        else if (ue5IncludePropertyGuid)
        {
            ReadUe5PropertyGuid(reader);
        }
        var flags = reader.ReadInt32();
        var historyType = reader.ReadByte();

        string? nameSpace = null;
        string? key = null;
        string? value = null;
        TextProperty? sourceFmt = null;
        TextProperty? sourceText = null;
        byte? transformType = null;
        string? tableId = null;
        string? textKey = null;
        int? isCultureInvariant = null;
        TextArgument[]? arguments = null;

        switch (historyType)
        {
            case 0:
                nameSpace = _stringSerializer.Deserialize(reader);
                key = _stringSerializer.Deserialize(reader);
                value = _stringSerializer.Deserialize(reader);
                break;
            case 1:
            case 2:
            case 3:
                sourceFmt = DeserializeTextProperty(reader, header, binarySize, index, padding, ue5Format: true);
                var argumentsCount = reader.ReadInt32();
                arguments = new TextArgument[argumentsCount];

                for (var x = 0; x < argumentsCount; x++)
                {
                    string? name = null;
                    if (historyType != 2)
                        name = _stringSerializer.Deserialize(reader);
                    var valueType = reader.ReadByte();
                    TextArgument textArgument = valueType switch
                    {
                        0 => new TextArgumentV0 { Name = name, ArgumentValue = reader.ReadInt32(), ArgumentValueUnknown = reader.ReadInt32() },
                        4 => new TextArgumentV4 { Name = name, ArgumentPropertyValue = DeserializeTextProperty(reader, header, ue5Format: true) },
                        _ => throw new InvalidDataException("Unknown valueType type in array text property"),
                    };
                    arguments[x] = textArgument;
                }
                break;
            case 10:
                sourceText = DeserializeTextProperty(reader, header, ue5Format: true);
                transformType = reader.ReadByte();
                break;
            case 11:
                tableId = _stringSerializer.Deserialize(reader);
                textKey = _stringSerializer.Deserialize(reader);
                break;
            case 255:
                if (header.BuildVersion < 140822)
                    break;
                isCultureInvariant = reader.ReadInt32();
                if (isCultureInvariant != 1)
                    break;
                value = _stringSerializer.Deserialize(reader);
                break;
            default:
                throw new InvalidDataException("Unknown history type in array text property");
        }

        return new TextProperty
        {
            Index = index ?? 0,
            Flags = flags,
            HistoryType = historyType,
            IsCultureInvariant = isCultureInvariant,
            Value = value,
            NameSpace = nameSpace,
            Key = key,
            SourceFmt = sourceFmt,
            SourceText = sourceText,
            TransformType = transformType,
            TableId = tableId,
            TextKey = textKey,
            Arguments = arguments
        };
    }

    private ArrayStructProperty DeserializeArrayStructProperty(BinaryReader reader, Header header, int length)
    {
        var propertyName = _stringSerializer.Deserialize(reader);
        var propertyType = _stringSerializer.Deserialize(reader);

        var binarySize = reader.ReadInt32();
        _ = reader.ReadInt32();
        var elementType = _stringSerializer.Deserialize(reader);

        var uuid1 = reader.ReadInt32();
        var uuid2 = reader.ReadInt32();
        var uuid3 = reader.ReadInt32();
        var uuid4 = reader.ReadInt32();
        _ = reader.ReadSByte();

        var values = new TypedData[length];

        for (var x = 0; x < length; x++)
        {
            values[x] = _typedDataSerializer.Deserialize(reader, header, elementType, true, binarySize);
        }

        var property = new ArrayStructProperty
        {
            PropertyName = propertyName,
            PropertyType = propertyType,
            ElementType = elementType,
            UUID = (uuid1, uuid2, uuid3, uuid4),
            Values = values
        };

        return property;
    }

    private static BoolProperty DeserializeBoolProperty(BinaryReader reader)
    {
        _ = reader.ReadInt32();
        var index = reader.ReadInt32();
        var value = reader.ReadSByte();
        _ = reader.ReadSByte();

        var property = new BoolProperty
        {
            Index = index,
            Value = value
        };

        return property;
    }

    private ByteProperty DeserializeByteProperty(BinaryReader reader)
    {
        var binarySize = reader.ReadInt32();
        var index = reader.ReadInt32();
        var type = _stringSerializer.Deserialize(reader);
        _ = reader.ReadSByte();

        sbyte? byteData = null;
        string? stringData = null;

        if (string.Equals(type, "None", StringComparison.Ordinal))
            byteData = reader.ReadSByte();
        else
            stringData = _stringSerializer.Deserialize(reader);

        var property = new ByteProperty
        {
            Index = index,
            Type = type,
            ByteData = byteData,
            StringData = stringData
        };

        return property;
    }

    private EnumProperty DeserializeEnumProperty(BinaryReader reader)
    {
        var binarySize = reader.ReadInt32();
        var index = reader.ReadInt32();
        var type = _stringSerializer.Deserialize(reader);
        _ = reader.ReadSByte();
        var value = _stringSerializer.Deserialize(reader);

        var property = new EnumProperty
        {
            Index = index,
            Type = type,
            Value = value
        };

        return property;
    }

    private static FloatProperty DeserializeFloatProperty(BinaryReader reader)
    {
        var binarySize = reader.ReadInt32();
        var index = reader.ReadInt32();
        _ = reader.ReadSByte();
        var value = reader.ReadSingle();

        var property = new FloatProperty
        {
            Index = index,
            Value = value
        };

        return property;
    }

    private static DoubleProperty DeserializeDoubleProperty(BinaryReader reader)
    {
        var binarySize = reader.ReadInt32();
        var index = reader.ReadInt32();
        _ = reader.ReadSByte();
        var value = reader.ReadDouble();

        var property = new DoubleProperty
        {
            Index = index,
            Value = value
        };

        return property;
    }

    private static IntProperty DeserializeIntProperty(BinaryReader reader)
    {
        var binarySize = reader.ReadInt32();
        var index = reader.ReadInt32();
        _ = reader.ReadSByte();
        var value = reader.ReadInt32();

        var property = new IntProperty
        {
            Index = index,
            Value = value
        };

        return property;
    }

    private static Int8Property DeserializeInt8Property(BinaryReader reader)
    {
        var binarySize = reader.ReadInt32();
        var index = reader.ReadInt32();
        _ = reader.ReadSByte();
        var value = reader.ReadSByte();

        var property = new Int8Property
        {
            Index = index,
            Value = value
        };

        return property;
    }

    private static UInt32Property DeserializeUInt32Property(BinaryReader reader)
    {
        var binarySize = reader.ReadInt32();
        var index = reader.ReadInt32();
        _ = reader.ReadSByte();
        var value = reader.ReadUInt32();

        var property = new UInt32Property
        {
            Index = index,
            Value = value
        };

        return property;
    }

    private static Int64Property DeserializeInt64Property(BinaryReader reader)
    {
        var binarySize = reader.ReadInt32();
        var index = reader.ReadInt32();
        _ = reader.ReadSByte();
        var value = reader.ReadInt64();

        var property = new Int64Property
        {
            Index = index,
            Value = value
        };

        return property;
    }

    private static UInt64Property DeserializeUInt64Property(BinaryReader reader)
    {
        var binarySize = reader.ReadInt32();
        var index = reader.ReadInt32();
        _ = reader.ReadSByte();
        var value = reader.ReadUInt64();

        var property = new UInt64Property
        {
            Index = index,
            Value = value
        };

        return property;
    }

    private MapProperty DeserializeMapProperty(BinaryReader reader, Header header, string? propertyName = null, string? type = null)
    {
        var binarySize = reader.ReadInt32();
        var index = reader.ReadInt32();
        var keyType = _stringSerializer.Deserialize(reader);
        var valueType = _stringSerializer.Deserialize(reader);
        var padding = reader.ReadSByte();
        var modeType = reader.ReadInt32();

        if (modeType == 2)
        {
            var disc1 = _stringSerializer.Deserialize(reader);
            var disc2 = _stringSerializer.Deserialize(reader);
        }
        else if (modeType == 3)
        {
            var disc1 = _hexSerializer.Deserialize(reader, 9);
            var disc2 = _stringSerializer.Deserialize(reader);
            var disc3 = _stringSerializer.Deserialize(reader);
        }

        var count = reader.ReadInt32();

        var property = new MapProperty
        {
            Index = index,
            KeyType = keyType,
            ValueType = valueType,
            ModeType = modeType,
            Elements = new Dictionary<UnionBase, UnionBase?>(count)
        };

        UnionBase key;
        UnionBase? value;

        for (var i = 0; i < count; i++)
        {
            switch (property.KeyType)
            {
                case nameof(IntProperty):
                    key = new IntUnion { Value = reader.ReadInt32() };
                    break;
                case nameof(Int64Property):
                    key = new Int64Union { Value = reader.ReadInt64() };
                    break;
                case nameof(NameProperty):
                    key = new NameUnion { Value = _stringSerializer.Deserialize(reader) };
                    break;
                case nameof(StrProperty):
                    key = new StrUnion { Value = _stringSerializer.Deserialize(reader) };
                    break;
                case nameof(ObjectProperty):
                    key = new ObjectReferenceUnion { Value = _objectReferenceSerializer.Deserialize(reader) };
                    break;
                case nameof(EnumProperty):
                    key = new EnumUnion { Value = _stringSerializer.Deserialize(reader) };
                    break;
                case nameof(StructProperty):
                    if (string.Equals(propertyName, "Destroyed_Foliage_Transform", StringComparison.Ordinal))
                    {
                        key = header.SaveVersion >= 41
                            ? new Vector3DUnion { Value = _vectorSerializer.DeserializeVec3D(reader) }
                            : new Vector3Union { Value = _vectorSerializer.DeserializeVec3(reader) };
                        break;
                    }
                    if (string.Equals(type, "/BuildGunUtilities/BGU_Subsystem.BGU_Subsystem_C", StringComparison.Ordinal)
                        || string.Equals(type, "/Script/NoImpure.NoImpureSubsystem", StringComparison.Ordinal))
                    {
                        key = new Vector3Union { Value = _vectorSerializer.DeserializeVec3(reader) };
                        break;
                    }
                    if (string.Equals(propertyName, "mSaveData", StringComparison.Ordinal) || string.Equals(propertyName, "mUnresolvedSaveData", StringComparison.Ordinal))
                    {
                        key = new Vector3IUnion { Value = _vectorSerializer.DeserializeVec3I(reader) };
                        break;
                    }
                    key = new PropertiesUnion { Value = [.. DeserializeProperties(reader, header)] };
                    break;
                default:
                    throw new InvalidDataException("Unknown dictionary property key type");
            }
            switch (property.ValueType)
            {
                case nameof(ByteProperty):
                    value = new ByteUnion
                    {
                        Value =
                                (property.KeyType == nameof(StrProperty) || string.Equals(type, "/Script/NoImpure.NoImpureSubsystem", StringComparison.Ordinal))
                                ? Convert.ToSByte(_stringSerializer.Deserialize(reader))
                                : reader.ReadSByte()
                    };
                    break;
                case nameof(BoolProperty):
                    value = new BoolUnion { Value = reader.ReadSByte() };
                    break;
                case nameof(IntProperty):
                    value = new IntUnion { Value = reader.ReadInt32() };
                    break;
                case nameof(Int64Property):
                    value = new Int64Union { Value = reader.ReadInt64() };
                    break;
                case nameof(DoubleProperty):
                    value = new DoubleUnion { Value = reader.ReadDouble() };
                    break;
                case nameof(FloatProperty):
                    value = new FloatUnion { Value = reader.ReadSingle() };
                    break;
                case nameof(StrProperty):
                    value = string.Equals(type, "/BuildGunUtilities/BGU_Subsystem.BGU_Subsystem_C", StringComparison.Ordinal)
                        ? new StrUnion { Unknown1 = reader.ReadSingle(), Unknown2 = reader.ReadSingle(), Unknown3 = reader.ReadSingle(), Value = _stringSerializer.Deserialize(reader) }
                        : null;
                    break;
                case nameof(ObjectProperty):
                    value = string.Equals(type, "/BuildGunUtilities/BGU_Subsystem.BGU_Subsystem_C", StringComparison.Ordinal)
                        ? new ObjectReferenceUnion { Unknown1 = reader.ReadSingle(), Unknown2 = reader.ReadSingle(), Unknown3 = reader.ReadSingle(), Unknown4 = reader.ReadSingle(), Unknown5 = _stringSerializer.Deserialize(reader) }
                        : new ObjectReferenceUnion { Value = _objectReferenceSerializer.Deserialize(reader) };
                    break;
                case nameof(TextProperty):
                    value = new TextUnion { Value = DeserializeTextProperty(reader, header) };
                    break;
                case nameof(StructProperty):
                    if (string.Equals(type, "LBBalancerData", StringComparison.Ordinal))
                    {
                        value = new LBBalancerUnion { NormalIndex = reader.ReadInt32(), OverflowIndex = reader.ReadInt32(), FilterIndex = reader.ReadInt32() };
                        break;
                    }
                    if (string.Equals(type, "/StorageStatsRoom/Sub_SR.Sub_SR_C", StringComparison.Ordinal) || string.Equals(type, "/CentralStorage/Subsystem_SC.Subsystem_SC_C", StringComparison.Ordinal))
                    {
                        value = header.SaveVersion >= 41
                            ? new StorageDoubleUnion { Unknown1 = reader.ReadDouble(), Unknown2 = reader.ReadDouble(), Unknown3 = reader.ReadDouble() }
                            : new StorageSingleUnion { Unknown1 = reader.ReadSingle(), Unknown2 = reader.ReadSingle(), Unknown3 = reader.ReadSingle() };
                        break;
                    }
                    value = new PropertiesUnion { Value = [.. DeserializeProperties(reader, header)] };
                    break;
                default:
                    throw new InvalidDataException("Unknown dictionary property value type");
            }

            property.Elements.Add(key, value);
        }

        return property;
    }

    private NameProperty DeserializeNameProperty(BinaryReader reader)
    {
        var binarySize = reader.ReadInt32();
        var index = reader.ReadInt32();
        _ = reader.ReadSByte();
        var value = _stringSerializer.Deserialize(reader);

        var property = new NameProperty
        {
            Index = index,
            Value = value
        };

        return property;
    }

    private ObjectProperty DeserializeObjectProperty(BinaryReader reader)
    {
        var binarySize = reader.ReadInt32();
        var index = reader.ReadInt32();
        _ = reader.ReadSByte();
        var value = _objectReferenceSerializer.Deserialize(reader);

        var property = new ObjectProperty
        {
            Index = index,
            Value = value
        };

        return property;
    }

    private SoftObjectProperty DeserializeSoftObjectProperty(BinaryReader reader)
    {
        var binarySize = reader.ReadInt32();
        var index = reader.ReadInt32();
        _ = reader.ReadSByte();
        var value = _softObjectReferenceSerializer.Deserialize(reader);

        var property = new SoftObjectProperty
        {
            Index = index,
            Value = value,
        };

        return property;
    }

    private SetProperty DeserializeSetProperty(BinaryReader reader, string propertyName)
    {
        var binarySize = reader.ReadInt32();
        var index = reader.ReadInt32();
        var type = _stringSerializer.Deserialize(reader);
        _ = reader.ReadSByte();
        _ = reader.ReadInt32();
        var count = reader.ReadInt32();

        var property = new SetProperty
        {
            Index = index,
            Type = type,
            Elements = DeserializeUnions(reader, propertyName, count, type).ToArray()
        };

        return property;
    }

    private IEnumerable<UnionBase> DeserializeUnions(BinaryReader reader, string propertyName, int count, string type)
    {
        for (var i = 0; i < count; i++)
        {
            UnionBase value;
            switch (type)
            {
                case nameof(ObjectProperty):
                    value = new ObjectReferenceUnion { Value = _objectReferenceSerializer.Deserialize(reader) };
                    break;
                case nameof(StructProperty):
                    if (propertyName.Equals("/Script/FactoryGame.FGFoliageRemoval", StringComparison.Ordinal)
                        || propertyName.Equals("mRemovalLocations", StringComparison.Ordinal))
                    {
                        value = new Vector3Union { Value = _vectorSerializer.DeserializeVec3(reader) };
                        break;
                    }
                    if (propertyName.Equals("mDestroyedPickups", StringComparison.Ordinal)
                        || propertyName.Equals("mLootedDropPods", StringComparison.Ordinal)
                        || propertyName.Equals("/Script/FactoryGame.FGScannableSubsystem", StringComparison.Ordinal))
                    {
                        value = new GuidUnion { Value = new System.Guid(reader.ReadBytes(16)) };
                        break;
                    }
                    value = new FINNetworkUnion { Value = DeserializeFINNetworkProperty(reader) };
                    break;
                case nameof(NameProperty):
                    value = new NameUnion { Value = _stringSerializer.Deserialize(reader) };
                    break;
                case nameof(StrProperty):
                    value = new StrUnion { Value = _stringSerializer.Deserialize(reader) };
                    break;
                case nameof(IntProperty):
                    value = new IntUnion { Value = reader.ReadInt32() };
                    break;
                case nameof(UInt32Property):
                    value = new UInt32Union { Value = reader.ReadUInt32() };
                    break;
                default:
                    throw new InvalidDataException("Unknown set property value type");
            }

            yield return value;
        }
    }

    private FINNetworkProperty DeserializeFINNetworkProperty(BinaryReader reader)
    {
        var objectReference = _objectReferenceSerializer.Deserialize(reader);
        FINNetworkProperty? previous = null;
        string? step = null;
        if (reader.ReadInt32() == 1)
            previous = DeserializeFINNetworkProperty(reader);
        if (reader.ReadInt32() == 1)
            step = _stringSerializer.Deserialize(reader);

        return new FINNetworkProperty { ObjectReference = objectReference, Previous = previous, Step = step };
    }

    private StrProperty DeserializeStrProperty(BinaryReader reader)
    {
        var binarySize = reader.ReadInt32();
        var index = reader.ReadInt32();
        _ = reader.ReadSByte();
        var value = _stringSerializer.Deserialize(reader);

        var property = new StrProperty
        {
            Index = index,
            Value = value
        };

        return property;
    }

    private StructProperty DeserializeStructProperty(BinaryReader reader, Header header)
    {
        var binarySize = reader.ReadInt32();
        //var positionStart = reader.BaseStream.Position;
        var index = reader.ReadInt32();
        var type = _stringSerializer.Deserialize(reader);
        _ = reader.ReadInt64();
        _ = reader.ReadInt64();
        _ = reader.ReadSByte();
        var typedData = _typedDataSerializer.Deserialize(reader, header, type, false, binarySize);

        var property = new StructProperty
        {
            Index = index,
            Type = type,
            Value = typedData
        };

        //var expectedPosition = positionStart + binarySize;
        //if (expectedPosition != reader.BaseStream.Position)
        //{
        //    var hex = _hexSerializer.Deserialize(reader, (expectedPosition - reader.BaseStream.Position).ToInt());
        //    // throw new BadReadException("Expected stream position does not match actual position");
        //}

        return property;
    }

    private static void ReadUe5PropertyGuid(BinaryReader reader)
    {
        if (reader.ReadByte() != 1)
            return;

        _ = reader.ReadUInt32();
        _ = reader.ReadUInt32();
        _ = reader.ReadUInt32();
        _ = reader.ReadUInt32();
    }

    private sealed class PropertyReadMetadata
    {
        public string? ArrayElementType { get; set; }
        public string? ArrayStructSubType { get; set; }
        public string? StructType { get; set; }
        public string? ByteEnumName { get; set; }
        public string? EnumTypeName { get; set; }
        public string? MapKeyType { get; set; }
        public string? MapValueType { get; set; }
        public string? SetElementType { get; set; }
    }

    private static string NormalizePropertyTypeName(string type) =>
        type.EndsWith("Property", StringComparison.Ordinal) ? type : type + "Property";

    private static string NormalizeArrayElementTypeName(string? type) =>
        string.IsNullOrEmpty(type) ? nameof(IntProperty) : NormalizePropertyTypeName(type);

    private static bool IsStructArrayElement(string? type) =>
        string.Equals(StripPropertySuffix(type), "Struct", StringComparison.Ordinal);

    private static string StripPropertySuffix(string? type) =>
        string.IsNullOrEmpty(type) ? string.Empty
        : type.EndsWith("Property", StringComparison.Ordinal) ? type[..^8] : type;

    private Property? DeserializePropertyUe5(BinaryReader reader, Header header, string type, string propertyName)
    {
        type = NormalizePropertyTypeName(type);
        var metadata = ReadUe5CustomPropertyMetadata(reader, header, type);
        var propertyLength = reader.ReadInt32();
        var valueStart = reader.BaseStream.Position;

        Property property = type switch
        {
            nameof(ArrayProperty) => DeserializeArrayPropertyUe5(reader, header, metadata),
            nameof(BoolProperty) => DeserializeBoolPropertyUe5(reader),
            nameof(ByteProperty) => DeserializeBytePropertyUe5(reader, metadata?.ByteEnumName, header),
            nameof(EnumProperty) => DeserializeEnumPropertyUe5(reader, metadata?.EnumTypeName),
            nameof(FloatProperty) => DeserializeFloatPropertyUe5(reader),
            nameof(IntProperty) => DeserializeIntPropertyUe5(reader),
            nameof(Int64Property) => DeserializeInt64PropertyUe5(reader),
            nameof(UInt64Property) => DeserializeUInt64PropertyUe5(reader),
            nameof(MapProperty) => DeserializeMapPropertyUe5(reader, header, propertyName, type, metadata),
            nameof(NameProperty) => DeserializeNamePropertyUe5(reader),
            nameof(ObjectProperty) => DeserializeObjectPropertyUe5(reader),
            nameof(InterfaceProperty) => DeserializeObjectPropertyUe5(reader),
            nameof(SoftObjectProperty) => DeserializeSoftObjectPropertyUe5(reader),
            nameof(SetProperty) => DeserializeSetPropertyUe5(reader, propertyName, metadata?.SetElementType),
            nameof(StrProperty) => DeserializeStrPropertyUe5(reader),
            nameof(StructProperty) => DeserializeStructPropertyUe5(reader, header, metadata?.StructType, propertyLength),
            nameof(TextProperty) => DeserializeTextProperty(reader, header, ue5Format: true, ue5IncludePropertyGuid: true),
            nameof(UInt32Property) => DeserializeUInt32PropertyUe5(reader),
            nameof(Int8Property) => DeserializeInt8PropertyUe5(reader),
            nameof(DoubleProperty) => DeserializeDoublePropertyUe5(reader),
            nameof(FINNetworkProperty) => DeserializeFINNetworkProperty(reader),
            "Persistent_LevelProperty" => DeserializeUnsupportedUe5Property(reader, header, propertyLength, propertyName, type),
            _ => DeserializeUnsupportedUe5Property(reader, header, propertyLength, propertyName, type)
        };

        property.Name = propertyName;

        var expectedEnd = valueStart + propertyLength;
        var diff = expectedEnd - reader.BaseStream.Position;
        if (diff > 0)
            reader.BaseStream.Seek(diff, SeekOrigin.Current);
        else if (diff < 0)
            reader.BaseStream.Seek(diff, SeekOrigin.Current);

        return property;
    }

    private static Property DeserializeUnsupportedUe5Property(BinaryReader reader, Header header, int propertyLength, string propertyName, string type) =>
        new StrProperty { Name = propertyName, Value = string.Empty };

    private PropertyReadMetadata ReadUe5CustomPropertyMetadata(BinaryReader reader, Header header, string type)
    {
        var metadata = new PropertyReadMetadata();
        var hasCustomData = reader.ReadInt32();

        switch (hasCustomData)
        {
            case 0:
                break;
            case 1:
                if (type == nameof(ArrayProperty))
                {
                    metadata.ArrayElementType = _stringSerializer.Deserialize(reader);
                    var arrayMode = reader.ReadInt32();
                    if (arrayMode == 1)
                    {
                        _ = _stringSerializer.Deserialize(reader);
                        UnrealFormatHelper.ReadPackageName(reader, _stringSerializer);
                    }
                    else if (arrayMode == 2)
                    {
                        _ = _stringSerializer.Deserialize(reader);
                        UnrealFormatHelper.ReadPackageName(reader, _stringSerializer);
                        metadata.ArrayStructSubType = _stringSerializer.Deserialize(reader);
                        _ = reader.ReadInt32();
                    }
                }
                else if (type == nameof(ByteProperty))
                {
                    metadata.ByteEnumName = _stringSerializer.Deserialize(reader);
                    UnrealFormatHelper.ReadPackageName(reader, _stringSerializer);
                }
                else if (type == nameof(SetProperty))
                {
                    metadata.SetElementType = _stringSerializer.Deserialize(reader);
                    UnrealFormatHelper.ReadPackageName(reader, _stringSerializer);
                }
                else if (type == nameof(StructProperty))
                {
                    metadata.StructType = _stringSerializer.Deserialize(reader);
                    UnrealFormatHelper.ReadPackageName(reader, _stringSerializer);
                }
                break;
            case 2:
                if (type == nameof(EnumProperty))
                {
                    metadata.EnumTypeName = _stringSerializer.Deserialize(reader);
                    UnrealFormatHelper.ReadPackageName(reader, _stringSerializer);
                    _ = _stringSerializer.Deserialize(reader);
                    _ = reader.ReadInt32();
                }
                else if (type == nameof(MapProperty))
                {
                    metadata.MapKeyType = _stringSerializer.Deserialize(reader);
                    UnrealFormatHelper.ReadPackageName(reader, _stringSerializer);
                    if (metadata.MapKeyType == nameof(EnumProperty))
                    {
                        _ = _stringSerializer.Deserialize(reader);
                        _ = reader.ReadInt32();
                    }
                    metadata.MapValueType = _stringSerializer.Deserialize(reader);
                    UnrealFormatHelper.ReadPackageName(reader, _stringSerializer);
                }
                break;
        }

        return metadata;
    }

    private ArrayProperty DeserializeArrayPropertyUe5(BinaryReader reader, Header header, PropertyReadMetadata? metadata)
    {
        var elementType = metadata?.ArrayElementType;
        if (IsStructArrayElement(elementType))
            elementType = metadata?.ArrayStructSubType ?? elementType;

        var normalizedType = NormalizeArrayElementTypeName(elementType);
        _ = reader.ReadSByte();
        var length = reader.ReadInt32();
        ArrayPropertyBase property = normalizedType == nameof(StructProperty)
            ? DeserializeArrayStructPropertyUe5(reader, header, length, elementType ?? nameof(StructProperty))
            : DeserializeArrayProperty(reader, header, normalizedType, length);

        return new ArrayProperty { Type = normalizedType, Property = property };
    }

    private ArrayStructProperty DeserializeArrayStructPropertyUe5(BinaryReader reader, Header header, int length, string elementType)
    {
        _ = reader.ReadSByte();

        var values = new TypedData[length];
        for (var x = 0; x < length; x++)
            values[x] = _typedDataSerializer.Deserialize(reader, header, elementType, true, 0);

        return new ArrayStructProperty { ElementType = elementType, Values = values };
    }

    private static BoolProperty DeserializeBoolPropertyUe5(BinaryReader reader)
    {
        var value = reader.ReadSByte();
        if (value == 16)
            value = 1;

        return new BoolProperty { Value = value };
    }

    private ByteProperty DeserializeBytePropertyUe5(BinaryReader reader, string? enumName, Header header)
    {
        _ = reader.ReadByte();

        if (!string.IsNullOrEmpty(enumName))
        {
            return new ByteProperty
            {
                Type = enumName,
                StringData = _stringSerializer.Deserialize(reader)
            };
        }

        return new ByteProperty { Type = "None", ByteData = reader.ReadSByte() };
    }

    private EnumProperty DeserializeEnumPropertyUe5(BinaryReader reader, string? typeName)
    {
        _ = reader.ReadByte();
        return new EnumProperty
        {
            Type = typeName ?? string.Empty,
            Value = _stringSerializer.Deserialize(reader)
        };
    }

    private static FloatProperty DeserializeFloatPropertyUe5(BinaryReader reader)
    {
        _ = reader.ReadSByte();
        return new FloatProperty { Value = reader.ReadSingle() };
    }

    private static DoubleProperty DeserializeDoublePropertyUe5(BinaryReader reader)
    {
        _ = reader.ReadSByte();
        return new DoubleProperty { Value = reader.ReadDouble() };
    }

    private static IntProperty DeserializeIntPropertyUe5(BinaryReader reader)
    {
        _ = reader.ReadSByte();
        return new IntProperty { Value = reader.ReadInt32() };
    }

    private static Int8Property DeserializeInt8PropertyUe5(BinaryReader reader)
    {
        _ = reader.ReadSByte();
        return new Int8Property { Value = reader.ReadSByte() };
    }

    private static UInt32Property DeserializeUInt32PropertyUe5(BinaryReader reader)
    {
        _ = reader.ReadSByte();
        return new UInt32Property { Value = reader.ReadUInt32() };
    }

    private static Int64Property DeserializeInt64PropertyUe5(BinaryReader reader)
    {
        _ = reader.ReadSByte();
        return new Int64Property { Value = reader.ReadInt64() };
    }

    private static UInt64Property DeserializeUInt64PropertyUe5(BinaryReader reader)
    {
        _ = reader.ReadSByte();
        return new UInt64Property { Value = reader.ReadUInt64() };
    }

    private NameProperty DeserializeNamePropertyUe5(BinaryReader reader)
    {
        _ = reader.ReadSByte();
        return new NameProperty { Value = _stringSerializer.Deserialize(reader) };
    }

    private ObjectProperty DeserializeObjectPropertyUe5(BinaryReader reader)
    {
        _ = reader.ReadSByte();
        return new ObjectProperty { Value = _objectReferenceSerializer.Deserialize(reader) };
    }

    private SoftObjectProperty DeserializeSoftObjectPropertyUe5(BinaryReader reader)
    {
        _ = reader.ReadSByte();
        return new SoftObjectProperty { Value = _softObjectReferenceSerializer.Deserialize(reader) };
    }

    private StrProperty DeserializeStrPropertyUe5(BinaryReader reader)
    {
        _ = reader.ReadSByte();
        return new StrProperty { Value = _stringSerializer.Deserialize(reader) };
    }

    private SetProperty DeserializeSetPropertyUe5(BinaryReader reader, string propertyName, string? elementType)
    {
        elementType ??= nameof(ObjectProperty);
        _ = reader.ReadSByte();
        _ = UnrealFormatHelper.ReadModeType(reader, _stringSerializer, _hexSerializer);
        var count = reader.ReadInt32();

        return new SetProperty
        {
            Type = elementType,
            Elements = DeserializeUnions(reader, propertyName, count, elementType).ToArray()
        };
    }

    private MapProperty DeserializeMapPropertyUe5(BinaryReader reader, Header header, string? propertyName, string? actorType, PropertyReadMetadata? metadata)
    {
        var keyType = metadata?.MapKeyType ?? nameof(IntProperty);
        var valueType = metadata?.MapValueType ?? nameof(IntProperty);

        _ = reader.ReadSByte();
        var modeType = UnrealFormatHelper.ReadModeType(reader, _stringSerializer, _hexSerializer);
        var count = reader.ReadInt32();

        var property = new MapProperty
        {
            KeyType = keyType,
            ValueType = valueType,
            ModeType = modeType,
            Elements = new Dictionary<UnionBase, UnionBase?>(count)
        };

        for (var i = 0; i < count; i++)
        {
            UnionBase key = property.KeyType switch
            {
                nameof(IntProperty) => new IntUnion { Value = reader.ReadInt32() },
                nameof(Int64Property) => new Int64Union { Value = reader.ReadInt64() },
                nameof(NameProperty) => new NameUnion { Value = _stringSerializer.Deserialize(reader) },
                nameof(StrProperty) => new StrUnion { Value = _stringSerializer.Deserialize(reader) },
                nameof(ObjectProperty) => new ObjectReferenceUnion { Value = _objectReferenceSerializer.Deserialize(reader) },
                nameof(EnumProperty) => new EnumUnion { Value = _stringSerializer.Deserialize(reader) },
                nameof(StructProperty) when string.Equals(propertyName, "Destroyed_Foliage_Transform", StringComparison.Ordinal) =>
                    new Vector3DUnion { Value = _vectorSerializer.DeserializeVec3D(reader) },
                nameof(StructProperty) when string.Equals(actorType, "/BuildGunUtilities/BGU_Subsystem.BGU_Subsystem_C", StringComparison.Ordinal)
                    || string.Equals(actorType, "/Script/NoImpure.NoImpureSubsystem", StringComparison.Ordinal) =>
                    new Vector3Union { Value = _vectorSerializer.DeserializeVec3(reader) },
                nameof(StructProperty) when string.Equals(propertyName, "mSaveData", StringComparison.Ordinal)
                    || string.Equals(propertyName, "mUnresolvedSaveData", StringComparison.Ordinal) =>
                    new Vector3IUnion { Value = _vectorSerializer.DeserializeVec3I(reader) },
                nameof(StructProperty) => new PropertiesUnion { Value = [.. DeserializeProperties(reader, header)] },
                _ => throw new InvalidDataException("Unknown dictionary property key type")
            };

            UnionBase? value = property.ValueType switch
            {
                nameof(ByteProperty) => new ByteUnion
                {
                    Value = property.KeyType == nameof(StrProperty) || string.Equals(actorType, "/Script/NoImpure.NoImpureSubsystem", StringComparison.Ordinal)
                        ? Convert.ToSByte(_stringSerializer.Deserialize(reader))
                        : reader.ReadSByte()
                },
                nameof(BoolProperty) => new BoolUnion { Value = reader.ReadSByte() },
                nameof(IntProperty) => new IntUnion { Value = reader.ReadInt32() },
                nameof(Int64Property) => new Int64Union { Value = reader.ReadInt64() },
                nameof(DoubleProperty) => new DoubleUnion { Value = reader.ReadDouble() },
                nameof(FloatProperty) => new FloatUnion { Value = reader.ReadSingle() },
                "IntVector" => new Vector3IUnion { Value = _vectorSerializer.DeserializeVec3I(reader) },
                nameof(StrProperty) => string.Equals(actorType, "/BuildGunUtilities/BGU_Subsystem.BGU_Subsystem_C", StringComparison.Ordinal)
                    ? new StrUnion { Unknown1 = reader.ReadSingle(), Unknown2 = reader.ReadSingle(), Unknown3 = reader.ReadSingle(), Value = _stringSerializer.Deserialize(reader) }
                    : new StrUnion { Value = _stringSerializer.Deserialize(reader) },
                nameof(ObjectProperty) => string.Equals(actorType, "/BuildGunUtilities/BGU_Subsystem.BGU_Subsystem_C", StringComparison.Ordinal)
                    ? new ObjectReferenceUnion { Unknown1 = reader.ReadSingle(), Unknown2 = reader.ReadSingle(), Unknown3 = reader.ReadSingle(), Unknown4 = reader.ReadSingle(), Unknown5 = _stringSerializer.Deserialize(reader) }
                    : new ObjectReferenceUnion { Value = _objectReferenceSerializer.Deserialize(reader) },
                nameof(TextProperty) => new TextUnion { Value = DeserializeTextProperty(reader, header) },
                nameof(StructProperty) when string.Equals(actorType, "LBBalancerData", StringComparison.Ordinal) =>
                    new LBBalancerUnion { NormalIndex = reader.ReadInt32(), OverflowIndex = reader.ReadInt32(), FilterIndex = reader.ReadInt32() },
                nameof(StructProperty) => new PropertiesUnion { Value = [.. DeserializeProperties(reader, header)] },
                _ => throw new InvalidDataException("Unknown dictionary property value type")
            };

            property.Elements.Add(key, value);
        }

        return property;
    }

    private StructProperty DeserializeStructPropertyUe5(BinaryReader reader, Header header, string? structType, int propertyLength)
    {
        structType ??= string.Empty;

        var hasIndex = reader.ReadByte();
        if (hasIndex == 9)
            _ = reader.ReadInt32();

        var typedData = _typedDataSerializer.Deserialize(reader, header, structType, false, propertyLength);

        return new StructProperty { Type = structType, Value = typedData };
    }

    private FPropertyTagNode ReadPropertyTagNode(BinaryReader reader)
    {
        var name = _stringSerializer.Deserialize(reader);
        var count = reader.ReadInt32();
        var children = new FPropertyTagNode[count];
        for (var i = 0; i < count; i++)
            children[i] = ReadPropertyTagNode(reader);
        return new FPropertyTagNode { Name = name, Children = children };
    }

    private Property? DeserializePropertyV12(BinaryReader reader, string propertyName)
    {
        var tagNode = ReadPropertyTagNode(reader);
        var binarySize = reader.ReadInt32();
        var flags = reader.ReadByte();
        var index = (flags & 0x1) != 0 ? reader.ReadInt32() : 0;
        System.Guid? propertyGuid = null;
        if ((flags & 0x2) != 0)
            propertyGuid = new System.Guid(reader.ReadBytes(16));

        var posBeforeValue = reader.BaseStream.Position;
        var raw = new RawProperty
        {
            Name = propertyName,
            Index = index,
            Type = tagNode.Name,
            TypeNode = tagNode,
            BinarySize = binarySize,
            Flags = flags,
            PropertyGuid = propertyGuid
        };

        if (binarySize > 0)
            TryParseKnownValue(reader, raw, binarySize);

        var expected = posBeforeValue + binarySize;
        var diff = expected - reader.BaseStream.Position;
        if (diff != 0)
            reader.BaseStream.Seek(diff, SeekOrigin.Current);

        return raw;
    }

    private void TryParseKnownValue(BinaryReader reader, RawProperty raw, int binarySize)
    {
        switch (raw.Type)
        {
            case "IntProperty":
                if (binarySize >= 4) raw.IntValue = reader.ReadInt32();
                break;
            case "UInt32Property":
                if (binarySize >= 4) raw.UIntValue = reader.ReadUInt32();
                break;
            case "Int64Property":
                if (binarySize >= 8) raw.LongValue = reader.ReadInt64();
                break;
            case "UInt64Property":
                if (binarySize >= 8) raw.ULongValue = reader.ReadUInt64();
                break;
            case "Int8Property":
                if (binarySize >= 1) raw.SByteValue = reader.ReadSByte();
                break;
            case "FloatProperty":
                if (binarySize >= 4) raw.FloatValue = reader.ReadSingle();
                break;
            case "DoubleProperty":
                if (binarySize >= 8) raw.DoubleValue = reader.ReadDouble();
                break;
            case "BoolProperty":
                raw.BoolValue = (raw.Flags & 0x10) != 0;
                break;
            case "ObjectProperty":
                raw.ObjectValue = ReadObjectRef(reader);
                break;
            case "StrProperty":
            case "NameProperty":
                raw.StringValue = _stringSerializer.Deserialize(reader);
                break;
            case "ArrayProperty" when raw.TypeNode is { Children.Count: > 0 }
                                  && IsObjectRefChild(raw.TypeNode.Children):
                {
                    var elementCount = reader.ReadInt32();
                    if (elementCount < 0 || elementCount > 1_000_000)
                        return;
                    var values = new ObjectReferenceValue[elementCount];
                    for (var i = 0; i < elementCount; i++)
                        values[i] = ReadObjectRef(reader);
                    raw.ArrayObjectValues = values;
                    break;
                }
            case "StructProperty":
                raw.StructValue = TryReadFixedStructValue(reader, raw, binarySize);
                break;
            case "MapProperty":
                raw.MapEntries = TryReadSimpleMapEntries(reader, raw);
                break;
        }
    }

    private static readonly HashSet<string> KnownFixedStructTypes = new(StringComparer.Ordinal)
    {
        "Vector", "Rotator",
        "Quat", "Vector4", "Vector4D",
        "Vector2D",
        "Box",
        "LinearColor", "Color",
        "IntPoint", "DateTime",
        "Guid",
        "TimerHandle", "SlateBrush",
        "FluidBox",
        "RailroadTrackPosition"
    };

    private StructValue TryReadFixedStructValue(BinaryReader reader, RawProperty raw, int binarySize)
    {
        var subType = First(raw.TypeNode?.Children)?.Name ?? "";
        if (!KnownFixedStructTypes.Contains(subType))
            return new StructValue(subType, null);

        var posBefore = reader.BaseStream.Position;
        object? value;
        try
        {
            value = subType switch
            {
                "Vector" or "Rotator" => ReadDoubles(reader, 3),
                "Quat" or "Vector4" or "Vector4D" => ReadDoubles(reader, 4),
                "Vector2D" => ReadDoubles(reader, 2),
                "Box" => (object) new BoxValue(ReadDoubles(reader, 3), ReadDoubles(reader, 3), reader.ReadByte() != 0),
                "LinearColor" => ReadFloats(reader, 4),
                "Color" => reader.ReadBytes(4),
                "IntPoint" or "DateTime" => reader.ReadInt64(),
                "Guid" => reader.ReadBytes(16),
                "TimerHandle" or "SlateBrush" => _stringSerializer.Deserialize(reader),
                "FluidBox" => reader.ReadSingle(),
                "RailroadTrackPosition" => new RailroadTrackPositionValue(
                    _stringSerializer.Deserialize(reader),
                    _stringSerializer.Deserialize(reader),
                    reader.ReadSingle(),
                    reader.ReadSingle()),
                _ => null
            };
        }
        catch (EndOfStreamException)
        {
            reader.BaseStream.Seek(posBefore, SeekOrigin.Begin);
            return new StructValue(subType, null);
        }

        return new StructValue(subType, value);
    }

    private static readonly HashSet<string> SimpleMapTypes = new(StringComparer.Ordinal)
    {
        "IntProperty", "Int64Property", "UInt32Property", "UInt64Property",
        "ByteProperty", "BoolProperty", "FloatProperty", "DoubleProperty",
        "StrProperty", "NameProperty", "EnumProperty",
        "ObjectProperty", "SoftObjectProperty", "InterfaceProperty"
    };

    private IReadOnlyList<MapEntryValue>? TryReadSimpleMapEntries(BinaryReader reader, RawProperty raw)
    {
        if (raw.TypeNode is not { Children.Count: >= 2 })
            return null;

        var keyType = ChildAt(raw.TypeNode.Children, 0)?.Name;
        var valueType = ChildAt(raw.TypeNode.Children, 1)?.Name;
        if (keyType is null || valueType is null)
            return null;
        if (!SimpleMapTypes.Contains(keyType) || !SimpleMapTypes.Contains(valueType))
            return null;

        var posBefore = reader.BaseStream.Position;
        try
        {
            var numToRemove = reader.ReadInt32();
            if (numToRemove > 0)
                return null;

            var elementCount = reader.ReadInt32();
            if (elementCount is < 0 or > 1_000_000)
                return null;

            var entries = new MapEntryValue[elementCount];
            for (var i = 0; i < elementCount; i++)
            {
                var k = ReadSimpleMapKeyOrValue(reader, keyType);
                var v = ReadSimpleMapKeyOrValue(reader, valueType);
                if (k is null || v is null)
                    return null;
                entries[i] = new MapEntryValue(k, v);
            }

            return entries;
        }
        catch (EndOfStreamException)
        {
            reader.BaseStream.Seek(posBefore, SeekOrigin.Begin);
            return null;
        }
    }

    private object? ReadSimpleMapKeyOrValue(BinaryReader reader, string typeName) =>
        typeName switch
        {
            "IntProperty" => reader.ReadInt32(),
            "Int64Property" => reader.ReadInt64(),
            "UInt32Property" => reader.ReadUInt32(),
            "UInt64Property" => reader.ReadUInt64(),
            "ByteProperty" => reader.ReadByte(),
            "BoolProperty" => reader.ReadByte() != 0,
            "FloatProperty" => reader.ReadSingle(),
            "DoubleProperty" => reader.ReadDouble(),
            "StrProperty" => _stringSerializer.Deserialize(reader),
            "NameProperty" => _stringSerializer.Deserialize(reader),
            "EnumProperty" => _stringSerializer.Deserialize(reader),
            "ObjectProperty" => ReadObjectRef(reader),
            "SoftObjectProperty" => ReadObjectRef(reader),
            "InterfaceProperty" => ReadObjectRef(reader),
            _ => null
        };

    private static double[] ReadDoubles(BinaryReader reader, int n)
    {
        var arr = new double[n];
        for (var i = 0; i < n; i++)
            arr[i] = reader.ReadDouble();
        return arr;
    }

    private static float[] ReadFloats(BinaryReader reader, int n)
    {
        var arr = new float[n];
        for (var i = 0; i < n; i++)
            arr[i] = reader.ReadSingle();
        return arr;
    }

    private static FPropertyTagNode? First(ICollection<FPropertyTagNode>? children)
    {
        if (children is null)
            return null;
        foreach (var c in children)
            return c;
        return null;
    }

    private static FPropertyTagNode? ChildAt(ICollection<FPropertyTagNode>? children, int index)
    {
        if (children is null)
            return null;
        var i = 0;
        foreach (var c in children)
        {
            if (i++ == index)
                return c;
        }
        return null;
    }

    private ObjectReferenceValue ReadObjectRef(BinaryReader reader)
    {
        var levelName = _stringSerializer.Deserialize(reader);
        var pathName = _stringSerializer.Deserialize(reader);
        return new ObjectReferenceValue(levelName, pathName);
    }

    private static bool IsObjectRefChild(ICollection<FPropertyTagNode> children)
    {
        foreach (var c in children)
            return c.Name == "ObjectProperty";
        return false;
    }
}

