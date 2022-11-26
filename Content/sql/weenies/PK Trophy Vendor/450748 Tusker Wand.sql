DELETE FROM `weenie` WHERE `class_Id` = 450748;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450748, 'wandtuskertailor', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450748,   1,      32768) /* ItemType - Caster */
     , (450748,   5,        0) /* EncumbranceVal */
     , (450748,   8,         10) /* Mass */
     , (450748,   9,   16777216) /* ValidLocations - Held */
     , (450748,  16,          1) /* ItemUseable - No */
     , (450748,  18,          1) /* UiEffects - Magical */
     , (450748,  19,         20) /* Value */
     , (450748,  46,        512) /* DefaultCombatStyle - Magic */
     , (450748,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450748,  94,         16) /* TargetType - Creature */
     , (450748, 150,        103) /* HookPlacement - Hook */
     , (450748, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450748,  22, True ) /* Inscribable */
     , (450748,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450748,  29,       1) /* WeaponDefense */
     , (450748,  39,     1.2) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450748,   1, 'Tusker Wand') /* Name */
     , (450748,  15, 'A wand with a shrunken tusker head on it.') /* ShortDesc */
     , (450748,  16, 'A wand with a shrunken tusker head on it.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450748,   1, 0x02000B7C) /* Setup */
     , (450748,   3, 0x20000014) /* SoundTable */
     , (450748,   8, 0x060022B6) /* Icon */
     , (450748,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450748,  36, 0x0E000016) /* MutateFilter */;
