DELETE FROM `weenie` WHERE `class_Id` = 30085;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (30085, 'dyerareeternalfoolproofdarkgreen', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (30085,   1,    4194304) /* ItemType - CraftCookingBase */
     , (30085,   3,         84) /* PaletteTemplate - DyeDarkGreen */
     , (30085,   5,          5) /* EncumbranceVal */
     , (30085,   8,         50) /* Mass */
     , (30085,  11,          1) /* MaxStackSize */
     , (30085,  12,          1) /* StackSize */
     , (30085,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (30085,  17,        163) /* RareId */
     , (30085,  19,          250) /* Value */
     , (30085,  33,         -1) /* Bonded - Slippery */
     , (30085,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (30085,  94,        134) /* TargetType - Vestements, Misc */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (30085,  11, True ) /* IgnoreCollisions */
     , (30085,  13, True ) /* Ethereal */
     , (30085,  14, True ) /* GravityStatus */
     , (30085,  19, True ) /* Attackable */
     , (30085,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (30085,   1, 'Perennial Verdalim Dye') /* Name */
     , (30085,  14, 'This item is used in cooking.') /* Use */
     , (30085,  15, 'An everlasting pot of dark green Verdalim dye. No matter how many times this dye is used the contents will never diminish.') /* ShortDesc */
     , (30085,  16, 'An everlasting pot of dark green Verdalim dye. No matter how many times this dye is used the contents will never diminish.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (30085,   1, 0x02000911) /* Setup */
     , (30085,   3, 0x20000014) /* SoundTable */
     , (30085,   6, 0x04000BEF) /* PaletteBase */
     , (30085,   8, 0x06005B10) /* Icon */
     , (30085,  22, 0x3400002B) /* PhysicsEffectTable */
     , (30085,  52, 0x06005B0C) /* IconUnderlay */;
