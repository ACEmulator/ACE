DELETE FROM `weenie` WHERE `class_Id` = 30084;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (30084, 'dyerareeternalfoolproofbotched', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (30084,   1,    4194304) /* ItemType - CraftCookingBase */
     , (30084,   3,         87) /* PaletteTemplate - DyeBotched */
     , (30084,   5,          5) /* EncumbranceVal */
     , (30084,   8,         50) /* Mass */
     , (30084,  11,          1) /* MaxStackSize */
     , (30084,  12,          1) /* StackSize */
     , (30084,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (30084,  17,        172) /* RareId */
     , (30084,  19,          250) /* Value */
     , (30084,  33,         -1) /* Bonded - Slippery */
     , (30084,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (30084,  94,        134) /* TargetType - Vestements, Misc */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (30084,  11, True ) /* IgnoreCollisions */
     , (30084,  13, True ) /* Ethereal */
     , (30084,  14, True ) /* GravityStatus */
     , (30084,  19, True ) /* Attackable */
     , (30084,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (30084,   1, 'Perennial Botched Dye') /* Name */
     , (30084,  14, 'This item is used in cooking.') /* Use */
     , (30084,  15, 'An everlasting pot of botched dye. No matter how many times this dye is used the contents will never diminish.') /* ShortDesc */
     , (30084,  16, 'An everlasting pot of botched dye. No matter how many times this dye is used the contents will never diminish.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (30084,   1, 0x02000911) /* Setup */
     , (30084,   3, 0x20000014) /* SoundTable */
     , (30084,   6, 0x04000BEF) /* PaletteBase */
     , (30084,   8, 0x06005B0F) /* Icon */
     , (30084,  22, 0x3400002B) /* PhysicsEffectTable */
     , (30084,  52, 0x06005B0C) /* IconUnderlay */;
