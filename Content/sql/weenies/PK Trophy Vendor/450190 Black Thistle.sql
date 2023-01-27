DELETE FROM `weenie` WHERE `class_Id` = 450190;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450190, 'daggerrareblackthistletailor', 6, '2021-11-01 00:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450190,   1,          1) /* ItemType - MeleeWeapon */
     , (450190,   3,          4) /* PaletteTemplate - Brown */
     , (450190,   5,        0) /* EncumbranceVal */
     , (450190,   8,         90) /* Mass */
     , (450190,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450190,  16,          1) /* ItemUseable - No */
     , (450190,  19,      20) /* Value */
     , (450190,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450190,  44,         0) /* Damage */
     , (450190,  45,          3) /* DamageType - Slash, Pierce */
     , (450190,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450190,  47,          6) /* AttackType - Thrust, Slash */
     , (450190,  48,         45) /* WeaponSkill - LightWeapons */
     , (450190,  49,         20) /* WeaponTime */
     , (450190,  51,          1) /* CombatUse - Melee */
     , (450190,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450190, 151,          2) /* HookType - Wall */
     , (450190, 353,          6) /* WeaponType - Dagger */;



INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450190,  11, True ) /* IgnoreCollisions */
     , (450190,  13, True ) /* Ethereal */
     , (450190,  14, True ) /* GravityStatus */
     , (450190,  19, True ) /* Attackable */
     , (450190,  22, True ) /* Inscribable */
     , (450190,  65, True ) /* IgnoreMagicResist */
     , (450190,  66, True ) /* IgnoreMagicArmor */
     , (450190, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450190,   5,  -0.033) /* ManaRate */
     , (450190,  12,    0.66) /* Shade */
     , (450190,  21,       1) /* WeaponLength */
     , (450190,  22,   0.192) /* DamageVariance */
     , (450190,  29,    1.18) /* WeaponDefense */
     , (450190,  39,       1) /* DefaultScale */
     , (450190,  62,    1.18) /* WeaponOffense */
     , (450190, 138,     1.2) /* SlayerDamageBonus */
     , (450190, 151,       1) /* IgnoreShield */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450190,   1, 'Black Thistle') /* Name */
     , (450190,  16, 'This dagger was created by a barbarian hedge wizard in the untamed territory between Milantos and Souia-Vey. The wizard used it as both a weapon and as a device for channeling his magical power. Somehow, the dagger ended up in Dereth, and it has developed a unique counter-reaction to the magical energies of the new world. It acquired the rare and deadly ability to punch through the magical protections afforded by Life Magic. At the same time, it seems to mark its bearer as a sort of magical lightning rod -- as if the force of the world''s magic itself is taking vengeance against the offender.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450190,   1, 0x02001354) /* Setup */
     , (450190,   3, 0x20000014) /* SoundTable */
     , (450190,   6, 0x04000BEF) /* PaletteBase */
     , (450190,   8, 0x06005B97) /* Icon */
     , (450190,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450190,  36, 0x0E000012) /* MutateFilter */
     , (450190,  46, 0x38000032) /* TsysMutationFilter */
     , (450190,  52, 0x06005B0C) /* IconUnderlay */;

