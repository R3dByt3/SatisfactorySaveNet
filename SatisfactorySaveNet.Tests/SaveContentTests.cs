namespace SatisfactorySaveNet.Tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public sealed class SaveContentTests
{
    private static IEnumerable<SaveFixture> Fixtures() => SaveFixture.Curated();

    [Test]
    [TestCaseSource(nameof(Fixtures))]
    public void Header_MatchesExpected(SaveFixture fixture)
    {
        var save = SaveTestHost.CreateSerializer().Deserialize(fixture.FullPath);

        save.Header.SaveVersion.Should().Be(fixture.SaveVersion);
        save.Header.SessionName.Should().Be(fixture.SessionName);
    }

    [Test]
    [TestCaseSource(nameof(Fixtures))]
    public void Statistics_MatchExpected(SaveFixture fixture)
    {
        var save = SaveTestHost.CreateSerializer().Deserialize(fixture.FullPath);
        var actual = SaveAnalysis.Analyze(save);

        actual.Should().Be(fixture.Expected, $"statistics for {fixture.FileName}");
    }

    [Test]
    public void UnlockSaves_AreIdentical()
    {
        var serializer = SaveTestHost.CreateSerializer();
        var saveA = serializer.Deserialize(Path.Combine(SaveFixture.SavesDirectory, "Unlock 1.1.sav"));
        var saveB = serializer.Deserialize(Path.Combine(SaveFixture.SavesDirectory, "Unlock 1.1-2.sav"));

        SaveAnalysis.Analyze(saveA).Should().Be(SaveAnalysis.Analyze(saveB));
    }

    [Test]
    public void FreshStartU8001_HasVehicleExtraData()
    {
        var save = SaveTestHost.CreateSerializer().Deserialize(
            Path.Combine(SaveFixture.SavesDirectory, "FreshStartU8001-vehicles-2.sav"));

        var vehiclesWithCargo = SaveAnalysis.EnumerateObjects(save)
            .Where(o => KnownConstants.IsVehicle(o.TypePath))
            .Where(o => o.ExtraData is VehicleData)
            .ToList();

        vehiclesWithCargo.Should().HaveCount(6);
    }
}
