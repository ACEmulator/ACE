DELETE FROM `weenie` WHERE `class_Id` = 450254;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450254, 'ace450254-corruptedheartwoodwandtailor', 35, '2021-11-01 00:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450254,   1,      32768) /* ItemType - Caster */
     , (450254,   5,         0) /* EncumbranceVal */
     , (450254,   9,   16777216) /* ValidLocations - Held */
     , (450254,  16,    6291464) /* ItemUseable - SourceContainedTargetRemoteNeverWalk */
     , (450254,  18,          1) /* UiEffects - Magical */
     , (450254,  19,        20) /* Value */
     , (450254,  46,        512) /* DefaultCombatStyle - Magic */
     , (450254,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450254,  94,         16) /* TargetType - Creature */
     , (450254, 131,         75) /* MaterialType - Oak */
     , (450254, 151,          2) /* HookType - Wall */
     , (450254, 353,         12) /* WeaponType - Magic */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450254,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450254,   5,  -0.025) /* ManaRate */
     , (450254,  29,    1.0) /* WeaponDefense */
     , (450254,  39,     1.6) /* DefaultScale */
     , (450254, 144,     0.0) /* ManaConversionMod */
     , (450254, 152,    1.0) /* ElementalDamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450254,   1, 'Corrupted Heartwood Wand') /* Name */
     , (450254,  14, 'This item may be tinkered and imbued like any loot-generated weapon.') /* Use */
     , (450254,  16, 'A wand imbued with the energies of the Viridian Rise.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450254,   1, 0x02001C4A) /* Setup */
     , (450254,   3, 0x20000014) /* SoundTable */
     , (450254,   8, 0x06007560) /* Icon */
     , (450254,  22, 0x3400002B) /* PhysicsEffectTable */;


