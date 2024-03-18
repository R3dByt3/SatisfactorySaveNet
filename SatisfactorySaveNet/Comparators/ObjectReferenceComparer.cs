using SatisfactorySaveNet.Abstracts.Model;
using System;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Comparators;

public class ObjectReferenceComparer : IEqualityComparer<ObjectReference>
{
    public static readonly IEqualityComparer<ObjectReference> Instance = new ObjectReferenceComparer();

    public bool Equals(ObjectReference? x, ObjectReference? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null) return false;
        if (y is null) return false;
        if (x.GetType() != y.GetType()) return false;
        return string.Equals(x.LevelName, y.LevelName, StringComparison.Ordinal) && string.Equals(x.PathName, y.PathName, StringComparison.Ordinal);
    }

    public int GetHashCode(ObjectReference obj)
    {
        return HashCode.Combine(obj.LevelName, obj.PathName);
    }
}