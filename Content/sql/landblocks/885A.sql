DELETE FROM `landblock_instance` WHERE `landblock` = 0x885A;

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7885A001,  4266, 0x885A0000, 34.9851, 125.912, 10.005, -0.589037, 0, 0, 0.808106,  True, '2005-02-09 10:00:00'); /* Old Bones */
/* @teleloc 0x885A0000 [34.985100 125.912003 10.005000] -0.589037 0.000000 0.000000 0.808106 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7885A002,  1759, 0x885A0000, 34.6967, 138.447, 10.005, 0.575548, 0, 0, -0.817768,  True, '2005-02-09 10:00:00'); /* Skeleton */
/* @teleloc 0x885A0000 [34.696701 138.447006 10.005000] 0.575548 0.000000 0.000000 -0.817768 */

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7885A003,  3955, 0x885A0000, 36.6312, 138.166, 10.005, 0.654313, 0, 0, -0.756224, False, '2005-02-09 10:00:00'); /* Linkable Monster Gen (15 min.) */
/* @teleloc 0x885A0000 [36.631199 138.166000 10.005000] 0.654313 0.000000 0.000000 -0.756224 */

INSERT INTO `landblock_instance_link` (`parent_GUID`, `child_GUID`, `last_Modified`)
VALUES (0x7885A003, 0x7885A001, '2005-02-09 10:00:00') /* Old Bones (4266) */
     , (0x7885A003, 0x7885A002, '2005-02-09 10:00:00') /* Skeleton (1759) */;
