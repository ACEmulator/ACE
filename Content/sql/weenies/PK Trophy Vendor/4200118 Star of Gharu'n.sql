DELETE FROM `weenie` WHERE `class_Id` = 4200118;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200118, 'spearrarestargharuntailor', 6, '2021-12-21 17:24:33') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200118,   1,          1) /* ItemType - MeleeWeapon */
     , (4200118,   5,          0) /* EncumbranceVal */
     , (4200118,   8,         90) /* Mass */
     , (4200118,   9,   33554432) /* ValidLocations - MeleeWeapon */
     , (4200118,  16,          1) /* ItemUseable - No */
     , (4200118,  17,        285) /* RareId */
     , (4200118,  18,         32) /* UiEffects - Fire */
     , (4200118,  19,         20) /* Value */
     , (4200118,  44,          1) /* Damage */
     , (4200118,  45,         16) /* DamageType - Fire */
     , (4200118,  46,          8) /* DefaultCombatStyle - TwoHanded */
     , (4200118,  47,          2) /* AttackType - Thrust */
     , (4200118,  48,         41) /* WeaponSkill - TwoHandedCombat */
     , (4200118,  49,         30) /* WeaponTime */
     , (4200118,  51,          1) /* CombatUse - Melee */
     , (4200118,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
	 , (4200118,  52,          1) /* ParentLocation - RightHand */
     , (4200118, 151,          2) /* HookType - Wall */
     , (4200118, 353,         11) /* WeaponType - TwoHanded */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200118,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200118,   5,  -0.033) /* ManaRate */
     , (4200118,  21,       0) /* WeaponLength */
     , (4200118,  22,    0.45) /* DamageVariance */
     , (4200118,  26,       0) /* MaximumVelocity */
     , (4200118,  29,    1.18) /* WeaponDefense */
     , (4200118,  39,     1.1) /* DefaultScale */
     , (4200118,  62,    1.18) /* WeaponOffense */
     , (4200118,  63,       1) /* DamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200118,   1, 'Star of Gharu''n') /* Name */
     , (4200118,  16, 'There was once a young Gharu''ndim noble in Tirethas named Javit al-Thirim who cared more about this appearance than duty. His father, despairing of his son''s vanity, forcibly enrolled him in the city guard. Even then, the weapon he brought to guard duty, a huge, unwieldy, extravagantly ornamented spear, was useless as a weapon. One night, as Javit served his guard duty at the lighthouse of Tirethas, a fierce storm blew in from Ironsea and shattered the mirror inside the lighthouse. Thinking quickly, Javit plunged his ornamental spear into the lighthouse flame, creating as bright a beacon as any mirror. The light of the spear was enough to guide ships into safe harbor until a replacement mirror arrived. The weapon''s glossy finish was ruined by the fire, but Javit had finally pleased his father, and the spear still shines with its own light.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200118,   1, 0x0200135A) /* Setup */
     , (4200118,   3, 0x20000014) /* SoundTable */
     , (4200118,   6, 0x04000BEF) /* PaletteBase */
     , (4200118,   7, 0x10000860) /* ClothingBase */
     , (4200118,   8, 0x06005BA3) /* Icon */
     , (4200118,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200118,  36, 0x0E000012) /* MutateFilter */
     , (4200118,  46, 0x38000032) /* TsysMutationFilter */
     , (4200118,  52, 0x06005B0C) /* IconUnderlay */;

