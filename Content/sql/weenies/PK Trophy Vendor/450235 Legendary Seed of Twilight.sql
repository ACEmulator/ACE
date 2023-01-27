DELETE FROM `weenie` WHERE `class_Id` = 450235;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450235, 'ace450235-legendaryseedoftwilighttailor', 35, '2022-05-17 03:47:03') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450235,   1,      32768) /* ItemType - Caster */
     , (450235,   3,          2) /* PaletteTemplate - Blue */
     , (450235,   5,         0) /* EncumbranceVal */
     , (450235,   9,   16777216) /* ValidLocations - Held */
     , (450235,  16,    6291464) /* ItemUseable - SourceContainedTargetRemoteNeverWalk */
     , (450235,  18,          1) /* UiEffects - Magical */
     , (450235,  19,      20) /* Value */
     , (450235,  46,        512) /* DefaultCombatStyle - Magic */
     , (450235,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450235,  94,         16) /* TargetType - Creature */
     , (450235, 151,          3) /* HookType - Floor, Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450235,  22, True ) /* Inscribable */
     , (450235,  23, True ) /* DestroyOnSell */
     , (450235,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450235,   5,  -0.025) /* ManaRate */
     , (450235,  29,     1.2) /* WeaponDefense */
     , (450235,  39,     0.6) /* DefaultScale */
     , (450235, 144,     0.2) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450235,   1, 'Legendary Seed of Twilight') /* Name */
     , (450235,  16, 'A large, glowing seed, empowered by the magics of the Light Falatacot.  This seed was retrieved from the Temple of Twilight, underneath the Inner Sea.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450235,   1, 0x02001BA5) /* Setup */
     , (450235,   3, 0x20000014) /* SoundTable */
     , (450235,   6, 0x04000BEF) /* PaletteBase */
     , (450235,   7, 0x10000841) /* ClothingBase */
     , (450235,   8, 0x060073F4) /* Icon */
     , (450235,  22, 0x3400002B) /* PhysicsEffectTable */;


