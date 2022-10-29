DELETE FROM `weenie` WHERE `class_Id` = 4200095;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200095, 'ace4200095-harbingerarmguardtailor', 2, '2021-11-01 00:00:00') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200095,   1,          2) /* ItemType - Armor */
     , (4200095,   3,         39) /* PaletteTemplate - Black */
     , (4200095,   4,      32768) /* ClothingPriority - Hands */
     , (4200095,   5,          1) /* EncumbranceVal */
     , (4200095,   8,         90) /* Mass */
     , (4200095,   9,         32) /* ValidLocations - HandWear */
     , (4200095,  16,          1) /* ItemUseable - No */
     , (4200095,  19,         20) /* Value */
     , (4200095,  27,          2) /* ArmorType - Leather */
     , (4200095,  28,          1) /* ArmorLevel */
     , (4200095,  45,          4) /* DamageType - Bludgeon */
     , (4200095,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200095, 151,          2) /* HookType - Wall */
     , (4200095, 159,          6) /* WieldSkillType - MeleeDefense */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200095,  22, True ) /* Inscribable */
     , (4200095,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200095,   5,  -0.017) /* ManaRate */
     , (4200095,  12,       0) /* Shade */
     , (4200095,  13,       0) /* ArmorModVsSlash */
     , (4200095,  14,       0) /* ArmorModVsPierce */
     , (4200095,  15,       0) /* ArmorModVsBludgeon */
     , (4200095,  16,       0) /* ArmorModVsCold */
     , (4200095,  17,       0) /* ArmorModVsFire */
     , (4200095,  18,       0) /* ArmorModVsAcid */
     , (4200095,  19,       0) /* ArmorModVsElectric */
     , (4200095,  22,       0) /* DamageVariance */
     , (4200095, 110,       1) /* BulkMod */
     , (4200095, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200095,   1, 'Harbinger Arm Guard') /* Name */
     , (4200095,  16, 'The hollowed out Arm of the Harbinger.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200095,   1, 0x0200159C) /* Setup */
     , (4200095,   3, 0x20000014) /* SoundTable */
     , (4200095,   7, 0x10000682) /* ClothingBase */
     , (4200095,   8, 0x060027CB) /* Icon */
     , (4200095,  22, 0x3400002B) /* PhysicsEffectTable */;