DELETE FROM `weenie` WHERE `class_Id` = 450227;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450227, 'wisporbhighnewtailor', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450227,   1,      32768) /* ItemType - Caster */
     , (450227,   3,          2) /* PaletteTemplate - Blue */
     , (450227,   5,         0) /* EncumbranceVal */
     , (450227,   8,         50) /* Mass */
     , (450227,   9,   16777216) /* ValidLocations - Held */
     , (450227,  16,    6291464) /* ItemUseable - SourceContainedTargetRemoteNeverWalk */
     , (450227,  18,          1) /* UiEffects - Magical */
     , (450227,  19,       20) /* Value */
     , (450227,  46,        512) /* DefaultCombatStyle - Magic */
     , (450227,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450227,  94,         16) /* TargetType - Creature */
     , (450227, 150,        103) /* HookPlacement - Hook */
     , (450227, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450227,  15, True ) /* LightsStatus */
     , (450227,  22, True ) /* Inscribable */
     , (450227,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450227,   5,   -0.05) /* ManaRate */
     , (450227,  12,     0.5) /* Shade */
     , (450227,  29,       1) /* WeaponDefense */
     , (450227,  39,     1.4) /* DefaultScale */
     , (450227,  76,     0.5) /* Translucency */
     , (450227, 144,    0.08) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450227,   1, 'Darker Heart') /* Name */
     , (450227,  16, 'The blackened, flaring heart of a powerful wisp.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450227,   1, 0x020009C5) /* Setup */
     , (450227,   3, 0x20000014) /* SoundTable */
     , (450227,   6, 0x04000BF8) /* PaletteBase */
     , (450227,   7, 0x10000249) /* ClothingBase */
     , (450227,   8, 0x06001F07) /* Icon */
     , (450227,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450227,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */
     , (450227,  37,         16) /* ItemSkillLimit - ManaConversion */;


