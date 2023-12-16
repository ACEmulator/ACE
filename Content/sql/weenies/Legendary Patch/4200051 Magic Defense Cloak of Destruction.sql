DELETE FROM `weenie` WHERE `class_Id` = 4200051;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200051, 'ace4200051-cloakofdestruction', 2, '2022-04-19 17:09:43') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200051,   1,          4) /* ItemType - Clothing */
     , (4200051,   3,          2) /* PaletteTemplate - Blue */
     , (4200051,   4,     131072) /* ClothingPriority - 131072 */
     , (4200051,   5,         75) /* EncumbranceVal */
     , (4200051,   9,  134217728) /* ValidLocations - Cloak */
     , (4200051,  16,          1) /* ItemUseable - No */
     , (4200051,  18,          1) /* UiEffects - Magical */
     , (4200051,  19,     5) /* Value */
     , (4200051,  28,          0) /* ArmorLevel */
     , (4200051,  36,       9999) /* ResistMagic */
     , (4200051,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200051, 158,          7) /* WieldRequirements - Level */
     , (4200051, 159,          1) /* WieldSkillType - Axe */
     , (4200051, 160,        180) /* WieldDifficulty */
     , (4200051, 169,         16) /* TsysMutationData */
     , (4200051, 265,         68) /* EquipmentSetId - CloakMagicDefense */
     , (4200051, 267,     604800) /* Lifespan */
     , (4200051, 319,          5) /* ItemMaxLevel */
     , (4200051, 320,          2) /* ItemXpStyle - ScalesWithLevel */
     , (4200051, 370,          3) /* GearDamage */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (4200051,   4, 31000000000) /* ItemTotalXp */
     , (4200051,   5, 1000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200051,  22, True ) /* Inscribable */
     , (4200051,  84, True ) /* IgnoreCloIcons */
     , (4200051, 100, False) /* Dyable */
     , (4200051, 112, True ) /* ProcSpellSelfTargeted */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200051,  13,     0.8) /* ArmorModVsSlash */
     , (4200051,  14,     0.8) /* ArmorModVsPierce */
     , (4200051,  15,       1) /* ArmorModVsBludgeon */
     , (4200051,  16,     0.2) /* ArmorModVsCold */
     , (4200051,  17,     0.2) /* ArmorModVsFire */
     , (4200051,  18,     0.1) /* ArmorModVsAcid */
     , (4200051,  19,     0.2) /* ArmorModVsElectric */
     , (4200051, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200051,   1, 'Magic Defense Cloak of Destruction') /* Name */
     , (4200051,  16, 'Magic Defense Cloak of Destruction') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200051,   1, 0x02001B2A) /* Setup */
     , (4200051,   3, 0x20000014) /* SoundTable */
     , (4200051,   6, 0x100007F2) /* PaletteBase */
     /*, (4200051,   7, 0x100007EB) /* ClothingBase */
	 , (4200051,   7, 0x1000085E) /* ClothingBase */
     , (4200051,   8, 0x0600709C) /* Icon */
     , (4200051,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200051,  36, 0x0E00001E) /* MutateFilter */
     , (4200051,  50, 100691000) /* IconOverlay */
     , (4200051,  55,       5753) /* ProcSpell */;
