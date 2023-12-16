DELETE FROM `weenie` WHERE `class_Id` = 480619;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480619, 'ace480619-cloakofdestructionmelee', 2, '2022-04-19 17:09:43') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480619,   1,          4) /* ItemType - Clothing */
     , (480619,   3,          2) /* PaletteTemplate - Blue */
     , (480619,   4,     131072) /* ClothingPriority - 131072 */
     , (480619,   5,         75) /* EncumbranceVal */
     , (480619,   9,  134217728) /* ValidLocations - Cloak */
     , (480619,  16,          1) /* ItemUseable - No */
     , (480619,  18,          1) /* UiEffects - Magical */
     , (480619,  19,     5) /* Value */
     , (480619,  28,          0) /* ArmorLevel */
     , (480619,  36,       9999) /* ResistMagic */
     , (480619,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480619, 158,          7) /* WieldRequirements - Level */
     , (480619, 159,          1) /* WieldSkillType - Axe */
     , (480619, 160,        180) /* WieldDifficulty */
     , (480619, 169,         16) /* TsysMutationData */
     , (480619, 265,         71) /* EquipmentSetId - CloakMagicDefense */
     , (480619, 267,     604800) /* Lifespan */
     , (480619, 319,          5) /* ItemMaxLevel */
     , (480619, 320,          2) /* ItemXpStyle - ScalesWithLevel */
     , (480619, 370,          3) /* GearDamage */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (480619,   4, 31000000000) /* ItemTotalXp */
     , (480619,   5, 1000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480619,  22, True ) /* Inscribable */
     , (480619,  84, True ) /* IgnoreCloIcons */
     , (480619, 100, False) /* Dyable */
     , (480619, 112, True ) /* ProcSpellSelfTargeted */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480619,  13,     0.8) /* ArmorModVsSlash */
     , (480619,  14,     0.8) /* ArmorModVsPierce */
     , (480619,  15,       1) /* ArmorModVsBludgeon */
     , (480619,  16,     0.2) /* ArmorModVsCold */
     , (480619,  17,     0.2) /* ArmorModVsFire */
     , (480619,  18,     0.1) /* ArmorModVsAcid */
     , (480619,  19,     0.2) /* ArmorModVsElectric */
     , (480619, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480619,   1, 'Melee Defense Cloak of Destruction') /* Name */
     , (480619,  16, 'Melee Defense Cloak of Destruction') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480619,   1, 0x02001B2A) /* Setup */
     , (480619,   3, 0x20000014) /* SoundTable */
     , (480619,   6, 0x100007F2) /* PaletteBase */
     /*, (480619,   7, 0x100007EB) /* ClothingBase */
	 , (480619,   7, 0x1000085E) /* ClothingBase */
     , (480619,   8, 0x0600709C) /* Icon */
     , (480619,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480619,  36, 0x0E00001E) /* MutateFilter */
     , (480619,  50, 100691000) /* IconOverlay */
     , (480619,  55,       5753) /* ProcSpell */;
