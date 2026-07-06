using SatisfactorySaveNet.Abstracts;
using SatisfactorySaveNet.Abstracts.Exceptions;
using SatisfactorySaveNet.Abstracts.Model;
using System;
using System.IO;

namespace SatisfactorySaveNet;

public class BodySerializer : IBodySerializer
{
    public static readonly IBodySerializer Instance = new BodySerializer(StringSerializer.Instance, ObjectHeaderSerializer.Instance, ObjectReferenceSerializer.Instance, ObjectSerializer.Instance);

    private readonly IStringSerializer _stringSerializer;
    private readonly IObjectHeaderSerializer _objectHeaderSerializer;
    private readonly IObjectReferenceSerializer _objectReferenceSerializer;
    private readonly IObjectSerializer _objectSerializer;

    public BodySerializer(IStringSerializer stringSerializer, IObjectHeaderSerializer objectHeaderSerializer, IObjectReferenceSerializer objectReferenceSerializer, IObjectSerializer objectSerializer)
    {
        _stringSerializer = stringSerializer;
        _objectHeaderSerializer = objectHeaderSerializer;
        _objectReferenceSerializer = objectReferenceSerializer;
        _objectSerializer = objectSerializer;
    }

    public BodyBase Deserialize(BinaryReader reader, Header header)
    {
        Grid? grid = null;
        if (header is { SaveVersion: >= 41, IsPartitionedWorld: 1 })
        {
            var partitionCount = reader.ReadInt32();
            var unknown1 = _stringSerializer.Deserialize(reader);
            var unknown2 = reader.ReadUInt32();
            var headHex1 = reader.ReadUInt32();
            _ = reader.ReadInt32();
            var unknown4 = _stringSerializer.Deserialize(reader);
            var headHex2 = reader.ReadUInt32();

            var data = new GridData[partitionCount - 1];

            for (var x = 1; x < partitionCount; x++)
            {
                var unknown6 = _stringSerializer.Deserialize(reader);
                var gridHex = reader.ReadUInt32();
                var count = reader.ReadUInt32();
                var nrLevels = reader.ReadInt32();

                var levels = new GridLevel[nrLevels];

                for (var y = 0; y < nrLevels; y++)
                {
                    var unknown9 = _stringSerializer.Deserialize(reader);
                    var unknown10 = reader.ReadUInt32();

                    levels[y] = new GridLevel
                    {
                        Unknown1 = unknown9,
                        Unknown2 = unknown10
                    };
                }

                data[x - 1] = new GridData
                {
                    Unknown1 = unknown6,
                    GridHex = gridHex,
                    Count = count,
                    Levels = levels
                };
            }

            grid = new Grid
            {
                Unknown1 = unknown1,
                Unknown2 = unknown2,
                HeadHex1 = headHex1,
                Unknown4 = unknown4,
                HeadHex2 = headHex2,
                Data = data
            };
        }

        if (header.SaveVersion >= 29)
        {
            var isV53 = header.SaveVersion >= 53;
            var nrLevels = reader.ReadInt32();
            var levels = new Level[nrLevels + 1];

            for (var i = 0; i <= nrLevels; i++)
            {
                var isPersistentLevel = i == nrLevels;
                var levelName = isPersistentLevel ? "Level " + header.MapName : _stringSerializer.Deserialize(reader);
                var binaryLength = isV53 || header.SaveVersion >= 41 ? reader.ReadInt64() : reader.ReadInt32();
                var position = reader.BaseStream.Position;
                int? saveVersion = null;

                if (isV53)
                {
                    header.ParseState.CurrentLevelSaveVersion = header.SaveVersion;
                    header.ParseState.CurrentLevelUE5Version = SaveParseState.DefaultUe5Version;

                    if (isPersistentLevel)
                    {
                        if (header.ParseState.SaveDataPackageVersion != null)
                        {
                            header.ParseState.CurrentLevelUE5Version = header.ParseState.SaveDataPackageVersion.PackageFileVersion.UE5Version;
                        }
                    }
                    else
                    {
                        reader.BaseStream.Seek(binaryLength, SeekOrigin.Current);
                        var skip = reader.ReadInt64();
                        reader.BaseStream.Seek(skip, SeekOrigin.Current);
                        header.ParseState.CurrentLevelSaveVersion = (int) reader.ReadUInt32();

                        var objectRefCount = reader.ReadInt32();
                        for (var r = 0; r < objectRefCount; r++)
                        {
                            _objectReferenceSerializer.Deserialize(reader);
                        }

                        if (reader.ReadInt32() == 1)
                        {
                            var levelDataPackage = UnrealFormatHelper.DeserializeDataPackageVersion(reader, _stringSerializer, HexSerializer.Instance);
                            header.ParseState.CurrentLevelUE5Version = levelDataPackage.PackageFileVersion.UE5Version;
                        }

                        reader.BaseStream.Seek(position, SeekOrigin.Begin);
                    }
                }
                else if (header.SaveVersion >= 51)
                {
                    if (isPersistentLevel)
                    {
                        saveVersion = header.SaveVersion;
                    }
                    else
                    {
                        reader.BaseStream.Seek(binaryLength, SeekOrigin.Current);
                        var skip = reader.ReadInt64();
                        reader.BaseStream.Seek(skip, SeekOrigin.Current);
                        saveVersion = reader.ReadInt32();
                        reader.BaseStream.Seek(-(skip + binaryLength + sizeof(int) + sizeof(long)), SeekOrigin.Current);
                    }
                }

                var levelSaveVersion = isV53 ? header.ParseState.CurrentLevelSaveVersion : saveVersion;

                var nrObjectHeaders = reader.ReadInt32();
                var objects = new ComponentObject[nrObjectHeaders];

                for (var j = 0; j < nrObjectHeaders; j++)
                {
                    objects[j] = _objectHeaderSerializer.Deserialize(reader, levelSaveVersion);
                }

                if (isV53 && isPersistentLevel)
                {
                    var levelPersistentFlag = reader.ReadInt32();
                    if (levelPersistentFlag != 0)
                    {
                        _ = _stringSerializer.Deserialize(reader);
                    }
                }

                ObjectReference[] collectables;

                if (reader.BaseStream.Position <= position + binaryLength - 4)
                {
                    var nrCollectables = reader.ReadInt32();

                    if (nrCollectables > 0 && header.SaveVersion >= 46 && isPersistentLevel)
                    {
                        _ = _stringSerializer.Deserialize(reader);
                        nrCollectables = reader.ReadInt32();
                    }

                    if (isV53)
                    {
                        if (nrCollectables > 0)
                        {
                            collectables = new ObjectReference[nrCollectables];
                            for (var j = 0; j < nrCollectables; j++)
                            {
                                collectables[j] = _objectReferenceSerializer.Deserialize(reader);
                            }
                        }
                        else
                        {
                            collectables = [];
                        }
                    }
                    else
                    {
                        collectables = new ObjectReference[nrCollectables];
                        for (var j = 0; j < nrCollectables; j++)
                        {
                            collectables[j] = _objectReferenceSerializer.Deserialize(reader);
                        }
                    }
                }
                else
                {
                    collectables = [];
                }

                if (isV53 && !isPersistentLevel)
                {
                    if (reader.BaseStream.Position < position + binaryLength - 4)
                    {
                        var nrSublevelCollectables = reader.ReadInt32();
                        for (var c = 0; c < nrSublevelCollectables; c++)
                        {
                            _objectReferenceSerializer.Deserialize(reader);
                        }
                    }
                    else if (reader.BaseStream.Position == position + binaryLength - 4)
                    {
                        _ = reader.ReadInt32();
                    }
                }

                var binarySizeObjects = isV53 || header.SaveVersion >= 41 ? reader.ReadInt64() : reader.ReadInt32();
                var positionStart = reader.BaseStream.Position;
                var nrObjects = reader.ReadInt32();

                if (nrObjects != nrObjectHeaders)
                {
                    throw new CorruptedSatisFactorySaveFileException("NrObjects does not match nrObjectHeaders");
                }

                for (var j = 0; j < nrObjects; j++)
                {
                    if (isV53)
                    {
                        try
                        {
                            objects[j] = _objectSerializer.Deserialize(reader, header, objects[j]);
                        }
                        catch (Exception ex)
                        {
                            throw new CorruptedSatisFactorySaveFileException(
                                $"Level '{levelName}' entity {j + 1}/{nrObjects} ({objects[j].TypePath}): {ex.Message}",
                                ex);
                        }
                    }
                    else
                    {
                        objects[j] = _objectSerializer.Deserialize(reader, header, objects[j]);
                    }
                }

                var expectedPosition = positionStart + binarySizeObjects;
                if (expectedPosition != reader.BaseStream.Position)
                {
                    if (isV53)
                    {
                        throw new BadReadException(
                            $"Level '{levelName}' entity block size mismatch: expected {expectedPosition}, at {reader.BaseStream.Position} (delta {reader.BaseStream.Position - expectedPosition})");
                    }

                    throw new BadReadException("Expected stream position does not match actual position");
                }

                if (isV53)
                {
                    if (reader.BaseStream.Position < position + binaryLength)
                    {
                        reader.BaseStream.Seek(position + binaryLength, SeekOrigin.Begin);
                    }
                }
                else if (!isPersistentLevel && header.SaveVersion >= 51)
                {
                    _ = reader.ReadUInt32();
                }

                ObjectReference[] secondCollectables;

                if (isV53)
                {
                    if (!isPersistentLevel)
                    {
                        _ = reader.ReadUInt32();

                        var nrSecondCollectables = reader.ReadInt32();
                        secondCollectables = new ObjectReference[nrSecondCollectables];
                        for (var j = 0; j < nrSecondCollectables; j++)
                        {
                            secondCollectables[j] = _objectReferenceSerializer.Deserialize(reader);
                        }

                        if (reader.ReadInt32() == 1)
                        {
                            _ = UnrealFormatHelper.DeserializeDataPackageVersion(reader, _stringSerializer, HexSerializer.Instance);
                        }
                    }
                    else
                    {
                        secondCollectables = [];
                    }
                }
                else
                {
                    var nrSecondCollectables = reader.ReadInt32();

                    if (nrSecondCollectables > 0 && header.SaveVersion >= 46 && isPersistentLevel)
                    {
                        _ = _stringSerializer.Deserialize(reader);
                        nrSecondCollectables = reader.ReadInt32();
                    }

                    secondCollectables = new ObjectReference[nrSecondCollectables];
                    for (var j = 0; j < nrSecondCollectables; j++)
                    {
                        secondCollectables[j] = _objectReferenceSerializer.Deserialize(reader);
                    }
                }

#pragma warning disable CS0618 // Type or member is obsolete
                levels[i] = new Level
                {
                    Name = levelName,
                    Objects = objects,
                    Collectables = collectables,
                    SecondCollectables = secondCollectables
                };
#pragma warning restore CS0618 // Type or member is obsolete
            }

            if (isV53)
            {
                if (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    var nrObjectReferences = reader.ReadInt32();
                    var objectReferences = new ObjectReference[nrObjectReferences];
                    for (var i = 0; i < nrObjectReferences; i++)
                    {
                        objectReferences[i] = _objectReferenceSerializer.Deserialize(reader);
                    }

                    DataPackageVersion? trailingSaveDataPackageVersion = null;
                    if (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        var trailingBytes = reader.ReadBytes((int) (reader.BaseStream.Length - reader.BaseStream.Position));
                        using var trailingReader = new BinaryReader(new MemoryStream(trailingBytes));
                        try
                        {
                            trailingSaveDataPackageVersion = UnrealFormatHelper.DeserializeDataPackageVersion(
                                trailingReader, _stringSerializer, HexSerializer.Instance);
                        }
                        catch (EndOfStreamException)
                        {
                        }
                    }

#pragma warning disable CS0618 // Type or member is obsolete
                    return new BodyV8
                    {
                        Levels = levels,
                        Grid = grid,
                        ObjectReferences = objectReferences,
                        TrailingSaveDataPackageVersion = trailingSaveDataPackageVersion
                    };
#pragma warning restore CS0618 // Type or member is obsolete
                }

#pragma warning disable CS0618 // Type or member is obsolete
                return new BodyV8 { Levels = levels, Grid = grid };
#pragma warning restore CS0618 // Type or member is obsolete
            }
            else
            {
                if (reader.BaseStream.Position == reader.BaseStream.Length)
                {
                    return new BodyV8
                    {
                        Levels = levels,
                        Grid = grid
                    };
                }

                var nrObjectReferences = reader.ReadInt32();
                var objectReferences = new ObjectReference[nrObjectReferences];

                for (var i = 0; i < nrObjectReferences; i++)
                {
                    objectReferences[i] = _objectReferenceSerializer.Deserialize(reader);
                }

#pragma warning disable CS0618 // Type or member is obsolete
                return new BodyV8
                {
                    Levels = levels,
                    Grid = grid,
                    ObjectReferences = objectReferences
                };
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }
        else
        {
            var nrObjectHeaders = reader.ReadInt32();
            var objects = new ComponentObject[nrObjectHeaders];

            for (var j = 0; j < nrObjectHeaders; j++)
            {
                objects[j] = _objectHeaderSerializer.Deserialize(reader, null);
            }

            var nrObjects = reader.ReadInt32();

            if (nrObjects != nrObjectHeaders)
            {
                throw new CorruptedSatisFactorySaveFileException("NrObjects does not match nrObjectHeaders");
            }

            for (var j = 0; j < nrObjects; j++)
            {
                objects[j] = _objectSerializer.Deserialize(reader, header, objects[j]);
            }

            var nrSecondCollectables = reader.ReadInt32();
            var collectables = new ObjectReference[nrSecondCollectables];

            for (var j = 0; j < nrSecondCollectables; j++)
            {
                collectables[j] = _objectReferenceSerializer.Deserialize(reader);
            }
#pragma warning disable CS0618 // Type or member is obsolete
            return new BodyPreV8
            {
                Collectables = collectables,
                Objects = objects,
            };
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}