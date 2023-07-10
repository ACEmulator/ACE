DELETE FROM `landblock_instance` WHERE `landblock` = 0xC68C;

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C68C000,   412, 0xC68C0000, 156, 81.48, 22, 1, 0, 0, 0, False, '2005-02-09 10:00:00'); /* Door */
/* @teleloc 0xC68C0000 [156.000000 81.480003 22.000000] 1.000000 0.000000 0.000000 0.000000 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C68C002,  4179, 0xC68C0103, 158.53, 82.1065, 21.205, -0.385317, 0, 0, -0.922784, False, '2005-02-09 10:00:00'); /* Bonfire */
/* @teleloc 0xC68C0103 [158.529999 82.106499 21.205000] -0.385317 0.000000 0.000000 -0.922784 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C68C004,  4213, 0xC68C0102, 154.362, 84.9348, 21.205, -0.844192, 0, 0, -0.536041,  True, '2005-02-09 10:00:00'); /* Leather Crafter */
/* @teleloc 0xC68C0102 [154.362000 84.934799 21.205000] -0.844192 0.000000 0.000000 -0.536041 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7C68C005,  1154, 0xC68C0000, 156.064, 78.1461, 22.005, 0.008132, 0, 0, -0.999967, False, '2005-02-09 10:00:00'); /* Linkable Monster Generator */
/* @teleloc 0xC68C0000 [156.063995 78.146103 22.004999] 0.008132 0.000000 0.000000 -0.999967 */

INSERT INTO `landblock_instance_link` (`parent_GUID`, `child_GUID`, `last_Modified`)
VALUES (0x7C68C005, 0x7C68C004, '2005-02-09 10:00:00') /* Leather Crafter (4213) */;
