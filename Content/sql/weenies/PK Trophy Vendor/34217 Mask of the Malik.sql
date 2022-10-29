DELETE FROM `weenie` WHERE `class_Id` = 34217;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (34217, 'ace34217-maskofthemalik', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (34217,   1,          2) /* ItemType - Armor */
     , (34217,   3,         13) /* PaletteTemplate - Purple */
     , (34217,   4,      16384) /* ClothingPriority - Head */
     , (34217,   5,          1) /* EncumbranceVal */
     , (34217,   9,          1) /* ValidLocations - HeadWear */
     , (34217,  19,         20) /* Value */
     , (34217, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (34217,   1, 'Mask of the Malik') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (34217,   1,   33560126) /* Setup */
     , (34217,   7,  268437173) /* ClothingBase */
     , (34217,   8,  100689233) /* Icon */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-05T21:53:18.6138763-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
