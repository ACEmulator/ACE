DELETE FROM `weenie` WHERE `class_Id` = 450214;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450214, 'wandrareeyemurammtailor', 35, '2021-11-17 16:56:08') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450214,   1,      32768) /* ItemType - Caster */
     , (450214,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450214,   5,        0) /* EncumbranceVal */
     , (450214,   8,         90) /* Mass */
     , (450214,   9,   16777216) /* ValidLocations - Held */
     , (450214,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450214,  18,         64) /* UiEffects - Lightning */
     , (450214,  19,      20) /* Value */
     , (450214,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450214,  46,        512) /* DefaultCombatStyle - Magic */
     , (450214,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450214,  94,         16) /* TargetType - Creature */
     , (450214, 151,          2) /* HookType - Wall */
     , (450214, 353,          0) /* WeaponType - Undef */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450214,  11, True ) /* IgnoreCollisions */
     , (450214,  13, True ) /* Ethereal */
     , (450214,  14, True ) /* GravityStatus */
     , (450214,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450214,   5,   -0.05) /* ManaRate */
     , (450214,  12,    0.66) /* Shade */
     , (450214,  29,    1.18) /* WeaponDefense */
     , (450214,  39,     1.2) /* DefaultScale */
     , (450214, 138,       2) /* SlayerDamageBonus */
     , (450214, 144,    0.18) /* ManaConversionMod */
     , (450214, 152,    1.25) /* ElementalDamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450214,   1, 'Eye of Muramm') /* Name */
     , (450214,  16, 'Muramm was a Falatacot witch of no small power who was murdered by her ungrateful disciple. Unhappy with his progress and jealous of Muramm''s power, the disciple murdered Muramm while she slept. Hoping to capture her powers, the disciple plucked an eye from his teacher''s lifeless body and placed it within the head of this scepter. Next he performed a powerful spell of healing on the eye to return it to life. He was successful and promptly died. Unfortunately for him, he did not fully understand the healing rites of the Falatacot and that to revive a life one must supply a life.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450214,   1, 0x0200137E) /* Setup */
     , (450214,   3, 0x20000014) /* SoundTable */
     , (450214,   6, 0x04000BEF) /* PaletteBase */
     , (450214,   8, 0x06005BFF) /* Icon */
     , (450214,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450214,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */
     , (450214,  28,       2142) /* Spell - Tempest */
     , (450214,  36, 0x0E000012) /* MutateFilter */
     , (450214,  46, 0x38000032) /* TsysMutationFilter */
     , (450214,  52, 0x06005B0C) /* IconUnderlay */;

