DELETE FROM `weenie` WHERE `class_Id` = 4200160;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200160, 'tailor-crossbowrareassassinswhisper', 3, '2021-11-01 00:00:00') /* MissileLauncher */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200160,   1,        256) /* ItemType - MissileWeapon */
     , (4200160,   3,          4) /* PaletteTemplate - Brown */
     , (4200160,   5,          1) /* EncumbranceVal */
     , (4200160,   8,         90) /* Mass */
     , (4200160,   9,    4194304) /* ValidLocations - MissileWeapon */
     , (4200160,  16,          1) /* ItemUseable - No */
     , (4200160,  17,        283) /* RareId */
     , (4200160,  18,       2048) /* UiEffects - Piercing */
     , (4200160,  19,         20) /* Value */
     , (4200160,  44,          0) /* Damage */
     , (4200160,  45,          0) /* DamageType - Pierce */
     , (4200160,  46,         32) /* DefaultCombatStyle - Crossbow */
     , (4200160,  48,         47) /* WeaponSkill - MissileWeapons */
     , (4200160,  49,         90) /* WeaponTime */
     , (4200160,  50,          2) /* AmmoType - Bolt */
     , (4200160,  51,          2) /* CombatUse - Missile */
     , (4200160,  52,          2) /* ParentLocation - LeftHand */
     , (4200160,  53,          3) /* PlacementPosition - LeftHand */
     , (4200160,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200160, 151,          2) /* HookType - Wall */
     , (4200160, 169,  118162702) /* TsysMutationData */
     , (4200160, 353,          9) /* WeaponType - Crossbow */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200160,  11, True ) /* IgnoreCollisions */
     , (4200160,  13, True ) /* Ethereal */
     , (4200160,  14, True ) /* GravityStatus */
     , (4200160,  19, True ) /* Attackable */
     , (4200160,  22, True ) /* Inscribable */
     , (4200160, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200160,   5,  -0.033) /* ManaRate */
     , (4200160,  12,    0.66) /* Shade */
     , (4200160,  21,       0) /* WeaponLength */
     , (4200160,  22,       0) /* DamageVariance */
     , (4200160,  26,    27.3) /* MaximumVelocity */
     , (4200160,  39,     1.2) /* DefaultScale */
     , (4200160, 110,    1.67) /* BulkMod */
     , (4200160, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200160,   1, 'Assassin''s Whisper') /* Name */
     , (4200160,  16, 'This crossbow once belonged to a Viamontian assassin named Rafelle. Rafelle was a very cocky young assassin, and she liked to speak with her victims before killing them. She would invariably claim to know "the sound that death makes" and then offer to whisper the sound in the victim''s ear. She would, of course, pull the trigger as she whispered this secret knowledge. One day, she dallied long enough with one of her victims that the victim''s bodyguard was able to sneak up behind her and slap the crossbow away as she pulled the trigger. That threw off her aim and the bolt ended up flying wild. Rafelle was disarmed, subdued, and executed. And what did she whisper in her victim''s ear, right before she tried to kill him? "Rafelle."') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200160,   1, 0x0200134D) /* Setup */
     , (4200160,   3, 0x20000014) /* SoundTable */
     , (4200160,   6, 0x04000BEF) /* PaletteBase */
     , (4200160,   8, 0x06005B89) /* Icon */
     , (4200160,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200160,  36, 0x0E000012) /* MutateFilter */
     , (4200160,  46, 0x38000032) /* TsysMutationFilter */
     , (4200160,  52, 0x06005B0C) /* IconUnderlay */;