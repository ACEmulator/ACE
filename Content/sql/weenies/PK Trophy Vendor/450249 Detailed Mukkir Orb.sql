DELETE FROM `weenie` WHERE `class_Id` = 450249;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450249, 'ace450249-detailedmukkirorbtailor', 35, '2021-11-17 16:56:08') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450249,   1,      32768) /* ItemType - Caster */
     , (450249,   5,        0) /* EncumbranceVal */
     , (450249,   9,   16777216) /* ValidLocations - Held */
     , (450249,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450249,  18,          1) /* UiEffects - Magical */
     , (450249,  19,       20) /* Value */
     , (450249,  46,        512) /* DefaultCombatStyle - Magic */
     , (450249,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450249,  94,         16) /* TargetType - Creature */
     , (450249, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450249,  11, True ) /* IgnoreCollisions */
     , (450249,  13, True ) /* Ethereal */
     , (450249,  14, True ) /* GravityStatus */
     , (450249,  19, True ) /* Attackable */
     , (450249,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450249,   5,   -0.05) /* ManaRate */
     , (450249,  29,       1) /* WeaponDefense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450249,   1, 'Detailed Mukkir Orb') /* Name */
     , (450249,  16, 'A casting device fancifully crafted in the shape of a Mukkir''s head.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450249,   1, 0x020014D1) /* Setup */
     , (450249,   3, 0x20000014) /* SoundTable */
     , (450249,   8, 0x0600621C) /* Icon */
     , (450249,  22, 0x3400002B) /* PhysicsEffectTable */

