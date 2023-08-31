DELETE FROM `weenie` WHERE `class_Id` = 72921;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (72921, 'ace72921-olthoitokenofluminance', 1, '2023-08-20 21:15:24') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (72921,   1,        128) /* ItemType - Misc */
     , (72921,   3,         20) /* PaletteTemplate - Silver */
     , (72921,   5,          1) /* EncumbranceVal */
     , (72921,   8,          1) /* Mass */
     , (72921,   9,          0) /* ValidLocations - None */
     , (72921,  16,          1) /* ItemUseable - No */
     , (72921,  19,         50) /* Value */
     , (72921,  33,          1) /* Bonded - Bonded */
     , (72921,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (72921, 114,          1) /* Attuned - Attuned */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (72921,  22, True ) /* Inscribable */
     , (72921,  23, True ) /* DestroyOnSell */
     , (72921,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (72921,  39,     0.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (72921,   1, 'Olthoi Token of Luminance') /* Name */
     , (72921,  14, 'You must be 150th level to turn in this token. You may only trade in one Olthoi Token per week for a reward.') /* Use */
     , (72921,  15, 'A reward token for killing the Olthoi on Olthoi Isle. This token may be traded to Mayor Trenlach for 15,000 Luminance.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (72921,   1, 0x02000181) /* Setup */
     , (72921,   3, 0x20000014) /* SoundTable */
     , (72921,   6, 0x04000BEF) /* PaletteBase */
     , (72921,   7, 0x10000178) /* ClothingBase */
     , (72921,   8, 0x0600223D) /* Icon */
     , (72921,  22, 0x3400002B) /* PhysicsEffectTable */;
