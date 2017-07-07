USE `ace_world`;

/* Create new generator link table */
DROP TABLE IF EXISTS `ace_object_generator_link`;
CREATE TABLE `ace_object_generator_link` (
  `aceObjectId` int(10) unsigned NOT NULL,
  `index` tinyint(3) unsigned NOT NULL,
  `generatorWeenieClassId` int(10) unsigned NOT NULL,
  `generatorWeight` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`aceObjectId`,`index`),
  KEY `idx_generator_link__AceObject` (`generatorWeenieClassId`),
  CONSTRAINT `fk_generator_link__AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE,
  CONSTRAINT `fk_generator_link__AceWeenieClass` FOREIGN KEY (`generatorWeenieClassId`) REFERENCES `ace_weenie_class` (`weenieClassId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

   
/* Generator Setup Variables */
SET @weenieClassId = 893;
SET @weenieClassDescription = 'drudgeskulkergen';
SET @name = 'Drudge Skulker Generator';

SET @ActivationCreateClass  = 7; /* ActivationCreateClass = wcid to generate */
SET @MaxGeneratedObjects    = 1;
SET @GeneratorType          = 2;
SET @GeneratorTimeType      = 0;
SET @GeneratorProbability   = 100;

SET @RegenerationInterval   = 120; /* RegenerationInterval in seconds */

/* Delete bad or outdated stuff */
DELETE FROM ace_weenie_class
WHERE weenieClassId = @weenieClassId; /* Unique WCID */

DELETE FROM ace_weenie_class
WHERE weenieClassDescription = @weenieClassDescription; /* Unique Name */

/* Insert new stuff */
INSERT INTO ace_weenie_class
	(weenieClassId,
	weenieClassDescription)
VALUES 
	(@weenieClassId, @weenieClassDescription);
	
INSERT INTO ace_object
	(aceObjectId,
	aceObjectDescriptionFlags,
	weenieClassId)
VALUES
	(@weenieClassId, 0, @weenieClassId);
    
INSERT INTO ace_object_properties_did
	(aceObjectId,
	didPropertyId,
	propertyValue)
VALUES 
	(@weenieClassId, 1, 33555051),
	(@weenieClassId, 8, 100667494);

INSERT INTO ace_object_properties_int
	(aceObjectId,
	intPropertyId,
	propertyValue)
VALUES 
	(@weenieClassId, 1, 0),                           /* ItemType */
	(@weenieClassId, 81, @MaxGeneratedObjects),       /* MaxGeneratedObjects */
	(@weenieClassId, 93, 1040),                       /* PhysicsState = 1040 */
	(@weenieClassId, 100, @GeneratorType),            /* GeneratorType */
	(@weenieClassId, 104, @ActivationCreateClass),    /* ActivationCreateClass = wcid to generate */
	(@weenieClassId, 142, @GeneratorTimeType),        /* GeneratorTimeType */
	(@weenieClassId, 9006, @GeneratorProbability);    /* GeneratorProbability */
    
INSERT INTO ace_object_properties_double
	(aceObjectId,
	dblPropertyId,
	propertyValue)
VALUES 
	(@weenieClassId, 41, @RegenerationInterval); /* RegenerationInterval in seconds */
    
INSERT INTO ace_object_properties_bool 
	(aceObjectId,
	boolPropertyId,
	propertyValue)
VALUES 
    (@weenieClassId, 1, True),
	(@weenieClassId, 11, True),
    (@weenieClassId, 14, True),
    (@weenieClassId, 19, True),
    (@weenieClassId, 26, True),
	(@weenieClassId, 32, True);
    
INSERT INTO ace_object_properties_string 
	(aceObjectId,
	strPropertyId,
	propertyValue)
VALUES 
	(@weenieClassId, 1, @name);
	
