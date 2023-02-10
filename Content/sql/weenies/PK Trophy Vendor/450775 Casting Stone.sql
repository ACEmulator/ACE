DELETE FROM `weenie` WHERE `class_Id` = 450775;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450775, 'ace450775-castingstonepk', 35, '2021-11-01 00:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450775,   1,      32768) /* ItemType - Caster */
     , (450775,   5,        200) /* EncumbranceVal */
     , (450775,   9,   16777216) /* ValidLocations - Held */
     , (450775,  16,          1) /* ItemUseable - No */
     , (450775,  18,          1) /* UiEffects - Magical */
     , (450775,  19,          20) /* Value */
     , (450775,  33,          1) /* Bonded - Bonded */
     , (450775,  46,        512) /* DefaultCombatStyle - Magic */
     , (450775,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450775,  94,         16) /* TargetType - Creature */
     , (450775, 151,          3) /* HookType - Floor, Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450775,  22, True ) /* Inscribable */
     , (450775,  23, True ) /* DestroyOnSell */
     , (450775,  69, False) /* IsSellable */
     , (450775,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450775,   5,  -0.025) /* ManaRate */
     , (450775,  39,     0.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450775,   1, 'Casting Stone') /* Name */
     , (450775,  16, 'A large stone that is remarkably light for its size. ') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450775,   1, 0x02000597) /* Setup */
     , (450775,   3, 0x20000014) /* SoundTable */
     , (450775,   8, 0x0600106C) /* Icon */
     , (450775,  22, 0x3400002B) /* PhysicsEffectTable */;

