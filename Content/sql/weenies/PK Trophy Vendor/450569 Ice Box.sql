DELETE FROM `weenie` WHERE `class_Id` = 450569;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450569, 'ace450569-iceboxtailor', 35, '2021-11-17 16:56:08') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450569,   1,      32768) /* ItemType - Caster */
     , (450569,   5,        0) /* EncumbranceVal */
     , (450569,   9,   16777216) /* ValidLocations - Held */
     , (450569,  16,     655364) /* ItemUseable - 655364 */
     , (450569,  19,         20) /* Value */
     , (450569,  46,        512) /* DefaultCombatStyle - Magic */
     , (450569,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450569,  94,         16) /* TargetType - Creature */
     , (450569, 150,        103) /* HookPlacement - Hook */
     , (450569, 151,          2) /* HookType - Wall */
     , (450569, 353,          0) /* WeaponType - Undef */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450569,  11, True ) /* IgnoreCollisions */
     , (450569,  13, True ) /* Ethereal */
     , (450569,  14, True ) /* GravityStatus */
     , (450569,  19, True ) /* Attackable */
     , (450569,  22, True ) /* Inscribable */
     , (450569,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450569,   5,   -0.05) /* ManaRate */
     , (450569,  29,       1) /* WeaponDefense */
     , (450569, 144,    0.15) /* ManaConversionMod */
     , (450569, 150,   1.025) /* WeaponMagicDefense */
     , (450569, 152,     1.1) /* ElementalDamageMod */
     , (450569, 157,       1) /* ResistanceModifier */
     , (450569, 159,    0.25) /* AbsorbMagicDamage */
	 , (450569,  39,     0.1) /* DefaultScale */;
		  
INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450569,   1, 'Ice Box') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450569,   1, 0x02000FF3) /* Setup */
     , (450569,   3, 0x20000014) /* SoundTable */
     , (450569,   8, 0x06002FBF) /* Icon */
     , (450569,  22, 0x3400002B) /* PhysicsEffectTable */;
	 

