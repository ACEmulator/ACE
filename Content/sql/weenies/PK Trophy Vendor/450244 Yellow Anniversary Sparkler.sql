DELETE FROM `weenie` WHERE `class_Id` = 450244;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450244, 'ace450244-yellowanniversarysparklertailor', 35, '2021-11-17 16:56:08') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450244,   1,      32768) /* ItemType - Caster */
     , (450244,   3,          2) /* PaletteTemplate - Blue */
     , (450244,   5,          0) /* EncumbranceVal */
     , (450244,   9,   16777216) /* ValidLocations - Held */
     , (450244,  16,          1) /* ItemUseable - No */
     , (450244,  19,         20) /* Value */
     , (450244,  46,        512) /* DefaultCombatStyle - Magic */
     , (450244,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450244,  94,         16) /* TargetType - Creature */
     , (450244, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450244,  11, True ) /* IgnoreCollisions */
     , (450244,  13, True ) /* Ethereal */
     , (450244,  14, True ) /* GravityStatus */
     , (450244,  19, True ) /* Attackable */
     , (450244,  22, True ) /* Inscribable */
     , (450244,  84, True ) /* IgnoreCloIcons */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450244,  12,       0) /* Shade */
     , (450244,  29,       1) /* WeaponDefense */
     , (450244,  39,     1.5) /* DefaultScale */
     , (450244, 144,       0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450244,   1, 'Yellow Anniversary Sparkler') /* Name */
     , (450244,  16, 'A bright sparkler meant to help commemorate Festival season and the anniversary of Asheron''s Call.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450244,   1, 0x02001639) /* Setup */
     , (450244,   3, 0x20000014) /* SoundTable */
     , (450244,   6, 0x04000BEF) /* PaletteBase */
     , (450244,   7, 0x1000012E) /* ClothingBase */
     , (450244,   8, 0x06006542) /* Icon */
     , (450244,  22, 0x3400002B) /* PhysicsEffectTable */;
