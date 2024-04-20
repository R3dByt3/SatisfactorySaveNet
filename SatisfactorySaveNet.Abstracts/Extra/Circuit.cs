using SatisfactorySaveNet.Abstracts.Model;

namespace SatisfactorySaveNet.Abstracts.Extra;

public class Circuit
{
    public int CircuitId { get; set; }
    public required ObjectReference ObjectReference { get; set; }
}
