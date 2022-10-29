DELETE FROM `weenie` WHERE `class_Id` = 4200162;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200162, 'tailor-crossbowrareironbull', 3, '2021-11-01 00:00:00') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200162,   1,        256) /* ItemType - MissileWeapon */
     , (4200162,   3,          4) /* PaletteTemplate - Brown */
     , (4200162,   5,          1) /* EncumbranceVal */
     , (4200162,   8,         90) /* Mass */
     , (4200162,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (4200162,  16,          1) /* ItemUseable - No */
     , (4200162,  17,        250) /* RareId */
     , (4200162,  19,         20) /* Value */
     , (4200162,  45,          0) /* DamageType - Pierce */
     , (4200162,  46,         32) /* DefaultCombatStyle - Crossbow */
     , (4200162,  48,         47) /* WeaponSkill - MissileWeapons */
     , (4200162,  49,        120) /* WeaponTime */
     , (4200162,  50,          2) /* AmmoType - Bolt */
     , (4200162,  51,          2) /* CombatUse - Missile */
     , (4200162,  52,          2) /* ParentLocation - LeftHand */
     , (4200162,  53,          3) /* PlacementPosition - LeftHand */
     , (4200162,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200162, 169,  118162702) /* TsysMutationData */
     , (4200162, 353,          9) /* WeaponType - Crossbow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200162,  11, True ) /* IgnoreCollisions */
     , (4200162,  13, True ) /* Ethereal */
     , (4200162,  14, True ) /* GravityStatus */
     , (4200162,  19, True ) /* Attackable */
     , (4200162,  22, True ) /* Inscribable */
     , (4200162, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200162,   5,  -0.033) /* ManaRate */
     , (4200162,  12,    0.66) /* Shade */
     , (4200162,  21,       0) /* WeaponLength */
     , (4200162,  22,       0) /* DamageVariance */
     , (4200162,  26,    27.3) /* MaximumVelocity */
     , (4200162,  39,     1.2) /* DefaultScale */
     , (4200162, 110,    1.67) /* BulkMod */
     , (4200162, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200162,   1, 'Iron Bull') /* Name */
     , (4200162,  16, 'This crossbow belonged to the famed Viamontian bowman Frecciano, a soldier in King Varicci''s army. When the royal legions laid siege to the fortress of the rebellious Duke of Bellenesse, Frecciano served as the captain of the Viamontian archers. As the battle progressed, a small group of the Duke''s cavalry charged toward the archers, trampling over the foot soldiers who stood in their way. Frecciano held his ground at the head of the archer formation, firing quarrels into the oncoming cavalry. He was impaled on the spear of the cavalry captain, but he somehow found the fortitude to reload his crossbow and fire it almost point-blank into the chest of the cavalry captain. The blunt quarrel at that short range crushed the heart of the captain upon impact. Help from the royal cavalry arrived soon after, and the Duke''s soldiers were annihilated. Frecciano died, but his crossbow lives on with the name granted to him upon death: Iron Bull.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200162,   1, 0x0200134B) /* Setup */
     , (4200162,   3, 0x20000014) /* SoundTable */
     , (4200162,   6, 0x04000BEF) /* PaletteBase */
     , (4200162,   8, 0x06005B85) /* Icon */
     , (4200162,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200162,  36, 0x0E000012) /* MutateFilter */
     , (4200162,  46, 0x38000032) /* TsysMutationFilter */
     , (4200162,  52, 0x06005B0C) /* IconUnderlay */;
