using SatisfactorySaveNet.Abstracts;
using SatisfactorySaveNet.Abstracts.Exceptions;
using SatisfactorySaveNet.Abstracts.Model;
using System.Collections.Generic;
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

    public Body? Deserialize(BinaryReader reader, Header header)
    {
        if (header.SaveVersion >= 41)
        {
            var x1 = reader.ReadInt32(); //ToDo: PowerGridData
            var x2 = _stringSerializer.Deserialize(reader);
            var x3 = reader.ReadInt64();
            var x4 = reader.ReadInt32();
            var x5 = _stringSerializer.Deserialize(reader);
            var x6 = reader.ReadInt32();
            for (var x = 1; x < x1; x++)
            {
                var s = _stringSerializer.Deserialize(reader);
                var y1 = reader.ReadInt32();
                var y2 = reader.ReadInt32();
                var y3 = reader.ReadInt32();
                for (var y = 0; y < y3; y++)
                {
                    var ss = _stringSerializer.Deserialize(reader);
                    var u = reader.ReadUInt32();
                }
            }
        }
        if (header.SaveVersion >= 29)
        {
            var nrLevels = reader.ReadInt32();
            var levels = new List<Level>(nrLevels);

            for (var i = 0; i <= nrLevels; i++)
            {
                var levelName = i == nrLevels ? "Level " + header.MapName : _stringSerializer.Deserialize(reader);
                var binaryLength = header.SaveVersion >= 41 ? reader.ReadInt64() : reader.ReadInt32();
                var position = reader.BaseStream.Position;

                var nrObjectHeaders = reader.ReadInt32();
                var objects = new List<ComponentObject>(nrObjectHeaders);

                for (var j = 0; j < nrObjectHeaders; j++)
                {
                    objects.Add(_objectHeaderSerializer.Deserialize(reader));
                }

                List<ObjectReference> collectables;

                if (header.SaveVersion >= 41)
                {
                    if (reader.BaseStream.Position < position + binaryLength - 4)
                    {
                        var nrCollectables = reader.ReadInt32();
                        collectables = new List<ObjectReference>(nrCollectables);
                        for (var j = 0; j < nrCollectables; j++)
                        {
                            collectables.Add(_objectReferenceSerializer.Deserialize(reader));
                        }
                    }
                    else if (reader.BaseStream.Position == position + binaryLength - 4)
                    {
                        reader.ReadInt32();
                        collectables = [];
                    }
                    else
                        collectables = [];
                }
                else
                {
                    var nrCollectables = reader.ReadInt32();
                    collectables = new List<ObjectReference>(nrCollectables);

                    for (var j = 0; j < nrCollectables; j++)
                    {
                        collectables.Add(_objectReferenceSerializer.Deserialize(reader));
                    }
                }

                var binarySizeObjects = header.SaveVersion >= 41 ? reader.ReadInt64() : reader.ReadInt32();
                var nrObjects = reader.ReadInt32();

                if (nrObjects != nrObjectHeaders)
                    throw new CorruptedSatisFactorySaveFileException("NrObjects does not match nrObjectHeaders");

                for (var j = 0; j < nrObjects; j++)
                {
                    objects[j] = _objectSerializer.Deserialize(reader, header, objects[j]);
                }

                var nrSecondCollectables = reader.ReadInt32();
                var secondCollectables = new List<ObjectReference>(nrSecondCollectables);

                for (var j = 0; j < nrSecondCollectables; j++)
                {
                    secondCollectables.Add(_objectReferenceSerializer.Deserialize(reader));
                }

#pragma warning disable CS0618 // Type or member is obsolete
                levels.Add(new Level
                {
                    Name = levelName,
                    Objects = objects,
                    Collectables = collectables,
                    SecondCollectables = secondCollectables
                });
#pragma warning restore CS0618 // Type or member is obsolete
            }

            return new Body
            {
                Levels = levels,
                //ObjectReferences =
            };
        }

        return null;
    }
}