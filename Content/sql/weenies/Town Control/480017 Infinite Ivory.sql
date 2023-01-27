DELETE FROM `weenie` WHERE `class_Id` = 480017;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480017, 'materialrareeternalivorypk', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480017,   1,        128) /* ItemType - Misc */
     , (480017,   3,         77) /* PaletteTemplate - BlueGreen */
     , (480017,   5,          5) /* EncumbranceVal */
     , (480017,   8,          5) /* Mass */
     , (480017,  11,          1) /* MaxStackSize */
     , (480017,  12,          1) /* StackSize */
     , (480017,  13,          5) /* StackUnitEncumbrance */
     , (480017,  14,          5) /* StackUnitMass */
     , (480017,  15,          0) /* StackUnitValue */
     , (480017,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (480017,  19,          500) /* Value */
     , (480017,  33,         1) /* Bonded - Slippery */
	 , (480017, 114,          1) /* Attuned - Attuned */
     , (480017,  92,        100) /* Structure */
     , (480017,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480017,  94,      35215) /* TargetType - Jewelry, Misc, Gem, RedirectableItemEnchantmentTarget */
     , (480017, 150,        103) /* HookPlacement - Hook */
     , (480017, 151,          9) /* HookType - Floor, Yard */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480017,  11, True ) /* IgnoreCollisions */
     , (480017,  13, True ) /* Ethereal */
     , (480017,  14, True ) /* GravityStatus */
     , (480017,  19, True ) /* Attackable */
     , (480017,  22, True ) /* Inscribable */
     , (480017,  23, True ) /* DestroyOnSell */
     , (480017,  63, True ) /* UnlimitedUse */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480017,   1, 'Infinite Ivory') /* Name */
     , (480017,  14, 'A bag that contains endless amounts of ivory. No matter how much ivory is used the bag will always remain full. Apply this material to any of a set of specific quest items in order to unattune it from your soul. Note that this action will also cause it to only be wieldable by you. Applying this material does not require a tinkering skill, does not add a tinker to the target''s count, and cannot destroy the target.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480017,   1, 0x02000181) /* Setup */
     , (480017,   3, 0x20000014) /* SoundTable */
     , (480017,   6, 0x04000BEF) /* PaletteBase */
     , (480017,   7, 0x100003CE) /* ClothingBase */
     , (480017,   8, 0x060026BF) /* Icon */
     , (480017,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480017,  50, 0x060026FF) /* IconOverlay */
     , (480017,  52, 0x06005B0C) /* IconUnderlay */;
