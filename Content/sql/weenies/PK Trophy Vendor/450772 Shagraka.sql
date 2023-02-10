DELETE FROM `weenie` WHERE `class_Id` = 450772;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450772, 'staffshagrakapk', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450772,   1,      32768) /* ItemType - Caster */
     , (450772,   5,        0) /* EncumbranceVal */
     , (450772,   8,         90) /* Mass */
     , (450772,   9,   16777216) /* ValidLocations - Held */
     , (450772,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450772,  18,          1) /* UiEffects - Magical */
     , (450772,  19,       20) /* Value */
     , (450772,  46,        512) /* DefaultCombatStyle - Magic */
     , (450772,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450772,  94,         16) /* TargetType - Creature */
     , (450772, 150,        103) /* HookPlacement - Hook */
     , (450772, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450772,  22, True ) /* Inscribable */
     , (450772,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450772,   5,  -0.033) /* ManaRate */
     , (450772,  29,       1) /* WeaponDefense */
     , (450772, 144,    0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450772,   1, 'Shagraka') /* Name */
     , (450772,  15, 'This stave is a symbol of the sorcerers of the Shagar Zharala. This particular stave was once the property of the Zharalim traitor Rheth Al'' Thok.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450772,   1, 0x0200102C) /* Setup */
     , (450772,   8, 0x0600305E) /* Icon */
     , (450772,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450772,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */;


