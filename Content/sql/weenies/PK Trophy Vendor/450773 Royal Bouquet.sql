DELETE FROM `weenie` WHERE `class_Id` = 450773;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450773, 'ace450773-royalbouquetpk', 35, '2022-05-10 03:49:02') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450773,   1,      32768) /* ItemType - Caster */
     , (450773,   5,         0) /* EncumbranceVal */
     , (450773,   9,   16777216) /* ValidLocations - Held */
     , (450773,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450773,  18,          1) /* UiEffects - Magical */
     , (450773,  19,       20) /* Value */
     , (450773,  46,        512) /* DefaultCombatStyle - Magic */
     , (450773,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450773,  94,         16) /* TargetType - Creature */
     , (450773, 150,        103) /* HookPlacement - Hook */
     , (450773, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450773,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450773,   5,   -0.05) /* ManaRate */
     , (450773,  29,       1) /* WeaponDefense */
     , (450773,  39,     1.5) /* DefaultScale */
     , (450773, 144,     0.1) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450773,   1, 'Royal Bouquet') /* Name */
     , (450773,  16, 'A magical bouquet of flowers which casts it''s beneficial magic on others.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450773,   1, 0x02001977) /* Setup */
     , (450773,   3, 0x20000064) /* SoundTable */
     , (450773,   8, 0x060024C6) /* Icon */
     , (450773,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450773,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */;


