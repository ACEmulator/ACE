DELETE FROM `weenie` WHERE `class_Id` = 48908;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (48908, 'ace48908-shatteredlegendarykey', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (48908,   1,        128) /* ItemType - Misc */
     , (48908,   5,         20) /* EncumbranceVal */
     , (48908,  16,          1) /* ItemUseable - No */
     , (48908,  18,         64) /* UiEffects - Lightning */
     , (48908,  19,          0) /* Value */
     , (48908,  33,          1) /* Bonded - Bonded */
     , (48908,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (48908, 114,          1) /* Attuned - Attuned */
     , (48908, 267,      86400) /* Lifespan */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (48908,  22, True ) /* Inscribable */
     , (48908,  23, True ) /* DestroyOnSell */
     , (48908,  69, False) /* IsSellable */
     , (48908,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (48908,   1, 'Shattered Legendary Key') /* Name */
     , (48908,  14, 'Use an intricate carving tool to carve this into something useful.') /* Use */
     , (48908,  16, 'A severely damaged and cracked Legendary Key') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (48908,   1, 0x02000160) /* Setup */
     , (48908,   3, 0x20000014) /* SoundTable */
     , (48908,   8, 0x0600740A) /* Icon */
     , (48908,  22, 0x3400002B) /* PhysicsEffectTable */;
