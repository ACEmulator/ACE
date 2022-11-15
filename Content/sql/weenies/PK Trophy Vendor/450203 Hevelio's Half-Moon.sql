DELETE FROM `weenie` WHERE `class_Id` = 450203;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450203, 'uarareheveliohalfmoontailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450203,   1,          1) /* ItemType - MeleeWeapon */
     , (450203,   5,        0) /* EncumbranceVal */
     , (450203,   8,         90) /* Mass */
     , (450203,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450203,  16,          1) /* ItemUseable - No */
     , (450203,  19,      20) /* Value */
     , (450203,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450203,  44,         0) /* Damage */
     , (450203,  45,          2) /* DamageType - Pierce */
     , (450203,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (450203,  47,          1) /* AttackType - Punch */
     , (450203,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450203,  49,         20) /* WeaponTime */
     , (450203,  51,          1) /* CombatUse - Melee */
     , (450203,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450203, 151,          2) /* HookType - Wall */
     , (450203, 353,          1) /* WeaponType - Unarmed */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450203,  11, True ) /* IgnoreCollisions */
     , (450203,  13, True ) /* Ethereal */
     , (450203,  14, True ) /* GravityStatus */
     , (450203,  19, True ) /* Attackable */
     , (450203,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450203,   5,   -0.05) /* ManaRate */
     , (450203,  21,       0) /* WeaponLength */
     , (450203,  22,   0.205) /* DamageVariance */
     , (450203,  26,       0) /* MaximumVelocity */
     , (450203,  29,    1.18) /* WeaponDefense */
     , (450203,  39,     0.7) /* DefaultScale */
     , (450203,  62,    1.18) /* WeaponOffense */
     , (450203,  63,       1) /* DamageMod */
     , (450203, 147,    0.25) /* CriticalFrequency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450203,   1, 'Hevelio''s Half-Moon') /* Name */
     , (450203,  16, 'Hevelio was a beloved crime lord who ruled the docks of Corcosa, the Viamontian royal city. He operated openly and made no secret of his trade or his power. He fed the poor and paid for the education of the children of his senior henchmen. He wielded great influence among the cities various guilds. In gratitude for his work in resolving a dispute between the guilds of the weaponsmiths and the miners, the chief of the weaponsmith''s guild crafted this beautiful hand weapon for him, designed to resemble the half moon which Hevelio used as his personal symbol. Hevelio was so taken with the weapon that he decreed that any time he was forced to kill a member of his own crime syndicate it would be with the Half-Moon. many of his victims were grateful to be so honored.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450203,   1, 0x02001367) /* Setup */
     , (450203,   3, 0x20000014) /* SoundTable */
     , (450203,   6, 0x04000BEF) /* PaletteBase */
     , (450203,   7, 0x10000860) /* ClothingBase */
     , (450203,   8, 0x06005BBD) /* Icon */
     , (450203,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450203,  36, 0x0E000012) /* MutateFilter */
     , (450203,  46, 0x38000032) /* TsysMutationFilter */
     , (450203,  52, 0x06005B0C) /* IconUnderlay */;

