DELETE FROM `weenie` WHERE `class_Id` = 1008799;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1008799, 'ace1008799-greatworkstaffofthelightbringer', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1008799,   1,      32768) /* ItemType - Caster */
     , (1008799,   3,         83) /* PaletteTemplate - Amber */
     , (1008799,   5,        100) /* EncumbranceVal */
     , (1008799,   8,         25) /* Mass */
     , (1008799,   9,   16777216) /* ValidLocations - Held */
     , (1008799,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (1008799,  18,          1) /* UiEffects - Magical */
     , (1008799,  19,         20) /* Value */
     , (1008799,  33,          1) /* Bonded - Bonded */
     , (1008799,  46,        512) /* DefaultCombatStyle - Magic */
     , (1008799,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (1008799,  94,         16) /* TargetType - Creature */
     , (1008799, 110,          0) /* ItemAllegianceRankLimit */
     , (1008799, 114,          1) /* Attuned - Attuned */
     , (1008799, 150,        103) /* HookPlacement - Hook */
     , (1008799, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1008799,  15, True ) /* LightsStatus */
     , (1008799,  22, True ) /* Inscribable */
     , (1008799,  23, True ) /* DestroyOnSell */
     , (1008799,  69, False) /* IsSellable */
     , (1008799,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1008799,  29,       1) /* WeaponDefense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1008799,   1, 'Great Work Staff of the Lightbringer') /* Name */
     , (1008799,  15, 'A trophy from the banishment of Bael''Zharon.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1008799,   1,   33556940) /* Setup */
     , (1008799,   3,  536870932) /* SoundTable */
     , (1008799,   6,   67111919) /* PaletteBase */
     , (1008799,   7,  268436103) /* ClothingBase */
     , (1008799,   8,  100671278) /* Icon */
     , (1008799,  22,  872415275) /* PhysicsEffectTable */
     , (1008799,  27, 1073742049) /* UseUserAnimation - UseMagicWand */
     , (1008799,  36,  234881046) /* MutateFilter */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-20T09:17:21.3371441-04:00",
  "ModifiedBy": "derek42588",
  "Changelog": [],
  "UserChangeSummary": "Ev Dalomar",
  "IsDone": false
}
*/
