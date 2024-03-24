using SatisfactorySaveNet.Abstracts;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SatisfactorySaveNet.Tests;

[TestFixture]
public class Tests
{
    private readonly ISaveFileSerializer _serializer = SaveFileSerializer.Instance;

    [Test]
    public void SaveFileSerializer_ShouldDeserialize_WhenSaveIsValid()
    {
        //var test = _serializer.Deserialize(@"D:\_Downloads\5CDA4312000000000000000000000000.sav");
        var test = _serializer.Deserialize(@"C:\Users\marvi\AppData\Local\FactoryGame\Saved\SaveGames\5d66aaf3a97b48968049b2531bb6e6f8\Gen 5_autosave_0.sav");
        using var fileStream = System.IO.File.OpenWrite(@"N:\SCIM_small.json");
        JsonSerializer.Serialize(fileStream, test, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() }, IncludeFields = true, MaxDepth = 1024, WriteIndented = false });  
        //var test = _serializer.Deserialize(@"C:\Users\marvi\AppData\Local\FactoryGame\Saved\SaveGames\5d66aaf3a97b48968049b2531bb6e6f8\Gen 5_autosave_0_CALCULATOR.sav");
    }
}