using SatisfactorySaveNet.Abstracts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactorySaveNet.Abstracts;

public interface IObjectReferenceSerializer
{
    public ObjectReference Deserialize(BinaryReader reader);
}