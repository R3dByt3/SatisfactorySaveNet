using SatisfactorySaveNet.Abstracts.Maths.Data;

namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class QuatD : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.QuatD;

    public QuaternionD Value { get; set; }
}