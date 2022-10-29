DELETE FROM `weenie` WHERE `class_Id` = 4200172;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200172, 'tailor-ace4200172-aerbaxsdefeat', 35, '2021-11-01 00:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200172,   1,      32768) /* ItemType - Caster */
     , (4200172,   5,         50) /* EncumbranceVal */
     , (4200172,   9,   16777216) /* ValidLocations - Held */
     , (4200172,  16,          1) /* ItemUseable - No */
     , (4200172,  18,         64) /* UiEffects - Lightning */
     , (4200172,  19,         20) /* Value */
     , (4200172,  33,          1) /* Bonded - Bonded */
     , (4200172,  46,        512) /* DefaultCombatStyle - Magic */
     , (4200172,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (4200172,  94,         16) /* TargetType - Creature */
     , (4200172, 114,          1) /* Attuned - Attuned */
     , (4200172, 151,          9) /* HookType - Floor, Yard */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200172,  22, True ) /* Inscribable */
     , (4200172,  23, True ) /* DestroyOnSell */
     , (4200172,  69, False) /* IsSellable */
     , (4200172,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200172,  29,       1) /* WeaponDefense */
     , (4200172, 144,       0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200172,   1, 'Aerbax''s Defeat') /* Name */
     , (4200172,  15, 'An orb crafted from a shard of Aerbax''s mask.  A palpable flux of conflicting energies swirls about the orb.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200172,   1, 0x02001778) /* Setup */
     , (4200172,   3, 0x20000014) /* SoundTable */
     , (4200172,   6, 0x04000BEF) /* PaletteBase */
     , (4200172,   8, 0x06006781) /* Icon */
     , (4200172,  22, 0x3400002B) /* PhysicsEffectTable */;
