using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Server.Physics.Common
{
    public class PalShift
    {
        public int CurTex;
        public List<PalShiftTex> LandTex;
        public List<SubPalette> SubPals;
        public uint MaxSubs;

        public uint SelectRot(int x, int y, ref LandDefs.Rotation rot, List<uint> palCode, int size, uint palID, ref int rIndex, bool minimizePal)
        {
            var first_tnum = (ulong)(1813693831 * y - x * (1647165151 * y + 1109124029) - 2031889191)
                                * 2.3283064e-10 * LandTex.Count;
            var tnum = first_tnum;
            var cur_tnum = first_tnum;

            LandDefs.Rotation curRot;

            var jmp = false;
            do
            {
                while (true)
                {
                    CurTex = (int)first_tnum;
                    if (CheckTerrain(palCode, ref palID))
                        break;

                    if (cur_tnum == first_tnum)
                    {
                        CurTex = 0;
                        rot = LandDefs.Rotation.Rot0;
                        return palCode[0];
                    }
                }
                var beginRot = GetBeginRotation(x, y, palCode, minimizePal);
                curRot = beginRot;
                jmp = false;
                while (!CheckRot(palCode[(int)curRot], ref rIndex))
                {
                    curRot = (LandDefs.Rotation)(((int)curRot + 1) & 3);
                    if (curRot == beginRot)
                    {
                        // y
                        tnum = (tnum + 1) % (ulong)LandTex.Count;
                        cur_tnum = tnum;
                        if (cur_tnum == first_tnum)
                        {
                            CurTex = 0;
                            rot = LandDefs.Rotation.Rot0;
                            return palCode[0];
                        }
                        jmp = true;
                        break;
                    }
                }
            }
            while (jmp);

            rot = curRot;
            return palCode[(int)curRot];
        }

        public bool CheckTerrain(List<uint> pCode, ref uint palID)
        {
            var numTerrainPalettes = LandTex[CurTex].TerrainPal.Count;
            var invByte = (byte)(8 - (byte)palID);
            //var firstPalette = 0x04000000;
            for (var i = 0; i < 4; i++)
            {
                var terrainIdx = pCode[0] >> (int)(palID + invByte) & 0x1F;
                bool found = false;
                for (var j = 0; j < numTerrainPalettes; j++)
                {
                    var terrainPal = LandTex[CurTex].TerrainPal[j];
                    palID = terrainPal.PalID;
                    if (terrainIdx == (uint)terrainPal.TerrainIndex)
                    {
                        found = true;
                        break;
                    }
                }
                invByte = (byte)(8 - (byte)palID);
                if (!found)
                    return false;
                palID++;
            }

            var curIdx = 0;
            if ((pCode[0] & 0xC000000) != 0)
                curIdx = 1;
            curIdx *= 2;
            if ((pCode[0] & 0x3000000) != 0)
                curIdx++;
            curIdx *= 2;
            if ((pCode[0] & 0xC00000) != 0)
                curIdx++;
            curIdx *= 2;
            if ((pCode[0] & 0x300000) != 0)
                curIdx++;
            if (curIdx != 0)
            {
                bool lastPalette = false;
                for (var i = 0; i < numTerrainPalettes; i++)
                {
                    var terrainPal = LandTex[CurTex].TerrainPal[i];
                    palID = terrainPal.PalID;
                    if ((int)terrainPal.TerrainIndex == 32)
                    {
                        lastPalette = true;
                        break;
                    }
                }
                if (!lastPalette)
                    return false;
            }
            return true;
        }

        public LandDefs.Rotation GetBeginRotation(int x, int y, List<uint> pCode, bool minimizePal)
        {
            if (minimizePal)
            {
                var lowest = uint.MaxValue;
                var rotation = LandDefs.Rotation.Rot0;

                if (pCode[0] < 0xFFFFFFFF)
                    lowest = pCode[0];
                if (pCode[1] < lowest)
                {
                    lowest = pCode[1];
                    rotation = LandDefs.Rotation.Rot90;
                }
                if (pCode[2] < lowest)
                {
                    lowest = pCode[2];
                    rotation = LandDefs.Rotation.Rot180;
                }
                if (pCode[3] < lowest)
                    rotation = LandDefs.Rotation.Rot270;

                return rotation;
            }

            var rotIdx = (ulong)(1813693831 * y - x * (501661475 * y + 1109124029) + 1225298869)
                    * 2.3283064e-10 * 4;

            return (LandDefs.Rotation)rotIdx;
        }

        public bool CheckRot(uint pCode, ref int rIndex)
        {
            var curCode = pCode;
            var curIdx = 0;
            if ((pCode & 0xC000000) != 0)
                curIdx = 1;
            curIdx *= 2;
            if ((pCode & 0x3000000) != 0)
                curIdx++;
            curIdx *= 2;
            if ((pCode & 0xC00000) != 0)
                curIdx++;
            curIdx *= 2;
            if ((pCode & 0x300000) != 0)
                curIdx++;

            var landTex = LandTex[CurTex];
            for (var i = 0; i < landTex.RoadCode.Count; i++)
            {
                var roadCode = landTex.RoadCode[i];
                if (roadCode.RoadCode == curIdx)
                {
                    rIndex = i;
                    return true;
                }
            }
            return false;
        }
    }
}
