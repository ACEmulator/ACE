DELETE FROM `weenie` WHERE `class_Id` = 11555;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (11555, 'plantspringpurple', 51, '2005-02-09 10:00:00') /* Stackable */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (11555,   1,    4194304) /* ItemType - CraftCookingBase */
     , (11555,   3,         92) /* PaletteTemplate - DyeSpringPurple */
     , (11555,   5,          5) /* EncumbranceVal */
     , (11555,   8,         50) /* Mass */
     , (11555,   9,          0) /* ValidLocations - None */
     , (11555,  11,        100) /* MaxStackSize */
     , (11555,  12,          1) /* StackSize */
     , (11555,  13,          5) /* StackUnitEncumbrance */
     , (11555,  14,         50) /* StackUnitMass */
     , (11555,  15,         1) /* StackUnitValue */
     , (11555,  16,          1) /* ItemUseable - No */
     , (11555,  19,         1) /* Value */
     , (11555,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (11555, 150,        103) /* HookPlacement - Hook */
     , (11555, 151,          9) /* HookType - Floor, Yard */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (11555,  39,     1.2) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (11555,   1, 'Relanim Plant') /* Name */
     , (11555,  15, 'A deep purple Relanim plant.') /* ShortDesc */
     , (11555,  16, 'A deep purple Relanim plant. ') /* LongDesc */
     , (11555,  20, 'Relanim Plants') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (11555,   1, 0x0200090E) /* Setup */
     , (11555,   3, 0x20000014) /* SoundTable */
     , (11555,   6, 0x04000BEF) /* PaletteBase */
     , (11555,   7, 0x10000241) /* ClothingBase */
     , (11555,   8, 0x06001D2F) /* Icon */
     , (11555,  22, 0x3400002B) /* PhysicsEffectTable */;
