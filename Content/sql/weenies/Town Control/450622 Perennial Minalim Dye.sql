DELETE FROM `weenie` WHERE `class_Id` = 450622;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450622, 'dyerareeternalfoolprooflightgreenpk', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450622,   1,    4194304) /* ItemType - CraftCookingBase */
     , (450622,   3,         89) /* PaletteTemplate - DyeWinterGreen */
     , (450622,   5,          5) /* EncumbranceVal */
     , (450622,   8,         50) /* Mass */
     , (450622,  11,          1) /* MaxStackSize */
     , (450622,  12,          1) /* StackSize */
     , (450622,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (450622,  17,        170) /* RareId */
     , (450622,  19,          50) /* Value */
     , (450622,  33,         -1) /* Bonded - Slippery */
	 , (450622, 267,      172800) /* Lifespan */
     , (450622,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450622,  94,        134) /* TargetType - Vestements, Misc */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450622,  11, True ) /* IgnoreCollisions */
     , (450622,  13, True ) /* Ethereal */
     , (450622,  14, True ) /* GravityStatus */
     , (450622,  19, True ) /* Attackable */
     , (450622,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450622,   1, 'Perennial Minalim Dye') /* Name */
     , (450622,  14, 'This item is used in cooking.') /* Use */
     , (450622,  15, 'An everlasting pot of winter green minalim dye. No matter how many times this dye is used the contents will never diminish.') /* ShortDesc */
     , (450622,  16, 'An everlasting pot of winter green minalim dye. No matter how many times this dye is used the contents will never diminish.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450622,   1, 0x02000911) /* Setup */
     , (450622,   3, 0x20000014) /* SoundTable */
     , (450622,   6, 0x04000BEF) /* PaletteBase */
     , (450622,   8, 0x06005B14) /* Icon */
     , (450622,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450622,  52, 0x06005B0C) /* IconUnderlay */;
