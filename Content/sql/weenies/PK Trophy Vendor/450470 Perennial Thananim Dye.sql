DELETE FROM `weenie` WHERE `class_Id` = 450470;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450470, 'dyerareeternalfoolproofblacktest', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450470,   1,    4194304) /* ItemType - CraftCookingBase */
     , (450470,   3,         93) /* PaletteTemplate - DyeSpringBlack */
     , (450470,   5,          5) /* EncumbranceVal */
     , (450470,   8,         50) /* Mass */
     , (450470,  11,          1) /* MaxStackSize */
     , (450470,  12,          1) /* StackSize */
     , (450470,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (450470,  17,        166) /* RareId */
     , (450470,  19,          0) /* Value */
     , (450470,  33,         -1) /* Bonded - Slippery */
	 , (450470, 267,      86400) /* Lifespan */
     , (450470,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450470,  94,        134) /* TargetType - Vestements, Misc */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450470,  11, True ) /* IgnoreCollisions */
     , (450470,  13, True ) /* Ethereal */
     , (450470,  14, True ) /* GravityStatus */
     , (450470,  19, True ) /* Attackable */
     , (450470,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450470,   1, 'Perennial Thananim Dye') /* Name */
     , (450470,  14, 'This item is used in cooking.') /* Use */
     , (450470,  15, 'An everlasting pot of charcoal black thananim dye. No matter how many times this dye is used the contents will never diminish.') /* ShortDesc */
     , (450470,  16, 'An everlasting pot of charcoal black thananim dye. No matter how many times this dye is used the contents will never diminish.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450470,   1, 0x02000911) /* Setup */
     , (450470,   3, 0x20000014) /* SoundTable */
     , (450470,   6, 0x04000BEF) /* PaletteBase */
     , (450470,   8, 0x06005B0D) /* Icon */
     , (450470,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450470,  52, 0x06005B0C) /* IconUnderlay */;
