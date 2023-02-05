DELETE FROM `weenie` WHERE `class_Id` = 4200141;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200141, 'tcgemportaldarkisle', 38, '2005-02-09 10:00:00') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200141,   1,       2048) /* ItemType - Gem */
     , (4200141,   3,         14) /* PaletteTemplate - Red */
     , (4200141,   5,          5) /* EncumbranceVal */
     , (4200141,   8,          5) /* Mass */
     , (4200141,   9,          0) /* ValidLocations - None */
     , (4200141,  16,          8) /* ItemUseable - Contained */
     , (4200141,  18,          1) /* UiEffects - Magical */
     , (4200141,  19,          250) /* Value */
     , (4200141,  33,          0) /* Bonded - Normal */
     , (4200141,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (4200141,  94,         16) /* TargetType - Creature */
     , (4200141, 106,        210) /* ItemSpellcraft */
     , (4200141, 107,         70) /* ItemCurMana */
     , (4200141, 108,         70) /* ItemMaxMana */
     , (4200141, 109,         40) /* ItemDifficulty */
     , (4200141, 110,          0) /* ItemAllegianceRankLimit */
     , (4200141, 114,          0) /* Attuned - Normal */
     , (4200141, 150,        103) /* HookPlacement - Hook */
     , (4200141, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200141,  15, True ) /* LightsStatus */
     , (4200141,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200141,  76,     0.5) /* Translucency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200141,   1, 'Dark Isle Portal Sending Gem') /* Name */
     , (4200141,  14, 'Double Click on this portal gem to transport yourself to Dark Isle.') /* Use */
     , (4200141,  15, 'A glowing red gem.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200141,   1, 0x02000921) /* Setup */
     , (4200141,   3, 0x20000014) /* SoundTable */
     , (4200141,   6, 0x04000BEF) /* PaletteBase */
     , (4200141,   7, 0x1000010B) /* ClothingBase */
     , (4200141,   8, 0x06002370) /* Icon */
     , (4200141,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200141,  28,        157) /* Spell - Summon Primary Portal I */
	 , (4200141,  31,    1910225) /* LinkedPortalOne - Portal to Holtburg */
     , (4200141,  36, 0x0E000016) /* MutateFilter */;
