DELETE FROM `weenie` WHERE `class_Id` = 450526;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450526, 'helminvaderlordplatinumtailor', 2, '2021-11-17 16:56:08') /* Clothing */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450526,   1,          2) /* ItemType - Armor */
     , (450526,   4,      16384) /* ClothingPriority - Head */
     , (450526,   5,        0) /* EncumbranceVal */
     , (450526,   9,          1) /* ValidLocations - HeadWear */
     , (450526,  16,          1) /* ItemUseable - No */
     , (450526,  18,          1) /* UiEffects - Magical */
     , (450526,  19,       20) /* Value */
     , (450526,  28,        0) /* ArmorLevel */
     , (450526,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450526, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450526,  11, True ) /* IgnoreCollisions */
     , (450526,  13, True ) /* Ethereal */
     , (450526,  14, True ) /* GravityStatus */
     , (450526,  19, True ) /* Attackable */
     , (450526,  22, True ) /* Inscribable */
     , (450526,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450526,   5,   -0.05) /* ManaRate */
     , (450526,  13,       1) /* ArmorModVsSlash */
     , (450526,  14,       1) /* ArmorModVsPierce */
     , (450526,  15,     1.2) /* ArmorModVsBludgeon */
     , (450526,  16,     0.8) /* ArmorModVsCold */
     , (450526,  17,     0.8) /* ArmorModVsFire */
     , (450526,  18,     0.7) /* ArmorModVsAcid */
     , (450526,  19,     0.7) /* ArmorModVsElectric */
     , (450526, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450526,   1, 'Platinum Invader Lord Helm') /* Name */
     , (450526,  16, 'A helm taken from one of the Knights of the Viamontian Platinum Legion.') /* LongDesc */
     , (450526,  33, 'PlatinumInvaderLordHelm') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450526,   1, 0x0200133C) /* Setup */
     , (450526,   3, 0x20000014) /* SoundTable */
     , (450526,   7, 0x100005DE) /* ClothingBase */
     , (450526,   8, 0x06005A95) /* Icon */
     , (450526,  22, 0x3400002B) /* PhysicsEffectTable */;


