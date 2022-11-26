DELETE FROM `weenie` WHERE `class_Id` = 450754;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450754, 'orbasteliarytailor', 35, '2021-11-01 00:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450754,   1,      32768) /* ItemType - Caster */
     , (450754,   5,        0) /* EncumbranceVal */
     , (450754,   8,         50) /* Mass */
     , (450754,   9,   16777216) /* ValidLocations - Held */
     , (450754,  16,    6291464) /* ItemUseable - SourceContainedTargetRemoteNeverWalk */
     , (450754,  18,          1) /* UiEffects - Magical */
     , (450754,  19,       20) /* Value */
     , (450754,  46,        512) /* DefaultCombatStyle - Magic */
     , (450754,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450754,  94,         16) /* TargetType - Creature */
     , (450754, 150,        103) /* HookPlacement - Hook */
     , (450754, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450754,  22, True ) /* Inscribable */
     , (450754,  23, True ) /* DestroyOnSell */
     , (450754,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450754,   5,  -0.033) /* ManaRate */
     , (450754,  29,       1) /* WeaponDefense */
     , (450754, 144,    0.05) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450754,   1, 'Asteliary Orb') /* Name */
     , (450754,  16, 'An orb enchanted with powerful magic, taken from the Asteliary dungeon.') /* LongDesc */
     , (450754,  33, 'OrbAsteliary') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450754,   1, 0x02000B69) /* Setup */
     , (450754,   3, 0x20000014) /* SoundTable */
     , (450754,   8, 0x0600228A) /* Icon */
     , (450754,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450754,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */
     , (450754,  37,         16) /* ItemSkillLimit - ManaConversion */;


