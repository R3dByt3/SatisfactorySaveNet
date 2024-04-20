using System.Runtime.Serialization;
using System;

namespace SatisfactorySaveNet.Abstracts.Exceptions;

[Serializable]
public class BadReadException : SatisFactoryException
{
    public BadReadException()
    {
    }

    public BadReadException(string? message) : base(message)
    {
    }

    public BadReadException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected BadReadException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
