DELETE FROM `weenie` WHERE `class_Id` = 30089;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (30089, 'dyerareeternalfoolprooflightgreen', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (30089,   1,    4194304) /* ItemType - CraftCookingBase */
     , (30089,   3,         89) /* PaletteTemplate - DyeWinterGreen */
     , (30089,   5,          5) /* EncumbranceVal */
     , (30089,   8,         50) /* Mass */
     , (30089,  11,          1) /* MaxStackSize */
     , (30089,  12,          1) /* StackSize */
     , (30089,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (30089,  17,        170) /* RareId */
     , (30089,  19,          250) /* Value */
     , (30089,  33,         -1) /* Bonded - Slippery */
     , (30089,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (30089,  94,        134) /* TargetType - Vestements, Misc */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (30089,  11, True ) /* IgnoreCollisions */
     , (30089,  13, True ) /* Ethereal */
     , (30089,  14, True ) /* GravityStatus */
     , (30089,  19, True ) /* Attackable */
     , (30089,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (30089,   1, 'Perennial Minalim Dye') /* Name */
     , (30089,  14, 'This item is used in cooking.') /* Use */
     , (30089,  15, 'An everlasting pot of winter green minalim dye. No matter how many times this dye is used the contents will never diminish.') /* ShortDesc */
     , (30089,  16, 'An everlasting pot of winter green minalim dye. No matter how many times this dye is used the contents will never diminish.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (30089,   1, 0x02000911) /* Setup */
     , (30089,   3, 0x20000014) /* SoundTable */
     , (30089,   6, 0x04000BEF) /* PaletteBase */
     , (30089,   8, 0x06005B14) /* Icon */
     , (30089,  22, 0x3400002B) /* PhysicsEffectTable */
     , (30089,  52, 0x06005B0C) /* IconUnderlay */;
