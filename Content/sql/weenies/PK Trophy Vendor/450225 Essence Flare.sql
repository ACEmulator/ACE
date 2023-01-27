DELETE FROM `weenie` WHERE `class_Id` = 450225;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450225, 'wisporbtailor', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450225,   1,      32768) /* ItemType - Caster */
     , (450225,   3,          2) /* PaletteTemplate - Blue */
     , (450225,   5,         0) /* EncumbranceVal */
     , (450225,   8,         50) /* Mass */
     , (450225,   9,   16777216) /* ValidLocations - Held */
     , (450225,  16,    6291464) /* ItemUseable - SourceContainedTargetRemoteNeverWalk */
     , (450225,  18,          1) /* UiEffects - Magical */
     , (450225,  19,        20) /* Value */
     , (450225,  46,        512) /* DefaultCombatStyle - Magic */
     , (450225,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450225,  94,         16) /* TargetType - Creature */
     , (450225, 150,        103) /* HookPlacement - Hook */
     , (450225, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450225,  15, True ) /* LightsStatus */
     , (450225,  22, True ) /* Inscribable */
     , (450225,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450225,   5,   -0.05) /* ManaRate */
     , (450225,  12,     0.5) /* Shade */
     , (450225,  29,       1) /* WeaponDefense */
     , (450225,  39,     1.2) /* DefaultScale */
     , (450225,  76,     0.5) /* Translucency */
     , (450225, 144,    0.05) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450225,   1, 'Essence Flare') /* Name */
     , (450225,  16, 'A flaring essence formerly trapped in a wisp.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450225,   1, 0x020009C7) /* Setup */
     , (450225,   3, 0x20000014) /* SoundTable */
     , (450225,   6, 0x04000BF8) /* PaletteBase */
     , (450225,   7, 0x10000249) /* ClothingBase */
     , (450225,   8, 0x06001F09) /* Icon */
     , (450225,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450225,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */
     , (450225,  37,         16) /* ItemSkillLimit - ManaConversion */;

