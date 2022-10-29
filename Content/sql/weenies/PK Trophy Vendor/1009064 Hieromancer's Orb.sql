DELETE FROM `weenie` WHERE `class_Id` = 1009064;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1009064, 'ace1009064-hieromancersorb', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1009064,   1,      32768) /* ItemType - Caster */
     , (1009064,   3,         61) /* PaletteTemplate - White */
     , (1009064,   5,         50) /* EncumbranceVal */
     , (1009064,   8,         50) /* Mass */
     , (1009064,   9,   16777216) /* ValidLocations - Held */
     , (1009064,  16,          1) /* ItemUseable - No */
     , (1009064,  18,          1) /* UiEffects - Magical */
     , (1009064,  19,         20) /* Value */
     , (1009064,  46,        512) /* DefaultCombatStyle - Magic */
     , (1009064,  52,          1) /* ParentLocation - RightHand */
     , (1009064,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1009064,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1009064,  94,         16) /* TargetType - Creature */
     , (1009064, 106,        225) /* ItemSpellcraft */
     , (1009064, 107,       1500) /* ItemCurMana */
     , (1009064, 108,       1500) /* ItemMaxMana */
     , (1009064, 115,        200) /* ItemSkillLevelLimit */
     , (1009064, 150,        103) /* HookPlacement - Hook */
     , (1009064, 151,          2) /* HookType - Wall */
     , (1009064, 176,         34) /* AppraisalItemSkill */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1009064,  11, True ) /* IgnoreCollisions */
     , (1009064,  13, True ) /* Ethereal */
     , (1009064,  14, True ) /* GravityStatus */
     , (1009064,  19, True ) /* Attackable */
     , (1009064,  22, True ) /* Inscribable */
     , (1009064,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1009064,   5, -0.05000000074505806) /* ManaRate */
     , (1009064,  29,       1) /* WeaponDefense */
     , (1009064, 144, 0.05000000074505806) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1009064,   1, 'Hieromancer''s Orb') /* Name */
     , (1009064,  16, 'An orb of the type carried by the Yalaini Order of Hieromancers, as an emblem of their station.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1009064,   1,   33556965) /* Setup */
     , (1009064,   3,  536870932) /* SoundTable */
     , (1009064,   6,   67111919) /* PaletteBase */
     , (1009064,   7,  268436123) /* ClothingBase */
     , (1009064,   8,  100671367) /* Icon */
     , (1009064,  22,  872415275) /* PhysicsEffectTable */
     , (1009064,  27, 1073742049) /* UseUserAnimation - UseMagicWand */
     , (1009064,  37,         34) /* ItemSkillLimit */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-14T18:07:36.5634158-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
