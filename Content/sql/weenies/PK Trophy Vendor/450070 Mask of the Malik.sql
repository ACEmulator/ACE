DELETE FROM `weenie` WHERE `class_Id` = 450070;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450070, 'ace450070-maskofthemaliktailor', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450070,   1,          2) /* ItemType - Armor */
     , (450070,   3,         13) /* PaletteTemplate - Purple */
     , (450070,   4,      16384) /* ClothingPriority - Head */
     , (450070,   5,          1) /* EncumbranceVal */
     , (450070,   9,          1) /* ValidLocations - HeadWear */
     , (450070,  19,         20) /* Value */
     , (450070, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450070,   1, 'Mask of the Malik') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450070,   1,   33560126) /* Setup */
     , (450070,   7,  268437173) /* ClothingBase */
     , (450070,   8,  100689233) /* Icon */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-05T21:53:18.6138763-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
