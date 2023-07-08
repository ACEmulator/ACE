DELETE FROM `weenie` WHERE `class_Id` = 8041;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (8041, 'plantdarkyellow', 51, '2005-02-09 10:00:00') /* Stackable */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (8041,   1,    4194304) /* ItemType - CraftCookingBase */
     , (8041,   3,         86) /* PaletteTemplate - DyeDarkYellow */
     , (8041,   5,          5) /* EncumbranceVal */
     , (8041,   8,         50) /* Mass */
     , (8041,   9,          0) /* ValidLocations - None */
     , (8041,  11,        100) /* MaxStackSize */
     , (8041,  12,          1) /* StackSize */
     , (8041,  13,          5) /* StackUnitEncumbrance */
     , (8041,  14,         50) /* StackUnitMass */
     , (8041,  15,         1) /* StackUnitValue */
     , (8041,  16,          1) /* ItemUseable - No */
     , (8041,  19,         1) /* Value */
     , (8041,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (8041, 150,        103) /* HookPlacement - Hook */
     , (8041, 151,          9) /* HookType - Floor, Yard */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (8041,  39,     1.2) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (8041,   1, 'Berimphur Plant') /* Name */
     , (8041,  15, 'A dark yellow berimphur plant.') /* ShortDesc */
     , (8041,  16, 'A dark yellow berimphur plant. ') /* LongDesc */
     , (8041,  20, 'Berimphur Plants') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (8041,   1, 0x0200090E) /* Setup */
     , (8041,   3, 0x20000014) /* SoundTable */
     , (8041,   6, 0x04000BEF) /* PaletteBase */
     , (8041,   7, 0x10000241) /* ClothingBase */
     , (8041,   8, 0x06001D2F) /* Icon */
     , (8041,  22, 0x3400002B) /* PhysicsEffectTable */;
