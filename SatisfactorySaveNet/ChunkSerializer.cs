using SatisfactorySaveNet.Abstracts;
using SatisfactorySaveNet.Abstracts.Model;
using System.IO;

namespace SatisfactorySaveNet;

public class ChunkSerializer : IChunkSerializer
{
    public static readonly IChunkSerializer Instance = new ChunkSerializer();

    public ChunkInfo Deserialize(BinaryReader reader)
    {
        var compressedSize = reader.ReadInt32();
        var compressedOffset = reader.ReadInt32();
        var uncompressedSize = reader.ReadInt32();
        var uncompressedOffset = reader.ReadInt32();

        return new ChunkInfo
        {
            CompressedSize = compressedSize,
            CompressedOffset = compressedOffset,
            UncompressedSize = uncompressedSize,
            UncompressedOffset = uncompressedOffset
        };
    }
}