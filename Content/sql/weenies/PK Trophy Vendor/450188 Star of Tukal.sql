DELETE FROM `weenie` WHERE `class_Id` = 450188;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450188, 'macerarestartukaltailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450188,   1,          1) /* ItemType - MeleeWeapon */
     , (450188,   5,        0) /* EncumbranceVal */
     , (450188,   8,         90) /* Mass */
     , (450188,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450188,  16,          1) /* ItemUseable - No */
     , (450188,  19,      20) /* Value */
     , (450188,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450188,  44,         0) /* Damage */
     , (450188,  45,          4) /* DamageType - Bludgeon */
     , (450188,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450188,  47,          4) /* AttackType - Slash */
     , (450188,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (450188,  49,         50) /* WeaponTime */
     , (450188,  51,          1) /* CombatUse - Melee */
     , (450188,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450188, 151,          2) /* HookType - Wall */
     , (450188, 353,          4) /* WeaponType - Mace */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450188,  11, True ) /* IgnoreCollisions */
     , (450188,  13, True ) /* Ethereal */
     , (450188,  14, True ) /* GravityStatus */
     , (450188,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450188,   5,  -0.033) /* ManaRate */
     , (450188,  21,       0) /* WeaponLength */
     , (450188,  22,     0.3) /* DamageVariance */
     , (450188,  26,       0) /* MaximumVelocity */
     , (450188,  29,    1.18) /* WeaponDefense */
     , (450188,  39,     1.1) /* DefaultScale */
     , (450188,  62,    1.18) /* WeaponOffense */
     , (450188,  63,       1) /* DamageMod */
     , (450188, 138,    1.15) /* SlayerDamageBonus */
     , (450188, 147,    0.25) /* CriticalFrequency */
     , (450188, 151,       1) /* IgnoreShield */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450188,   1, 'Star of Tukal') /* Name */
     , (450188,  16, 'This weapon was forged by smiths underneath the mighty Lugian fortress of Linvak Tukal to serve as a goodwill gift in celebration of the alliance between humans and Lugians. Lord Kresovus and Queen Elysa had intended to organize a festival and games to commemorate the alliance, with this mace to be given to the human winner of a tournament of strength. Unfortunately, the Lugian courier carrying this beautiful weapon to Queen Elysa was ambushed and killed. The festival was quietly cancelled.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450188,   1, 0x02001352) /* Setup */
     , (450188,   3, 0x20000014) /* SoundTable */
     , (450188,   6, 0x04000BEF) /* PaletteBase */
     , (450188,   7, 0x10000860) /* ClothingBase */
     , (450188,   8, 0x06005B93) /* Icon */
     , (450188,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450188,  36, 0x0E000012) /* MutateFilter */
     , (450188,  46, 0x38000032) /* TsysMutationFilter */
     , (450188,  52, 0x06005B0C) /* IconUnderlay */;
