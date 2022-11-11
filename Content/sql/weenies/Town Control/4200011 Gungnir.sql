DELETE FROM `weenie` WHERE `class_Id` = 4200011;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200011, 'ace4200011-gungnir', 6, '2022-01-20 01:51:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200011,   1,          1) /* ItemType - MeleeWeapon */
     , (4200011,   5,        400) /* EncumbranceVal */
     , (4200011,   8,         90) /* Mass */
     , (4200011,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (4200011,  16,          1) /* ItemUseable - No */
     , (4200011,  17,        285) /* RareId */
     , (4200011,  18,         32) /* UiEffects - Fire */
     , (4200011,  19,      50000) /* Value */
     , (4200011,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (4200011,  44,         75) /* Damage */
     , (4200011,  45,         16) /* DamageType - Fire */
     , (4200011,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (4200011,  47,          2) /* AttackType - Thrust */
     , (4200011,  48,         45) /* WeaponSkill - LightWeapons */
     , (4200011,  49,         30) /* WeaponTime */
     , (4200011,  51,          1) /* CombatUse - Melee */
     , (4200011,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200011, 106,        400) /* ItemSpellcraft */
     , (4200011, 107,       3200) /* ItemCurMana */
     , (4200011, 108,       3200) /* ItemMaxMana */
     , (4200011, 109,          0) /* ItemDifficulty */
     , (4200011, 151,          2) /* HookType - Wall */
     , (4200011, 166,         31) /* SlayerCreatureType - Human */
     , (4200011, 179,        512) /* ImbuedEffect - FireRending */
     , (4200011, 265,         41) /* EquipmentSetId - RareDamageBoost */
     , (4200011, 319,         50) /* ItemMaxLevel */
     , (4200011, 320,          1) /* ItemXpStyle - Fixed */
     , (4200011, 353,          5) /* WeaponType - Spear */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (4200011,   4,          0) /* ItemTotalXp */
     , (4200011,   5, 2000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200011,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200011,   5,  -0.033) /* ManaRate */
     , (4200011,  21,       0) /* WeaponLength */
     , (4200011,  22,    0.45) /* DamageVariance */
     , (4200011,  26,       0) /* MaximumVelocity */
     , (4200011,  29,    1.18) /* WeaponDefense */
     , (4200011,  39,       2) /* DefaultScale */
     , (4200011,  62,    1.18) /* WeaponOffense */
     , (4200011,  63,       1) /* DamageMod */
     , (4200011, 136,       3) /* CriticalMultiplier */
     , (4200011, 138,       3) /* SlayerDamageBonus */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200011,   1, 'Gungnir') /* Name */
     , (4200011,  16, 'This spear was crafted by Dvalinn using only the rarest of metals and forged in the firey magma pools of Crater. It is so well balanced that it could strike any target, no matter the skill or strength of the wielder') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200011,   1, 0x0200135A) /* Setup */
     , (4200011,   3, 0x20000014) /* SoundTable */
     , (4200011,   6, 0x04000BEF) /* PaletteBase */
     , (4200011,   7, 0x10000860) /* ClothingBase */
     , (4200011,   8, 0x06005BA3) /* Icon */
     , (4200011,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200011,  36, 0x0E000012) /* MutateFilter */
     , (4200011,  46, 0x38000032) /* TsysMutationFilter */
     , (4200011,  52, 0x06005B0C) /* IconUnderlay */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (4200011,  3963,      2)  /* Epic Coordination */
     , (4200011,  3965,      2)  /* Epic Strength */
     , (4200011,  4297,      2)  /* Incantation of Coordination Self */
     , (4200011,  4325,      2)  /* Incantation of Strength Self */
     , (4200011,  4395,      2)  /* Aura of Incantation of Blood Drinker Self */
     , (4200011,  4400,      2)  /* Aura of Incantation of Defender Self */
     , (4200011,  4518,      2)  /* Incantation of Light Weapon Mastery Self */
     , (4200011,  4661,      2)  /* Epic Blood Thirst */
     , (4200011,  4682,      2)  /* Epic Stamina Gain */
     , (4200011,  4686,      2)  /* Epic Light Weapon Aptitude */;

/* Lifestoned Changelog:
{
  "Changelog": [
    {
      "created": "2022-01-20T01:24:23.2335222Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    }
  ],
  "IsDone": false
}
*/
