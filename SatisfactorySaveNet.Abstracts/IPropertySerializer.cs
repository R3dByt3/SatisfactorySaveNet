using SatisfactorySaveNet.Abstracts.Model;
using SatisfactorySaveNet.Abstracts.Model.Properties;
using System.Collections.Generic;
using System.IO;

namespace SatisfactorySaveNet.Abstracts;

public interface IPropertySerializer
{
    public IEnumerable<Property> DeserializeProperties(BinaryReader reader, Header? header = null, string? type = null);
    public Property? DeserializeProperty(BinaryReader reader, Header? header = null, string? type = null);
}
