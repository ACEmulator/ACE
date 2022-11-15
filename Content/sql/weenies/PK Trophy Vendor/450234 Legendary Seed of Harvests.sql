DELETE FROM `weenie` WHERE `class_Id` = 450234;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450234, 'ace450234-legendaryseedofharveststailor', 35, '2022-06-06 04:05:48') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450234,   1,      32768) /* ItemType - Caster */
     , (450234,   3,         14) /* PaletteTemplate - Red */
     , (450234,   5,         0) /* EncumbranceVal */
     , (450234,   9,   16777216) /* ValidLocations - Held */
     , (450234,  16,    6291464) /* ItemUseable - SourceContainedTargetRemoteNeverWalk */
     , (450234,  18,          1) /* UiEffects - Magical */
     , (450234,  19,      20) /* Value */
     , (450234,  46,        512) /* DefaultCombatStyle - Magic */
     , (450234,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450234,  94,         16) /* TargetType - Creature */
     , (450234, 151,          3) /* HookType - Floor, Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450234,  22, True ) /* Inscribable */
     , (450234,  23, True ) /* DestroyOnSell */
     , (450234,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450234,   5,  -0.025) /* ManaRate */
     , (450234,  29,    1.15) /* WeaponDefense */
     , (450234,  39,     0.6) /* DefaultScale */
     , (450234, 144,    0.15) /* ManaConversionMod */
     , (450234, 157,       1) /* ResistanceModifier */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450234,   1, 'Legendary Seed of Harvests') /* Name */
     , (450234,  16, 'A large, glowing seed, empowered by the magics of the Light Falatacot.  This seed was retrieved from the Temple of Harvests, underneath the Valley of Death.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450234,   1, 0x02001BA5) /* Setup */
     , (450234,   3, 0x20000014) /* SoundTable */
     , (450234,   6, 0x04000BEF) /* PaletteBase */
     , (450234,   7, 0x1000083F) /* ClothingBase */
     , (450234,   8, 0x060073EF) /* Icon */
     , (450234,  22, 0x3400002B) /* PhysicsEffectTable */;

