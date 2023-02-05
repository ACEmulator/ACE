DELETE FROM `weenie` WHERE `class_Id` = 450618;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450618, 'dyerareeternalfoolproofdarkgreenpk', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450618,   1,    4194304) /* ItemType - CraftCookingBase */
     , (450618,   3,         84) /* PaletteTemplate - DyeDarkGreen */
     , (450618,   5,          5) /* EncumbranceVal */
     , (450618,   8,         50) /* Mass */
     , (450618,  11,          1) /* MaxStackSize */
     , (450618,  12,          1) /* StackSize */
     , (450618,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (450618,  17,        163) /* RareId */
     , (450618,  19,          20) /* Value */
     , (450618,  33,         -1) /* Bonded - Slippery */
	 , (450618, 267,      172800) /* Lifespan */
     , (450618,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450618,  94,        134) /* TargetType - Vestements, Misc */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450618,  11, True ) /* IgnoreCollisions */
     , (450618,  13, True ) /* Ethereal */
     , (450618,  14, True ) /* GravityStatus */
     , (450618,  19, True ) /* Attackable */
     , (450618,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450618,   1, 'Perennial Verdalim Dye') /* Name */
     , (450618,  14, 'This item is used in cooking.') /* Use */
     , (450618,  15, 'An everlasting pot of dark green Verdalim dye. No matter how many times this dye is used the contents will never diminish.') /* ShortDesc */
     , (450618,  16, 'An everlasting pot of dark green Verdalim dye. No matter how many times this dye is used the contents will never diminish.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450618,   1, 0x02000911) /* Setup */
     , (450618,   3, 0x20000014) /* SoundTable */
     , (450618,   6, 0x04000BEF) /* PaletteBase */
     , (450618,   8, 0x06005B10) /* Icon */
     , (450618,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450618,  52, 0x06005B0C) /* IconUnderlay */;
