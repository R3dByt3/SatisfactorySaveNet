using System;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model;

public class Level
{
    public string Name { get; set; } = string.Empty;
    public IList<ObjectReference> Collectables { get; set; } = [];
    public IList<ComponentObject> Objects { get; set; } = [];
    [Obsolete("These information seem to be obsolete")]
    public IList<ObjectReference>? SecondCollectables { get; set; } = [];
}