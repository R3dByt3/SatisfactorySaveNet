using SatisfactorySaveNet.Abstracts;
using SatisfactorySaveNet.Abstracts.Model;

namespace SatisfactorySaveNet;

public class HeaderSerializer : IHeaderSerializer
{
    public static readonly IHeaderSerializer Instance = new HeaderSerializer(StringSerializer.Instance);

    private readonly IStringSerializer _stringSerializer;

    public HeaderSerializer(IStringSerializer stringSerializer)
    {
        _stringSerializer = stringSerializer;
    }

    public Header Deserialize(BinaryReader reader)
    {
        var header = new Header
        {
            HeaderVersion = reader.ReadInt32(),
            SaveVersion = reader.ReadInt32(),
            BuildVersion = reader.ReadInt32(),

            MapName = _stringSerializer.Deserialize(reader),
            MapOptions = _stringSerializer.Deserialize(reader),
            SessionName = _stringSerializer.Deserialize(reader),

            PlayedSeconds = reader.ReadInt32(),
            SaveDateTimeUtc = new DateTime(reader.ReadInt64(), DateTimeKind.Utc),
        };

        //ToDo: Set flag to inform about possible loss of information due to deprecated reader

        //if (header.HeaderVersion > SaveHeaderVersion.LatestVersion)
        //if (header.SaveVersion < FSaveCustomVersion.DROPPED_WireSpanFromConnnectionComponents || header.SaveVersion > FSaveCustomVersion.LatestVersion)

        if (header.HeaderVersion >= 5)
            header.SessionVisibility = reader.ReadSByte();

        if (header.HeaderVersion >= 7)
            header.EditorObjectVersion = reader.ReadInt32();

        if (header.HeaderVersion >= 8)
        {
            header.ModMetadata = _stringSerializer.Deserialize(reader);
            header.IsModdedSave = reader.ReadInt32() > 0;
        }

        if (header.HeaderVersion >= 10)
            header.SaveIdentifier = _stringSerializer.Deserialize(reader);

        if (header.HeaderVersion >= 13)
        {
            reader.BaseStream.Seek(28, SeekOrigin.Current);
            //byte[][] slices;
            //var pos = reader.BaseStream.Position;
            //var magicMatch = false;
            //var chunkMatch = false;
            //var compressedMatch = false;
            //var uncompressedMatch = false;
            //
            //do
            //{
            //    var bytes = reader.ReadBytes(sizeof(int) * 12);
            //    reader.BaseStream.Seek(-((sizeof(int) * 12) - 1), SeekOrigin.Current);
            //    slices = bytes.Chunk(sizeof(int)).ToArray();
            //    magicMatch = BitConverter.ToInt32(slices[0]) == ChunkInfo.MagicValue;
            //    chunkMatch = BitConverter.ToInt32(slices[2]) == ChunkInfo.ChunkSize;
            //    //compressedMatch = BitConverter.ToInt32(slices[4]) == BitConverter.ToInt32(slices[8]);
            //    //uncompressedMatch = BitConverter.ToInt32(slices[6]) == BitConverter.ToInt32(slices[10]);
            //} while (!(magicMatch && chunkMatch));
            //
            //reader.BaseStream.Seek(-1, SeekOrigin.Current);
            //
            //var count = reader.BaseStream.Position - pos;
            //
            //Console.WriteLine(count);
        }

        return header;
    }
}