/* Add generator instances */
SET @landblockRaw   = 2847145993;
SET @posX           = 40.729305;
SET @posY           = 18.956450;
SET @posZ           = 85.265892;
SET @qW             = 0.903499;
SET @qX             = 0.000000;
SET @qY             = 0.000000;
SET @qZ             = 0.428589;
   
/* Insert new generator instance */
INSERT INTO ace_object
	(aceObjectDescriptionFlags,
    weenieClassId)
SELECT 
    aceObjectDescriptionFlags,
    weenieClassId
FROM ace_object
WHERE aceObjectId = @weenieClassId;

SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_properties_did WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_properties_did SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;

SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_properties_int WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_properties_int SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;

SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_properties_double WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_properties_double SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;

SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_properties_bool WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_properties_bool SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;

SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_properties_string WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_properties_string SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;

/*
SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_generator_link WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_generator_link SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;
*/

INSERT INTO ace_position 
	(aceObjectId,
	positionType,
	landblockRaw,
	posX,
	posY,
	posZ,
	qW,
	qX,
	qY,
	qZ)
VALUES 
	(last_insert_id(), 1, @landblockRaw, @posX, @posY, @posZ, @qW, @qX, @qY, @qZ);

    
/* Generator Setup Variables */
SET @weenieClassId = 954;
SET @weenieClassDescription = 'drudgesneakergen';
SET @name = 'Drudge Sneaker Generator';

SET @ActivationCreateClass  = 940; /* ActivationCreateClass = wcid to generate */
SET @MaxGeneratedObjects    = 1;
SET @GeneratorType          = 2;
SET @GeneratorTimeType      = 0;
SET @GeneratorProbability   = 100;

SET @RegenerationInterval   = 120; /* RegenerationInterval in seconds */

/* Delete bad or outdated stuff */
DELETE FROM ace_weenie_class
WHERE weenieClassId = @weenieClassId; /* Unique WCID */

DELETE FROM ace_weenie_class
WHERE weenieClassDescription = @weenieClassDescription; /* Unique Name */

/* Insert new stuff */
INSERT INTO ace_weenie_class
	(weenieClassId,
	weenieClassDescription)
VALUES 
	(@weenieClassId, @weenieClassDescription);
	
INSERT INTO ace_object
	(aceObjectId,
	aceObjectDescriptionFlags,
	weenieClassId)
VALUES
	(@weenieClassId, 0, @weenieClassId);
    
INSERT INTO ace_object_properties_did
	(aceObjectId,
	didPropertyId,
	propertyValue)
VALUES 
	(@weenieClassId, 1, 33555051),
	(@weenieClassId, 8, 100667494);

INSERT INTO ace_object_properties_int
	(aceObjectId,
	intPropertyId,
	propertyValue)
VALUES 
	(@weenieClassId, 1, 0),                           /* ItemType */
	(@weenieClassId, 81, @MaxGeneratedObjects),       /* MaxGeneratedObjects */
	(@weenieClassId, 93, 1040),                       /* PhysicsState = 1040 */
	(@weenieClassId, 100, @GeneratorType),            /* GeneratorType */
	(@weenieClassId, 104, @ActivationCreateClass),    /* ActivationCreateClass = wcid to generate */
	(@weenieClassId, 142, @GeneratorTimeType),        /* GeneratorTimeType */
	(@weenieClassId, 9006, @GeneratorProbability);    /* GeneratorProbability */
    
INSERT INTO ace_object_properties_double
	(aceObjectId,
	dblPropertyId,
	propertyValue)
VALUES 
	(@weenieClassId, 41, @RegenerationInterval); /* RegenerationInterval in seconds */
    
INSERT INTO ace_object_properties_bool 
	(aceObjectId,
	boolPropertyId,
	propertyValue)
VALUES 
    (@weenieClassId, 1, True),
	(@weenieClassId, 11, True),
    (@weenieClassId, 14, True),
    (@weenieClassId, 19, True),
    (@weenieClassId, 26, True),
	(@weenieClassId, 32, True);
    
