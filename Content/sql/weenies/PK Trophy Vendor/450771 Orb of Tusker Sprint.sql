DELETE FROM `weenie` WHERE `class_Id` = 450771;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450771, 'orbtuskersprintpk', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450771,   1,      32768) /* ItemType - Caster */
     , (450771,   3,         20) /* PaletteTemplate - Silver */
     , (450771,   5,         0) /* EncumbranceVal */
     , (450771,   8,         50) /* Mass */
     , (450771,   9,   16777216) /* ValidLocations - Held */
     , (450771,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450771,  18,          1) /* UiEffects - Magical */
     , (450771,  19,       20) /* Value */
     , (450771,  46,        512) /* DefaultCombatStyle - Magic */
     , (450771,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450771,  94,         16) /* TargetType - Creature */
     , (450771, 150,        103) /* HookPlacement - Hook */
     , (450771, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450771,  15, True ) /* LightsStatus */
     , (450771,  22, True ) /* Inscribable */
     , (450771,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450771,   5,   -0.05) /* ManaRate */
     , (450771,  12,     0.6) /* Shade */
     , (450771,  29,       1) /* WeaponDefense */
     , (450771,  39,     1.3) /* DefaultScale */
     , (450771,  76,     0.4) /* Translucency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450771,   1, 'Orb of Tusker Sprint') /* Name */
     , (450771,  15, 'A light orb that seems to want to fly out of your hands.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450771,   1, 0x020000ED) /* Setup */
     , (450771,   3, 0x20000014) /* SoundTable */
     , (450771,   6, 0x04000BF8) /* PaletteBase */
     , (450771,   7, 0x10000127) /* ClothingBase */
     , (450771,   8, 0x06001532) /* Icon */
     , (450771,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450771,  36, 0x0E000016) /* MutateFilter */
     , (450771,  37,  620757051) /* ItemSkillLimit */;
