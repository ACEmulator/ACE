DELETE FROM `weenie` WHERE `class_Id` = 450081;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450081, 'shieldbanditnewtailor', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450081,   1,          2) /* ItemType - Armor */
     , (450081,   3,         39) /* PaletteTemplate - Black */
     , (450081,   5,        0) /* EncumbranceVal */
     , (450081,   9,    2097152) /* ValidLocations - Shield */
     , (450081,  16,          1) /* ItemUseable - No */
     , (450081,  18,          1) /* UiEffects - Magical */
     , (450081,  19,        20) /* Value */
     , (450081,  27,          2) /* ArmorType - Leather */
     , (450081,  28,        150) /* ArmorLevel */
     , (450081,  51,          4) /* CombatUse - Shield */
     , (450081,  56,        0) /* ShieldValue */
     , (450081,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450081, 150,        103) /* HookPlacement - Hook */
     , (450081, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450081,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450081,   5,  -0.033) /* ManaRate */
     , (450081,  13,       1) /* ArmorModVsSlash */
     , (450081,  14,     1.2) /* ArmorModVsPierce */
     , (450081,  15,       1) /* ArmorModVsBludgeon */
     , (450081,  16,     0.6) /* ArmorModVsCold */
     , (450081,  17,     0.6) /* ArmorModVsFire */
     , (450081,  18,     0.6) /* ArmorModVsAcid */
     , (450081,  19,     0.6) /* ArmorModVsElectric */
     , (450081,  39,       1) /* DefaultScale */
     , (450081, 110,       1) /* BulkMod */
     , (450081, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450081,   1, 'Bandit Shield') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450081,   1, 0x02000162) /* Setup */
     , (450081,   3, 0x20000014) /* SoundTable */
     , (450081,   6, 0x04000BEF) /* PaletteBase */
     , (450081,   7, 0x10000097) /* ClothingBase */
     , (450081,   8, 0x06002956) /* Icon */
     , (450081,  22, 0x3400002B) /* PhysicsEffectTable */;


