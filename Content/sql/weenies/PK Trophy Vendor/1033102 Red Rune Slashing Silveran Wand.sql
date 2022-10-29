DELETE FROM `weenie` WHERE `class_Id` = 1033102;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1033102, 'ace1033102-redruneslashingsilveranwand', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1033102,   1,      32768) /* ItemType - Caster */
     , (1033102,   5,          1) /* EncumbranceVal */
     , (1033102,   9,   16777216) /* ValidLocations - Held */
     , (1033102,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (1033102,  18,       1024) /* UiEffects - Slashing */
     , (1033102,  19,         20) /* Value */
     , (1033102,  46,        512) /* DefaultCombatStyle - Magic */
     , (1033102,  52,          1) /* ParentLocation - RightHand */
     , (1033102,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1033102,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1033102,  94,         16) /* TargetType - Creature */
     , (1033102, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1033102,  11, True ) /* IgnoreCollisions */
     , (1033102,  13, True ) /* Ethereal */
     , (1033102,  14, True ) /* GravityStatus */
     , (1033102,  19, True ) /* Attackable */
     , (1033102,  22, True ) /* Inscribable */
     , (1033102,  69, True ) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1033102,   1, 'Red Rune Slashing Silveran Wand') /* Name */
     , (1033102,  16, 'A spellcasting wand crafted by Silveran smiths, once commissioned by Varicci on Ispar for the Royal Armory.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1033102,   1,   33559921) /* Setup */
     , (1033102,   3,  536870932) /* SoundTable */
     , (1033102,   8,  100688910) /* Icon */
     , (1033102,  22,  872415275) /* PhysicsEffectTable */
     , (1033102,  28,       2146) /* Spell - Evisceration */
     , (1033102,  50,  100688915) /* IconOverlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-30T06:00:38.605452-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": false
}
*/
