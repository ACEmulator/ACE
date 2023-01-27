DELETE FROM `weenie` WHERE `class_Id` = 480019;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480019, 'materialrareeternalleatherpk', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480019,   1,        128) /* ItemType - Misc */
     , (480019,   3,         77) /* PaletteTemplate - BlueGreen */
     , (480019,   5,          5) /* EncumbranceVal */
     , (480019,   8,          5) /* Mass */
     , (480019,  11,          1) /* MaxStackSize */
     , (480019,  12,          1) /* StackSize */
     , (480019,  13,          5) /* StackUnitEncumbrance */
     , (480019,  14,          5) /* StackUnitMass */
     , (480019,  15,          0) /* StackUnitValue */
     , (480019,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (480019,  19,          500) /* Value */
	 , (480019, 114,          1) /* Attuned - Attuned */
     , (480019,  33,         1) /* Bonded - Slippery */
     , (480019,  92,        100) /* Structure */
     , (480019,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480019,  94,      35215) /* TargetType - Jewelry, Misc, Gem, RedirectableItemEnchantmentTarget */
     , (480019, 150,        103) /* HookPlacement - Hook */
     , (480019, 151,          9) /* HookType - Floor, Yard */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480019,  11, True ) /* IgnoreCollisions */
     , (480019,  13, True ) /* Ethereal */
     , (480019,  14, True ) /* GravityStatus */
     , (480019,  19, True ) /* Attackable */
     , (480019,  22, True ) /* Inscribable */
     , (480019,  23, True ) /* DestroyOnSell */
     , (480019,  63, True ) /* UnlimitedUse */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480019,   1, 'Infinite Leather') /* Name */
     , (480019,  14, 'A bag that contains endless amounts of leather. No matter how much leather is used the bag will always remain full. Apply this material to a treasure-generated item in order to render the item "Retained." Retained items cannot be salvaged or sold to vendors. Applying this material does not require a tinkering skill, does not add a tinker to the target''s count, and cannot destroy the target.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480019,   1, 0x02000181) /* Setup */
     , (480019,   3, 0x20000014) /* SoundTable */
     , (480019,   6, 0x04000BEF) /* PaletteBase */
     , (480019,   7, 0x10000179) /* ClothingBase */
     , (480019,   8, 0x06006702) /* Icon */
     , (480019,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480019,  50, 0x06002703) /* IconOverlay */
     , (480019,  52, 0x06005B0C) /* IconUnderlay */;
