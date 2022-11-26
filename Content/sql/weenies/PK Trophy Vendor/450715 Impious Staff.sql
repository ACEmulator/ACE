DELETE FROM `weenie` WHERE `class_Id` = 450715;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450715, 'staffimpioustailor', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450715,   1,      32768) /* ItemType - Caster */
     , (450715,   3,         39) /* PaletteTemplate - Black */
     , (450715,   5,         0) /* EncumbranceVal */
     , (450715,   8,         25) /* Mass */
     , (450715,   9,   16777216) /* ValidLocations - Held */
     , (450715,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450715,  18,          1) /* UiEffects - Magical */
     , (450715,  19,      20) /* Value */
     , (450715,  33,          1) /* Bonded - Bonded */
     , (450715,  46,        512) /* DefaultCombatStyle - Magic */
     , (450715,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450715,  94,         16) /* TargetType - Creature */
     , (450715, 150,        103) /* HookPlacement - Hook */
     , (450715, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450715,  22, True ) /* Inscribable */
     , (450715,  23, True ) /* DestroyOnSell */
     , (450715,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450715,  29,       1) /* WeaponDefense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450715,   1, 'Impious Staff') /* Name */
     , (450715,  15, 'This staff is made from a metal alloy and carbonized iron.') /* ShortDesc */
     , (450715,  16, 'Made from a metal alloy and carbonized iron. This staff once belonged to an ancient group of acolytes who possessed magical powers.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450715,   1, 0x0200024E) /* Setup */
     , (450715,   3, 0x20000014) /* SoundTable */
     , (450715,   6, 0x04000BEF) /* PaletteBase */
     , (450715,   7, 0x10000154) /* ClothingBase */
     , (450715,   8, 0x0600151E) /* Icon */
     , (450715,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450715,  36, 0x0E000016) /* MutateFilter */;
