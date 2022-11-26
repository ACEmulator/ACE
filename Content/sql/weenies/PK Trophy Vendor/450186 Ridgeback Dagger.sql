DELETE FROM `weenie` WHERE `class_Id` = 450186;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450186, 'daggerrareridgebackdaggertailor2', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450186,   1,          1) /* ItemType - MeleeWeapon */
     , (450186,   5,        0) /* EncumbranceVal */
     , (450186,   8,         90) /* Mass */
     , (450186,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450186,  16,          1) /* ItemUseable - No */
     , (450186,  19,      20) /* Value */
     , (450186,  45,          2) /* DamageType - Pierce */
     , (450186,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450186,  47,          2) /* AttackType - Thrust */
     , (450186,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (450186,  51,          1) /* CombatUse - Melee */
     , (450186,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450186, 151,          2) /* HookType - Wall */
	 , (450186, 353,          6) /* WeaponType - Dagger */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450186,  11, True ) /* IgnoreCollisions */
     , (450186,  13, True ) /* Ethereal */
     , (450186,  14, True ) /* GravityStatus */
     , (450186,  19, True ) /* Attackable */
     , (450186,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450186,   1, 'Ridgeback Dagger') /* Name */
     , (450186,  16, 'Since the earliest days of the kingdom of Milantos, the rulers of that dark land have hunted the Bristleback Boar as a ritual of manhood. It is forbidden for anyone to hunt the huge and vicious creatures without royal permission, and the meat, which is so tough it is nearly impossible to eat, is served only in royal halls. A series of these daggers, with their backs ridged like the great boar, was commissioned by King Viktosz III to commemorate his son''s first hunt.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450186,   1, 0x0200134E) /* Setup */
     , (450186,   3, 0x20000014) /* SoundTable */
     , (450186,   6, 0x04000BEF) /* PaletteBase */
     , (450186,   7, 0x10000860) /* ClothingBase */
     , (450186,   8, 0x06005B8B) /* Icon */
     , (450186,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450186,  36, 0x0E000012) /* MutateFilter */
     , (450186,  46, 0x38000032) /* TsysMutationFilter */
     , (450186,  52, 0x06005B0C) /* IconUnderlay */;


