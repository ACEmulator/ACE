DELETE FROM `weenie` WHERE `class_Id` = 450745;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450745, 'wanddrudgetailor', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450745,   1,      32768) /* ItemType - Caster */
     , (450745,   5,        0) /* EncumbranceVal */
     , (450745,   8,         10) /* Mass */
     , (450745,   9,   16777216) /* ValidLocations - Held */
     , (450745,  16,          1) /* ItemUseable - No */
     , (450745,  18,          1) /* UiEffects - Magical */
     , (450745,  19,         20) /* Value */
     , (450745,  46,        512) /* DefaultCombatStyle - Magic */
     , (450745,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450745,  94,         16) /* TargetType - Creature */
     , (450745, 150,        103) /* HookPlacement - Hook */
     , (450745, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450745,  22, True ) /* Inscribable */
     , (450745,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450745,  29,       1) /* WeaponDefense */
     , (450745,  39,     1.2) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450745,   1, 'Drudge Wand') /* Name */
     , (450745,  15, 'A wand with a shrunken drudge head on it.') /* ShortDesc */
     , (450745,  16, 'A wand with a shrunken drudge head on it.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450745,   1, 0x02000B79) /* Setup */
     , (450745,   3, 0x20000014) /* SoundTable */
     , (450745,   8, 0x060022B3) /* Icon */
     , (450745,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450745,  36, 0x0E000016) /* MutateFilter */;
