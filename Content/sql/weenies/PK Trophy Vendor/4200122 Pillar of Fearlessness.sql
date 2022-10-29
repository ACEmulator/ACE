DELETE FROM `weenie` WHERE `class_Id` = 4200122;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200122, 'spearrarepillarfearlessness2htailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200122,   1,          1) /* ItemType - MeleeWeapon */
     , (4200122,   5,          0) /* EncumbranceVal */
     , (4200122,   8,         90) /* Mass */
     , (4200122,   9,   33554432) /* ValidLocations - MeleeWeapon */
     , (4200122,  16,          1) /* ItemUseable - No */
     , (4200122,  17,        253) /* RareId */
     , (4200122,  19,         20) /* Value */
     , (4200122,  44,          1) /* Damage */
     , (4200122,  45,          1) /* DamageType - Slash */
     , (4200122,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (4200122,  47,          4) /* AttackType - Slash */
     , (4200122,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (4200122,  49,         30) /* WeaponTime */
     , (4200122,  51,          1) /* CombatUse - Melee */
     , (4200122,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200122, 353,         11) /* WeaponType - TwoHanded */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200122,  11, True ) /* IgnoreCollisions */
     , (4200122,  13, True ) /* Ethereal */
     , (4200122,  14, True ) /* GravityStatus */
     , (4200122,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200122,   5,  -0.033) /* ManaRate */
     , (4200122,  21,       0) /* WeaponLength */
     , (4200122,  22,    0.45) /* DamageVariance */
     , (4200122,  26,       0) /* MaximumVelocity */
     , (4200122,  29,       1) /* WeaponDefense */
     , (4200122,  39,     1.1) /* DefaultScale */
     , (4200122,  62,       1) /* WeaponOffense */
     , (4200122,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200122,   1, 'Pillar of Fearlessness') /* Name */
     , (4200122,  16, 'The four Sho principles of Detachment, Compassion, Humility, and Discipline are well known, and scholars have spent centuries meditating upon their relationship. One maverick scholar, a warrior-monk named Ra Shin, proposed that, once the four pillars were mastered, there was a fifth pillar: Fearlessness. He believed that the a seeker of enlightenment who had achieved perfect understanding of the Four Principles had nothing more to fear, no need for caution. Ra Shin was last seen carrying this spear into the den of a dozen winter bears to test his understanding.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200122,   1, 0x02001358) /* Setup */
     , (4200122,   3, 0x20000014) /* SoundTable */
     , (4200122,   6, 0x04000BEF) /* PaletteBase */
     , (4200122,   7, 0x10000860) /* ClothingBase */
     , (4200122,   8, 0x06005B9F) /* Icon */
     , (4200122,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200122,  36, 0x0E000012) /* MutateFilter */
     , (4200122,  46, 0x38000032) /* TsysMutationFilter */
     , (4200122,  52, 0x06005B0C) /* IconUnderlay */;