INSERT INTO ace_object_properties_string 
	(aceObjectId,
	strPropertyId,
	propertyValue)
VALUES 
	(@weenieClassId, 1, @name);
	
   
/* Generator Setup Variables */
SET @weenieClassId = 4172;
SET @weenieClassDescription = 'drudgecampgen';
SET @name = 'Drudge Camp Generator';

SET @ActivationCreateClass  = 0; /* ActivationCreateClass = wcid to generate */
SET @MaxGeneratedObjects    = 3;
SET @GeneratorType          = 2;
SET @GeneratorTimeType      = 0;
SET @GeneratorProbability   = 100;

SET @RegenerationInterval   = 120; /* RegenerationInterval in seconds */

/* Delete bad or outdated stuff */
DELETE FROM ace_weenie_class
WHERE weenieClassId = @weenieClassId; /* Unique WCID */

DELETE FROM ace_weenie_class
WHERE weenieClassDescription = @weenieClassDescription; /* Unique Name */

/* Insert new stuff */
INSERT INTO ace_weenie_class
	(weenieClassId,
	weenieClassDescription)
VALUES 
	(@weenieClassId, @weenieClassDescription);
	
INSERT INTO ace_object
	(aceObjectId,
	aceObjectDescriptionFlags,
	weenieClassId)
VALUES
	(@weenieClassId, 0, @weenieClassId);
    
INSERT INTO ace_object_properties_did
	(aceObjectId,
	didPropertyId,
	propertyValue)
VALUES 
	(@weenieClassId, 1, 33555051),
	(@weenieClassId, 8, 100667494);

INSERT INTO ace_object_properties_int
	(aceObjectId,
	intPropertyId,
	propertyValue)
VALUES 
	(@weenieClassId, 1, 0),                           /* ItemType */
	(@weenieClassId, 81, @MaxGeneratedObjects),       /* MaxGeneratedObjects */
	(@weenieClassId, 93, 1040),                       /* PhysicsState = 1040 */
	(@weenieClassId, 100, @GeneratorType),            /* GeneratorType */
	(@weenieClassId, 104, @ActivationCreateClass),    /* ActivationCreateClass = wcid to generate */
	(@weenieClassId, 142, @GeneratorTimeType),        /* GeneratorTimeType */
	(@weenieClassId, 9006, @GeneratorProbability);    /* GeneratorProbability */
    
INSERT INTO ace_object_properties_double
	(aceObjectId,
	dblPropertyId,
	propertyValue)
VALUES 
	(@weenieClassId, 41, @RegenerationInterval); /* RegenerationInterval in seconds */
    
INSERT INTO ace_object_properties_bool 
	(aceObjectId,
	boolPropertyId,
	propertyValue)
VALUES 
    (@weenieClassId, 1, True),
	(@weenieClassId, 11, True),
    (@weenieClassId, 14, True),
    (@weenieClassId, 19, True),
    (@weenieClassId, 26, True),
	(@weenieClassId, 32, True);
    
INSERT INTO ace_object_properties_string 
	(aceObjectId,
	strPropertyId,
	propertyValue)
VALUES 
	(@weenieClassId, 1, @name);
    
INSERT INTO ace_object_generator_link
	(aceObjectId,
	`index`,
	generatorWeenieClassId,
	generatorWeight)
VALUES
	(@weenieClassId, 1, 893, 60),
	(@weenieClassId, 2, 954, 40);
	
/* Add generator instances */
SET @landblockRaw   = 2847145993;
SET @posX           = 45.804615;
SET @posY           = 7.771394;
SET @posZ           = 88.062149;
SET @qW             = 0.701760;
SET @qX             = 0.000000;
SET @qY             = 0.000000;
SET @qZ             = 0.712413;
   
/* Insert new generator instance */
INSERT INTO ace_object
	(aceObjectDescriptionFlags,
    weenieClassId)
SELECT 
    aceObjectDescriptionFlags,
    weenieClassId
