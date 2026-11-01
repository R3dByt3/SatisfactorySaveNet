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

    public IEnumerable<Property> DeserializeProperties(BinaryReader reader, Header? header = null, string? type = null, long? expectedPosition = null, SaveParseContext? context = null)
    {
        if (expectedPosition != null && expectedPosition < reader.BaseStream.Position)
            yield break;

        Property? property;
        while ((property = DeserializeProperty(reader, header, type, context)) != null)
        {
            yield return property;

            if (expectedPosition != null && expectedPosition < reader.BaseStream.Position)
                yield break;
        }
    }

    [return: NotNullIfNotNull(nameof(type))]
    public Property? DeserializeProperty(BinaryReader reader, Header? header = null, string? type = null, SaveParseContext? context = null)
    {
        var propertyName = string.Empty;
        List<PropertyTypeNameNode>? typeNameNodes = null;
        var tag = default(PropertyTag);

        if (type == null)
        {
            propertyName = _stringSerializer.Deserialize(reader);

            if (string.Equals(propertyName, "None", StringComparison.Ordinal) || string.IsNullOrWhiteSpace(propertyName))
                return null;

            type = _stringSerializer.Deserialize(reader);

            if (header != null && header.SaveVersion >= 53 && context is { } parseContext && parseContext.LevelUE5Version >= 1011)
            {
                typeNameNodes = ReadPropertyTypeNameNodes(reader);
                tag = DerivePropertyTag(type, typeNameNodes, parseContext);
            }
        }

        Property property = type switch
        {
            nameof(ArrayProperty) => header == null ? throw new ArgumentNullException(nameof(header)) : DeserializeArrayProperty(reader, header, context, tag),
            nameof(BoolProperty) => DeserializeBoolProperty(reader, context, tag),
            nameof(ByteProperty) => DeserializeByteProperty(reader, context, tag),
            nameof(EnumProperty) => DeserializeEnumProperty(reader, context, tag),
            nameof(FloatProperty) => DeserializeFloatProperty(reader, tag),
            nameof(IntProperty) => DeserializeIntProperty(reader, tag),
            nameof(Int64Property) => DeserializeInt64Property(reader, tag),
            nameof(UInt64Property) => DeserializeUInt64Property(reader, tag),
            nameof(MapProperty) => header == null ? throw new ArgumentNullException(nameof(header)) : DeserializeMapProperty(reader, header, propertyName, type, context, tag),
            nameof(NameProperty) => DeserializeNameProperty(reader, tag),
            nameof(ObjectProperty) => DeserializeObjectProperty(reader, tag),
            nameof(SoftObjectProperty) => DeserializeSoftObjectProperty(reader, tag),
            nameof(SetProperty) => DeserializeSetProperty(reader, propertyName, context, tag),
            nameof(StrProperty) => DeserializeStrProperty(reader, tag),
            nameof(StructProperty) => header == null ? throw new ArgumentNullException(nameof(header)) : DeserializeStructProperty(reader, header, context, tag),
            nameof(TextProperty) => header == null ? throw new ArgumentNullException(nameof(header)) : DeserializeTextProperty(reader, header, context: context, tag: tag),
            nameof(UInt32Property) => DeserializeUInt32Property(reader, tag),
            nameof(Int8Property) => DeserializeInt8Property(reader, tag),
            nameof(DoubleProperty) => DeserializeDoubleProperty(reader, tag),
            nameof(FINNetworkProperty) => DeserializeFINNetworkProperty(reader),

            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        property.Name = propertyName;

        if (typeNameNodes != null)
            property.TypeNameNodes = typeNameNodes;

        return property;
    }

    private readonly record struct PropertyTag(
        bool NewLayout,
        string? InnerType = null,
        string? StructureSubType = null,
        string? EnumType = null,
        string? EnumName = null,
        string? ByteEnumName = null,
        string? KeyType = null,
        string? ValueType = null,
        bool ByteNewLayout = false,
        string? InnerTypeName = null,
        string? KeyTypeName = null,
        string? ValueTypeName = null);

    private List<PropertyTypeNameNode> ReadPropertyTypeNameNodes(BinaryReader reader)
    {
        var nodes = new List<PropertyTypeNameNode>();
        var count = reader.ReadInt32();

        for (var i = 0; i < count; i++)
        {
            var node = new PropertyTypeNameNode
            {
                Name = _stringSerializer.Deserialize(reader)
            };

            var children = ReadPropertyTypeNameNodes(reader);
            if (children.Count > 0)
                node.Nodes = children;

            nodes.Add(node);
        }

        return nodes;
    }

    private static PropertyTag DerivePropertyTag(string type, List<PropertyTypeNameNode> nodes, SaveParseContext context)
    {
        string? innerType = null;
        string? structureSubType = null;
        string? enumType = null;
        string? enumName = null;
        string? byteEnumName = null;
        string? keyType = null;
        string? valueType = null;
        string? innerTypeName = null;
        string? keyTypeName = null;
        string? valueTypeName = null;

        switch (type)
        {
            case nameof(ArrayProperty):
                innerTypeName = nodes[0].Name;
                innerType = StripPropertySuffix(nodes[0].Name);
                if (string.Equals(innerType, "Struct", StringComparison.Ordinal))
                    structureSubType = nodes[0].Nodes!.First().Name;
                else if (string.Equals(innerType, "Enum", StringComparison.Ordinal))
                    enumName = nodes[0].Nodes!.First().Name;
                break;
            case nameof(SetProperty):
            case nameof(StructProperty):
                innerTypeName = nodes[0].Name;
                innerType = StripPropertySuffix(nodes[0].Name);
                break;
            case nameof(ByteProperty):
                if (nodes.Count > 0)
                    byteEnumName = StripPropertySuffix(nodes[0].Name);
                break;
            case nameof(EnumProperty):
                if (nodes.Count > 0)
                    enumType = StripPropertySuffix(nodes[0].Name);
                break;
            case nameof(MapProperty):
                keyTypeName = nodes[0].Name;
                valueTypeName = nodes[1].Name;
                keyType = StripPropertySuffix(nodes[0].Name);
                valueType = StripPropertySuffix(nodes[1].Name);
                break;
        }

        return new PropertyTag(
            true,
            innerType,
            structureSubType,
            enumType,
            enumName,
            byteEnumName,
            keyType,
            valueType,
            context.EntitySaveVersion >= 53 && context.LevelUE5Version >= 1011,
            innerTypeName,
            keyTypeName,
            valueTypeName);
    }

    private static string StripPropertySuffix(string name)
    {
        var index = name.IndexOf("Property", StringComparison.Ordinal);
        return index < 0 ? name : name.Remove(index, "Property".Length);
    }

    private static int ReadPropertyIndex(BinaryReader reader, PropertyTag tag) => tag.NewLayout ? 0 : reader.ReadInt32();

    private static void ReadPropertyGuid(BinaryReader reader, PropertyTag tag)
    {
        if (tag.NewLayout)
        {
            if (reader.ReadByte() == 1)
                _ = reader.ReadBytes(16);
        }
        else
        {
            _ = reader.ReadSByte();
        }
    }

    private static sbyte ReadTextPadding(BinaryReader reader, PropertyTag tag)
    {
        var padding = reader.ReadSByte();
        if (tag.NewLayout && padding == 1)
            _ = reader.ReadBytes(16);
        return padding;
    }

    private readonly record struct ModeTypeData(int ModeType, string? ModeUnk1, string? ModeUnk2, string? ModeUnk3);

    private ModeTypeData ReadModeType(BinaryReader reader)
    {
        var modeType = reader.ReadInt32();

        if (modeType == 2)
            return new ModeTypeData(modeType, null, _stringSerializer.Deserialize(reader), _stringSerializer.Deserialize(reader));

        if (modeType == 3)
            return new ModeTypeData(modeType, _hexSerializer.Deserialize(reader, 9), _stringSerializer.Deserialize(reader), _stringSerializer.Deserialize(reader));

        return new ModeTypeData(modeType, null, null, null);
    }

    private ArrayPropertyBase DeserializeArrayProperty(BinaryReader reader, Header header, string type, int count, SaveParseContext? context, PropertyTag tag = default)
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
            nameof(TextProperty) => DeserializeArrayTextProperty(reader, header, count, context, tag),
            nameof(SoftObjectProperty) => DeserializeArraySoftObjectProperty(reader, count),
            nameof(ObjectProperty) => DeserializeArrayObjectProperty(reader, count),
            nameof(InterfaceProperty) => DeserializeArrayInterfaceProperty(reader, count),
            nameof(StructProperty) => DeserializeArrayStructProperty(reader, header, count, context, tag),
            //ToDo: All implemented?

            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    private ArrayTextProperty DeserializeArrayTextProperty(BinaryReader reader, Header header, int count, SaveParseContext? context, PropertyTag tag = default)
    {
        var values = new TextProperty[count];

        for (var x = 0; x < count; x++)
        {
            values[x] = DeserializeTextPropertyValue(reader, header, context);
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

    private ArrayProperty DeserializeArrayProperty(BinaryReader reader, Header header, SaveParseContext? context, PropertyTag tag = default)
    {
        var binarySize = reader.ReadInt32();
        var index = ReadPropertyIndex(reader, tag);
        var type = tag.NewLayout
            ? tag.InnerTypeName ?? throw new InvalidDataException("Missing array property type name")
            : _stringSerializer.Deserialize(reader);
        _ = reader.ReadSByte();
        var length = reader.ReadInt32();

        var property = DeserializeArrayProperty(reader, header, type, length, context, tag);

        return new ArrayProperty
        {
            Index = index,
            Type = type,
            Property = property
        };
    }

    private TextProperty DeserializeTextProperty(BinaryReader reader, Header header, SaveParseContext? context = null, PropertyTag tag = default)
    {
        _ = reader.ReadInt32();
        var index = ReadPropertyIndex(reader, tag);
        _ = ReadTextPadding(reader, tag);

        var property = DeserializeTextPropertyValue(reader, header, context);
        property.Index = index;

        return property;
    }

    private TextProperty DeserializeTextPropertyValue(BinaryReader reader, Header header, SaveParseContext? context = null)
    {
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
        byte? valueType = null;
        int? argumentValue = null;
        int? argumentValueUnknown = null;
        int? argumentValueUnknown1 = null;
        int? argumentValueUnknown2 = null;
        TextProperty? argumentPropertyValue = null;

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
                sourceFmt = DeserializeTextPropertyValue(reader, header, context);
                var argumentsCount = reader.ReadInt32();
                arguments = new TextArgument[argumentsCount];

                for (var x = 0; x < argumentsCount; x++)
                {
                    var name = historyType == 2 ? string.Empty : _stringSerializer.Deserialize(reader);
                    var argumentValueType = reader.ReadByte();
                    TextArgument textArgument = argumentValueType switch
                    {
                        0 => new TextArgumentV0 { Name = name, ArgumentValue = reader.ReadInt32(), ArgumentValueUnknown = reader.ReadInt32() },
                        4 => new TextArgumentV4 { Name = name, ArgumentPropertyValue = DeserializeTextPropertyValue(reader, header, context) },
                        _ => throw new InvalidDataException("Unknown valueType type in array text property"),
                    };
                    arguments[x] = textArgument;
                }
                break;
            case 4:
                var historyValueType = reader.ReadByte();
                valueType = historyValueType;
                switch (historyValueType)
                {
                    case 0:
                        argumentValue = reader.ReadInt32();
                        argumentValueUnknown = reader.ReadInt32();
                        argumentValueUnknown1 = reader.ReadInt32();
                        argumentValueUnknown2 = reader.ReadInt32();
                        break;
                    case 4:
                        argumentPropertyValue = DeserializeTextPropertyValue(reader, header, context);
                        break;
                    default:
                        throw new InvalidDataException("Unknown valueType type in array text property");
                }
                break;
            case 10:
                sourceText = DeserializeTextPropertyValue(reader, header, context);
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
            Arguments = arguments,
            ValueType = valueType,
            ArgumentValue = argumentValue,
            ArgumentValueUnknown = argumentValueUnknown,
            ArgumentValueUnknown1 = argumentValueUnknown1,
            ArgumentValueUnknown2 = argumentValueUnknown2,
            ArgumentPropertyValue = argumentPropertyValue
        };
    }

    private ArrayStructProperty DeserializeArrayStructProperty(BinaryReader reader, Header header, int length, SaveParseContext? context, PropertyTag tag = default)
    {
        string propertyName;
        string propertyType;
        string elementType;
        (int, int, int, int) uuid;
        int binarySize;

        if (tag.NewLayout)
        {
            propertyName = string.Empty;
            propertyType = tag.InnerTypeName ?? throw new InvalidDataException("Missing array property type name");
            binarySize = 0;
            elementType = tag.StructureSubType ?? throw new InvalidDataException("Missing array struct subtype");
            uuid = (0, 0, 0, 0);
        }
        else
        {
            propertyName = _stringSerializer.Deserialize(reader);
            propertyType = _stringSerializer.Deserialize(reader);

            binarySize = reader.ReadInt32();
            _ = reader.ReadInt32();
            elementType = _stringSerializer.Deserialize(reader);

            var uuid1 = reader.ReadInt32();
            var uuid2 = reader.ReadInt32();
            var uuid3 = reader.ReadInt32();
            var uuid4 = reader.ReadInt32();
            _ = reader.ReadSByte();

            uuid = (uuid1, uuid2, uuid3, uuid4);
        }

        var values = new TypedData[length];

        for (var x = 0; x < length; x++)
        {
            values[x] = _typedDataSerializer.Deserialize(reader, header, elementType, true, binarySize, context);
        }

        var property = new ArrayStructProperty
        {
            PropertyName = propertyName,
            PropertyType = propertyType,
            ElementType = elementType,
            UUID = uuid,
            Values = values
        };

        return property;
    }

    private static BoolProperty DeserializeBoolProperty(BinaryReader reader, SaveParseContext? context, PropertyTag tag = default)
    {
        _ = reader.ReadInt32();
        var index = 0;
        sbyte value;

        if (tag.NewLayout)
        {
            var rawValue = reader.ReadByte();
            value = (sbyte)(rawValue == 16 ? 1 : rawValue);
        }
        else
        {
            index = reader.ReadInt32();
            value = reader.ReadSByte();
            _ = reader.ReadSByte();
        }

        var property = new BoolProperty
        {
            Index = index,
            Value = value
        };

        return property;
    }

    private ByteProperty DeserializeByteProperty(BinaryReader reader, SaveParseContext? context, PropertyTag tag = default)
    {
        var binarySize = reader.ReadInt32();
        var index = 0;
        sbyte? byteData = null;
        string? stringData = null;
        string type;

        if (tag.NewLayout && tag.ByteNewLayout)
        {
            _ = reader.ReadByte();

            if (tag.ByteEnumName != null)
            {
                type = tag.ByteEnumName;
                stringData = _stringSerializer.Deserialize(reader);
            }
            else
            {
                type = "None";
                byteData = reader.ReadSByte();
            }
        }
        else
        {
            if (!tag.NewLayout)
                index = reader.ReadInt32();

            type = _stringSerializer.Deserialize(reader);
            ReadPropertyGuid(reader, tag);

            if (string.Equals(type, "None", StringComparison.Ordinal))
                byteData = reader.ReadSByte();
            else
                stringData = _stringSerializer.Deserialize(reader);
        }

        var property = new ByteProperty
        {
            Index = index,
            Type = type,
            ByteData = byteData,
            StringData = stringData
        };

        return property;
    }

    private EnumProperty DeserializeEnumProperty(BinaryReader reader, SaveParseContext? context, PropertyTag tag = default)
    {
        var binarySize = reader.ReadInt32();
        var index = 0;
        string type;
        string value;

        if (tag.NewLayout)
        {
            _ = reader.ReadByte();
            type = tag.EnumType!;
            value = _stringSerializer.Deserialize(reader);
        }
        else
        {
            index = reader.ReadInt32();
            type = _stringSerializer.Deserialize(reader);
            _ = reader.ReadSByte();
            value = _stringSerializer.Deserialize(reader);
        }

        var property = new EnumProperty
        {
            Index = index,
            Type = type,
            Value = value
        };

        return property;
    }

    private static FloatProperty DeserializeFloatProperty(BinaryReader reader, PropertyTag tag = default)
    {
        var binarySize = reader.ReadInt32();
        var index = ReadPropertyIndex(reader, tag);
        ReadPropertyGuid(reader, tag);
        var value = reader.ReadSingle();

        var property = new FloatProperty
        {
            Index = index,
            Value = value
        };

        return property;
    }

    private static DoubleProperty DeserializeDoubleProperty(BinaryReader reader, PropertyTag tag = default)
    {
        var binarySize = reader.ReadInt32();
        var index = ReadPropertyIndex(reader, tag);
        ReadPropertyGuid(reader, tag);
        var value = reader.ReadDouble();

        var property = new DoubleProperty
        {
            Index = index,
            Value = value
        };

        return property;
    }

    private static IntProperty DeserializeIntProperty(BinaryReader reader, PropertyTag tag = default)
    {
        var binarySize = reader.ReadInt32();
        var index = ReadPropertyIndex(reader, tag);
        ReadPropertyGuid(reader, tag);
        var value = reader.ReadInt32();

        var property = new IntProperty
        {
            Index = index,
            Value = value
        };

        return property;
    }

    private static Int8Property DeserializeInt8Property(BinaryReader reader, PropertyTag tag = default)
    {
        var binarySize = reader.ReadInt32();
        var index = ReadPropertyIndex(reader, tag);
        ReadPropertyGuid(reader, tag);
        var value = reader.ReadSByte();

        var property = new Int8Property
        {
            Index = index,
            Value = value
        };

        return property;
    }

    private static UInt32Property DeserializeUInt32Property(BinaryReader reader, PropertyTag tag = default)
    {
        var binarySize = reader.ReadInt32();
        var index = ReadPropertyIndex(reader, tag);
        ReadPropertyGuid(reader, tag);
        var value = reader.ReadUInt32();

        var property = new UInt32Property
        {
            Index = index,
            Value = value
        };

        return property;
    }

    private static Int64Property DeserializeInt64Property(BinaryReader reader, PropertyTag tag = default)
    {
        var binarySize = reader.ReadInt32();
        var index = ReadPropertyIndex(reader, tag);
        ReadPropertyGuid(reader, tag);
        var value = reader.ReadInt64();

        var property = new Int64Property
        {
            Index = index,
            Value = value
        };

        return property;
    }

    private static UInt64Property DeserializeUInt64Property(BinaryReader reader, PropertyTag tag = default)
    {
        var binarySize = reader.ReadInt32();
        var index = ReadPropertyIndex(reader, tag);
        ReadPropertyGuid(reader, tag);
        var value = reader.ReadUInt64();

        var property = new UInt64Property
        {
            Index = index,
            Value = value
        };

        return property;
    }

    private MapProperty DeserializeMapProperty(BinaryReader reader, Header header, string? propertyName = null, string? type = null, SaveParseContext? context = null, PropertyTag tag = default)
    {
        var binarySize = reader.ReadInt32();
        var index = ReadPropertyIndex(reader, tag);
        var keyType = tag.NewLayout
            ? tag.KeyTypeName ?? throw new InvalidDataException("Missing map key property type name")
            : _stringSerializer.Deserialize(reader);
        var valueType = tag.NewLayout
            ? tag.ValueTypeName ?? throw new InvalidDataException("Missing map value property type name")
            : _stringSerializer.Deserialize(reader);
        var padding = reader.ReadSByte();
        var mode = ReadModeType(reader);

        var count = reader.ReadInt32();

        var property = new MapProperty
        {
            Index = index,
            KeyType = keyType,
            ValueType = valueType,
            ModeType = mode.ModeType,
            ModeUnk1 = mode.ModeUnk1,
            ModeUnk2 = mode.ModeUnk2,
            ModeUnk3 = mode.ModeUnk3,
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
                    if (string.Equals(type, "/BuildGunUtilities/BGU_Subsystem.BGU_Subsystem_C", StringComparison.Ordinal) || string.Equals(type, "/Script/NoImpure.NoImpureSubsystem")) //ToDo: Debug?
                    {
                        key = new Vector3Union { Value = _vectorSerializer.DeserializeVec3(reader) };
                        break;
                    }
                    if (string.Equals(propertyName, "mSaveData", StringComparison.Ordinal) || string.Equals(propertyName, "mUnresolvedSaveData", StringComparison.Ordinal))
                    {
                        key = new Vector3IUnion { Value = _vectorSerializer.DeserializeVec3I(reader) };
                        break;
                    }
                    key = new PropertiesUnion { Value = [.. DeserializeProperties(reader, header, context: context)] };
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
                                (property.KeyType == nameof(StrProperty) || string.Equals(property.KeyType, "/Script/NoImpure.NoImpureSubsystem", StringComparison.Ordinal))
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
                        : new StrUnion { Value = _stringSerializer.Deserialize(reader) };
                    break;
                case nameof(ObjectProperty):
                    value = string.Equals(type, "/BuildGunUtilities/BGU_Subsystem.BGU_Subsystem_C", StringComparison.Ordinal)
                        ? new ObjectReferenceUnion { Unknown1 = reader.ReadSingle(), Unknown2 = reader.ReadSingle(), Unknown3 = reader.ReadSingle(), Unknown4 = reader.ReadSingle(), Unknown5 = _stringSerializer.Deserialize(reader) }
                        : new ObjectReferenceUnion { Value = _objectReferenceSerializer.Deserialize(reader) };
                    break;
                case nameof(TextProperty):
                    value = new TextUnion { Value = DeserializeTextPropertyValue(reader, header, context) };
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
                    value = new PropertiesUnion { Value = [.. DeserializeProperties(reader, header, context: context)] };
                    break;
                default:
                    throw new InvalidDataException("Unknown dictionary property value type");
            }

            property.Elements.Add(key, value);
        }

        return property;
    }

    private NameProperty DeserializeNameProperty(BinaryReader reader, PropertyTag tag = default)
    {
        var binarySize = reader.ReadInt32();
        var index = ReadPropertyIndex(reader, tag);
        ReadPropertyGuid(reader, tag);
        var value = _stringSerializer.Deserialize(reader);

        var property = new NameProperty
        {
            Index = index,
            Value = value
        };

        return property;
    }

    private ObjectProperty DeserializeObjectProperty(BinaryReader reader, PropertyTag tag = default)
    {
        var binarySize = reader.ReadInt32();
        var index = ReadPropertyIndex(reader, tag);
        ReadPropertyGuid(reader, tag);
        var value = _objectReferenceSerializer.Deserialize(reader);

        var property = new ObjectProperty
        {
            Index = index,
            Value = value
        };

        return property;
    }

    private SoftObjectProperty DeserializeSoftObjectProperty(BinaryReader reader, PropertyTag tag = default)
    {
        var binarySize = reader.ReadInt32();
        var index = ReadPropertyIndex(reader, tag);
        ReadPropertyGuid(reader, tag);
        var value = _softObjectReferenceSerializer.Deserialize(reader);

        var property = new SoftObjectProperty
        {
            Index = index,
            Value = value,
        };

        return property;
    }

    private SetProperty DeserializeSetProperty(BinaryReader reader, string propertyName, SaveParseContext? context = null, PropertyTag tag = default)
    {
        var binarySize = reader.ReadInt32();
        var index = ReadPropertyIndex(reader, tag);
        var type = context is not null && context.EntitySaveVersion >= 53
            ? tag.InnerTypeName ?? throw new InvalidDataException("Missing set property type name")
            : _stringSerializer.Deserialize(reader);
        _ = reader.ReadSByte();
        var mode = ReadModeType(reader);
        var count = reader.ReadInt32();

        var property = new SetProperty
        {
            Index = index,
            Type = type,
            ModeType = mode.ModeType,
            ModeUnk1 = mode.ModeUnk1,
            ModeUnk2 = mode.ModeUnk2,
            ModeUnk3 = mode.ModeUnk3,
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

    private StrProperty DeserializeStrProperty(BinaryReader reader, PropertyTag tag = default)
    {
        var binarySize = reader.ReadInt32();
        var index = ReadPropertyIndex(reader, tag);
        ReadPropertyGuid(reader, tag);
        var value = _stringSerializer.Deserialize(reader);

        var property = new StrProperty
        {
            Index = index,
            Value = value
        };

        return property;
    }

    private StructProperty DeserializeStructProperty(BinaryReader reader, Header header, SaveParseContext? context, PropertyTag tag = default)
    {
        var binarySize = reader.ReadInt32();
        //var positionStart = reader.BaseStream.Position;
        var index = ReadPropertyIndex(reader, tag);
        string type;
        if (tag.NewLayout)
        {
            type = tag.InnerType ?? throw new InvalidDataException("Missing struct property type name");
        }
        else
        {
            type = _stringSerializer.Deserialize(reader);
            _ = reader.ReadInt64();
            _ = reader.ReadInt64();
        }
        var hasIndex = reader.ReadSByte();
        if (tag.NewLayout && hasIndex == 9)
            index = reader.ReadInt32();
        var typedData = _typedDataSerializer.Deserialize(reader, header, type, false, binarySize, context);

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
}
