DELETE FROM `weenie` WHERE `class_Id` = 450619;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450619, 'dyerareeternalfoolproofdarkredpk', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450619,   1,    4194304) /* ItemType - CraftCookingBase */
     , (450619,   3,         85) /* PaletteTemplate - DyeDarkRed */
     , (450619,   5,          5) /* EncumbranceVal */
     , (450619,   8,         50) /* Mass */
     , (450619,  11,          1) /* MaxStackSize */
     , (450619,  12,          1) /* StackSize */
     , (450619,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (450619,  17,        164) /* RareId */
     , (450619,  19,          50) /* Value */
     , (450619,  33,         -1) /* Bonded - Slippery */
	 , (450619, 267,      172800) /* Lifespan */
     , (450619,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450619,  94,        134) /* TargetType - Vestements, Misc */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450619,  11, True ) /* IgnoreCollisions */
     , (450619,  13, True ) /* Ethereal */
     , (450619,  14, True ) /* GravityStatus */
     , (450619,  19, True ) /* Attackable */
     , (450619,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450619,   1, 'Perennial Hennacin Dye') /* Name */
     , (450619,  14, 'This item is used in cooking.') /* Use */
     , (450619,  15, 'An everlasting pot of dark red hennacin dye. No matter how many times this dye is used the contents will never diminish.') /* ShortDesc */
     , (450619,  16, 'An everlasting pot of dark red hennacin dye. No matter how many times this dye is used the contents will never diminish.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450619,   1, 0x02000911) /* Setup */
     , (450619,   3, 0x20000014) /* SoundTable */
     , (450619,   6, 0x04000BEF) /* PaletteBase */
     , (450619,   8, 0x06005B11) /* Icon */
     , (450619,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450619,  52, 0x06005B0C) /* IconUnderlay */;
