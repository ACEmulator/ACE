DELETE FROM `weenie` WHERE `class_Id` = 30082;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (30082, 'dyerareeternalfoolproofblack', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (30082,   1,    4194304) /* ItemType - CraftCookingBase */
     , (30082,   3,         93) /* PaletteTemplate - DyeSpringBlack */
     , (30082,   5,          5) /* EncumbranceVal */
     , (30082,   8,         50) /* Mass */
     , (30082,  11,          1) /* MaxStackSize */
     , (30082,  12,          1) /* StackSize */
     , (30082,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (30082,  17,        166) /* RareId */
     , (30082,  19,          250) /* Value */
     , (30082,  33,         -1) /* Bonded - Slippery */
     , (30082,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (30082,  94,        134) /* TargetType - Vestements, Misc */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (30082,  11, True ) /* IgnoreCollisions */
     , (30082,  13, True ) /* Ethereal */
     , (30082,  14, True ) /* GravityStatus */
     , (30082,  19, True ) /* Attackable */
     , (30082,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (30082,   1, 'Perennial Thananim Dye') /* Name */
     , (30082,  14, 'This item is used in cooking.') /* Use */
     , (30082,  15, 'An everlasting pot of charcoal black thananim dye. No matter how many times this dye is used the contents will never diminish.') /* ShortDesc */
     , (30082,  16, 'An everlasting pot of charcoal black thananim dye. No matter how many times this dye is used the contents will never diminish.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (30082,   1, 0x02000911) /* Setup */
     , (30082,   3, 0x20000014) /* SoundTable */
     , (30082,   6, 0x04000BEF) /* PaletteBase */
     , (30082,   8, 0x06005B0D) /* Icon */
     , (30082,  22, 0x3400002B) /* PhysicsEffectTable */
     , (30082,  52, 0x06005B0C) /* IconUnderlay */;
