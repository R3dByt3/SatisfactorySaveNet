using Microsoft.IO;
using SatisfactorySaveNet.Abstracts;
using SatisfactorySaveNet.Abstracts.Exceptions;
using SatisfactorySaveNet.Abstracts.Model;
using System.IO.Compression;

namespace SatisfactorySaveNet;

public class SaveFileSerializer : ISaveFileSerializer
{
    public static readonly ISaveFileSerializer Instance = new SaveFileSerializer(HeaderSerializer.Instance, ChunkSerializer.Instance, BodySerializer.Instance);

    private const int BlockSize = 256 * 1024;
    private const int LargeBufferMultiple = 64 * 1024 * 1024;
    private const int MaxBufferSize = 128 * 1024 * 1024;

    private static readonly RecyclableMemoryStreamManager Manager = new(new(BlockSize, LargeBufferMultiple, MaxBufferSize, 100 * BlockSize, MaxBufferSize * 4));//BlockSize, LargeBufferMultiple, MaxBufferSize);

    static SaveFileSerializer()
    {
#if DEBUG
        //Manager.GenerateCallStacks = true;
#endif
        //Manager.AggressiveBufferReturn = true;
        //Manager.MaximumFreeLargePoolBytes = MaxBufferSize * 4;
        //Manager.MaximumFreeSmallPoolBytes = 100 * BlockSize;
    }

    private readonly IHeaderSerializer _headerSerializer;
    private readonly IChunkSerializer _chunkSerializer;
    private readonly IBodySerializer _bodySerializer;

    public SaveFileSerializer(IHeaderSerializer headerSerializer, IChunkSerializer chunkSerializer, IBodySerializer bodySerializer)
    {
        _headerSerializer = headerSerializer;
        _chunkSerializer = chunkSerializer;
        _bodySerializer = bodySerializer;
    }

    public SatisfactorySave Deserialize(string path)
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        return Deserialize(stream);
    }

    public SatisfactorySave Deserialize(byte[] data)
    {
        using var stream = Manager.GetStream(data);
        return Deserialize(stream);
    }

    public SatisfactorySave Deserialize(Stream stream)
    {
        if (stream.Length == 0)
            throw new CorruptedSatisFactorySaveFileException("Save file is empty");

        using var reader = new BinaryReader(stream);

        var header = _headerSerializer.Deserialize(reader);

        Body body;

        if (header.SaveVersion < 21)
        {
            body = _bodySerializer.Deserialize(reader, header);
        }
        else
        {
            using var buffer = Manager.GetStream();
            var uncompressedSize = 0L;

            while (stream.Position < stream.Length)
            {
                var chunkInfo = _chunkSerializer.Deserialize(reader); //0, 4, 8, 12

                if (chunkInfo.CompressedSize != ChunkInfo.MagicValue || chunkInfo.UncompressedSize != ChunkInfo.ChunkSize)
                    throw new CorruptedSatisFactorySaveFileException("Corrupted chunk was read");

                if (header.HeaderVersion >= 13)
                    _ = reader.ReadByte();

                var summary = _chunkSerializer.Deserialize(reader);  //16, 20, 24, 28

                var subChunk = _chunkSerializer.Deserialize(reader); //32, 36, 40, 44

                if (subChunk.UncompressedSize != summary.UncompressedSize)
                    throw new CorruptedSatisFactorySaveFileException("Corrupted sub chunk was read");

                //var startPosition = stream.Position;

                using var chunk = Manager.GetStream();
                chunk.Write(reader.ReadBytes(summary.CompressedSize));
                chunk.Seek(0, SeekOrigin.Begin);

                using (var zStream = new ZLibStream(chunk, CompressionMode.Decompress, true))
                {
                    zStream.CopyTo(buffer);
                }

                //stream.Position = startPosition + summary.CompressedSize;

                uncompressedSize += summary.UncompressedSize;
            }

            buffer.Position = 0;

            using var bufferReader = new BinaryReader(buffer);

            long dataLength;
            if (header.HeaderVersion >= 13)
                dataLength = bufferReader.ReadInt64();
            else
                dataLength = bufferReader.ReadInt32();

            var offset = header.HeaderVersion >= 13 ? 8 : 4;

            if (uncompressedSize != dataLength + offset)
                throw new CorruptedSatisFactorySaveFileException("Umcompressed size mismatch detected");

            body = _bodySerializer.Deserialize(bufferReader, header);
        }

        return new SatisfactorySave(header, body);
    }
}