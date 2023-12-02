using SatisfactorySaveNet.Abstracts;
using System.Text;

namespace SatisfactorySaveNet;

public class StringSerializer : IStringSerializer
{
    public static readonly IStringSerializer Instance = new StringSerializer();

    public string Deserialize(BinaryReader reader)
    {
        return new string(ReadCharArray(reader)).TrimEnd('\0');
    }

    private static char[] ReadCharArray(BinaryReader reader)
    {
        var count = reader.ReadInt32();
        if (count >= 0)
        {
            var bytes = reader.ReadBytes(count);
            return Encoding.UTF8.GetChars(bytes);
        }
        else
        {
            var bytes = reader.ReadBytes(count * -2);
            return Encoding.Unicode.GetChars(bytes);
        }
    }
}