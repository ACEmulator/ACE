DELETE FROM `weenie` WHERE `class_Id` = 450621;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450621, 'dyerareeternalfoolprooflightbluepk', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450621,   1,    4194304) /* ItemType - CraftCookingBase */
     , (450621,   3,         88) /* PaletteTemplate - DyeWinterBlue */
     , (450621,   5,          5) /* EncumbranceVal */
     , (450621,   8,         50) /* Mass */
     , (450621,  11,          1) /* MaxStackSize */
     , (450621,  12,          1) /* StackSize */
     , (450621,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (450621,  17,        169) /* RareId */
     , (450621,  19,          50) /* Value */
     , (450621,  33,         -1) /* Bonded - Slippery */
	 , (450621, 267,      172800) /* Lifespan */
     , (450621,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450621,  94,        134) /* TargetType - Vestements, Misc */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450621,  11, True ) /* IgnoreCollisions */
     , (450621,  13, True ) /* Ethereal */
     , (450621,  14, True ) /* GravityStatus */
     , (450621,  19, True ) /* Attackable */
     , (450621,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450621,   1, 'Perennial Lapyan Dye') /* Name */
     , (450621,  14, 'This item is used in cooking.') /* Use */
     , (450621,  15, 'An everlasting pot of fair blue lapyan dye. No matter how many times this dye is used the contents will never diminish.') /* ShortDesc */
     , (450621,  16, 'An everlasting pot of fair blue lapyan dye. No matter how many times this dye is used the contents will never diminish.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450621,   1, 0x02000911) /* Setup */
     , (450621,   3, 0x20000014) /* SoundTable */
     , (450621,   6, 0x04000BEF) /* PaletteBase */
     , (450621,   8, 0x06005B13) /* Icon */
     , (450621,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450621,  52, 0x06005B0C) /* IconUnderlay */;
