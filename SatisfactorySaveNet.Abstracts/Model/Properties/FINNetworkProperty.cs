namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class FINNetworkProperty : Property
{
    public override PropertyConstraint PropertyValueType => PropertyConstraint.FINNetwork;

    public required ObjectReference ObjectReference { get; set; }
    public FINNetworkProperty? Previous { get; set; }
    public string? Step { get; set; }
}
