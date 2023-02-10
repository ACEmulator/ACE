DELETE FROM `weenie` WHERE `class_Id` = 450770;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450770, 'orbtuskerleappk', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450770,   1,      32768) /* ItemType - Caster */
     , (450770,   3,         77) /* PaletteTemplate - BlueGreen */
     , (450770,   5,         0) /* EncumbranceVal */
     , (450770,   8,         50) /* Mass */
     , (450770,   9,   16777216) /* ValidLocations - Held */
     , (450770,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450770,  18,          1) /* UiEffects - Magical */
     , (450770,  19,       20) /* Value */
     , (450770,  46,        512) /* DefaultCombatStyle - Magic */
     , (450770,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450770,  94,         16) /* TargetType - Creature */
     , (450770, 150,        103) /* HookPlacement - Hook */
     , (450770, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450770,  15, True ) /* LightsStatus */
     , (450770,  22, True ) /* Inscribable */
     , (450770,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450770,   5,   -0.05) /* ManaRate */
     , (450770,  12,     0.6) /* Shade */
     , (450770,  29,       1) /* WeaponDefense */
     , (450770,  39,     1.3) /* DefaultScale */
     , (450770,  76,     0.4) /* Translucency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450770,   1, 'Orb of Tusker Leap') /* Name */
     , (450770,  15, 'A light and slightly bouncy orb.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450770,   1, 0x020000ED) /* Setup */
     , (450770,   3, 0x20000014) /* SoundTable */
     , (450770,   6, 0x04000BF8) /* PaletteBase */
     , (450770,   7, 0x10000127) /* ClothingBase */
     , (450770,   8, 0x06001532) /* Icon */
     , (450770,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450770,  36, 0x0E000016) /* MutateFilter */
     , (450770,  37,  620757051) /* ItemSkillLimit */;
