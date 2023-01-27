DELETE FROM `weenie` WHERE `class_Id` = 450251;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450251, 'ace450251-fingeroftheharbingertailor', 35, '2021-11-01 00:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450251,   1,      32768) /* ItemType - Caster */
     , (450251,   5,        0) /* EncumbranceVal */
     , (450251,   9,   16777216) /* ValidLocations - Held */
     , (450251,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450251,  18,          1) /* UiEffects - Magical */
     , (450251,  19,      20) /* Value */
     , (450251,  46,        512) /* DefaultCombatStyle - Magic */
     , (450251,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450251,  94,         16) /* TargetType - Creature */
     , (450251, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450251,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450251,   5,   -0.05) /* ManaRate */
     , (450251,  29,    1.08) /* WeaponDefense */
     , (450251,  39,     1.0) /* DefaultScale */
     , (450251, 144,     0.0) /* ManaConversionMod */
     , (450251, 147,    0.0) /* CriticalFrequency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450251,   1, 'Finger of the Harbinger') /* Name */
     , (450251,  16, 'The Harbinger''s casting finger.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450251,   1, 0x02001576) /* Setup */
     , (450251,   3, 0x20000014) /* SoundTable */
     , (450251,   8, 0x06006429) /* Icon */
     , (450251,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450251,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */
     , (450251,  37,         34) /* ItemSkillLimit - WarMagic */;

