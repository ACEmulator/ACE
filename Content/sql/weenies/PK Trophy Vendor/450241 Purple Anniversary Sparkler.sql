DELETE FROM `weenie` WHERE `class_Id` = 450241;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450241, 'ace450241-purpleanniversarysparklertailor', 35, '2021-11-17 16:56:08') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450241,   1,      32768) /* ItemType - Caster */
     , (450241,   3,          2) /* PaletteTemplate - Blue */
     , (450241,   5,          0) /* EncumbranceVal */
     , (450241,   9,   16777216) /* ValidLocations - Held */
     , (450241,  16,          1) /* ItemUseable - No */
     , (450241,  19,         20) /* Value */
     , (450241,  46,        512) /* DefaultCombatStyle - Magic */
     , (450241,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450241,  94,         16) /* TargetType - Creature */
     , (450241, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450241,  11, True ) /* IgnoreCollisions */
     , (450241,  13, True ) /* Ethereal */
     , (450241,  14, True ) /* GravityStatus */
     , (450241,  19, True ) /* Attackable */
     , (450241,  22, True ) /* Inscribable */
     , (450241,  84, True ) /* IgnoreCloIcons */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450241,  12,       0) /* Shade */
     , (450241,  29,       1) /* WeaponDefense */
     , (450241,  39,     1.5) /* DefaultScale */
     , (450241, 144,       0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450241,   1, 'Purple Anniversary Sparkler') /* Name */
     , (450241,  16, 'A bright sparkler meant to help commemorate Festival season and the anniversary of Asheron''s Call.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450241,   1, 0x02001636) /* Setup */
     , (450241,   3, 0x20000014) /* SoundTable */
     , (450241,   6, 0x04000BEF) /* PaletteBase */
     , (450241,   7, 0x1000012E) /* ClothingBase */
     , (450241,   8, 0x0600653F) /* Icon */
     , (450241,  22, 0x3400002B) /* PhysicsEffectTable */;
