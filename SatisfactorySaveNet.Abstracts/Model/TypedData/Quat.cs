using SatisfactorySaveNet.Abstracts.Maths.Data;

namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class Quat : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.Quat;

    public Quaternion Value { get; set; }
}