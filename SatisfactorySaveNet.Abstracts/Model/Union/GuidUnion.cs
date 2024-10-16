using System;

namespace SatisfactorySaveNet.Abstracts.Model.Union;

public class GuidUnion : UnionBase
{
    public override UnionConstraint Type => UnionConstraint.Guid;

    public Guid Value { get; set; }
}
