DELETE FROM `weenie` WHERE `class_Id` = 490030;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490030, 'olthoikinglongclaw', 1, '2005-02-09 10:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490030,   1,        128) /* ItemType - Misc */
     , (490030,   5,         10) /* EncumbranceVal */
     , (490030,   8,         10) /* Mass */
     , (490030,  16,          1) /* ItemUseable - No */
     , (490030,  19,          0) /* Value */
     , (490030,  33,         -1) /* Bonded - Bonded */
     , (490030,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490030,  22, True ) /* Inscribable */
     , (490030,  23, True ) /* DestroyOnSell */;
	 
INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490030,  39,     0.33) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490030,   1, 'Olthoi King Claw') /* Name */
     , (490030,  16, 'A long claw from the Olthoi King, which must be dead if you are holding it.') /* LongDesc */
     , (490030,  33, 'OlthoiKingClaw') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490030,   1, 0x02000E74) /* Setup */
     , (490030,   3, 0x20000014) /* SoundTable */
     , (490030,   8, 0x06002AF4) /* Icon */
     , (490030,  52, 0x06005B0C) /* IconUnderlay */
     , (490030,  22, 0x34000074) /* PhysicsEffectTable */;
