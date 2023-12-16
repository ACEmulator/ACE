DELETE FROM `weenie` WHERE `class_Id` = 480618;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480618, 'ace480618-cloakofdestructionwar', 2, '2022-04-19 17:09:43') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480618,   1,          4) /* ItemType - Clothing */
     , (480618,   3,          2) /* PaletteTemplate - Blue */
     , (480618,   4,     131072) /* ClothingPriority - 131072 */
     , (480618,   5,         75) /* EncumbranceVal */
     , (480618,   9,  134217728) /* ValidLocations - Cloak */
     , (480618,  16,          1) /* ItemUseable - No */
     , (480618,  18,          1) /* UiEffects - Magical */
     , (480618,  19,     5) /* Value */
     , (480618,  28,          0) /* ArmorLevel */
     , (480618,  36,       9999) /* ResistMagic */
     , (480618,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480618, 158,          7) /* WieldRequirements - Level */
     , (480618, 159,          1) /* WieldSkillType - Axe */
     , (480618, 160,        180) /* WieldDifficulty */
     , (480618, 169,         16) /* TsysMutationData */
     , (480618, 265,         81) /* EquipmentSetId - CloakMagicDefense */
     , (480618, 267,     604800) /* Lifespan */
     , (480618, 319,          5) /* ItemMaxLevel */
     , (480618, 320,          2) /* ItemXpStyle - ScalesWithLevel */
     , (480618, 370,          3) /* GearDamage */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (480618,   4, 31000000000) /* ItemTotalXp */
     , (480618,   5, 1000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480618,  22, True ) /* Inscribable */
     , (480618,  84, True ) /* IgnoreCloIcons */
     , (480618, 100, False) /* Dyable */
     , (480618, 112, True ) /* ProcSpellSelfTargeted */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480618,  13,     0.8) /* ArmorModVsSlash */
     , (480618,  14,     0.8) /* ArmorModVsPierce */
     , (480618,  15,       1) /* ArmorModVsBludgeon */
     , (480618,  16,     0.2) /* ArmorModVsCold */
     , (480618,  17,     0.2) /* ArmorModVsFire */
     , (480618,  18,     0.1) /* ArmorModVsAcid */
     , (480618,  19,     0.2) /* ArmorModVsElectric */
     , (480618, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480618,   1, 'War Magic Cloak of Destruction') /* Name */
     , (480618,  16, 'War Magic Cloak of Destruction') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480618,   1, 0x02001B2A) /* Setup */
     , (480618,   3, 0x20000014) /* SoundTable */
     , (480618,   6, 0x100007F2) /* PaletteBase */
     /*, (480618,   7, 0x100007EB) /* ClothingBase */
	 , (480618,   7, 0x1000085E) /* ClothingBase */
     , (480618,   8, 0x0600709C) /* Icon */
     , (480618,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480618,  36, 0x0E00001E) /* MutateFilter */
     , (480618,  50, 100691000) /* IconOverlay */
     , (480618,  55,       5753) /* ProcSpell */;
