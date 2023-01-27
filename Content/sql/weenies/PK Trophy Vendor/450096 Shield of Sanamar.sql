DELETE FROM `weenie` WHERE `class_Id` = 450096;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450096, 'ace450096-shieldofsanamartailor', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450096,   1,          2) /* ItemType - Armor */
     , (450096,   5,        0) /* EncumbranceVal */
     , (450096,   9,    2097152) /* ValidLocations - Shield */
     , (450096,  16,          1) /* ItemUseable - No */
     , (450096,  18,          1) /* UiEffects - Magical */
     , (450096,  19,       20) /* Value */
     , (450096,  28,        0) /* ArmorLevel */
     , (450096,  51,          4) /* CombatUse - Shield */
     , (450096,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450096, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450096,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450096,   5,   -0.05) /* ManaRate */
     , (450096,  13,       1) /* ArmorModVsSlash */
     , (450096,  14,     1.1) /* ArmorModVsPierce */
     , (450096,  15,       1) /* ArmorModVsBludgeon */
     , (450096,  16,     0.5) /* ArmorModVsCold */
     , (450096,  17,     0.8) /* ArmorModVsFire */
     , (450096,  18,     0.8) /* ArmorModVsAcid */
     , (450096,  19,     0.5) /* ArmorModVsElectric */
     , (450096,  39,     0.9) /* DefaultScale */
     , (450096, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450096,   1, 'Shield of Sanamar') /* Name */
     , (450096,  16, 'A shield given by Turien di Furza, for valor in the retrieval of the stolen blade, The Sword of Bellenesse, for King Varicci II.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450096,   1, 0x02001524) /* Setup */
     , (450096,   3, 0x20000014) /* SoundTable */
     , (450096,   7, 0x10000803) /* ClothingBase */
     , (450096,   8, 0x06006300) /* Icon */
     , (450096,  22, 0x3400002B) /* PhysicsEffectTable */;


