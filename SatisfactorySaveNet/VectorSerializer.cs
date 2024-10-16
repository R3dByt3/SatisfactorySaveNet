using SatisfactorySaveNet.Abstracts;
using SatisfactorySaveNet.Abstracts.Maths.Data;
using SatisfactorySaveNet.Abstracts.Maths.Vector;
using System.IO;

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
    public Vector4D DeserializeVec4D(BinaryReader reader)
    {
        var x = reader.ReadDouble();
        var y = reader.ReadDouble();
        var z = reader.ReadDouble();
        var w = reader.ReadDouble();

        return new Vector4D(x, y, z, w);
    }

    public Vector3 DeserializeVec3(BinaryReader reader)
    {
        var x = reader.ReadSingle();
        var y = reader.ReadSingle();
        var z = reader.ReadSingle();

        return new Vector3(x, y, z);
    }

    public Vector3D DeserializeVec3D(BinaryReader reader)
    {
        var x = reader.ReadDouble();
        var y = reader.ReadDouble();
        var z = reader.ReadDouble();

        return new Vector3D(x, y, z);
    }

    public Vector2 DeserializeVec2(BinaryReader reader)
    {
        var x = reader.ReadSingle();
        var y = reader.ReadSingle();

        return new Vector2(x, y);
    }

    public Vector2I DeserializeVec2I(BinaryReader reader)
    {
        var x = reader.ReadInt32();
        var y = reader.ReadInt32();

        return new Vector2I(x, y);
    }

    public Vector2D DeserializeVec2D(BinaryReader reader)
    {
        var x = reader.ReadDouble();
        var y = reader.ReadDouble();

        return new Vector2D(x, y);
    }

    public Quaternion DeserializeQuaternion(BinaryReader reader)
    {
        var x = reader.ReadSingle();
        var y = reader.ReadSingle();
        var z = reader.ReadSingle();
        var w = reader.ReadSingle();

        return new Quaternion(x, y, z, w);
    }

    public QuaternionD DeserializeQuaternionD(BinaryReader reader)
    {
        var x = reader.ReadDouble();
        var y = reader.ReadDouble();
        var z = reader.ReadDouble();
        var w = reader.ReadDouble();

        return new QuaternionD(x, y, z, w);
    }

    public Vector3I DeserializeVec3I(BinaryReader reader)
    {
        var x = reader.ReadInt32();
        var y = reader.ReadInt32();
        var z = reader.ReadInt32();

        var vector3I = new Vector3I(x, y, z);
        return vector3I;
    }

    public Vector4I DeserializeVec4BAs4I(BinaryReader reader)
    {
        var x = reader.ReadSByte();
        var y = reader.ReadSByte();
        var z = reader.ReadSByte();
        var w = reader.ReadSByte();

        var vector4I = new Vector4I(x, y, z, w);
        return vector4I;
    }

    public Color4 DeserializeColor4(BinaryReader reader)
    {
        var r = reader.ReadSingle();
        var g = reader.ReadSingle();
        var b = reader.ReadSingle();
        var a = reader.ReadSingle();

        var color4 = new Color4(r, g, b, a);

        return color4;
    }
}