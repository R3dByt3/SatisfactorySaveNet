using SatisfactorySaveNet.Abstracts.Maths.Vector;

namespace SatisfactorySaveNet.Abstracts.Extra;

public class Spline
{
    public Vector3D Location { get; set; }
    public Vector3D ArriveTangent { get; set; }
    public Vector3D LeaveTangent { get; set; }
}
