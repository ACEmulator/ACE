DELETE FROM `weenie` WHERE `class_Id` = 480621;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480621, 'ace480621-cloakofdestructionvoid', 2, '2022-04-19 17:09:43') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480621,   1,          4) /* ItemType - Clothing */
     , (480621,   3,          2) /* PaletteTemplate - Blue */
     , (480621,   4,     131072) /* ClothingPriority - 131072 */
     , (480621,   5,         75) /* EncumbranceVal */
     , (480621,   9,  134217728) /* ValidLocations - Cloak */
     , (480621,  16,          1) /* ItemUseable - No */
     , (480621,  18,          1) /* UiEffects - Magical */
     , (480621,  19,     5) /* Value */
     , (480621,  28,          0) /* ArmorLevel */
     , (480621,  36,       9999) /* ResistMagic */
     , (480621,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480621, 158,          7) /* WieldRequirements - Level */
     , (480621, 159,          1) /* WieldSkillType - Axe */
     , (480621, 160,        180) /* WieldDifficulty */
     , (480621, 169,         16) /* TsysMutationData */
     , (480621, 265,         80) /* EquipmentSetId - CloakMagicDefense */
     , (480621, 267,     604800) /* Lifespan */
     , (480621, 319,          5) /* ItemMaxLevel */
     , (480621, 320,          2) /* ItemXpStyle - ScalesWithLevel */
     , (480621, 370,          3) /* GearDamage */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (480621,   4, 31000000000) /* ItemTotalXp */
     , (480621,   5, 1000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480621,  22, True ) /* Inscribable */
     , (480621,  84, True ) /* IgnoreCloIcons */
     , (480621, 100, False) /* Dyable */
     , (480621, 112, True ) /* ProcSpellSelfTargeted */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480621,  13,     0.8) /* ArmorModVsSlash */
     , (480621,  14,     0.8) /* ArmorModVsPierce */
     , (480621,  15,       1) /* ArmorModVsBludgeon */
     , (480621,  16,     0.2) /* ArmorModVsCold */
     , (480621,  17,     0.2) /* ArmorModVsFire */
     , (480621,  18,     0.1) /* ArmorModVsAcid */
     , (480621,  19,     0.2) /* ArmorModVsElectric */
     , (480621, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480621,   1, 'Void Magic Cloak of Destruction') /* Name */
     , (480621,  16, 'Void Magic Cloak of Destruction') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480621,   1, 0x02001B2A) /* Setup */
     , (480621,   3, 0x20000014) /* SoundTable */
     , (480621,   6, 0x100007F2) /* PaletteBase */
     /*, (480621,   7, 0x100007EB) /* ClothingBase */
	 , (480621,   7, 0x1000085E) /* ClothingBase */
     , (480621,   8, 0x0600709C) /* Icon */
     , (480621,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480621,  36, 0x0E00001E) /* MutateFilter */
     , (480621,  50, 100691000) /* IconOverlay */
     , (480621,  55,       5753) /* ProcSpell */;
