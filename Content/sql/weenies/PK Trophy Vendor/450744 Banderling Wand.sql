DELETE FROM `weenie` WHERE `class_Id` = 450744;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450744, 'wandbanderlingtailor', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450744,   1,      32768) /* ItemType - Caster */
     , (450744,   5,        0) /* EncumbranceVal */
     , (450744,   8,         10) /* Mass */
     , (450744,   9,   16777216) /* ValidLocations - Held */
     , (450744,  16,          1) /* ItemUseable - No */
     , (450744,  18,          1) /* UiEffects - Magical */
     , (450744,  19,         20) /* Value */
     , (450744,  46,        512) /* DefaultCombatStyle - Magic */
     , (450744,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450744,  94,         16) /* TargetType - Creature */
     , (450744, 150,        103) /* HookPlacement - Hook */
     , (450744, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450744,  22, True ) /* Inscribable */
     , (450744,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450744,  29,       1) /* WeaponDefense */
     , (450744,  39,     1.2) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450744,   1, 'Banderling Wand') /* Name */
     , (450744,  15, 'A wand with a shrunken banderling head on it.') /* ShortDesc */
     , (450744,  16, 'A wand with a shrunken banderling head on it.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450744,   1, 0x02000B78) /* Setup */
     , (450744,   3, 0x20000014) /* SoundTable */
     , (450744,   8, 0x060022B2) /* Icon */
     , (450744,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450744,  36, 0x0E000016) /* MutateFilter */;
