using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SatisfactorySaveNet.Abstracts;
using SatisfactorySaveNet.Abstracts.Maths.Data;
using SatisfactorySaveNet.Abstracts.Maths.Vector;
using System.IO;

namespace SatisfactorySaveNet;

public class VectorSerializer : IVectorSerializer
{
    public static readonly IVectorSerializer Instance = new VectorSerializer(NullLoggerFactory.Instance);

    private readonly ILogger<VectorSerializer> _logger;

    public VectorSerializer(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<VectorSerializer>() ?? NullLogger<VectorSerializer>.Instance;
    }

    public Vector4 DeserializeVec4(BinaryReader reader)
    {
        var x = reader.ReadSingle();
        var y = reader.ReadSingle();
        var z = reader.ReadSingle();
        var w = reader.ReadSingle();

        return new Vector4(x, y, z, w);
    }

    public Vector4 DeserializeVec4I(BinaryReader reader)
    {
        var x = reader.ReadInt32();
        var y = reader.ReadInt32();
        var z = reader.ReadInt32();
        var w = reader.ReadInt32();

        var vector4I = new Vector4I(x, y, z, w);
        return vector4I;
    }

    public Vector4D DeserializeVec4D(BinaryReader reader)
    {
        var x = reader.ReadDouble();
        var y = reader.ReadDouble();
        var z = reader.ReadDouble();
        var w = reader.ReadDouble();

        var vector4D = new Vector4D(x, y, z, w);
//#if DEBUG
//        var sf1 = new System.Diagnostics.StackTrace(true).GetFrame(1)!;
//        var sf2 = new System.Diagnostics.StackTrace(true).GetFrame(2)!;
//        _logger.LogInformation("DeserializeVec4D - {Vector4D} - ContainsScientificNumber {ContainsScientificNumber} - File {File1} - Line {Line1} - File {File2} - Line {Line2}", vector4D, ContainsScientificNumber(x, y, z, w), sf1.GetFileName(), sf1.GetFileLineNumber(), sf2.GetFileName(), sf2.GetFileLineNumber());
//#endif
        return vector4D;
    }

    public Vector3 DeserializeVec3(BinaryReader reader)
    {
        var x = reader.ReadSingle();
        var y = reader.ReadSingle();
        var z = reader.ReadSingle();

        var vector3 = new Vector3(x, y, z);
//#if DEBUG
//        var sf1 = new System.Diagnostics.StackTrace(true).GetFrame(1)!;
//        var sf2 = new System.Diagnostics.StackTrace(true).GetFrame(2)!;
//        var csn = ContainsScientificNumber(x, y, z);
//        _logger.LogInformation("DeserializeVec3 - {Vector3} - ContainsScientificNumber {ContainsScientificNumber} - File {File1} - Line {Line1} - File {File2} - Line {Line2}", vector3, csn, sf1.GetFileName(), sf1.GetFileLineNumber(), sf2.GetFileName(), sf2.GetFileLineNumber());
//#endif
        return vector3;
    }

    public Vector3D DeserializeVec3D(BinaryReader reader)
    {
        var x = reader.ReadDouble();
        var y = reader.ReadDouble();
        var z = reader.ReadDouble();

        var vector3D = new Vector3D(x, y, z);
//#if DEBUG
//        var sf1 = new System.Diagnostics.StackTrace(true).GetFrame(1)!;
//        var sf2 = new System.Diagnostics.StackTrace(true).GetFrame(2)!;
//        var csn = ContainsScientificNumber(x, y, z);
//        _logger.LogInformation("DeserializeVec3D - {Vector3D} - ContainsScientificNumber {ContainsScientificNumber} - File {File1} - Line {Line1} - File {File2} - Line {Line2}", vector3D, csn, sf1.GetFileName(), sf1.GetFileLineNumber(), sf2.GetFileName(), sf2.GetFileLineNumber());
//#endif
        return vector3D;
    }

    public Vector2 DeserializeVec2(BinaryReader reader)
    {
        var x = reader.ReadSingle();
        var y = reader.ReadSingle();

        var vector2 = new Vector2(x, y);
//#if DEBUG
//        var sf1 = new System.Diagnostics.StackTrace(true).GetFrame(1)!;
//        var sf2 = new System.Diagnostics.StackTrace(true).GetFrame(2)!;
//        _logger.LogInformation("DeserializeVec2 - {Vector2} - ContainsScientificNumber {ContainsScientificNumber} - File {File1} - Line {Line1} - File {File2} - Line {Line2}", vector2, ContainsScientificNumber(x, y), sf1.GetFileName(), sf1.GetFileLineNumber(), sf2.GetFileName(), sf2.GetFileLineNumber());
//#endif
        return vector2;
    }

    public Vector2I DeserializeVec2I(BinaryReader reader)
    {
        var x = reader.ReadInt32();
        var y = reader.ReadInt32();

        var vector2I = new Vector2I(x, y);
//#if DEBUG
//        var sf1 = new System.Diagnostics.StackTrace(true).GetFrame(1)!;
//        var sf2 = new System.Diagnostics.StackTrace(true).GetFrame(2)!;
//        _logger.LogInformation("DeserializeVec2I - {Vector2I} - - File {File1} - Line {Line1} - File {File2} - Line {Line2}", vector2I, sf1.GetFileName(), sf1.GetFileLineNumber(), sf2.GetFileName(), sf2.GetFileLineNumber());
//#endif
        return vector2I;
    }

