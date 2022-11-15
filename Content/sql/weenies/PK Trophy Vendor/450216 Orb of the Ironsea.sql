DELETE FROM `weenie` WHERE `class_Id` = 450216;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450216, 'wandrareorbironseatailor', 35, '2021-11-17 16:56:08') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450216,   1,      32768) /* ItemType - Caster */
     , (450216,   3,          4) /* PaletteTemplate - Brown */
     , (450216,   5,        0) /* EncumbranceVal */
     , (450216,   8,         90) /* Mass */
     , (450216,   9,   16777216) /* ValidLocations - Held */
     , (450216,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450216,  19,      20) /* Value */
     , (450216,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450216,  46,        512) /* DefaultCombatStyle - Magic */
     , (450216,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450216,  94,         16) /* TargetType - Creature */
     , (450216, 151,          2) /* HookType - Wall */
     , (450216, 353,          0) /* WeaponType - Undef */;



INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450216,  11, True ) /* IgnoreCollisions */
     , (450216,  13, True ) /* Ethereal */
     , (450216,  14, True ) /* GravityStatus */
     , (450216,  19, True ) /* Attackable */
     , (450216,  22, True ) /* Inscribable */
     , (450216, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450216,   5,  -0.033) /* ManaRate */
     , (450216,  12,    0.66) /* Shade */
     , (450216,  29,    1.0) /* WeaponDefense */
     , (450216,  39,       1) /* DefaultScale */
     , (450216, 138,    1.25) /* SlayerDamageBonus */
     , (450216, 144,     0.2) /* ManaConversionMod */
     , (450216, 147,    0.0) /* CriticalFrequency */
     , (450216, 152,    1.0) /* ElementalDamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450216,   1, 'Orb of the Ironsea') /* Name */
     , (450216,  16, 'Although this jewel looks solid, one has only to touch it to realize otherwise. The surface ripples like water when disturbed and yet somehow still manages to hold its spherical shape. Legend has it that this water comes from the deepest parts of the Ironsea and can only be retrieved by coaxing the denizens that live there to the surface. Such water is highly sought after by mages as it seems to help them cast their spells with more power and efficiency.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450216,   1, 0x02001380) /* Setup */
     , (450216,   3, 0x20000014) /* SoundTable */
     , (450216,   6, 0x04000BEF) /* PaletteBase */
     , (450216,   8, 0x06005C03) /* Icon */
     , (450216,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450216,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */
     , (450216,  28,       2132) /* Spell - The Spike */
     , (450216,  36, 0x0E000012) /* MutateFilter */
     , (450216,  46, 0x38000032) /* TsysMutationFilter */
     , (450216,  52, 0x06005B0C) /* IconUnderlay */;

