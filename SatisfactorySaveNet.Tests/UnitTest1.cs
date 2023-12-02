using SatisfactorySaveNet.Abstracts;

namespace SatisfactorySaveNet.Tests;

[TestFixture]
public class Tests
{
    private readonly ISaveFileSerializer _serializer = SaveFileSerializer.Instance;

    [Test]
    public void SaveFileSerializer_ShouldDeserialize_WhenSaveIsValid()
    {
        var test = _serializer.Deserialize(@"C:\Users\marvi\AppData\Local\FactoryGame\Saved\SaveGames\5d66aaf3a97b48968049b2531bb6e6f8\Gen 5_autosave_0.sav");
        //var test = _serializer.Deserialize(@"C:\Users\marvi\AppData\Local\FactoryGame\Saved\SaveGames\5d66aaf3a97b48968049b2531bb6e6f8\Gen 5_autosave_0_CALCULATOR.sav");
    }
}