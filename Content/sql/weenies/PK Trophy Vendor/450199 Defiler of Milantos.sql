DELETE FROM `weenie` WHERE `class_Id` = 450199;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450199, 'swordraredefilermilantostailor', 6, '2021-11-17 16:56:08') /* MeleeWeapon */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450199,   1,          1) /* ItemType - MeleeWeapon */
     , (450199,   5,        0) /* EncumbranceVal */
     , (450199,   8,         90) /* Mass */
     , (450199,   9,    1048576) /* ValidLocations - MeleeWeapon */
     , (450199,  16,          1) /* ItemUseable - No */
     , (450199,  19,      20) /* Value */
     , (450199,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450199,  44,         0) /* Damage */
     , (450199,  45,          3) /* DamageType - Slash, Pierce */
     , (450199,  46,          2) /* DefaultCombatStyle - OneHanded */
     , (450199,  47,          6) /* AttackType - Thrust, Slash */
     , (450199,  48,         46) /* WeaponSkill - FinesseWeapons */
     , (450199,  49,         40) /* WeaponTime */
     , (450199,  51,          1) /* CombatUse - Melee */
     , (450199,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450199, 151,          2) /* HookType - Wall */
     , (450199, 353,          2) /* WeaponType - Sword */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450199,  11, True ) /* IgnoreCollisions */
     , (450199,  13, True ) /* Ethereal */
     , (450199,  14, True ) /* GravityStatus */
     , (450199,  19, True ) /* Attackable */
     , (450199,  22, True ) /* Inscribable */
     , (450199,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450199,   5,  -0.033) /* ManaRate */
     , (450199,  21,       0) /* WeaponLength */
     , (450199,  22,   0.205) /* DamageVariance */
     , (450199,  26,       0) /* MaximumVelocity */
     , (450199,  29,    1.18) /* WeaponDefense */
     , (450199,  39,     1.1) /* DefaultScale */
     , (450199,  62,    1.18) /* WeaponOffense */
     , (450199,  63,       1) /* DamageMod */
     , (450199, 147,     0.3) /* CriticalFrequency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450199,   1, 'Defiler of Milantos') /* Name */
     , (450199,  16, 'This sword was the personal weapon of Karuz, sorceror-king of Milantos. At his direction, Milantan sorcerers had been kidnapping villagers of neighboring Souia-Vey to perform foul rituals upon them. Their depredations were halted by the arrival of the hero, Brador, who fought with a great sword of ice. Karuz decided to end Brador''s interference personally, and brought forth his own terrible weapon to do battle with the interloper. There was a great clash, which laid waste to the land all around them. When the dust and darkness cleared, neither Karuz nor his foe were anywhere to be found.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450199,   1, 0x02001361) /* Setup */
     , (450199,   3, 0x20000014) /* SoundTable */
     , (450199,   6, 0x04000BEF) /* PaletteBase */
     , (450199,   7, 0x10000860) /* ClothingBase */
     , (450199,   8, 0x06005BB1) /* Icon */
     , (450199,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450199,  36, 0x0E000012) /* MutateFilter */
     , (450199,  46, 0x38000032) /* TsysMutationFilter */
     , (450199,  52, 0x06005B0C) /* IconUnderlay */;

