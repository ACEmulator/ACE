DELETE FROM `weenie` WHERE `class_Id` = 450741;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450741, 'staffnexustailor', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450741,   1,      32768) /* ItemType - Caster */
     , (450741,   3,          2) /* PaletteTemplate - Blue */
     , (450741,   5,        0) /* EncumbranceVal */
     , (450741,   8,         25) /* Mass */
     , (450741,   9,   16777216) /* ValidLocations - Held */
     , (450741,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450741,  18,          1) /* UiEffects - Magical */
     , (450741,  19,          20) /* Value */
     , (450741,  46,        512) /* DefaultCombatStyle - Magic */
     , (450741,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450741,  94,         16) /* TargetType - Creature */
     , (450741, 150,        103) /* HookPlacement - Hook */
     , (450741, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450741,  15, True ) /* LightsStatus */
     , (450741,  22, True ) /* Inscribable */
     , (450741,  23, True ) /* DestroyOnSell */
     , (450741,  69, False) /* IsSellable */
     , (450741,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450741,  29,       1) /* WeaponDefense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450741,   1, 'Nexus Staff of the Lightbringer') /* Name */
     , (450741,  15, 'A trophy from the banishment of Bael''Zharon.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450741,   1, 0x020009CC) /* Setup */
     , (450741,   3, 0x20000014) /* SoundTable */
     , (450741,   6, 0x04000BEF) /* PaletteBase */
     , (450741,   7, 0x10000287) /* ClothingBase */
     , (450741,   8, 0x06001F30) /* Icon */
     , (450741,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450741,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */
     , (450741,  36, 0x0E000016) /* MutateFilter */;
