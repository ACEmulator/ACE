DELETE FROM `weenie` WHERE `class_Id` = 450321;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450321, 'macetumerokwartailor', 6, '2005-02-09 10:00:00') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450321,   1,          1) /* ItemType - MeleeWeapon */
     , (450321,   3,         14) /* PaletteTemplate - Red */
     , (450321,   5,        0) /* EncumbranceVal */
     , (450321,   8,        360) /* Mass */
     , (450321,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450321,  16,          1) /* ItemUseable - No */
     , (450321,  18,          1) /* UiEffects - Magical */
     , (450321,  19,       20) /* Value */
     , (450321,  44,         0) /* Damage */
     , (450321,  45,          4) /* DamageType - Bludgeon */
     , (450321,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450321,  47,          4) /* AttackType - Slash */
     , (450321,  48,          5) /* WeaponSkill - Mace */
     , (450321,  49,         40) /* WeaponTime */
     , (450321,  51,          1) /* CombatUse - Melee */
     , (450321,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450321, 150,        103) /* HookPlacement - Hook */
     , (450321, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450321,  15, True ) /* LightsStatus */
     , (450321,  22, True ) /* Inscribable */
     , (450321,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450321,   5,  -0.025) /* ManaRate */
     , (450321,  21,    0.62) /* WeaponLength */
     , (450321,  22,    0.95) /* DamageVariance */
     , (450321,  29,    1.06) /* WeaponDefense */
     , (450321,  39,     1.2) /* DefaultScale */
     , (450321,  62,    1.06) /* WeaponOffense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450321,   1, 'Assault Mace') /* Name */
     , (450321,  16, 'A mace given as a reward for defeating the leaders of the Shreth Clan.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450321,   1, 0x02000AD4) /* Setup */
     , (450321,   3, 0x20000014) /* SoundTable */
     , (450321,   6, 0x04000BEF) /* PaletteBase */
     , (450321,   7, 0x100002E7) /* ClothingBase */
     , (450321,   8, 0x06002102) /* Icon */
     , (450321,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450321,  37,          5) /* ItemSkillLimit - Mace */;


