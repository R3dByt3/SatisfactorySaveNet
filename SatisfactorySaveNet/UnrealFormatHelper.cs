using SatisfactorySaveNet.Abstracts;
using SatisfactorySaveNet.Abstracts.Model;
using System.IO;

namespace SatisfactorySaveNet;

internal static class UnrealFormatHelper
{
    public static DataPackageVersion DeserializeDataPackageVersion(BinaryReader reader, IStringSerializer stringSerializer, IHexSerializer hexSerializer)
    {
        var saveObjectVersionDataVersion = reader.ReadInt32();
        var ue4Version = reader.ReadInt32();
        var ue5Version = reader.ReadInt32();
        var licenseeVersion = reader.ReadInt32();

        var engineVersion = new EngineVersion
        {
            Major = reader.ReadUInt16(),
            Minor = reader.ReadUInt16(),
            Patch = reader.ReadUInt16(),
            Changelist = reader.ReadUInt32(),
            Branch = stringSerializer.Deserialize(reader)
        };

        var customVersionCount = reader.ReadInt32();
        var customVersions = new CustomVersionEntry[customVersionCount];
        for (var i = 0; i < customVersionCount; i++)
        {
            customVersions[i] = new CustomVersionEntry
            {
                Key = hexSerializer.Deserialize(reader, 16),
                Version = reader.ReadInt32()
            };
        }

        return new DataPackageVersion
        {
            SaveObjectVersionDataVersion = saveObjectVersionDataVersion,
            PackageFileVersion = new PackageFileVersion
            {
                UE4Version = ue4Version,
                UE5Version = ue5Version
            },
            LicenseeVersion = licenseeVersion,
            EngineVersion = engineVersion,
            CustomVersionContainer = customVersions
        };
    }

    public static void ReadPackageName(BinaryReader reader, IStringSerializer stringSerializer)
    {
        if (reader.ReadInt32() == 0)
            return;

        _ = stringSerializer.Deserialize(reader);
        if (reader.ReadInt32() != 0)
        {
            _ = stringSerializer.Deserialize(reader);
            _ = stringSerializer.Deserialize(reader);
        }
    }

    public static (int A, int B, int C, int D)? ReadSparseGuid(BinaryReader reader)
    {
        var a = reader.ReadUInt32();
        var b = reader.ReadUInt32();
        var c = reader.ReadUInt32();
        var d = reader.ReadUInt32();

        if (a == 0 && b == 0 && c == 0 && d == 0)
            return null;

        return ((int)a, (int)b, (int)c, (int)d);
    }

    public static int ReadModeType(BinaryReader reader, IStringSerializer stringSerializer, IHexSerializer hexSerializer)
    {
        var modeType = reader.ReadInt32();
        if (modeType == 2)
        {
            _ = stringSerializer.Deserialize(reader);
            _ = stringSerializer.Deserialize(reader);
        }
        else if (modeType == 3)
        {
            _ = hexSerializer.Deserialize(reader, 9);
            _ = stringSerializer.Deserialize(reader);
            _ = stringSerializer.Deserialize(reader);
        }

        return modeType;
    }
}
