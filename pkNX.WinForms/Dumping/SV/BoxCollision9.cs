
using pkNX.Structures;
using pkNX.Structures.FlatBuffers;

public class BoxCollision9 : IContainsV3f
{
    public required PackedVec3f Position { get; init; }
    public required PackedVec3f Size { get; init; }

    public bool ContainsPoint(float x, float y, float z) => ContainsPoint(x, y, z, 0, 0, 0);

    public bool ContainsPoint(float x, float y, float z, float toleranceX, float toleranceY, float toleranceZ)
    {
        var box_lx = Position.X - (Size.X / 2.0f) - toleranceX;
        var box_hx = Position.X + (Size.X / 2.0f) + toleranceX;
        if (box_lx > box_hx || !(box_lx <= x && x <= box_hx))
            return false;

        var box_lz = Position.Z - (Size.Z / 2.0f) - toleranceZ;
        var box_hz = Position.Z + (Size.Z / 2.0f) + toleranceZ;
        if (box_lz > box_hz || !(box_lz <= z && z <= box_hz))
            return false;

        var ly = y - 10000;
        var hy = y + 1;
        var box_ly = Position.Y - (Size.Y / 2.0f) - toleranceY;
        var box_hy = Position.Y + (Size.Y / 2.0f) + toleranceY;
        if (box_ly > box_hy || !(box_ly <= hy && ly <= box_hy))
            return false;

        return true;
    }
}
