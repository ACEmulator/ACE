DELETE FROM `weenie` WHERE `class_Id` = 4200017;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200017, 'towncontroltrinketofxp', 1, '2021-11-17 16:56:08') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200017,   1,          8) /* ItemType - Jewelry */
     , (4200017,   5,         60) /* EncumbranceVal */
     , (4200017,   9,   67108864) /* ValidLocations - TrinketOne */
     , (4200017,  16,          1) /* ItemUseable - No */
     , (4200017,  18,          1) /* UI Effects Magical */
     , (4200017,  19,         25) /* Value */
     , (4200017,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200017, 106,         50) /* ItemSpellcraft */
     , (4200017, 107,       6000) /* ItemCurMana */
     , (4200017, 108,       6000) /* ItemMaxMana */
     , (4200017,  33,          1) /* Bonded */
     , (4200017, 114,          1) /* Attuned */
     , (4200017, 109,         15) /* ItemDifficulty */
     , (4200017, 267,     172800) /* Lifespan */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200017,  11, True ) /* IgnoreCollisions */
     , (4200017,  13, True ) /* Ethereal */
     , (4200017,  14, True ) /* GravityStatus */
     , (4200017,  19, True ) /* Attackable */
     , (4200017,  22, True ) /* Inscribable */
     , (4200017,  91, False) /* Retained */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200017,   5,  -0.049) /* ManaRate */
     , (4200017,  39,    0.67) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200017,   1, 'Town Control Trinket of Experience') /* Name */
     , (4200017,  16, 'A trinket of experience, grants you an additional 12% Experience from quests and creature kills.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200017,   1, 0x02000179) /* Setup */
     , (4200017,   3, 0x20000014) /* SoundTable */
     , (4200017,   8, 100668277) /* Icon */
     , (4200017,  52, 100673920) /* IconUnderlay */
     , (4200017,  22, 0x3400002B) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (4200017,  5153,      2)  /* Augmented Understanding I */
     , (4200017,  5154,      2)  /* Augmented Understanding II */
     , (4200017,  5137,      2)  /* Augmented Understanding III */;
