using CommunityToolkit.HighPerformance.Buffers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SatisfactorySaveNet.Abstracts;
using System;
using System.IO;
#if DEBUG
using System.Linq;
#endif
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
        var encodingIdentifier = reader.ReadInt32();

        long expectedBytes;
        Encoding encoding;
        if (encodingIdentifier >= 0)
        {
            expectedBytes = encodingIdentifier;
            encoding = Encoding.UTF8;
        }
        else
        {
            expectedBytes = encodingIdentifier * -2L;
            encoding = Encoding.Unicode;
        }

        try
        {
            var stream = reader.BaseStream;
            if (stream.CanSeek)
            {
                var remaining = stream.Length - stream.Position;
                if (expectedBytes > remaining || expectedBytes < 0)
                {
                    return [];
                }
            }
        }
        catch
        {
            // Ignore any errors while checking stream state and fall back to a safe read below.
        }

        var bytes = reader.ReadBytes((int)expectedBytes);
        return bytes.Length != expectedBytes ?  [] : encoding.GetChars(bytes);
    }
}