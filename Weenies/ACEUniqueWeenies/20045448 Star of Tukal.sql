DELETE FROM `weenie` WHERE `class_Id` = 20045448;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (20045448, 'ace20045448-staroftukal', 6, '2023-06-05 00:31:03') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (20045448,   1,          1) /* ItemType - MeleeWeapon */
     , (20045448,   5,        850) /* EncumbranceVal */
     , (20045448,   8,         90) /* Mass */
     , (20045448,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (20045448,  16,          1) /* ItemUseable - No */
     , (20045448,  19,       5000) /* Value */
     , (20045448,  44,         56) /* Damage */
     , (20045448,  45,          4) /* DamageType - Bludgeon */
     , (20045448,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (20045448,  47,          4) /* AttackType - Slash */
     , (20045448,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (20045448,  49,         50) /* WeaponTime */
     , (20045448,  51,          1) /* CombatUse - Melee */
     , (20045448,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (20045448, 106,        480) /* ItemSpellcraft */
     , (20045448, 107,       2500) /* ItemCurMana */
     , (20045448, 108,       2500) /* ItemMaxMana */
     , (20045448, 109,          0) /* ItemDifficulty */
     , (20045448, 124,          2) /* Version */
     , (20045448, 151,          2) /* HookType - Wall */
     , (20045448, 166,         31) /* SlayerCreatureType - Human */
     , (20045448, 179,          1) /* ImbuedEffect - CriticalStrike */
     , (20045448, 263,          4) /* ResistanceModifierType - Bludgeon */
     , (20045448, 353,          4) /* WeaponType - Mace */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (20045448,   4,          0) /* ItemTotalXp */
     , (20045448,   5, 2000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (20045448,  11, True ) /* IgnoreCollisions */
     , (20045448,  13, True ) /* Ethereal */
     , (20045448,  14, True ) /* GravityStatus */
     , (20045448,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (20045448,   5,       0) /* ManaRate */
     , (20045448,  21,       1) /* WeaponLength */
     , (20045448,  22,     0.4) /* DamageVariance */
     , (20045448,  26,       0) /* MaximumVelocity */
     , (20045448,  29,     1.05) /* WeaponDefense */
     , (20045448,  39,       3.6) /* DefaultScale */
     , (20045448,  62,     1.2) /* WeaponOffense */
     , (20045448,  63,     1.5) /* DamageMod */
     , (20045448, 136,    1.35) /* CriticalMultiplier */
     , (20045448, 138,       2) /* SlayerDamageBonus */
     , (20045448, 147,    0.32) /* CriticalFrequency */
     , (20045448, 150,    1.05) /* WeaponMagicDefense */
     , (20045448, 155,     1.3) /* IgnoreArmor */
     , (20045448, 156,     0.5) /* ProcSpellRate */
     , (20045448, 157,     1.9) /* ResistanceModifier */
     , (20045448, 159,    0.15) /* AbsorbMagicDamage */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (20045448,   1, 'Lugian Star of Tukal') /* Name */
     , (20045448,  16, 'This weapon was forged by smiths underneath the mighty Lugian fortress of Linvak Tukal to serve as a goodwill gift in celebration of the alliance between humans and Lugians. Lord Kresovus and Queen Elysa had intended to organize a festival and games to commemorate the alliance, with this mace to be given to the human winner of a tournament of strength. Unfortunately, the Lugian courier carrying this beautiful weapon to Queen Elysa was ambushed and killed. The festival was quietly cancelled.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (20045448,   1, 0x02001352) /* Setup */
     , (20045448,   3, 0x20000014) /* SoundTable */
     , (20045448,   6, 0x04000BEF) /* PaletteBase */
     , (20045448,   7, 0x10000860) /* ClothingBase */
     , (20045448,   8, 0x06005B93) /* Icon */
     , (20045448,  22, 0x3400002B) /* PhysicsEffectTable */
     , (20045448,  46, 0x38000032) /* TsysMutationFilter */
     , (20045448,  52, 0x06005B0C) /* IconUnderlay */
     , (20045448,  55,       4477) /* ProcSpell - Incantation of Corruption */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES

     (20045448,  2966,      2)  /* Aura of Murderous Thirst */
     , (20045448,  6089,      2)  /* Legendary Blood Thirst */
     , (20045448,  6094,      2)  /* Legendary Heart Thirst */
     , (20045448,  6006,      2)  /* Aura of Incantation of Defender Other */
     , (20045448,  6091,      2)  /* Legendary Defender */
          , (20045448,  6014,      2)  /* Aura of Incantation of Heart Seeker Other */
     , (20045448,  2116,      2)  /* Aura of Atlan's Alacrity */;
