DELETE FROM `weenie` WHERE `class_Id` = 30052466;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (30052466, 'ace30052466-acidicmist', 13, '2021-11-17 16:56:08') /* HotSpot */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (30052466,   1,        128) /* ItemType - Misc */
     , (30052466,   5,          1) /* EncumbranceVal */
     , (30052466,  16,          1) /* ItemUseable - No */
     , (30052466,  19,          1) /* Value */
     , (30052466,  44,        150) /* Damage */
     , (30052466,  45,         32) /* DamageType - Acid */
     , (30052466,  93,         12) /* PhysicsState - Ethereal, ReportCollisions */
     , (30052466, 119,          0) /* Active */
     , (30052466, 267,         20) /* Lifespan */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (30052466,   1, True ) /* Stuck */
     , (30052466,  11, False) /* IgnoreCollisions */
     , (30052466,  12, True ) /* ReportCollisions */
     , (30052466,  13, True ) /* Ethereal */
     , (30052466,  14, False) /* GravityStatus */
     , (30052466,  24, True ) /* UiHidden */
     , (30052466,  55, True ) /* IsHot */
     , (30052466,  57, False) /* AffectsAis */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (30052466,  22,    0.15) /* DamageVariance */
     , (30052466,  39,       1) /* DefaultScale */
     , (30052466,  76,     0.8) /* Translucency */
     , (30052466, 105,       2) /* HotspotCycleTime */
     , (30052466, 106,     0.2) /* HotspotCycleTimeVariance */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (30052466,   1, 'Acidic Mist') /* Name */
     , (30052466,  17, 'You suffer %i damage from the acidic mist!') /* ActivationTalk */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (30052466,   1, 0x02000B22) /* Setup Fire Cloud- 0x02000B22 Acidic Mist - 0x02001C1A */
     , (30052466,   3, 0x20000052) /* SoundTable */
     , (30052466,   8, 0x06001049) /* Icon */;
