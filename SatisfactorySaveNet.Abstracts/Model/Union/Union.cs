using SatisfactorySaveNet.Abstracts.Maths.Matrix;
using System;
using Half = SatisfactorySaveNet.Abstracts.Maths.Data.Half;

namespace SatisfactorySaveNet.Abstracts.Model.Union;

public enum UnionConstraint
{
    None,
    Half,
    Matrix4D
}

public unsafe class Union
{
    private readonly byte[] _data = new byte[128];
    private UnionConstraint _constraint;

    public Half AsHalf
    {
        get
        {
            if (_constraint != UnionConstraint.Half)
                throw new InvalidCastException();

            fixed (byte* pData = _data)
            {
                return *(Half*) pData;
            }
        }
        set
        {
            fixed (byte* pData = _data)
            {
                *(Half*) pData = value;
            }
            _constraint = UnionConstraint.Half;
        }
    }

    public Matrix4D AsMatrix4D
    {
        get
        {
            if (_constraint != UnionConstraint.Matrix4D)
                throw new InvalidCastException();

            fixed (byte* pData = _data)
            {
                return *(Matrix4D*) pData;
            }
        }
        set
        {
            fixed (byte* pData = _data)
            {
                *(Matrix4D*) pData = value;
            }
            _constraint = UnionConstraint.Matrix4D;
        }
    }
}

public unsafe struct UnionStr
{
    private fixed byte _data[128];
    private UnionConstraint _constraint;

    public Half AsHalf
    {
        get
        {
            if (_constraint != UnionConstraint.Half)
                throw new InvalidCastException();

            fixed (byte* pData = _data)
            {
                return *(Half*) pData;
            }
        }
        set
        {
            fixed (byte* pData = _data)
            {
                *(Half*) pData = value;
            }
            _constraint = UnionConstraint.Half;
        }
    }

    public Matrix4D AsMatrix4D
    {
        get
        {
            if (_constraint != UnionConstraint.Matrix4D)
                throw new InvalidCastException();

            fixed (byte* pData = _data)
            {
                return *(Matrix4D*) pData;
            }
        }
        set
        {
            fixed (byte* pData = _data)
            {
                *(Matrix4D*) pData = value;
            }
            _constraint = UnionConstraint.Matrix4D;
        }
    }
}
