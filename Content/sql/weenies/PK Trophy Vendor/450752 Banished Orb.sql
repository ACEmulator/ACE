DELETE FROM `weenie` WHERE `class_Id` = 450752;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450752, 'orbbanishedtailor', 35, '2021-11-17 16:56:08') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450752,   1,      32768) /* ItemType - Caster */
     , (450752,   5,         0) /* EncumbranceVal */
     , (450752,   8,         50) /* Mass */
     , (450752,   9,   16777216) /* ValidLocations - Held */
     , (450752,  16,          1) /* ItemUseable - No */
     , (450752,  19,       20) /* Value */
     , (450752,  46,        512) /* DefaultCombatStyle - Magic */
     , (450752,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450752,  94,         16) /* TargetType - Creature */
     , (450752, 150,        103) /* HookPlacement - Hook */
     , (450752, 151,          2) /* HookType - Wall */
     , (450752, 353,          0) /* WeaponType - Undef */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450752,  11, True ) /* IgnoreCollisions */
     , (450752,  13, True ) /* Ethereal */
     , (450752,  14, True ) /* GravityStatus */
     , (450752,  19, True ) /* Attackable */
     , (450752,  22, True ) /* Inscribable */
     , (450752,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450752,   5,  -0.033) /* ManaRate */
     , (450752,  29,       1) /* WeaponDefense */
     , (450752,  39,       1) /* DefaultScale */
     , (450752, 144,       0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450752,   1, 'Banished Orb') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450752,   1, 0x020012D5) /* Setup */
     , (450752,   3, 0x20000014) /* SoundTable */
     , (450752,   8, 0x0600376E) /* Icon */
     , (450752,  22, 0x3400002B) /* PhysicsEffectTable */;


