namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class TimerHandle : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.TimerHandle;

    public string Value { get; set; } = string.Empty;
}
