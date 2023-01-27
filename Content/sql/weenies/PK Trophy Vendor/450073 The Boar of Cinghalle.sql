DELETE FROM `weenie` WHERE `class_Id` = 450073;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450073, 'ace450073-theboarofcinghalletailor', 2, '2021-11-20 00:19:18') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450073,   1,          2) /* ItemType - Armor */
     , (450073,   3,          8) /* PaletteTemplate - Green */
     , (450073,   4,      16384) /* ClothingPriority - Head */
     , (450073,   5,          1) /* EncumbranceVal */
     , (450073,   9,          1) /* ValidLocations - HeadWear */
     , (450073,  19,         20) /* Value */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450073,  11, True ) /* IgnoreCollisions */
     , (450073,  13, False) /* Ethereal */
     , (450073,  14, True ) /* GravityStatus */
     , (450073,  19, True ) /* Attackable */
     , (450073,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450073,   1, 'The Boar of Cinghalle') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450073,   1,   33560108) /* Setup */
     , (450073,   7,  268437156) /* ClothingBase */
     , (450073,   8,  100689160) /* Icon */
     , (450073,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-06-05T21:39:17.7138223-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
