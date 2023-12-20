DELETE FROM `weenie` WHERE `class_Id` = 490029;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490029, 'bloodofBZ', 1, '2005-02-09 10:00:00') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490029,   1,        128) /* ItemType - Misc */
     , (490029,   3,         14) /* PaletteTemplate - Red */
     , (490029,   5,         10) /* EncumbranceVal */
     , (490029,   8,        200) /* Mass */
     , (490029,   9,          0) /* ValidLocations - None */
     , (490029,  16,          1) /* ItemUseable - No */
     , (490029,  19,          0) /* Value */
     , (490029,  33,          -1) /* Bonded - Bonded */
     , (490029,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490029,  22, True ) /* Inscribable */
     , (490029,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490029,  39,     0.33) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490029,   1, 'Blood of the Hopeslayer') /* Name */
     , (490029,  15, 'A vial of the Hopeslayer''s Blood.  As you examine it, you notice the contents are almost black, as if devouring the ambient light.') /* ShortDesc */
     , (490029,  33, 'BloodofBZ') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490029,   1, 0x02000E74) /* Setup */
     , (490029,   3, 0x20000014) /* SoundTable */
     , (490029,   6, 0x04000BEF) /* PaletteBase */
     , (490029,   7, 0x10000168) /* ClothingBase */
     , (490029,   8, 0x06001F5E) /* Icon */
	 , (490029,  52, 0x06005B0C) /* IconUnderlay */
     , (490029,  22, 0x34000074) /* PhysicsEffectTable */;
