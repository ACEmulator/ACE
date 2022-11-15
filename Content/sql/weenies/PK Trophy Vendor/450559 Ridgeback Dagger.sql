DELETE FROM `weenie` WHERE `class_Id` = 450559;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450559, 'ace450559-ridgebackdaggert', 6, '2022-06-06 04:05:48') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450559,   1,          1) /* ItemType - MeleeWeapon */
     , (450559,   5,        0) /* EncumbranceVal */
     , (450559,   8,         90) /* Mass */
     , (450559,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450559,  16,          1) /* ItemUseable - No */
     , (450559,  19,      20) /* Value */
     , (450559,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450559,  44,         0) /* Damage */
     , (450559,  45,          2) /* DamageType - Pierce */
     , (450559,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450559,  47,          2) /* AttackType - Thrust */
     , (450559,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (450559,  49,         20) /* WeaponTime */
     , (450559,  51,          1) /* CombatUse - Melee */
     , (450559,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450559, 151,          2) /* HookType - Wall */
     , (450559, 353,          6) /* WeaponType - Dagger */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450559,  11, True ) /* IgnoreCollisions */
     , (450559,  13, True ) /* Ethereal */
     , (450559,  14, True ) /* GravityStatus */
     , (450559,  19, True ) /* Attackable */
     , (450559,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450559,   5,  -0.033) /* ManaRate */
     , (450559,  21,       0) /* WeaponLength */
     , (450559,  22,   0.192) /* DamageVariance */
     , (450559,  26,       0) /* MaximumVelocity */
     , (450559,  29,    1.18) /* WeaponDefense */
     , (450559,  39,       1) /* DefaultScale */
     , (450559,  62,    1.18) /* WeaponOffense */
     , (450559,  63,       1) /* DamageMod */
     , (450559, 155,       1) /* IgnoreArmor */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450559,   1, 'Ridgeback Dagger') /* Name */
     , (450559,  16, 'Since the earliest days of the kingdom of Milantos, the rulers of that dark land have hunted the Bristleback Boar as a ritual of manhood. It is forbidden for anyone to hunt the huge and vicious creatures without royal permission, and the meat, which is so tough it is nearly impossible to eat, is served only in royal halls. A series of these daggers, with their backs ridged like the great boar, was commissioned by King Viktosz III to commemorate his son''s first hunt.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450559,   1, 0x0200134E) /* Setup */
     , (450559,   3, 0x20000014) /* SoundTable */
     , (450559,   6, 0x04000BEF) /* PaletteBase */
     , (450559,   7, 0x10000860) /* ClothingBase */
     , (450559,   8, 0x06005B8B) /* Icon */
     , (450559,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450559,  36, 0x0E000012) /* MutateFilter */
     , (450559,  46, 0x38000032) /* TsysMutationFilter */
     , (450559,  52, 0x06005B0C) /* IconUnderlay */;

