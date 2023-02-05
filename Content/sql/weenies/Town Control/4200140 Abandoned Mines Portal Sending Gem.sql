DELETE FROM `weenie` WHERE `class_Id` = 4200140;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200140, 'tcgemportalabandonedmines', 38, '2005-02-09 10:00:00') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200140,   1,       2048) /* ItemType - Gem */
     , (4200140,   3,         14) /* PaletteTemplate - Red */
     , (4200140,   5,          5) /* EncumbranceVal */
     , (4200140,   8,          5) /* Mass */
     , (4200140,   9,          0) /* ValidLocations - None */
     , (4200140,  11,         25) /* MaxStackSize */
     , (4200140,  12,          1) /* StackSize */
     , (4200140,  13,          5) /* StackUnitEncumbrance */
     , (4200140,  14,          5) /* StackUnitMass */
     , (4200140,  15,          0) /* StackUnitValue */
     , (4200140,  16,          8) /* ItemUseable - Contained */
     , (4200140,  18,          1) /* UiEffects - Magical */
     , (4200140,  19,          1) /* Value */
     , (4200140,  33,          0) /* Bonded - Normal */
     , (4200140,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (4200140,  94,         16) /* TargetType - Creature */
     , (4200140, 106,        210) /* ItemSpellcraft */
     , (4200140, 107,         70) /* ItemCurMana */
     , (4200140, 108,         70) /* ItemMaxMana */
     , (4200140, 109,         40) /* ItemDifficulty */
     , (4200140, 110,          0) /* ItemAllegianceRankLimit */
     , (4200140, 114,          0) /* Attuned - Normal */
     , (4200140, 150,        103) /* HookPlacement - Hook */
     , (4200140, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200140,  15, True ) /* LightsStatus */
     , (4200140,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200140,  76,     0.5) /* Translucency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200140,   1, 'Abandoned Mines Portal Sending Gem') /* Name */
     , (4200140,  14, 'Double Click on this portal gem to transport yourself to the Abandoned Mines.') /* Use */
     , (4200140,  15, 'A glowing red gem.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200140,   1, 0x02000921) /* Setup */
     , (4200140,   3, 0x20000014) /* SoundTable */
     , (4200140,   6, 0x04000BEF) /* PaletteBase */
     , (4200140,   7, 0x1000010B) /* ClothingBase */
     , (4200140,   8, 0x06002370) /* Icon */
     , (4200140,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200140,  28,        157) /* Spell - Summon Primary Portal I */
	 , (4200140,  31,    4200139) /* LinkedPortalOne - Portal to Holtburg */
     , (4200140,  36, 0x0E000016) /* MutateFilter */;
