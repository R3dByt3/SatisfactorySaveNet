using SatisfactorySaveNet.Abstracts;
using SatisfactorySaveNet.Abstracts.Exceptions;
using SatisfactorySaveNet.Abstracts.Model;
using System.IO;

namespace SatisfactorySaveNet;

public class ObjectHeaderSerializer : IObjectHeaderSerializer
{
    public static readonly IObjectHeaderSerializer Instance = new ObjectHeaderSerializer(StringSerializer.Instance, VectorSerializer.Instance, ObjectReferenceSerializer.Instance);

    private readonly IStringSerializer _stringSerializer;
    private readonly IVectorSerializer _vectorSerializer;
    private readonly IObjectReferenceSerializer _objectReferenceSerializer;

    public ObjectHeaderSerializer(IStringSerializer stringSerializer, IVectorSerializer vectorSerializer, IObjectReferenceSerializer objectReferenceSerializer)
    {
        _stringSerializer = stringSerializer;
        _vectorSerializer = vectorSerializer;
        _objectReferenceSerializer = objectReferenceSerializer;
    }

    public ComponentObject Deserialize(BinaryReader reader, int? saveVersion)
    {
        var type = reader.ReadInt32();
        return type switch
        {
            ComponentObject.TypeID => DeserializeComponentHeader(reader, saveVersion),
            ActorObject.TypeID => DeserializeActorHeader(reader, saveVersion),
            _ => throw new CorruptedSatisFactorySaveFileException("Encountered unknown object type")
        };
    }

    private ActorObject DeserializeActorHeader(BinaryReader reader, int? saveVersion)
    {
        var typePath = _stringSerializer.Deserialize(reader);
        var objectReference = _objectReferenceSerializer.Deserialize(reader);
        
        uint? flags = null;
        if (saveVersion >= 51)
            flags = reader.ReadUInt32();

        var needTransform = reader.ReadInt32();
        var rotation = _vectorSerializer.DeserializeVec4(reader);
        var position = _vectorSerializer.DeserializeVec3(reader);
        var scale = _vectorSerializer.DeserializeVec3(reader);
        var wasPlacedInLevel = reader.ReadInt32();

        return new ActorObject
        {
            TypePath = typePath,
            ObjectReference = objectReference,
            NeedTransform = needTransform,
            Rotation = rotation,
            Position = position,
            Scale = scale,
            PlacedInLevel = wasPlacedInLevel,
            Flags = flags
        };
    }

    private ComponentObject DeserializeComponentHeader(BinaryReader reader, int? saveVersion)
    {
        var typePath = _stringSerializer.Deserialize(reader);
        var objectReference = _objectReferenceSerializer.Deserialize(reader);

        uint? flags = null;
        if (saveVersion >= 51)
            flags = reader.ReadUInt32();

        var parentActorName = _stringSerializer.Deserialize(reader);

        return new ComponentObject
        {
            TypePath = typePath,
            ObjectReference = objectReference,
            ParentActorName = parentActorName,
            Flags = flags
        };
    }
}