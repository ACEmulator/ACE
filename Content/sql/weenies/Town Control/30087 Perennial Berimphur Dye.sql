DELETE FROM `weenie` WHERE `class_Id` = 30087;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (30087, 'dyerareeternalfoolproofdarkyellow', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (30087,   1,    4194304) /* ItemType - CraftCookingBase */
     , (30087,   3,         86) /* PaletteTemplate - DyeDarkYellow */
     , (30087,   5,          5) /* EncumbranceVal */
     , (30087,   8,         50) /* Mass */
     , (30087,  11,          1) /* MaxStackSize */
     , (30087,  12,          1) /* StackSize */
     , (30087,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (30087,  17,        165) /* RareId */
     , (30087,  19,          250) /* Value */
     , (30087,  33,         -1) /* Bonded - Slippery */
     , (30087,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (30087,  94,        134) /* TargetType - Vestements, Misc */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (30087,  11, True ) /* IgnoreCollisions */
     , (30087,  13, True ) /* Ethereal */
     , (30087,  14, True ) /* GravityStatus */
     , (30087,  19, True ) /* Attackable */
     , (30087,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (30087,   1, 'Perennial Berimphur Dye') /* Name */
     , (30087,  14, 'This item is used in cooking.') /* Use */
     , (30087,  15, 'An everlasting pot of dark yellow berimphur dye. No matter how many times this dye is used the contents will never diminish.') /* ShortDesc */
     , (30087,  16, 'An everlasting pot of dark yellow berimphur dye. No matter how many times this dye is used the contents will never diminish.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (30087,   1, 0x02000911) /* Setup */
     , (30087,   3, 0x20000014) /* SoundTable */
     , (30087,   6, 0x04000BEF) /* PaletteBase */
     , (30087,   8, 0x06005B12) /* Icon */
     , (30087,  22, 0x3400002B) /* PhysicsEffectTable */
     , (30087,  52, 0x06005B0C) /* IconUnderlay */;
