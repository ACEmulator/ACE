DELETE FROM `weenie` WHERE `class_Id` = 480516;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480516, 'ace480516-housemhoireshieldpk', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480516,   1,          2) /* ItemType - Armor */
     , (480516,   5,       0) /* EncumbranceVal */
     , (480516,   8,        230) /* Mass */
     , (480516,   9,    2097152) /* ValidLocations - Shield */
     , (480516,  16,          1) /* ItemUseable - No */
     , (480516,  19,      20) /* Value */
     , (480516,  27,          2) /* ArmorType - Leather */
     , (480516,  28,        0) /* ArmorLevel */
     , (480516,  51,          4) /* CombatUse - Shield */
     , (480516,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480516, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480516,  22, True ) /* Inscribable */
     , (480516,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480516,   5,  -0.033) /* ManaRate */
     , (480516,  13,       1) /* ArmorModVsSlash */
     , (480516,  14,     0.8) /* ArmorModVsPierce */
     , (480516,  15,     1.2) /* ArmorModVsBludgeon */
     , (480516,  16,     0.6) /* ArmorModVsCold */
     , (480516,  17,     0.6) /* ArmorModVsFire */
     , (480516,  18,       1) /* ArmorModVsAcid */
     , (480516,  19,     0.6) /* ArmorModVsElectric */
     , (480516,  39,     1.3) /* DefaultScale */
     , (480516, 110,       1) /* BulkMod */
     , (480516, 111,    1.33) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480516,   1, 'House Mhoire Shield') /* Name */
     , (480516,  15, 'This shield was crafted for the Lords of House Mhoire.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480516,   1, 0x020017F9) /* Setup */
     , (480516,   3, 0x20000014) /* SoundTable */
     , (480516,   8, 0x060067E7) /* Icon */
     , (480516,  22, 0x3400002B) /* PhysicsEffectTable */;
