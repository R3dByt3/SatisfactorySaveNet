using System.Collections.Generic;
namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayInterfaceProperty : IArrayProperty
{
    public ICollection<ObjectReference> Values { get; set; } = [];
}
