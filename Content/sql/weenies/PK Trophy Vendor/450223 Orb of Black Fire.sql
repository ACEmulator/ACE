DELETE FROM `weenie` WHERE `class_Id` = 450223;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450223, 'orbblackfiretailor', 35, '2021-11-01 00:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450223,   1,      32768) /* ItemType - Caster */
     , (450223,   5,         0) /* EncumbranceVal */
     , (450223,   8,         50) /* Mass */
     , (450223,   9,   16777216) /* ValidLocations - Held */
     , (450223,  16,    6291464) /* ItemUseable - SourceContainedTargetRemoteNeverWalk */
     , (450223,  18,          1) /* UiEffects - Magical */
     , (450223,  19,       20) /* Value */
     , (450223,  46,        512) /* DefaultCombatStyle - Magic */
     , (450223,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450223,  94,         16) /* TargetType - Creature */
     , (450223, 150,        103) /* HookPlacement - Hook */
     , (450223, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450223,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450223,   5,  -0.025) /* ManaRate */
     , (450223,  29,       1) /* WeaponDefense */
     , (450223,  39,     0.8) /* DefaultScale */
     , (450223, 144,    0.03) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450223,   1, 'Orb of Black Fire') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450223,   1, 0x02000EE9) /* Setup */
     , (450223,   3, 0x20000014) /* SoundTable */
     , (450223,   6, 0x04000BF8) /* PaletteBase */
     , (450223,   7, 0x10000127) /* ClothingBase */
     , (450223,   8, 0x06002A42) /* Icon */
     , (450223,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450223,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */
     , (450223,  28,        145) /* Spell - Flame Volley V */
     , (450223,  37,         33) /* ItemSkillLimit - LifeMagic */;

