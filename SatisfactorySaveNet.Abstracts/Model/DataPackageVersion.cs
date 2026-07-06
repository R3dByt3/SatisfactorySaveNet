using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model;

public class DataPackageVersion
{
    public int SaveObjectVersionDataVersion { get; set; }

    public required PackageFileVersion PackageFileVersion { get; set; }

    public int LicenseeVersion { get; set; }

    public required EngineVersion EngineVersion { get; set; }

    public ICollection<CustomVersionEntry> CustomVersionContainer { get; set; } = [];
}

public class PackageFileVersion
{
    public int UE4Version { get; set; }

    public int UE5Version { get; set; }
}

public class EngineVersion
{
    public ushort Major { get; set; }

    public ushort Minor { get; set; }

    public ushort Patch { get; set; }

    public uint Changelist { get; set; }

    public string Branch { get; set; } = string.Empty;
}

public class CustomVersionEntry
{
    public required string Key { get; set; }

    public int Version { get; set; }
}
