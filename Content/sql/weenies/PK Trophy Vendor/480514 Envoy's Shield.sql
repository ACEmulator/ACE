DELETE FROM `weenie` WHERE `class_Id` = 480514;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480514, 'shieldenvoypk', 1, '2005-02-09 10:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480514,   1,          2) /* ItemType - Armor */
     , (480514,   5,          0) /* EncumbranceVal */
     , (480514,   8,          5) /* Mass */
     , (480514,   9,    2097152) /* ValidLocations - Shield */
     , (480514,  16,          1) /* ItemUseable - No */
     , (480514,  19,        20) /* Value */
     , (480514,  27,          2) /* ArmorType - Leather */
     , (480514,  28,         0) /* ArmorLevel */
     , (480514,  51,          4) /* CombatUse - Shield */
     , (480514,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480514, 150,        103) /* HookPlacement - Hook */
     , (480514, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480514,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480514,  13,       1) /* ArmorModVsSlash */
     , (480514,  14,     0.8) /* ArmorModVsPierce */
     , (480514,  15,     1.2) /* ArmorModVsBludgeon */
     , (480514,  16,     0.6) /* ArmorModVsCold */
     , (480514,  17,     0.6) /* ArmorModVsFire */
     , (480514,  18,       1) /* ArmorModVsAcid */
     , (480514,  19,     0.6) /* ArmorModVsElectric */
     , (480514,  39,       1) /* DefaultScale */
     , (480514, 110,       1) /* BulkMod */
     , (480514, 111,    1.33) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480514,   1, 'Envoy''s Shield') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480514,   1, 0x02001035) /* Setup */
     , (480514,   3, 0x20000014) /* SoundTable */
     , (480514,   8, 0x060013FF) /* Icon */
     , (480514,  22, 0x3400002B) /* PhysicsEffectTable */;
