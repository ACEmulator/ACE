DELETE FROM `weenie` WHERE `class_Id` = 480511;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480511, 'ace480511-reforgedashbanepk', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480511,   1,          1) /* ItemType - MeleeWeapon */
     , (480511,   5,        0) /* EncumbranceVal */
     , (480511,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480511,  16,          1) /* ItemUseable - No */
     , (480511,  18,         32) /* UiEffects - Fire */
     , (480511,  19,      20) /* Value */
     , (480511,  44,         0) /* Damage */
     , (480511,  45,         16) /* DamageType - Fire */
     , (480511,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480511,  47,          6) /* AttackType - Thrust, Slash */
     , (480511,  48,         45) /* WeaponSkill - LightWeapons */
     , (480511,  49,         20) /* WeaponTime */
     , (480511,  51,          1) /* CombatUse - Melee */
     , (480511,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480511, 151,          2) /* HookType - Wall */
     , (480511, 353,          2) /* WeaponType - Sword */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480511,  22, True ) /* Inscribable */
     , (480511,  23, True ) /* DestroyOnSell */
     , (480511,  69, False) /* IsSellable */
     , (480511,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480511,  21,    0.95) /* WeaponLength */
     , (480511,  39,     1.2) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480511,   1, 'Reforged Ashbane') /* Name */
     , (480511,  16, 'A heavily enchanted flaming sword, wrought from magically-reinforced silver.  The magics are so elegantly inlaid into the weapon that there is no visible enchantment on the blade.  Its ivory haft is inscribed ''Ashbane,'' and bears the name of Leikotha Arenir.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480511,   1, 0x02001127) /* Setup */
     , (480511,   3, 0x20000014) /* SoundTable */
     , (480511,   8, 0x06001E19) /* Icon */
     , (480511,  22, 0x3400002B) /* PhysicsEffectTable */;
