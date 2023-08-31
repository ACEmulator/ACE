DELETE FROM `weenie` WHERE `class_Id` = 30091;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (30091, 'dyerareeternalfoolproofsilver', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (30091,   1,    4194304) /* ItemType - CraftCookingBase */
     , (30091,   3,         90) /* PaletteTemplate - DyeWinterSilver */
     , (30091,   5,          5) /* EncumbranceVal */
     , (30091,   8,         50) /* Mass */
     , (30091,  11,          1) /* MaxStackSize */
     , (30091,  12,          1) /* StackSize */
     , (30091,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (30091,  17,        171) /* RareId */
     , (30091,  19,          250) /* Value */
     , (30091,  33,         -1) /* Bonded - Slippery */
     , (30091,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (30091,  94,        134) /* TargetType - Vestements, Misc */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (30091,  11, True ) /* IgnoreCollisions */
     , (30091,  13, True ) /* Ethereal */
     , (30091,  14, True ) /* GravityStatus */
     , (30091,  19, True ) /* Attackable */
     , (30091,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (30091,   1, 'Perennial Argenory Dye') /* Name */
     , (30091,  14, 'This item is used in cooking.') /* Use */
     , (30091,  15, 'An everlasting pot of silvery white argenory dye. No matter how many times this dye is used the contents will never diminish.') /* ShortDesc */
     , (30091,  16, 'An everlasting pot of silvery white argenory dye. No matter how many times this dye is used the contents will never diminish.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (30091,   1, 0x02000911) /* Setup */
     , (30091,   3, 0x20000014) /* SoundTable */
     , (30091,   6, 0x04000BEF) /* PaletteBase */
     , (30091,   8, 0x06005B16) /* Icon */
     , (30091,  22, 0x3400002B) /* PhysicsEffectTable */
     , (30091,  52, 0x06005B0C) /* IconUnderlay */;
