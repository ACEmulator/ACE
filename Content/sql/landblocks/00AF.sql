DELETE FROM `landblock_instance` WHERE `landblock` = 0x00AF;

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x700AF04F, 34510, 0x00AF011A, 42.5, -20, 0.014, -0.707107, 0, 0, -0.707107, False, '2021-11-01 00:00:00'); /* Stone Fists */
/* @teleloc 0x00AF011A [42.500000 -20.000000 0.014000] -0.707107 0.000000 0.000000 -0.707107 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x700AF050, 34511, 0x00AF011F, 42.5, -30, 0.058, -0.707107, 0, 0, -0.707107, False, '2021-11-01 00:00:00'); /* Azaxis */
/* @teleloc 0x00AF011F [42.500000 -30.000000 0.058000] -0.707107 0.000000 0.000000 -0.707107 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x700AF051, 34512, 0x00AF011F, 37.5, -30, 0.011, 0.707107, 0, 0, -0.707107, False, '2021-11-01 00:00:00'); /* Killagurg */
/* @teleloc 0x00AF011F [37.500000 -30.000000 0.011000] 0.707107 0.000000 0.000000 -0.707107 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x700AF052, 34513, 0x00AF0129, 37.5, -50, 0, 0.707107, 0, 0, -0.707107, False, '2021-11-01 00:00:00'); /* Demon Swarm Matron */
/* @teleloc 0x00AF0129 [37.500000 -50.000000 0.000000] 0.707107 0.000000 0.000000 -0.707107 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x700AF057, 34535, 0x00AF014A, 72.1801, -40, 0, 0.707107, 0, 0, -0.707107, False, '2021-11-01 00:00:00'); /* Locked Door */
/* @teleloc 0x00AF014A [72.180099 -40.000000 0.000000] 0.707107 0.000000 0.000000 -0.707107 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x700AF076, 34608, 0x00AF0110, 26.3536, -10, 0.017856, -0.707107, 0, 0, -0.707107, False, '2021-11-01 00:00:00'); /* Colosseum Vault */
/* @teleloc 0x00AF0110 [26.353600 -10.000000 0.017856] -0.707107 0.000000 0.000000 -0.707107 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x700AF088, 36519, 0x00AF0139, 53, -10, 0.0065, -0.707107, 0, 0, -0.707107, False, '2021-11-01 00:00:00'); /* Colosseum Coin Collector */
/* @teleloc 0x00AF0139 [53.000000 -10.000000 0.006500] -0.707107 0.000000 0.000000 -0.707107 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x700AF093,  1154, 0x00AF0118, 37, -13.0434, 0, 1, 0, 0, 0, False, '2021-11-01 00:00:00'); /* Linkable Monster Generator */
/* @teleloc 0x00AF0118 [37.000000 -13.043400 0.000000] 1.000000 0.000000 0.000000 0.000000 */

INSERT INTO `landblock_instance_link` (`parent_GUID`, `child_GUID`, `last_Modified`)
VALUES (0x700AF093, 0x700AF094, '2021-11-01 00:00:00') /* Garbage Barrel (34726) */
     , (0x700AF093, 0x700AF095, '2021-11-01 00:00:00') /* Garbage Barrel (34726) */
     , (0x700AF093, 0x700AF096, '2021-11-01 00:00:00') /* Colosseum Arena (34727) */
     , (0x700AF093, 0x700AF097, '2021-11-01 00:00:00') /* Advanced Colosseum Arena (34728) */;

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x700AF094, 34726, 0x00AF0118, 37, -13.0434, 0, 1, 0, 0, 0,  True, '2021-11-01 00:00:00'); /* Garbage Barrel */
/* @teleloc 0x00AF0118 [37.000000 -13.043400 0.000000] 1.000000 0.000000 0.000000 0.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x700AF095, 34726, 0x00AF0118, 43, -13.0567, 0, 1, 0, 0, 0,  True, '2021-11-01 00:00:00'); /* Garbage Barrel */
/* @teleloc 0x00AF0118 [43.000000 -13.056700 0.000000] 1.000000 0.000000 0.000000 0.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x700AF096, 34727, 0x00AF010F, 30.8082, 4.51301, 0, 1, 0, 0, 0,  True, '2021-11-01 00:00:00'); /* Colosseum Arena */
/* @teleloc 0x00AF010F [30.808201 4.513010 0.000000] 1.000000 0.000000 0.000000 0.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x700AF097, 34728, 0x00AF0138, 49.1829, 4.27901, 0, 1, 0, 0, 0,  True, '2021-11-01 00:00:00'); /* Advanced Colosseum Arena */
/* @teleloc 0x00AF0138 [49.182899 4.279010 0.000000] 1.000000 0.000000 0.000000 0.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x700AF112, 28282, 0x00AF0117, 40, 0, 0.0075, 0, 0, 0, -1, False, '2021-11-01 00:00:00'); /* Linkable Monster Gen - 10 sec. */
/* @teleloc 0x00AF0117 [40.000000 0.000000 0.007500] 0.000000 0.000000 0.000000 -1.000000 */

