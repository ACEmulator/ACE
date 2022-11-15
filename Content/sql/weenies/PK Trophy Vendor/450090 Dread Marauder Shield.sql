DELETE FROM `weenie` WHERE `class_Id` = 450090;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450090, 'shieldraredreadmaraudertailor', 1, '2021-11-17 16:56:08') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450090,   1,          2) /* ItemType - Armor */
     , (450090,   3,          4) /* PaletteTemplate - Brown */
     , (450090,   5,        0) /* EncumbranceVal */
     , (450090,   8,         90) /* Mass */
     , (450090,   9,    2097152) /* ValidLocations - Shield */
     , (450090,  16,          1) /* ItemUseable - No */
     , (450090,  17,        271) /* RareId */
     , (450090,  19,     20) /* Value */
     , (450090,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450090,  27,          2) /* ArmorType - Leather */
     , (450090,  28,        0) /* ArmorLevel */
     , (450090,  51,          4) /* CombatUse - Shield */
     , (450090,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450090, 151,          2) /* HookType - Wall */;



INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450090,  11, True ) /* IgnoreCollisions */
     , (450090,  13, True ) /* Ethereal */
     , (450090,  14, True ) /* GravityStatus */
     , (450090,  19, True ) /* Attackable */
     , (450090,  22, True ) /* Inscribable */
     , (450090,  91, False) /* Retained */
     , (450090, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450090,   5,  -0.033) /* ManaRate */
     , (450090,  12,    0.66) /* Shade */
     , (450090,  13,     1.1) /* ArmorModVsSlash */
     , (450090,  14,     1.1) /* ArmorModVsPierce */
     , (450090,  15,     0.9) /* ArmorModVsBludgeon */
     , (450090,  16,     1.4) /* ArmorModVsCold */
     , (450090,  17,     0.9) /* ArmorModVsFire */
     , (450090,  18,     0.9) /* ArmorModVsAcid */
     , (450090,  19,     1.3) /* ArmorModVsElectric */
     , (450090, 110,    1.67) /* BulkMod */
     , (450090, 111,       1) /* SizeMod */
     , (450090, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450090,   1, 'Dread Marauder Shield') /* Name */
     , (450090,  16, 'In all the lands around the Iron Sea, there was no band of mercenaries more feared than the Dread Marauders. They hail from the steppe-lands of the Souia-Vey, but they were cast out of that nomadic society for brutality and barbarism beyond even the standards of the mounted raiders. Fearless and without mercy, these cutthroats hired themselves out to anyone with enough coin. This shield bears the insignia of the Dread Marauders.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450090,   1, 0x0200137B) /* Setup */
     , (450090,   3, 0x20000014) /* SoundTable */
     , (450090,   6, 0x0400007E) /* PaletteBase */
     , (450090,   8, 0x06005BF6) /* Icon */
     , (450090,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450090,  36, 0x0E000012) /* MutateFilter */
     , (450090,  46, 0x38000032) /* TsysMutationFilter */
     , (450090,  52, 0x06005B0C) /* IconUnderlay */;

