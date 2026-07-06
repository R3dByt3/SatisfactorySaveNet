namespace SatisfactorySaveNet.Tests;

public sealed record SaveFixture(
    string FileName,
    int SaveVersion,
    string SessionName,
    SaveStatistics Expected)
{
    public string FullPath => Path.Combine(SavesDirectory, FileName);

    public static string SavesDirectory =>
        Path.Combine(TestContext.CurrentContext.TestDirectory, "Saves");

    public static IEnumerable<SaveFixture> Curated()
    {
        yield return new SaveFixture(
            "Release 001.sav",
            46,
            "Release",
            new SaveStatistics(1084, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1));

        yield return new SaveFixture(
            "Fresh 1.1 Dismantled.sav",
            52,
            "Fresh 1.1 Stable",
            new SaveStatistics(1465, 11, 0, 0, 0, 0, 0, 0, 0, 1, 1));

        yield return new SaveFixture(
            "FreshStartU8001-vehicles-2.sav",
            42,
            "FreshStartU8",
            new SaveStatistics(2921, 28, 6, 1, 1, 3, 0, 0, 1, 1, 1));

        yield return new SaveFixture(
            "Release 032.sav",
            46,
            "Release",
            new SaveStatistics(6853, 617, 3, 0, 0, 195, 22, 212, 1, 1, 1));

        yield return new SaveFixture(
            "Open Factory.sav",
            52,
            "Open Factory World Tour",
            new SaveStatistics(12125, 531, 0, 0, 0, 332, 73, 227, 0, 1, 1));

        yield return new SaveFixture(
            "Unlock 1.1.sav",
            51,
            "Unlocked",
            new SaveStatistics(1344, 34, 0, 0, 0, 9, 1, 3, 0, 1, 1));

        yield return new SaveFixture(
            "den1.sav",
            30,
            "Den1",
            new SaveStatistics(124066, 15048, 6, 13, 24, 5201, 450, 4361, 0, 2, 1));

        yield return new SaveFixture(
            "Save_1.2.sav",
            60,
            "Sector 39 Extraction",
            new SaveStatistics(28832, 2320, 11, 0, 4, 819, 127, 936, 0, 7, 1));

        yield return new SaveFixture(
            "KoenDenn_autosave_1.sav",
            52,
            "KoenDenn",
            new SaveStatistics(153622, 21178, 1, 21, 22, 10937, 2964, 5443, 4, 4, 1));
    }
}
