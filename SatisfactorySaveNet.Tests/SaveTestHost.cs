using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace SatisfactorySaveNet.Tests;

internal static class SaveTestHost
{
    public static ISaveFileSerializer CreateSerializer()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ISaveFileSerializer, SaveFileSerializer>();
        services.AddSingleton<IHeaderSerializer, HeaderSerializer>();
        services.AddSingleton<IChunkSerializer, ChunkSerializer>();
        services.AddSingleton<IBodySerializer, BodySerializer>();
        services.AddSingleton<IStringSerializer, StringSerializer>();
        services.AddSingleton<IHexSerializer, HexSerializer>();
        services.AddSingleton<IObjectHeaderSerializer, ObjectHeaderSerializer>();
        services.AddSingleton<IObjectReferenceSerializer, ObjectReferenceSerializer>();
        services.AddSingleton<IObjectSerializer, ObjectSerializer>();
        services.AddSingleton<IVectorSerializer, VectorSerializer>();
        services.AddSingleton<IPropertySerializer, PropertySerializer>();
        services.AddSingleton<IExtraDataSerializer, ExtraDataSerializer>();
        services.AddSingleton<ISoftObjectReferenceSerializer, SoftObjectReferenceSerializer>();
        services.AddSingleton<ILoggerFactory>(NullLoggerFactory.Instance);

        return services.BuildServiceProvider().GetRequiredService<ISaveFileSerializer>();
    }
}
