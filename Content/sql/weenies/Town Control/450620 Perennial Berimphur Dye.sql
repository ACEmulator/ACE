DELETE FROM `weenie` WHERE `class_Id` = 450620;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450620, 'dyerareeternalfoolproofdarkyellowpk', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450620,   1,    4194304) /* ItemType - CraftCookingBase */
     , (450620,   3,         86) /* PaletteTemplate - DyeDarkYellow */
     , (450620,   5,          5) /* EncumbranceVal */
     , (450620,   8,         50) /* Mass */
     , (450620,  11,          1) /* MaxStackSize */
     , (450620,  12,          1) /* StackSize */
     , (450620,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (450620,  17,        165) /* RareId */
     , (450620,  19,          50) /* Value */
     , (450620,  33,         -1) /* Bonded - Slippery */
	 , (450620, 267,      172800) /* Lifespan */
     , (450620,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450620,  94,        134) /* TargetType - Vestements, Misc */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450620,  11, True ) /* IgnoreCollisions */
     , (450620,  13, True ) /* Ethereal */
     , (450620,  14, True ) /* GravityStatus */
     , (450620,  19, True ) /* Attackable */
     , (450620,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450620,   1, 'Perennial Berimphur Dye') /* Name */
     , (450620,  14, 'This item is used in cooking.') /* Use */
     , (450620,  15, 'An everlasting pot of dark yellow berimphur dye. No matter how many times this dye is used the contents will never diminish.') /* ShortDesc */
     , (450620,  16, 'An everlasting pot of dark yellow berimphur dye. No matter how many times this dye is used the contents will never diminish.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450620,   1, 0x02000911) /* Setup */
     , (450620,   3, 0x20000014) /* SoundTable */
     , (450620,   6, 0x04000BEF) /* PaletteBase */
     , (450620,   8, 0x06005B12) /* Icon */
     , (450620,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450620,  52, 0x06005B0C) /* IconUnderlay */;
