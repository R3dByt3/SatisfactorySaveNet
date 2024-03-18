using SatisfactorySaveNet.Abstracts.Model;
using System.IO;

namespace SatisfactorySaveNet.Abstracts;

public interface IHeaderSerializer
{
    public Header Deserialize(BinaryReader reader);
}