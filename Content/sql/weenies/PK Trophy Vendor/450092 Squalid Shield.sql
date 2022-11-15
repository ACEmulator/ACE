DELETE FROM `weenie` WHERE `class_Id` = 450092;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450092, 'ace450092-squalidshieldtailor', 1, '2021-11-17 16:56:08') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450092,   1,          2) /* ItemType - Armor */
     , (450092,   5,        0) /* EncumbranceVal */
     , (450092,   9,    2097152) /* ValidLocations - Shield */
     , (450092,  16,          1) /* ItemUseable - No */
     , (450092,  19,       20) /* Value */
     , (450092,  28,        0) /* ArmorLevel */
     , (450092,  51,          4) /* CombatUse - Shield */
     , (450092,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450092, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450092,  11, True ) /* IgnoreCollisions */
     , (450092,  13, True ) /* Ethereal */
     , (450092,  14, True ) /* GravityStatus */
     , (450092,  19, True ) /* Attackable */
     , (450092,  22, True ) /* Inscribable */
     , (450092,  69, True ) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450092,   5,   -0.05) /* ManaRate */
     , (450092,  13,       1) /* ArmorModVsSlash */
     , (450092,  14,     1.2) /* ArmorModVsPierce */
     , (450092,  15,       1) /* ArmorModVsBludgeon */
     , (450092,  16,     1.4) /* ArmorModVsCold */
     , (450092,  17,     0.5) /* ArmorModVsFire */
     , (450092,  18,     0.8) /* ArmorModVsAcid */
     , (450092,  19,     0.8) /* ArmorModVsElectric */
     , (450092, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450092,   1, 'Squalid Shield') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450092,   1, 0x020013F6) /* Setup */
     , (450092,   3, 0x20000014) /* SoundTable */
     , (450092,   8, 0x06005F9B) /* Icon */
     , (450092,  22, 0x3400002B) /* PhysicsEffectTable */;
