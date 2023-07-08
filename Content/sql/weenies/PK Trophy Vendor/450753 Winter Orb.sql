DELETE FROM `weenie` WHERE `class_Id` = 450753;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450753, 'ace450753-winterorbtailor', 35, '2021-11-17 16:56:08') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450753,   1,      32768) /* ItemType - Caster */
     , (450753,   5,         0) /* EncumbranceVal */
     , (450753,   9,   16777216) /* ValidLocations - Held */
     , (450753,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450753,  18,          1) /* UiEffects - Magical */
     , (450753,  19,       20) /* Value */
     , (450753,  46,        512) /* DefaultCombatStyle - Magic */
     , (450753,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450753,  94,         16) /* TargetType - Creature */
     , (450753, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450753,  11, True ) /* IgnoreCollisions */
     , (450753,  13, True ) /* Ethereal */
     , (450753,  14, True ) /* GravityStatus */
     , (450753,  15, True ) /* LightsStatus */
     , (450753,  19, True ) /* Attackable */
     , (450753,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450753,   5,  -0.033) /* ManaRate */
     , (450753,  29,    1.08) /* WeaponDefense */
     , (450753, 144,    0.05) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450753,   1, 'Winter Orb') /* Name */
     , (450753,  16, 'A frozen orb containing a swirling snow storm. A beautiful light seems to shine in the depths of the storm.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450753,   1, 0x02001503) /* Setup */
     , (450753,   3, 0x20000014) /* SoundTable */
     , (450753,   6, 0x04000BEF) /* PaletteBase */
     , (450753,   8, 0x060062BF) /* Icon */
     , (450753,  22, 0x3400002B) /* PhysicsEffectTable */;

