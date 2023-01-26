DELETE FROM `weenie` WHERE `class_Id` = 4200090;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200090, 'ace4200090-noticeofremoval', 1, '2022-01-28 03:20:10') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200090,   1,        128) /* ItemType - Misc */
     , (4200090,   5,       9000) /* EncumbranceVal */
     , (4200090,   8,       1800) /* Mass */
     , (4200090,  16,          1) /* ItemUseable - No */
     , (4200090,  19,        125) /* Value */
     , (4200090,  93,       1048) /* PhysicsState - ReportCollisions, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200090,   1, True ) /* Stuck */
     , (4200090,  12, True ) /* ReportCollisions */
     , (4200090,  13, False) /* Ethereal */
     , (4200090,  22, False) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200090,   1, 'Notice of Removal') /* Name */
     , (4200090,  16, 'This Portal has been confiscated by the Town Treasurers of Holtburg, Shoushi, and Yaraq. If you would like to visit this portal at it''s new locations you will need to be in control of one of those towns!') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200090,   1, 0x02000290) /* Setup */
     , (4200090,   8, 0x060012D3) /* Icon */;

/* Lifestoned Changelog:
{
  "Changelog": [
    {
      "created": "2022-01-28T03:09:12.0984002Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    }
  ],
  "IsDone": false
}
*/
