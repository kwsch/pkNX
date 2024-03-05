using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pkNX.Structures.FlatBuffers;

public partial struct Sphere(AABB bounds)
{
    public override string ToString() => $"{{ Radius: {Radius}, Center: {Center} }}";
}
