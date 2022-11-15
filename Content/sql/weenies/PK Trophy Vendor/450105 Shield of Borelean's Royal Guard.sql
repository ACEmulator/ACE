DELETE FROM `weenie` WHERE `class_Id` = 450105;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450105, 'ace450105-shieldofboreleansroyalguardtailor', 1, '2021-11-17 16:56:08') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450105,   1,          2) /* ItemType - Armor */
     , (450105,   5,        0) /* EncumbranceVal */
     , (450105,   9,    2097152) /* ValidLocations - Shield */
     , (450105,  16,          1) /* ItemUseable - No */
     , (450105,  18,          1) /* UiEffects - Magical */
     , (450105,  19,       20) /* Value */
     , (450105,  27,          2) /* ArmorType - Leather */
     , (450105,  28,        0) /* ArmorLevel */
     , (450105,  51,          4) /* CombatUse - Shield */
     , (450105,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450105,   1, False) /* Stuck */
     , (450105,  11, True ) /* IgnoreCollisions */
     , (450105,  13, True ) /* Ethereal */
     , (450105,  14, True ) /* GravityStatus */
     , (450105,  19, True ) /* Attackable */
     , (450105,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450105,   5,   -0.05) /* ManaRate */
     , (450105,  13,       1) /* ArmorModVsSlash */
     , (450105,  14,       1) /* ArmorModVsPierce */
     , (450105,  15,     1.2) /* ArmorModVsBludgeon */
     , (450105,  16,     0.6) /* ArmorModVsCold */
     , (450105,  17,     0.6) /* ArmorModVsFire */
     , (450105,  18,     0.6) /* ArmorModVsAcid */
     , (450105,  19,     0.6) /* ArmorModVsElectric */
     , (450105, 110,       1) /* BulkMod */
     , (450105, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450105,   1, 'Shield of Borelean''s Royal Guard') /* Name */
     , (450105,  15, 'A shield, bearing the heraldry of Prince Borelean Strathelar') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450105,   1, 0x02001975) /* Setup */
     , (450105,   3, 0x20000014) /* SoundTable */
     , (450105,   8, 0x06006C1B) /* Icon */
     , (450105,  22, 0x3400002B) /* PhysicsEffectTable */;
