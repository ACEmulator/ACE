DELETE FROM `landblock_instance` WHERE `landblock` = 0x007F;

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F000, 87532, 0x007F02EC, 10, -840.747, -0.063, 1, 0, 0, 0, False, '2021-11-01 00:00:00'); /* Surface */
/* @teleloc 0x007F02EC [10.000000 -840.747009 -0.063000] 1.000000 0.000000 0.000000 0.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F001,  7924, 0x007F02E2, -3.61739, -819.68, 0.055, -0.041692, 0, 0, 0.99913, False, '2021-11-01 00:00:00'); /* Linkable Monster Generator ( 5 Min.) */
/* @teleloc 0x007F02E2 [-3.617390 -819.679993 0.055000] -0.041692 0.000000 0.000000 0.999130 */

INSERT INTO `landblock_instance_link` (`parent_GUID`, `child_GUID`, `last_Modified`)
VALUES (0x7007F001, 0x7007F002, '2021-11-01 00:00:00') /* Viamontian Tribune (30291) */
     , (0x7007F001, 0x7007F003, '2021-11-01 00:00:00') /* Viamontian Tribune (30291) */
     , (0x7007F001, 0x7007F004, '2021-11-01 00:00:00') /* Royal Thaumaturge (30297) */
     , (0x7007F001, 0x7007F005, '2021-11-01 00:00:00') /* Royal Thaumaturge (30297) */
     , (0x7007F001, 0x7007F006, '2021-11-01 00:00:00') /* Royal Thaumaturge (30297) */
     , (0x7007F001, 0x7007F007, '2021-11-01 00:00:00') /* Viamontian Tribune (30291) */
     , (0x7007F001, 0x7007F008, '2021-11-01 00:00:00') /* Viamontian Tribune (30291) */
     , (0x7007F001, 0x7007F009, '2021-11-01 00:00:00') /* Viamontian Tribune (30291) */
     , (0x7007F001, 0x7007F00A, '2021-11-01 00:00:00') /* Viamontian Tribune (30291) */
     , (0x7007F001, 0x7007F00B, '2021-11-01 00:00:00') /* Royal Thaumaturge (30297) */
     , (0x7007F001, 0x7007F00C, '2021-11-01 00:00:00') /* Royal Thaumaturge (30297) */
     , (0x7007F001, 0x7007F00D, '2021-11-01 00:00:00') /* Royal Thaumaturge (30297) */
     , (0x7007F001, 0x7007F00E, '2021-11-01 00:00:00') /* Viamontian Tribune (30291) */
     , (0x7007F001, 0x7007F00F, '2021-11-01 00:00:00') /* Viamontian Tribune (30291) */
     , (0x7007F001, 0x7007F010, '2021-11-01 00:00:00') /* Royal Thaumaturge (30297) */
     , (0x7007F001, 0x7007F011, '2021-11-01 00:00:00') /* Viamontian Tribune (30291) */
     , (0x7007F001, 0x7007F012, '2021-11-01 00:00:00') /* Viamontian Tribune (30291) */
     , (0x7007F001, 0x7007F013, '2021-11-01 00:00:00') /* Royal Thaumaturge (30297) */
     , (0x7007F001, 0x7007F014, '2021-11-01 00:00:00') /* Royal Thaumaturge (30297) */
     , (0x7007F001, 0x7007F015, '2021-11-01 00:00:00') /* Viamontian Tribune (30291) */
     , (0x7007F001, 0x7007F016, '2021-11-01 00:00:00') /* Royal Thaumaturge (30297) */
     , (0x7007F001, 0x7007F017, '2021-11-01 00:00:00') /* Royal Thaumaturge (30297) */
     , (0x7007F001, 0x7007F018, '2021-11-01 00:00:00') /* Viamontian Tribune (30291) */
     , (0x7007F001, 0x7007F019, '2021-11-01 00:00:00') /* Viamontian Tribune (30291) */
     , (0x7007F001, 0x7007F01A, '2021-11-01 00:00:00') /* Viamontian Tribune (30291) */
     , (0x7007F001, 0x7007F01B, '2021-11-01 00:00:00') /* Royal Thaumaturge (30297) */
     , (0x7007F001, 0x7007F01C, '2021-11-01 00:00:00') /* Viamontian Tribune (30291) */
     , (0x7007F001, 0x7007F01D, '2021-11-01 00:00:00') /* Viamontian Tribune (30291) */
     , (0x7007F001, 0x7007F01E, '2021-11-01 00:00:00') /* Summoning Chamber Adept (87530) */
     , (0x7007F001, 0x7007F01F, '2021-11-01 00:00:00') /* Summoning Chamber Adept (87530) */
     , (0x7007F001, 0x7007F020, '2021-11-01 00:00:00') /* Summoning Chamber Adept (87530) */
     , (0x7007F001, 0x7007F021, '2021-11-01 00:00:00') /* Summoning Chamber Adept (87530) */
     , (0x7007F001, 0x7007F022, '2021-11-01 00:00:00') /* Summoning Chamber Adept (87530) */
     , (0x7007F001, 0x7007F025, '2021-11-01 00:00:00') /* Adept of Fire (35128) */
     , (0x7007F001, 0x7007F026, '2021-11-01 00:00:00') /* Adept of Fire (35128) */
     , (0x7007F001, 0x7007F027, '2021-11-01 00:00:00') /* Adept of Lightning (35130) */
     , (0x7007F001, 0x7007F028, '2021-11-01 00:00:00') /* Adept of Lightning (35130) */
     , (0x7007F001, 0x7007F029, '2021-11-01 00:00:00') /* Viamontian Tribune (30291) */
     , (0x7007F001, 0x7007F02A, '2021-11-01 00:00:00') /* Viamontian Tribune (30291) */
     , (0x7007F001, 0x7007F02B, '2021-11-01 00:00:00') /* Viamontian Tribune (30291) */
     , (0x7007F001, 0x7007F02C, '2021-11-01 00:00:00') /* Viamontian Tribune (30291) */
     , (0x7007F001, 0x7007F02D, '2021-11-01 00:00:00') /* Viamontian Tribune (30291) */
     , (0x7007F001, 0x7007F02E, '2021-11-01 00:00:00') /* Adept of Fire (35128) */
     , (0x7007F001, 0x7007F02F, '2021-11-01 00:00:00') /* Adept of Lightning (35130) */;

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F002, 30291, 0x007F02E9, 6.41336, -807.57, 0.00679, 0.046259, 0, 0, 0.998929,  True, '2021-11-01 00:00:00'); /* Viamontian Tribune */
/* @teleloc 0x007F02E9 [6.413360 -807.570007 0.006790] 0.046259 0.000000 0.000000 0.998929 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F003, 30291, 0x007F02E9, 14.1422, -807.74, 0.00679, -0.003724, 0, 0, 0.999993,  True, '2021-11-01 00:00:00'); /* Viamontian Tribune */
/* @teleloc 0x007F02E9 [14.142200 -807.739990 0.006790] -0.003724 0.000000 0.000000 0.999993 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F004, 30297, 0x007F02AF, -0.150907, -727.525, -5.995, -0.2658, 0, 0, 0.964028,  True, '2021-11-01 00:00:00'); /* Royal Thaumaturge */
/* @teleloc 0x007F02AF [-0.150907 -727.525024 -5.995000] -0.265800 0.000000 0.000000 0.964028 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F005, 30297, 0x007F02AF, 3.7659, -734.088, -5.995, -0.2658, 0, 0, 0.964028,  True, '2021-11-01 00:00:00'); /* Royal Thaumaturge */
/* @teleloc 0x007F02AF [3.765900 -734.088013 -5.995000] -0.265800 0.000000 0.000000 0.964028 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F006, 30297, 0x007F02D3, 80.4182, -749.939, -5.995, 0.702598, 0, 0, 0.711587,  True, '2021-11-01 00:00:00'); /* Royal Thaumaturge */
/* @teleloc 0x007F02D3 [80.418198 -749.939026 -5.995000] 0.702598 0.000000 0.000000 0.711587 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F007, 30291, 0x007F02D3, 82.5704, -753.601, -5.99321, 0.702598, 0, 0, 0.711587,  True, '2021-11-01 00:00:00'); /* Viamontian Tribune */
/* @teleloc 0x007F02D3 [82.570396 -753.601013 -5.993210] 0.702598 0.000000 0.000000 0.711587 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F008, 30291, 0x007F02D3, 82.4698, -745.688, -5.99321, 0.702598, 0, 0, 0.711587,  True, '2021-11-01 00:00:00'); /* Viamontian Tribune */
/* @teleloc 0x007F02D3 [82.469803 -745.687988 -5.993210] 0.702598 0.000000 0.000000 0.711587 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F009, 30291, 0x007F027B, 69.2769, -680.342, -11.9932, 0.947632, 0, 0, -0.319365,  True, '2021-11-01 00:00:00'); /* Viamontian Tribune */
/* @teleloc 0x007F027B [69.276901 -680.341980 -11.993200] 0.947632 0.000000 0.000000 -0.319365 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F00A, 30291, 0x007F02A1, 91.2358, -658.977, -11.9932, -0.502398, 0, 0, -0.864636,  True, '2021-11-01 00:00:00'); /* Viamontian Tribune */
/* @teleloc 0x007F02A1 [91.235802 -658.976990 -11.993200] -0.502398 0.000000 0.000000 -0.864636 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F00B, 30297, 0x007F0279, 69.2068, -658.02, -11.995, 0.300644, 0, 0, -0.953737,  True, '2021-11-01 00:00:00'); /* Royal Thaumaturge */
/* @teleloc 0x007F0279 [69.206802 -658.020020 -11.995000] 0.300644 0.000000 0.000000 -0.953737 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F00C, 30297, 0x007F02A3, 90.1534, -680.343, -11.995, 0.851543, 0, 0, 0.524284,  True, '2021-11-01 00:00:00'); /* Royal Thaumaturge */
/* @teleloc 0x007F02A3 [90.153397 -680.343018 -11.995000] 0.851543 0.000000 0.000000 0.524284 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F00D, 30297, 0x007F0248, 79.9782, -590.039, -17.995, 0.01019, 0, 0, -0.999948,  True, '2021-11-01 00:00:00'); /* Royal Thaumaturge */
/* @teleloc 0x007F0248 [79.978203 -590.039001 -17.995001] 0.010190 0.000000 0.000000 -0.999948 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F00E, 30291, 0x007F0256, 90.1021, -591.333, -17.9932, -0.019076, 0, 0, 0.999818,  True, '2021-11-01 00:00:00'); /* Viamontian Tribune */
/* @teleloc 0x007F0256 [90.102097 -591.333008 -17.993200] -0.019076 0.000000 0.000000 0.999818 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F00F, 30291, 0x007F0234, 69.9702, -590.413, -17.9932, 0.010246, 0, 0, 0.999947,  True, '2021-11-01 00:00:00'); /* Viamontian Tribune */
/* @teleloc 0x007F0234 [69.970200 -590.413025 -17.993200] 0.010246 0.000000 0.000000 0.999947 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F010, 30297, 0x007F01FE, 50.3521, -538.844, -17.995, 0.367291, 0, 0, 0.930106,  True, '2021-11-01 00:00:00'); /* Royal Thaumaturge */
/* @teleloc 0x007F01FE [50.352100 -538.843994 -17.995001] 0.367291 0.000000 0.000000 0.930106 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F011, 30291, 0x007F01DA, 39.6409, -530.627, -17.9932, -0.019576, 0, 0, 0.999808,  True, '2021-11-01 00:00:00'); /* Viamontian Tribune */
/* @teleloc 0x007F01DA [39.640900 -530.627014 -17.993200] -0.019576 0.000000 0.000000 0.999808 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F012, 30291, 0x007F0211, 61.4359, -529.73, -17.9932, 0.520338, 0, 0, 0.853961,  True, '2021-11-01 00:00:00'); /* Viamontian Tribune */
/* @teleloc 0x007F0211 [61.435902 -529.729980 -17.993200] 0.520338 0.000000 0.000000 0.853961 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F013, 30297, 0x007F01F4, 50.1325, -519.056, -17.995, 0.408502, 0, 0, 0.912758,  True, '2021-11-01 00:00:00'); /* Royal Thaumaturge */
/* @teleloc 0x007F01F4 [50.132500 -519.056030 -17.995001] 0.408502 0.000000 0.000000 0.912758 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F014, 30297, 0x007F025D, 99.9325, -491.766, -17.995, -0.115519, 0, 0, -0.993305,  True, '2021-11-01 00:00:00'); /* Royal Thaumaturge */
/* @teleloc 0x007F025D [99.932503 -491.765991 -17.995001] -0.115519 0.000000 0.000000 -0.993305 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F015, 30291, 0x007F025D, 101.05, -489.347, -17.9932, -0.115519, 0, 0, -0.993305,  True, '2021-11-01 00:00:00'); /* Viamontian Tribune */
/* @teleloc 0x007F025D [101.050003 -489.346985 -17.993200] -0.115519 0.000000 0.000000 -0.993305 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F016, 30297, 0x007F02A5, 98.2246, -443.705, -11.995, 0.735914, 0, 0, 0.677075,  True, '2021-11-01 00:00:00'); /* Royal Thaumaturge */
/* @teleloc 0x007F02A5 [98.224602 -443.704987 -11.995000] 0.735914 0.000000 0.000000 0.677075 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F017, 30297, 0x007F0270, 63.2551, -444.166, -11.995, -0.774222, 0, 0, 0.632914,  True, '2021-11-01 00:00:00'); /* Royal Thaumaturge */
/* @teleloc 0x007F0270 [63.255100 -444.165985 -11.995000] -0.774222 0.000000 0.000000 0.632914 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F018, 30291, 0x007F023E, 84.1828, -427.799, -17.9932, 0.016429, 0, 0, 0.999865,  True, '2021-11-01 00:00:00'); /* Viamontian Tribune */
/* @teleloc 0x007F023E [84.182800 -427.799011 -17.993200] 0.016429 0.000000 0.000000 0.999865 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F019, 30291, 0x007F023E, 75.9925, -427.53, -17.9932, 0.016429, 0, 0, 0.999865,  True, '2021-11-01 00:00:00'); /* Viamontian Tribune */
/* @teleloc 0x007F023E [75.992500 -427.529999 -17.993200] 0.016429 0.000000 0.000000 0.999865 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F01A, 30291, 0x007F019A, 118.508, -410.191, -23.9932, 0.656689, 0, 0, 0.754162,  True, '2021-11-01 00:00:00'); /* Viamontian Tribune */
/* @teleloc 0x007F019A [118.508003 -410.191010 -23.993200] 0.656689 0.000000 0.000000 0.754162 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F01B, 30297, 0x007F019A, 120.921, -409.15, -23.995, 0.656689, 0, 0, 0.754162,  True, '2021-11-01 00:00:00'); /* Royal Thaumaturge */
/* @teleloc 0x007F019A [120.920998 -409.149994 -23.995001] 0.656689 0.000000 0.000000 0.754162 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F01C, 30291, 0x007F01A6, 160.217, -420.143, -23.9932, 0.189315, 0, 0, 0.981916,  True, '2021-11-01 00:00:00'); /* Viamontian Tribune */
/* @teleloc 0x007F01A6 [160.216995 -420.143005 -23.993200] 0.189315 0.000000 0.000000 0.981916 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F01D, 30291, 0x007F01AA, 160.601, -460.569, -23.9932, 0.995827, 0, 0, 0.09126,  True, '2021-11-01 00:00:00'); /* Viamontian Tribune */
/* @teleloc 0x007F01AA [160.600998 -460.569000 -23.993200] 0.995827 0.000000 0.000000 0.091260 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F01E, 87530, 0x007F01AC, 172.163, -437.173, -23.994, -0.715223, 0, 0, -0.698896,  True, '2021-11-01 00:00:00'); /* Summoning Chamber Adept */
/* @teleloc 0x007F01AC [172.162994 -437.173004 -23.993999] -0.715223 0.000000 0.000000 -0.698896 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F01F, 87530, 0x007F01AC, 172.03, -442.924, -23.994, -0.715223, 0, 0, -0.698896,  True, '2021-11-01 00:00:00'); /* Summoning Chamber Adept */
/* @teleloc 0x007F01AC [172.029999 -442.924011 -23.993999] -0.715223 0.000000 0.000000 -0.698896 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F020, 87530, 0x007F018D, 251.491, -440.219, -29.994, 0.712354, 0, 0, 0.701821,  True, '2021-11-01 00:00:00'); /* Summoning Chamber Adept */
/* @teleloc 0x007F018D [251.490997 -440.218994 -29.993999] 0.712354 0.000000 0.000000 0.701821 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F021, 87530, 0x007F0192, 256.241, -443.187, -29.994, 0.712354, 0, 0, 0.701821,  True, '2021-11-01 00:00:00'); /* Summoning Chamber Adept */
/* @teleloc 0x007F0192 [256.240997 -443.187012 -29.993999] 0.712354 0.000000 0.000000 0.701821 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F022, 87530, 0x007F0192, 256.33, -437.22, -29.994, 0.712354, 0, 0, 0.701821,  True, '2021-11-01 00:00:00'); /* Summoning Chamber Adept */
/* @teleloc 0x007F0192 [256.329987 -437.220001 -29.993999] 0.712354 0.000000 0.000000 0.701821 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F023, 87532, 0x007F0196, 272.023, -440.354, -30.063, 0.712354, 0, 0, 0.701821, False, '2021-11-01 00:00:00'); /* Surface */
/* @teleloc 0x007F0196 [272.023010 -440.354004 -30.063000] 0.712354 0.000000 0.000000 0.701821 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F024, 87533, 0x007F018B, 250.091, -419.422, -30.063, -0.01038, 0, 0, -0.999946, False, '2021-11-01 00:00:00'); /* Summoning Chamber */
/* @teleloc 0x007F018B [250.091003 -419.421997 -30.063000] -0.010380 0.000000 0.000000 -0.999946 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F025, 35128, 0x007F017D, 244.389, -280.011, -29.995, 0.659983, 0, 0, -0.75128,  True, '2021-11-01 00:00:00'); /* Adept of Fire */
/* @teleloc 0x007F017D [244.389008 -280.010986 -29.995001] 0.659983 0.000000 0.000000 -0.751280 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F026, 35128, 0x007F018F, 255.584, -259.961, -29.995, 0.601835, 0, 0, 0.798621,  True, '2021-11-01 00:00:00'); /* Adept of Fire */
/* @teleloc 0x007F018F [255.584000 -259.960999 -29.995001] 0.601835 0.000000 0.000000 0.798621 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F027, 35130, 0x007F0190, 255.808, -279.971, -29.995, 0.660336, 0, 0, 0.75097,  True, '2021-11-01 00:00:00'); /* Adept of Lightning */
/* @teleloc 0x007F0190 [255.807999 -279.971008 -29.995001] 0.660336 0.000000 0.000000 0.750970 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F028, 35130, 0x007F017C, 244.251, -260.018, -29.995, 0.668973, 0, 0, -0.743287,  True, '2021-11-01 00:00:00'); /* Adept of Lightning */
/* @teleloc 0x007F017C [244.251007 -260.018005 -29.995001] 0.668973 0.000000 0.000000 -0.743287 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F029, 30291, 0x007F0184, 247.736, -246.671, -29.9932, -0.022944, 0, 0, -0.999737,  True, '2021-11-01 00:00:00'); /* Viamontian Tribune */
/* @teleloc 0x007F0184 [247.735992 -246.671005 -29.993200] -0.022944 0.000000 0.000000 -0.999737 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F02A, 30291, 0x007F0184, 252.052, -246.869, -29.9932, -0.022944, 0, 0, -0.999737,  True, '2021-11-01 00:00:00'); /* Viamontian Tribune */
/* @teleloc 0x007F0184 [252.052002 -246.869003 -29.993200] -0.022944 0.000000 0.000000 -0.999737 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F02B, 30291, 0x007F016D, 256.767, -219.992, -35.9932, -0.720605, 0, 0, -0.693346,  True, '2021-11-01 00:00:00'); /* Viamontian Tribune */
/* @teleloc 0x007F016D [256.766998 -219.992004 -35.993198] -0.720605 0.000000 0.000000 -0.693346 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F02C, 30291, 0x007F015A, 242.674, -220.31, -35.9932, 0.696369, 0, 0, -0.717684,  True, '2021-11-01 00:00:00'); /* Viamontian Tribune */
/* @teleloc 0x007F015A [242.673996 -220.309998 -35.993198] 0.696369 0.000000 0.000000 -0.717684 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F02D, 30291, 0x007F0136, 249.89, -168.547, -41.9932, 0.015992, 0, 0, -0.999872,  True, '2021-11-01 00:00:00'); /* Viamontian Tribune */
/* @teleloc 0x007F0136 [249.889999 -168.546997 -41.993198] 0.015992 0.000000 0.000000 -0.999872 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F02E, 35128, 0x007F0136, 251.998, -165.673, -41.995, 0.015992, 0, 0, -0.999872,  True, '2021-11-01 00:00:00'); /* Adept of Fire */
/* @teleloc 0x007F0136 [251.998001 -165.673004 -41.994999] 0.015992 0.000000 0.000000 -0.999872 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F02F, 35130, 0x007F0136, 246.577, -165.846, -41.995, 0.015992, 0, 0, -0.999872,  True, '2021-11-01 00:00:00'); /* Adept of Lightning */
/* @teleloc 0x007F0136 [246.576996 -165.845993 -41.994999] 0.015992 0.000000 0.000000 -0.999872 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F030, 87532, 0x007F0146, 289.389, -159.951, -42.063, -0.710526, 0, 0, -0.703671, False, '2021-11-01 00:00:00'); /* Surface */
/* @teleloc 0x007F0146 [289.389008 -159.951004 -42.063000] -0.710526 0.000000 0.000000 -0.703671 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7007F031, 87531, 0x007F0123, 208.462, -159.588, -41.9864, 0.686291, 0, 0, -0.727327, False, '2023-03-10 19:46:54'); /* Bound Falatacot */
/* @teleloc 0x007F0123 [208.462006 -159.587997 -41.986401] 0.686291 0.000000 0.000000 -0.727327 */
