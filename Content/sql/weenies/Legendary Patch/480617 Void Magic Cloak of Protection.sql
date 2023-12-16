DELETE FROM `weenie` WHERE `class_Id` = 480617;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480617, 'ace480617-cloakofprotectionvoid', 2, '2022-04-19 17:09:43') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480617,   1,          4) /* ItemType - Clothing */
     , (480617,   3,          2) /* PaletteTemplate - Blue */
     , (480617,   4,     131072) /* ClothingPriority - 131072 */
     , (480617,   5,         75) /* EncumbranceVal */
     , (480617,   9,  134217728) /* ValidLocations - Cloak */
     , (480617,  16,          1) /* ItemUseable - No */
     , (480617,  18,          1) /* UiEffects - Magical */
     , (480617,  19,     5) /* Value */
     , (480617,  28,          0) /* ArmorLevel */
     , (480617,  36,       9999) /* ResistMagic */
     , (480617,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480617, 158,          7) /* WieldRequirements - Level */
     , (480617, 159,          1) /* WieldSkillType - Axe */
     , (480617, 160,        180) /* WieldDifficulty */
     , (480617, 169,         16) /* TsysMutationData */
     , (480617, 265,         80) /* EquipmentSetId - CloakMagicDefense */
     , (480617, 267,     604800) /* Lifespan */
     , (480617, 319,          5) /* ItemMaxLevel */
     , (480617, 320,          2) /* ItemXpStyle - ScalesWithLevel */
	 , (480617, 352,          2) /* CloakWeaveProc -200 dmg */
     , (480617, 371,          3) /* GearDamageResist */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (480617,   4, 31000000000) /* ItemTotalXp */
     , (480617,   5, 1000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480617,  22, True ) /* Inscribable */
     , (480617,  84, True ) /* IgnoreCloIcons */
     , (480617, 100, False) /* Dyable */
     , (480617, 112, True ) /* ProcSpellSelfTargeted */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480617,  13,     0.8) /* ArmorModVsSlash */
     , (480617,  14,     0.8) /* ArmorModVsPierce */
     , (480617,  15,       1) /* ArmorModVsBludgeon */
     , (480617,  16,     0.2) /* ArmorModVsCold */
     , (480617,  17,     0.2) /* ArmorModVsFire */
     , (480617,  18,     0.1) /* ArmorModVsAcid */
     , (480617,  19,     0.2) /* ArmorModVsElectric */
     , (480617, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480617,   1, 'Void Cloak of Protection') /* Name */
     , (480617,  16, 'Void Cloak of Protection') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480617,   1, 0x02001B2A) /* Setup */
     , (480617,   3, 0x20000014) /* SoundTable */
     , (480617,   6, 0x100007F2) /* PaletteBase */
     /*, (480617,   7, 0x100007EB) /* ClothingBase */
	 , (480617,   7, 0x100007F6) /* ClothingBase */
     , (480617,   8, 0x0600709C) /* Icon */
     , (480617,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480617,  36, 0x0E00001E) /* MutateFilter */
     , (480617,  50, 100691000) /* IconOverlay */
     /*, (480617,  55,       5753)  ProcSpell */;
