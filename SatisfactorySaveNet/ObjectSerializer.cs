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
    public static readonly IObjectSerializer Instance = new ObjectSerializer(NullLoggerFactory.Instance, StringSerializer.Instance, ObjectReferenceSerializer.Instance, PropertySerializer.Instance, ExtraDataSerializer.Instance, HexSerializer.Instance, DataPackageVersionSerializer.Instance);

    private readonly IStringSerializer _stringSerializer;
    private readonly IObjectReferenceSerializer _objectReferenceSerializer;
    private readonly IPropertySerializer _propertySerializer;
    private readonly IExtraDataSerializer _extraDataSerializer;
    private readonly IHexSerializer _hexSerializer;
    private readonly IDataPackageVersionSerializer _dataPackageVersionSerializer;
    private readonly ILogger<ObjectSerializer> _logger;

    public ObjectSerializer(ILoggerFactory loggerFactory, IStringSerializer stringSerializer, IObjectReferenceSerializer objectReferenceSerializer, IPropertySerializer propertySerializer, IExtraDataSerializer extraDataSerializer, IHexSerializer hexSerializer, IDataPackageVersionSerializer? dataPackageVersionSerializer = null)
    {
        _stringSerializer = stringSerializer;
        _objectReferenceSerializer = objectReferenceSerializer;
        _propertySerializer = propertySerializer;
        _extraDataSerializer = extraDataSerializer;
        _hexSerializer = hexSerializer;
        _dataPackageVersionSerializer = dataPackageVersionSerializer ?? DataPackageVersionSerializer.Instance;
        _logger = loggerFactory.CreateLogger<ObjectSerializer>();
    }

    public ComponentObject Deserialize(BinaryReader reader, Header header, ComponentObject componentObject, SaveParseContext? context = null)
    {
        return componentObject switch
        {
            ActorObject actorObject => DeserializeActor(reader, header, actorObject, context),
            ComponentObject => DeserializeComponent(reader, header, componentObject, context),
            _ => throw new CorruptedSatisFactorySaveFileException("Encountered unknown object type")
        };
    }

    private ActorObject DeserializeActor(BinaryReader reader, Header header, ActorObject actorObject, SaveParseContext? context)
    {
        var entitySaveVersion = ReadEntityVersion(reader, header, actorObject, context);
        var binarySize = reader.ReadInt32();
        var positionStart = reader.BaseStream.Position;

        PeekLevelUE5Version(reader, entitySaveVersion, positionStart, binarySize, context);

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

        if (context != null)
            ApplyPropertyPreamble(reader, header, context);

        var properties = _propertySerializer.DeserializeProperties(reader, header, expectedPosition: expectedPosition, context: context).ToArray();

        actorObject.Properties = properties;
        actorObject.ExtraData = _extraDataSerializer.Deserialize(reader, actorObject.TypePath, header, expectedPosition, context);

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

    private ComponentObject DeserializeComponent(BinaryReader reader, Header header, ComponentObject componentObject, SaveParseContext? context)
    {
        var entitySaveVersion = ReadEntityVersion(reader, header, componentObject, context);
        var binarySize = reader.ReadInt32();
        var positionStart = reader.BaseStream.Position;

        PeekLevelUE5Version(reader, entitySaveVersion, positionStart, binarySize, context);

        if (reader.BaseStream.Position != positionStart + binarySize)
        {
            if (context != null)
                ApplyPropertyPreamble(reader, header, context);

            var properties = _propertySerializer.DeserializeProperties(reader, header, context: context).ToArray();
            componentObject.Properties = properties;
        }

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

    private static int ReadEntityVersion(BinaryReader reader, Header header, ComponentObject componentObject, SaveParseContext? context)
    {
        var entitySaveVersion = header.SaveVersion;
        var levelSaveVersion = context?.LevelSaveVersion ?? header.SaveVersion;
        if (levelSaveVersion != header.SaveVersion && levelSaveVersion <= header.SaveVersion)
            entitySaveVersion = levelSaveVersion;

        if (header.SaveVersion >= 41)
        {
            var version = reader.ReadUInt32();
            if ((version != (uint)header.SaveVersion || version != (uint)levelSaveVersion) && version <= (uint)header.SaveVersion)
            {
                entitySaveVersion = (int)version;
                if (version != (uint)header.SaveVersion)
                    componentObject.EntitySaveVersion = (int)version;
            }

            var migrate = reader.ReadUInt32();
            if (migrate != 0)
                componentObject.ShouldMigrateObjectRefsToPersistent = migrate;
        }

        if (context != null)
            context.EntitySaveVersion = entitySaveVersion;

        return entitySaveVersion;
    }

    private void PeekLevelUE5Version(BinaryReader reader, int entitySaveVersion, long positionStart, long binarySize, SaveParseContext? context)
    {
        if (entitySaveVersion < 53)
            return;

        var savedPosition = reader.BaseStream.Position;
        reader.BaseStream.Seek(positionStart + binarySize, SeekOrigin.Begin);
        if (reader.ReadInt32() != 0)
        {
            var dataPackageVersion = _dataPackageVersionSerializer.Deserialize(reader);
            if (context != null)
                context.LevelUE5Version = dataPackageVersion.PackageFileVersion.UE5Version;
        }

        reader.BaseStream.Position = savedPosition;
    }

    private static void ApplyPropertyPreamble(BinaryReader reader, Header header, SaveParseContext context)
    {
        if (context.EntitySaveVersion < 53)
            context.LevelUE5Version = 1000;

        if (context.EntitySaveVersion == 53 && context.LevelUE5Version == 1000)
        {
            var b = reader.ReadByte();
            reader.BaseStream.Position--;
            if (b == 0)
                context.LevelUE5Version = 1011;
        }

        if (header.SaveVersion >= 53 && context.LevelUE5Version >= 1011)
            _ = reader.ReadByte();
    }
}