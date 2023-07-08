DELETE FROM `weenie` WHERE `class_Id` = 450746;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450746, 'wandmosswarttailor', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450746,   1,      32768) /* ItemType - Caster */
     , (450746,   5,        0) /* EncumbranceVal */
     , (450746,   8,         10) /* Mass */
     , (450746,   9,   16777216) /* ValidLocations - Held */
     , (450746,  16,          1) /* ItemUseable - No */
     , (450746,  18,          1) /* UiEffects - Magical */
     , (450746,  19,         20) /* Value */
     , (450746,  46,        512) /* DefaultCombatStyle - Magic */
     , (450746,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450746,  94,         16) /* TargetType - Creature */
     , (450746, 150,        103) /* HookPlacement - Hook */
     , (450746, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450746,  22, True ) /* Inscribable */
     , (450746,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450746,  29,       1) /* WeaponDefense */
     , (450746,  39,     1.2) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450746,   1, 'Mosswart Wand') /* Name */
     , (450746,  15, 'A wand with a shrunken mosswart head on it.') /* ShortDesc */
     , (450746,  16, 'A wand with a shrunken mosswart head on it.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450746,   1, 0x02000B7A) /* Setup */
     , (450746,   3, 0x20000014) /* SoundTable */
     , (450746,   8, 0x060022B4) /* Icon */
     , (450746,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450746,  36, 0x0E000016) /* MutateFilter */;
