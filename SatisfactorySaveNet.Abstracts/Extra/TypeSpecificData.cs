using SatisfactorySaveNet.Abstracts.Model;
using SatisfactorySaveNet.Abstracts.Model.Properties;

namespace SatisfactorySaveNet.Abstracts.Extra;

public class TypeSpecificData
{
    public required ObjectReference ObjectReference { get; set; }
    public required Property[] Properties { get; set; }
}
