DELETE FROM `weenie` WHERE `class_Id` = 4200010;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200010, 'ace4200010-protectorsspike', 6, '2022-01-20 01:11:10') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200010,   1,          1) /* ItemType - MeleeWeapon */
     , (4200010,   5,        850) /* EncumbranceVal */
     , (4200010,   8,         90) /* Mass */
     , (4200010,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (4200010,  16,          1) /* ItemUseable - No */
     , (4200010,  19,      50000) /* Value */
     , (4200010,  44,         85) /* Damage */
     , (4200010,  45,          4) /* DamageType - Bludgeon */
     , (4200010,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (4200010,  47,          4) /* AttackType - Slash */
     , (4200010,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (4200010,  49,         50) /* WeaponTime */
     , (4200010,  51,          1) /* CombatUse - Melee */
     , (4200010,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200010, 106,        350) /* ItemSpellcraft */
     , (4200010, 107,       2500) /* ItemCurMana */
     , (4200010, 108,       2500) /* ItemMaxMana */
     , (4200010, 109,          0) /* ItemDifficulty */
     , (4200010, 151,          2) /* HookType - Wall */
     , (4200010, 166,         31) /* SlayerCreatureType - Human */
     , (4200010, 179,         32) /* ImbuedEffect - BludgeonRending */
     , (4200010, 353,          4) /* WeaponType - Mace */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200010,  11, True ) /* IgnoreCollisions */
     , (4200010,  13, True ) /* Ethereal */
     , (4200010,  14, True ) /* GravityStatus */
     , (4200010,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200010,   5,  -0.033) /* ManaRate */
     , (4200010,  21,       0) /* WeaponLength */
     , (4200010,  22,     0.2) /* DamageVariance */
     , (4200010,  26,       0) /* MaximumVelocity */
     , (4200010,  29,    1.18) /* WeaponDefense */
     , (4200010,  39,       2) /* DefaultScale */
     , (4200010,  62,     1.2) /* WeaponOffense */
     , (4200010,  63,       1) /* DamageMod */
     , (4200010, 138,     3.4) /* SlayerDamageBonus */
     , (4200010, 147,    0.25) /* CriticalFrequency */
     , (4200010, 151,       1) /* IgnoreShield */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200010,   1, 'Protector''s Spike') /* Name */
     , (4200010,  16, 'This weapon was forged from the metal of an ancient dying star..') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200010,   1, 0x02001352) /* Setup */
     , (4200010,   3, 0x20000014) /* SoundTable */
     , (4200010,   6, 0x04000BEF) /* PaletteBase */
     , (4200010,   7, 0x10000860) /* ClothingBase */
     , (4200010,   8, 0x06005B93) /* Icon */
     , (4200010,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200010,  36, 0x0E000012) /* MutateFilter */
     , (4200010,  46, 0x38000032) /* TsysMutationFilter */
     , (4200010,  52, 0x06005B0C) /* IconUnderlay */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (4200010,  4395,      2)  /* Aura of Incantation of Blood Drinker Self */
     , (4200010,  4400,      2)  /* Aura of Incantation of Defender Self */
     , (4200010,  4470,      2)  /* Incantation of Lightning Protection Self */
     , (4200010,  6089,      2)  /* Legendary Blood Thirst */
     , (4200010,  4712,      2)  /* Epic Heavy Weapon Aptitude */;

/* Lifestoned Changelog:
{
  "LastModified": "2022-01-20T01:09:06.5170365Z",
  "ModifiedBy": "ACE.Adapter",
  "Changelog": [
    {
      "created": "2022-01-20T01:00:09.4016554Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    },
    {
      "created": "2022-01-20T01:08:43.3224637Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    },
    {
      "created": "2022-01-20T01:09:06.5164836Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    }
  ],
  "UserChangeSummary": "Weenie exported from ACEmulator world database using ACE.Adapter",
  "IsDone": false
}
*/
