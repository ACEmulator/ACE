DELETE FROM `weenie` WHERE `class_Id` = 450252;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450252, 'ace450252-orbofeternalfrosttailor', 35, '2021-11-17 16:56:08') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450252,   1,      32768) /* ItemType - Caster */
     , (450252,   5,         0) /* EncumbranceVal */
     , (450252,   9,   16777216) /* ValidLocations - Held */
     , (450252,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450252,  18,          1) /* UiEffects - Magical */
     , (450252,  19,       20) /* Value */
     , (450252,  46,        512) /* DefaultCombatStyle - Magic */
     , (450252,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450252,  94,         16) /* TargetType - Creature */
     , (450252, 150,        103) /* HookPlacement - Hook */
     , (450252, 151,          2) /* HookType - Wall */
     , (450252, 353,          0) /* WeaponType - Undef */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450252,  11, True ) /* IgnoreCollisions */
     , (450252,  13, True ) /* Ethereal */
     , (450252,  14, True ) /* GravityStatus */
     , (450252,  15, True ) /* LightsStatus */
     , (450252,  19, True ) /* Attackable */
     , (450252,  22, True ) /* Inscribable */
     , (450252,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450252,  29,       1) /* WeaponDefense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450252,   1, 'Orb of Eternal Frost') /* Name */
     , (450252,  16, 'A frozen orb.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450252,   1, 0x02001661) /* Setup */
     , (450252,   3, 0x20000014) /* SoundTable */
     , (450252,   6, 0x04000BEF) /* PaletteBase */
     , (450252,   8, 0x060062BF) /* Icon */
     , (450252,  22, 0x3400002B) /* PhysicsEffectTable */;
