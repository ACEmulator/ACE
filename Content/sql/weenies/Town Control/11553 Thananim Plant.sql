DELETE FROM `weenie` WHERE `class_Id` = 11553;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (11553, 'plantspringblack', 51, '2005-02-09 10:00:00') /* Stackable */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (11553,   1,    4194304) /* ItemType - CraftCookingBase */
     , (11553,   3,         93) /* PaletteTemplate - DyeSpringBlack */
     , (11553,   5,          5) /* EncumbranceVal */
     , (11553,   8,         50) /* Mass */
     , (11553,   9,          0) /* ValidLocations - None */
     , (11553,  11,        100) /* MaxStackSize */
     , (11553,  12,          1) /* StackSize */
     , (11553,  13,          5) /* StackUnitEncumbrance */
     , (11553,  14,         50) /* StackUnitMass */
     , (11553,  15,         1) /* StackUnitValue */
     , (11553,  16,          1) /* ItemUseable - No */
     , (11553,  19,         1) /* Value */
     , (11553,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (11553, 150,        103) /* HookPlacement - Hook */
     , (11553, 151,          9) /* HookType - Floor, Yard */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (11553,  39,     1.2) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (11553,   1, 'Thananim Plant') /* Name */
     , (11553,  15, 'A charcoal Thananim plant.') /* ShortDesc */
     , (11553,  16, 'A charcoal black Thananim plant. ') /* LongDesc */
     , (11553,  20, 'Thananim Plants') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (11553,   1, 0x0200090E) /* Setup */
     , (11553,   3, 0x20000014) /* SoundTable */
     , (11553,   6, 0x04000BEF) /* PaletteBase */
     , (11553,   7, 0x10000241) /* ClothingBase */
     , (11553,   8, 0x06001D2F) /* Icon */
     , (11553,  22, 0x3400002B) /* PhysicsEffectTable */;
