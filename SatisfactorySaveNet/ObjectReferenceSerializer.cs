using SatisfactorySaveNet.Abstracts;
using SatisfactorySaveNet.Abstracts.Model;

namespace SatisfactorySaveNet;

public class ObjectReferenceSerializer : IObjectReferenceSerializer
{
    public static readonly IObjectReferenceSerializer Instance = new ObjectReferenceSerializer(StringSerializer.Instance);

    private readonly IStringSerializer _stringSerializer;

    public ObjectReferenceSerializer(IStringSerializer stringSerializer)
    {
        _stringSerializer = stringSerializer;
    }

    public ObjectReference Deserialize(BinaryReader reader)
    {
        var levelName = _stringSerializer.Deserialize(reader);
        var pathName = _stringSerializer.Deserialize(reader);

        return new ObjectReference
        {
            LevelName = levelName,
            PathName = pathName
        };
    }
}