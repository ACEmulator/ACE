DELETE FROM `weenie` WHERE `class_Id` = 450781;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450781, 'casterpiercingpk', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450781,   1,      32768) /* ItemType - Caster */
     , (450781,   3,         21) /* PaletteTemplate - Gold */
     , (450781,   5,         0) /* EncumbranceVal */
     , (450781,   8,         50) /* Mass */
     , (450781,   9,   16777216) /* ValidLocations - Held */
     , (450781,  16,          1) /* ItemUseable - No */
     , (450781,  18,       2048) /* UiEffects - Piercing */
     , (450781,  19,        20) /* Value */
     , (450781,  46,        512) /* DefaultCombatStyle - Magic */
     , (450781,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450781,  94,         16) /* TargetType - Creature */
     , (450781, 150,        103) /* HookPlacement - Hook */
     , (450781, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450781,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450781,  29,       1) /* WeaponDefense */
     , (450781,  39,     1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450781,   1, 'Prickly Orb') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450781,   1, 0x020011EB) /* Setup */
     , (450781,   3, 0x20000014) /* SoundTable */
     , (450781,   6, 0x0400195D) /* PaletteBase */
     , (450781,   7, 0x10000588) /* ClothingBase */
     , (450781,   8, 0x06001532) /* Icon */
     , (450781,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450781,  27, 0x40000031) /* UseUserAnimation - MagicHeal */
     , (450781,  36, 0x0E000016) /* MutateFilter */
     , (450781,  46, 0x38000030) /* TsysMutationFilter */;
