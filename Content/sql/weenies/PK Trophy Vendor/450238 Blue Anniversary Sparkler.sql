DELETE FROM `weenie` WHERE `class_Id` = 450238;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450238, 'ace450238-blueanniversarysparklertailor', 35, '2021-11-17 16:56:08') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450238,   1,      32768) /* ItemType - Caster */
     , (450238,   3,          2) /* PaletteTemplate - Blue */
     , (450238,   5,          0) /* EncumbranceVal */
     , (450238,   9,   16777216) /* ValidLocations - Held */
     , (450238,  16,          1) /* ItemUseable - No */
     , (450238,  19,         20) /* Value */
     , (450238,  46,        512) /* DefaultCombatStyle - Magic */
     , (450238,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450238,  94,         16) /* TargetType - Creature */
     , (450238, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450238,  11, True ) /* IgnoreCollisions */
     , (450238,  13, True ) /* Ethereal */
     , (450238,  14, True ) /* GravityStatus */
     , (450238,  19, True ) /* Attackable */
     , (450238,  22, True ) /* Inscribable */
     , (450238,  84, True ) /* IgnoreCloIcons */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450238,  12,       0) /* Shade */
     , (450238,  29,       1) /* WeaponDefense */
     , (450238,  39,     1.5) /* DefaultScale */
     , (450238, 144,       0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450238,   1, 'Blue Anniversary Sparkler') /* Name */
     , (450238,  16, 'A bright sparkler meant to help commemorate Festival season and the anniversary of Asheron''s Call.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450238,   1, 0x02001633) /* Setup */
     , (450238,   3, 0x20000014) /* SoundTable */
     , (450238,   6, 0x04000BEF) /* PaletteBase */
     , (450238,   7, 0x1000012E) /* ClothingBase */
     , (450238,   8, 0x0600653D) /* Icon */
     , (450238,  22, 0x3400002B) /* PhysicsEffectTable */;
