using SatisfactorySaveNet.Abstracts.Model;

namespace SatisfactorySaveNet.Abstracts;

public interface IChunkSerializer
{
    public ChunkInfo Deserialize(BinaryReader reader);
}