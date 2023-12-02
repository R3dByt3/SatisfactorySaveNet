using System.Runtime.Serialization;

namespace SatisfactorySaveNet.Abstracts.Exceptions;

[Serializable]
public class CorruptedSatisFactorySaveFileException : SatisFactoryException
{
    public CorruptedSatisFactorySaveFileException()
    {
    }

    public CorruptedSatisFactorySaveFileException(string? message) : base(message)
    {
    }

    public CorruptedSatisFactorySaveFileException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected CorruptedSatisFactorySaveFileException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}