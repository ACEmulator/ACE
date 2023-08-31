DELETE FROM `weenie` WHERE `class_Id` = 30092;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (30092, 'materialrareeternalivory', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (30092,   1,        128) /* ItemType - Misc */
     , (30092,   3,         77) /* PaletteTemplate - BlueGreen */
     , (30092,   5,          5) /* EncumbranceVal */
     , (30092,   8,          5) /* Mass */
     , (30092,  11,          1) /* MaxStackSize */
     , (30092,  12,          1) /* StackSize */
     , (30092,  13,          5) /* StackUnitEncumbrance */
     , (30092,  14,          5) /* StackUnitMass */
     , (30092,  15,          500) /* StackUnitValue */
     , (30092,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (30092,  17,        151) /* RareId */
     , (30092,  19,          100) /* Value */
	 , (30092, 114,          1) /* Attuned - Attuned */
     , (30092,  33,         1) /* Bonded - Slippery */
     , (30092,  92,        100) /* Structure */
     , (30092,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (30092,  94,      35215) /* TargetType - Jewelry, Misc, Gem, RedirectableItemEnchantmentTarget */
     , (30092, 150,        103) /* HookPlacement - Hook */
     , (30092, 151,          9) /* HookType - Floor, Yard */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (30092,  11, True ) /* IgnoreCollisions */
     , (30092,  13, True ) /* Ethereal */
     , (30092,  14, True ) /* GravityStatus */
     , (30092,  19, True ) /* Attackable */
     , (30092,  22, True ) /* Inscribable */
     , (30092,  23, True ) /* DestroyOnSell */
     , (30092,  63, True ) /* UnlimitedUse */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (30092,   1, 'Infinite Ivory') /* Name */
     , (30092,  14, 'A bag that contains endless amounts of ivory. No matter how much ivory is used the bag will always remain full. Apply this material to any of a set of specific quest items in order to unattune it from your soul. Note that this action will also cause it to only be wieldable by you. Applying this material does not require a tinkering skill, does not add a tinker to the target''s count, and cannot destroy the target.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (30092,   1, 0x02000181) /* Setup */
     , (30092,   3, 0x20000014) /* SoundTable */
     , (30092,   6, 0x04000BEF) /* PaletteBase */
     , (30092,   7, 0x100003CE) /* ClothingBase */
     , (30092,   8, 0x060026BF) /* Icon */
     , (30092,  22, 0x3400002B) /* PhysicsEffectTable */
     , (30092,  50, 0x060026FF) /* IconOverlay */
     , (30092,  52, 0x06005B0C) /* IconUnderlay */;
