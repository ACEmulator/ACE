DELETE FROM `weenie` WHERE `class_Id` = 480615;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480615, 'ace480615-cloakofprotectionmelee', 2, '2022-04-19 17:09:43') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480615,   1,          4) /* ItemType - Clothing */
     , (480615,   3,          2) /* PaletteTemplate - Blue */
     , (480615,   4,     131072) /* ClothingPriority - 131072 */
     , (480615,   5,         75) /* EncumbranceVal */
     , (480615,   9,  134217728) /* ValidLocations - Cloak */
     , (480615,  16,          1) /* ItemUseable - No */
     , (480615,  18,          1) /* UiEffects - Magical */
     , (480615,  19,     5) /* Value */
     , (480615,  28,          0) /* ArmorLevel */
     , (480615,  36,       9999) /* ResistMagic */
     , (480615,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480615, 158,          7) /* WieldRequirements - Level */
     , (480615, 159,          1) /* WieldSkillType - Axe */
     , (480615, 160,        180) /* WieldDifficulty */
     , (480615, 169,         16) /* TsysMutationData */
     , (480615, 265,         71) /* EquipmentSetId - CloakMagicDefense */
     , (480615, 267,     604800) /* Lifespan */
     , (480615, 319,          5) /* ItemMaxLevel */
     , (480615, 320,          2) /* ItemXpStyle - ScalesWithLevel */
	 , (480615, 352,          2) /* CloakWeaveProc -200 dmg */
     , (480615, 371,          3) /* GearDamageResist */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (480615,   4, 31000000000) /* ItemTotalXp */
     , (480615,   5, 1000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480615,  22, True ) /* Inscribable */
     , (480615,  84, True ) /* IgnoreCloIcons */
     , (480615, 100, False) /* Dyable */
     , (480615, 112, True ) /* ProcSpellSelfTargeted */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480615,  13,     0.8) /* ArmorModVsSlash */
     , (480615,  14,     0.8) /* ArmorModVsPierce */
     , (480615,  15,       1) /* ArmorModVsBludgeon */
     , (480615,  16,     0.2) /* ArmorModVsCold */
     , (480615,  17,     0.2) /* ArmorModVsFire */
     , (480615,  18,     0.1) /* ArmorModVsAcid */
     , (480615,  19,     0.2) /* ArmorModVsElectric */
     , (480615, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480615,   1, 'Melee Defense Cloak of Protection') /* Name */
     , (480615,  16, 'Melee Defense Cloak of Protection') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480615,   1, 0x02001B2A) /* Setup */
     , (480615,   3, 0x20000014) /* SoundTable */
     , (480615,   6, 0x100007F2) /* PaletteBase */
     /*, (480615,   7, 0x100007EB) /* ClothingBase */
	 , (480615,   7, 0x100007F6) /* ClothingBase */
     , (480615,   8, 0x0600709C) /* Icon */
     , (480615,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480615,  36, 0x0E00001E) /* MutateFilter */
     , (480615,  50, 100691000) /* IconOverlay */
     /*, (480615,  55,       5753)  ProcSpell */;
