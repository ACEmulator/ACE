DELETE FROM `weenie` WHERE `class_Id` = 51965;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (51965, 'ace51965-rynthidtentacledagger', 6, '2021-01-03 07:35:26') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (51965,   1,          1) /* ItemType - MeleeWeapon */
     , (51965,   5,        135) /* EncumbranceVal */
     , (51965,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (51965,  16,          1) /* ItemUseable - No */
     , (51965,  18,          1) /* UiEffects - Magical */
     , (51965,  19,          0) /* Value */
     , (51965,  33,          1) /* Bonded - Bonded */
     , (51965,  44,         26) /* Damage */
     , (51965,  45,         16) /* DamageType - Fire */
     , (51965,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (51965,  47,        160) /* AttackType - DoubleSlash, DoubleThrust */
     , (51965,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (51965,  49,         20) /* WeaponTime */
     , (51965,  51,          1) /* CombatUse - Melee */
     , (51965,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (51965, 106,        475) /* ItemSpellcraft */
     , (51965, 107,       3000) /* ItemCurMana */
     , (51965, 108,       3000) /* ItemMaxMana */
     , (51965, 114,          1) /* Attuned - Attuned */
     , (51965, 151,          2) /* HookType - Wall */
     , (51965, 158,          2) /* WieldRequirements - RawSkill */
     , (51965, 159,         46) /* WieldSkillType - FinesseWeapons */
     , (51965, 160,        420) /* WieldDifficulty */
     , (51965, 166,         19) /* SlayerCreatureType - Virindi */
     , (51965, 353,          6) /* WeaponType - Dagger */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (51965,  11, True ) /* IgnoreCollisions */
     , (51965,  13, True ) /* Ethereal */
     , (51965,  14, True ) /* GravityStatus */
     , (51965,  19, True ) /* Attackable */
     , (51965,  22, True ) /* Inscribable */
     , (51965,  69, False) /* IsSellable */
     , (51965,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (51965,   5,  -0.033) /* ManaRate */
     , (51965,  21,       0) /* WeaponLength */
     , (51965,  22,    0.35) /* DamageVariance */
     , (51965,  26,       0) /* MaximumVelocity */
     , (51965,  29,    1.15) /* WeaponDefense */
     , (51965,  39,     0.9) /* DefaultScale */
     , (51965,  62,    1.25) /* WeaponOffense */
     , (51965,  63,       1) /* DamageMod */
     , (51965, 136,     2.5) /* CriticalMultiplier */
     , (51965, 138,       2) /* SlayerDamageBonus */
     , (51965, 147,    0.25) /* CriticalFrequency */
     , (51965, 155,       1) /* IgnoreArmor */
     , (51965, 52000,   0.7);

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (51965,   1, 'Rynthid Tentacle Dagger') /* Name */
     , (51965,  15, 'A dagger crafted from enchanted obsidian and Rynthid tentacles.') /* ShortDesc */
     , (51965,  33, 'TentacleWeaponPickup') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (51965,   1,   33561599) /* Setup */
     , (51965,   3,  536870932) /* SoundTable */
     , (51965,   6,   67111919) /* PaletteBase */
     , (51965,   8,  100693230) /* Icon */
     , (51965,  22,  872415275) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (51965,  3963,      0)  /* Epic Coordination */
     , (51965,  4019,      0)  /* Epic Quickness */
     , (51965,  4395,      0)  /* Aura of Incantation of Blood Drinker Self */
     , (51965,  4400,      0)  /* Aura of Incantation of Defender Self */
     , (51965,  4405,      0)  /* Aura of Incantation of Heart Seeker Self */
     , (51965,  4417,      0)  /* Aura of Incantation of Swift Killer Self */
     , (51965,  6047,      0)  /* Legendary Finesse Weapon Aptitude */;
