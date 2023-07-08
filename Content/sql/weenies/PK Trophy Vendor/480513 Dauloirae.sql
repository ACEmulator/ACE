DELETE FROM `weenie` WHERE `class_Id` = 480513;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480513, 'shieldgaerlanpk', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480513,   1,          2) /* ItemType - Armor */
     , (480513,   5,        0) /* EncumbranceVal */
     , (480513,   8,        880) /* Mass */
     , (480513,   9,    2097152) /* ValidLocations - Shield */
     , (480513,  16,          1) /* ItemUseable - No */
     , (480513,  18,          1) /* UiEffects - Magical */
     , (480513,  19,       20) /* Value */
     , (480513,  27,          2) /* ArmorType - Leather */
     , (480513,  28,        0) /* ArmorLevel */
     , (480513,  51,          4) /* CombatUse - Shield */
     , (480513,  56,        0) /* ShieldValue */
     , (480513,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480513, 150,        103) /* HookPlacement - Hook */
     , (480513, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480513,  22, True ) /* Inscribable */
     , (480513,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480513,   5,   -0.05) /* ManaRate */
     , (480513,  13,     0.8) /* ArmorModVsSlash */
     , (480513,  14,     1.2) /* ArmorModVsPierce */
     , (480513,  15,     0.5) /* ArmorModVsBludgeon */
     , (480513,  16,     0.5) /* ArmorModVsCold */
     , (480513,  17,     0.5) /* ArmorModVsFire */
     , (480513,  18,     1.2) /* ArmorModVsAcid */
     , (480513,  19,     0.5) /* ArmorModVsElectric */
     , (480513, 110,       1) /* BulkMod */
     , (480513, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480513,   1, 'Dauloirae') /* Name */
     , (480513,  15, 'An obsidian shield enhanced to defend against the piercing attacks of olthoi.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480513,   1, 0x02000DCC) /* Setup */
     , (480513,   3, 0x20000014) /* SoundTable */
     , (480513,   8, 0x060027D3) /* Icon */
     , (480513,  22, 0x3400002B) /* PhysicsEffectTable */;


