using SatisfactorySaveNet.Abstracts.Model;
using System;
using System.Collections.Generic;

namespace SatisfactorySaveNet.Abstracts.Model.Properties;

public class RawProperty : Property
{
    public override PropertyConstraint PropertyValueType => PropertyConstraint.Raw;

    public required string Type { get; set; }
    public FPropertyTagNode? TypeNode { get; set; }
    public int BinarySize { get; set; }
    public byte Flags { get; set; }
    public Guid? PropertyGuid { get; set; }

    public int? IntValue { get; set; }
    public uint? UIntValue { get; set; }
    public long? LongValue { get; set; }
    public ulong? ULongValue { get; set; }
    public sbyte? SByteValue { get; set; }
    public float? FloatValue { get; set; }
    public double? DoubleValue { get; set; }
    public bool? BoolValue { get; set; }

    public ObjectReferenceValue? ObjectValue { get; set; }
    public IReadOnlyList<ObjectReferenceValue>? ArrayObjectValues { get; set; }
    public string? StringValue { get; set; }
    public StructValue? StructValue { get; set; }
    public IReadOnlyList<MapEntryValue>? MapEntries { get; set; }
}

public sealed record StructValue(string TypeName, object? Value);

public readonly record struct BoxValue(double[] Min, double[] Max, bool IsValid);
public readonly record struct RailroadTrackPositionValue(string Root, string InstanceName, float Offset, float Forward);
public readonly record struct MapEntryValue(object Key, object Value);
public readonly record struct ObjectReferenceValue(string LevelName, string PathName);
