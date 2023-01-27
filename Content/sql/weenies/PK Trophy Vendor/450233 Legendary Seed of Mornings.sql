DELETE FROM `weenie` WHERE `class_Id` = 450233;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450233, 'ace450233-legendaryseedofmorningstailor', 35, '2022-05-17 03:47:03') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450233,   1,      32768) /* ItemType - Caster */
     , (450233,   5,         0) /* EncumbranceVal */
     , (450233,   9,   16777216) /* ValidLocations - Held */
     , (450233,  16,    6291464) /* ItemUseable - SourceContainedTargetRemoteNeverWalk */
     , (450233,  18,          1) /* UiEffects - Magical */
     , (450233,  19,      20) /* Value */
     , (450233,  46,        512) /* DefaultCombatStyle - Magic */
     , (450233,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450233,  94,         16) /* TargetType - Creature */
     , (450233, 151,          3) /* HookType - Floor, Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450233,  22, True ) /* Inscribable */
     , (450233,  23, True ) /* DestroyOnSell */
     , (450233,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450233,   5,  -0.025) /* ManaRate */
     , (450233,  29,     1.2) /* WeaponDefense */
     , (450233,  39,     0.6) /* DefaultScale */
     , (450233, 144,     0.2) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450233,   1, 'Legendary Seed of Mornings') /* Name */
     , (450233,  16, 'A large, glowing seed, empowered by the magics of the Light Falatacot.  This seed was retrieved from the Temple of Mornings, underneath the desert sands.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450233,   1, 0x02001BA5) /* Setup */
     , (450233,   3, 0x20000014) /* SoundTable */
     , (450233,   6, 0x04000BEF) /* PaletteBase */
     , (450233,   8, 0x060073EA) /* Icon */
     , (450233,  22, 0x3400002B) /* PhysicsEffectTable */

