using SatisfactorySaveNet.Abstracts.Model.Properties;

namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class FINNetworkTrace : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.FINNetworkTrace;

    public FINNetworkProperty[] Values { get; set; } = [];
}
