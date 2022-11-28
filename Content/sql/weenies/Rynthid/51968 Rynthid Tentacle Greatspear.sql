DELETE FROM `weenie` WHERE `class_Id` = 51968;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (51968, 'ace51968-rynthidtentaclegreatspear', 6, '2020-10-24 22:12:55') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (51968,   1,          1) /* ItemType - MeleeWeapon */
     , (51968,   5,        700) /* EncumbranceVal */
     , (51968,   9,   33554432) /* ValidLocations - TwoHanded */
     , (51968,  16,          1) /* ItemUseable - No */
     , (51968,  18,          1) /* UiEffects - Magical */
     , (51968,  19,          0) /* Value */
     , (51968,  33,          1) /* Bonded - Bonded */
     , (51968,  44,         65) /* Damage */
     , (51968,  45,         16) /* DamageType - Fire */
     , (51968,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (51968,  47,          2) /* AttackType - Thrust */
     , (51968,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (51968,  49,         40) /* WeaponTime */
     , (51968,  51,          5) /* CombatUse - TwoHanded */
     , (51968,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (51968, 106,        475) /* ItemSpellcraft */
     , (51968, 107,       2726) /* ItemCurMana */
     , (51968, 108,       3000) /* ItemMaxMana */
     , (51968, 114,          1) /* Attuned - Attuned */
     , (51968, 151,          2) /* HookType - Wall */
     , (51968, 158,          2) /* WieldRequirements - RawSkill */
     , (51968, 159,         41) /* WieldSkillType - TwoHandedCombat */
     , (51968, 160,        420) /* WieldDifficulty */
     , (51968, 166,         19) /* SlayerCreatureType - Virindi */
     , (51968, 353,         11) /* WeaponType - TwoHanded */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (51968,  11, True ) /* IgnoreCollisions */
     , (51968,  13, True ) /* Ethereal */
     , (51968,  14, True ) /* GravityStatus */
     , (51968,  19, True ) /* Attackable */
     , (51968,  22, True ) /* Inscribable */
     , (51968,  69, False) /* IsSellable */
     , (51968,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (51968,   5,  -0.033) /* ManaRate */
     , (51968,  21,       0) /* WeaponLength */
     , (51968,  22,     0.4) /* DamageVariance */
     , (51968,  26,       0) /* MaximumVelocity */
     , (51968,  29,    1.15) /* WeaponDefense */
     , (51968,  39,     1.2) /* DefaultScale */
     , (51968,  62,     1.2) /* WeaponOffense */
     , (51968,  63,       1) /* DamageMod */
     , (51968, 136,     2.5) /* CriticalMultiplier */
     , (51968, 138,       2) /* SlayerDamageBonus */
     , (51968, 147,    0.25) /* CriticalFrequency */
     , (51968, 155,       1) /* IgnoreArmor */
     , (51968, 52000,   0.7);

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (51968,   1, 'Rynthid Tentacle Greatspear') /* Name */
     , (51968,  15, 'A two handed spear crafted from enchanted obsidian and Rynthid tentacles.') /* ShortDesc */
     , (51968,  33, 'TentacleWeaponPickup') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (51968,   1,   33561604) /* Setup */
     , (51968,   3,  536870932) /* SoundTable */
     , (51968,   6,   67111919) /* PaletteBase */
     , (51968,   8,  100693233) /* Icon */
     , (51968,  22,  872415275) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (51968,  3963,      2)  /* Epic Coordination */
     , (51968,  3965,      2)  /* Epic Strength */
     , (51968,  4395,      2)  /* Aura of Incantation of Blood Drinker Self */
     , (51968,  4400,      2)  /* Aura of Incantation of Defender Self */
     , (51968,  4405,      2)  /* Aura of Incantation of Heart Seeker Self */
     , (51968,  4417,      2)  /* Aura of Incantation of Swift Killer Self */
     , (51968,  6073,      2)  /* Legendary Two Handed Combat Aptitude */;
