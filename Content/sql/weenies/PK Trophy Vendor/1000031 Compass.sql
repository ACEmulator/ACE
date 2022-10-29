DELETE FROM `weenie` WHERE `class_Id` = 1000031;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1000031, 'ace1000031-compass', 1, '2021-11-20 00:19:18') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1000031,   1,          8) /* ItemType - Jewelry */
     , (1000031,   5,         10) /* EncumbranceVal */
     , (1000031,   9,   67108864) /* ValidLocations - TrinketOne */
     , (1000031,  16,          1) /* ItemUseable - No */
     , (1000031,  18,          1) /* UiEffects - Magical */
     , (1000031,  19,         10) /* Value */
     , (1000031,  33,          1) /* Bonded - Bonded */
     , (1000031,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1000031, 105,          6) /* ItemWorkmanship */
     , (1000031, 106,         10) /* ItemSpellcraft */
     , (1000031, 107,     100000) /* ItemCurMana */
     , (1000031, 108,     100000) /* ItemMaxMana */
     , (1000031, 109,         10) /* ItemDifficulty */
     , (1000031, 131,         63) /* MaterialType - Silver */
     , (1000031, 158,          7) /* WieldRequirements - Level */
     , (1000031, 159,          1) /* WieldSkillType - Axe */
     , (1000031, 172,          1) /* AppraisalLongDescDecoration */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1000031,  22, True ) /* Inscribable */
     , (1000031,  91, True ) /* Retained */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1000031,   5, -0.005555556155741215) /* ManaRate */
     , (1000031,  39, 0.6700000166893005) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1000031,   1, 'Compass') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1000031,   1,   33554680) /* Setup */
     , (1000031,   3,  536870932) /* SoundTable */
     , (1000031,   6,   67111919) /* PaletteBase */
     , (1000031,   8,  100690596) /* Icon */
     , (1000031,  22,  872415275) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (1000031,  2010,      2)  /* Warrior's Ultimate Vigor */
     , (1000031,  2377,      2)  /* Vision of Annihilation */
     , (1000031,  2520,      2)  /* Major Life Magic Aptitude */
     , (1000031,  2534,      2)  /* Major War Magic Aptitude */
     , (1000031,  2573,      2)  /* Major Endurance */
     , (1000031,  2575,      2)  /* Major Quickness */
     , (1000031,  2969,      2)  /* Mother's Blessing */
     , (1000031,  3361,      2)  /* The Art of Destruction */
     , (1000031,  3366,      2)  /* The Heart's Touch */;

/* Lifestoned Changelog:
{
  "LastModified": null,
  "ModifiedBy": null,
  "Changelog": [
    {
      "created": "2021-03-07T13:40:53.5042766Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    }
  ],
  "UserChangeSummary": null,
  "IsDone": false
}
*/
