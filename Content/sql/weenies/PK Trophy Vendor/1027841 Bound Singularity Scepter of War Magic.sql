DELETE FROM `weenie` WHERE `class_Id` = 1027841;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1027841, 'ace1027841-boundsingularityscepterofwarmagic', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1027841,   1,      32768) /* ItemType - Caster */
     , (1027841,   3,         82) /* PaletteTemplate - PinkPurple */
     , (1027841,   5,          1) /* EncumbranceVal */
     , (1027841,   8,         90) /* Mass */
     , (1027841,   9,   16777216) /* ValidLocations - Held */
     , (1027841,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (1027841,  18,          1) /* UiEffects - Magical */
     , (1027841,  19,         20) /* Value */
     , (1027841,  46,        512) /* DefaultCombatStyle - Magic */
     , (1027841,  52,          1) /* ParentLocation - RightHand */
     , (1027841,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1027841,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1027841,  94,         16) /* TargetType - Creature */
     , (1027841, 150,        103) /* HookPlacement - Hook */
     , (1027841, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1027841,  11, True ) /* IgnoreCollisions */
     , (1027841,  13, True ) /* Ethereal */
     , (1027841,  14, True ) /* GravityStatus */
     , (1027841,  19, True ) /* Attackable */
     , (1027841,  22, True ) /* Inscribable */
     , (1027841,  23, True ) /* DestroyOnSell */
     , (1027841,  84, True ) /* IgnoreCloIcons */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1027841,   1, 'Bound Singularity Scepter of War Magic') /* Name */
     , (1027841,  15, 'A scepter imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1027841,   1,   33558795) /* Setup */
     , (1027841,   6,   67111919) /* PaletteBase */
     , (1027841,   7,  268435755) /* ClothingBase */
     , (1027841,   8,  100676590) /* Icon */
     , (1027841,  22,  872415275) /* PhysicsEffectTable */
     , (1027841,  27, 1073742049) /* UseUserAnimation - UseMagicWand */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-29T23:23:31.5851876-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
