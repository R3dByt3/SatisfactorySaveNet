using SatisfactorySaveNet.Abstracts.Model;
using System.IO;

namespace SatisfactorySaveNet.Abstracts;

public interface IChunkSerializer
{
    public ChunkInfo Deserialize(BinaryReader reader);
}