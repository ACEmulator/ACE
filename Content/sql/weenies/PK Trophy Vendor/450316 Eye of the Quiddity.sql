DELETE FROM `weenie` WHERE `class_Id` = 450316;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450316, 'loorbquidditytailor', 35, '2021-11-17 16:56:08') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450316,   1,      32768) /* ItemType - Caster */
     , (450316,   5,         0) /* EncumbranceVal */
     , (450316,   8,         50) /* Mass */
     , (450316,   9,   16777216) /* ValidLocations - Held */
     , (450316,  16,    6291464) /* ItemUseable - SourceContainedTargetRemoteNeverWalk */
     , (450316,  18,          1) /* UiEffects - Magical */
     , (450316,  19,       20) /* Value */
     , (450316,  46,        512) /* DefaultCombatStyle - Magic */
     , (450316,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450316,  94,         16) /* TargetType - Creature */
     , (450316, 150,        103) /* HookPlacement - Hook */
     , (450316, 151,          6) /* HookType - Wall, Ceiling */
     , (450316, 353,          0) /* WeaponType - Undef */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450316,  11, True ) /* IgnoreCollisions */
     , (450316,  13, True ) /* Ethereal */
     , (450316,  14, True ) /* GravityStatus */
     , (450316,  15, True ) /* LightsStatus */
     , (450316,  19, True ) /* Attackable */
     , (450316,  22, True ) /* Inscribable */
     , (450316,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450316,   5,  -0.025) /* ManaRate */
     , (450316,  29,       1) /* WeaponDefense */
     , (450316,  39,     0.8) /* DefaultScale */
     , (450316, 144,       0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450316,   1, 'Eye of the Quiddity') /* Name */
     , (450316,  15, 'An orb with an eye encased within.') /* ShortDesc */
     , (450316,  16, 'An orb made of a strange pulsating energy. Gazing at it makes you dizzy') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450316,   1, 0x02000A7B) /* Setup */
     , (450316,   3, 0x20000014) /* SoundTable */
     , (450316,   8, 0x060020CC) /* Icon */
     , (450316,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450316,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */
     , (450316,  36, 0x0E000016) /* MutateFilter */
     , (450316,  37,         16) /* ItemSkillLimit - ManaConversion */;


