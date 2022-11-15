DELETE FROM `weenie` WHERE `class_Id` = 450211;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450211, 'bowrarecorsairsarc`tailor', 3, '2021-11-17 16:56:08') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450211,   1,        256) /* ItemType - MissileWeapon */
     , (450211,   3,          4) /* PaletteTemplate - Brown */
     , (450211,   5,        0) /* EncumbranceVal */
     , (450211,   8,         90) /* Mass */
     , (450211,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (450211,  16,          1) /* ItemUseable - No */
     , (450211,  18,         32) /* UiEffects - Fire */
     , (450211,  19,      20) /* Value */
     , (450211,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450211,  44,          0) /* Damage */
     , (450211,  45,         16) /* DamageType - Fire */
     , (450211,  46,         16) /* DefaultCombatStyle - Bow */
     , (450211,  48,         47) /* WeaponSkill - MissileWeapons */
     , (450211,  49,         70) /* WeaponTime */
     , (450211,  50,          1) /* AmmoType - Arrow */
     , (450211,  51,          2) /* CombatUse - Missile */
     , (450211,  52,          2) /* ParentLocation - LeftHand */
     , (450211,  53,          3) /* PlacementPosition - LeftHand */
     , (450211,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450211, 151,          2) /* HookType - Wall */
     , (450211, 353,          8) /* WeaponType - Bow */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450211,  11, True ) /* IgnoreCollisions */
     , (450211,  13, True ) /* Ethereal */
     , (450211,  14, True ) /* GravityStatus */
     , (450211,  19, True ) /* Attackable */
     , (450211,  22, True ) /* Inscribable */
     , (450211,  91, False) /* Retained */
     , (450211, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450211,   5,  -0.033) /* ManaRate */
     , (450211,  12,    0.66) /* Shade */
     , (450211,  21,       0) /* WeaponLength */
     , (450211,  22,       0) /* DamageVariance */
     , (450211,  26,    27.3) /* MaximumVelocity */
     , (450211,  29,    1.18) /* WeaponDefense */
     , (450211,  39,     1.3) /* DefaultScale */
     , (450211,  62,       1) /* WeaponOffense */
     , (450211,  63,     2.7) /* DamageMod */
     , (450211, 110,    1.67) /* BulkMod */
     , (450211, 111,       1) /* SizeMod */
     , (450211, 138,     1.2) /* SlayerDamageBonus */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450211,   1, 'Corsair''s Arc') /* Name */
     , (450211,  16, 'A stout shortbow adorned with jewels and carvings of fish. Oddly, it seems to be unusually warm to the touch. These bows are rumored to be used by the fearsome Sword Squall pirates of the Ironsea. Of course, these are only rumors, since no one has actually seen a Sword Squall Pirate and lived to tell the tale.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450211,   1, 0x02001375) /* Setup */
     , (450211,   3, 0x20000014) /* SoundTable */
     , (450211,   6, 0x04000BEF) /* PaletteBase */
     , (450211,   8, 0x06005BDA) /* Icon */
     , (450211,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450211,  36, 0x0E000012) /* MutateFilter */
     , (450211,  46, 0x38000032) /* TsysMutationFilter */
     , (450211,  52, 0x06005B0C) /* IconUnderlay */;

