using SatisfactorySaveNet.Abstracts.Model;

namespace SatisfactorySaveNet.Abstracts;

public class SaveParseContext
{
    public int LevelSaveVersion { get; set; }
    public int EntitySaveVersion { get; set; }
    public int LevelUE5Version { get; set; } = 1000;
    public DataPackageVersion? SaveDataPackageVersion { get; set; }
}
