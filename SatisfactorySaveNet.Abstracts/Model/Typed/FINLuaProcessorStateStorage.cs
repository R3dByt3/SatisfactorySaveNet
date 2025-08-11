using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.Typed;

public class FINLuaProcessorStateStorage : TypedData
{
    public override TypedDataConstraint Type => TypedDataConstraint.FINLuaProcessorStateStorage;

    public ICollection<FINNetworkTrace> Traces { get; set; } = [];
    public ICollection<ObjectReference> ObjectReferences { get; set; } = [];
    public string Thread { get; set; } = string.Empty;
    public string Globals { get; set; } = string.Empty;
    public ICollection<TypedData?> TypedData { get; set; } = [];
}
