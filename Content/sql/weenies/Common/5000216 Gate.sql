DELETE FROM `weenie` WHERE `class_Id` = 5000216;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (5000216, 'WoodWalls', 19, '2020-01-08 07:59:44') /* Door */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (5000216,   1,        128) /* ItemType - Misc */
     , (5000216,   8,        500) /* Mass */
     , (5000216,  16,          1) /* ItemUseable - No */
     , (5000216,  19,          0) /* Value */
     , (5000216,  38,      50000) /* ResistLockpick */
     , (5000216,  93,          8) /* PhysicsState - ReportCollisions */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (5000216,   1, True ) /* Stuck */
     , (5000216,   2, False) /* Open */
     , (5000216,   3, True ) /* Locked */
     , (5000216,  11, False) /* IgnoreCollisions */
     , (5000216,  12, True ) /* ReportCollisions */
     , (5000216,  13, False) /* Ethereal */
     , (5000216,  14, False) /* GravityStatus */
     , (5000216,  19, True ) /* Attackable */
     , (5000216,  24, True ) /* UiHidden */
     , (5000216,  33, False) /* ResetMessagePending */
     , (5000216,  34, False) /* DefaultOpen */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (5000216,  11,     300) /* ResetInterval */
     , (5000216,  39,       4) /* DefaultScale */
     , (5000216,  54,       2) /* UseRadius */
     , (5000216,  76,       1) /* Translucency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (5000216,   1, 'Gate') /* Name */
     , (5000216,  14, 'Use this item to open it.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (5000216,   1,   33557118) /* Setup */
     , (5000216,   2,  150995139) /* MotionTable */
     , (5000216,   3,  536870947) /* SoundTable */
     , (5000216,   8,  100668183) /* Icon */
     , (5000216,  22,  872415275) /* PhysicsEffectTable */;
