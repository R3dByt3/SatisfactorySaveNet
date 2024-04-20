using System;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model;

public class Level
{
    public string Name { get; set; } = string.Empty;
    public ICollection<ObjectReference> Collectables { get; set; } = [];
    public ICollection<ComponentObject> Objects { get; set; } = [];
    [Obsolete("These information seem to be obsolete")]
    public ICollection<ObjectReference>? SecondCollectables { get; set; } = [];
}