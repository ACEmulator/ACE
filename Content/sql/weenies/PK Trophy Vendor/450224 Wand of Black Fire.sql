DELETE FROM `weenie` WHERE `class_Id` = 450224;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450224, 'wandblackfiretailor', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450224,   1,      32768) /* ItemType - Caster */
     , (450224,   3,         39) /* PaletteTemplate - Black */
     , (450224,   5,        0) /* EncumbranceVal */
     , (450224,   8,         50) /* Mass */
     , (450224,   9,   16777216) /* ValidLocations - Held */
     , (450224,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450224,  18,          1) /* UiEffects - Magical */
     , (450224,  19,       20) /* Value */
     , (450224,  46,        512) /* DefaultCombatStyle - Magic */
     , (450224,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450224,  94,         16) /* TargetType - Creature */
     , (450224, 150,        103) /* HookPlacement - Hook */
     , (450224, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450224,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450224,   5,  -0.025) /* ManaRate */
     , (450224,  29,       1) /* WeaponDefense */
     , (450224,  39,     0.8) /* DefaultScale */
     , (450224, 144,    0.03) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450224,   1, 'Wand of Black Fire') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450224,   1, 0x02000EF2) /* Setup */
     , (450224,   3, 0x20000014) /* SoundTable */
     , (450224,   6, 0x04000BEF) /* PaletteBase */
     , (450224,   7, 0x1000012E) /* ClothingBase */
     , (450224,   8, 0x06002A43) /* Icon */
     , (450224,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450224,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */
     , (450224,  28,        145) /* Spell - Flame Volley V */
     , (450224,  37,         34) /* ItemSkillLimit - WarMagic */;


