DELETE FROM `weenie` WHERE `class_Id` = 38942;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (38942, 'ace38942-grandcasinochest', 20, '2021-11-01 00:00:00') /* Chest */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (38942,   1,        512) /* ItemType - Container */
     , (38942,   5,       9000) /* EncumbranceVal */
     , (38942,   6,         -1) /* ItemsCapacity */
     , (38942,   7,         -1) /* ContainersCapacity */
     , (38942,   8,       3000) /* Mass */
     , (38942,  16,         48) /* ItemUseable - ViewedRemote */
     , (38942,  19,       2500) /* Value */
     , (38942,  37,         30) /* ResistItemAppraisal */
     , (38942,  38,       9999) /* ResistLockpick */
     , (38942,  81,          4) /* MaxGeneratedObjects */
     , (38942,  82,          4) /* InitGeneratedObjects */
     , (38942,  93,       1048) /* PhysicsState - ReportCollisions, IgnoreCollisions, Gravity */
     , (38942,  96,        500) /* EncumbranceCapacity */
     , (38942, 100,          1) /* GeneratorType - Relative */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (38942,   1, True ) /* Stuck */
     , (38942,   2, False) /* Open */
     , (38942,   3, True ) /* Locked */
     , (38942,  12, True ) /* ReportCollisions */
     , (38942,  13, False) /* Ethereal */
     , (38942,  19, True ) /* Attackable */
     , (38942,  33, False) /* ResetMessagePending */
     , (38942,  34, False) /* DefaultOpen */
     , (38942,  35, True ) /* DefaultLocked */
     , (38942,  86, True ) /* ChestRegenOnClose */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (38942,  39,       3) /* DefaultScale */
     , (38942,  41,      60) /* RegenerationInterval */
     , (38942,  43,       1) /* GeneratorRadius */
     , (38942,  54,       1) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (38942,   1, 'Grand Casino Chest') /* Name */
     , (38942,  12, 'grandcasinokey') /* LockCode */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (38942,   1, 0x02000A23) /* Setup */
     , (38942,   2, 0x09000004) /* MotionTable */
     , (38942,   3, 0x20000021) /* SoundTable */
     , (38942,   7, 0x100002C0) /* ClothingBase */
     , (38942,   8, 0x06001FF8) /* Icon */
     , (38942,  22, 0x3400002B) /* PhysicsEffectTable */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (38942, -1, 349, 1, 1, 1, 2, 72, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate RANDOMLY GENERATED TREASURE from Loot Tier 6 from Death Treasure Table id: 349 (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: ContainTreasure */
     , (38942, -1, 1001, 1, 1, 1, 2, 72, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate RANDOMLY GENERATED TREASURE from Loot Tier 7 from Death Treasure Table id: 1001 (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: ContainTreasure */
     , (38942, 0.0025, 36619, 1, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Foolproof (36619) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (38942, 0.005, 36620, 1, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Foolproof (36620) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (38942, 0.0075, 36621, 1, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Foolproof (36621) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (38942, 0.01, 36622, 1, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Foolproof (36622) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (38942, 0.0125, 36623, 1, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Foolproof (36623) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (38942, 0.015, 36624, 1, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Foolproof (36624) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (38942, 0.0175, 36625, 1, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Foolproof (36625) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (38942, 0.02, 36634, 1, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Foolproof (36634) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (38942, 0.0225, 36626, 1, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Foolproof (36626) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (38942, 0.025, 36627, 1, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Foolproof (36627) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (38942, 0.0275, 36628, 1, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Foolproof (36628) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (38942, 0.03, 36635, 1, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Foolproof (36635) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (38942, 0.0325, 36636, 1, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Foolproof (36636) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (38942, 0.1075, 35383, 1, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Ancient Mhoire Coin (35383) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (38942, 0.2075, 36518, 1, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Colosseum Coin (36518) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (38942, 0.2825, 36376, 1, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Small Olthoi Venom Sac (36376) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (38942, 0.4, 44240, 1, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate A'nekshay Token (44240) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (38942, 0.7, 38456, 1, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Mana Forge Key (38456) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (38942, 0.875, 34277, 1, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Ancient Falatacot Trinket (34277) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */
     , (38942, 1, 43142, 1, 1, 1, 2, 8, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Ornate Gear Marker (43142) (x1 up to max of 1) - Regenerate upon PickUp - Location to (re)Generate: Contain */;