FROM ace_object
WHERE aceObjectId = @weenieClassId;

SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_properties_did WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_properties_did SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;

SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_properties_int WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_properties_int SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;

SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_properties_double WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_properties_double SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;

SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_properties_bool WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_properties_bool SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;

SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_properties_string WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_properties_string SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;


SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_generator_link WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_generator_link SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;


INSERT INTO ace_position 
	(aceObjectId,
	positionType,
	landblockRaw,
	posX,
	posY,
	posZ,
	qW,
	qX,
	qY,
	qZ)
VALUES 
	(last_insert_id(), 1, @landblockRaw, @posX, @posY, @posZ, @qW, @qX, @qY, @qZ);
    
 
/* Generator Setup Variables */
SET @weenieClassId = 1996;
SET @weenieClassDescription = 'lowaaluviangen';
SET @name = 'Low A Aluvian Generator';

SET @ActivationCreateClass  = 0; /* ActivationCreateClass = wcid to generate */
SET @MaxGeneratedObjects    = 10;
SET @GeneratorType          = 1;
SET @GeneratorTimeType      = 0;
SET @GeneratorProbability   = 100;

SET @RegenerationInterval   = 120; /* RegenerationInterval in seconds */

/* Delete bad or outdated stuff */
DELETE FROM ace_weenie_class
WHERE weenieClassId = @weenieClassId; /* Unique WCID */

DELETE FROM ace_weenie_class
WHERE weenieClassDescription = @weenieClassDescription; /* Unique Name */

/* Insert new stuff */
INSERT INTO ace_weenie_class
	(weenieClassId,
	weenieClassDescription)
VALUES 
	(@weenieClassId, @weenieClassDescription);
	
INSERT INTO ace_object
	(aceObjectId,
	aceObjectDescriptionFlags,
	weenieClassId)
VALUES
	(@weenieClassId, 0, @weenieClassId);
    
INSERT INTO ace_object_properties_did
	(aceObjectId,
	didPropertyId,
	propertyValue)
VALUES 
	(@weenieClassId, 1, 33555051),
	(@weenieClassId, 8, 100667494);

INSERT INTO ace_object_properties_int
	(aceObjectId,
	intPropertyId,
	propertyValue)
VALUES 
	(@weenieClassId, 1, 0),                           /* ItemType */
	(@weenieClassId, 81, @MaxGeneratedObjects),       /* MaxGeneratedObjects */
	(@weenieClassId, 93, 1040),                       /* PhysicsState = 1040 */
	(@weenieClassId, 100, @GeneratorType),            /* GeneratorType */
	(@weenieClassId, 104, @ActivationCreateClass),    /* ActivationCreateClass = wcid to generate */
	(@weenieClassId, 142, @GeneratorTimeType),        /* GeneratorTimeType */
	(@weenieClassId, 9006, @GeneratorProbability);    /* GeneratorProbability */
    
INSERT INTO ace_object_properties_double
	(aceObjectId,
	dblPropertyId,
	propertyValue)
VALUES 
	(@weenieClassId, 41, @RegenerationInterval); /* RegenerationInterval in seconds */
    
INSERT INTO ace_object_properties_bool 
	(aceObjectId,
	boolPropertyId,
	propertyValue)
VALUES 
    (@weenieClassId, 1, True),
	(@weenieClassId, 11, True),
    (@weenieClassId, 14, True),
    (@weenieClassId, 19, True),
    (@weenieClassId, 26, True),
	(@weenieClassId, 32, True);
    
INSERT INTO ace_object_properties_string 
	(aceObjectId,
	strPropertyId,
	propertyValue)
VALUES 
	(@weenieClassId, 1, @name);
    
INSERT INTO ace_object_generator_link
	(aceObjectId,
	`index`,
	generatorWeenieClassId,
	generatorWeight)
VALUES
	(@weenieClassId, 1, 4172, 100);
	
