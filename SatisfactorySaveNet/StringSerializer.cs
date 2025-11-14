using CommunityToolkit.HighPerformance.Buffers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SatisfactorySaveNet.Abstracts;
using System;
using System.IO;
using System.Text;

namespace SatisfactorySaveNet;

public class StringSerializer : IStringSerializer
{
    public static readonly IStringSerializer Instance = new StringSerializer(NullLoggerFactory.Instance);
    private readonly ILogger<StringSerializer> _logger;

    public StringSerializer(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<StringSerializer>();
    }

    public string Deserialize(BinaryReader reader)
    {
        Span<char> chars = ReadCharArray(reader);
        var result = StringPool.Shared.GetOrAdd(chars.TrimEnd('\0'));
#if DEBUG
        var sf1 = new System.Diagnostics.StackTrace(true).GetFrame(1)!;
        var sf2 = new System.Diagnostics.StackTrace(true).GetFrame(2)!;
        _logger.LogInformation("DeserializeString {Value} - IsASCII {IsASCII} - File {File1} - Line {Line1} - File {File2} - Line {Line2}", result, result.All(char.IsAscii), sf1.GetFileName(), sf1.GetFileLineNumber(), sf2.GetFileName(), sf2.GetFileLineNumber());
#endif
        return result;
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