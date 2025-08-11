using System;

namespace SatisfactorySaveNet.Abstracts.Model;

public class Header
{
    /// <summary>
    /// For a version list see the header SaveCustomVersion.h in the Community resources <see href="https://satisfactory.fandom.com/wiki/Community_resources">see here</see>
    /// </summary>
    public int HeaderVersion { get; set; }

    /// <summary>
    /// For a version list see the header FGSaveManagerInterface.h in the Community resources <see href="https://satisfactory.fandom.com/wiki/Community_resources">see here</see>
    /// </summary>
    public int SaveVersion { get; set; }

    //ToDo: Review "Seems to always be 66297"
    /// <summary>
    /// This is Patch <see href="https://satisfactory.fandom.com/wiki/Patch_0.6.1.3">0.6.1.3</see>
    /// </summary>
    public int BuildVersion { get; set; }

    /// <summary>
    /// "Persistent_Level"
    /// </summary>
    public string MapName { get; set; } = string.Empty;

    /// <summary>
    /// An URL style list of arguments of the session.
    /// Contains the startLocation, sessionName and visibility
    /// </summary>
    public string MapOptions { get; set; } = string.Empty;

    /// <summary>
    /// Name of the saved game as entered when creating a new game
    /// </summary>
    public string SessionName { get; set; } = string.Empty;

    /// <summary>
    /// Amount of seconds spent in this save
    /// </summary>
    public int PlayedSeconds { get; set; }

    /// <summary>
    /// Unix utc timestamp of when the save was saved
    /// </summary>
    public DateTime SaveDateTimeUtc { get; set; }

    /// <summary>
    /// This is "private" visibility, 1 would be "friends only" 
    /// </summary>
    public byte? SessionVisibility { get; set; }

    /// <summary>
    /// Depends on the <see href="https://docs.unrealengine.com/4.26/en-US/ProgrammingAndScripting/ProgrammingWithCPP/UnrealArchitecture/VersioningAssetsAndPackages/">unreal engine</see> version used 
    /// </summary>
    public int? EditorObjectVersion { get; set; }

    /// <summary>
    /// Empty if no mods where used 
    /// </summary>
    public string ModMetadata { get; set; } = string.Empty;

    /// <summary>
    /// False if no mods where used
    /// IsModdedSave != 0 <=> True
    /// </summary>
    public int? IsModdedSave { get; set; }

    /// <summary>
    /// A unique identifier (<see href="https://en.wikipedia.org/wiki/Universally_unique_identifier">GUID</see>) for this save, for analytics purposes 
    /// </summary>
    public string? SaveIdentifier { get; set; }

    /// <summary>
    /// Unknown yet
    /// IsPartitionedWorld != 0 <=> True
    /// </summary>
    public int? IsPartitionedWorld { get; set; }

    /// <summary>
    /// Propably some hash for the savegame
    /// </summary>
    public string? SaveDataHash { get; set; }

    /// <summary>
    /// Is creative enabled
    /// IsCreativeModeEnabled != 0 <=> True
    /// </summary>
    public int? IsCreativeModeEnabled { get; set; }

    /// <summary>
    /// Name of the save
    /// </summary>
    public string? SaveName { get; set; }
}