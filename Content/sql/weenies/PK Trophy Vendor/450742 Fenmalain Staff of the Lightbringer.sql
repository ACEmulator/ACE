DELETE FROM `weenie` WHERE `class_Id` = 450742;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450742, 'stafffenmalaintailor', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450742,   1,      32768) /* ItemType - Caster */
     , (450742,   3,         13) /* PaletteTemplate - Purple */
     , (450742,   5,        0) /* EncumbranceVal */
     , (450742,   8,         25) /* Mass */
     , (450742,   9,   16777216) /* ValidLocations - Held */
     , (450742,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450742,  18,          1) /* UiEffects - Magical */
     , (450742,  19,          20) /* Value */
     , (450742,  46,        512) /* DefaultCombatStyle - Magic */
     , (450742,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450742, 150,        103) /* HookPlacement - Hook */
     , (450742, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450742,  15, True ) /* LightsStatus */
     , (450742,  22, True ) /* Inscribable */
     , (450742,  23, True ) /* DestroyOnSell */
     , (450742,  69, False) /* IsSellable */
     , (450742,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450742,  12,     0.9) /* Shade */
     , (450742,  29,       1) /* WeaponDefense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450742,   1, 'Fenmalain Staff of the Lightbringer') /* Name */
     , (450742,  15, 'A trophy from the banishment of Bael''Zharon.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450742,   1, 0x020009CC) /* Setup */
     , (450742,   3, 0x20000014) /* SoundTable */
     , (450742,   6, 0x04000BEF) /* PaletteBase */
     , (450742,   7, 0x10000287) /* ClothingBase */
     , (450742,   8, 0x06001F2D) /* Icon */
     , (450742,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450742,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */
     , (450742,  36, 0x0E000016) /* MutateFilter */;
