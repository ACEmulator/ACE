DELETE FROM `weenie` WHERE `class_Id` = 1910604;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1910604, '1910604materialamethyst', 44, '2005-02-09 10:00:00') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1910604,   1, 1073741824) /* ItemType - TinkeringMaterial */
     , (1910604,   3,         77) /* PaletteTemplate - BlueGreen */
     , (1910604,   5,        100) /* EncumbranceVal */
     , (1910604,   8,        100) /* Mass */
     , (1910604,   9,          0) /* ValidLocations - None */
     , (1910604,  11,          1) /* MaxStackSize */
     , (1910604,  12,          1) /* StackSize */
     , (1910604,  13,        100) /* StackUnitEncumbrance */
     , (1910604,  14,        100) /* StackUnitMass */
     , (1910604,  15,         10) /* StackUnitValue */
     , (1910604,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (1910604,  19,         10) /* Value */
     , (1910604,  33,          1) /* Bonded - Bonded */
     , (1910604,  91,        100) /* MaxStructure */
     , (1910604,  92,        100) /* Structure */
     , (1910604,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1910604,  94,      33025) /* TargetType - WeaponOrCaster */
     , (1910604, 105,        100) /* ItemWorkmanship */
     , (1910604, 131,         12) /* MaterialType - Amethyst */
     , (1910604, 150,        103) /* HookPlacement - Hook */
     , (1910604, 151,          9) /* HookType - Floor, Yard */
     , (1910604, 170,         10) /* NumItemsInMaterial */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1910604,  22, True ) /* Inscribable */
     , (1910604,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1910604,   1, 'Salvaged Amethyst') /* Name */
     , (1910604,  14, 'Apply this material to any loot generated weapon to remove the player name wield requirement.') /* Use */
     , (1910604,  15, 'Chips of amethyst material salvaged from old items.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1910604,   1,   33554817) /* Setup */
     , (1910604,   3,  536870932) /* SoundTable */
     , (1910604,   6,   67111919) /* PaletteBase */
     , (1910604,   7,  268436430) /* ClothingBase */
     , (1910604,   8,  100667436) /* Icon */
     , (1910604,  22,  872415275) /* PhysicsEffectTable */
     , (1910604,  50,  100673261) /* IconOverlay */;
