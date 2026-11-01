using SatisfactorySaveNet.Abstracts;
using SatisfactorySaveNet.Abstracts.Model;
using System;
using System.IO;

namespace SatisfactorySaveNet;

public class DataPackageVersionSerializer : IDataPackageVersionSerializer
{
    public static readonly IDataPackageVersionSerializer Instance = new DataPackageVersionSerializer(StringSerializer.Instance);

    private readonly IStringSerializer _stringSerializer;

    public DataPackageVersionSerializer(IStringSerializer stringSerializer)
    {
        _stringSerializer = stringSerializer;
    }

    public DataPackageVersion Deserialize(BinaryReader reader)
    {
        var saveObjectVersionDataVersion = reader.ReadInt32();
        var ue4Version = reader.ReadInt32();
        var ue5Version = reader.ReadInt32();
        var licenseeVersion = reader.ReadInt32();
        var inMajor = reader.ReadUInt16();
        var inMinor = reader.ReadUInt16();
        var inPatch = reader.ReadUInt16();
        var inChangelist = reader.ReadUInt32();
        var inBranch = _stringSerializer.Deserialize(reader);

        var nrCustomVersions = reader.ReadInt32();
        var customVersions = new CustomVersion[nrCustomVersions];

        for (var i = 0; i < nrCustomVersions; i++)
        {
            customVersions[i] = new CustomVersion
            {
                Key = new Guid(reader.ReadBytes(16)),
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
            EngineVersion = new EngineVersion
            {
                InMajor = inMajor,
                InMinor = inMinor,
                InPatch = inPatch,
                InChangelist = inChangelist,
                InBranch = inBranch
            },
            CustomVersionContainer = customVersions
        };
    }
}
