using SatisfactorySaveNet.Abstracts.Extra;
using SatisfactorySaveNet.Abstracts.Model.Properties;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model;

public class ComponentObject
{
    public const int TypeID = 0;

    public virtual int Type => TypeID;
    public string TypePath { get; set; } = string.Empty;
    public string RootObject { get; set; } = string.Empty;
    public string InstanceName { get; set; } = string.Empty;

    public string ParentActorName { get; set; } = string.Empty;

    public ICollection<Property> Properties { get; set; } = [];
    public ExtraData? ExtraData { get; set; }
    public int? EntitySaveVersion { get; set; }
}