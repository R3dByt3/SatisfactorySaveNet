using SatisfactorySaveNet.Abstracts.Model;
using System.IO;

namespace SatisfactorySaveNet.Abstracts;

public interface IObjectSerializer
{
    public ComponentObject Deserialize(BinaryReader reader, Header header, ComponentObject componentObject);
}