namespace SatisfactorySaveNet.Abstracts.Model;

/// <summary>
/// SatisfactorySave is the main class for parsing a savegame
/// </summary>
public class SatisfactorySave
{
    /// <summary>
    /// Header part of the save containing things like the version and metadata
    /// </summary>
    public Header Header { get; set; }

    /// <summary>
    /// Body part of the save containing things like subLevels
    /// </summary>
    public Body? Body { get; set; }

    public SatisfactorySave(Header header, Body? body)
    {
        Header = header;
        Body = body;
    }
}