using SatisfactorySaveNet.Abstracts;
using SatisfactorySaveNet.Abstracts.Maths.Vector;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SatisfactorySaveNet.Tests;

[TestFixture]
public class Tests
{
    private readonly ISaveFileSerializer _serializer = SaveFileSerializer.Instance;

    [Test]
    public void SaveFileSerializer_ShouldDeserialize_WhenSaveIsValid()
    {
        //var test = _serializer.Deserialize(@"D:\_Downloads\5CDA4312000000000000000000000000.sav");
        var test = _serializer.Deserialize(@"C:\Users\marvi\AppData\Local\FactoryGame\Saved\SaveGames\5d66aaf3a97b48968049b2531bb6e6f8\Gen 5_autosave_0.sav");
        //var test = _serializer.Deserialize(@"C:\Users\marvi\AppData\Local\FactoryGame\Saved\SaveGames\5d66aaf3a97b48968049b2531bb6e6f8\Gen 5_autosave_0_CALCULATOR.sav");
    }

    [Test]
    public void GetMaxStructSize()
    {
        var assembly = Assembly.GetAssembly(typeof(Vector3))!;

        var types = assembly.GetTypes();

        var maxStructSize = 0;
        var maxStructName = "";

        foreach (var type in types)
        {
            if (type.IsValueType && type.IsValueType && !type.IsEnum)
            {
                try
                {
                    var size = Marshal.SizeOf(type);
                    Console.WriteLine($"Struct: {type.Name}, Size: {size} bytes");

                    if (size > maxStructSize)
                    {
                        maxStructSize = size;
                        maxStructName = type.Name;
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        Console.WriteLine($"The struct with the biggest size is {maxStructName} with a size of {maxStructSize} bytes.");
    }
}