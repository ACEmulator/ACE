DELETE FROM `weenie` WHERE `class_Id` = 450749;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450749, 'ace450749-tuskerpawwandtailor', 35, '2022-05-17 03:47:03') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450749,   1,      32768) /* ItemType - Caster */
     , (450749,   5,        0) /* EncumbranceVal */
     , (450749,   9,   16777216) /* ValidLocations - Held */
     , (450749,  16,          1) /* ItemUseable - No */
     , (450749,  18,          1) /* UiEffects - Magical */
     , (450749,  19,          20) /* Value */
     , (450749,  46,        512) /* DefaultCombatStyle - Magic */
     , (450749,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450749,  94,         16) /* TargetType - Creature */
     , (450749, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450749,  22, True ) /* Inscribable */
     , (450749,  23, True ) /* DestroyOnSell */
     , (450749,  69, False) /* IsSellable */
     , (450749,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450749,   5,  -0.017) /* ManaRate */
     , (450749,  29,    1.15) /* WeaponDefense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450749,   1, 'Tusker Paw Wand') /* Name */
     , (450749,  16, 'A short little wand with a tusker paw on the end. The paw seems to clench when you cast magic.  Also useful for reaching hard to get things on the top shelf.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450749,   1, 0x02001719) /* Setup */
     , (450749,   3, 0x20000014) /* SoundTable */
     , (450749,   8, 0x0600669F) /* Icon */
     , (450749,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450749,  37,         16) /* ItemSkillLimit - ManaConversion */;


