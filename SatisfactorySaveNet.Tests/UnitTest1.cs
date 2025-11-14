using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SatisfactorySaveNet.Abstracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SatisfactorySaveNet.Tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class Tests
{
    private readonly IServiceCollection _services = new ServiceCollection();
    private ISaveFileSerializer _serializer = null!;
    private IServiceProvider _serviceProvider = null!;

    [OneTimeSetUp]
    public void Setup()
    {
        _services.AddSingleton<ISaveFileSerializer, SaveFileSerializer>();
        _services.AddSingleton<IHeaderSerializer, HeaderSerializer>();
        _services.AddSingleton<IChunkSerializer, ChunkSerializer>();
        _services.AddSingleton<IBodySerializer, BodySerializer>();
        _services.AddSingleton<IStringSerializer, StringSerializer>();
        _services.AddSingleton<IHexSerializer, HexSerializer>();
        _services.AddSingleton<IObjectHeaderSerializer, ObjectHeaderSerializer>();
        _services.AddSingleton<IObjectReferenceSerializer, ObjectReferenceSerializer>();
        _services.AddSingleton<IObjectSerializer, ObjectSerializer>();
        _services.AddSingleton<IVectorSerializer, VectorSerializer>();
        _services.AddSingleton<IPropertySerializer, PropertySerializer>();
        _services.AddSingleton<IExtraDataSerializer, ExtraDataSerializer>();
        _services.AddSingleton<ISoftObjectReferenceSerializer, SoftObjectReferenceSerializer>();

        var testConfiguration = new Dictionary<string, string?>
        {
            {"Seq:ServerUrl", "http://localhost:5341"},
            {"Seq:ApiKey", "5MFmwJZ6hTb5jPblKjNF"},
            {"Seq:MinimumLevel", "Trace"},
            {"Seq:LevelOverride:Microsoft", "Warning"}
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(testConfiguration)
            .Build();

        //_services.AddSingleton<ILoggerFactory>(_ => NullLoggerFactory.Instance);
        _services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.SetMinimumLevel(LogLevel.Trace);
            loggingBuilder.AddSeq(configuration.GetSection("Seq"));
        });
        _serviceProvider = _services.BuildServiceProvider();
        _serializer = _serviceProvider.GetRequiredService<ISaveFileSerializer>();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        (_serviceProvider as IDisposable)?.Dispose();
    }

    [Test]
    public void SaveFileSerializer_ShouldDeserialize_WhenSaveIsValid()
    {
        //var test = _serializer.Deserialize(@"D:\_Downloads\5CDA4312000000000000000000000000.sav");
        var test = _serializer.Deserialize(@"C:\Users\marvi\AppData\Local\FactoryGame\Saved\SaveGames\5d66aaf3a97b48968049b2531bb6e6f8\Rails_autosave_2.sav");
        using var fileStream = System.IO.File.OpenWrite(@"N:\SCIM_small.json");
        JsonSerializer.Serialize(fileStream, test, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() }, IncludeFields = true, MaxDepth = 1024, WriteIndented = false });  
        //var test = _serializer.Deserialize(@"C:\Users\marvi\AppData\Local\FactoryGame\Saved\SaveGames\5d66aaf3a97b48968049b2531bb6e6f8\Gen 5_autosave_0_CALCULATOR.sav");
    }

    private static IEnumerable<TestCaseData> Files()
    {
        foreach (var file in Directory.GetFiles(@"/mnt/data/nextcloud/TMP/", "*.sav"))
        {
            yield return new TestCaseData(file);
        }
        foreach (var file in Directory.GetFiles(@"/mnt/data/tmp/sf/", "*.sav"))
        {
            yield return new TestCaseData(file);
        }
        foreach (var file in Directory.GetFiles(@"/home/marvin/Games/epic-games-store/drive_c/users/marvin/AppData/Local/FactoryGame/Saved/SaveGames/5d66aaf3a97b48968049b2531bb6e6f8/", "*.sav"))
        {
            yield return new TestCaseData(file);
        }
        foreach (var file in Directory.GetFiles(@"/home/marvin/Games/epic-games-store/drive_c/users/marvin/AppData/Local/FactoryGame/Saved/SaveGames/76561198023947483/", "*.sav"))
        {
            yield return new TestCaseData(file);
        }
    }

    [Test]
    [TestCaseSource(nameof(Files))]
    public void ReadFiles(string path)
    {
        var act = () => _serializer.Deserialize(path);
        act.Should().NotThrow();
    }
}