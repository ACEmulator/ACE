DELETE FROM `weenie` WHERE `class_Id` = 4200096;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200096, 'bootsraretrackertailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200096,   1,          2) /* ItemType - Armor */
     , (4200096,   3,          1) /* PaletteTemplate - AquaBlue */
     , (4200096,   4,      65536) /* ClothingPriority - Feet */
     , (4200096,   5,          1) /* EncumbranceVal */
     , (4200096,   8,         90) /* Mass */
     , (4200096,   9,        256) /* ValidLocations - FootWear */
     , (4200096,  16,          1) /* ItemUseable - No */
     , (4200096,  19,         20) /* Value */
     , (4200096,  27,          2) /* ArmorType - Leather */
     , (4200096,  28,          1) /* ArmorLevel */
     , (4200096,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200096, 150,        103) /* HookPlacement - Hook */
     , (4200096, 151,          1) /* HookType - Floor */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200096,  11, True ) /* IgnoreCollisions */
     , (4200096,  13, True ) /* Ethereal */
     , (4200096,  14, True ) /* GravityStatus */
     , (4200096,  19, True ) /* Attackable */
     , (4200096,  22, True ) /* Inscribable */
     , (4200096,  91, False) /* Retained */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200096,   1, 'Tracker Boots') /* Name */
     , (4200096,  16, 'These boots are made from finest Auroch leather. Soft and supple, they are the ultimate in style and comfort. Far from just being stylish, these boots allow the user to move speedily and effortlessly over any terrain.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200096,   1, 0x02001379) /* Setup */
     , (4200096,   3, 0x20000014) /* SoundTable */
     , (4200096,   6, 0x0400007E) /* PaletteBase */
     , (4200096,   7, 0x100005E5) /* ClothingBase */
     , (4200096,   8, 0x06005BF1) /* Icon */
     , (4200096,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200096,  36, 0x0E000012) /* MutateFilter */
     , (4200096,  46, 0x38000032) /* TsysMutationFilter */
     , (4200096,  52, 0x06005B0C) /* IconUnderlay */;


