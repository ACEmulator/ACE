DELETE FROM `weenie` WHERE `class_Id` = 480568;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480568, 'macevaguratpk', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480568,   1,          1) /* ItemType - MeleeWeapon */
     , (480568,   5,        0) /* EncumbranceVal */
     , (480568,   8,        360) /* Mass */
     , (480568,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (480568,  16,          1) /* ItemUseable - No */
     , (480568,  18,          1) /* UiEffects - Magical */
     , (480568,  19,        20) /* Value */
     , (480568,  44,         0) /* Damage */
     , (480568,  45,          4) /* DamageType - Bludgeon */
     , (480568,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (480568,  47,          4) /* AttackType - Slash */
     , (480568,  48,         45) /* WeaponSkill - LightWeapons */
     , (480568,  49,         45) /* WeaponTime */
     , (480568,  51,          1) /* CombatUse - Melee */
     , (480568,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480568, 150,        103) /* HookPlacement - Hook */
     , (480568, 151,          2) /* HookType - Wall */
     , (480568, 353,          4) /* WeaponType - Mace */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480568,  11, True ) /* IgnoreCollisions */
     , (480568,  13, True ) /* Ethereal */
     , (480568,  14, True ) /* GravityStatus */
     , (480568,  19, True ) /* Attackable */
     , (480568,  22, True ) /* Inscribable */
     , (480568,  23, True ) /* DestroyOnSell */
     , (480568,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480568,   5,  -0.033) /* ManaRate */
     , (480568,  21,    0.62) /* WeaponLength */
     , (480568,  22,     0.5) /* DamageVariance */
     , (480568,  26,       0) /* MaximumVelocity */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480568,   1, 'Vagurat''s Hand') /* Name */
     , (480568,  16, 'A mace crafted to look like the Mosswart relic, The Hand of Vagurat. A small stamp on the hilt reads: A Ketnan Product.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480568,   1, 0x02001111) /* Setup */
     , (480568,   3, 0x20000014) /* SoundTable */
     , (480568,   8, 0x0600340D) /* Icon */
     , (480568,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480568,  37,          5) /* ItemSkillLimit - Mace */;


