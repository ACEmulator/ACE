DELETE FROM `landblock_instance` WHERE `landblock` = 0x0145;

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7014502D, 15759, 0x01450111, -3.78318, -66.2644, 0.005, 0.911915, 0, 0, 0.410379, False, '2005-02-09 10:00:00'); /* Linkable Item Generator */
/* @teleloc 0x01450111 [-3.783180 -66.264397 0.005000] 0.911915 0.000000 0.000000 0.410379 */

INSERT INTO `landblock_instance_link` (`parent_GUID`, `child_GUID`, `last_Modified`)
VALUES (0x7014502D, 0x7014502E, '2005-02-09 10:00:00') /* Alloy Instrument (25317) */;

INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)
VALUES (0x7014502E, 25317, 0x01450111, -2.93721, -67.4071, 0.005, 0.911915, 0, 0, 0.410379,  True, '2005-02-09 10:00:00'); /* Alloy Instrument */
/* @teleloc 0x01450111 [-2.937210 -67.407097 0.005000] 0.911915 0.000000 0.000000 0.410379 */
