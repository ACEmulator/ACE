DELETE FROM `weenie` WHERE `class_Id` = 450250;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450250, 'ace450250-orbofthebabybunnybootytailor', 35, '2021-11-01 00:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450250,   1,      32768) /* ItemType - Caster */
     , (450250,   3,         61) /* PaletteTemplate - White */
     , (450250,   5,         0) /* EncumbranceVal */
     , (450250,   9,   16777216) /* ValidLocations - Held */
     , (450250,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450250,  18,          1) /* UiEffects - Magical */
     , (450250,  19,      20) /* Value */
     , (450250,  33,          1) /* Bonded - Bonded */
     , (450250,  46,        512) /* DefaultCombatStyle - Magic */
     , (450250,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450250,  94,         16) /* TargetType - Creature */
     , (450250, 150,        103) /* HookPlacement - Hook */
     , (450250, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450250,  22, True ) /* Inscribable */
     , (450250,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450250,   5,   -0.02) /* ManaRate */
     , (450250,  29,       1) /* WeaponDefense */
     , (450250,  39,     0.5) /* DefaultScale */
     , (450250, 144,    0.0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450250,   1, 'Orb of the Baby Bunny Booty') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450250,   1, 0x02000986) /* Setup */
     , (450250,   3, 0x20000014) /* SoundTable */
     , (450250,   6, 0x040001B4) /* PaletteBase */
     , (450250,   7, 0x1000010D) /* ClothingBase */
     , (450250,   8, 0x060016BC) /* Icon */
     , (450250,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450250,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */;

