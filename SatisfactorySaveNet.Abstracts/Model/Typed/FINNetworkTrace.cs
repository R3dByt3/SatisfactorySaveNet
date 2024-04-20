using SatisfactorySaveNet.Abstracts.Model.Properties;

namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class FINNetworkTrace : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.FINNetworkTrace;

    public FINNetworkProperty[] Values { get; set; } = [];
}
