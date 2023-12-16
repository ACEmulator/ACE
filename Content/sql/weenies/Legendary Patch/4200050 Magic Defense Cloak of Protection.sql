DELETE FROM `weenie` WHERE `class_Id` = 4200050;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200050, 'ace4200050-cloakofprotection', 2, '2022-04-19 17:09:43') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200050,   1,          4) /* ItemType - Clothing */
     , (4200050,   3,          2) /* PaletteTemplate - Blue */
     , (4200050,   4,     131072) /* ClothingPriority - 131072 */
     , (4200050,   5,         75) /* EncumbranceVal */
     , (4200050,   9,  134217728) /* ValidLocations - Cloak */
     , (4200050,  16,          1) /* ItemUseable - No */
     , (4200050,  18,          1) /* UiEffects - Magical */
     , (4200050,  19,     5) /* Value */
     , (4200050,  28,          0) /* ArmorLevel */
     , (4200050,  36,       9999) /* ResistMagic */
     , (4200050,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200050, 158,          7) /* WieldRequirements - Level */
     , (4200050, 159,          1) /* WieldSkillType - Axe */
     , (4200050, 160,        180) /* WieldDifficulty */
     , (4200050, 169,         16) /* TsysMutationData */
     , (4200050, 265,         68) /* EquipmentSetId - CloakMagicDefense */
     , (4200050, 267,     604800) /* Lifespan */
     , (4200050, 319,          5) /* ItemMaxLevel */
     , (4200050, 320,          2) /* ItemXpStyle - ScalesWithLevel */
	 , (4200050, 352,          2) /* CloakWeaveProc -200 dmg */
     , (4200050, 371,          3) /* GearDamageResist */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (4200050,   4, 31000000000) /* ItemTotalXp */
     , (4200050,   5, 1000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200050,  22, True ) /* Inscribable */
     , (4200050,  84, True ) /* IgnoreCloIcons */
     , (4200050, 100, False) /* Dyable */
     , (4200050, 112, True ) /* ProcSpellSelfTargeted */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200050,  13,     0.8) /* ArmorModVsSlash */
     , (4200050,  14,     0.8) /* ArmorModVsPierce */
     , (4200050,  15,       1) /* ArmorModVsBludgeon */
     , (4200050,  16,     0.2) /* ArmorModVsCold */
     , (4200050,  17,     0.2) /* ArmorModVsFire */
     , (4200050,  18,     0.1) /* ArmorModVsAcid */
     , (4200050,  19,     0.2) /* ArmorModVsElectric */
     , (4200050, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200050,   1, 'Magic Defense Cloak of Protection') /* Name */
     , (4200050,  16, 'Magic Defense Cloak of Protection') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200050,   1, 0x02001B2A) /* Setup */
     , (4200050,   3, 0x20000014) /* SoundTable */
     , (4200050,   6, 0x100007F2) /* PaletteBase */
     /*, (4200050,   7, 0x100007EB) /* ClothingBase */
	 , (4200050,   7, 0x100007F6) /* ClothingBase */
     , (4200050,   8, 0x0600709C) /* Icon */
     , (4200050,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200050,  36, 0x0E00001E) /* MutateFilter */
     , (4200050,  50, 100691000) /* IconOverlay */
     /*, (4200050,  55,       5753)  ProcSpell */;
