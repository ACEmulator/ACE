DELETE FROM `weenie` WHERE `class_Id` = 450093;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450093, 'ace450093-shieldofyanshitailor', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450093,   1,          2) /* ItemType - Armor */
     , (450093,   5,        0) /* EncumbranceVal */
     , (450093,   9,    2097152) /* ValidLocations - Shield */
     , (450093,  16,          1) /* ItemUseable - No */
     , (450093,  18,          1) /* UiEffects - Magical */
     , (450093,  19,       20) /* Value */
     , (450093,  28,        0) /* ArmorLevel */
     , (450093,  51,          4) /* CombatUse - Shield */
     , (450093,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450093, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450093,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450093,   5,   -0.05) /* ManaRate */
     , (450093,  13,       1) /* ArmorModVsSlash */
     , (450093,  14,     1.1) /* ArmorModVsPierce */
     , (450093,  15,       1) /* ArmorModVsBludgeon */
     , (450093,  16,     0.5) /* ArmorModVsCold */
     , (450093,  17,     0.8) /* ArmorModVsFire */
     , (450093,  18,     0.8) /* ArmorModVsAcid */
     , (450093,  19,     0.5) /* ArmorModVsElectric */
     , (450093, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450093,   1, 'Shield of Yanshi') /* Name */
     , (450093,  16, 'A shield given by Royal Guard Adrana, for valor in the defense of Yanshi.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450093,   1, 0x02001500) /* Setup */
     , (450093,   3, 0x20000014) /* SoundTable */
     , (450093,   8, 0x0600629D) /* Icon */
     , (450093,  22, 0x3400002B) /* PhysicsEffectTable */;


