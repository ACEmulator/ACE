DELETE FROM `weenie` WHERE `class_Id` = 34143;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (34143, 'ace34143-theboarofcinghalle', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (34143,   1,          2) /* ItemType - Armor */
     , (34143,   3,          8) /* PaletteTemplate - Green */
     , (34143,   4,      16384) /* ClothingPriority - Head */
     , (34143,   5,          1) /* EncumbranceVal */
     , (34143,   9,          1) /* ValidLocations - HeadWear */
     , (34143,  19,         20) /* Value */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (34143,  11, True ) /* IgnoreCollisions */
     , (34143,  13, False) /* Ethereal */
     , (34143,  14, True ) /* GravityStatus */
     , (34143,  19, True ) /* Attackable */
     , (34143,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (34143,   1, 'The Boar of Cinghalle') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (34143,   1,   33560108) /* Setup */
     , (34143,   7,  268437156) /* ClothingBase */
     , (34143,   8,  100689160) /* Icon */
     , (34143,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-05T21:39:17.7138223-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
