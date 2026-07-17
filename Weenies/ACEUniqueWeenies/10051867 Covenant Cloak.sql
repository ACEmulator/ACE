DELETE FROM `weenie` WHERE `class_Id` = 10051867;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10051867, 'ace10051867-covenantCloak', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10051867,   1,          4) /* ItemType - Clothing */
     , (10051867,   4,     131072) /* ClothingPriority - 131072 */
     , (10051867,   5,         10) /* EncumbranceVal */
     , (10051867,   9,  134217728) /* ValidLocations - Cloak */
     , (10051867,  16,          1) /* ItemUseable - No */
     , (10051867,  18,          1) /* UiEffects - Magical */
     , (10051867,  19,      100) /* Value */
     , (10051867,  28,          0) /* ArmorLevel */
     , (10051867,  33,          1) /* Bonded - Bonded */
     , (10051867,  36,       9999) /* ResistMagic */
     , (10051867,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     -- , (10051867, 114,          1) /* Attuned - Attuned */
     , (10051867, 158,          7) /* WieldRequirements - Level */
     , (10051867, 159,          1) /* WieldSkillType - Axe */
     -- , (10051867, 160,        180) /* WieldDifficulty */
     -- , (10051867, 265,         68) /* EquipmentSetId - CloakMagicDefense */
     , (10051867, 319,          5) /* ItemMaxLevel */
     , (10051867, 320,          1) /* ItemXpStyle - ScalesWithLevel */
     , (10051867, 352,          1) /* CloakWeaveProc */
     , (10051867,  371,       3) /* Dmg rating */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (10051867,   4,          0) /* ItemTotalXp */
     , (10051867,   5, 1000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10051867,  22, True ) /* Inscribable */
     , (10051867,  23, True ) /* DestroyOnSell */
     , (10051867,  99, True ) /* Ivoryable */
     , (10051867, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10051867,  13,     0.8) /* ArmorModVsSlash */
     , (10051867,  14,     0.8) /* ArmorModVsPierce */
     , (10051867,  15,       1) /* ArmorModVsBludgeon */
     , (10051867,  16,     0.2) /* ArmorModVsCold */
     , (10051867,  17,     0.2) /* ArmorModVsFire */
     , (10051867,  18,     0.1) /* ArmorModVsAcid */
     , (10051867,  19,     0.2) /* ArmorModVsElectric */
     , (10051867, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10051867,   1, 'Covenant Cloak') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10051867,   1, 0x02001B2A) /* Setup */
     , (10051867,   3, 0x20000014) /* SoundTable */
     , (10051867,   7, 0x100007F4) /* ClothingBase */
     , (10051867,   6, 0x100007F2) /* PaletteBase */
     , (10051867,   8, 0x060070A4) /* Icon */
     , (10051867,  22, 0x3400002B) /* PhysicsEffectTable */
     , (10051867,  50, 0x06006C38) /* IconOverlay */
     , (10051867,  55,       4654) /* ProcSpell - Mana to health self 7 */;
