namespace SatisfactorySaveNet.Abstracts.Model.ExtraData;

public class BlueprintExtraData : IExtraData
{
    public List<ObjectReference> Objects { get; set; }

    public BlueprintExtraData(List<ObjectReference> objects)
    {
        Objects = objects;
    }
}