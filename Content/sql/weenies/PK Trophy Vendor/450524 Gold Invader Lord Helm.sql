DELETE FROM `weenie` WHERE `class_Id` = 450524;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450524, 'helminvaderlordgoldtailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450524,   1,          2) /* ItemType - Armor */
     , (450524,   4,      16384) /* ClothingPriority - Head */
     , (450524,   5,        0) /* EncumbranceVal */
     , (450524,   9,          1) /* ValidLocations - HeadWear */
     , (450524,  16,          1) /* ItemUseable - No */
     , (450524,  18,          1) /* UiEffects - Magical */
     , (450524,  19,       20) /* Value */
     , (450524,  28,        0) /* ArmorLevel */
     , (450524,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450524, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450524,  11, True ) /* IgnoreCollisions */
     , (450524,  13, True ) /* Ethereal */
     , (450524,  14, True ) /* GravityStatus */
     , (450524,  19, True ) /* Attackable */
     , (450524,  22, True ) /* Inscribable */
     , (450524,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450524,   5,   -0.05) /* ManaRate */
     , (450524,  13,       1) /* ArmorModVsSlash */
     , (450524,  14,       1) /* ArmorModVsPierce */
     , (450524,  15,     1.2) /* ArmorModVsBludgeon */
     , (450524,  16,     0.8) /* ArmorModVsCold */
     , (450524,  17,     0.8) /* ArmorModVsFire */
     , (450524,  18,     0.7) /* ArmorModVsAcid */
     , (450524,  19,     0.7) /* ArmorModVsElectric */
     , (450524, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450524,   1, 'Gold Invader Lord Helm') /* Name */
     , (450524,  16, 'A helm taken from one of the Knights of the Viamontian Gold Legion.') /* LongDesc */
     , (450524,  33, 'GoldInvaderLordHelm') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450524,   1, 0x0200133B) /* Setup */
     , (450524,   3, 0x20000014) /* SoundTable */
     , (450524,   7, 0x100005DC) /* ClothingBase */
     , (450524,   8, 0x06005A92) /* Icon */
     , (450524,  22, 0x3400002B) /* PhysicsEffectTable */;

