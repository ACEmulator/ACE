DELETE FROM `weenie` WHERE `class_Id` = 30093;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (30093, 'materialrareeternalleather', 44, '2021-11-17 16:56:08') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (30093,   1,        128) /* ItemType - Misc */
     , (30093,   3,         77) /* PaletteTemplate - BlueGreen */
     , (30093,   5,          5) /* EncumbranceVal */
     , (30093,   8,          5) /* Mass */
     , (30093,  11,          1) /* MaxStackSize */
     , (30093,  12,          1) /* StackSize */
     , (30093,  13,          5) /* StackUnitEncumbrance */
     , (30093,  14,          5) /* StackUnitMass */
     , (30093,  15,          500) /* StackUnitValue */
     , (30093,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (30093,  17,        152) /* RareId */
     , (30093,  19,          100) /* Value */
	 , (30093, 114,          1) /* Attuned - Attuned */
     , (30093,  33,         1) /* Bonded */
     , (30093,  92,        100) /* Structure */
     , (30093,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (30093,  94,      35215) /* TargetType - Jewelry, Misc, Gem, RedirectableItemEnchantmentTarget */
     , (30093, 150,        103) /* HookPlacement - Hook */
     , (30093, 151,          9) /* HookType - Floor, Yard */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (30093,  11, True ) /* IgnoreCollisions */
     , (30093,  13, True ) /* Ethereal */
     , (30093,  14, True ) /* GravityStatus */
     , (30093,  19, True ) /* Attackable */
     , (30093,  22, True ) /* Inscribable */
     , (30093,  23, True ) /* DestroyOnSell */
     , (30093,  63, True ) /* UnlimitedUse */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (30093,   1, 'Infinite Leather') /* Name */
     , (30093,  14, 'A bag that contains endless amounts of leather. No matter how much leather is used the bag will always remain full. Apply this material to a treasure-generated item in order to render the item "Retained." Retained items cannot be salvaged or sold to vendors. Applying this material does not require a tinkering skill, does not add a tinker to the target''s count, and cannot destroy the target.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (30093,   1, 0x02000181) /* Setup */
     , (30093,   3, 0x20000014) /* SoundTable */
     , (30093,   6, 0x04000BEF) /* PaletteBase */
     , (30093,   7, 0x10000179) /* ClothingBase */
     , (30093,   8, 0x06006702) /* Icon */
     , (30093,  22, 0x3400002B) /* PhysicsEffectTable */
     , (30093,  50, 0x06002703) /* IconOverlay */
     , (30093,  52, 0x06005B0C) /* IconUnderlay */;
