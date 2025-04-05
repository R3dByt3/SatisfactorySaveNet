using SatisfactorySaveNet.Abstracts.Model;
using System.IO;

namespace SatisfactorySaveNet.Abstracts;

public interface IObjectHeaderSerializer
{
    public ComponentObject Deserialize(BinaryReader reader, int? saveVersion);
}