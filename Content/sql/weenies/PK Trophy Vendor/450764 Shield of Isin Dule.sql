DELETE FROM `weenie` WHERE `class_Id` = 450764;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450764, 'ace450764-shieldofisinduletailor', 1, '2021-11-17 16:56:08') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450764,   1,          2) /* ItemType - Armor */
     , (450764,   5,        0) /* EncumbranceVal */
     , (450764,   9,    2097152) /* ValidLocations - Shield */
     , (450764,  16,          1) /* ItemUseable - No */
     , (450764,  19,      20) /* Value */
     , (450764,  28,        0) /* ArmorLevel */
     , (450764,  51,          4) /* CombatUse - Shield */
     , (450764,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450764, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450764,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450764,   5,  -0.025) /* ManaRate */
     , (450764,  13,       1) /* ArmorModVsSlash */
     , (450764,  14,     1.8) /* ArmorModVsPierce */
     , (450764,  15,     1.8) /* ArmorModVsBludgeon */
     , (450764,  16,       2) /* ArmorModVsCold */
     , (450764,  17,     0.8) /* ArmorModVsFire */
     , (450764,  18,       1) /* ArmorModVsAcid */
     , (450764,  19,     0.8) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450764,   1, 'Shield of Isin Dule') /* Name */
     , (450764,  14, 'Turn this into the Shadow Hunter if you would rather have an experience reward.') /* Use */
     , (450764,  16, 'A shadowy shield with surprising strength. As you attempt to push your finger through the shadowy substance it becomes increasingly resistant to your push.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450764,   1, 0x02001573) /* Setup */
     , (450764,   3, 0x20000014) /* SoundTable */
     , (450764,   8, 0x06006417) /* Icon */
     , (450764,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450764,  37,         48) /* ItemSkillLimit - Shield */;

