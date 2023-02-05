DELETE FROM `weenie` WHERE `class_Id` = 4200152;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200152, 'VisibleWoodWalls', 19, '2020-01-08 07:59:44') /* Door */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200152,   1,        128) /* ItemType - Misc */
     , (4200152,   8,        500) /* Mass */
     , (4200152,  16,          1) /* ItemUseable - No */
     , (4200152,  19,          0) /* Value */
     , (4200152,  38,     500000) /* ResistLockpick */
     , (4200152,  93,          8) /* PhysicsState - ReportCollisions */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200152,   1, True ) /* Stuck */
     , (4200152,   2, False) /* Open */
     , (4200152,   3, True ) /* Locked */
     , (4200152,  11, False) /* IgnoreCollisions */
     , (4200152,  12, True ) /* ReportCollisions */
     , (4200152,  13, False) /* Ethereal */
     , (4200152,  14, False) /* GravityStatus */
     , (4200152,  19, True ) /* Attackable */
     , (4200152,  24, True ) /* UiHidden */
     , (4200152,  33, False) /* ResetMessagePending */
     , (4200152,  34, False) /* DefaultOpen */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200152,  11,     300) /* ResetInterval */
     , (4200152,  39,       4) /* DefaultScale */
     , (4200152,  54,       2) /* UseRadius */
;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200152,   1, 'Gate') /* Name */
     , (4200152,  14, 'Use this item to open it.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200152,   1,   33557118) /* Setup */
     , (4200152,   2,  150995139) /* MotionTable */
     , (4200152,   3,  536870947) /* SoundTable */
     , (4200152,   8,  100668183) /* Icon */
     , (4200152,  22,  872415275) /* PhysicsEffectTable */;
