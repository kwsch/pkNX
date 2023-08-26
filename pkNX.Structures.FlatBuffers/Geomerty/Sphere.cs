using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pkNX.Structures.FlatBuffers;

public partial struct Sphere
{
    public Sphere(AABB bounds)
    {
        Center = (PackedVec3f)bounds.Center;
        Radius = bounds.Extents.Magnitude;
    }

    public override string ToString() => $"{{ Radius: {Radius}, Center: {Center} }}";
}
