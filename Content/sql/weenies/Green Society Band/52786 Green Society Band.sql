DELETE FROM `weenie` WHERE `class_Id` = 52786;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (52786, 'ace52786-greensocietyband', 1, '2022-03-31 06:02:40') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (52786,   1,          8) /* ItemType - Jewelry */
     , (52786,   3,         39) /* PaletteTemplate - Black */
     , (52786,   5,         50) /* EncumbranceVal */
     , (52786,   9,     786432) /* ValidLocations - FingerWear */
     , (52786,  16,          1) /* ItemUseable - No */
     , (52786,  18,          1) /* UiEffects - Magical */
     , (52786,  19,       5000) /* Value */
     , (52786,  33,          1) /* Bonded - Bonded */
     , (52786,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (52786, 106,        325) /* ItemSpellcraft */
     , (52786, 107,       1000) /* ItemCurMana */
     , (52786, 108,       1000) /* ItemMaxMana */
     , (52786, 109,          0) /* ItemDifficulty */
     , (52786, 114,          1) /* Attuned - Attuned */
     , (52786, 158,          7) /* WieldRequirements - Level */
     , (52786, 159,          1) /* WieldSkillType - Axe */
     , (52786, 160,        180) /* WieldDifficulty */
     , (52786, 265,        134) /* EquipmentSetId - GreenSocietyBand */
     , (52786, 319,         10) /* ItemMaxLevel */
     , (52786, 320,          1) /* ItemXpStyle - Fixed */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (52786,   4,          0) /* ItemTotalXp */
     , (52786,   5, 4000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (52786,  22, True ) /* Inscribable */
     , (52786,  23, True ) /* DestroyOnSell */
     , (52786,  69, False) /* IsSellable */
     , (52786,  84, True ) /* IgnoreCloIcons */
     , (52786,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (52786,   5,  -0.033) /* ManaRate */
     , (52786,  12,     0.5) /* Shade */
     , (52786,  39,     0.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (52786,   1, 'Green Society Band') /* Name */
     , (52786,  16, 'A dark black ring set with five emeralds.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (52786,   1, 0x02000103) /* Setup */
     , (52786,   3, 0x20000014) /* SoundTable */
     , (52786,   6, 0x04000BEF) /* PaletteBase */
     , (52786,   7, 0x10000129) /* ClothingBase */
     , (52786,   8, 0x06007541) /* Icon */
     , (52786,  22, 0x3400002B) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (52786,  4025,      2)  /* Cast Iron Stomach */
	 , (52786,  6073,      2)  /* Legendary Two Handed Combat Aptitude */;
