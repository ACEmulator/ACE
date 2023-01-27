DELETE FROM `weenie` WHERE `class_Id` = 450242;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450242, 'ace450242-redanniversarysparklertailor', 35, '2021-11-17 16:56:08') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450242,   1,      32768) /* ItemType - Caster */
     , (450242,   3,          2) /* PaletteTemplate - Blue */
     , (450242,   5,          0) /* EncumbranceVal */
     , (450242,   9,   16777216) /* ValidLocations - Held */
     , (450242,  16,          1) /* ItemUseable - No */
     , (450242,  19,         20) /* Value */
     , (450242,  46,        512) /* DefaultCombatStyle - Magic */
     , (450242,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450242,  94,         16) /* TargetType - Creature */
     , (450242, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450242,  11, True ) /* IgnoreCollisions */
     , (450242,  13, True ) /* Ethereal */
     , (450242,  14, True ) /* GravityStatus */
     , (450242,  19, True ) /* Attackable */
     , (450242,  22, True ) /* Inscribable */
     , (450242,  84, True ) /* IgnoreCloIcons */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450242,  12,       0) /* Shade */
     , (450242,  29,       1) /* WeaponDefense */
     , (450242,  39,     1.5) /* DefaultScale */
     , (450242, 144,       0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450242,   1, 'Red Anniversary Sparkler') /* Name */
     , (450242,  16, 'A bright sparkler meant to help commemorate Festival season and the anniversary of Asheron''s Call.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450242,   1, 0x02001637) /* Setup */
     , (450242,   3, 0x20000014) /* SoundTable */
     , (450242,   6, 0x04000BEF) /* PaletteBase */
     , (450242,   7, 0x1000012E) /* ClothingBase */
     , (450242,   8, 0x06006540) /* Icon */
     , (450242,  22, 0x3400002B) /* PhysicsEffectTable */;
