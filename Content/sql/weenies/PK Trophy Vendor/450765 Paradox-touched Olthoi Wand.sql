DELETE FROM `weenie` WHERE `class_Id` = 450765;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450765, 'ace450765-paradoxtouchedolthoiwandtailor', 35, '2021-11-01 00:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450765,   1,      32768) /* ItemType - Caster */
     , (450765,   5,        0) /* EncumbranceVal */
     , (450765,   9,   16777216) /* ValidLocations - Held */
     , (450765,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450765,  18,          1) /* UiEffects - Magical */
     , (450765,  19,      20) /* Value */
     , (450765,  46,        512) /* DefaultCombatStyle - Magic */
     , (450765,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450765,  94,         16) /* TargetType - Creature */
     , (450765, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450765,  22, True ) /* Inscribable */
     , (450765,  69, False) /* IsSellable */
     , (450765,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450765,   5,  -0.033) /* ManaRate */
     , (450765,  29,    1.15) /* WeaponDefense */
     , (450765,  39,     1.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450765,   1, 'Paradox-touched Olthoi Wand') /* Name */
     , (450765,  16, 'A wand, crafted from the remains of the stronger Paradox-touched Olthoi.  Something about the nature of these creatures makes the weapon naturally deadlier versus normal Olthoi.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450765,   1, 0x020019F9) /* Setup */
     , (450765,   3, 0x20000014) /* SoundTable */
     , (450765,   8, 0x06006D93) /* Icon */
     , (450765,  22, 0x3400002B) /* PhysicsEffectTable */;

