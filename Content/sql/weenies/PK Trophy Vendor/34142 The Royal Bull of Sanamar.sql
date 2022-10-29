DELETE FROM `weenie` WHERE `class_Id` = 34142;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (34142, 'ace34142-theroyalbullofsanamar', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (34142,   1,          2) /* ItemType - Armor */
     , (34142,   3,         14) /* PaletteTemplate - Red */
     , (34142,   4,      16384) /* ClothingPriority - Head */
     , (34142,   5,          1) /* EncumbranceVal */
     , (34142,   9,          1) /* ValidLocations - HeadWear */
     , (34142,  19,         20) /* Value */
     , (34142, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (34142,   1, 'The Royal Bull of Sanamar') /* Name */
     , (34142,  16, 'An ornate representation of the heraldic bull of King Varicci II of Sanamar, King of New Viamont and Protector of the Halatean Isles.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (34142,   1,   33560107) /* Setup */
     , (34142,   7,  268437155) /* ClothingBase */
     , (34142,   8,  100689155) /* Icon */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-05T21:47:09.3134251-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
