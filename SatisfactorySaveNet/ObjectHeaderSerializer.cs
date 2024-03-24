using SatisfactorySaveNet.Abstracts;
using SatisfactorySaveNet.Abstracts.Exceptions;
using SatisfactorySaveNet.Abstracts.Model;
using System.IO;

namespace SatisfactorySaveNet;

public class ObjectHeaderSerializer : IObjectHeaderSerializer
{
    public static readonly IObjectHeaderSerializer Instance = new ObjectHeaderSerializer(StringSerializer.Instance, VectorSerializer.Instance);

    private readonly IStringSerializer _stringSerializer;
    private readonly IVectorSerializer _vectorSerializer;

    public ObjectHeaderSerializer(IStringSerializer stringSerializer, IVectorSerializer vectorSerializer)
    {
        _stringSerializer = stringSerializer;
        _vectorSerializer = vectorSerializer;
    }

    public ComponentObject Deserialize(BinaryReader reader)
    {
        var type = reader.ReadInt32();
        return type switch
        {
            ComponentObject.TypeID => DeserializeComponentHeader(reader),
            ActorObject.TypeID => DeserializeActorHeader(reader),
            _ => throw new CorruptedSatisFactorySaveFileException("Encountered unknown object type")
        };
    }

    private ActorObject DeserializeActorHeader(BinaryReader reader)
    {
        var typePath = _stringSerializer.Deserialize(reader);
        var rootObject = _stringSerializer.Deserialize(reader);
        var instanceName = _stringSerializer.Deserialize(reader);
        var needTransform = reader.ReadInt32();
        var rotation = _vectorSerializer.DeserializeVec4(reader);
        var position = _vectorSerializer.DeserializeVec3(reader);
        var scale = _vectorSerializer.DeserializeVec3(reader);
        var wasPlacedInLevel = reader.ReadInt32();

        return new ActorObject
        {
            TypePath = typePath,
            RootObject = rootObject,
            InstanceName = instanceName,
            NeedTransform = needTransform,
            Rotation = rotation,
            Position = position,
            Scale = scale,
            PlacedInLevel = wasPlacedInLevel
        };
    }

    private ComponentObject DeserializeComponentHeader(BinaryReader reader)
    {
        var typePath = _stringSerializer.Deserialize(reader);
        var rootObject = _stringSerializer.Deserialize(reader);
        var instanceName = _stringSerializer.Deserialize(reader);
        var parentActorName = _stringSerializer.Deserialize(reader);

        return new ComponentObject
        {
            TypePath = typePath,
            RootObject = rootObject,
            InstanceName = instanceName,
            ParentActorName = parentActorName
        };
    }
}