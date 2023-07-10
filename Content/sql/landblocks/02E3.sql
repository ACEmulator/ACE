DELETE FROM `landblock_instance` WHERE `landblock` = 0x02E3;

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x702E3000,  2179, 0x02E30116, 74.682, -40, 6.005, 0.710744, 0, 0, -0.70345, False, '2005-02-09 10:00:00'); /* Door */
/* @teleloc 0x02E30116 [74.681999 -40.000000 6.005000] 0.710744 0.000000 0.000000 -0.703450 */

INSERT INTO `landblock_instance_link` (`parent_GUID`, `child_GUID`, `last_Modified`)
VALUES (0x702E3000, 0x702E3005, '2005-02-09 10:00:00') /* Lever (2609) */;

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x702E3001,  2179, 0x02E30117, 70, -44.721, 6.005, 1, 0, 0, 0, False, '2005-02-09 10:00:00'); /* Door */
/* @teleloc 0x02E30117 [70.000000 -44.721001 6.005000] 1.000000 0.000000 0.000000 0.000000 */

INSERT INTO `landblock_instance_link` (`parent_GUID`, `child_GUID`, `last_Modified`)
VALUES (0x702E3001, 0x702E3004, '2005-02-09 10:00:00') /* Lever (2609) */;

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x702E3002,  2179, 0x02E3011C, 70, -65.164, 6.005, 0.016266, 0, 0, -0.999868, False, '2005-02-09 10:00:00'); /* Door */
/* @teleloc 0x02E3011C [70.000000 -65.164001 6.005000] 0.016266 0.000000 0.000000 -0.999868 */

INSERT INTO `landblock_instance_link` (`parent_GUID`, `child_GUID`, `last_Modified`)
VALUES (0x702E3002, 0x702E3007, '2005-02-09 10:00:00') /* Lever (2609) */;

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x702E3003,  2179, 0x02E3011D, 74.838, -70, 6.005, 0.725591, 0, 0, -0.688127, False, '2005-02-09 10:00:00'); /* Door */
/* @teleloc 0x02E3011D [74.837997 -70.000000 6.005000] 0.725591 0.000000 0.000000 -0.688127 */

INSERT INTO `landblock_instance_link` (`parent_GUID`, `child_GUID`, `last_Modified`)
VALUES (0x702E3003, 0x702E3006, '2005-02-09 10:00:00') /* Lever (2609) */;

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x702E3004,  2609, 0x02E30159, 60.1701, -51.478, 12.055, -0.707107, 0, 0, -0.707107,  True, '2005-02-09 10:00:00'); /* Lever */
/* @teleloc 0x02E30159 [60.170101 -51.478001 12.055000] -0.707107 0.000000 0.000000 -0.707107 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x702E3005,  2609, 0x02E30159, 58.3996, -51.5946, 12.055, -0.707107, 0, 0, -0.707107,  True, '2005-02-09 10:00:00'); /* Lever */
/* @teleloc 0x02E30159 [58.399601 -51.594601 12.055000] -0.707107 0.000000 0.000000 -0.707107 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x702E3006,  2609, 0x02E3015A, 58.6144, -59.7702, 12.055, -0.707107, 0, 0, -0.707107,  True, '2005-02-09 10:00:00'); /* Lever */
/* @teleloc 0x02E3015A [58.614399 -59.770199 12.055000] -0.707107 0.000000 0.000000 -0.707107 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x702E3007,  2609, 0x02E3015A, 60.7142, -59.8905, 12.055, -0.707107, 0, 0, -0.707107,  True, '2005-02-09 10:00:00'); /* Lever */
/* @teleloc 0x02E3015A [60.714199 -59.890499 12.055000] -0.707107 0.000000 0.000000 -0.707107 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x702E300B,  7938, 0x02E301A6, 131.511, -62.3886, 12.005, -0.042755, 0, 0, 0.999086, False, '2005-02-09 10:00:00'); /* Warning for PK Arena! */
/* @teleloc 0x02E301A6 [131.511002 -62.388599 12.005000] -0.042755 0.000000 0.000000 0.999086 */
