using System;

namespace SatisfactorySaveNet;

public static class LongExtensions
{
    public static int ToInt(this long value)
    {
        if (value > int.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(value), value, null);

        return (int) value;
    }
}
