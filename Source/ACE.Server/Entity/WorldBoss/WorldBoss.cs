using ACE.Common;
using ACE.Entity;
using ACE.Server.Entity.TownControl;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Server.Entity.WorldBoss
{
    public static class WorldBosses
    {
        private static Dictionary<uint, WorldBoss> _worldBossMap;
        public static Dictionary<uint, WorldBoss> WorldBossMap
        {
            get
            {
                if (_worldBossMap == null)
                {
                    _worldBossMap = new Dictionary<uint, WorldBoss>();

                    //BZ near Ayan
                    var bz = new WorldBoss();
                    bz.WeenieID = 490011;
                    bz.Name = "Bael'Zharon";
                    bz.SpawnMsg = "A sudden burst of deep black and sickly red erupts in the distant sky towards Ayan Baqur, unimaginably bright yet devoid of light. Seconds later the ground lurches chaotically around you as the air grows thick and oily with the taste of rot. Pain rips through your consciousness like a searing electrical surge and an unwelcome image of a terrible visage thrashes loose in your mind. The image cannot be mistaken; Bael'Zharon roams the lands, thirsting for the blood of all challengers.";
                    bz.SpawnLocations = new Dictionary<uint, Position>();

                    //0x163B0015 [57.036785 116.795227 78.005005] -0.826930 0.000000 0.000000 -0.562305
                    bz.SpawnLocations.Add(0x163B, new Position(0x163B0015, 57.036785f, 116.795227f, 78.005005f, 0f, 0f, -0.562305f, -0.826930f));

                    //0x142D0025 [106.761009 101.662949 18.004999] 0.859963 0.000000 0.000000 -0.510356
                    bz.SpawnLocations.Add(0x142D, new Position(0x142D0025, 106.761009f, 101.662949f, 18.004999f, 0f, 0f, -0.510356f, 0.859963f));

                    //0x132D002F [132.212296 155.478958 -0.895000] 0.185605 0.000000 0.000000 0.982624
                    bz.SpawnLocations.Add(0x132D, new Position(0x132D002F, 132.212296f, 155.478958f, -0.895000f, 0f, 0f, 0.982624f, 0.185605f));

                    //0x123C002C [140.998459 93.142860 60.005001] -0.082937 0.000000 0.000000 0.996555
                    bz.SpawnLocations.Add(0x123C, new Position(0x123C002C, 140.998459f, 93.142860f, 60.005001f, 0f, 0f, 0.996555f, -0.082937f));

                    //0x1139003F [171.123154 156.852219 0.005000] -0.151193 0.000000 0.000000 0.988504
                    bz.SpawnLocations.Add(0x1139, new Position(0x1139003F, 171.123154f, 156.852219f, 0.005000f, 0f, 0f, 0.988504f, -0.151193f));

                    //0x10390019 [77.843712 23.334404 46.005001] 0.435064 0.000000 0.000000 -0.900399
                    bz.SpawnLocations.Add(0x1039, new Position(0x10390019, 77.843712f, 23.334404f, 46.005001f, 0f, 0f, -0.900399f, 0.435064f));

                    //0x10370027 [104.356216 149.221420 58.005001] 0.280491 0.000000 0.000000 -0.959857
                    bz.SpawnLocations.Add(0x1037, new Position(0x10370027, 104.356216f, 149.221420f, 58.005001f, 0f, 0f, -0.959857f, 0.280491f));

                    //0x1831000F [33.068218 165.019897 90.005005] 0.641824 0.000000 0.000000 0.766852
                    bz.SpawnLocations.Add(0x1831, new Position(0x1831000F, 33.068218f, 165.019897f, 90.005005f, 0f, 0f, 0.766852f, 0.641824f));

                    //0x1932001F [75.145576 159.921112 18.004999] 0.999579 0.000000 0.000000 -0.028997
                    bz.SpawnLocations.Add(0x1932, new Position(0x1932001F, 75.145576f, 159.921112f, 18.004999f, 0f, 0f, -0.028997f, 0.999579f));

                    //0x1A330032 [156.810760 24.869272 46.077442] 0.879722 0.000000 0.000000 0.475488
                    bz.SpawnLocations.Add(0x1A33, new Position(0x1A330032, 156.810760f, 24.869272f, 46.077442f, 0f, 0f, 0.475488f, 0.879722f));

                    //0x1B3A003D [185.901779 98.714279 38.005001] 0.857765 0.000000 0.000000 0.514041
                    bz.SpawnLocations.Add(0x1B3A, new Position(0x1B3A003D, 185.901779f, 98.714279f, 38.005001f, 0f, 0f, 0.514041f, 0.857765f));

                    //0x1A3C002B [132.808884 65.670265 72.005005] -0.699983 0.000000 0.000000 -0.714159
                    bz.SpawnLocations.Add(0x1A3C, new Position(0x1A3C002B, 132.808884f, 65.670265f, 72.005005f, 0f, 0f, -0.714159f, -0.699983f));

                    //0x173D0035 [163.126877 102.190727 40.005001] -0.539252 0.000000 0.000000 -0.842145
                    bz.SpawnLocations.Add(0x173D, new Position(0x173D0035, 163.126877f, 102.190727f, 40.005001f, 0f, 0f, -0.842145f, -0.539252f));

                    //0x183F0033 [149.667404 53.657219 80.005005] 0.594196 0.000000 0.000000 0.804321
                    bz.SpawnLocations.Add(0x183F, new Position(0x183F0033, 149.667404f, 53.657219f, 80.005005f, 0f, 0f, 0.804321f, 0.594196f));

                    //0x153D002D [132.513412 113.941681 60.005001] 0.999841 0.000000 0.000000 -0.017848
                    bz.SpawnLocations.Add(0x153D, new Position(0x153D002D, 132.513412f, 113.941681f, 60.005001f, 0f, 0f, -0.017848f, 0.999841f));

                    //0x173A001B [92.401810 66.428429 38.005001] -0.038401 0.000000 0.000000 -0.999262
                    bz.SpawnLocations.Add(0x173A, new Position(0x173A001B, 92.401810f, 66.428429f, 38.005001f, 0f, 0f, -0.999262f, -0.038401f));

                    //0x16340003 [18.660233 50.542988 40.005001] -0.009317 0.000000 0.000000 0.999957
                    bz.SpawnLocations.Add(0x1634, new Position(0x16340003, 18.660233f, 50.542988f, 40.005001f, 0f, 0f, 0.999957f, -0.009317f));

                    //0x1531001A [83.563210 34.917675 38.058796] -0.990832 0.000000 0.000000 0.135100
                    bz.SpawnLocations.Add(0x1531, new Position(0x1531001A, 83.563210f, 34.917675f, 38.058796f, 0f, 0f, 0.135100f, -0.990832f));

                    //0x1339000D [35.624466 106.731056 -0.895000] -0.484776 0.000000 0.000000 -0.874638
                    bz.SpawnLocations.Add(0x1339, new Position(0x1339000D, 35.624466f, 106.731056f, -0.895000f, 0f, 0f, -0.874638f, -0.484776f));

                    //0x13430037 [160.628510 144.946091 60.005001] 0.265608 0.000000 0.000000 -0.964081
                    bz.SpawnLocations.Add(0x1343, new Position(0x13430037, 160.628510f, 144.946091f, 60.005001f, 0f, 0f, -0.964081f, 0.265608f));

                    //0x14420031 [144.907990 23.576054 60.005001] 0.019788 0.000000 0.000000 -0.999804
                    bz.SpawnLocations.Add(0x1442, new Position(0x14420031, 144.907990f, 23.576054f, 60.005001f, 0f, 0f, -0.999804f, 0.019788f));

                    //0x1441002C [142.531723 87.262405 -0.445000] -0.712602 0.000000 0.000000 -0.701568
                    bz.SpawnLocations.Add(0x1441, new Position(0x1441002C, 142.531723f, 87.262405f, -0.445000f, 0f, 0f, -0.701568f, -0.712602f));

                    //0x163A001B [82.981041 52.481529 38.005001] 0.792871 0.000000 0.000000 0.609390
                    bz.SpawnLocations.Add(0x163A, new Position(0x163A001B, 82.981041f, 52.481529f, 38.005001f, 0f, 0f, 0.609390f, 0.792871f));

                    //0x1234001B [83.548294 66.071083 0.005000] 0.777441 0.000000 0.000000 0.628955
                    bz.SpawnLocations.Add(0x1234, new Position(0x1234001B, 83.548294f, 66.071083f, 0.005000f, 0f, 0f, 0.628955f, 0.777441f));

                    //0x0F34001C [91.332733 91.767746 0.005000] 0.827183 0.000000 0.000000 -0.561932
                    bz.SpawnLocations.Add(0x0F34, new Position(0x0F34001C, 91.332733f, 91.767746f, 0.005000f, 0f, 0f, -0.561932f, 0.827183f));

                    //0x26100034 [152.342300 77.060448 10.004999] 0.942787 0.000000 0.000000 -0.333397
                    bz.SpawnLocations.Add(0x2610, new Position(0x26100034, 152.342300f, 77.060448f, 10.004999f, 0f, 0f, -0.333397f, 0.942787f));

                    _worldBossMap.Add(490011, bz);

                    //Martine south of Candeth
                    var martine = new WorldBoss();
                    martine.WeenieID = 490039;
                    martine.Name = "Martine";
                    martine.SpawnMsg = "A distant shriek of madness echoes through the air, seeming to come from both nowhere and everywhere. The shriek resolves into a voice, hollow and intense, desparately searching for something long lost. The sound fades to a whisper, almost a whimper and then a sob. Through the tortured sounds floats a name; Melanay. With a timbre of deep desire that flashes to lost hope, Sir Candeth Martine scours the lands in search of his beloved, thirsting for revenge against every soul who has ever stood in his way.";
                    martine.SpawnLocations = new Dictionary<uint, Position>();

                    //0x2A0E003B [179.955505 68.841629 11.264489] 0.915736 0.000000 0.000000 0.401780
                    martine.SpawnLocations.Add(0x2A0E, new Position(0x2A0E003B, 179.955505f, 68.841629f, 11.264489f, 0f, 0f, 0.401780f, 0.915736f));

                    //0x1D17000E [47.073410 143.592133 74.005005] 0.209154 0.000000 0.000000 -0.977883
                    martine.SpawnLocations.Add(0x1D17, new Position(0x1D17000E, 47.073410f, 143.592133f, 74.005005f, 0f, 0f, -0.977883f, 0.209154f));

                    //0x280F0009 [45.150307 21.003601 16.004999] 0.865324 0.000000 0.000000 -0.501213
                    martine.SpawnLocations.Add(0x280F, new Position(0x280F0009, 45.150307f, 21.003601f, 16.004999f, 0f, 0f, -0.501213f, 0.865324f));

                    //0x24120033 [144.176544 50.511131 56.005001] 0.886703 0.000000 0.000000 -0.462340
                    martine.SpawnLocations.Add(0x2412, new Position(0x24120033, 144.176544f, 50.511131f, 56.005001f, 0f, 0f, -0.462340f, 0.886703f));

                    //0x2514000A [42.951023 24.837976 10.846496] 0.670492 0.000000 0.000000 -0.741917
                    martine.SpawnLocations.Add(0x2514, new Position(0x2514000A, 42.951023f, 24.837976f, 10.846496f, 0f, 0f, -0.741917f, 0.670492f));

                    //0x25160001 [3.861884 10.861797 56.005001] 0.931599 0.000000 0.000000 -0.363489
                    martine.SpawnLocations.Add(0x2516, new Position(0x25160001, 3.861884f, 10.861797f, 56.005001f, 0f, 0f, -0.363489f, 0.931599f));

                    //0x261B003C [186.582825 75.783958 56.005001] 0.199859 0.000000 0.000000 -0.979825
                    martine.SpawnLocations.Add(0x261B, new Position(0x261B003C, 186.582825f, 75.783958f, 56.005001f, 0f, 0f, -0.979825f, 0.199859f));

                    //0x2B19001A [73.045929 29.875187 84.005005] 0.131555 0.000000 0.000000 -0.991309
                    martine.SpawnLocations.Add(0x2B19, new Position(0x2B19001A, 73.045929f, 29.875187f, 84.005005f, 0f, 0f, -0.991309f, 0.131555f));

                    //0x2B13003E [187.855286 124.680305 56.005001] -0.177765 0.000000 0.000000 -0.984073
                    martine.SpawnLocations.Add(0x2B13, new Position(0x2B13003E, 187.855286f, 124.680305f, 56.005001f, 0f, 0f, -0.984073f, -0.177765f));

                    //0x2D15003F [171.475052 161.428146 74.005005] -0.738025 0.000000 0.000000 -0.674773
                    martine.SpawnLocations.Add(0x2D15, new Position(0x2D15003F, 171.475052f, 161.428146f, 74.005005f, 0f, 0f, -0.674773f, -0.738025f));

                    //0x31170019 [72.499748 23.279175 19.801571] 0.183725 0.000000 0.000000 0.982978
                    martine.SpawnLocations.Add(0x3117, new Position(0x31170019, 72.499748f, 23.279175f, 19.801571f, 0f, 0f, 0.982978f, 0.183725f));

                    //0x36130020 [73.677101 177.614609 24.004999] -0.484036 0.000000 0.000000 -0.875048
                    martine.SpawnLocations.Add(0x3613, new Position(0x36130020, 73.677101f, 177.614609f, 24.004999f, 0f, 0f, -0.875048f, -0.484036f));

                    //0x3611000B [33.384853 59.054295 12.004999] -0.662756 0.000000 0.000000 -0.748835
                    martine.SpawnLocations.Add(0x3611, new Position(0x3611000B, 33.384853f, 59.054295f, 12.004999f, 0f, 0f, -0.748835f, -0.662756f));

                    //0x310E0023 [115.807823 54.071014 11.848429] -0.999991 0.000000 0.000000 0.004157
                    martine.SpawnLocations.Add(0x310E, new Position(0x310E0023, 115.807823f, 54.071014f, 11.848429f, 0f, 0f, 0.004157f, -0.999991f));

                    //0x2F0C0030 [141.317337 172.003052 0.338588] -0.996610 0.000000 0.000000 -0.082270
                    martine.SpawnLocations.Add(0x2F0C, new Position(0x2F0C0030, 141.317337f, 172.003052f, 0.338588f, 0f, 0f, -0.082270f, -0.996610f));

                    //0x2B0D0010 [27.147856 173.740601 8.267321] -0.998086 0.000000 0.000000 0.061834
                    martine.SpawnLocations.Add(0x2B0D, new Position(0x2B0D0010, 27.147856f, 173.740601f, 8.267321f, 0f, 0f, 0.061834f, -0.998086f));

                    //0x27180024 [113.499283 84.738968 56.005001] -0.939718 0.000000 0.000000 0.341951
                    martine.SpawnLocations.Add(0x2718, new Position(0x27180024, 113.499283f, 84.738968f, 56.005001f, 0f, 0f, 0.341951f, -0.939718f));

                    //0x3313001F [72.261925 149.403015 56.005001] -0.053219 0.000000 0.000000 0.998583
                    martine.SpawnLocations.Add(0x3313, new Position(0x3313001F, 72.261925f, 149.403015f, 56.005001f, 0f, 0f, 0.998583f, -0.053219f));

                    //0x33110036 [145.756546 121.258209 14.004999] 0.333957 0.000000 0.000000 0.942588
                    martine.SpawnLocations.Add(0x3311, new Position(0x33110036, 145.756546f, 121.258209f, 14.004999f, 0f, 0f, 0.942588f, 0.333957f));

                    //0x35120031 [148.236984 4.453219 10.004999] -0.996521 0.000000 0.000000 0.083343
                    martine.SpawnLocations.Add(0x3512, new Position(0x35120031, 148.236984f, 4.453219f, 10.004999f, 0f, 0f, 0.083343f, -0.996521f));

                    //0x2F13002A [137.784042 26.336439 56.005001] 0.887972 0.000000 0.000000 -0.459897
                    martine.SpawnLocations.Add(0x2F13, new Position(0x2F13002A, 137.784042f, 26.336439f, 56.005001f, 0f, 0f, -0.459897f, 0.887972f));

                    //0x2A120018 [64.273781 182.438965 56.005001] 0.579746 0.000000 0.000000 -0.814797
                    martine.SpawnLocations.Add(0x2A12, new Position(0x2A120018, 64.273781f, 182.438965f, 56.005001f, 0f, 0f, -0.814797f, 0.579746f));

                    //0x2D130028 [99.093803 176.942490 12.979780] 0.887528 0.000000 0.000000 0.460753
                    martine.SpawnLocations.Add(0x2D13, new Position(0x2D130028, 99.093803f, 176.942490f, 12.979780f, 0f, 0f, 0.460753f, 0.887528f));

                    //0x2B110020 [91.687714 171.896667 48.005001] -0.999231 0.000000 0.000000 -0.039208
                    martine.SpawnLocations.Add(0x2B11, new Position(0x2B110020, 91.687714f, 171.896667f, 48.005001f, 0f, 0f, -0.039208f, -0.999231f));

                    _worldBossMap.Add(490039, martine);

                    //Olthoi King in ML Plateau
                    var ok = new WorldBoss();
                    ok.WeenieID = 490010;
                    ok.Name = "Olthoi King";
                    ok.SpawnMsg = "A sudden acidic stench fills the air, presaging the emergence of a terrible beast. From the direction of the Olthoi wastelands to the northeast, a deluge of unmistakably insectoid shrieks and rattles pours fourth; an acrid cacophony of pestilence and a challenge to the temerity of man. Deep within the caustic land of the Olthoi, a King has risen. Go forth to steal glory from the clutches of certain death.";
                    ok.SpawnLocations = new Dictionary<uint, Position>();

                    //0xC9BE0027 [97.721176 167.088745 27.937508] 0.970669 0.000000 0.000000 -0.240419
                    ok.SpawnLocations.Add(0xC9BE, new Position(0xC9BE0027, 97.721176f, 167.088745f, 27.937508f, 0f, 0f, -0.240419f, 0.970669f));

                    //0xC6BC0009 [24.430271 3.501464 111.933289] 0.887970 0.000000 0.000000 -0.459901
                    ok.SpawnLocations.Add(0xC6BC, new Position(0xC6BC0009, 24.430271f, 3.501464f, 111.933289f, 0f, 0f, -0.459901f, 0.887970f));

                    //0xC4B7003C [181.098526 79.573730 215.373856] 0.900816 0.000000 0.000000 -0.434202
                    ok.SpawnLocations.Add(0xC4B7, new Position(0xC4B7003C, 181.098526f, 79.573730f, 215.373856f, 0f, 0f, -0.434202f, 0.900816f));

                    //0xC7BB000A [27.782013 35.803383 132.706207] 0.826360 0.000000 0.000000 -0.563142
                    ok.SpawnLocations.Add(0xC7BB, new Position(0xC7BB000A, 27.782013f, 35.803383f, 132.706207f, 0f, 0f, -0.563142f, 0.826360f));

                    //0xC8BB0028 [117.505363 185.801392 85.589226] 0.229214 0.000000 0.000000 -0.973376
                    ok.SpawnLocations.Add(0xC8BB, new Position(0xC8BB0028, 117.505363f, 185.801392f, 85.589226f, 0f, 0f, -0.973376f, 0.229214f));

                    //0xCBBB002F [137.317551 155.121414 52.561871] -0.542615 0.000000 0.000000 -0.839981
                    ok.SpawnLocations.Add(0xCBBB, new Position(0xCBBB002F, 137.317551f, 155.121414f, 52.561871f, 0f, 0f, -0.839981f, -0.542615f));

                    //0xCEBA0018 [48.804699 169.226517 54.005001] 0.599044 0.000000 0.000000 -0.800716
                    ok.SpawnLocations.Add(0xCEBA, new Position(0xCEBA0018, 48.804699f, 169.226517f, 54.005001f, 0f, 0f, -0.800716f, 0.599044f));

                    //0xD2BB0026 [116.180519 130.383835 166.004990] 0.256708 0.000000 0.000000 -0.966489
                    ok.SpawnLocations.Add(0xD2BB, new Position(0xD2BB0026, 116.180519f, 130.383835f, 166.004990f, 0f, 0f, -0.966489f, 0.256708f));

                    //0xD0BC0038 [150.253235 188.561447 67.333755] 0.140133 0.000000 0.000000 0.990133
                    ok.SpawnLocations.Add(0xD0BC, new Position(0xD0BC0038, 150.253235f, 188.561447f, 67.333755f, 0f, 0f, 0.990133f, 0.140133f));

                    //0xCDBD0012 [71.539680 29.330975 30.004999] 0.593417 0.000000 0.000000 0.804895
                    ok.SpawnLocations.Add(0xCDBD, new Position(0xCDBD0012, 71.539680f, 29.330975f, 30.004999f, 0f, 0f, 0.804895f, 0.593417f));

                    //0xCABE003A [174.442413 26.033909 25.468132] 0.996222 0.000000 0.000000 0.086847
                    ok.SpawnLocations.Add(0xCABE, new Position(0xCABE003A, 174.442413f, 26.033909f, 25.468132f, 0f, 0f, 0.086847f, 0.996222f));

                    //0xCCC1002D [137.092117 100.606575 0.005000] 0.671505 0.000000 0.000000 0.741000
                    ok.SpawnLocations.Add(0xCCC1, new Position(0xCCC1002D, 137.092117f, 100.606575f, 0.005000f, 0f, 0f, 0.741000f, 0.671505f));

                    //0xC9C20039 [190.563507 0.261763 0.005000] 0.541150 0.000000 0.000000 0.840926
                    ok.SpawnLocations.Add(0xC9C2, new Position(0xC9C20039, 190.563507f, 0.261763f, 0.005000f, 0f, 0f, 0.840926f, 0.541150f));

                    //0xC7C00027 [96.086555 159.233353 29.997787] 0.127236 0.000000 0.000000 0.991872
                    ok.SpawnLocations.Add(0xC7C0, new Position(0xC7C00027, 96.086555f, 159.233353f, 29.997787f, 0f, 0f, 0.991872f, 0.127236f));

                    //0xC6BE0024 [101.417053 76.680870 109.553581] -0.562903 0.000000 0.000000 0.826523
                    ok.SpawnLocations.Add(0xC6BE, new Position(0xC6BE0024, 101.417053f, 76.680870f, 109.553581f, 0f, 0f, 0.826523f, -0.562903f));

                    //0xC9BB0017 [53.951443 148.253342 99.296104] -0.585335 0.000000 0.000000 0.810792
                    ok.SpawnLocations.Add(0xC9BB, new Position(0xC9BB0017, 53.951443f, 148.253342f, 99.296104f, 0f, 0f, 0.810792f, -0.585335f));

                    //0xCABB0038 [150.420013 171.969101 59.800758] -0.446019 0.000000 0.000000 0.895023
                    ok.SpawnLocations.Add(0xCABB, new Position(0xCABB0038, 150.420013f, 171.969101f, 59.800758f, 0f, 0f, 0.895023f, -0.446019f));

                    //0xCDBC0002 [11.616997 41.764633 44.005001] -0.660094 0.000000 0.000000 0.751183
                    ok.SpawnLocations.Add(0xCDBC, new Position(0xCDBC0002, 11.616997f, 41.764633f, 44.005001f, 0f, 0f, 0.751183f, -0.660094f));

                    //0xC9BA0039 [168.335648 5.163098 95.977036] -0.999862 0.000000 0.000000 0.016583
                    ok.SpawnLocations.Add(0xC9BA, new Position(0xC9BA0039, 168.335648f, 5.163098f, 95.977036f, 0f, 0f, 0.016583f, -0.999862f));

                    //0xC6B70020 [72.729393 172.095108 148.004990] -0.968973 0.000000 0.000000 -0.247166
                    ok.SpawnLocations.Add(0xC6B7, new Position(0xC6B70020, 72.729393f, 172.095108f, 148.004990f, 0f, 0f, -0.247166f, -0.968973f));

                    //0xC3B70020 [79.152260 174.389542 183.472534] 0.878718 0.000000 0.000000 -0.477340
                    ok.SpawnLocations.Add(0xC3B7, new Position(0xC3B70020, 79.152260f, 174.389542f, 183.472534f, 0f, 0f, -0.477340f, 0.878718f));

                    //0xC2BE0030 [132.672089 178.534790 128.315292] 0.961621 0.000000 0.000000 -0.274382
                    ok.SpawnLocations.Add(0xC2BE, new Position(0xC2BE0030, 132.672089f, 178.534790f, 128.315292f, 0f, 0f, -0.274382f, 0.961621f));

                    //0xC3BC0014 [61.329491 92.074692 222.894196] 0.992730 0.000000 0.000000 0.120365
                    ok.SpawnLocations.Add(0xC3BC, new Position(0xC3BC0014, 61.329491f, 92.074692f, 222.894196f, 0f, 0f, 0.120365f, 0.992730f));

                    _worldBossMap.Add(490010, ok);
                }

                return _worldBossMap;
            }
        }

        public static bool IsWorldBoss(uint weenieId)
        {
            return WorldBosses.WorldBossMap.ContainsKey(weenieId);
        }

        public static WorldBoss GetRandomWorldBoss()
        {
            var i = ThreadSafeRandom.Next(0, WorldBosses.WorldBossMap.Count - 1);
            return WorldBosses.WorldBossMap.Values.ElementAt(i);
        }        
    }

    public class WorldBoss
    {
        public uint WeenieID { get; set; }

        public string Name { get; set; }

        public string SpawnMsg { get; set; }

        public Position Location { get; set; }

        public Dictionary<uint, Position> SpawnLocations { get; set; }

        public KeyValuePair<uint, Position> RollRandomSpawnLocation()
        {
            var randomKey = SpawnLocations.Keys.OrderBy(x => Guid.NewGuid()).First();
            return new KeyValuePair<uint,Position>(randomKey, SpawnLocations[randomKey]);
        }        
    }
}
