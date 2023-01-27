DELETE FROM `weenie` WHERE `class_Id` = 1910618;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1910618, 'ace1910618-prismaticamulicoat', 2, '2021-04-18 02:49:41') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1910618,   1,          2) /* ItemType - Armor */
     , (1910618,   3,         39) /* PaletteTemplate - Black */
     , (1910618,   4,      1024) /* ClothingPriority - OuterwearChest, OuterwearUpperArms, OuterwearLowerArms */
     , (1910618,   5,       1600) /* EncumbranceVal */
     , (1910618,   9,       512) /* ValidLocations - ChestArmor, UpperArmArmor, LowerArmArmor */
     , (1910618,  16,          1) /* ItemUseable - No */
     , (1910618,  18,          1) /* UiEffects - Magical */
     , (1910618,  19,         20) /* Value */
     , (1910618,  28,          1) /* ArmorLevel */
     , (1910618,  33,          1) /* Bonded - Bonded */
     , (1910618,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1910618, 105,          1) /* ItemWorkmanship */
     , (1910618, 106,        310) /* ItemSpellcraft */
     , (1910618, 107,       2400) /* ItemCurMana */
     , (1910618, 108,       2400) /* ItemMaxMana */
     , (1910618, 115,        380) /* ItemSkillLevelLimit */
     , (1910618, 158,          7) /* WieldRequirements - Level */
     , (1910618, 159,          1) /* WieldSkillType - Axe */
     , (1910618, 160,        100) /* WieldDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1910618,  11, True ) /* IgnoreCollisions */
     , (1910618,  13, True ) /* Ethereal */
     , (1910618,  14, True ) /* GravityStatus */
     , (1910618,  19, True ) /* Attackable */
     , (1910618,  22, True ) /* Inscribable */
     , (1910618,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1910618,   5, -0.08330000191926956) /* ManaRate */
     , (1910618,  13,     0.5) /* ArmorModVsSlash */
     , (1910618,  14,     0.5) /* ArmorModVsPierce */
     , (1910618,  15,     0.5) /* ArmorModVsBludgeon */
     , (1910618,  16,       2) /* ArmorModVsCold */
     , (1910618,  17,       2) /* ArmorModVsFire */
     , (1910618,  18,       2) /* ArmorModVsAcid */
     , (1910618,  19,       2) /* ArmorModVsElectric */
     , (1910618, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1910618,   1, 'Prismatic Amuli Coat') /* Name */
     , (1910618,  16, 'An Amuli Coat infused with the power of the Elements. A soft glow surrounds the coat and storms can be seen waxing and waning within the crystal surface.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1910618,   1,   33554854) /* Setup */
     , (1910618,   3,  536870932) /* SoundTable */
     , (1910618,   7,  268437101) /* ClothingBase */
     , (1910618,   8,  100688617) /* Icon */
     , (1910618,  22,  872415275) /* PhysicsEffectTable */
     , (1910618,  37,          6) /* ItemSkillLimit */;

/* Lifestoned Changelog:
{
  "LastModified": null,
  "ModifiedBy": null,
  "Changelog": [
    {
      "created": "2021-04-15T01:24:33.4839966Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    }
  ],
  "UserChangeSummary": null,
  "IsDone": false
}
*/
