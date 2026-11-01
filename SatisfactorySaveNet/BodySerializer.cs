using SatisfactorySaveNet.Abstracts;
using SatisfactorySaveNet.Abstracts.Exceptions;
using SatisfactorySaveNet.Abstracts.Model;
using System;
using System.IO;
using System.Linq;

namespace SatisfactorySaveNet;

public class BodySerializer : IBodySerializer
{
    private static readonly Guid PersistentLevelCustomVersionKey = new(new byte[]
    {
        0xED, 0x68, 0xB0, 0xE4, 0xE9, 0x42, 0x94, 0xF4,
        0x0B, 0xDA, 0x31, 0xA2, 0x41, 0xBB, 0x46, 0x2E
    });

    public static readonly IBodySerializer Instance = new BodySerializer(StringSerializer.Instance, ObjectHeaderSerializer.Instance, ObjectReferenceSerializer.Instance, ObjectSerializer.Instance, DataPackageVersionSerializer.Instance);

    private readonly IStringSerializer _stringSerializer;
    private readonly IObjectHeaderSerializer _objectHeaderSerializer;
    private readonly IObjectReferenceSerializer _objectReferenceSerializer;
    private readonly IObjectSerializer _objectSerializer;
    private readonly IDataPackageVersionSerializer _dataPackageVersionSerializer;

    public BodySerializer(IStringSerializer stringSerializer, IObjectHeaderSerializer objectHeaderSerializer, IObjectReferenceSerializer objectReferenceSerializer, IObjectSerializer objectSerializer, IDataPackageVersionSerializer? dataPackageVersionSerializer = null)
    {
        _stringSerializer = stringSerializer;
        _objectHeaderSerializer = objectHeaderSerializer;
        _objectReferenceSerializer = objectReferenceSerializer;
        _objectSerializer = objectSerializer;
        _dataPackageVersionSerializer = dataPackageVersionSerializer ?? DataPackageVersionSerializer.Instance;
    }

