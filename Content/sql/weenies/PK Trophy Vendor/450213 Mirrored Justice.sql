DELETE FROM `weenie` WHERE `class_Id` = 450213;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450213, 'shieldraremirroredjusticetailor', 1, '2021-11-01 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450213,   1,          2) /* ItemType - Armor */
     , (450213,   3,          4) /* PaletteTemplate - Brown */
     , (450213,   5,        0) /* EncumbranceVal */
     , (450213,   9,    2097152) /* ValidLocations - Shield */
     , (450213,  16,          1) /* ItemUseable - No */
     , (450213,  19,      20) /* Value */
     , (450213,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450213,  27,          2) /* ArmorType - Leather */
     , (450213,  28,        0) /* ArmorLevel */
     , (450213,  51,          4) /* CombatUse - Shield */
     , (450213,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450213, 151,          2) /* HookType - Wall */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450213,  22, True ) /* Inscribable */
     , (450213, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450213,   5,  -0.033) /* ManaRate */
     , (450213,  12,     0.7) /* Shade */
     , (450213,  13,     1.1) /* ArmorModVsSlash */
     , (450213,  14,     0.8) /* ArmorModVsPierce */
     , (450213,  15,     1.4) /* ArmorModVsBludgeon */
     , (450213,  16,     0.9) /* ArmorModVsCold */
     , (450213,  17,     0.9) /* ArmorModVsFire */
     , (450213,  18,     0.9) /* ArmorModVsAcid */
     , (450213,  19,     0.9) /* ArmorModVsElectric */
     , (450213, 110,     1.7) /* BulkMod */
     , (450213, 111,       1) /* SizeMod */
     , (450213, 165,       1) /* ArmorModVsNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450213,   1, 'Mirrored Justice') /* Name */
     , (450213,  16, 'This shield once belonged to Sir Alterio the Vain of Viamont. So consumed was he with his looks that he had this shield made so that he could gaze at his reflection whenever he felt the need to... which was quite often, by most accounts.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450213,   1, 0x0200137D) /* Setup */
     , (450213,   3, 0x20000014) /* SoundTable */
     , (450213,   6, 0x0400007E) /* PaletteBase */
     , (450213,   8, 0x06005BFC) /* Icon */
     , (450213,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450213,  36, 0x0E000012) /* MutateFilter */
     , (450213,  46, 0x38000032) /* TsysMutationFilter */
     , (450213,  52, 0x06005B0C) /* IconUnderlay */;


