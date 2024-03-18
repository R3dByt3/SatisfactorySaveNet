using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using OneOf;
using OneOf.Types;
using SatisfactorySaveNet.Abstracts.Maths.Matrix;
using SatisfactorySaveNet.Abstracts.Model.Union;
using Half = SatisfactorySaveNet.Abstracts.Maths.Data.Half;

namespace SatisfactorySaveNet.Benchmarks;

[MemoryDiagnoser]
public class Unions
{
    private Half _half = new();
    private Matrix4D _matrix4D = new();
    private readonly Union _unionHalf = new();
    private readonly Union _unionMatrix4D = new();
    private UnionStr _unionStrHalf = new();
    private UnionStr _unionStrMatrix4D = new();
    private object _boxHalf;
    private object _boxMatrix4D;
    private OneOf<Half, None> _oneOfHalf;
    private OneOf<Matrix4D, None> _oneOfMatrix4D;

    public Unions()
    {
        _boxHalf = _half;
        _boxMatrix4D = _matrix4D;
        _oneOfHalf = _half;
        _oneOfMatrix4D = _matrix4D;
        _unionHalf.AsHalf = _half;
        _unionStrHalf.AsHalf = _half;
        _unionMatrix4D.AsMatrix4D = _matrix4D;
        _unionStrMatrix4D.AsMatrix4D = _matrix4D;
    }

    [Benchmark]
    public void BoxingHalf()
    {
        _boxHalf = _half;
    }

    [Benchmark]
    public void UnionHalf()
    {
        _unionHalf.AsHalf = _half;
    }

    [Benchmark]
    public void UnionStrHalf()
    {
        _unionStrHalf.AsHalf = _half;
    }

    [Benchmark]
    public void OneOfHalf()
    {
        _oneOfHalf = _half;
    }

    [Benchmark]
    public void UnboxingHalf()
    {
        _half = (Half) _boxHalf;
    }

    [Benchmark]
    public void UnunionHalf()
    {
        _half = _unionHalf.AsHalf;
    }

    [Benchmark]
    public void UnunionStrHalf()
    {
        _half = _unionStrHalf.AsHalf;
    }

    [Benchmark]
    public void UnoneOfHalf()
    {
        _half = _oneOfHalf.AsT0;
    }

    ///////////////////////

    [Benchmark]
    public void BoxingMatrix4D()
    {
        _boxMatrix4D = _matrix4D;
    }

    [Benchmark]
    public void UnionMatrix4D()
    {
        _unionMatrix4D.AsMatrix4D = _matrix4D;
    }

    [Benchmark]
    public void UnionStrMatrix4D()
    {
        _unionStrMatrix4D.AsMatrix4D = _matrix4D;
    }

    [Benchmark]
    public void OneOfMatrix4D()
    {
        _oneOfMatrix4D = _matrix4D;
    }

    [Benchmark]
    public void UnboxingMatrix4D()
    {
        _matrix4D = (Matrix4D) _boxMatrix4D;
    }

    [Benchmark]
    public void UnunionMatrix4D()
    {
        _matrix4D = _unionMatrix4D.AsMatrix4D;
    }

    [Benchmark]
    public void UnunionStrMatrix4D()
    {
        _matrix4D = _unionStrMatrix4D.AsMatrix4D;
    }

    [Benchmark]
    public void UnoneOfMatrix4D()
    {
        _matrix4D = _oneOfMatrix4D.AsT0;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        //ToDo: Benchmark MemoryManager
        //ToDo: Boxing vs Generic union vs Unsafe union
        var summary = BenchmarkRunner.Run<Unions>();
    }
}
