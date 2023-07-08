DELETE FROM `weenie` WHERE `class_Id` = 450716;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450716, 'staffmagic345menhir-xptailor', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450716,   1,      32768) /* ItemType - Caster */
     , (450716,   5,        0) /* EncumbranceVal */
     , (450716,   8,        200) /* Mass */
     , (450716,   9,   16777216) /* ValidLocations - Held */
     , (450716,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450716,  18,          1) /* UiEffects - Magical */
     , (450716,  19,          20) /* Value */
     , (450716,  46,        512) /* DefaultCombatStyle - Magic */
     , (450716,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450716,  94,         16) /* TargetType - Creature */
     , (450716, 150,        103) /* HookPlacement - Hook */
     , (450716, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450716,  22, True ) /* Inscribable */
     , (450716,  23, True ) /* DestroyOnSell */
     , (450716,  69, False) /* IsSellable */
     , (450716,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450716,   5,   -0.05) /* ManaRate */
     , (450716,  29,       1) /* WeaponDefense */
     , (450716, 144,    0.07) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450716,   1, 'Stave of Palenqual') /* Name */
     , (450716,  16, 'The Stave of Palenqual, an Aun Tumerok relic. This magic caster was given to the Aun xuta by spirits called the Deru. It is the embodiment of Marae Lassel''s spirit - a single great totem for the island as a whole. Three Tumerok fetishes are attached to this weapon; those of Siraluun, Storm, and Tonk.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450716,   1, 0x02000AF0) /* Setup */
     , (450716,   3, 0x20000014) /* SoundTable */
     , (450716,   6, 0x04000BEF) /* PaletteBase */
     , (450716,   8, 0x0600217C) /* Icon */
     , (450716,  22, 0x3400002B) /* PhysicsEffectTable */;

