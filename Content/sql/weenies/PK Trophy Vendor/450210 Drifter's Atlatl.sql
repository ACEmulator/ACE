DELETE FROM `weenie` WHERE `class_Id` = 450210;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450210, 'atlatlraredriftersatlatltailor', 3, '2021-11-01 00:00:00') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450210,   1,        256) /* ItemType - MissileWeapon */
     , (450210,   3,          1) /* PaletteTemplate - AquaBlue */
     , (450210,   5,        0) /* EncumbranceVal */
     , (450210,   8,         90) /* Mass */
     , (450210,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (450210,  16,          1) /* ItemUseable - No */
     , (450210,  19,      20) /* Value */
     , (450210,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450210,  44,          0) /* Damage */
     , (450210,  45,          2) /* DamageType - Pierce */
     , (450210,  46,       1024) /* DefaultCombatStyle - Atlatl */
     , (450210,  48,         47) /* WeaponSkill - MissileWeapons */
     , (450210,  49,         15) /* WeaponTime */
     , (450210,  50,          4) /* AmmoType - Atlatl */
     , (450210,  51,          2) /* CombatUse - Missile */
     , (450210,  52,          2) /* ParentLocation - LeftHand */
     , (450210,  53,          3) /* PlacementPosition - LeftHand */
     , (450210,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450210, 151,          2) /* HookType - Wall */
     , (450210, 353,         10) /* WeaponType - Thrown */;



INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450210,  11, True ) /* IgnoreCollisions */
     , (450210,  13, True ) /* Ethereal */
     , (450210,  14, True ) /* GravityStatus */
     , (450210,  19, True ) /* Attackable */
     , (450210,  22, True ) /* Inscribable */
     , (450210, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450210,   5,  -0.033) /* ManaRate */
     , (450210,  12,    0.66) /* Shade */
     , (450210,  21,       0) /* WeaponLength */
     , (450210,  22,       0) /* DamageVariance */
     , (450210,  26,    27.3) /* MaximumVelocity */
     , (450210,  29,    1.18) /* WeaponDefense */
     , (450210,  39,     1.2) /* DefaultScale */
     , (450210,  63,     2.9) /* DamageMod */
     , (450210, 110,       1) /* BulkMod */
     , (450210, 111,       1) /* SizeMod */
     , (450210, 138,    1.23) /* SlayerDamageBonus */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450210,   1, 'Drifter''s Atlatl') /* Name */
     , (450210,  16, 'A superbly carved atlatl, whittled from Tusker ivory. Such atatls are utilitarian in nature and meant to be a means of protection while traveling. It looks like the person who made this atlatl had started to carve a hole to turn it into a pipe, before realizing that would make it useless as a weapon. The initials "UU" can be seen, carved into the stem.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450210,   1, 0x02001371) /* Setup */
     , (450210,   3, 0x20000014) /* SoundTable */
     , (450210,   6, 0x04000BEF) /* PaletteBase */
     , (450210,   8, 0x06005BD2) /* Icon */
     , (450210,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450210,  36, 0x0E000012) /* MutateFilter */
     , (450210,  46, 0x38000032) /* TsysMutationFilter */
     , (450210,  52, 0x06005B0C) /* IconUnderlay */;

