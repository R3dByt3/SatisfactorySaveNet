using SatisfactorySaveNet.Abstracts.Model;

namespace SatisfactorySaveNet.Abstracts;

public interface IBodySerializer
{
    public Body Deserialize(BinaryReader reader, Header header);
}