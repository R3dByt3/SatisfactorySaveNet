namespace SatisfactorySaveNet.Abstracts.Model;

/// <summary>
/// Transient state used while parsing a save file (not persisted in the .sav header).
/// </summary>
public class SaveParseState
{
    public const int DefaultUe5Version = 1000;

    public const int Ue5PropertyFormatVersion = 1011;

    public DataPackageVersion? SaveDataPackageVersion { get; set; }

    public int CurrentLevelSaveVersion { get; set; }

    public int CurrentLevelUE5Version { get; set; } = DefaultUe5Version;

    public int CurrentEntitySaveVersion { get; set; }

      /// <summary>Struct type name while parsing inline struct property lists (SCIM <c>readProperty(structType)</c> context).</summary>
    public string? CurrentStructTypeName { get; set; }

    /// <summary>End position for UE5 inline struct property payload (<c>currentPropertyLength</c>).</summary>
    public long? CurrentStructPayloadEnd { get; set; }

    /// <summary>End position of the current entity property block.</summary>
    public long? CurrentEntityPayloadEnd { get; set; }

    public bool UseUe5PropertyFormat(int saveVersion) =>
        saveVersion >= 53 && CurrentLevelUE5Version >= Ue5PropertyFormatVersion;
}
