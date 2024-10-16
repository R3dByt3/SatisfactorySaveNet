using SatisfactorySaveNet.Abstracts.Maths.Data;
using SatisfactorySaveNet.Abstracts.Maths.Vector;
using SatisfactorySaveNet.Abstracts.Model;

namespace SatisfactorySaveNet.Abstracts.Extra;

public class ExtraInstance
{
    public required string PathName { get; set; }
    public Vector4D Rotation { get; set; }
    public Vector3D Translation { get; set; }
    public Vector3D Scale { get; set; }
    public required ObjectReference SwatchDescription { get; set; }
    public required ObjectReference MaterialDescription { get; set; }
    public required ObjectReference PatternDescription { get; set; }
    public required ObjectReference SkinDescription { get; set; }
    public Color4 PrimaryColor { get; set; }
    public Color4 SecondaryColor { get; set; }
    public required ObjectReference PaintFinish { get; set; }
    public sbyte PatternRotation { get; set; }
    public required ObjectReference BuildWithRecipe { get; set; }
    public required ObjectReference BlueprintProxy { get; set; }
}
