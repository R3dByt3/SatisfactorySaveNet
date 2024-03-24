using System.Collections.Generic;
namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class ArrayInterfaceProperty : IArrayProperty
{
    public IList<ObjectReference> Values { get; set; } = [];
}
