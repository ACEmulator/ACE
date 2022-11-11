DELETE FROM `weenie` WHERE `class_Id` = 42142662;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (42142662, 'bloodbathedmace', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (42142662,   1,          1) /* ItemType - MeleeWeapon */
     , (42142662,   5,        675) /* EncumbranceVal */
     , (42142662,   8,         90) /* Mass */
     , (42142662,   9,   33554432) /* ValidLocations - TwoHanded */
     , (42142662,  16,          1) /* ItemUseable - No */
     , (42142662,  19,      50000) /* Value */
     , (42142662,  44,         85) /* Damage */
     , (42142662,  45,          4) /* DamageType - Bludgeon */
     , (42142662,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (42142662,  47,          4) /* AttackType - Slash */
     , (42142662,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (42142662,  49,         50) /* WeaponTime */
     , (42142662,  51,          5) /* CombatUse - TwoHanded */
     , (42142662,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (42142662, 106,        350) /* ItemSpellcraft */
     , (42142662, 107,       2000) /* ItemCurMana */
     , (42142662, 108,       2000) /* ItemMaxMana */
     , (42142662, 109,          0) /* ItemDifficulty */
     , (42142662, 151,          2) /* HookType - Wall */
     , (42142662, 166,         31) /* SlayerCreatureType - Human */
     , (42142662, 179,         32) /* ImbuedEffect - BludgeonRending */
     , (42142662, 292,          5) /* Cleaving */
     , (42142662, 114,         -2) /* destroy on death */
     , (42142662, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (42142662,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (42142662,   5,  -0.033) /* ManaRate */
     , (42142662,  21,       1) /* WeaponLength */
     , (42142662,  22,    0.19) /* DamageVariance */
     , (42142662,  26,       0) /* MaximumVelocity */
     , (42142662,  29,    1.18) /* WeaponDefense */
     , (42142662,  39,       3) /* DefaultScale */
     , (42142662,  62,    1.18) /* WeaponOffense */
     , (42142662,  63,       1) /* DamageMod */
     , (42142662, 138,     3.4) /* SlayerDamageBonus */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (42142662,   1, 'Blood Bathed Mace') /* Name */
     , (42142662,  16, 'Battle bathed this mace in the blood of many Isparian; its head specially designed to crush the skulls of anyone who contests it.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (42142662,   1, 0x02001A38) /* Setup */
     , (42142662,   3, 0x20000014) /* SoundTable */
     , (42142662,   6, 0x04000BEF) /* PaletteBase */
     , (42142662,   7, 0x10000860) /* ClothingBase */
     , (42142662,   8, 0x06006F34) /* Icon */
     , (42142662,  22, 0x3400002B) /* PhysicsEffectTable */
     , (42142662,  36, 0x0E000012) /* MutateFilter */
     , (42142662,  46, 0x38000032) /* TsysMutationFilter */
     , (42142662,  52, 0x06005B0C) /* IconUnderlay */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (42142662,  4400,      2)  /* Aura of Incantation of Defender Self */
     , (42142662,  4596,      2)  /* Incantation of Magic Resistance Self */
     , (42142662,  6089,      2)  /* Legendary Blood Thirst */
     , (42142662,  5032,      2)  /* Incantation of Two Handed Combat Mastery Self */
     , (42142662,  6073,      2)  /* Legendary Two Handed Combat Aptitude */;
