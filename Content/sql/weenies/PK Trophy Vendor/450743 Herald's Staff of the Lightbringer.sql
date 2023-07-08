DELETE FROM `weenie` WHERE `class_Id` = 450743;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450743, 'staffheraldtailor', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450743,   1,      32768) /* ItemType - Caster */
     , (450743,   3,         39) /* PaletteTemplate - Black */
     , (450743,   5,        0) /* EncumbranceVal */
     , (450743,   8,         25) /* Mass */
     , (450743,   9,   16777216) /* ValidLocations - Held */
     , (450743,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450743,  18,          1) /* UiEffects - Magical */
     , (450743,  19,          20) /* Value */
     , (450743,  46,        512) /* DefaultCombatStyle - Magic */
     , (450743,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450743, 150,        103) /* HookPlacement - Hook */
     , (450743, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450743,  15, True ) /* LightsStatus */
     , (450743,  22, True ) /* Inscribable */
     , (450743,  23, True ) /* DestroyOnSell */
     , (450743,  69, False) /* IsSellable */
     , (450743,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450743,  29,       1) /* WeaponDefense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450743,   1, 'Herald''s Staff of the Lightbringer') /* Name */
     , (450743,  15, 'A trophy from the banishment of Bael''Zharon.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450743,   1, 0x020009CC) /* Setup */
     , (450743,   3, 0x20000014) /* SoundTable */
     , (450743,   6, 0x04000BEF) /* PaletteBase */
     , (450743,   7, 0x10000287) /* ClothingBase */
     , (450743,   8, 0x06001F2F) /* Icon */
     , (450743,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450743,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */
     , (450743,  36, 0x0E000016) /* MutateFilter */;
