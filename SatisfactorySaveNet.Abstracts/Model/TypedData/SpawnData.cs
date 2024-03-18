using SatisfactorySaveNet.Abstracts.Model.Properties;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class SpawnData : ITypedData
{
    public IList<Property> Properties { get; set; } = [];
}
