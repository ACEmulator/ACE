DELETE FROM `weenie` WHERE `class_Id` = 450245;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450245, 'orbwhitebunnytailor', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450245,   1,      32768) /* ItemType - Caster */
     , (450245,   3,         61) /* PaletteTemplate - White */
     , (450245,   5,         0) /* EncumbranceVal */
     , (450245,   8,         10) /* Mass */
     , (450245,   9,   16777216) /* ValidLocations - Held */
     , (450245,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450245,  18,          1) /* UiEffects - Magical */
     , (450245,  19,        20) /* Value */
     , (450245,  33,          1) /* Bonded - Bonded */
     , (450245,  46,        512) /* DefaultCombatStyle - Magic */
     , (450245,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450245,  94,         16) /* TargetType - Creature */
     , (450245, 150,        103) /* HookPlacement - Hook */
     , (450245, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450245,  22, True ) /* Inscribable */
     , (450245,  23, True ) /* DestroyOnSell */
     , (450245,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450245,  12,     0.5) /* Shade */
     , (450245,  29,       1) /* WeaponDefense */
     , (450245,  39,     1.6) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450245,   1, 'Orb of the Bunny Booty') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450245,   1, 0x02000986) /* Setup */
     , (450245,   3, 0x20000014) /* SoundTable */
     , (450245,   6, 0x040001B4) /* PaletteBase */
     , (450245,   7, 0x1000010D) /* ClothingBase */
     , (450245,   8, 0x060016BC) /* Icon */
     , (450245,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450245,  36, 0x0E000016) /* MutateFilter */;
