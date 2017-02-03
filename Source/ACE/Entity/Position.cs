using System;
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

    public Position(float NorthSouth, float EastWest)
    {
        NorthSouth -= 0.5f;
        EastWest -= 0.5f;
        NorthSouth *= 10.0f;
        EastWest *= 10.0f;

        uint basex = (uint)(EastWest + 0x400);
        uint basey = (uint)(NorthSouth + 0x400);

        if (basex < 0 || basex >= 0x7F8 || basey < 0 || basey >= 0x7F8)
            throw new Exception("Bad coordinates");  // TODO: Instead of throwing exception should we set to a default location?

        float xOffset = ((basex & 7) * 24.0f) + 12;
        float yOffset = ((basey & 7) * 24.0f) + 12;
        float zOffset = GetZFromCellXY(Cell, xOffset, yOffset);

        Cell = GetCellFromBase(basex, basey);
        Offset = new Vector3(xOffset, yOffset, zOffset);
        Facing = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);        
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

    private float GetZFromCellXY(uint cell, float xOffset, float yOffset)
    {
        // TODO: Load correct z from file
        return 200.0f; 
    }

    private uint GetCellFromBase(uint basex, uint basey)
    {
        byte blockx = (byte)(basex >> 3);
        byte blocky = (byte)(basey >> 3);
        byte cellx = (byte)(basex & 7);
        byte celly = (byte)(basey & 7);

        uint block = (uint)((blockx << 8) | blocky);
        uint cell = (uint)((cellx << 3) | celly);

        return (block << 16) | (cell + 1);
    }
}
