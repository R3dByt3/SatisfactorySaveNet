using SatisfactorySaveNet.Abstracts;

namespace SatisfactorySaveNet;

public class HexSerializer : IHexSerializer
{
    public static readonly HexSerializer Instance = new();

    public string Deserialize(BinaryReader reader, int length)
    {
        var hexChars = new char[length];

        for (int i = 0; i < length; i++)
        {
            char hexChar = (char)reader.ReadByte();
            hexChars[i] = hexChar;
        }

        return new string(hexChars.ToArray());
    }
}
