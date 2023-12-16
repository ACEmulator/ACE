DELETE FROM `weenie` WHERE `class_Id` = 480614;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480614, 'ace480614-cloakofprotectionwar', 2, '2022-04-19 17:09:43') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480614,   1,          4) /* ItemType - Clothing */
     , (480614,   3,          2) /* PaletteTemplate - Blue */
     , (480614,   4,     131072) /* ClothingPriority - 131072 */
     , (480614,   5,         75) /* EncumbranceVal */
     , (480614,   9,  134217728) /* ValidLocations - Cloak */
     , (480614,  16,          1) /* ItemUseable - No */
     , (480614,  18,          1) /* UiEffects - Magical */
     , (480614,  19,     5) /* Value */
     , (480614,  28,          0) /* ArmorLevel */
     , (480614,  36,       9999) /* ResistMagic */
     , (480614,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480614, 158,          7) /* WieldRequirements - Level */
     , (480614, 159,          1) /* WieldSkillType - Axe */
     , (480614, 160,        180) /* WieldDifficulty */
     , (480614, 169,         16) /* TsysMutationData */
     , (480614, 265,         81) /* EquipmentSetId - CloakMagicDefense */
     , (480614, 267,     604800) /* Lifespan */
     , (480614, 319,          5) /* ItemMaxLevel */
     , (480614, 320,          2) /* ItemXpStyle - ScalesWithLevel */
	 , (480614, 352,          2) /* CloakWeaveProc -200 dmg */
     , (480614, 371,          3) /* GearDamageResist */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (480614,   4, 31000000000) /* ItemTotalXp */
     , (480614,   5, 1000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480614,  22, True ) /* Inscribable */
     , (480614,  84, True ) /* IgnoreCloIcons */
     , (480614, 100, False) /* Dyable */
     , (480614, 112, True ) /* ProcSpellSelfTargeted */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480614,  13,     0.8) /* ArmorModVsSlash */
     , (480614,  14,     0.8) /* ArmorModVsPierce */
     , (480614,  15,       1) /* ArmorModVsBludgeon */
     , (480614,  16,     0.2) /* ArmorModVsCold */
     , (480614,  17,     0.2) /* ArmorModVsFire */
     , (480614,  18,     0.1) /* ArmorModVsAcid */
     , (480614,  19,     0.2) /* ArmorModVsElectric */
     , (480614, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480614,   1, 'War Magic Cloak of Protection') /* Name */
     , (480614,  16, 'War Magic Cloak of Protection') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480614,   1, 0x02001B2A) /* Setup */
     , (480614,   3, 0x20000014) /* SoundTable */
     , (480614,   6, 0x100007F2) /* PaletteBase */
     /*, (480614,   7, 0x100007EB) /* ClothingBase */
	 , (480614,   7, 0x100007F6) /* ClothingBase */
     , (480614,   8, 0x0600709C) /* Icon */
     , (480614,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480614,  36, 0x0E00001E) /* MutateFilter */
     , (480614,  50, 100691000) /* IconOverlay */
     /*, (480614,  55,       5753)  ProcSpell */;
