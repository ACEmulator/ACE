DELETE FROM `weenie` WHERE `class_Id` = 450615;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450615, 'dyerareeternalfoolproofblackpk', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450615,   1,    4194304) /* ItemType - CraftCookingBase */
     , (450615,   3,         93) /* PaletteTemplate - DyeSpringBlack */
     , (450615,   5,          5) /* EncumbranceVal */
     , (450615,   8,         50) /* Mass */
     , (450615,  11,          1) /* MaxStackSize */
     , (450615,  12,          1) /* StackSize */
     , (450615,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (450615,  17,        166) /* RareId */
     , (450615,  19,          50) /* Value */
     , (450615,  33,         -1) /* Bonded - Slippery */
	 , (450615, 267,      172800) /* Lifespan */
     , (450615,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450615,  94,        134) /* TargetType - Vestements, Misc */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450615,  11, True ) /* IgnoreCollisions */
     , (450615,  13, True ) /* Ethereal */
     , (450615,  14, True ) /* GravityStatus */
     , (450615,  19, True ) /* Attackable */
     , (450615,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450615,   1, 'Perennial Thananim Dye') /* Name */
     , (450615,  14, 'This item is used in cooking.') /* Use */
     , (450615,  15, 'An everlasting pot of charcoal black thananim dye. No matter how many times this dye is used the contents will never diminish.') /* ShortDesc */
     , (450615,  16, 'An everlasting pot of charcoal black thananim dye. No matter how many times this dye is used the contents will never diminish.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450615,   1, 0x02000911) /* Setup */
     , (450615,   3, 0x20000014) /* SoundTable */
     , (450615,   6, 0x04000BEF) /* PaletteBase */
     , (450615,   8, 0x06005B0D) /* Icon */
     , (450615,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450615,  52, 0x06005B0C) /* IconUnderlay */;
