DELETE FROM `weenie` WHERE `class_Id` = 450522;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450522, 'helminvaderlordcoppertailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450522,   1,          2) /* ItemType - Armor */
     , (450522,   4,      16384) /* ClothingPriority - Head */
     , (450522,   5,        0) /* EncumbranceVal */
     , (450522,   9,          1) /* ValidLocations - HeadWear */
     , (450522,  16,          1) /* ItemUseable - No */
     , (450522,  18,          1) /* UiEffects - Magical */
     , (450522,  19,       20) /* Value */
     , (450522,  28,        0) /* ArmorLevel */
     , (450522,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450522, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450522,  11, True ) /* IgnoreCollisions */
     , (450522,  13, True ) /* Ethereal */
     , (450522,  14, True ) /* GravityStatus */
     , (450522,  19, True ) /* Attackable */
     , (450522,  22, True ) /* Inscribable */
     , (450522,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450522,   5,   -0.05) /* ManaRate */
     , (450522,  13,       1) /* ArmorModVsSlash */
     , (450522,  14,       1) /* ArmorModVsPierce */
     , (450522,  15,     1.2) /* ArmorModVsBludgeon */
     , (450522,  16,     0.8) /* ArmorModVsCold */
     , (450522,  17,     0.8) /* ArmorModVsFire */
     , (450522,  18,     0.7) /* ArmorModVsAcid */
     , (450522,  19,     0.7) /* ArmorModVsElectric */
     , (450522, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450522,   1, 'Copper Invader Lord Helm') /* Name */
     , (450522,  16, 'A helm taken from one of the Knights of the Viamontian Copper Legion.') /* LongDesc */
     , (450522,  33, 'CopperInvaderLordHelm') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450522,   1, 0x0200133A) /* Setup */
     , (450522,   3, 0x20000014) /* SoundTable */
     , (450522,   7, 0x100005DA) /* ClothingBase */
     , (450522,   8, 0x06005A8F) /* Icon */
     , (450522,  22, 0x3400002B) /* PhysicsEffectTable */;


