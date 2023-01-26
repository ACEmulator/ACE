DELETE FROM `weenie` WHERE `class_Id` = 450616;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450616, 'dyerareeternalfoolproofbluepk', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450616,   1,    4194304) /* ItemType - CraftCookingBase */
     , (450616,   3,         91) /* PaletteTemplate - DyeSpringBlue */
     , (450616,   5,          5) /* EncumbranceVal */
     , (450616,   8,         50) /* Mass */
     , (450616,  11,          1) /* MaxStackSize */
     , (450616,  12,          1) /* StackSize */
     , (450616,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (450616,  17,        167) /* RareId */
     , (450616,  19,          50) /* Value */
     , (450616,  33,         -1) /* Bonded - Slippery */
	 , (450616, 267,      172800) /* Lifespan */
     , (450616,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450616,  94,        134) /* TargetType - Vestements, Misc */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450616,  11, True ) /* IgnoreCollisions */
     , (450616,  13, True ) /* Ethereal */
     , (450616,  14, True ) /* GravityStatus */
     , (450616,  19, True ) /* Attackable */
     , (450616,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450616,   1, 'Perennial Colban Dye') /* Name */
     , (450616,  14, 'This item is used in cooking.') /* Use */
     , (450616,  15, 'An everlasting pot of royal blue colban dye. No matter how many times this dye is used the contents will never diminish.') /* ShortDesc */
     , (450616,  16, 'An everlasting pot of royal blue colban dye. No matter how many times this dye is used the contents will never diminish.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450616,   1, 0x02000911) /* Setup */
     , (450616,   3, 0x20000014) /* SoundTable */
     , (450616,   6, 0x04000BEF) /* PaletteBase */
     , (450616,   8, 0x06005B0E) /* Icon */
     , (450616,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450616,  52, 0x06005B0C) /* IconUnderlay */;
