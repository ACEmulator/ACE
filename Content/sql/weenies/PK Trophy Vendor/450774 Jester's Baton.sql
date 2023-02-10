DELETE FROM `weenie` WHERE `class_Id` = 450774;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450774, 'ace450774-jestersbatonpk', 35, '2022-12-28 05:57:21') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450774,   1,      32768) /* ItemType - Caster */
     , (450774,   5,        0) /* EncumbranceVal */
     , (450774,   9,   16777216) /* ValidLocations - Held */
     , (450774,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450774,  18,         32) /* UiEffects - Fire */
     , (450774,  19,          20) /* Value */
     , (450774,  46,        512) /* DefaultCombatStyle - Magic */
     , (450774,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450774,  94,         16) /* TargetType - Creature */
     , (450774, 106,        400) /* ItemSpellcraft */
     , (450774, 107,      10000) /* ItemCurMana */
     , (450774, 108,      10000) /* ItemMaxMana */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450774,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450774,  39,     0.8) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450774,   1, 'Jester''s Baton') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450774,   1, 0x020017D9) /* Setup */
     , (450774,   3, 0x20000014) /* SoundTable */
     , (450774,   6, 0x04000BEF) /* PaletteBase */
     , (450774,   8, 0x060067B3) /* Icon */
     , (450774,  22, 0x3400002B) /* PhysicsEffectTable */;
