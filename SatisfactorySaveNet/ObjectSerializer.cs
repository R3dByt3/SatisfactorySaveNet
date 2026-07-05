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
        if (header.SaveVersion >= 53)
        {
            return componentObject switch
            {
                ActorObject actorObject => DeserializeActorSave53(reader, header, actorObject),
                ComponentObject => DeserializeComponentSave53(reader, header, componentObject),
                _ => throw new CorruptedSatisFactorySaveFileException("Encountered unknown object type")
            };
        }

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

    private const int SerializeDataPackageVersionAndCustomVersions = 53;

    private ActorObject DeserializeActorSave53(BinaryReader reader, Header header, ActorObject actorObject)
    {
        ReadEntityHeaderSave53(reader, header, actorObject);
        var perObjectSaveVersion = header.ParseState.CurrentEntitySaveVersion;

        var binarySize = reader.ReadInt32();
        var positionStart = reader.BaseStream.Position;

        ReadEntityDataPackageAtBlockEndSave53(reader, header, binarySize, positionStart);

        var parentObjectRoot = _stringSerializer.Deserialize(reader);
        var parentObjectName = _stringSerializer.Deserialize(reader);

        var componentsCount = reader.ReadInt32();
        var components = new ObjectReference[componentsCount];
        for (var i = 0; i < componentsCount; i++)
        {
            components[i] = _objectReferenceSerializer.Deserialize(reader);
        }

        actorObject.ParentObjectRoot = parentObjectRoot;
        actorObject.ParentObjectName = parentObjectName;
        actorObject.Components = components;

        var expectedPosition = positionStart + binarySize;
        if (expectedPosition == reader.BaseStream.Position)
        {
            ReadTrailingDataPackageVersionSave53(reader, header, actorObject, perObjectSaveVersion);
            return actorObject;
        }

        TryApplyUe5VersionFixSave53(reader, header);

        header.ParseState.CurrentEntityPayloadEnd = expectedPosition;
        try
        {
            actorObject.Properties = _propertySerializer.DeserializeProperties(reader, header, expectedPosition: expectedPosition, saveVersion: perObjectSaveVersion).ToArray();
        }
        finally
        {
            header.ParseState.CurrentEntityPayloadEnd = null;
        }

        ReadOptionalObjectGuidSave53(reader, perObjectSaveVersion);

        if (ShouldDeserializeExtraDataSave53(actorObject.TypePath, perObjectSaveVersion))
        {
            actorObject.ExtraData = _extraDataSerializer.Deserialize(reader, actorObject.TypePath, header, expectedPosition);
        }

        ConsumeMissingBytes(reader, expectedPosition);
        ReadTrailingDataPackageVersionSave53(reader, header, actorObject, perObjectSaveVersion);
        return actorObject;
    }

    private ComponentObject DeserializeComponentSave53(BinaryReader reader, Header header, ComponentObject componentObject)
    {
        ReadEntityHeaderSave53(reader, header, componentObject);
        var perObjectSaveVersion = header.ParseState.CurrentEntitySaveVersion;

        var binarySize = reader.ReadInt32();
        var positionStart = reader.BaseStream.Position;

        ReadEntityDataPackageAtBlockEndSave53(reader, header, binarySize, positionStart);

        TryApplyUe5VersionFixSave53(reader, header);

        var componentEnd = positionStart + binarySize;
        header.ParseState.CurrentEntityPayloadEnd = componentEnd;
        try
        {
            componentObject.Properties = _propertySerializer.DeserializeProperties(reader, header, expectedPosition: componentEnd, saveVersion: perObjectSaveVersion).ToArray();
        }
        finally
        {
            header.ParseState.CurrentEntityPayloadEnd = null;
        }

        ReadOptionalObjectGuidSave53(reader, perObjectSaveVersion);

        ConsumeMissingBytes(reader, positionStart + binarySize);
        ReadTrailingDataPackageVersionSave53(reader, header, componentObject, perObjectSaveVersion);
        return componentObject;
    }

    private static bool ShouldDeserializeExtraDataSave53(string typePath, int perObjectSaveVersion)
    {
        if (perObjectSaveVersion < SerializeDataPackageVersionAndCustomVersions)
        {
            return true;
        }

        return KnownConstants.IsConveyor(typePath)
               || KnownConstants.IsPowerLine(typePath)
               || typePath == "/Game/FactoryGame/-Shared/Blueprint/BP_CircuitSubsystem.BP_CircuitSubsystem_C";
    }

    private static void ReadOptionalObjectGuidSave53(BinaryReader reader, int perObjectSaveVersion)
    {
        if (perObjectSaveVersion < SerializeDataPackageVersionAndCustomVersions)
        {
            return;
        }

        if (reader.ReadInt32() > 0)
        {
            _ = reader.ReadBytes(16);
        }
    }

    private void ReadEntityHeaderSave53(BinaryReader reader, Header header, ComponentObject componentObject)
    {
        header.ParseState.CurrentEntitySaveVersion = header.SaveVersion;
        if (header.ParseState.CurrentLevelSaveVersion != header.SaveVersion
            && header.ParseState.CurrentLevelSaveVersion <= header.SaveVersion)
        {
            header.ParseState.CurrentEntitySaveVersion = header.ParseState.CurrentLevelSaveVersion;
        }

        var version = reader.ReadUInt32();
        if (version != header.SaveVersion && version <= header.SaveVersion)
        {
            header.ParseState.CurrentEntitySaveVersion = (int) version;
            componentObject.EntitySaveVersion = (int) version;
        }

        var migrateFlag = reader.ReadUInt32();
        if (migrateFlag != 0)
        {
            componentObject.ShouldMigrateObjectRefsToPersistentFlag = migrateFlag;
        }
    }

    private void ReadEntityDataPackageAtBlockEndSave53(BinaryReader reader, Header header, int binarySize, long blockStart)
    {
        reader.BaseStream.Seek(blockStart + binarySize, SeekOrigin.Begin);
        if (reader.ReadInt32() != 0)
        {
            var dataPackage = UnrealFormatHelper.DeserializeDataPackageVersion(reader, _stringSerializer, HexSerializer.Instance);
            header.ParseState.CurrentLevelUE5Version = dataPackage.PackageFileVersion.UE5Version;
        }
        reader.BaseStream.Seek(blockStart, SeekOrigin.Begin);
    }

    private void ReadTrailingDataPackageVersionSave53(BinaryReader reader, Header header, ComponentObject componentObject, int perObjectSaveVersion)
    {
        if (perObjectSaveVersion < SerializeDataPackageVersionAndCustomVersions)
        {
            return;
        }

        if (reader.ReadInt32() == 0)
        {
            return;
        }

        componentObject.DataPackageVersion = UnrealFormatHelper.DeserializeDataPackageVersion(reader, _stringSerializer, HexSerializer.Instance);
    }

    private static void TryApplyUe5VersionFixSave53(BinaryReader reader, Header header)
    {
        if (header.ParseState.CurrentEntitySaveVersion >= 53
            && header.ParseState.CurrentLevelUE5Version == SaveParseState.DefaultUe5Version)
        {
            var probe = reader.ReadByte();
            reader.BaseStream.Seek(-1, SeekOrigin.Current);
            if (probe == 0)
            {
                header.ParseState.CurrentLevelUE5Version = SaveParseState.Ue5PropertyFormatVersion;
            }
        }
    }

    private void ConsumeMissingBytes(BinaryReader reader, long expectedPosition)
    {
        var missingBytes = expectedPosition - reader.BaseStream.Position;

        if (missingBytes > 0)
        {
            var hex = _hexSerializer.Deserialize(reader, missingBytes.ToInt());
            if (hex.Any(c => c != '\0'))
            {
                _logger.LogWarning("Missing bytes: {MissingBytes}", hex);
            }
        }
        else if (missingBytes < 0)
        {
            reader.BaseStream.Seek(missingBytes, SeekOrigin.Current);
        }
    }
}