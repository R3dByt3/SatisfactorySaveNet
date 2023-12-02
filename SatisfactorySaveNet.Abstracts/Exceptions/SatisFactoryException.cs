using System.Runtime.Serialization;

namespace SatisfactorySaveNet.Abstracts.Exceptions;

[Serializable]
public abstract class SatisFactoryException : Exception
{
    protected SatisFactoryException()
    {
    }

    protected SatisFactoryException(string? message) : base(message)
    {
    }

    protected SatisFactoryException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected SatisFactoryException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}