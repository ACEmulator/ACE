USE `ace_world`;

CREATE TABLE `ace_object_generator_profile` (
  `profileId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT '0',
  `probability` float NOT NULL DEFAULT '1',
  `weenieClassId` int(10) unsigned NOT NULL DEFAULT '0',
  `delay` float DEFAULT '0',
  `initCreate` int(10) unsigned NOT NULL DEFAULT '1',
  `maxCreate` int(10) unsigned NOT NULL DEFAULT '1',
  `whenCreate` int(10) unsigned NOT NULL DEFAULT '2',
  `whereCreate` int(10) unsigned NOT NULL DEFAULT '4',
  `stackSize` int(10) DEFAULT NULL,
  `paletteId` int(10) unsigned DEFAULT NULL,
  `shade` float DEFAULT NULL,
  `landblockRaw` int(10) unsigned DEFAULT NULL,
  `posX` float DEFAULT NULL,
  `posY` float DEFAULT NULL,
  `posZ` float DEFAULT NULL,
  `qW` float DEFAULT NULL,
  `qX` float DEFAULT NULL,
  `qY` float DEFAULT NULL,
  `qZ` float DEFAULT NULL,
  PRIMARY KEY (`profileId`),
  KEY `fk_gen_ao_idx` (`aceObjectId`),
  KEY `fk_gen_weenie_idx` (`weenieClassId`),
  CONSTRAINT `fk_gen_ao` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE,
  CONSTRAINT `fk_gen_weenie` FOREIGN KEY (`weenieClassId`) REFERENCES `ace_weenie_class` (`weenieClassId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

USE `ace_world`;

ALTER TABLE `ace_world`.`ace_landblock` 
ADD COLUMN `linkSlot` INT(5) NULL DEFAULT NULL AFTER `qZ`,
ADD COLUMN `linkSource` TINYINT NULL DEFAULT NULL AFTER `linkSlot`,
ADD INDEX `linkSlot` (`linkSlot` ASC);

USE `ace_world`;

/* Delete bad or outdated stuff */
SET @weenieClassId = 99999;
SET @weenieClassDescription = 'ace99999-counterreset';

DELETE FROM ace_weenie_class
WHERE weenieClassId = @weenieClassId; /* Unique WCID */

DELETE FROM ace_weenie_class
WHERE weenieClassId = 5485;

DELETE FROM ace_weenie_class
WHERE weenieClassDescription = @weenieClassDescription; /* Unique Name */


/* Weenie - MiscObjects - Place Holder Object (3666) */
/* Weenie Setup Variables */
SET @weenieClassId = 3666;
SET @weenieClassDescription = 'placeholder';
SET @name = 'Place Holder Object';
SET @setupDID = 33554700;
SET @iconDID = 100667487;
DELETE FROM ace_weenie_class WHERE weenieClassId = @weenieClassId;

INSERT INTO ace_weenie_class (`weenieClassId`, `weenieClassDescription`)
VALUES (@weenieClassId, @weenieClassDescription);

INSERT INTO `ace_object` (`aceObjectId`, `aceObjectDescriptionFlags`, `weenieClassId`, `weenieHeaderFlags`, `weenieHeaderFlags2`, `currentMotionState`, `physicsDescriptionFlag`)
VALUES (@weenieClassId, 0, @weenieClassId, NULL, NULL, NULL, NULL);

INSERT INTO `ace_object_properties_string` (`aceObjectId`, `strPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 1, @name) /* NAME_STRING */;

INSERT INTO `ace_object_properties_did` (`aceObjectId`, `didPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 1, @setupDID) /* SETUP_DID */
     , (@weenieClassId, 8, @iconDID) /* ICON_DID */;

INSERT INTO `ace_object_properties_int` (`aceObjectId`, `intPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 93, 1044) /* PHYSICS_STATE_INT */
     , (@weenieClassId, 9007, 1) /* Generic_WeenieType */;

INSERT INTO `ace_object_properties_bool` (`aceObjectId`, `boolPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 1, True) /* STUCK_BOOL */
     , (@weenieClassId, 18, True) /* VISIBILITY_BOOL */;

     
/* Weenie - Generators - Linkable Monster Generator (1154) */
/* Generator Setup Variables */
SET @weenieClassId = 1154;
SET @weenieClassDescription = 'linkmonstergen';
SET @name = 'Linkable Monster Generator';
SET @setupDID = 33555051;
SET @iconDID = 100667494;
SET @maxGeneratedObjects = 0;
SET @initGeneratedObjects = 0;
SET @regenerationInterval = 60.0; 
DELETE FROM ace_weenie_class WHERE weenieClassId = @weenieClassId;

DELETE FROM ace_weenie_class
WHERE weenieClassDescription = @weenieClassDescription;

INSERT INTO ace_weenie_class (`weenieClassId`, `weenieClassDescription`)
VALUES (@weenieClassId, @weenieClassDescription);

INSERT INTO `ace_object` (`aceObjectId`, `aceObjectDescriptionFlags`, `weenieClassId`, `weenieHeaderFlags`, `weenieHeaderFlags2`, `currentMotionState`, `physicsDescriptionFlag`)
VALUES (@weenieClassId, 0, @weenieClassId, NULL, NULL, NULL, NULL);

INSERT INTO `ace_object_properties_string` (`aceObjectId`, `strPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 1, @name) /* NAME_STRING */;

INSERT INTO `ace_object_properties_did` (`aceObjectId`, `didPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 1, @setupDID) /* SETUP_DID */
     , (@weenieClassId, 8, @iconDID) /* ICON_DID */;

INSERT INTO `ace_object_properties_int` (`aceObjectId`, `intPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 1, 0) /* ITEM_TYPE_INT */
     , (@weenieClassId, 66, 1) /* CHECKPOINT_STATUS_INT */
     , (@weenieClassId, 81, @maxGeneratedObjects) /* MAX_GENERATED_OBJECTS_INT */
     , (@weenieClassId, 82, @initGeneratedObjects) /* INIT_GENERATED_OBJECTS_INT */
     , (@weenieClassId, 93, 1044) /* PHYSICS_STATE_INT */
     , (@weenieClassId, 9007, 1) /* Generic_WeenieType */;

INSERT INTO `ace_object_properties_bool` (`aceObjectId`, `boolPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 1, True) /* STUCK_BOOL */
     , (@weenieClassId, 11, True) /* IGNORE_COLLISIONS_BOOL */
     , (@weenieClassId, 18, True) /* VISIBILITY_BOOL */;
     
INSERT INTO `ace_object_properties_double` (`aceObjectId`, `dblPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 41, @regenerationInterval) /* REGENERATION_INTERVAL_FLOAT */;

INSERT INTO `ace_object_generator_profile` (`aceObjectId`, `probability`, `weenieClassId`, `delay`, `initCreate`, `maxCreate`, `whenCreate`, `whereCreate`, `stackSize`, `paletteId`, `shade`,
    `landblockRaw`,
    `posX`, `posY`, `posZ`,
    `qW`, `qX`, `qY`, `qZ`)
VALUES (@weenieClassId, -1.0, 3666, @regenerationInterval, 1, 1, 2, 4, -1, 0, 0, 0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0);

/* Weenie - Generators - Linkable Item Generator (1542) */
/* Generator Setup Variables */
SET @weenieClassId = 1542;
SET @weenieClassDescription = 'linkitemgen';
SET @name = 'Linkable Item Generator';
SET @setupDID = 33555051;
SET @iconDID = 100667494;
SET @maxGeneratedObjects = 0;
SET @initGeneratedObjects = 0;
SET @regenerationInterval = 60.0; 
DELETE FROM ace_weenie_class WHERE weenieClassId = @weenieClassId;

INSERT INTO ace_weenie_class (`weenieClassId`, `weenieClassDescription`)
VALUES (@weenieClassId, @weenieClassDescription);

INSERT INTO `ace_object` (`aceObjectId`, `aceObjectDescriptionFlags`, `weenieClassId`, `weenieHeaderFlags`, `weenieHeaderFlags2`, `currentMotionState`, `physicsDescriptionFlag`)
VALUES (@weenieClassId, 0, @weenieClassId, NULL, NULL, NULL, NULL);

INSERT INTO `ace_object_properties_string` (`aceObjectId`, `strPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 1, @name) /* NAME_STRING */;

INSERT INTO `ace_object_properties_did` (`aceObjectId`, `didPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 1, @setupDID) /* SETUP_DID */
     , (@weenieClassId, 8, @iconDID) /* ICON_DID */;

INSERT INTO `ace_object_properties_int` (`aceObjectId`, `intPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 1, 0) /* ITEM_TYPE_INT */
     , (@weenieClassId, 66, 1) /* CHECKPOINT_STATUS_INT */
     , (@weenieClassId, 81, @maxGeneratedObjects) /* MAX_GENERATED_OBJECTS_INT */
     , (@weenieClassId, 82, @initGeneratedObjects) /* INIT_GENERATED_OBJECTS_INT */
     , (@weenieClassId, 93, 1044) /* PHYSICS_STATE_INT */
     , (@weenieClassId, 9007, 1) /* Generic_WeenieType */;

INSERT INTO `ace_object_properties_bool` (`aceObjectId`, `boolPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 1, True) /* STUCK_BOOL */
     , (@weenieClassId, 11, True) /* IGNORE_COLLISIONS_BOOL */
     , (@weenieClassId, 18, True) /* VISIBILITY_BOOL */;
     
INSERT INTO `ace_object_properties_double` (`aceObjectId`, `dblPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 41, @regenerationInterval) /* REGENERATION_INTERVAL_FLOAT */;

INSERT INTO `ace_object_generator_profile` (`aceObjectId`, `probability`, `weenieClassId`, `delay`, `initCreate`, `maxCreate`, `whenCreate`, `whereCreate`, `stackSize`, `paletteId`, `shade`,
    `landblockRaw`,
    `posX`, `posY`, `posZ`,
    `qW`, `qX`, `qY`, `qZ`)
VALUES (@weenieClassId, -1.0, 3666, @regenerationInterval, 1, 1, 2, 4, -1, 0, 0, 0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0);


/* Weenie - Generators - Linkable Item Generator (10s) (15759) */
/* Generator Setup Variables */
SET @weenieClassId = 15759;
SET @weenieClassDescription = 'linkitemgen10seconds';
SET @name = 'Linkable Item Generator (10s)';
SET @setupDID = 33555051;
SET @iconDID = 100667494;
SET @maxGeneratedObjects = 0;
SET @initGeneratedObjects = 0;
SET @regenerationInterval = 10.0; 
DELETE FROM ace_weenie_class WHERE weenieClassId = @weenieClassId;

INSERT INTO ace_weenie_class (`weenieClassId`, `weenieClassDescription`)
VALUES (@weenieClassId, @weenieClassDescription);

INSERT INTO `ace_object` (`aceObjectId`, `aceObjectDescriptionFlags`, `weenieClassId`, `weenieHeaderFlags`, `weenieHeaderFlags2`, `currentMotionState`, `physicsDescriptionFlag`)
VALUES (@weenieClassId, 0, @weenieClassId, NULL, NULL, NULL, NULL);

INSERT INTO `ace_object_properties_string` (`aceObjectId`, `strPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 1, @name) /* NAME_STRING */;

INSERT INTO `ace_object_properties_did` (`aceObjectId`, `didPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 1, @setupDID) /* SETUP_DID */
     , (@weenieClassId, 8, @iconDID) /* ICON_DID */;

INSERT INTO `ace_object_properties_int` (`aceObjectId`, `intPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 1, 0) /* ITEM_TYPE_INT */
     , (@weenieClassId, 66, 1) /* CHECKPOINT_STATUS_INT */
     , (@weenieClassId, 81, @maxGeneratedObjects) /* MAX_GENERATED_OBJECTS_INT */
     , (@weenieClassId, 82, @initGeneratedObjects) /* INIT_GENERATED_OBJECTS_INT */
     , (@weenieClassId, 93, 1044) /* PHYSICS_STATE_INT */
     , (@weenieClassId, 9007, 1) /* Generic_WeenieType */;

INSERT INTO `ace_object_properties_bool` (`aceObjectId`, `boolPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 1, True) /* STUCK_BOOL */
     , (@weenieClassId, 11, True) /* IGNORE_COLLISIONS_BOOL */
     , (@weenieClassId, 18, True) /* VISIBILITY_BOOL */;
     
INSERT INTO `ace_object_properties_double` (`aceObjectId`, `dblPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 41, @regenerationInterval) /* REGENERATION_INTERVAL_FLOAT */;

INSERT INTO `ace_object_generator_profile` (`aceObjectId`, `probability`, `weenieClassId`, `delay`, `initCreate`, `maxCreate`, `whenCreate`, `whereCreate`, `stackSize`, `paletteId`, `shade`,
    `landblockRaw`,
    `posX`, `posY`, `posZ`,
    `qW`, `qX`, `qY`, `qZ`)
VALUES (@weenieClassId, -1.0, 3666, @regenerationInterval, 1, 1, 2, 4, -1, 0, 0, 0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0);


/* Weenie - Generators - Linkable Monster Generator (10s)(28282) */
/* Generator Setup Variables */
SET @weenieClassId = 28282;
SET @weenieClassDescription = 'linkmonstergen10seconds';
SET @name = 'Linkable Monster Generator (10s)';
SET @setupDID = 33555051;
SET @iconDID = 100667494;
SET @maxGeneratedObjects = 0;
SET @initGeneratedObjects = 0;
SET @regenerationInterval = 10.0; 
SET @generatorRadius = 1.0; 
DELETE FROM ace_weenie_class WHERE weenieClassId = @weenieClassId;

INSERT INTO ace_weenie_class (`weenieClassId`, `weenieClassDescription`)
VALUES (@weenieClassId, @weenieClassDescription);

INSERT INTO `ace_object` (`aceObjectId`, `aceObjectDescriptionFlags`, `weenieClassId`, `weenieHeaderFlags`, `weenieHeaderFlags2`, `currentMotionState`, `physicsDescriptionFlag`)
VALUES (@weenieClassId, 0, @weenieClassId, NULL, NULL, NULL, NULL);

INSERT INTO `ace_object_properties_string` (`aceObjectId`, `strPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 1, @name) /* NAME_STRING */;

INSERT INTO `ace_object_properties_did` (`aceObjectId`, `didPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 1, @setupDID) /* SETUP_DID */
     , (@weenieClassId, 8, @iconDID) /* ICON_DID */;

INSERT INTO `ace_object_properties_int` (`aceObjectId`, `intPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 1, 0) /* ITEM_TYPE_INT */
     , (@weenieClassId, 66, 1) /* CHECKPOINT_STATUS_INT */
     , (@weenieClassId, 81, @maxGeneratedObjects) /* MAX_GENERATED_OBJECTS_INT */
     , (@weenieClassId, 82, @initGeneratedObjects) /* INIT_GENERATED_OBJECTS_INT */
     , (@weenieClassId, 93, 1044) /* PHYSICS_STATE_INT */
     , (@weenieClassId, 9007, 1) /* Generic_WeenieType */;

INSERT INTO `ace_object_properties_bool` (`aceObjectId`, `boolPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 1, True) /* STUCK_BOOL */
     , (@weenieClassId, 11, True) /* IGNORE_COLLISIONS_BOOL */
     , (@weenieClassId, 18, True) /* VISIBILITY_BOOL */;
     
INSERT INTO `ace_object_properties_double` (`aceObjectId`, `dblPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 41, @regenerationInterval) /* REGENERATION_INTERVAL_FLOAT */
     , (@weenieClassId, 43, @generatorRadius) /* GENERATOR_RADIUS_FLOAT */;

INSERT INTO `ace_object_generator_profile` (`aceObjectId`, `probability`, `weenieClassId`, `delay`, `initCreate`, `maxCreate`, `whenCreate`, `whereCreate`, `stackSize`, `paletteId`, `shade`,
    `landblockRaw`,
    `posX`, `posY`, `posZ`,
    `qW`, `qX`, `qY`, `qZ`)
VALUES (@weenieClassId, -1.0, 3666, @regenerationInterval, 1, 1, 2, 4, -1, 0, 0, 0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0);


INSERT INTO `ace_landblock` (`weenieClassId`, `landblockRaw`, `posX`, `posY`, `posZ`, `qW`, `qX`, `qY`, `qZ`, `linkSlot`, `linkSource`)
VALUES (15759, CONV('7F0301B1', 16, 10), 19.598906, -29.117907, 0.005000, -0.002319, 0, 0, 0.999997, 1, true);

INSERT INTO `ace_landblock` (`weenieClassId`, `landblockRaw`, `posX`, `posY`, `posZ`, `qW`, `qX`, `qY`, `qZ`, `linkSlot`)
VALUES (13239, 2130903478, 22.2098, -40.2234, 0.67375, 0.102269, 0, 0, -0.994757, 1)
     , (13240, 2130903472, 18.3629, -21.0976, 0, -0.922891, 0, 0, 0.385062, 1)
     , (13241, 2130903478, 17.7939, -41.728, -0.002500013, -0.481744, 0, 0, -0.876312, 1)
     , (5090, 2130903469, 7.739, -30.103, 1.5, 0.702712, 0, 0, -0.711474, 1)
     , (13237, 2130903478, 17.849, -37.9237, 0.079, -0.491793, 0, 0, -0.870712, 1);
     
    
INSERT INTO `ace_landblock` (`weenieClassId`, `landblockRaw`, `posX`, `posY`, `posZ`, `qW`, `qX`, `qY`, `qZ`, `linkSlot`, `linkSource`)
VALUES (28282, CONV('7F03023A', 16, 10), 55.030128, -15.203622, 0.005000, 0.498111, 0, 0, -0.867113, 2, true);

INSERT INTO `ace_landblock` (`weenieClassId`, `landblockRaw`, `posX`, `posY`, `posZ`, `qW`, `qX`, `qY`, `qZ`, `linkSlot`)
VALUES (12698, 2130903610, 60.9185, -20.011, 0.009000003, -0.7153111, 0, 0, -0.6988061, 2)
     , (12698, 2130903705, 86.856, -20.211, 0.009000003, -0.707107, 0, 0, -0.707107, 2)
     , (12698, 2130903682, 83.7375, -6.96577, 0.009000003, 0.487769, 0, 0, 0.872973, 2)
     , (12698, 2130903660, 69.9163, -11.2541, 0.009000003, -0.655506, 0, 0, -0.75519, 2)
     , (12698, 2130903662, 69.3977, -28.0201, 0.009000003, -0.691234, 0, 0, -0.722631, 2)
     , (12698, 2130903684, 79.3977, -28.0201, 0.009000003, -0.691234, 0, 0, -0.722631, 2)
     , (12698, 2130903684, 83.6883, -31.0056, 0.009000003, 0.853479, 0, 0, 0.521127, 2)
     , (12698, 2130903705, 87.9597, -15.8886, 0.009000003, 0.5688431, 0, 0, 0.8224461, 2)
     , (12698, 2130903683, 77.3078, -20.2622, 0.009000003, -0.695954, 0, 0, -0.718086, 2);

     
/* Weenie - Generators - Cow Generator (385) */
/* Generator Setup Variables */
SET @weenieClassId = 385;
SET @weenieClassDescription = 'cowgenerator';
SET @name = 'Cow Generator';
SET @setupDID = 33555051;
SET @iconDID = 100667494;
SET @maxGeneratedObjects = 2;
SET @initGeneratedObjects = 1;
SET @regenerationInterval = 60.0; 
SET @generatorRadius = 1.0; 
DELETE FROM ace_weenie_class WHERE weenieClassId = @weenieClassId;

INSERT INTO ace_weenie_class (`weenieClassId`, `weenieClassDescription`)
VALUES (@weenieClassId, @weenieClassDescription);

INSERT INTO `ace_object` (`aceObjectId`, `aceObjectDescriptionFlags`, `weenieClassId`, `weenieHeaderFlags`, `weenieHeaderFlags2`, `currentMotionState`, `physicsDescriptionFlag`)
VALUES (@weenieClassId, 0, @weenieClassId, NULL, NULL, NULL, NULL);

INSERT INTO `ace_object_properties_string` (`aceObjectId`, `strPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 1, @name) /* NAME_STRING */;

INSERT INTO `ace_object_properties_did` (`aceObjectId`, `didPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 1, @setupDID) /* SETUP_DID */
     , (@weenieClassId, 8, @iconDID) /* ICON_DID */;

INSERT INTO `ace_object_properties_int` (`aceObjectId`, `intPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 1, 0) /* ITEM_TYPE_INT */
     , (@weenieClassId, 81, @maxGeneratedObjects) /* MAX_GENERATED_OBJECTS_INT */
     , (@weenieClassId, 82, @initGeneratedObjects) /* INIT_GENERATED_OBJECTS_INT */
     , (@weenieClassId, 93, 1044) /* PHYSICS_STATE_INT */
     , (@weenieClassId, 9007, 1) /* Generic_WeenieType */;

INSERT INTO `ace_object_properties_bool` (`aceObjectId`, `boolPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 1, True) /* STUCK_BOOL */
     , (@weenieClassId, 11, True) /* IGNORE_COLLISIONS_BOOL */
     , (@weenieClassId, 18, True) /* VISIBILITY_BOOL */;
     
INSERT INTO `ace_object_properties_double` (`aceObjectId`, `dblPropertyId`, `propertyValue`)
VALUES (@weenieClassId, 41, @regenerationInterval) /* REGENERATION_INTERVAL_FLOAT */
     , (@weenieClassId, 43, @generatorRadius) /* GENERATOR_RADIUS_FLOAT */;

INSERT INTO `ace_object_generator_profile` (`aceObjectId`, `probability`, `weenieClassId`, `delay`, `initCreate`, `maxCreate`, `whenCreate`, `whereCreate`, `stackSize`, `paletteId`, `shade`,
    `landblockRaw`,
    `posX`, `posY`, `posZ`,
    `qW`, `qX`, `qY`, `qZ`)
VALUES (@weenieClassId, 0.899999976158142, 14, 300.0, 1, 1, 1, 2, -1, 0, 0, 0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0);

INSERT INTO `ace_landblock` (`weenieClassId`, `landblockRaw`, `posX`, `posY`, `posZ`, `qW`, `qX`, `qY`, `qZ`)
VALUES (385, 2847146023, 111.6, 158.04, 66, 1, 0, 0, 0);
