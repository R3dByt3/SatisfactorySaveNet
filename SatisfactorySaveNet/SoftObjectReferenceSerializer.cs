using SatisfactorySaveNet.Abstracts;
using SatisfactorySaveNet.Abstracts.Model;
using System.IO;

namespace SatisfactorySaveNet;

public class SoftObjectReferenceSerializer : ISoftObjectReferenceSerializer
{
    public static readonly ISoftObjectReferenceSerializer Instance = new SoftObjectReferenceSerializer(StringSerializer.Instance);

    private readonly IStringSerializer _stringSerializer;

    public SoftObjectReferenceSerializer(IStringSerializer stringSerializer)
    {
        _stringSerializer = stringSerializer;
    }

    public SoftObjectReference Deserialize(BinaryReader reader)
    {
        var levelName = _stringSerializer.Deserialize(reader);
        var pathName = _stringSerializer.Deserialize(reader);
        var unknown1 = _stringSerializer.Deserialize(reader);

        return new SoftObjectReference
        {
            LevelName = levelName,
            PathName = pathName,
            Unknown1 = unknown1
        };
    }
}