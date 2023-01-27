DELETE FROM `weenie` WHERE `class_Id` = 450232;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450232, 'ace450232-idolofthereclusetailor', 35, '2022-05-17 03:47:03') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450232,   1,      32768) /* ItemType - Caster */
     , (450232,   3,         77) /* PaletteTemplate - BlueGreen */
     , (450232,   5,        0) /* EncumbranceVal */
     , (450232,   9,   16777216) /* ValidLocations - Held */
     , (450232,  16,    6291464) /* ItemUseable - SourceContainedTargetRemoteNeverWalk */
     , (450232,  19,       20) /* Value */
     , (450232,  46,        512) /* DefaultCombatStyle - Magic */
     , (450232,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450232,  94,         16) /* TargetType - Creature */
     , (450232, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450232,  22, True ) /* Inscribable */
     , (450232,  23, True ) /* DestroyOnSell */
     , (450232,  69, False) /* IsSellable */
     , (450232,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450232,   5,  -0.025) /* ManaRate */
     , (450232,  12,     0.5) /* Shade */
     , (450232,  29,     1.2) /* WeaponDefense */
     , (450232,  39,     0.4) /* DefaultScale */
     , (450232, 144,     0.1) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450232,   1, 'Idol of the Recluse') /* Name */
     , (450232,  14, 'Use this item to equip it.') /* Use */
     , (450232,  16, 'An idol depicting the ancient Mu-miyah Recluse. ') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450232,   1, 0x02001C15) /* Setup */
     , (450232,   6, 0x0400007E) /* PaletteBase */
     , (450232,   7, 0x100000BD) /* ClothingBase */
     , (450232,   8, 0x060016C2) /* Icon */
     , (450232,  28,       3203) /* Spell - Eradicate All Magic Other */;

