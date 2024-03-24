using SatisfactorySaveNet.Abstracts;
using SatisfactorySaveNet.Abstracts.Exceptions;
using SatisfactorySaveNet.Abstracts.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SatisfactorySaveNet;

public class ObjectSerializer : IObjectSerializer
{
    public static readonly IObjectSerializer Instance = new ObjectSerializer(StringSerializer.Instance, ObjectReferenceSerializer.Instance, PropertySerializer.Instance);

    private readonly IStringSerializer _stringSerializer;
    private readonly IObjectReferenceSerializer _objectReferenceSerializer;
    private readonly IPropertySerializer _propertySerializer;

    public ObjectSerializer(IStringSerializer stringSerializer, IObjectReferenceSerializer objectReferenceSerializer, IPropertySerializer propertySerializer)
    {
        _stringSerializer = stringSerializer;
        _objectReferenceSerializer = objectReferenceSerializer;
        _propertySerializer = propertySerializer;
    }

    public ComponentObject Deserialize(BinaryReader reader, Header header, ComponentObject componentObject)
    {
        return componentObject switch
        {
            ActorObject actorObject => DeserializeActor(reader, header, actorObject),
            ComponentObject => DeserializeComponent(reader, header, componentObject),
            _ => throw new CorruptedSatisFactorySaveFileException("Encountered unknown object type")
        };
    }

    private ActorObject DeserializeActor(BinaryReader reader, Header header, ActorObject actorObject)
    {
        if (header.SaveVersion >= 41)
        {
            var version = reader.ReadInt32();
            if (version != header.SaveVersion)
                actorObject.EntitySaveVersion = version;
            _ = reader.ReadInt32();
        }
        var binarySize = reader.ReadInt32();
        var positionStart = reader.BaseStream.Position;

        var parentObjectRoot = _stringSerializer.Deserialize(reader);

        var parentObjectName = _stringSerializer.Deserialize(reader);

        var componentsCount = reader.ReadInt32();
        var components = new List<ObjectReference>(componentsCount);

        for (var i = 0; i < componentsCount; i++)
        {
            var objectRef = _objectReferenceSerializer.Deserialize(reader);
            components.Add(objectRef);
        }

        actorObject.ParentObjectRoot = parentObjectRoot;
        actorObject.ParentObjectName = parentObjectName;

        var expectedPosition = positionStart + binarySize;

        if (expectedPosition == reader.BaseStream.Position)
            return actorObject;

        var properties = _propertySerializer.DeserializeProperties(reader, header).ToList();

        actorObject.Components = components;
        actorObject.Properties = properties;

        var missingBytes = expectedPosition - reader.BaseStream.Position;

        if (missingBytes > 4)
        {
            Console.WriteLine(actorObject.TypePath);
            var binary = reader.ReadBytes(Cast(missingBytes));
            //var hex = new string(binary.Select(Convert.ToChar).ToArray());
        }
        else
            reader.BaseStream.Seek(4, SeekOrigin.Current);

        return actorObject;
    }

    private static int Cast(long value)
    {
        if (value > int.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(value), value, null);

        return (int) value;
    }

    private ComponentObject DeserializeComponent(BinaryReader reader, Header header, ComponentObject componentObject)
    {
        if (header.SaveVersion >= 41)
        {
            var version = reader.ReadInt32();
            if (version != header.SaveVersion)
                componentObject.EntitySaveVersion = version;
            _ = reader.ReadInt32();
        }
        var binarySize = reader.ReadInt32();
        var positionStart = reader.BaseStream.Position;

        var properties = _propertySerializer.DeserializeProperties(reader, header).ToList();
        componentObject.Properties = properties;

        var expectedPosition = positionStart + binarySize;
        var missingBytes = expectedPosition - reader.BaseStream.Position;

        if (missingBytes > 4)
        {
            Console.WriteLine(componentObject.TypePath);
            var binary = reader.ReadBytes(Cast(missingBytes));
            //var hex = new string(binary.Select(Convert.ToChar).ToArray());
        }
        else
            reader.BaseStream.Seek(4, SeekOrigin.Current);

        return componentObject;
    }
}