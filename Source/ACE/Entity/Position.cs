using System.IO;
using System.Numerics;

public class Position
{
    public uint Cell { get; set; }
    public uint Dungeon { get; set; }
    public Vector3 Offset { get; set; }
    public Quaternion Facing { get; set; }

    public Position(uint cell, float x, float y, float z, float qx = 0.0f, float qy = 0.0f, float qz = 0.0f, float qw = 0.0f)
    {
        Cell    = cell;
        Dungeon = cell >> 16;
        Offset  = new Vector3(x, y, z);
        Facing  = new Quaternion(qx, qy, qz, qw);
    }

    public Position(BinaryReader payload)
    {
        Cell    = payload.ReadUInt32();
        Dungeon = Cell >> 16;
        Offset  = new Vector3(payload.ReadSingle(), payload.ReadSingle(), payload.ReadSingle());

        // packet stream isn't the same order as the quaternion constructor
        float qw = payload.ReadSingle();
        Facing  = new Quaternion(payload.ReadSingle(), payload.ReadSingle(), payload.ReadSingle(), qw);
    }

    public void Write(BinaryWriter payload, bool quaternion = true)
    {
        payload.Write(Cell);
        payload.Write(Offset.X);
        payload.Write(Offset.Y);
        payload.Write(Offset.Z);

        if (quaternion)
        {
            payload.Write(Facing.W);
            payload.Write(Facing.X);
            payload.Write(Facing.Y);
            payload.Write(Facing.Z);
        }
    }
}
