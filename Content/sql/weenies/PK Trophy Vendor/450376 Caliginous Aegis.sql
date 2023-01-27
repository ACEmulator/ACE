DELETE FROM `weenie` WHERE `class_Id` = 450376;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450376, 'ace450376-caliginousaegistailor', 1, '2021-11-17 16:56:08') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450376,   1,          2) /* ItemType - Armor */
     , (450376,   5,       0) /* EncumbranceVal */
     , (450376,   9,    2097152) /* ValidLocations - Shield */
     , (450376,  16,          1) /* ItemUseable - No */
     , (450376,  19,       20) /* Value */
     , (450376,  28,        0) /* ArmorLevel */
     , (450376,  51,          4) /* CombatUse - Shield */
     , (450376,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450376, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450376,  11, True ) /* IgnoreCollisions */
     , (450376,  13, True ) /* Ethereal */
     , (450376,  14, True ) /* GravityStatus */
     , (450376,  19, True ) /* Attackable */
     , (450376,  22, True ) /* Inscribable */
     , (450376, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450376,   5,  -0.025) /* ManaRate */
     , (450376,  13,       2) /* ArmorModVsSlash */
     , (450376,  14,       1) /* ArmorModVsPierce */
     , (450376,  15,       1) /* ArmorModVsBludgeon */
     , (450376,  16,       1) /* ArmorModVsCold */
     , (450376,  17,       2) /* ArmorModVsFire */
     , (450376,  18,       1) /* ArmorModVsAcid */
     , (450376,  19,       1) /* ArmorModVsElectric */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450376,   1, 'Caliginous Aegis') /* Name */
     , (450376,  16, 'A now powerless Aegis taken from the body of Archon Traesis. The Archon''s death has drained the magic absorbing abilities of the Aegis and returned it to what it once was. Perhaps with the right solution, you could restore its original abilities.') /* LongDesc */
     , (450376,  33, 'caligaegis') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450376,   1, 0x02001442) /* Setup */
     , (450376,   3, 0x20000014) /* SoundTable */
     , (450376,   8, 0x06006056) /* Icon */
     , (450376,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450376,  37,          6) /* ItemSkillLimit - MeleeDefense */;


