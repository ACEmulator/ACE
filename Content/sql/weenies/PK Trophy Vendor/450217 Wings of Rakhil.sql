DELETE FROM `weenie` WHERE `class_Id` = 450217;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450217, 'wandrarewingsrakhiltailor', 35, '2022-06-06 04:05:48') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450217,   1,      32768) /* ItemType - Caster */
     , (450217,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450217,   5,        0) /* EncumbranceVal */
     , (450217,   8,         90) /* Mass */
     , (450217,   9,   16777216) /* ValidLocations - Held */
     , (450217,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450217,  19,      20) /* Value */
     , (450217,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450217,  46,        512) /* DefaultCombatStyle - Magic */
     , (450217,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450217,  94,         16) /* TargetType - Creature */
     , (450217, 151,          2) /* HookType - Wall */
     , (450217, 353,          0) /* WeaponType - Undef */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450217,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450217,   5,  -0.033) /* ManaRate */
     , (450217,  12,    0.66) /* Shade */
     , (450217,  29,    1.0) /* WeaponDefense */
     , (450217,  39,     1.2) /* DefaultScale */
     , (450217, 136,       2) /* CriticalMultiplier */
     , (450217, 144,    0.0) /* ManaConversionMod */
     , (450217, 152,    1.0) /* ElementalDamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450217,   1, 'Wings of Rakhil') /* Name */
     , (450217,  16, 'Rakhil al-Kur was the First Malik, the founder of the nation of Gharu''n. He was the one who led the first wave of nomadic peoples in a campaign of conquest, sweeping the antiquated Roulean Empire out of Tirethas. A great mage of Tirethas who sought to curry favor with the Malik crafted this wand for him, which bears the mark of Rakhil''s chosen symbol, the eagle. Rakhil found the present pleasing enough, but he showed little patience for magical study and this wand soon ended up gathering dust in the royal storehouse of the al-Nafalt.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450217,   1, 0x02001381) /* Setup */
     , (450217,   3, 0x20000014) /* SoundTable */
     , (450217,   6, 0x04000BEF) /* PaletteBase */
     , (450217,   8, 0x06005C05) /* Icon */
     , (450217,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450217,  27, 0x400000E0) /* UseUserAnimation - UseMagicStaff */
     , (450217,  28,       2128) /* Spell - Ilservian's Flame */
     , (450217,  36, 0x0E000012) /* MutateFilter */
     , (450217,  46, 0x38000032) /* TsysMutationFilter */
     , (450217,  52, 0x06005B0C) /* IconUnderlay */;

