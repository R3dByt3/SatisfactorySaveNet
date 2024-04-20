using SatisfactorySaveNet.Abstracts.Model.Properties;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class SpawnData : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.SpawnData;

    public ICollection<Property> Properties { get; set; } = [];
}
