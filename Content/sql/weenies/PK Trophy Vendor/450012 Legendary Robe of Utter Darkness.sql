DELETE FROM `weenie` WHERE `class_Id` = 450012;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450012, 'ace450012-legendaryrobeofutterdarknesstailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450012,   1,          4) /* ItemType - Clothing */
     , (450012,   4,       1024) /* ClothingPriority - OuterwearChest */
     , (450012,   5,        0) /* EncumbranceVal */
     , (450012,   9,        512) /* ValidLocations - ChestArmor */
     , (450012,  16,          1) /* ItemUseable - No */
     , (450012,  18,          1) /* UiEffects - Magical */
     , (450012,  19,     20) /* Value */
     , (450012,  28,       0) /* ArmorLevel */
     , (450012,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450012,  22, True ) /* Inscribable */
     , (450012,  69, False) /* IsSellable */
     , (450012,  99, False) /* Ivoryable */
     , (450012, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450012,   5,    -0.5) /* ManaRate */
     , (450012,  13,     0.6) /* ArmorModVsSlash */
     , (450012,  14,     0.6) /* ArmorModVsPierce */
     , (450012,  15,     0.6) /* ArmorModVsBludgeon */
     , (450012,  16,     0.6) /* ArmorModVsCold */
     , (450012,  17,     0.6) /* ArmorModVsFire */
     , (450012,  18,     0.6) /* ArmorModVsAcid */
     , (450012,  19,     0.6) /* ArmorModVsElectric */
     , (450012, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450012,   1, 'Legendary Robe of Utter Darkness') /* Name */
     , (450012,  14, 'This robe may be tailored onto breastplates.') /* Use */
     , (450012,  16, 'Hoshino Kei''s corrupted Robe of Perfect Light, which became infused with Dark Falatacot Magics during the ritual which raised her as one of the undead.') /* LongDesc */
     , (450012,  33, 'UtterDarknessRobePickup') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450012,   1, 0x020001A6) /* Setup */
     , (450012,   3, 0x20000014) /* SoundTable */
     , (450012,   7, 0x10000824) /* ClothingBase */
     , (450012,   8, 0x060072AE) /* Icon */
     , (450012,  22, 0x3400002B) /* PhysicsEffectTable */;


