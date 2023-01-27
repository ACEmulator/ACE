DELETE FROM `weenie` WHERE `class_Id` = 450239;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450239, 'ace450239-greenanniversarysparklertailor', 35, '2021-11-17 16:56:08') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450239,   1,      32768) /* ItemType - Caster */
     , (450239,   3,          2) /* PaletteTemplate - Blue */
     , (450239,   5,          0) /* EncumbranceVal */
     , (450239,   9,   16777216) /* ValidLocations - Held */
     , (450239,  16,          1) /* ItemUseable - No */
     , (450239,  19,         20) /* Value */
     , (450239,  46,        512) /* DefaultCombatStyle - Magic */
     , (450239,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450239,  94,         16) /* TargetType - Creature */
     , (450239, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450239,  11, True ) /* IgnoreCollisions */
     , (450239,  13, True ) /* Ethereal */
     , (450239,  14, True ) /* GravityStatus */
     , (450239,  19, True ) /* Attackable */
     , (450239,  22, True ) /* Inscribable */
     , (450239,  84, True ) /* IgnoreCloIcons */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450239,  12,       0) /* Shade */
     , (450239,  29,       1) /* WeaponDefense */
     , (450239,  39,     1.5) /* DefaultScale */
     , (450239, 144,       0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450239,   1, 'Green Anniversary Sparkler') /* Name */
     , (450239,  16, 'A bright sparkler meant to help commemorate Festival season and the anniversary of Asheron''s Call.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450239,   1, 0x02001634) /* Setup */
     , (450239,   3, 0x20000014) /* SoundTable */
     , (450239,   6, 0x04000BEF) /* PaletteBase */
     , (450239,   7, 0x1000012E) /* ClothingBase */
     , (450239,   8, 0x0600653E) /* Icon */
     , (450239,  22, 0x3400002B) /* PhysicsEffectTable */;