/* Add generator instances */
SET @landblockRaw   = 2847080471;
SET @posX           = 50.559231;
SET @posY           = 157.634384;
SET @posZ           = 94.218269;
SET @qW             = -0.347880;
SET @qX             = 0.000000;
SET @qY             = 0.000000;
SET @qZ             = -0.937539;
   
/* Insert new generator instance */
INSERT INTO ace_object
	(aceObjectDescriptionFlags,
    weenieClassId)
SELECT 
    aceObjectDescriptionFlags,
    weenieClassId
FROM ace_object
WHERE aceObjectId = @weenieClassId;

SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_properties_did WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_properties_did SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;

SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_properties_int WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_properties_int SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;

SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_properties_double WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_properties_double SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;

SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_properties_bool WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_properties_bool SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;

SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_properties_string WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_properties_string SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;


SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_generator_link WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_generator_link SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;


INSERT INTO ace_position 
	(aceObjectId,
	positionType,
	landblockRaw,
	posX,
	posY,
	posZ,
	qW,
	qX,
	qY,
	qZ)
VALUES 
	(last_insert_id(), 1, @landblockRaw, @posX, @posY, @posZ, @qW, @qX, @qY, @qZ);


/* Add generator instances */
SET @landblockRaw   = 2830303296;
SET @posX           = 189.942215;
SET @posY           = 170.000732;
SET @posZ           = 92.980865;
SET @qW             = 0;
SET @qX             = 0.000000;
SET @qY             = 0.000000;
SET @qZ             = 0;
   
/* Insert new generator instance */
INSERT INTO ace_object
	(aceObjectDescriptionFlags,
    weenieClassId)
SELECT 
    aceObjectDescriptionFlags,
    weenieClassId
FROM ace_object
WHERE aceObjectId = @weenieClassId;

SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_properties_did WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_properties_did SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;

SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_properties_int WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_properties_int SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;

SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_properties_double WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_properties_double SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;

SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_properties_bool WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_properties_bool SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;

SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_properties_string WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_properties_string SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;


SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_generator_link WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_generator_link SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;


INSERT INTO ace_position 
	(aceObjectId,
	positionType,
	landblockRaw,
	posX,
	posY,
	posZ,
	qW,
	qX,
	qY,
	qZ)
VALUES 
	(last_insert_id(), 1, @landblockRaw, @posX, @posY, @posZ, @qW, @qX, @qY, @qZ);

    
/* Add generator instances */
SET @landblockRaw   = 2813526055;
SET @posX           = 103.956352;
SET @posY           = 156.198883;
SET @posZ           = 30.004999;
SET @qW             = 0;
SET @qX             = 0.000000;
SET @qY             = 0.000000;
SET @qZ             = 0;
   
/* Insert new generator instance */
INSERT INTO ace_object
	(aceObjectDescriptionFlags,
    weenieClassId)
SELECT 
    aceObjectDescriptionFlags,
    weenieClassId
FROM ace_object
WHERE aceObjectId = @weenieClassId;

SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_properties_did WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_properties_did SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;

SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_properties_int WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_properties_int SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;

SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_properties_double WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_properties_double SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;

SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_properties_bool WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_properties_bool SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;

SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_properties_string WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_properties_string SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;


SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp SELECT * from ace_object_generator_link WHERE aceObjectId = @weenieClassId;
UPDATE tmp SET aceObjectId = last_insert_id();
INSERT INTO ace_object_generator_link SELECT tmp.* FROM tmp;
DROP TEMPORARY TABLE tmp;
SET SQL_SAFE_UPDATES = 1;


INSERT INTO ace_position 
	(aceObjectId,
	positionType,
	landblockRaw,
	posX,
	posY,
	posZ,
	qW,
	qX,
	qY,
	qZ)
VALUES 
	(last_insert_id(), 1, @landblockRaw, @posX, @posY, @posZ, @qW, @qX, @qY, @qZ);
