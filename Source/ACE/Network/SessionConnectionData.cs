using ACE.Common.Cryptography;
using ACE.Common;
using ACE.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network
{
    public class SessionConnectionData
    {
        public uint PacketSequence { get; set; }
        public uint FragmentSequence { get; set; }
        public ISAAC IssacClient { get; }
        public ISAAC IssacServer { get; }

        public double ServerTime { get; set; }

        public SessionConnectionData(ConnectionType type)
        {
            IssacClient = new ISAAC(type == ConnectionType.Login ? ISAAC.ClientSeed : ISAAC.WorldClientSeed);
            IssacServer = new ISAAC(type == ConnectionType.Login ? ISAAC.ServerSeed : ISAAC.WorldServerSeed);

            //ServerTime = Convert.ToDouble(DerethDateTime.UtcNowtoDerethTime());
            //ServerTime = (7350 * 30) + (7350 * 31) + (7350 * 1);
            //ServerTime = (7350 * 365) + (7350 * 8.2);

            //ServerTime = (2743020 * 1);
            //ServerTime = (210 + 475) + (475 * 8) + (7615 * 270); //(7615 * 29) + (7615 * 30) + (7615 * 30); //(7608 * 30); //+ (7608 * 30); //4020; //+ (7680 * 10); //+ 7680 + 7680; // 6350;

            //           1/2 period  + 8 periods + 29 days     +  30 days    +  30 days
            //ServerTime = (210 + 475) + (475 * 8) + (7615 * 29) + (7615 * 30) + (7615 * 30) + (7615 * 31); //(7608 * 30); //+ (7608 * 30); //4020; //+ (7680 * 10); //+ 7680 + 7680; // 6350;
            //ServerTime = (210 + 475) + (475 * 8) + (7555 * 29) + (7555 * 30) + (7555 * 30) + (7555 * 31); //(7608 * 30); //+ (7608 * 30); //4020; //+ (7680 * 10); //+ 7680 + 7680; // 6350;
            //ServerTime = ((210 + 475) + (475 * 8)) + (7611.25 * 29); //+ (7555 * 30) + (7555 * 30) + (7555 * 31); //(7608 * 30); //+ (7608 * 30); //4020; //+ (7680 * 10); //+ 7680 + 7680; // 6350;

            //ServerTime = ((210 + 475) + (475 * 8)) + (7611.25 * 29) + (7611.25 * 30); //+ (7555 * 30) + (7555 * 30) + (7555 * 31); //(7608 * 30); //+ (7608 * 30); //4020; //+ (7680 * 10); //+ 7680 + 7680; // 6350;
            //ServerTime = 453600;

            //ServerTime = (((210 + 475) + (475 * 8)) + ((7618.75 * 29) + (7618.75 * 30) + (7618.75 * 30) + (7618.75 * 30) + (7618.75 * 30) + (7618.75 * 30) + (7618.75 * 30) + (7618.75 * 30) + (7618.75 * 30))) + (7618.75 * 359); //+ (7555 * 30) + (7555 * 30) + (7555 * 31); //(7608 * 30); //+ (7608 * 30); //4020; //+ (7680 * 10); //+ 7680 + 7680; // 6350;
            //ServerTime = (((210 + 475) + (475 * 8)) + ((7618.75 * 29) + (7618.75 * 30) + (7618.75 * 30) + (7618.75 * 30) + (7618.75 * 30) + (7618.75 * 30) + (7618.75 * 30) + (7618.75 * 30) + (7618.75 * 30))); //+ (7555 * 30) + (7555 * 30) + (7555 * 31); //(7608 * 30); //+ (7608 * 30); //4020; //+ (7680 * 10); //+ 7680 + 7680; // 6350;
            //ServerTime = (((210 + 475) + (475 * 8)) + ((7618.75 * 29) + (7618.75 * 30) + (7618.75 * 30) + (7618.75 * 30) + (7618.75 * 30) + (7618.75 * 30) + (7618.75 * 30) + (7618.75 * 30) + (7618.75 * 30))); //+ (7555 * 30) + (7555 * 30) + (7555 * 31); //(7608 * 30); //+ (7608 * 30); //4020; //+ (7680 * 10); //+ 7680 + 7680; // 6350;
            //ServerTime = 2053800;
            //ServerTime = 2053800 + (7618.75 * 30);
            //ServerTime = 2053800 + (7620 * 30) + (7620 * 30) + (7620 * 30) + (7620 * 30) + (7620 * 30) + (7620 * 30) + (7620 * 30) + (7620 * 30) + (7620 * 30) + (7620 * 30) + (7620 * 30) + (7620 * 30);
            //ServerTime = 2053800 + ((7620 * (30 * 12) * 390));
            //ServerTime = (476.875 * 9) + (7620 * 29);
            //ServerTime = Convert.ToDouble(DerethDateTime.UtcNowtoDerethTime());
            //ServerTime = DerethDateTime.MaxValue;
            //DerethDateTime.Today.
            //ServerTime = 1073741824;
            //System.Diagnostics.Debug.WriteLine(DerethDateTime.MaxValue - ServerTime);

            //DerethDateTime blah = new DerethDateTime((476.875 * 18));
            //DerethDateTime blah = new DerethDateTime((2053800 - 7620) + (476.875 * 15));
            //DerethDateTime blah = new DerethDateTime(2053800 + (476.875 * 250));
            //DerethDateTime blah = new DerethDateTime(2053800 + ((7620 * (30 * 12) * 2)));
            //DerethDateTime blah = new DerethDateTime(2053800 + ((7620 * (30 * 12) * 105)));
            //DerethDateTime blah = new DerethDateTime(0 + ((7620 * (30 * 12) * 105)));
            //DerethDateTime blah = new DerethDateTime(2053800);
            //DerethDateTime blah = new DerethDateTime(DerethDateTime.MaxValue - (7620 * (30 * 36)));
            //DerethDateTime blah = new DerethDateTime(DerethDateTime.MaxValue);
            //DerethDateTime blah = new DerethDateTime(DerethDateTime.MaxValue - 476.25);
            //DerethDateTime blah = new DerethDateTime(83843749);
            //DerethDateTime blah = new DerethDateTime(105201.15552052);
            //DerethDateTime blah = new DerethDateTime(675 * 4);
            //DerethDateTime blah = new DerethDateTime(925.75);
            //((210 + 475) + (475 * 8)) + (7611.25 * 29); //+ (7555 * 30) + (7555 * 30) + (7555 * 31)
            //DerethDateTime blah = new DerethDateTime(((210 + 475) + (475 * 8)) + (7611.25 * 29) + 11123453.3345222);
            //DerethDateTime blah = new DerethDateTime(hour: (int)DerethDateTime.Hours.Evensong_and_Half);
            //DerethDateTime blah = new DerethDateTime(year: 11);
            //DerethDateTime blah = new DerethDateTime(month: 7, day: 30, year: 27, hour: 12);
            //DerethDateTime blah = new DerethDateTime(month: (int)DerethDateTime.Months.Seedsow, day: 18, year: 157, hour: 12);
            //DerethDateTime blah = new DerethDateTime(month: (int)DerethDateTime.Months.Seedsow, day: 18, hour: 12);
            //DerethDateTime blah = new DerethDateTime(month: (int)DerethDateTime.Months.Coldeve, hour: 9);
            //DerethDateTime blah = new DerethDateTime(month: (int)DerethDateTime.Months.Coldeve, year: 11, hour: 9);
            //DerethDateTime blah = new DerethDateTime(month: (int)DerethDateTime.Months.Coldeve, year: 241, hour: 2);
            //DerethDateTime blah = new DerethDateTime(month: (int)DerethDateTime.Months.Snowreap, day: 21, hour: 2);
            //DerethDateTime blah = new DerethDateTime(month: (int)DerethDateTime.Months.Snowreap, day: 21, year: 11, hour: 2);
            //DerethDateTime blah = new DerethDateTime(month: (int)DerethDateTime.Months.Snowreap, day: 21, year: 21, hour: 2);
            //DerethDateTime blah = new DerethDateTime(month: (int)DerethDateTime.Months.Morningthaw, day: 21, hour: 2);
            //DerethDateTime blah = new DerethDateTime(month: (int)DerethDateTime.Months.Morningthaw, year: 11, day: 21, hour: 2);
            //DerethDateTime blah = new DerethDateTime(month: (int)DerethDateTime.Months.Solclaim, day: 21, hour: 2);
            //DerethDateTime blah = new DerethDateTime(month: (int)DerethDateTime.Months.Solclaim, year: 11, day: 21, hour: 2);
            //DerethDateTime blah = new DerethDateTime(month: (int)DerethDateTime.Months.Solclaim, year: 97, day: 7, hour: 6);
            //DerethDateTime blah = new DerethDateTime(month: (int)DerethDateTime.Months.Wintersebb, hour: 9);
            //DerethDateTime blah = new DerethDateTime(month: (int)DerethDateTime.Months.Wintersebb, year: 11, hour: 9);
            //DerethDateTime blah = new DerethDateTime(month: (int)DerethDateTime.Months.Wintersebb, year: 12, hour: 9);
            //DerethDateTime blah = new DerethDateTime(month: (int)DerethDateTime.Months.Wintersebb, year: 19, hour: 9);
            //DerethDateTime blah = new DerethDateTime(10,DerethDateTime.Months.Solclaim);
            //DerethDateTime blah = new DerethDateTime().AddMonths(12).AddMonths(12).AddMonths(-4);//.AddMonths(3);
            //DerethDateTime blah = new DerethDateTime().AddMonths(12).AddDays(359).AddDays(1).AddDays(-2);
            //DerethDateTime blah = new DerethDateTime().AddMonths(12).AddDays(359).AddDays(1).AddDays(-2).AddHours(10).AddHours(-1).AddHours(-2).AddHours(18).AddHours(-1);
            //DerethDateTime blah = new DerethDateTime().AddMonths(12).SubtractMonths(1);
            //System.Diagnostics.Debug.WriteLine(blah.ToString());
            //ServerTime = blah.Ticks;
            //blah.test();

            ServerTime = WorldManager.PortalYearTicks;
        }
    }
}
