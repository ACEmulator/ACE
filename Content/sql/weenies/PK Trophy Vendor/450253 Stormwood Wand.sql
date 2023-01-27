DELETE FROM `weenie` WHERE `class_Id` = 450253;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450253, 'ace450253-stormwoodwand', 35, '2021-11-01 00:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450253,   1,      32768) /* ItemType - Caster */
     , (450253,   5,         50) /* EncumbranceVal */
     , (450253,   9,   16777216) /* ValidLocations - Held */
     , (450253,  16,    6291464) /* ItemUseable - SourceContainedTargetRemoteNeverWalk */
     , (450253,  18,          1) /* UiEffects - Magical */
     , (450253,  19,        20) /* Value */
     , (450253,  46,        512) /* DefaultCombatStyle - Magic */
     , (450253,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450253,  94,         16) /* TargetType - Creature */
     , (450253, 131,         75) /* MaterialType - Oak */
     , (450253, 151,          2) /* HookType - Wall */
     , (450253, 353,         12) /* WeaponType - Magic */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450253,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450253,   5,  -0.025) /* ManaRate */
     , (450253,  29,    1.0) /* WeaponDefense */
     , (450253,  39,     4.0) /* DefaultScale */
     , (450253, 144,     0.0) /* ManaConversionMod */
     , (450253, 152,    1.0) /* ElementalDamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450253,   1, 'Stormwood Wand') /* Name */
     , (450253,  14, 'This item may be tinkered and imbued like any loot-generated weapon.') /* Use */
     , (450253,  16, 'A wand imbued with the energies of the Viridian Rise.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450253,   1, 0x02001C4B) /* Setup */
     , (450253,   3, 0x20000014) /* SoundTable */
     , (450253,   8, 0x06007560) /* Icon */
     , (450253,  22, 0x3400002B) /* PhysicsEffectTable */;

