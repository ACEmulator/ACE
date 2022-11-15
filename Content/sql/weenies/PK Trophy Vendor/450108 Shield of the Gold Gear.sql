DELETE FROM `weenie` WHERE `class_Id` = 450108;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450108, 'ace450108-shieldofthegoldgeartailor', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450108,   1,          2) /* ItemType - Armor */
     , (450108,   5,        0) /* EncumbranceVal */
     , (450108,   9,    2097152) /* ValidLocations - Shield */
     , (450108,  16,          1) /* ItemUseable - No */
     , (450108,  18,          1) /* UiEffects - Magical */
     , (450108,  19,         20) /* Value */
     , (450108,  28,        0) /* ArmorLevel */
     , (450108,  51,          4) /* CombatUse - Shield */
     , (450108,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450108, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450108,  22, True ) /* Inscribable */
     , (450108,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450108,   5,  -0.025) /* ManaRate */
     , (450108,  13,     1.3) /* ArmorModVsSlash */
     , (450108,  14,     1.3) /* ArmorModVsPierce */
     , (450108,  15,     1.3) /* ArmorModVsBludgeon */
     , (450108,  16,     0.8) /* ArmorModVsCold */
     , (450108,  17,       1) /* ArmorModVsFire */
     , (450108,  18,     0.8) /* ArmorModVsAcid */
     , (450108,  19,     1.2) /* ArmorModVsElectric */
     , (450108, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450108,   1, 'Shield of the Gold Gear') /* Name */
     , (450108,  16, 'A shield forged in the Gear Knight style.  A minor Gearcrafting effect has been added to further protect its wielder.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450108,   1, 0x02001A08) /* Setup */
     , (450108,   3, 0x20000014) /* SoundTable */
     , (450108,   7, 0x10000800) /* ClothingBase */
     , (450108,   8, 0x06006E08) /* Icon */
     , (450108,  22, 0x3400002B) /* PhysicsEffectTable */;