    public Vector2D DeserializeVec2D(BinaryReader reader)
    {
        var x = reader.ReadDouble();
        var y = reader.ReadDouble();

        var vector2D = new Vector2D(x, y);
//#if DEBUG
//        var sf1 = new System.Diagnostics.StackTrace(true).GetFrame(1)!;
//        var sf2 = new System.Diagnostics.StackTrace(true).GetFrame(2)!;
//        _logger.LogInformation("DeserializeVec2D - {Vector2D} - ContainsScientificNumber {ContainsScientificNumber} - File {File1} - Line {Line1} - File {File2} - Line {Line2}", vector2D, ContainsScientificNumber(x, y), sf1.GetFileName(), sf1.GetFileLineNumber(), sf2.GetFileName(), sf2.GetFileLineNumber());
//#endif
        return vector2D;
    }

    public Quaternion DeserializeQuaternion(BinaryReader reader)
    {
        var x = reader.ReadSingle();
        var y = reader.ReadSingle();
        var z = reader.ReadSingle();
        var w = reader.ReadSingle();

        var quaternion = new Quaternion(x, y, z, w);
//#if DEBUG
//        var sf1 = new System.Diagnostics.StackTrace(true).GetFrame(1)!;
//        var sf2 = new System.Diagnostics.StackTrace(true).GetFrame(2)!;
//        _logger.LogInformation("DeserializeQuaternion - {Quaternion} - ContainsScientificNumber {ContainsScientificNumber} - File {File1} - Line {Line1} - File {File2} - Line {Line2}", quaternion, ContainsScientificNumber(x, y, z, w), sf1.GetFileName(), sf1.GetFileLineNumber(), sf2.GetFileName(), sf2.GetFileLineNumber());
//#endif
        return quaternion;
    }

    public QuaternionD DeserializeQuaternionD(BinaryReader reader)
    {
        var x = reader.ReadDouble();
        var y = reader.ReadDouble();
        var z = reader.ReadDouble();
        var w = reader.ReadDouble();

        var quaternionD = new QuaternionD(x, y, z, w);
//#if DEBUG
//        var sf1 = new System.Diagnostics.StackTrace(true).GetFrame(1)!;
//        var sf2 = new System.Diagnostics.StackTrace(true).GetFrame(2)!;
//        _logger.LogInformation("DeserializeQuaternionD - {QuaternionD} - ContainsScientificNumber {ContainsScientificNumber} - File {File1} - Line {Line1} - File {File2} - Line {Line2}", quaternionD, ContainsScientificNumber(x, y, z, w), sf1.GetFileName(), sf1.GetFileLineNumber(), sf2.GetFileName(), sf2.GetFileLineNumber());
//#endif
        return quaternionD;
    }

    public Vector3I DeserializeVec3I(BinaryReader reader)
    {
        var x = reader.ReadInt32();
        var y = reader.ReadInt32();
        var z = reader.ReadInt32();

        var vector3I = new Vector3I(x, y, z);
//#if DEBUG
//        var sf1 = new System.Diagnostics.StackTrace(true).GetFrame(1)!;
//        var sf2 = new System.Diagnostics.StackTrace(true).GetFrame(2)!;
//        _logger.LogInformation("DeserializeVector3I - {Vector3I} - File {File1} - Line {Line1} - File {File2} - Line {Line2}", vector3I, sf1.GetFileName(), sf1.GetFileLineNumber(), sf2.GetFileName(), sf2.GetFileLineNumber());
//#endif
        return vector3I;
    }

    public Vector4I DeserializeVec4BAs4I(BinaryReader reader)
    {
        var x = reader.ReadSByte();
        var y = reader.ReadSByte();
        var z = reader.ReadSByte();
        var w = reader.ReadSByte();

        var vector4I = new Vector4I(x, y, z, w);
//#if DEBUG
//        var sf1 = new System.Diagnostics.StackTrace(true).GetFrame(1)!;
//        var sf2 = new System.Diagnostics.StackTrace(true).GetFrame(2)!;
//        _logger.LogInformation("DeserializeVector4I - {Vector4I} - File {File1} - Line {Line1} - File {File2} - Line {Line2}", vector4I, sf1.GetFileName(), sf1.GetFileLineNumber(), sf2.GetFileName(), sf2.GetFileLineNumber());
//#endif
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

    //#if DEBUG
    //    private static bool ContainsScientificNumber(params double[] values)
    //    {
    //        foreach (var number in values)
    //        {
    //            if (number.ToString().Contains('e', System.StringComparison.OrdinalIgnoreCase))
    //                return true;
    //        }
    //
    //        return false;
    //    }
    //    private static bool ContainsScientificNumber(params float[] values)
    //    {
    //        foreach (var number in values)
    //        {
    //            if (number.ToString().Contains('e', System.StringComparison.OrdinalIgnoreCase))
    //                return true;
    //        }
    //
    //        return false;
    //    }
    //#endif
}