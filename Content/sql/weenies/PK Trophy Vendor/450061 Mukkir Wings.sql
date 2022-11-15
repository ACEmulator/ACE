DELETE FROM `weenie` WHERE `class_Id` = 450061;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450061, 'ace450061-mukkirwingstailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450061,   1,          4) /* ItemType - Clothing */
     , (450061,   4,     131072) /* ClothingPriority - 131072 */
     , (450061,   5,         0) /* EncumbranceVal */
     , (450061,   9,  134217728) /* ValidLocations - Cloak */
     , (450061,  16,          1) /* ItemUseable - No */
     , (450061,  18,          1) /* UiEffects - Magical */
     , (450061,  19,      20) /* Value */
     , (450061,  36,       9999) /* ResistMagic */
     , (450061,  65,        101) /* Placement - Resting */
     , (450061,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450061,   1, False) /* Stuck */
     , (450061,  11, True ) /* IgnoreCollisions */
     , (450061,  13, True ) /* Ethereal */
     , (450061,  14, True ) /* GravityStatus */
     , (450061,  19, True ) /* Attackable */
     , (450061,  22, True ) /* Inscribable */
     , (450061,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450061,   1, 'Mukkir Wings') /* Name */
     , (450061,  16, 'A set of beautifully preserved Mukkir Wings, removed from one of the greatest of the Mukkir.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450061,   1, 0x02001B2A) /* Setup */
     , (450061,   3, 0x20000014) /* SoundTable */
     , (450061,   7, 0x10000867) /* ClothingBase */
     , (450061,   8, 0x060074F6) /* Icon */
     , (450061,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450061,  50, 0x06006C37) /* IconOverlay */;
