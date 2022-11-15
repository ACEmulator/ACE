DELETE FROM `weenie` WHERE `class_Id` = 450094;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450094, 'ace450094-shieldofelysasroyalguardtailor', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450094,   1,          2) /* ItemType - Armor */
     , (450094,   5,        0) /* EncumbranceVal */
     , (450094,   9,    2097152) /* ValidLocations - Shield */
     , (450094,  16,          1) /* ItemUseable - No */
     , (450094,  18,          1) /* UiEffects - Magical */
     , (450094,  19,       20) /* Value */
     , (450094,  27,          2) /* ArmorType - Leather */
     , (450094,  28,        0) /* ArmorLevel */
     , (450094,  51,          4) /* CombatUse - Shield */
     , (450094,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450094,   1, False) /* Stuck */
     , (450094,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450094,   5,   -0.05) /* ManaRate */
     , (450094,  13,       1) /* ArmorModVsSlash */
     , (450094,  14,       1) /* ArmorModVsPierce */
     , (450094,  15,     1.2) /* ArmorModVsBludgeon */
     , (450094,  16,     0.6) /* ArmorModVsCold */
     , (450094,  17,     0.6) /* ArmorModVsFire */
     , (450094,  18,     0.6) /* ArmorModVsAcid */
     , (450094,  19,     0.6) /* ArmorModVsElectric */
     , (450094, 110,       1) /* BulkMod */
     , (450094, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450094,   1, 'Shield of Elysa''s Royal Guard') /* Name */
     , (450094,  15, 'A shield, bearing the Strathelar coat of arms. This is standard issue for those who have joined Queen Elysa''s army, though it has been given on occasion to honor those who have acted in the interests of the kingdom.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450094,   1, 0x02001507) /* Setup */
     , (450094,   3, 0x20000014) /* SoundTable */
     , (450094,   8, 0x060062AC) /* Icon */
     , (450094,  22, 0x3400002B) /* PhysicsEffectTable */;
