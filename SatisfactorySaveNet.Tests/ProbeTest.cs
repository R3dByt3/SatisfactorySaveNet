namespace SatisfactorySaveNet.Tests;

[TestFixture]
public sealed class ProbeTest
{
    [Test]
    [Explicit("Run locally to print save statistics for fixture maintenance")]
    public void PrintAllSaveStatistics()
    {
        var serializer = SaveTestHost.CreateSerializer();
        var savesDir = Path.Combine(TestContext.CurrentContext.TestDirectory, "Saves");

        foreach (var path in Directory.GetFiles(savesDir, "*.sav"))
        {
            var save = serializer.Deserialize(path);
            var stats = SaveAnalysis.Analyze(save);
            Console.WriteLine($"{Path.GetFileName(path)}|{save.Header.SaveVersion}|{save.Header.SessionName}|{stats}");
        }
    }
}
