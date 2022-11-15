DELETE FROM `weenie` WHERE `class_Id` = 450215;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450215, 'wandrarefrorecrystaltailor', 35, '2021-11-17 16:56:08') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450215,   1,      32768) /* ItemType - Caster */
     , (450215,   3,          4) /* PaletteTemplate - Brown */
     , (450215,   5,        0) /* EncumbranceVal */
     , (450215,   8,         90) /* Mass */
     , (450215,   9,   16777216) /* ValidLocations - Held */
     , (450215,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450215,  18,        128) /* UiEffects - Frost */
     , (450215,  19,      20) /* Value */
     , (450215,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450215,  46,        512) /* DefaultCombatStyle - Magic */
     , (450215,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450215,  94,         16) /* TargetType - Creature */
     , (450215, 150,        103) /* HookPlacement - Hook */
     , (450215, 151,          2) /* HookType - Wall */
     , (450215, 353,          0) /* WeaponType - Undef */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450215,  11, True ) /* IgnoreCollisions */
     , (450215,  13, True ) /* Ethereal */
     , (450215,  14, True ) /* GravityStatus */
     , (450215,  19, True ) /* Attackable */
     , (450215,  22, True ) /* Inscribable */
     , (450215, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450215,   5,  -0.033) /* ManaRate */
     , (450215,  12,    0.66) /* Shade */
     , (450215,  29,    1.0) /* WeaponDefense */
     , (450215,  39,       1) /* DefaultScale */
     , (450215, 144,    0.0) /* ManaConversionMod */
     , (450215, 147,    0.0) /* CriticalFrequency */
     , (450215, 152,    1.0) /* ElementalDamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450215,   1, 'Wand of the Frore Crystal') /* Name */
     , (450215,  16, 'A mage who wandered through the northern mountains, researching ley lines, found a piece of unnaturally cold crystal in the middle of a set of standing stones. He affixed the crystal to his wand, and found that the wand suddenly became unbearably cold to touch. Rather than drop the wand, he held on for dear life. He finally mastered the wand well enough to wield it, but not before it had frozen his hand so thoroughly as to render it permanently useless.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450215,   1, 0x0200137F) /* Setup */
     , (450215,   3, 0x20000014) /* SoundTable */
     , (450215,   6, 0x04000BEF) /* PaletteBase */
     , (450215,   8, 0x06005C01) /* Icon */
     , (450215,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450215,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */
     , (450215,  28,       2136) /* Spell - Icy Torment */
     , (450215,  36, 0x0E000012) /* MutateFilter */
     , (450215,  46, 0x38000032) /* TsysMutationFilter */
     , (450215,  52, 0x06005B0C) /* IconUnderlay */;

