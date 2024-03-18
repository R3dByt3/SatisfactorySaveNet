namespace SatisfactorySaveNet.Abstracts.Model;

public class ChunkInfo
{
    public const int MagicValue = unchecked((int) 0x9E2A83C1);
    public const int ChunkSize = 128 * 1024;

    public int CompressedSize { get; set; }
    public int CompressedOffset { get; set; }
    public int UncompressedSize { get; set; }
    public int UncompressedOffset { get; set; }
}