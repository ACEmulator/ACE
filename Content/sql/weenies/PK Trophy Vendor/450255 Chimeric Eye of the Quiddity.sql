DELETE FROM `weenie` WHERE `class_Id` = 450255;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450255, 'ace450255-chimericeyeofthequiddity', 35, '2022-06-06 04:05:48') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450255,   1,      32768) /* ItemType - Caster */
     , (450255,   3,         82) /* PaletteTemplate - PinkPurple */
     , (450255,   5,         0) /* EncumbranceVal */
     , (450255,   9,   16777216) /* ValidLocations - Held */
     , (450255,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450255,  18,          1) /* UiEffects - Magical */
     , (450255,  19,          20) /* Value */
     , (450255,  46,        512) /* DefaultCombatStyle - Magic */
     , (450255,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450255,  94,         16) /* TargetType - Creature */
     , (450255, 353,          0) /* WeaponType - Undef */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450255,  22, True ) /* Inscribable */
     , (450255,  69, False) /* IsSellable */
     , (450255,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450255,   5,   -0.05) /* ManaRate */
     , (450255,  29,     1.2) /* WeaponDefense */
     , (450255,  39,     0.8) /* DefaultScale */
     , (450255, 136,     1.7) /* CriticalMultiplier */
     , (450255, 144,    0.0) /* ManaConversionMod */
     , (450255, 147,     0.0) /* CriticalFrequency */
     , (450255, 152,     1.0) /* ElementalDamageMod */
     , (450255, 157,       0) /* ResistanceModifier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450255,   1, 'Chimeric Eye of the Quiddity') /* Name */
     , (450255,  16, 'A powerful but unstable weapon made from congealed Portal Energy, pulled from a rift into Portalspace itself.  The origin of these weapons is unknown, and they do not survive exposure to Dereth for more than a few hours.  (This weapon has a 3 hour duration from the time of its creation.)') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450255,   1, 0x02001184) /* Setup */
     , (450255,   3, 0x20000014) /* SoundTable */
     , (450255,   6, 0x04000BEF) /* PaletteBase */
     , (450255,   7, 0x100002E7) /* ClothingBase */
     , (450255,   8, 0x060035C7) /* Icon */
     , (450255,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450255,  52, 0x060065FB) /* IconUnderlay */;

