DELETE FROM `weenie` WHERE `class_Id` = 480517;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480517, 'ace480517-pumpkinshieldpk', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480517,   1,          2) /* ItemType - Armor */
     , (480517,   5,        0) /* EncumbranceVal */
     , (480517,   9,    2097152) /* ValidLocations - Shield */
     , (480517,  16,          1) /* ItemUseable - No */
     , (480517,  18,          1) /* UiEffects - Magical */
     , (480517,  19,       20) /* Value */
     , (480517,  27,          2) /* ArmorType - Leather */
     , (480517,  28,        0) /* ArmorLevel */
     , (480517,  51,          4) /* CombatUse - Shield */
     , (480517,  56,        0) /* ShieldValue */
     , (480517,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480517, 150,        103) /* HookPlacement - Hook */
     , (480517, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480517,  22, True ) /* Inscribable */
     , (480517,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480517,   5,   -0.05) /* ManaRate */
     , (480517,  13,     1.5) /* ArmorModVsSlash */
     , (480517,  14,     1.5) /* ArmorModVsPierce */
     , (480517,  15,     1.5) /* ArmorModVsBludgeon */
     , (480517,  16,     0.8) /* ArmorModVsCold */
     , (480517,  17,     0.8) /* ArmorModVsFire */
     , (480517,  18,     0.8) /* ArmorModVsAcid */
     , (480517,  19,     0.8) /* ArmorModVsElectric */
     , (480517,  39,     0.9) /* DefaultScale */
     , (480517, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480517,   1, 'Pumpkin Shield') /* Name */
     , (480517,  16, 'The thick shell of a large pumpkin. It''s surprisingly strong and lightweight.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480517,   1, 0x0200174B) /* Setup */
     , (480517,   3, 0x20000014) /* SoundTable */
     , (480517,   7, 0x10000811) /* ClothingBase */
     , (480517,   8, 0x06001E2B) /* Icon */
     , (480517,  22, 0x3400002B) /* PhysicsEffectTable */;


