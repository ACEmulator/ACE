DELETE FROM `weenie` WHERE `class_Id` = 480515;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480515, 'ace480515-shieldofperfectlightpk', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480515,   1,          2) /* ItemType - Armor */
     , (480515,   5,        0) /* EncumbranceVal */
     , (480515,   9,    2097152) /* ValidLocations - Shield */
     , (480515,  16,          1) /* ItemUseable - No */
     , (480515,  18,          1) /* UiEffects - Magical */
     , (480515,  19,      20) /* Value */
     , (480515,  28,        0) /* ArmorLevel */
     , (480515,  51,          4) /* CombatUse - Shield */
     , (480515,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480515, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480515,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480515,   5,  -0.025) /* ManaRate */
     , (480515,  13,     1.8) /* ArmorModVsSlash */
     , (480515,  14,       1) /* ArmorModVsPierce */
     , (480515,  15,     1.8) /* ArmorModVsBludgeon */
     , (480515,  16,       2) /* ArmorModVsCold */
     , (480515,  17,     0.8) /* ArmorModVsFire */
     , (480515,  18,       2) /* ArmorModVsAcid */
     , (480515,  19,     0.8) /* ArmorModVsElectric */
     , (480515, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480515,   1, 'Shield of Perfect Light') /* Name */
     , (480515,  16, 'A shield glowing with a brilliant light. Although the shield looks insubstantial it strongly resists your efforts to penetrate the magical barrier it contains.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480515,   1, 0x020016CD) /* Setup */
     , (480515,   3, 0x20000014) /* SoundTable */
     , (480515,   7, 0x1000080A) /* ClothingBase */
     , (480515,   8, 0x06006615) /* Icon */
     , (480515,  22, 0x3400002B) /* PhysicsEffectTable */;


