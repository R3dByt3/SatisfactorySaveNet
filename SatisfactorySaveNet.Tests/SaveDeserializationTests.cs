namespace SatisfactorySaveNet.Tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public sealed class SaveDeserializationTests
{
    private static IEnumerable<string> AllSaveFiles()
    {
        var savesDir = SaveFixture.SavesDirectory;
        if (!Directory.Exists(savesDir))
        {
            yield break;
        }

        foreach (var file in Directory.GetFiles(savesDir, "*.sav"))
        {
            yield return file;
        }
    }

    [Test]
    [TestCaseSource(nameof(AllSaveFiles))]
    public void Deserialize_DoesNotThrow_ForBundledSave(string path)
    {
        var serializer = SaveTestHost.CreateSerializer();
        var act = () => serializer.Deserialize(path);
        act.Should().NotThrow();
    }

    [Test]
    public void Deserialize_ReturnsBody_ForModernSave()
    {
        var path = Path.Combine(SaveFixture.SavesDirectory, "FreshStartU8001-vehicles-2.sav");
        var save = SaveTestHost.CreateSerializer().Deserialize(path);

        save.Header.SaveVersion.Should().Be(42);
        save.Body.Should().NotBeNull();
        save.Body!.BodyType.Should().Be(BodyConstraint.V8);
    }

}
