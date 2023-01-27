DELETE FROM `weenie` WHERE `class_Id` = 450240;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450240, 'ace450240-orangeanniversarysparklertailor', 35, '2021-11-17 16:56:08') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450240,   1,      32768) /* ItemType - Caster */
     , (450240,   3,          2) /* PaletteTemplate - Blue */
     , (450240,   5,          0) /* EncumbranceVal */
     , (450240,   9,   16777216) /* ValidLocations - Held */
     , (450240,  16,          1) /* ItemUseable - No */
     , (450240,  19,         20) /* Value */
     , (450240,  46,        512) /* DefaultCombatStyle - Magic */
     , (450240,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450240,  94,         16) /* TargetType - Creature */
     , (450240, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450240,  11, True ) /* IgnoreCollisions */
     , (450240,  13, True ) /* Ethereal */
     , (450240,  14, True ) /* GravityStatus */
     , (450240,  19, True ) /* Attackable */
     , (450240,  22, True ) /* Inscribable */
     , (450240,  84, True ) /* IgnoreCloIcons */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450240,  12,       0) /* Shade */
     , (450240,  29,       1) /* WeaponDefense */
     , (450240,  39,     1.5) /* DefaultScale */
     , (450240, 144,       0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450240,   1, 'Orange Anniversary Sparkler') /* Name */
     , (450240,  16, 'A bright sparkler meant to help commemorate Festival season and the anniversary of Asheron''s Call.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450240,   1, 0x02001635) /* Setup */
     , (450240,   3, 0x20000014) /* SoundTable */
     , (450240,   6, 0x04000BEF) /* PaletteBase */
     , (450240,   7, 0x1000012E) /* ClothingBase */
     , (450240,   8, 0x06006543) /* Icon */
     , (450240,  22, 0x3400002B) /* PhysicsEffectTable */;
