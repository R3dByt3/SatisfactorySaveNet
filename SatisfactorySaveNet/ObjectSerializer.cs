using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SatisfactorySaveNet.Abstracts;
using SatisfactorySaveNet.Abstracts.Exceptions;
using SatisfactorySaveNet.Abstracts.Model;
using System.IO;
using System.Linq;

namespace SatisfactorySaveNet;

public class ObjectSerializer : IObjectSerializer
{
    public static readonly IObjectSerializer Instance = new ObjectSerializer(NullLoggerFactory.Instance, StringSerializer.Instance, ObjectReferenceSerializer.Instance, PropertySerializer.Instance, ExtraDataSerializer.Instance, HexSerializer.Instance);

    private readonly IStringSerializer _stringSerializer;
    private readonly IObjectReferenceSerializer _objectReferenceSerializer;
    private readonly IPropertySerializer _propertySerializer;
    private readonly IExtraDataSerializer _extraDataSerializer;
    private readonly IHexSerializer _hexSerializer;
    private readonly ILogger<ObjectSerializer> _logger;

    public ObjectSerializer(ILoggerFactory loggerFactory, IStringSerializer stringSerializer, IObjectReferenceSerializer objectReferenceSerializer, IPropertySerializer propertySerializer, IExtraDataSerializer extraDataSerializer, IHexSerializer hexSerializer)
    {
        _stringSerializer = stringSerializer;
        _objectReferenceSerializer = objectReferenceSerializer;
        _propertySerializer = propertySerializer;
        _extraDataSerializer = extraDataSerializer;
        _hexSerializer = hexSerializer;
        _logger = loggerFactory.CreateLogger<ObjectSerializer>();
    }

    public ComponentObject Deserialize(BinaryReader reader, Header header, ComponentObject componentObject)
    {
        return componentObject switch
        {
            ActorObject actorObject => DeserializeActor(reader, header, actorObject),
            ComponentObject => DeserializeComponent(reader, header, componentObject),
            _ => throw new CorruptedSatisFactorySaveFileException("Encountered unknown object type")
        };
    }

    private ActorObject DeserializeActor(BinaryReader reader, Header header, ActorObject actorObject)
    {
        if (header.SaveVersion >= 41)
        {
            var version = reader.ReadInt32();
            if (version != header.SaveVersion)
                actorObject.EntitySaveVersion = version;
            _ = reader.ReadInt32();
        }
        var binarySize = reader.ReadInt32();
        var positionStart = reader.BaseStream.Position;

        var parentObjectRoot = _stringSerializer.Deserialize(reader);

        var parentObjectName = _stringSerializer.Deserialize(reader);

        var componentsCount = reader.ReadInt32();
        var components = new ObjectReference[componentsCount];

        for (var i = 0; i < componentsCount; i++)
        {
            var objectRef = _objectReferenceSerializer.Deserialize(reader);
            components[i] = objectRef;
        }

        actorObject.ParentObjectRoot = parentObjectRoot;
        actorObject.ParentObjectName = parentObjectName;

        var expectedPosition = positionStart + binarySize;
        actorObject.Components = components;

        if (expectedPosition == reader.BaseStream.Position)
            return actorObject;

        var properties = _propertySerializer.DeserializeProperties(reader, header, expectedPosition: expectedPosition).ToArray();

        actorObject.Properties = properties;
        actorObject.ExtraData = _extraDataSerializer.Deserialize(reader, actorObject.TypePath, header, expectedPosition);

        var missingBytes = expectedPosition - reader.BaseStream.Position;

        if (missingBytes > 0)
        {
            var hex = _hexSerializer.Deserialize(reader, missingBytes.ToInt());
            if (hex.Any(c => c != '\0'))
                _logger.LogWarning("Missing bytes: {MissingBytes}", hex);
        }
        else if (missingBytes < 0)
            reader.BaseStream.Seek(missingBytes, SeekOrigin.Current);

        return actorObject;
    }

    private ComponentObject DeserializeComponent(BinaryReader reader, Header header, ComponentObject componentObject)
    {
        if (header.SaveVersion >= 41)
        {
            var version = reader.ReadInt32();
            if (version != header.SaveVersion)
                componentObject.EntitySaveVersion = version;
            _ = reader.ReadInt32();
        }
        var binarySize = reader.ReadInt32();
        var positionStart = reader.BaseStream.Position;

        var properties = _propertySerializer.DeserializeProperties(reader, header).ToArray();
        componentObject.Properties = properties;

        var expectedPosition = positionStart + binarySize;
        var missingBytes = expectedPosition - reader.BaseStream.Position;

        if (missingBytes > 0)
        {
            var hex = _hexSerializer.Deserialize(reader, missingBytes.ToInt());
            if (hex.Any(c => c != '\0'))
                _logger.LogCritical("BAD READ {MissingBytes}", missingBytes);
        }
        else if (missingBytes < 0)
            reader.BaseStream.Seek(missingBytes, SeekOrigin.Current);

        return componentObject;
    }
}