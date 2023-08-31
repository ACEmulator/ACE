DELETE FROM `weenie` WHERE `class_Id` = 30086;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (30086, 'dyerareeternalfoolproofdarkred', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (30086,   1,    4194304) /* ItemType - CraftCookingBase */
     , (30086,   3,         85) /* PaletteTemplate - DyeDarkRed */
     , (30086,   5,          5) /* EncumbranceVal */
     , (30086,   8,         50) /* Mass */
     , (30086,  11,          1) /* MaxStackSize */
     , (30086,  12,          1) /* StackSize */
     , (30086,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (30086,  17,        164) /* RareId */
     , (30086,  19,          250) /* Value */
     , (30086,  33,         -1) /* Bonded - Slippery */
     , (30086,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (30086,  94,        134) /* TargetType - Vestements, Misc */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (30086,  11, True ) /* IgnoreCollisions */
     , (30086,  13, True ) /* Ethereal */
     , (30086,  14, True ) /* GravityStatus */
     , (30086,  19, True ) /* Attackable */
     , (30086,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (30086,   1, 'Perennial Hennacin Dye') /* Name */
     , (30086,  14, 'This item is used in cooking.') /* Use */
     , (30086,  15, 'An everlasting pot of dark red hennacin dye. No matter how many times this dye is used the contents will never diminish.') /* ShortDesc */
     , (30086,  16, 'An everlasting pot of dark red hennacin dye. No matter how many times this dye is used the contents will never diminish.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (30086,   1, 0x02000911) /* Setup */
     , (30086,   3, 0x20000014) /* SoundTable */
     , (30086,   6, 0x04000BEF) /* PaletteBase */
     , (30086,   8, 0x06005B11) /* Icon */
     , (30086,  22, 0x3400002B) /* PhysicsEffectTable */
     , (30086,  52, 0x06005B0C) /* IconUnderlay */;
