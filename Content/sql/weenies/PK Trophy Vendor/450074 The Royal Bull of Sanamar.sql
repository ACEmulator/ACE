DELETE FROM `weenie` WHERE `class_Id` = 450074;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450074, 'ace450074-theroyalbullofsanamartailor', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450074,   1,          2) /* ItemType - Armor */
     , (450074,   3,         14) /* PaletteTemplate - Red */
     , (450074,   4,      16384) /* ClothingPriority - Head */
     , (450074,   5,          1) /* EncumbranceVal */
     , (450074,   9,          1) /* ValidLocations - HeadWear */
     , (450074,  19,         20) /* Value */
     , (450074, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450074,   1, 'The Royal Bull of Sanamar') /* Name */
     , (450074,  16, 'An ornate representation of the heraldic bull of King Varicci II of Sanamar, King of New Viamont and Protector of the Halatean Isles.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450074,   1,   33560107) /* Setup */
     , (450074,   7,  268437155) /* ClothingBase */
     , (450074,   8,  100689155) /* Icon */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-05T21:47:09.3134251-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
