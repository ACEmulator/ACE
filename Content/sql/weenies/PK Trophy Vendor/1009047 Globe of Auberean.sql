DELETE FROM `weenie` WHERE `class_Id` = 1009047;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1009047, 'ace1009047-globeofauberean', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1009047,   1,      32768) /* ItemType - Caster */
     , (1009047,   3,          2) /* PaletteTemplate - Blue */
     , (1009047,   5,         10) /* EncumbranceVal */
     , (1009047,   8,         10) /* Mass */
     , (1009047,   9,   16777216) /* ValidLocations - Held */
     , (1009047,  16,          1) /* ItemUseable - No */
     , (1009047,  18,          1) /* UiEffects - Magical */
     , (1009047,  19,         20) /* Value */
     , (1009047,  46,        512) /* DefaultCombatStyle - Magic */
     , (1009047,  52,          1) /* ParentLocation - RightHand */
     , (1009047,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1009047,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1009047,  94,         16) /* TargetType - Creature */
     , (1009047, 150,        103) /* HookPlacement - Hook */
     , (1009047, 151,          1) /* HookType - Floor */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1009047,  11, True ) /* IgnoreCollisions */
     , (1009047,  13, True ) /* Ethereal */
     , (1009047,  14, True ) /* GravityStatus */
     , (1009047,  19, True ) /* Attackable */
     , (1009047,  22, True ) /* Inscribable */
     , (1009047,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1009047,  29,       1) /* WeaponDefense */
     , (1009047,  39,       1) /* DefaultScale */
     , (1009047, 144,       0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1009047,   1, 'Globe of Auberean') /* Name */
     , (1009047,  15, 'A magical orb.') /* ShortDesc */
     , (1009047,  16, 'A magical orb, painted to show the continents and islands of Auberean. The island of Dereth can barely be seen, a tiny speck in the northern oceans.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1009047,   1,   33556967) /* Setup */
     , (1009047,   3,  536870932) /* SoundTable */
     , (1009047,   6,   67113133) /* PaletteBase */
     , (1009047,   7,  268436124) /* ClothingBase */
     , (1009047,   8,  100671368) /* Icon */
     , (1009047,  22,  872415275) /* PhysicsEffectTable */
     , (1009047,  36,  234881046) /* MutateFilter */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-20T08:22:12.7395969-04:00",
  "ModifiedBy": "derek42588",
  "Changelog": [],
  "UserChangeSummary": "Ev Dalomar",
  "IsDone": false
}
*/
