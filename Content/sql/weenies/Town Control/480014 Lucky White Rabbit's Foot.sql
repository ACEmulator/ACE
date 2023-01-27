DELETE FROM `weenie` WHERE `class_Id` = 480014;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480014, 'ace480014-luckywhiterabbitsfootpk', 44, '2021-11-01 00:00:00') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480014,   1, 128) /* ItemType - TinkeringMaterial */
     , (480014,   5,         25) /* EncumbranceVal */
     , (480014,  11,          1) /* MaxStackSize */
     , (480014,  12,          1) /* StackSize */
     , (480014,  13,         25) /* StackUnitEncumbrance */
     , (480014,  15,          0) /* StackUnitValue */
     , (480014,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (480014,  19,          250) /* Value */
     , (480014,  33,          1) /* Bonded - Bonded */
     , (480014,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480014,  94,        257) /* TargetType - Weapon */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480014,  22, True ) /* Inscribable */
     , (480014,  23, True ) /* DestroyOnSell */
     , (480014,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480014,  39,     0.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480014,   1, 'Lucky White Rabbit''s Foot') /* Name */
     , (480014,  14, 'Attach this rabbit foot to a treasure-generated weapon to improve the weapon''s variance by 20%. You may only attach one rabbit foot to a weapon.') /* Use */
     , (480014,  16, 'A beautiful white rabbit''s foot.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480014,   1, 0x02000181) /* Setup */
     , (480014,   3, 0x20000014) /* SoundTable */
     , (480014,   8, 0x060063D4) /* Icon */
     , (480014,  22, 0x3400002B) /* PhysicsEffectTable */;
