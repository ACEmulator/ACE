DELETE FROM `weenie` WHERE `class_Id` = 4200020;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200020, 'burningblunt', 38, '2005-02-09 10:00:00') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200020,   1,        128) /* ItemType - Misc */
     , (4200020,   5,         75) /* EncumbranceVal */
     , (4200020,   8,         75) /* Mass */
     , (4200020,  16,          8) /* ItemUseable - Contained */
     , (4200020,  18,        256) /* UiEffects - Acid */
     , (4200020,  19,         5) /* Value */
     , (4200020,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200020,  94,         16) /* TargetType - Creature */
     , (4200020, 106,        150) /* ItemSpellcraft */
     , (4200020, 107,         50) /* ItemCurMana */
     , (4200020, 108,         50) /* ItemMaxMana */
     , (4200020, 109,        200) /* ItemDifficulty */
     , (4200020, 114,          1) /* Attuned - Attuned */
     , (4200020,  33,          1) /* Bonded - Bonded */
     , (4200020, 267,     172800) /* Lifespan */
     , (4200020, 280,          3) /* SharedCooldown */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200020,  23, True ) /* DestroyOnSell */
     , (4200020,  63,      True ) /* UnlimitedUse */;

     INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200020,  39,     0.4) /* DefaultScale */
     , (4200020, 167,      30) /* CooldownDuration */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200020,   1, 'Burning Blunt') /* Name */
     , (4200020,  14, 'Hit this Burning Blunt to get the highest of high') /* Use */
     , (4200020,  16, 'A Burning Blunt, inside smolders the sticky icky, Blaze one with your boys!') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200020,   1, 0x02000181) /* Setup */
     , (4200020,   3, 0x20000014) /* SoundTable */
     , (4200020,   6, 0x04000BEF) /* PaletteBase */
     , (4200020,   7, 0x10000178) /* ClothingBase */
     , (4200020,   8, 0x060030E4) /* Icon */
     , (4200020,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200020,  28,       6340) /* Spell - Gauntlet Vitality III */
     , (4200020,  52,  100687785) /* IconUnderlay */;
