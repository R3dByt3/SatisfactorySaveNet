using SatisfactorySaveNet.Abstracts.Maths.Data;

namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class QuatD : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.QuatD;

    public QuaternionD Value { get; set; }
}