DELETE FROM `weenie` WHERE `class_Id` = 450740;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450740, 'staffgreatworktailor', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450740,   1,      32768) /* ItemType - Caster */
     , (450740,   3,         83) /* PaletteTemplate - Amber */
     , (450740,   5,        0) /* EncumbranceVal */
     , (450740,   8,         25) /* Mass */
     , (450740,   9,   16777216) /* ValidLocations - Held */
     , (450740,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450740,  18,          1) /* UiEffects - Magical */
     , (450740,  19,          20) /* Value */
     , (450740,  46,        512) /* DefaultCombatStyle - Magic */
     , (450740,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450740,  94,         16) /* TargetType - Creature */
     , (450740, 150,        103) /* HookPlacement - Hook */
     , (450740, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450740,  15, True ) /* LightsStatus */
     , (450740,  22, True ) /* Inscribable */
     , (450740,  23, True ) /* DestroyOnSell */
     , (450740,  69, False) /* IsSellable */
     , (450740,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450740,  29,       1) /* WeaponDefense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450740,   1, 'Great Work Staff of the Lightbringer') /* Name */
     , (450740,  15, 'A trophy from the banishment of Bael''Zharon.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450740,   1, 0x020009CC) /* Setup */
     , (450740,   3, 0x20000014) /* SoundTable */
     , (450740,   6, 0x04000BEF) /* PaletteBase */
     , (450740,   7, 0x10000287) /* ClothingBase */
     , (450740,   8, 0x06001F2E) /* Icon */
     , (450740,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450740,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */
     , (450740,  36, 0x0E000016) /* MutateFilter */;
