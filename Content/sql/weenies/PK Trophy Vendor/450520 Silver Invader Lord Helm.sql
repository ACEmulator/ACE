DELETE FROM `weenie` WHERE `class_Id` = 450520;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450520, 'helminvaderlordsilvertailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450520,   1,          2) /* ItemType - Armor */
     , (450520,   4,      16384) /* ClothingPriority - Head */
     , (450520,   5,        0) /* EncumbranceVal */
     , (450520,   9,          1) /* ValidLocations - HeadWear */
     , (450520,  16,          1) /* ItemUseable - No */
     , (450520,  18,          1) /* UiEffects - Magical */
     , (450520,  19,       20) /* Value */
     , (450520,  28,        0) /* ArmorLevel */
     , (450520,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450520, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450520,  11, True ) /* IgnoreCollisions */
     , (450520,  13, True ) /* Ethereal */
     , (450520,  14, True ) /* GravityStatus */
     , (450520,  19, True ) /* Attackable */
     , (450520,  22, True ) /* Inscribable */
     , (450520,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450520,   5,   -0.05) /* ManaRate */
     , (450520,  13,       1) /* ArmorModVsSlash */
     , (450520,  14,       1) /* ArmorModVsPierce */
     , (450520,  15,     1.2) /* ArmorModVsBludgeon */
     , (450520,  16,     0.8) /* ArmorModVsCold */
     , (450520,  17,     0.8) /* ArmorModVsFire */
     , (450520,  18,     0.7) /* ArmorModVsAcid */
     , (450520,  19,     0.7) /* ArmorModVsElectric */
     , (450520, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450520,   1, 'Silver Invader Lord Helm') /* Name */
     , (450520,  16, 'A helm taken from one of the Knights of the Viamontian Silver Legion.') /* LongDesc */
     , (450520,  33, 'SilverInvaderLordHelm') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450520,   1, 0x02001339) /* Setup */
     , (450520,   3, 0x20000014) /* SoundTable */
     , (450520,   7, 0x100005D8) /* ClothingBase */
     , (450520,   8, 0x06005A8C) /* Icon */
     , (450520,  22, 0x3400002B) /* PhysicsEffectTable */;

