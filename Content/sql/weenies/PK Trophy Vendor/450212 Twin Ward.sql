DELETE FROM `weenie` WHERE `class_Id` = 450212;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450212, 'shieldraretwinwardtailor', 1, '2021-11-17 16:56:08') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450212,   1,          2) /* ItemType - Armor */
     , (450212,   3,          4) /* PaletteTemplate - Brown */
     , (450212,   5,        0) /* EncumbranceVal */
     , (450212,   8,         90) /* Mass */
     , (450212,   9,    2097152) /* ValidLocations - Shield */
     , (450212,  16,          1) /* ItemUseable - No */
     , (450212,  19,      20) /* Value */
     , (450212,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450212,  27,          8) /* ArmorType - Scalemail */
     , (450212,  28,        0) /* ArmorLevel */
     , (450212,  51,          4) /* CombatUse - Shield */
     , (450212,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450212, 151,          2) /* HookType - Wall */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450212,  11, True ) /* IgnoreCollisions */
     , (450212,  13, True ) /* Ethereal */
     , (450212,  14, True ) /* GravityStatus */
     , (450212,  19, True ) /* Attackable */
     , (450212,  22, True ) /* Inscribable */
     , (450212,  91, False) /* Retained */
     , (450212, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450212,   5,  -0.033) /* ManaRate */
     , (450212,  12,    0.66) /* Shade */
     , (450212,  13,     1.1) /* ArmorModVsSlash */
     , (450212,  14,     0.9) /* ArmorModVsPierce */
     , (450212,  15,     1.3) /* ArmorModVsBludgeon */
     , (450212,  16,     0.9) /* ArmorModVsCold */
     , (450212,  17,     0.9) /* ArmorModVsFire */
     , (450212,  18,     1.1) /* ArmorModVsAcid */
     , (450212,  19,     0.9) /* ArmorModVsElectric */
     , (450212, 110,    1.67) /* BulkMod */
     , (450212, 111,       1) /* SizeMod */
     , (450212, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450212,   1, 'Twin Ward') /* Name */
     , (450212,  16, '"Why would you want to use a shield with a target plain as day on it?" said one. "''Tis ugly!" said another. The only response by the smith was, "Would you rather have an archer aiming for your head or for your shield?"') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450212,   1, 0x0200137A) /* Setup */
     , (450212,   3, 0x20000014) /* SoundTable */
     , (450212,   6, 0x0400007E) /* PaletteBase */
     , (450212,   8, 0x06005BF3) /* Icon */
     , (450212,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450212,  36, 0x0E000012) /* MutateFilter */
     , (450212,  46, 0x38000032) /* TsysMutationFilter */
     , (450212,  52, 0x06005B0C) /* IconUnderlay */;
