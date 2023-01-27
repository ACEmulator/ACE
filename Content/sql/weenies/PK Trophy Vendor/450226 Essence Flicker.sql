DELETE FROM `weenie` WHERE `class_Id` = 450226;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450226, 'wisporblowtailor', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450226,   1,      32768) /* ItemType - Caster */
     , (450226,   3,          2) /* PaletteTemplate - Blue */
     , (450226,   5,         0) /* EncumbranceVal */
     , (450226,   8,         50) /* Mass */
     , (450226,   9,   16777216) /* ValidLocations - Held */
     , (450226,  16,    6291464) /* ItemUseable - SourceContainedTargetRemoteNeverWalk */
     , (450226,  18,          1) /* UiEffects - Magical */
     , (450226,  19,        20) /* Value */
     , (450226,  46,        512) /* DefaultCombatStyle - Magic */
     , (450226,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450226,  94,         16) /* TargetType - Creature */
     , (450226, 150,        103) /* HookPlacement - Hook */
     , (450226, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450226,  15, True ) /* LightsStatus */
     , (450226,  22, True ) /* Inscribable */
     , (450226,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450226,   5,  -0.025) /* ManaRate */
     , (450226,  12,     0.5) /* Shade */
     , (450226,  29,       1) /* WeaponDefense */
     , (450226,  39,       1) /* DefaultScale */
     , (450226,  76,     0.5) /* Translucency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450226,   1, 'Essence Flicker') /* Name */
     , (450226,  16, 'A flickering essence formerly trapped in a wisp.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450226,   1, 0x020009C6) /* Setup */
     , (450226,   3, 0x20000014) /* SoundTable */
     , (450226,   6, 0x04000BF8) /* PaletteBase */
     , (450226,   7, 0x10000249) /* ClothingBase */
     , (450226,   8, 0x06001F08) /* Icon */
     , (450226,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450226,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */
     , (450226,  37,         16) /* ItemSkillLimit - ManaConversion */;


