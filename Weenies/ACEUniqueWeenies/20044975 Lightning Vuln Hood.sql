DELETE FROM `weenie` WHERE `class_Id` = 20044975;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (20044975, 'LightningVulnHood', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (20044975,   1,          4) /* ItemType - Clothing */
     , (20044975,   3,         14) /* PaletteTemplate - Red */
     , (20044975,   4,      16384) /* ClothingPriority - Head */
     , (20044975,   5,         15) /* EncumbranceVal */
     , (20044975,   9,          1) /* ValidLocations - HeadWear */
     , (20044975,  16,          1) /* ItemUseable - No */
         , (20044975,  18,          1) /* UiEffects - Magical */
     , (20044975,  19,        100) /* Value */
     , (20044975,  27,          1) /* ArmorType - Cloth */
     , (20044975,  28,        10) /* ArmorLevel */
     , (20044975,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (20044975, 150,        103) /* HookPlacement - Hook */
     , (20044975, 106,        580) /* ItemSpellcraft */
     , (20044975, 109,        1) /* ItemDifficulty */
     , (20044975, 151,          2) /* HookType - Wall */
     , (20044975, 107,       5000) /* ItemCurMana */
     , (20044975, 108,       5000) /* ItemMaxMana */
     , (20044975, 169,  218105360) /* TsysMutationData */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (20044975,  11, True ) /* IgnoreCollisions */
     , (20044975,  13, True ) /* Ethereal */
     , (20044975,  14, True ) /* GravityStatus */
     , (20044975,  19, True ) /* Attackable */
     , (20044975,  22, True ) /* Inscribable */
     , (20044975, 100, True ) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (20044975,  13,     1.2) /* ArmorModVsSlash */
     , (20044975,  14,     0.8) /* ArmorModVsPierce */
     , (20044975,  15,       1) /* ArmorModVsBludgeon */
     , (20044975,  16,     0.5) /* ArmorModVsCold */
     , (20044975,  17,     0.5) /* ArmorModVsFire */
     , (20044975,  18,     0.3) /* ArmorModVsAcid */
     , (20044975,  19,     0.8) /* ArmorModVsElectric */
     , (20044975, 156,     0.5) /* ProcSpellRate */
     , (20044975, 165,       1) /* ArmorModVsNether */
     ,(20044975,   5,       0) /* ManaRate */;


     INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES
     (20044975,  55,       4483) /* ProcSpell - Lightning Vuln 8*/;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (20044975,   1, 'Hood') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (20044975,   1, 0x0200070D) /* Setup */
     , (20044975,   3, 0x20000014) /* SoundTable */
     , (20044975,   6, 0x0400007E) /* PaletteBase */
     , (20044975,   7, 0x100007FA) /* ClothingBase */
     , (20044975,   8, 0x06001B88) /* Icon */
     , (20044975,  22, 0x3400002B) /* PhysicsEffectTable */;
