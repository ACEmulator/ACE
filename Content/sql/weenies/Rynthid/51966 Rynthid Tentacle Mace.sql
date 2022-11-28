DELETE FROM `weenie` WHERE `class_Id` = 51966;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (51966, 'ace51966-rynthidtentaclemace', 6, '2020-10-24 22:12:55') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (51966,   1,          1) /* ItemType - MeleeWeapon */
     , (51966,   5,        700) /* EncumbranceVal */
     , (51966,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (51966,  16,          1) /* ItemUseable - No */
     , (51966,  18,          1) /* UiEffects - Magical */
     , (51966,  19,          0) /* Value */
     , (51966,  33,          1) /* Bonded - Bonded */
     , (51966,  44,         50) /* Damage */
     , (51966,  45,         16) /* DamageType - Fire */
     , (51966,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (51966,  47,          4) /* AttackType - Slash */
     , (51966,  48,         45) /* WeaponSkill - LightWeapons */
     , (51966,  49,         40) /* WeaponTime */
     , (51966,  51,          1) /* CombatUse - Melee */
     , (51966,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (51966, 106,        475) /* ItemSpellcraft */
     , (51966, 107,       3000) /* ItemCurMana */
     , (51966, 108,       3000) /* ItemMaxMana */
     , (51966, 114,          1) /* Attuned - Attuned */
     , (51966, 151,          2) /* HookType - Wall */
     , (51966, 158,          2) /* WieldRequirements - RawSkill */
     , (51966, 159,         45) /* WieldSkillType - LightWeapons */
     , (51966, 160,        420) /* WieldDifficulty */
     , (51966, 166,         19) /* SlayerCreatureType - Virindi */
     , (51966, 353,          4) /* WeaponType - Mace */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (51966,  11, True ) /* IgnoreCollisions */
     , (51966,  13, True ) /* Ethereal */
     , (51966,  14, True ) /* GravityStatus */
     , (51966,  19, True ) /* Attackable */
     , (51966,  22, True ) /* Inscribable */
     , (51966,  69, False) /* IsSellable */
     , (51966,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (51966,   5,  -0.033) /* ManaRate */
     , (51966,  21,       0) /* WeaponLength */
     , (51966,  22,     0.4) /* DamageVariance */
     , (51966,  26,       0) /* MaximumVelocity */
     , (51966,  29,    1.15) /* WeaponDefense */
     , (51966,  62,    1.25) /* WeaponOffense */
     , (51966,  63,       1) /* DamageMod */
     , (51966, 136,     2.5) /* CriticalMultiplier */
     , (51966, 138,       2) /* SlayerDamageBonus */
     , (51966, 147,    0.25) /* CriticalFrequency */
     , (51966, 155,       1) /* IgnoreArmor */
     , (51966, 52000,       0.7) ;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (51966,   1, 'Rynthid Tentacle Mace') /* Name */
     , (51966,  15, 'A mace crafted from enchanted obsidian and Rynthid tentacles.') /* ShortDesc */
     , (51966,  33, 'TentacleWeaponPickup') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (51966,   1,   33561602) /* Setup */
     , (51966,   3,  536870932) /* SoundTable */
     , (51966,   6,   67111919) /* PaletteBase */
     , (51966,   8,  100693231) /* Icon */
     , (51966,  22,  872415275) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (51966,  3963,      2)  /* Epic Coordination */
     , (51966,  3965,      2)  /* Epic Strength */
     , (51966,  4395,      2)  /* Aura of Incantation of Blood Drinker Self */
     , (51966,  4400,      2)  /* Aura of Incantation of Defender Self */
     , (51966,  4405,      2)  /* Aura of Incantation of Heart Seeker Self */
     , (51966,  4417,      2)  /* Aura of Incantation of Swift Killer Self */
     , (51966,  6043,      2)  /* Legendary Light Weapon Aptitude */;