    public BodyBase Deserialize(BinaryReader reader, Header header)
    {
        var context = new SaveParseContext
        {
            LevelSaveVersion = header.SaveVersion,
            EntitySaveVersion = header.SaveVersion
        };
        DataPackageVersion? saveDataPackageVersion = null;
        if (header.SaveVersion >= 53)
        {
            saveDataPackageVersion = _dataPackageVersionSerializer.Deserialize(reader);
            if (!saveDataPackageVersion.CustomVersionContainer.Any(customVersion => customVersion.Key == PersistentLevelCustomVersionKey && customVersion.Version == 40))
            {
                saveDataPackageVersion.CustomVersionContainer.Add(new CustomVersion
                {
                    Key = PersistentLevelCustomVersionKey,
                    Version = 40
                });
            }
            context.SaveDataPackageVersion = saveDataPackageVersion;
        }
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
            var nrLevels = reader.ReadInt32();
            var levels = new Level[nrLevels + 1];

            for (var i = 0; i <= nrLevels; i++)
            {
                var levelName = i == nrLevels ? "Level " + header.MapName : _stringSerializer.Deserialize(reader);
                context.LevelSaveVersion = header.SaveVersion;
                context.LevelUE5Version = 1000;
                var binaryLength = header.SaveVersion >= 41 ? reader.ReadInt64() : reader.ReadInt32();
                var position = reader.BaseStream.Position;
                int? saveVersion = null;
                DataPackageVersion? levelDataPackageVersion = null;

                if (header.SaveVersion >= 51)
                {
                    if (i == nrLevels)
                    {
                        saveVersion = header.SaveVersion;
                        if (header.SaveVersion >= 53)
                        {
                            context.LevelUE5Version = saveDataPackageVersion!.PackageFileVersion.UE5Version;
                        }
                    }
                    else
                    {
                        var prescanStart = reader.BaseStream.Position;
                        reader.BaseStream.Seek(binaryLength, SeekOrigin.Current);
                        var skip = reader.ReadInt64();
                        reader.BaseStream.Seek(skip, SeekOrigin.Current);
                        saveVersion = reader.ReadInt32();
                        if (header.SaveVersion >= 53)
                        {
                            var nrDestroyedRefs = reader.ReadInt32();
                            for (var j = 0; j < nrDestroyedRefs; j++)
                            {
                                _ = _objectReferenceSerializer.Deserialize(reader);
                            }
                            if (reader.ReadInt32() == 1)
                            {
                                levelDataPackageVersion = _dataPackageVersionSerializer.Deserialize(reader);
                                context.LevelUE5Version = levelDataPackageVersion.PackageFileVersion.UE5Version;
                            }
                        }
                        reader.BaseStream.Position = prescanStart;
                    }
                }

                var nrObjectHeaders = reader.ReadInt32();
                var objects = new ComponentObject[nrObjectHeaders];

                for (var j = 0; j < nrObjectHeaders; j++)
                {
                    objects[j] = _objectHeaderSerializer.Deserialize(reader, saveVersion);
                }

                ObjectReference[] collectables;
                
                if (reader.BaseStream.Position <= position + binaryLength - 4)
                {
                    var nrCollectables = reader.ReadInt32();
                        
                    if (nrCollectables > 0 && header.SaveVersion >= 46 && i == nrLevels)
                    {
                        var unknownStr = _stringSerializer.Deserialize(reader);
                        nrCollectables = reader.ReadInt32();
                    }
                        
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

                var binarySizeObjects = header.SaveVersion >= 41 ? reader.ReadInt64() : reader.ReadInt32();
                var positionStart = reader.BaseStream.Position;
                var nrObjects = reader.ReadInt32();

                if (nrObjects != nrObjectHeaders)
                    throw new CorruptedSatisFactorySaveFileException("NrObjects does not match nrObjectHeaders");

                for (var j = 0; j < nrObjects; j++)
                {
                    context.EntitySaveVersion = saveVersion.HasValue && saveVersion.Value <= header.SaveVersion ? saveVersion.Value : header.SaveVersion;
                    objects[j] = _objectSerializer.Deserialize(reader, header, objects[j], context);
                    if (context.EntitySaveVersion >= 53 && reader.ReadInt32() != 0)
                    {
                        objects[j].DataPackageVersion = _dataPackageVersionSerializer.Deserialize(reader);
                    }
                }

                var expectedPosition = positionStart + binarySizeObjects;
                if (expectedPosition != reader.BaseStream.Position)
                    throw new BadReadException("Expected stream position does not match actual position");

                if (i != nrLevels && header.SaveVersion >= 51)
                    _ = reader.ReadUInt32();

                var nrSecondCollectables = reader.ReadInt32();

                if (nrSecondCollectables > 0 && header.SaveVersion >= 46 && i == nrLevels)
                {
                    var unknownStr = _stringSerializer.Deserialize(reader);
                    nrSecondCollectables = reader.ReadInt32();
                }

                var secondCollectables = new ObjectReference[nrSecondCollectables];
                for (var j = 0; j < nrSecondCollectables; j++)
                {
                    secondCollectables[j] = _objectReferenceSerializer.Deserialize(reader);
                }

                if (i != nrLevels && header.SaveVersion >= 53 && reader.ReadInt32() == 1)
                {
                    _ = _dataPackageVersionSerializer.Deserialize(reader);
                }

#pragma warning disable CS0618 // Type or member is obsolete
                levels[i] = new Level
                {
                    Name = levelName,
                    Objects = objects,
                    Collectables = collectables,
                    SecondCollectables = secondCollectables,
                    SaveVersion = saveVersion ?? header.SaveVersion,
                    DataPackageVersion = levelDataPackageVersion
                };
#pragma warning restore CS0618 // Type or member is obsolete
            }

            if (reader.BaseStream.Position == reader.BaseStream.Length)
            {
                return new BodyV8
                {
                    Levels = levels,
                    Grid = grid,
                    SaveDataPackageVersion = saveDataPackageVersion
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
                ObjectReferences = objectReferences,
                SaveDataPackageVersion = saveDataPackageVersion
            };
#pragma warning restore CS0618 // Type or member is obsolete
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
                throw new CorruptedSatisFactorySaveFileException("NrObjects does not match nrObjectHeaders");
            
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