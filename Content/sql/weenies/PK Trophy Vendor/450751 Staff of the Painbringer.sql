DELETE FROM `weenie` WHERE `class_Id` = 450751;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450751, 'staffpainbringertailor', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450751,   1,      32768) /* ItemType - Caster */
     , (450751,   3,         53) /* PaletteTemplate - BlueDullSilver */
     , (450751,   5,         0) /* EncumbranceVal */
     , (450751,   8,         60) /* Mass */
     , (450751,   9,   16777216) /* ValidLocations - Held */
     , (450751,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450751,  18,          1) /* UiEffects - Magical */
     , (450751,  19,       20) /* Value */
     , (450751,  46,        512) /* DefaultCombatStyle - Magic */
     , (450751,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450751,  94,         16) /* TargetType - Creature */
     , (450751, 150,        103) /* HookPlacement - Hook */
     , (450751, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450751,  15, True ) /* LightsStatus */
     , (450751,  22, True ) /* Inscribable */
     , (450751,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450751,   5,  -0.033) /* ManaRate */
     , (450751,  29,    1.06) /* WeaponDefense */
     , (450751, 144,    0.06) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450751,   1, 'Staff of the Painbringer') /* Name */
     , (450751,  16, 'The head of the Painbringer is mounted atop this large, magical battle staff.  The craftsmanship is superb -- one would hardly guess that it was made by a tailor!') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450751,   1, 0x02001092) /* Setup */
     , (450751,   3, 0x20000014) /* SoundTable */
     , (450751,   6, 0x0400102F) /* PaletteBase */
     , (450751,   7, 0x1000025B) /* ClothingBase */
     , (450751,   8, 0x06003327) /* Icon */
     , (450751,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450751,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */;


