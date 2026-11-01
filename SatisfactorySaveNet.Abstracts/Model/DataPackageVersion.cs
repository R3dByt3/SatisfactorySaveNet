using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model;

public class DataPackageVersion
{
    public int SaveObjectVersionDataVersion { get; set; }
    public PackageFileVersion PackageFileVersion { get; set; } = new();
    public int LicenseeVersion { get; set; }
    public EngineVersion EngineVersion { get; set; } = new();
    public ICollection<CustomVersion> CustomVersionContainer { get; set; } = [];
}
