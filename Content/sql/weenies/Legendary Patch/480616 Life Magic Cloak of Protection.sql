DELETE FROM `weenie` WHERE `class_Id` = 480616;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480616, 'ace480616-cloakofprotectionlife', 2, '2022-04-19 17:09:43') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480616,   1,          4) /* ItemType - Clothing */
     , (480616,   3,          2) /* PaletteTemplate - Blue */
     , (480616,   4,     131072) /* ClothingPriority - 131072 */
     , (480616,   5,         75) /* EncumbranceVal */
     , (480616,   9,  134217728) /* ValidLocations - Cloak */
     , (480616,  16,          1) /* ItemUseable - No */
     , (480616,  18,          1) /* UiEffects - Magical */
     , (480616,  19,     5) /* Value */
     , (480616,  28,          0) /* ArmorLevel */
     , (480616,  36,       9999) /* ResistMagic */
     , (480616,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480616, 158,          7) /* WieldRequirements - Level */
     , (480616, 159,          1) /* WieldSkillType - Axe */
     , (480616, 160,        180) /* WieldDifficulty */
     , (480616, 169,         16) /* TsysMutationData */
     , (480616, 265,         65) /* EquipmentSetId - CloakMagicDefense */
     , (480616, 267,     604800) /* Lifespan */
     , (480616, 319,          5) /* ItemMaxLevel */
     , (480616, 320,          2) /* ItemXpStyle - ScalesWithLevel */
	 , (480616, 352,          2) /* CloakWeaveProc -200 dmg */
     , (480616, 371,          3) /* GearDamageResist */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (480616,   4, 31000000000) /* ItemTotalXp */
     , (480616,   5, 1000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480616,  22, True ) /* Inscribable */
     , (480616,  84, True ) /* IgnoreCloIcons */
     , (480616, 100, False) /* Dyable */
     , (480616, 112, True ) /* ProcSpellSelfTargeted */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480616,  13,     0.8) /* ArmorModVsSlash */
     , (480616,  14,     0.8) /* ArmorModVsPierce */
     , (480616,  15,       1) /* ArmorModVsBludgeon */
     , (480616,  16,     0.2) /* ArmorModVsCold */
     , (480616,  17,     0.2) /* ArmorModVsFire */
     , (480616,  18,     0.1) /* ArmorModVsAcid */
     , (480616,  19,     0.2) /* ArmorModVsElectric */
     , (480616, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480616,   1, 'Life Magic Cloak of Protection') /* Name */
     , (480616,  16, 'Life Magic Cloak of Protection') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480616,   1, 0x02001B2A) /* Setup */
     , (480616,   3, 0x20000014) /* SoundTable */
     , (480616,   6, 0x100007F2) /* PaletteBase */
     /*, (480616,   7, 0x100007EB) /* ClothingBase */
	 , (480616,   7, 0x100007F6) /* ClothingBase */
     , (480616,   8, 0x0600709C) /* Icon */
     , (480616,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480616,  36, 0x0E00001E) /* MutateFilter */
     , (480616,  50, 100691000) /* IconOverlay */
     /*, (480616,  55,       5753)  ProcSpell */;
