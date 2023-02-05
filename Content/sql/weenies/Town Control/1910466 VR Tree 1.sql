DELETE FROM `weenie` WHERE `class_Id` = 1910466;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1910466, 'ace1910466-VRtree', 1, '2020-05-31 06:03:27') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1910466,   1,        128) /* ItemType - Misc */
     , (1910466,   5,      70000) /* EncumbranceVal */
     , (1910466,   9,          0) /* ValidLocations - None */
     , (1910466,   8,      14000) /* Mass */
     , (1910466,  16,          1) /* ItemUseable - No */
     , (1910466,  19,        900) /* Value */
     , (1910466,  93,      1032) /* PhysicsState - ReportCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1910466,   1, True ) /* Stuck */
     , (1910466,  13, False) /* Ethereal */
     , (1910466,  24, True ) /* UiHidden */
     , (1910466,  41, True ) /* Report Collisions As Environment */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1910466,  44,      -1) /* TimeToRot */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1910466,   1, 'VRtree1') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1910466,   1,   33561657) /* Setup */
     , (1910466,   8,  100667505) /* Icon */;

/* Lifestoned Changelog:
{
  "Changelog": [
    {
      "created": "2020-02-21T06:30:33.8170601Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    }
  ],
  "IsDone": false
}
*/
