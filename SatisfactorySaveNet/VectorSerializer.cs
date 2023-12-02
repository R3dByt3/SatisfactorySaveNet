using SatisfactorySaveNet.Abstracts;
using System.Numerics;

namespace SatisfactorySaveNet;

public class VectorSerializer : IVectorSerializer
{
    public static readonly IVectorSerializer Instance = new VectorSerializer();

    public Vector4 DeserializeVec4(BinaryReader reader)
    {
        var x = reader.ReadSingle();
        var y = reader.ReadSingle();
        var z = reader.ReadSingle();
        var w = reader.ReadSingle();

        return new Vector4(x, y, z, w);
    }

    public Vector3 DeserializeVec3(BinaryReader reader)
    {
        var x = reader.ReadSingle();
        var y = reader.ReadSingle();
        var z = reader.ReadSingle();

        return new Vector3(x, y, z);
    }

    public Vector2 DeserializeVec2(BinaryReader reader)
    {
        var x = reader.ReadSingle();
        var y = reader.ReadSingle();

        return new Vector2(x, y);
    }

    public Quaternion DeserializeQuaternion(BinaryReader reader)
    {
        var x = reader.ReadSingle();
        var y = reader.ReadSingle();
        var z = reader.ReadSingle();
        var w = reader.ReadSingle();

        return new Quaternion(x, y, z, w);
    }
}