using SatisfactorySaveNet.Abstracts;

namespace SatisfactorySaveNet;

public class ExtraDataSerializer : IExtraDataSerializer
{
    public static readonly IExtraDataSerializer Instance = new ExtraDataSerializer();

    //public IExtraData Deserialize(BinaryReader reader, string type)
    //{
    //
    //}
}
