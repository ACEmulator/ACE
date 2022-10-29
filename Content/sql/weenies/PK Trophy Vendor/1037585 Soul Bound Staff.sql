DELETE FROM `weenie` WHERE `class_Id` = 1037585;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1037585, 'ace1037585-soulboundstaff', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1037585,   1,      32768) /* ItemType - Caster */
     , (1037585,   5,         50) /* EncumbranceVal */
     , (1037585,   9,   16777216) /* ValidLocations - Held */
     , (1037585,  16,    6291464) /* ItemUseable - SourceContainedTargetRemoteNeverWalk */
     , (1037585,  18,          1) /* UiEffects - Magical */
     , (1037585,  19,         20) /* Value */
     , (1037585,  33,          1) /* Bonded - Bonded */
     , (1037585,  46,        512) /* DefaultCombatStyle - Magic */
     , (1037585,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (1037585,  94,         16) /* TargetType - Creature */
     , (1037585, 114,          1) /* Attuned - Attuned */
     , (1037585, 151,          2) /* HookType - Wall */
     , (1037585, 263,          2) /* ResistanceModifierType */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1037585,  11, True ) /* IgnoreCollisions */
     , (1037585,  19, True ) /* Attackable */
     , (1037585,  22, True ) /* Inscribable */
     , (1037585,  23, True ) /* DestroyOnSell */
     , (1037585,  69, False) /* IsSellable */
     , (1037585,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1037585,   5, -0.05000000074505806) /* ManaRate */
     , (1037585,  39, 0.699999988079071) /* DefaultScale */
     , (1037585,  76, 0.699999988079071) /* Translucency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1037585,   1, 'Soul Bound Staff') /* Name */
     , (1037585,  15, 'A ghostly blue casting staff, bound to your soul.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1037585,   1,   33560575) /* Setup */
     , (1037585,   3,  536870932) /* SoundTable */
     , (1037585,   8,  100675639) /* Icon */
     , (1037585,  22,  872415275) /* PhysicsEffectTable */
     , (1037585,  27, 1073742049) /* UseUserAnimation - UseMagicWand */
     , (1037585,  28,       2132) /* Spell - The Spike */
     , (1037585,  52,  100689896) /* IconUnderlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-19T21:48:12.2448764-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "removed some spells ",
  "IsDone": true
}
*/
