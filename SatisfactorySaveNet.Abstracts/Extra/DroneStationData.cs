using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Extra;

public class DroneStationData : ExtraData
{
    public override ExtraDataConstraint Type => ExtraDataConstraint.DroneStationData;

    public int Unknown1 { get; set; }
    public int Unknown2 { get; set; }
    /// <summary>
    /// Should only be null if Missing is filled, which indicates that there were unparseable data
    /// </summary>
    public ICollection<DroneStationAction>? ActiveActions { get; set; }
    /// <summary>
    /// Should only be null if Missing is filled, which indicates that there were unparseable data
    /// </summary>
    public ICollection<DroneStationAction>? ActionQueue { get; set; }
    public string? Missing { get; set; }
}
