DELETE FROM `weenie` WHERE `class_Id` = 1910619;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1910619, 'ace1910619-prismaticamulileggings', 2, '2021-04-18 02:49:41') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1910619,   1,          2) /* ItemType - Armor */
     , (1910619,   3,         39) /* PaletteTemplate - Black */
     , (1910619,   4,       2816) /* ClothingPriority - OuterwearUpperLegs, OuterwearLowerLegs, OuterwearAbdomen */
     , (1910619,   5,       2288) /* EncumbranceVal */
     , (1910619,   9,      25600) /* ValidLocations - AbdomenArmor, UpperLegArmor, LowerLegArmor */
     , (1910619,  16,          1) /* ItemUseable - No */
     , (1910619,  18,          1) /* UiEffects - Magical */
     , (1910619,  19,         20) /* Value */
     , (1910619,  28,          1) /* ArmorLevel */
     , (1910619,  33,          1) /* Bonded - Bonded */
     , (1910619,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1910619, 105,          1) /* ItemWorkmanship */
     , (1910619, 106,        310) /* ItemSpellcraft */
     , (1910619, 107,       2400) /* ItemCurMana */
     , (1910619, 108,       2400) /* ItemMaxMana */
     , (1910619, 115,        380) /* ItemSkillLevelLimit */
     , (1910619, 158,          7) /* WieldRequirements - Level */
     , (1910619, 159,          1) /* WieldSkillType - Axe */
     , (1910619, 160,        100) /* WieldDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1910619,  11, True ) /* IgnoreCollisions */
     , (1910619,  13, True ) /* Ethereal */
     , (1910619,  14, True ) /* GravityStatus */
     , (1910619,  19, True ) /* Attackable */
     , (1910619,  22, True ) /* Inscribable */
     , (1910619,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1910619,   5, -0.08330000191926956) /* ManaRate */
     , (1910619,  13,     0.5) /* ArmorModVsSlash */
     , (1910619,  14,     0.5) /* ArmorModVsPierce */
     , (1910619,  15,     0.5) /* ArmorModVsBludgeon */
     , (1910619,  16,       2) /* ArmorModVsCold */
     , (1910619,  17,       2) /* ArmorModVsFire */
     , (1910619,  18,       2) /* ArmorModVsAcid */
     , (1910619,  19,       2) /* ArmorModVsElectric */
     , (1910619, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1910619,   1, 'Prismatic Amuli Leggings') /* Name */
     , (1910619,  16, 'A set of Amuli Leggings infused with the power of the Elements. A soft glow surrounds the leggings and storms can be seen waxing and waning within the crystal surface.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1910619,   1,   33554856) /* Setup */
     , (1910619,   3,  536870932) /* SoundTable */
     , (1910619,   7,  268437102) /* ClothingBase */
     , (1910619,   8,  100688616) /* Icon */
     , (1910619,  22,  872415275) /* PhysicsEffectTable */
     , (1910619,  37,          6) /* ItemSkillLimit */;

/* Lifestoned Changelog:
{
  "LastModified": null,
  "ModifiedBy": null,
  "Changelog": [
    {
      "created": "2021-04-15T01:24:37.1683194Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    }
  ],
  "UserChangeSummary": null,
  "IsDone": false
}
*/
