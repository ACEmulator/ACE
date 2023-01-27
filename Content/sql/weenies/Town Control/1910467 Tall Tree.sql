DELETE FROM `weenie` WHERE `class_Id` = 1910467;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1910467, 'ace1910467-talltree', 1, '2020-05-31 06:03:27') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1910467,   1,        128) /* ItemType - Misc */
     , (1910467,   5,      70000) /* EncumbranceVal */
     , (1910467,   9,          0) /* ValidLocations - None */
     , (1910467,   8,      14000) /* Mass */
     , (1910467,  16,          1) /* ItemUseable - No */
     , (1910467,  19,        900) /* Value */
     , (1910467,  93,      1032) /* PhysicsState - ReportCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1910467,   1, True ) /* Stuck */
     , (1910467,  13, False) /* Ethereal */
     , (1910467,  24, True ) /* UiHidden */
     , (1910467,  41, True ) /* Report Collisions As Environment */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1910467,  44,      -1) /* TimeToRot */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1910467,   1, 'Tall Tree') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1910467,   1,   33555361) /* Setup */
     , (1910467,   8,  100671332) /* Icon */;

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
