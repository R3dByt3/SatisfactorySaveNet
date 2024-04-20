namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class TimerHandle : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.TimerHandle;

    public string Value { get; set; } = string.Empty;
}
