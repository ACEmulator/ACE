DELETE FROM `weenie` WHERE `class_Id` = 450058;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450058, 'ace450058-rynthidenergytentaclestailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450058,   1,          4) /* ItemType - Clothing */
     , (450058,   4,     131072) /* ClothingPriority - 131072 */
     , (450058,   5,         10) /* EncumbranceVal */
     , (450058,   9,  134217728) /* ValidLocations - Cloak */
     , (450058,  16,          1) /* ItemUseable - No */
     , (450058,  18,          1) /* UiEffects - Magical */
     , (450058,  19,      20) /* Value */
     , (450058,  28,          0) /* ArmorLevel */
     , (450058,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450058,  22, True ) /* Inscribable */
     , (450058,  23, True ) /* DestroyOnSell */
     , (450058,  99, True ) /* Ivoryable */
     , (450058, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450058,  13,     0.8) /* ArmorModVsSlash */
     , (450058,  14,     0.8) /* ArmorModVsPierce */
     , (450058,  15,       1) /* ArmorModVsBludgeon */
     , (450058,  16,     0.2) /* ArmorModVsCold */
     , (450058,  17,     0.2) /* ArmorModVsFire */
     , (450058,  18,     0.1) /* ArmorModVsAcid */
     , (450058,  19,     0.2) /* ArmorModVsElectric */
     , (450058, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450058,   1, 'Rynthid Energy Tentacles') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450058,   1, 0x02001B2A) /* Setup */
     , (450058,   3, 0x20000014) /* SoundTable */
     , (450058,   7, 0x1000085F) /* ClothingBase */
     , (450058,   8, 0x060074E9) /* Icon */
     , (450058,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450058,  50, 0x06006C37) /* IconOverlay */;
