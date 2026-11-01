using SatisfactorySaveNet.Abstracts.Model;
using System.IO;

namespace SatisfactorySaveNet.Abstracts;

public interface IDataPackageVersionSerializer
{
    public DataPackageVersion Deserialize(BinaryReader reader);
}
