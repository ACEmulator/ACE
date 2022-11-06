DELETE FROM `weenie` WHERE `class_Id` = 30058;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (30058, 'portalaugmentationrealmmain3', 7, '2021-11-04 00:07:18') /* Portal */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (30058,   1,      65536) /* ItemType - Portal */
     , (30058,  16,         32) /* ItemUseable - Remote */
     , (30058,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (30058,  86,        125) /* MinLevel */
     , (30058,  93,       3084) /* PhysicsState - Ethereal, ReportCollisions, Gravity, LightingOn */
     , (30058, 111,          1) /* PortalBitmask - Unrestricted */
     , (30058, 133,          4) /* ShowableOnRadar - ShowAlways */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (30058,   1, True ) /* Stuck */
     , (30058,  11, False) /* IgnoreCollisions */
     , (30058,  12, True ) /* ReportCollisions */
     , (30058,  13, True ) /* Ethereal */
     , (30058,  15, True ) /* LightsStatus */
     , (30058,  88, True ) /* PortalShowDestination */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (30058,  54, -0.10000000149011612) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (30058,   1, 'Augmentation Realm Main Level') /* Name */
     , (30058,  16, 'You must speak with Fiun Rehlyun before you can use this portal.') /* LongDesc */
     , (30058,  38, 'Augmentation Realm Main Level') /* AppraisalPortalDestination */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (30058,   1,   33555925) /* Setup */
     , (30058,   2,  150994947) /* MotionTable */
     , (30058,   8,  100667499) /* Icon */;

INSERT INTO `weenie_properties_position` (`object_Id`, `position_Type`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (30058, 2, 5636705, 129.225, -210.094, -5.995, 0.707107, 0, 0, -0.707107) /* Destination */
/* @teleloc 0x00560261 [129.225006 -210.093994 -5.995000] 0.707107 0.000000 0.000000 -0.707107 */;

/* Lifestoned Changelog:
{
  "LastModified": "2018-11-24T16:38:56.3868025-05:00",
  "ModifiedBy": "fdsfsd",
  "Changelog": [
    {
      "created": "2018-11-23T23:59:11.9930821-05:00",
      "author": "Zarto ",
      "comment": "Updated to include PORTAL_SHOW_DESTINATION_BOOL."
    },
    {
      "created": "2018-11-24T16:51:29.6179337-05:00",
      "author": "fdsfsd",
      "comment": "Updated to include PORTAL_SHOW_DESTINATION_BOOL."
    }
  ],
  "UserChangeSummary": "Updated to include PORTAL_SHOW_DESTINATION_BOOL.",
  "IsDone": true
}
*/
