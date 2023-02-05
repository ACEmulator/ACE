DELETE FROM `weenie` WHERE `class_Id` = 450624;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450624, 'dyerareeternalfoolproofsilverpk', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450624,   1,    4194304) /* ItemType - CraftCookingBase */
     , (450624,   3,         90) /* PaletteTemplate - DyeWinterSilver */
     , (450624,   5,          5) /* EncumbranceVal */
     , (450624,   8,         50) /* Mass */
     , (450624,  11,          1) /* MaxStackSize */
     , (450624,  12,          1) /* StackSize */
     , (450624,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (450624,  17,        171) /* RareId */
     , (450624,  19,          50) /* Value */
     , (450624,  33,         -1) /* Bonded - Slippery */
	 , (450624, 267,      172800) /* Lifespan */
     , (450624,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450624,  94,        134) /* TargetType - Vestements, Misc */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450624,  11, True ) /* IgnoreCollisions */
     , (450624,  13, True ) /* Ethereal */
     , (450624,  14, True ) /* GravityStatus */
     , (450624,  19, True ) /* Attackable */
     , (450624,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450624,   1, 'Perennial Argenory Dye') /* Name */
     , (450624,  14, 'This item is used in cooking.') /* Use */
     , (450624,  15, 'An everlasting pot of silvery white argenory dye. No matter how many times this dye is used the contents will never diminish.') /* ShortDesc */
     , (450624,  16, 'An everlasting pot of silvery white argenory dye. No matter how many times this dye is used the contents will never diminish.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450624,   1, 0x02000911) /* Setup */
     , (450624,   3, 0x20000014) /* SoundTable */
     , (450624,   6, 0x04000BEF) /* PaletteBase */
     , (450624,   8, 0x06005B16) /* Icon */
     , (450624,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450624,  52, 0x06005B0C) /* IconUnderlay */;
