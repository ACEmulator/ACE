DELETE FROM `weenie` WHERE `class_Id` = 480620;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480620, 'ace480620-cloakofdestructionlife', 2, '2022-04-19 17:09:43') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480620,   1,          4) /* ItemType - Clothing */
     , (480620,   3,          2) /* PaletteTemplate - Blue */
     , (480620,   4,     131072) /* ClothingPriority - 131072 */
     , (480620,   5,         75) /* EncumbranceVal */
     , (480620,   9,  134217728) /* ValidLocations - Cloak */
     , (480620,  16,          1) /* ItemUseable - No */
     , (480620,  18,          1) /* UiEffects - Magical */
     , (480620,  19,     5) /* Value */
     , (480620,  28,          0) /* ArmorLevel */
     , (480620,  36,       9999) /* ResistMagic */
     , (480620,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480620, 158,          7) /* WieldRequirements - Level */
     , (480620, 159,          1) /* WieldSkillType - Axe */
     , (480620, 160,        180) /* WieldDifficulty */
     , (480620, 169,         16) /* TsysMutationData */
     , (480620, 265,         65) /* EquipmentSetId - CloakMagicDefense */
     , (480620, 267,     604800) /* Lifespan */
     , (480620, 319,          5) /* ItemMaxLevel */
     , (480620, 320,          2) /* ItemXpStyle - ScalesWithLevel */
     , (480620, 370,          3) /* GearDamage */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (480620,   4, 31000000000) /* ItemTotalXp */
     , (480620,   5, 1000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480620,  22, True ) /* Inscribable */
     , (480620,  84, True ) /* IgnoreCloIcons */
     , (480620, 100, False) /* Dyable */
     , (480620, 112, True ) /* ProcSpellSelfTargeted */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480620,  13,     0.8) /* ArmorModVsSlash */
     , (480620,  14,     0.8) /* ArmorModVsPierce */
     , (480620,  15,       1) /* ArmorModVsBludgeon */
     , (480620,  16,     0.2) /* ArmorModVsCold */
     , (480620,  17,     0.2) /* ArmorModVsFire */
     , (480620,  18,     0.1) /* ArmorModVsAcid */
     , (480620,  19,     0.2) /* ArmorModVsElectric */
     , (480620, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480620,   1, 'Life Magic Cloak of Destruction') /* Name */
     , (480620,  16, 'Life Magic Cloak of Destruction') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480620,   1, 0x02001B2A) /* Setup */
     , (480620,   3, 0x20000014) /* SoundTable */
     , (480620,   6, 0x100007F2) /* PaletteBase */
     /*, (480620,   7, 0x100007EB) /* ClothingBase */
	 , (480620,   7, 0x1000085E) /* ClothingBase */
     , (480620,   8, 0x0600709C) /* Icon */
     , (480620,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480620,  36, 0x0E00001E) /* MutateFilter */
     , (480620,  50, 100691000) /* IconOverlay */
     , (480620,  55,       5753) /* ProcSpell */;