INSERT INTO `landblock_instance_link` (`parent_GUID`, `child_GUID`, `last_Modified`)
VALUES (0x700AF112, 0x700AF114, '2021-11-01 00:00:00') /* Master Arbitrator (34441) */
     , (0x700AF112, 0x700AF115, '2021-11-01 00:00:00') /* Colosseum Ticket Vendor (34442) */
     , (0x700AF112, 0x700AF116, '2021-11-01 00:00:00') /* Gladiator Diemos (35869) */
     , (0x700AF112, 0x700AF117, '2021-11-01 00:00:00') /* The Master (35870) */
     , (0x700AF112, 0x700AF11C, '2021-11-01 00:00:00');

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x700AF113, 71705, 0x00AF0117, 38, 4.4, 0, 0, 0, 0, -1, False, '2021-11-01 00:00:00'); /* Colo Arena One Stopgap Generator */
/* @teleloc 0x00AF0117 [38.000000 4.400000 0.000000] 0.000000 0.000000 0.000000 -1.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x700AF114, 34441, 0x00AF0117, 40, 0, 0.0075, 0, 0, 0, -1,  True, '2021-11-01 00:00:00'); /* Master Arbitrator */
/* @teleloc 0x00AF0117 [40.000000 0.000000 0.007500] 0.000000 0.000000 0.000000 -1.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x700AF115, 34442, 0x00AF0139, 50, -13, 0.0065, 1, 0, 0, 0,  True, '2021-11-01 00:00:00'); /* Colosseum Ticket Vendor */
/* @teleloc 0x00AF0139 [50.000000 -13.000000 0.006500] 1.000000 0.000000 0.000000 0.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x700AF116, 35869, 0x00AF0142, 60, -37.3, 0.008, 0, 0, 0, -1,  True, '2021-11-01 00:00:00'); /* Gladiator Diemos */
/* @teleloc 0x00AF0142 [60.000000 -37.299999 0.008000] 0.000000 0.000000 0.000000 -1.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x700AF117, 35870, 0x00AF0133, 42.5, -70, 0, -0.707107, 0, 0, -0.707107,  True, '2021-11-01 00:00:00'); /* The Master */
/* @teleloc 0x00AF0133 [42.500000 -70.000000 0.000000] -0.707107 0.000000 0.000000 -0.707107 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x700AF118, 71511, 0x00AF0117, 39, 4.4, 0, 0, 0, 0, -1, False, '2021-11-01 00:00:00'); /* Colo Arena Two Stopgap Generator */
/* @teleloc 0x00AF0117 [39.000000 4.400000 0.000000] 0.000000 0.000000 0.000000 -1.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x700AF119, 71512, 0x00AF0117, 40, 4.39999, 0, 0, 0, 0, -1, False, '2021-11-01 00:00:00'); /* Colo Arena Three Stopgap Generator */
/* @teleloc 0x00AF0117 [40.000000 4.399990 0.000000] 0.000000 0.000000 0.000000 -1.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x700AF11A, 71513, 0x00AF0117, 41, 4.4, 0, 0, 0, 0, -1, False, '2021-11-01 00:00:00'); /* Colo Arena Four Stopgap Generator */
/* @teleloc 0x00AF0117 [41.000000 4.400000 0.000000] 0.000000 0.000000 0.000000 -1.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x700AF11B, 71514, 0x00AF0117, 42, 4.4, 0, 0, 0, 0, -1, False, '2021-11-01 00:00:00'); /* Colo Arena Five Stopgap Generator */
/* @teleloc 0x00AF0117 [42.000000 4.400000 0.000000] 0.000000 0.000000 0.000000 -1.000000 */
