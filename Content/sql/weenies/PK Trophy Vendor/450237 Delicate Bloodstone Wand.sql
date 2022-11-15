DELETE FROM `weenie` WHERE `class_Id` = 450237;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450237, 'ace450237-delicatebloodstonewandtailor', 35, '2021-11-01 00:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450237,   1,      32768) /* ItemType - Caster */
     , (450237,   3,          8) /* PaletteTemplate - Green */
     , (450237,   5,        0) /* EncumbranceVal */
     , (450237,   9,   16777216) /* ValidLocations - Held */
     , (450237,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450237,  18,          1) /* UiEffects - Magical */
     , (450237,  19,      20) /* Value */
     , (450237,  46,        512) /* DefaultCombatStyle - Magic */
     , (450237,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450237,  94,         16) /* TargetType - Creature */
     , (450237, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450237,  22, True ) /* Inscribable */
     , (450237,  69, False) /* IsSellable */
     , (450237,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450237,   5,   -0.03) /* ManaRate */
     , (450237,  29,    1.15) /* WeaponDefense */
     , (450237, 144,     0.2) /* ManaConversionMod */
     , (450237, 147,    0.06) /* CriticalFrequency */
     , (450237, 157,       1) /* ResistanceModifier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450237,   1, 'Delicate Bloodstone Wand') /* Name */
     , (450237,  16, 'A wand, crafted from the delicate remains of the shattered Master Bloodstone.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450237,   1, 0x02001A4A) /* Setup */
     , (450237,   3, 0x20000014) /* SoundTable */
     , (450237,   6, 0x04000BEF) /* PaletteBase */
     , (450237,   7, 0x10000839) /* ClothingBase */
     , (450237,   8, 0x060025E3) /* Icon */
     , (450237,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450237,  28,       5531) /* Spell - Bloodstone Bolt VII */;


