DELETE FROM `weenie` WHERE `class_Id` = 450059;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450059, 'ace450059-rynthidenergyfieldtailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450059,   1,          4) /* ItemType - Clothing */
     , (450059,   4,     131072) /* ClothingPriority - 131072 */
     , (450059,   5,         10) /* EncumbranceVal */
     , (450059,   9,  134217728) /* ValidLocations - Cloak */
     , (450059,  16,          1) /* ItemUseable - No */
     , (450059,  18,          1) /* UiEffects - Magical */
     , (450059,  19,      20) /* Value */
     , (450059,  28,          0) /* ArmorLevel */
     , (450059,  36,       9999) /* ResistMagic */
     , (450059,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450059,  22, True ) /* Inscribable */
     , (450059,  23, True ) /* DestroyOnSell */
     , (450059,  99, True ) /* Ivoryable */
     , (450059, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450059,  13,     0.8) /* ArmorModVsSlash */
     , (450059,  14,     0.8) /* ArmorModVsPierce */
     , (450059,  15,       1) /* ArmorModVsBludgeon */
     , (450059,  16,     0.2) /* ArmorModVsCold */
     , (450059,  17,     0.2) /* ArmorModVsFire */
     , (450059,  18,     0.1) /* ArmorModVsAcid */
     , (450059,  19,     0.2) /* ArmorModVsElectric */
     , (450059, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450059,   1, 'Rynthid Energy Field') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450059,   1, 0x02001B2A) /* Setup */
     , (450059,   3, 0x20000014) /* SoundTable */
     , (450059,   7, 0x1000085E) /* ClothingBase */
     , (450059,   8, 0x060074E8) /* Icon */
     , (450059,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450059,  50, 0x06006C37) /* IconOverlay */;
