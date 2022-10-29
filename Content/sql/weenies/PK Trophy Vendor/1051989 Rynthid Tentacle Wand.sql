DELETE FROM `weenie` WHERE `class_Id` = 1051989;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1051989, 'ace1051989-rynthidtentaclewand', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1051989,   1,      32768) /* ItemType - Caster */
     , (1051989,   5,        150) /* EncumbranceVal */
     , (1051989,   9,   16777216) /* ValidLocations - Held */
     , (1051989,  16,          1) /* ItemUseable - No */
     , (1051989,  18,          1) /* UiEffects - Magical */
     , (1051989,  19,         20) /* Value */
     , (1051989,  46,      66048) /* DefaultCombatStyle - Magic, StubbornMagic */
     , (1051989,  52,          1) /* ParentLocation - RightHand */
     , (1051989,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1051989,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1051989,  94,         16) /* TargetType - Creature */
     , (1051989, 106,        475) /* ItemSpellcraft */
     , (1051989, 107,       3000) /* ItemCurMana */
     , (1051989, 108,       3000) /* ItemMaxMana */
     , (1051989, 151,          2) /* HookType - Wall */
     , (1051989, 159,         34) /* WieldSkillType - WarMagic */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1051989,  11, True ) /* IgnoreCollisions */
     , (1051989,  13, True ) /* Ethereal */
     , (1051989,  14, True ) /* GravityStatus */
     , (1051989,  19, True ) /* Attackable */
     , (1051989,  22, True ) /* Inscribable */
     , (1051989,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1051989,   5, -0.032999999821186066) /* ManaRate */
     , (1051989, 144, 0.0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1051989,   1, 'Rynthid Tentacle Wand') /* Name */
     , (1051989,  16, 'A wand crafted from enchanted obsidian and Rynthid tentacles.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1051989,   1,   33561603) /* Setup */
     , (1051989,   3,  536870932) /* SoundTable */
     , (1051989,   6,   67111919) /* PaletteBase */
     , (1051989,   8,  100693234) /* Icon */
     , (1051989,  22,  872415275) /* PhysicsEffectTable */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-15T03:31:58.2957326-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
