DELETE FROM `weenie` WHERE `class_Id` = 450766;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450766, 'staffshendolaintailor', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450766,   1,      32768) /* ItemType - Caster */
     , (450766,   3,         13) /* PaletteTemplate - Purple */
     , (450766,   5,        0) /* EncumbranceVal */
     , (450766,   8,         25) /* Mass */
     , (450766,   9,   16777216) /* ValidLocations - Held */
     , (450766,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450766,  18,          1) /* UiEffects - Magical */
     , (450766,  19,          20) /* Value */
     , (450766,  46,        512) /* DefaultCombatStyle - Magic */
     , (450766,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450766,  94,         16) /* TargetType - Creature */
     , (450766, 114,          1) /* Attuned - Attuned */
     , (450766, 150,        103) /* HookPlacement - Hook */
     , (450766, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450766,  15, True ) /* LightsStatus */
     , (450766,  22, True ) /* Inscribable */
     , (450766,  23, True ) /* DestroyOnSell */
     , (450766,  69, False) /* IsSellable */
     , (450766,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450766,  12,     0.2) /* Shade */
     , (450766,  29,       1) /* WeaponDefense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450766,   1, 'Shendolain Staff of the Lightbringer') /* Name */
     , (450766,  15, 'A trophy from the banishment of Bael''Zharon.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450766,   1, 0x020009CC) /* Setup */
     , (450766,   3, 0x20000014) /* SoundTable */
     , (450766,   6, 0x04000BEF) /* PaletteBase */
     , (450766,   7, 0x10000287) /* ClothingBase */
     , (450766,   8, 0x06001F31) /* Icon */
     , (450766,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450766,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */
     , (450766,  36, 0x0E000016) /* MutateFilter */;
