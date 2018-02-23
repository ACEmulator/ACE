using System.Numerics;

namespace ACE.Server.Physics.Common
{
    public class LandDefs
    {
        public static Vector3 GetBlockOffset(int cellFrom, int cellTo)
        {
            // refactor me
            if (cellFrom >> 16 == cellTo >> 16)
                return Vector3.Zero;

            int xShift21 = 0, xShift16 = 0;
            int yShift21 = 0, yShift16 = 0;

            if (cellFrom != 0)
            {
                xShift21 = (cellFrom >> 21) & 0x7F8;
                xShift16 = 8 * (cellFrom >> 16);
            }
            if (cellTo != 0)
            {
                yShift21 = (cellTo >> 21) & 0x7F8;
                yShift16 = 8 * (cellTo >> 16) & 0xFF;
            }
            else
                yShift21 = yShift16 = cellFrom;

            var shift21Diff = (yShift21 - xShift21);
            var shift16Diff = (yShift16 - xShift16);

            return new Vector3(shift21Diff * 24, shift16Diff * 24, 0);
        }
    }
}
