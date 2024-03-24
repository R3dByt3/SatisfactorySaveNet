using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.TypedData;

public class FINLuaProcessorStateStorage : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.FINLuaProcessorStateStorage;

    public IList<FINNetworkTrace> Traces { get; set; } = [];
    public IList<ObjectReference> ObjectReferences { get; set; } = [];
    public string Thread { get; set; } = string.Empty;
    public string Globals { get; set; } = string.Empty;
    public IList<TypedData> TypedData { get; set; } = [];
}
