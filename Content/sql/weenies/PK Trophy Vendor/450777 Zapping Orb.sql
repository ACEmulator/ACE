DELETE FROM `weenie` WHERE `class_Id` = 450777;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450777, 'casterelectricpk', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450777,   1,      32768) /* ItemType - Caster */
     , (450777,   3,         82) /* PaletteTemplate - PinkPurple */
     , (450777,   5,         0) /* EncumbranceVal */
     , (450777,   8,         50) /* Mass */
     , (450777,   9,   16777216) /* ValidLocations - Held */
     , (450777,  16,          1) /* ItemUseable - No */
     , (450777,  18,         64) /* UiEffects - Lightning */
     , (450777,  19,        20) /* Value */
     , (450777,  46,        512) /* DefaultCombatStyle - Magic */
     , (450777,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450777,  94,         16) /* TargetType - Creature */
     , (450777, 150,        103) /* HookPlacement - Hook */
     , (450777, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450777,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450777,  29,       1) /* WeaponDefense */
     , (450777,  39,     1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450777,   1, 'Zapping Orb') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450777,   1, 0x020011EE) /* Setup */
     , (450777,   3, 0x20000014) /* SoundTable */
     , (450777,   6, 0x0400195D) /* PaletteBase */
     , (450777,   7, 0x10000588) /* ClothingBase */
     , (450777,   8, 0x06001532) /* Icon */
     , (450777,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450777,  27, 0x40000031) /* UseUserAnimation - MagicHeal */
     , (450777,  36, 0x0E000016) /* MutateFilter */
     , (450777,  46, 0x38000030) /* TsysMutationFilter */;
