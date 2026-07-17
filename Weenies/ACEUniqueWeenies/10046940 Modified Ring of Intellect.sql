DELETE FROM `weenie` WHERE `class_Id` = 10046940;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10046940, 'ace10046940-modifiedringofintellect', 1, '2023-03-23 00:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10046940,   1,          8) /* ItemType - Jewelry */
     , (10046940,   5,        100) /* EncumbranceVal */
     , (10046940,   9,     786432) /* ValidLocations - FingerWear */
     , (10046940,  16,          1) /* ItemUseable - No */
     , (10046940,  18,          1) /* UiEffects - Magical */
     , (10046940,  19,       4000) /* Value */
     , (10046940,  33,          1) /* Bonded - Bonded */
     , (10046940,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10046940, 106,        580) /* ItemSpellcraft */
     , (10046940, 107,        750) /* ItemCurMana */
     , (10046940, 108,        750) /* ItemMaxMana */
     , (10046940,  376,          3) /* healing bonus */
     , (10046940, 109,        1) /* ItemDifficulty */
     , (10046940, 114,          1) /* Attuned - Attuned */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10046940,  22, True ) /* Inscribable */
     , (10046940,  23, True ) /* DestroyOnSell */
     , (10046940,  69, False) /* IsSellable */
     , (10046940,  99, True ) /* Ivoryable */
     , (10046940, 112, False) /* cast on self*/;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10046940,   5,  -0) /* ManaRate */
     , (10046940, 156,     .15) /* ProcSpellRate */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10046940,   1, 'Modified Ring of Intellect') /* Name */
     , (10046940,  15, 'An obsidian ring with a precious sapphire set within.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10046940,   1, 0x02000102) /* Setup */
     , (10046940,   3, 0x20000014) /* SoundTable */
     , (10046940,   6, 0x04000BEF) /* PaletteBase */
     , (10046940,   8, 0x060027DB) /* Icon */
     , (10046940,  22, 0x3400002B) /* PhysicsEffectTable */
     , (10046940,  55,       5338) /* ProcSpell - Incantation of Destructive Curse */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (10046940,  4705,      2)  /* Epic Mana Conversion Prowess */
     , (10046940,  4601,      2)  /* Incantation of Mana Conversion Mastery Other */;
