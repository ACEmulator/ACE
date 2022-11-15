DELETE FROM `weenie` WHERE `class_Id` = 450206;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450206, 'uararesteelbutterflytailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450206,   1,          1) /* ItemType - MeleeWeapon */
     , (450206,   5,        0) /* EncumbranceVal */
     , (450206,   8,         90) /* Mass */
     , (450206,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450206,  16,          1) /* ItemUseable - No */
     , (450206,  19,      20) /* Value */
     , (450206,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450206,  44,         0) /* Damage */
     , (450206,  45,          1) /* DamageType - Slash */
     , (450206,  46,          1) /* DefaultCombatStyle - Unarmed */
     , (450206,  47,          1) /* AttackType - Punch */
     , (450206,  48,         44) /* WeaponSkill - HeavyWeapons */
     , (450206,  49,         20) /* WeaponTime */
     , (450206,  51,          1) /* CombatUse - Melee */
     , (450206,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450206, 151,          2) /* HookType - Wall */
     , (450206, 353,          1) /* WeaponType - Unarmed */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450206,  11, True ) /* IgnoreCollisions */
     , (450206,  13, True ) /* Ethereal */
     , (450206,  14, True ) /* GravityStatus */
     , (450206,  19, True ) /* Attackable */
     , (450206,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450206,   5,  -0.033) /* ManaRate */
     , (450206,  21,       0) /* WeaponLength */
     , (450206,  22,     0.5) /* DamageVariance */
     , (450206,  26,       0) /* MaximumVelocity */
     , (450206,  29,    1.18) /* WeaponDefense */
     , (450206,  39,       1) /* DefaultScale */
     , (450206,  62,    1.18) /* WeaponOffense */
     , (450206,  63,       1) /* DamageMod */
     , (450206, 138,    1.15) /* SlayerDamageBonus */
     , (450206, 147,    0.25) /* CriticalFrequency */
     , (450206, 151,       1) /* IgnoreShield */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450206,   1, 'Steel Butterfly') /* Name */
     , (450206,  16, 'Elegant. Beautiful. Deadly. Forged to resemble a graceful butterfly, these nekodes would serve as beautiful decorations. But do not be fooled by the intricate designs, the wings of this butterfly are razor sharp! These are the weapons of the warrior monks who defend the temples of the Phoenix in the homeland of the Sho.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450206,   1, 0x0200136A) /* Setup */
     , (450206,   3, 0x20000014) /* SoundTable */
     , (450206,   6, 0x04000BEF) /* PaletteBase */
     , (450206,   7, 0x10000860) /* ClothingBase */
     , (450206,   8, 0x06005BC3) /* Icon */
     , (450206,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450206,  36, 0x0E000012) /* MutateFilter */
     , (450206,  46, 0x38000032) /* TsysMutationFilter */
     , (450206,  52, 0x06005B0C) /* IconUnderlay */;

