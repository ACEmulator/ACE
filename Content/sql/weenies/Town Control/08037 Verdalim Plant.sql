DELETE FROM `weenie` WHERE `class_Id` = 8037;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (8037, 'plantdarkgreen', 51, '2005-02-09 10:00:00') /* Stackable */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (8037,   1,    4194304) /* ItemType - CraftCookingBase */
     , (8037,   3,         84) /* PaletteTemplate - DyeDarkGreen */
     , (8037,   5,          5) /* EncumbranceVal */
     , (8037,   8,         50) /* Mass */
     , (8037,   9,          0) /* ValidLocations - None */
     , (8037,  11,        100) /* MaxStackSize */
     , (8037,  12,          1) /* StackSize */
     , (8037,  13,          5) /* StackUnitEncumbrance */
     , (8037,  14,         50) /* StackUnitMass */
     , (8037,  15,         1) /* StackUnitValue */
     , (8037,  16,          1) /* ItemUseable - No */
     , (8037,  19,         1) /* Value */
     , (8037,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (8037, 150,        103) /* HookPlacement - Hook */
     , (8037, 151,          9) /* HookType - Floor, Yard */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (8037,  39,     1.2) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (8037,   1, 'Verdalim Plant') /* Name */
     , (8037,  15, 'A dark green verdalim plant.') /* ShortDesc */
     , (8037,  16, 'A dark green verdalim plant. ') /* LongDesc */
     , (8037,  20, 'Verdalim Plants') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (8037,   1, 0x0200090E) /* Setup */
     , (8037,   3, 0x20000014) /* SoundTable */
     , (8037,   6, 0x04000BEF) /* PaletteBase */
     , (8037,   7, 0x10000241) /* ClothingBase */
     , (8037,   8, 0x06001D2F) /* Icon */
     , (8037,  22, 0x3400002B) /* PhysicsEffectTable */;
