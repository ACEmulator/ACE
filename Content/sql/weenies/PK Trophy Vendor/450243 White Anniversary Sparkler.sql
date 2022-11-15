DELETE FROM `weenie` WHERE `class_Id` = 450243;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450243, 'ace450243-whiteanniversarysparklertailor', 35, '2021-11-17 16:56:08') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450243,   1,      32768) /* ItemType - Caster */
     , (450243,   3,          2) /* PaletteTemplate - Blue */
     , (450243,   5,          0) /* EncumbranceVal */
     , (450243,   9,   16777216) /* ValidLocations - Held */
     , (450243,  16,          1) /* ItemUseable - No */
     , (450243,  19,         20) /* Value */
     , (450243,  46,        512) /* DefaultCombatStyle - Magic */
     , (450243,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450243,  94,         16) /* TargetType - Creature */
     , (450243, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450243,  11, True ) /* IgnoreCollisions */
     , (450243,  13, True ) /* Ethereal */
     , (450243,  14, True ) /* GravityStatus */
     , (450243,  19, True ) /* Attackable */
     , (450243,  22, True ) /* Inscribable */
     , (450243,  84, True ) /* IgnoreCloIcons */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450243,  12,       0) /* Shade */
     , (450243,  29,       1) /* WeaponDefense */
     , (450243,  39,     1.5) /* DefaultScale */
     , (450243, 144,       0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450243,   1, 'White Anniversary Sparkler') /* Name */
     , (450243,  16, 'A bright sparkler meant to help commemorate Festival season and the anniversary of Asheron''s Call.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450243,   1, 0x02001638) /* Setup */
     , (450243,   3, 0x20000014) /* SoundTable */
     , (450243,   6, 0x04000BEF) /* PaletteBase */
     , (450243,   7, 0x1000012E) /* ClothingBase */
     , (450243,   8, 0x06006541) /* Icon */
     , (450243,  22, 0x3400002B) /* PhysicsEffectTable */;
