DELETE FROM `weenie` WHERE `class_Id` = 51967;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (51967, 'ace51967-rynthidtentaclespear', 6, '2020-08-12 18:18:17') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (51967,   1,          1) /* ItemType - MeleeWeapon */
     , (51967,   5,        700) /* EncumbranceVal */
     , (51967,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (51967,  16,          1) /* ItemUseable - No */
     , (51967,  18,          1) /* UiEffects - Magical */
     , (51967,  19,      10000) /* Value */
     , (51967,  33,          1) /* Bonded - Bonded */
     , (51967,  44,         66) /* Damage */
     , (51967,  45,         16) /* DamageType - Fire */
     , (51967,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (51967,  47,          2) /* AttackType - Thrust */
     , (51967,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (51967,  49,         30) /* WeaponTime */
     , (51967,  51,          1) /* CombatUse - Melee */
     , (51967,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (51967, 106,        475) /* ItemSpellcraft */
     , (51967, 107,       2974) /* ItemCurMana */
     , (51967, 108,       3000) /* ItemMaxMana */
     , (51967, 114,          1) /* Attuned - Attuned */
     , (51967, 151,          2) /* HookType - Wall */
     , (51967, 158,          2) /* WieldRequirements - RawSkill */
     , (51967, 159,         44) /* WieldSkillType - HeavyWeapons */
     , (51967, 160,        420) /* WieldDifficulty */
     , (51967, 166,         19) /* SlayerCreatureType - Virindi */
     , (51967, 353,          5) /* WeaponType - Spear */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (51967,  11, True ) /* IgnoreCollisions */
     , (51967,  13, True ) /* Ethereal */
     , (51967,  14, True ) /* GravityStatus */
     , (51967,  19, True ) /* Attackable */
     , (51967,  22, True ) /* Inscribable */
     , (51967,  69, False) /* IsSellable */
     , (51967,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (51967,   1,       5) /* HeartbeatInterval */
     , (51967,   5,  -0.033) /* ManaRate */
     , (51967,  21,     1.5) /* WeaponLength */
     , (51967,  22,     0.5) /* DamageVariance */
     , (51967,  29,    1.15) /* WeaponDefense */
     , (51967,  62,    1.25) /* WeaponOffense */
     , (51967,  63,       1) /* DamageMod */
     , (51967, 136,     2.5) /* CriticalMultiplier */
     , (51967, 138,       2) /* SlayerDamageBonus */
     , (51967, 147,    0.25) /* CriticalFrequency */
     , (51967, 155,       1) /* IgnoreArmor */
     , (51967, 52000,       0.7) ;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (51967,   1, 'Rynthid Tentacle Spear') /* Name */
     , (51967,  16, 'A one handed spear crafted from enchanted obsidian and Rynthid tentacles.') /* LongDesc */
     , (51967,  33, 'TentacleWeaponPickup') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (51967,   1,   33561600) /* Setup */
     , (51967,   3,  536870932) /* SoundTable */
     , (51967,   6,   67111919) /* PaletteBase */
     , (51967,   8,  100693232) /* Icon */
     , (51967,  22,  872415275) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (51967,  3963,      2)  /* Epic Coordination */
     , (51967,  3965,      2)  /* Epic Strength */
     , (51967,  4395,      2)  /* Aura of Incantation of Blood Drinker Self */
     , (51967,  4400,      2)  /* Aura of Incantation of Defender Self */
     , (51967,  4405,      2)  /* Aura of Incantation of Heart Seeker Self */
     , (51967,  4417,      2)  /* Aura of Incantation of Swift Killer Self */
     , (51967,  6072,      2)  /* Legendary Heavy Weapon Aptitude */;
