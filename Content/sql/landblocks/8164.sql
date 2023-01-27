DELETE FROM `landblock_instance` WHERE `landblock` = 0x8164;

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x78164000,   412, 0x81640105, 13.76, 107.05, 30.01, -0.707107, 0, 0, -0.707107, False, '2021-11-01 00:00:00'); /* Door */
/* @teleloc 0x81640105 [13.760000 107.050003 30.010000] -0.707107 0.000000 0.000000 -0.707107 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x78164002,   509, 0x81640000, 19.2176, 148.024, 24, 0.614668, 0, 0, 0.788786, False, '2021-11-01 00:00:00'); /* Life Stone */
/* @teleloc 0x81640000 [19.217600 148.024002 24.000000] 0.614668 0.000000 0.000000 0.788786 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x78164003,  4676, 0x81640000, 1.40252, 152.563, 24.005, 0.994956, 0, 0, -0.100316, False, '2021-11-01 00:00:00'); /* Yaraq Outpost */
/* @teleloc 0x81640000 [1.402520 152.563004 24.004999] 0.994956 0.000000 0.000000 -0.100316 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x78164004,  4707, 0x81640108, 9.55362, 153.031, 23.705, -0.066113, 0, 0, 0.997812, False, '2021-11-01 00:00:00'); /* Lu'zura the Shopkeeper */
/* @teleloc 0x81640108 [9.553620 153.031006 23.705000] -0.066113 0.000000 0.000000 0.997812 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x78164006,  5108, 0x81640000, 19.6281, 144.634, 24.005, 0.99998, 0, 0, -0.006378, False, '2021-11-01 00:00:00'); /* LIFESTONES SIGN */
/* @teleloc 0x81640000 [19.628099 144.634003 24.004999] 0.999980 0.000000 0.000000 -0.006378 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x78164007,  5082, 0x81640000, 12.6469, 150.518, 24.005, 0.999633, 0, 0, -0.027077, False, '2021-11-01 00:00:00'); /* East Yaraq Outpost */
/* @teleloc 0x81640000 [12.646900 150.518005 24.004999] 0.999633 0.000000 0.000000 -0.027077 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
<<<<<<< Updated upstream
VALUES (0x78164008, 4200004, 0x8164000D, 41.3957, 116.436, 31.5046, 0.945676, 0, 0, 0.325112, False, '2022-01-11 18:42:42'); /* Yaraq Generator Switch */
/* @teleloc 0x8164000D [41.395699 116.435997 31.504601] 0.945676 0.000000 0.000000 0.325112 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x78164009,  4200047, 0x81640017, 59.3132, 159.402, 22.055, 0.597293, 0, 0, 0.802023, False, '2022-01-11 18:54:05'); /* Linkable Monster Gen (2 hours) */
/* @teleloc 0x81640017 [59.313202 159.401993 22.055000] 0.597293 0.000000 0.000000 0.802023 */

INSERT INTO `landblock_instance_link` (`parent_GUID`, `child_GUID`, `last_Modified`)
VALUES (0x78164009, 0x7816400A, '2022-01-11 18:54:47') /* Mosswart Governor (4200003) */;

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7816400A, 4200003, 0x81640017, 60.539, 158.706, 22.0105, 0.597293, 0, 0, 0.802023,  True, '2022-01-11 18:54:47'); /* Mosswart Governor */
/* @teleloc 0x81640017 [60.539001 158.705994 22.010500] 0.597293 0.000000 0.000000 0.802023 */
=======
VALUES (0x78164009, 4200047, 0x81640017, 59.3132, 159.402, 22.055, 0.597293, 0, 0, 0.802023, False, '2022-01-11 18:54:05'); /* Linkable Monster Gen (2 hours) */
/* @teleloc 0x81640017 [59.313202 159.401993 22.055000] 0.597293 0.000000 0.000000 0.802023 */

INSERT INTO `landblock_instance_link` (`parent_GUID`, `child_GUID`, `last_Modified`)
VALUES (0x78164009, 0x7816400A, '2022-01-11 18:54:47') /* Yaraq Governor (4200003) */;

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7816400A, 4200003, 0x81640017, 60.539, 158.706, 22.0105, 0.597293, 0, 0, 0.802023,  True, '2022-01-11 18:54:47'); /* Yaraq Governor */
/* @teleloc 0x81640017 [60.539001 158.705994 22.010500] 0.597293 0.000000 0.000000 0.802023 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7816400C, 4200006, 0x8164000D, 45.3878, 117.284, 31.8373, -0.985984, 0, 0, 0.166839, False, '2023-01-24 22:22:14'); /* Yaraq Generator Switch */
/* @teleloc 0x8164000D [45.387798 117.283997 31.837299] -0.985984 0.000000 0.000000 0.166839 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7816400D,   252, 0x81640010, 35.6308, 168.192, 23.0858, 0.01869, 0, 0, -0.999825, False, '2023-01-24 22:57:25'); /* Stone Portal Frame */
/* @teleloc 0x81640010 [35.630798 168.192001 23.085800] 0.018690 0.000000 0.000000 -0.999825 */
>>>>>>> Stashed changes
