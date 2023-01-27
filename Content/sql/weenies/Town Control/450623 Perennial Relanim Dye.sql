DELETE FROM `weenie` WHERE `class_Id` = 450623;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450623, 'dyerareeternalfoolproofpurplepk', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450623,   1,    4194304) /* ItemType - CraftCookingBase */
     , (450623,   3,         92) /* PaletteTemplate - DyeSpringPurple */
     , (450623,   5,          5) /* EncumbranceVal */
     , (450623,   8,         50) /* Mass */
     , (450623,  11,          1) /* MaxStackSize */
     , (450623,  12,          1) /* StackSize */
     , (450623,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (450623,  17,        168) /* RareId */
     , (450623,  19,          50) /* Value */
     , (450623,  33,         -1) /* Bonded - Slippery */
	 , (450623, 267,      172800) /* Lifespan */
     , (450623,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450623,  94,        134) /* TargetType - Vestements, Misc */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450623,  11, True ) /* IgnoreCollisions */
     , (450623,  13, True ) /* Ethereal */
     , (450623,  14, True ) /* GravityStatus */
     , (450623,  19, True ) /* Attackable */
     , (450623,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450623,   1, 'Perennial Relanim Dye') /* Name */
     , (450623,  14, 'This item is used in cooking.') /* Use */
     , (450623,  15, 'An everlasting pot of deep purple relanim dye. No matter how many times this dye is used the contents will never diminish.') /* ShortDesc */
     , (450623,  16, 'An everlasting pot of deep purple relanim dye. No matter how many times this dye is used the contents will never diminish.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450623,   1, 0x02000911) /* Setup */
     , (450623,   3, 0x20000014) /* SoundTable */
     , (450623,   6, 0x04000BEF) /* PaletteBase */
     , (450623,   8, 0x06005B15) /* Icon */
     , (450623,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450623,  52, 0x06005B0C) /* IconUnderlay */;
