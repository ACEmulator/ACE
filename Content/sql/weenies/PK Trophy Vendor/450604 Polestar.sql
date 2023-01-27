DELETE FROM `weenie` WHERE `class_Id` = 450604;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450604, 'staffdaintailor', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450604,   1,      32768) /* ItemType - Caster */
     , (450604,   5,        0) /* EncumbranceVal */
     , (450604,   8,         25) /* Mass */
     , (450604,   9,   16777216) /* ValidLocations - Held */
     , (450604,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450604,  18,          1) /* UiEffects - Magical */
     , (450604,  19,      20) /* Value */
     , (450604,  46,        512) /* DefaultCombatStyle - Magic */
     , (450604,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450604,  94,         16) /* TargetType - Creature */
     , (450604, 150,        103) /* HookPlacement - Hook */
     , (450604, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450604,  15, True ) /* LightsStatus */
     , (450604,  22, True ) /* Inscribable */
     , (450604,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450604,   5,  -0.033) /* ManaRate */
     , (450604,  29,       1) /* WeaponDefense */
     , (450604, 144,     0.1) /* ManaConversionMod */
     , (450604, 147,    0.15) /* CriticalFrequency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450604,   1, 'Polestar') /* Name */
     , (450604,   7, 'May this staff guide you to your own truths.') /* Inscription */
     , (450604,   8, 'Lady Dain') /* ScribeName */
     , (450604,  16, 'The staff almost guides your hand towards your targets.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450604,   1, 0x02000FE4) /* Setup */
     , (450604,   3, 0x20000014) /* SoundTable */
     , (450604,   6, 0x04000BEF) /* PaletteBase */
     , (450604,   8, 0x06002DE6) /* Icon */
     , (450604,  22, 0x3400002B) /* PhysicsEffectTable */;


