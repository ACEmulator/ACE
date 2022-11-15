DELETE FROM `weenie` WHERE `class_Id` = 450086;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450086, 'shieldenvoytailor', 1, '2005-02-09 10:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450086,   1,          2) /* ItemType - Armor */
     , (450086,   5,          0) /* EncumbranceVal */
     , (450086,   8,          5) /* Mass */
     , (450086,   9,    2097152) /* ValidLocations - Shield */
     , (450086,  16,          1) /* ItemUseable - No */
     , (450086,  19,        20) /* Value */
     , (450086,  27,          2) /* ArmorType - Leather */
     , (450086,  28,         0) /* ArmorLevel */
     , (450086,  51,          4) /* CombatUse - Shield */
     , (450086,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450086, 150,        103) /* HookPlacement - Hook */
     , (450086, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450086,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450086,  13,       1) /* ArmorModVsSlash */
     , (450086,  14,     0.8) /* ArmorModVsPierce */
     , (450086,  15,     1.2) /* ArmorModVsBludgeon */
     , (450086,  16,     0.6) /* ArmorModVsCold */
     , (450086,  17,     0.6) /* ArmorModVsFire */
     , (450086,  18,       1) /* ArmorModVsAcid */
     , (450086,  19,     0.6) /* ArmorModVsElectric */
     , (450086,  39,       1) /* DefaultScale */
     , (450086, 110,       1) /* BulkMod */
     , (450086, 111,    1.33) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450086,   1, 'Envoy''s Shield') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450086,   1, 0x02001035) /* Setup */
     , (450086,   3, 0x20000014) /* SoundTable */
     , (450086,   8, 0x060013FF) /* Icon */
     , (450086,  22, 0x3400002B) /* PhysicsEffectTable */;
