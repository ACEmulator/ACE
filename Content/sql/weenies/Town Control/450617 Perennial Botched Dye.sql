DELETE FROM `weenie` WHERE `class_Id` = 450617;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450617, 'dyerareeternalfoolproofbotchedpk', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450617,   1,    4194304) /* ItemType - CraftCookingBase */
     , (450617,   3,         87) /* PaletteTemplate - DyeBotched */
     , (450617,   5,          5) /* EncumbranceVal */
     , (450617,   8,         50) /* Mass */
     , (450617,  11,          1) /* MaxStackSize */
     , (450617,  12,          1) /* StackSize */
     , (450617,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (450617,  17,        172) /* RareId */
     , (450617,  19,          50) /* Value */
     , (450617,  33,         -1) /* Bonded - Slippery */
	 , (450617, 267,      172800) /* Lifespan */
     , (450617,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450617,  94,        134) /* TargetType - Vestements, Misc */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450617,  11, True ) /* IgnoreCollisions */
     , (450617,  13, True ) /* Ethereal */
     , (450617,  14, True ) /* GravityStatus */
     , (450617,  19, True ) /* Attackable */
     , (450617,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450617,   1, 'Perennial Botched Dye') /* Name */
     , (450617,  14, 'This item is used in cooking.') /* Use */
     , (450617,  15, 'An everlasting pot of botched dye. No matter how many times this dye is used the contents will never diminish.') /* ShortDesc */
     , (450617,  16, 'An everlasting pot of botched dye. No matter how many times this dye is used the contents will never diminish.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450617,   1, 0x02000911) /* Setup */
     , (450617,   3, 0x20000014) /* SoundTable */
     , (450617,   6, 0x04000BEF) /* PaletteBase */
     , (450617,   8, 0x06005B0F) /* Icon */
     , (450617,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450617,  52, 0x06005B0C) /* IconUnderlay */;
