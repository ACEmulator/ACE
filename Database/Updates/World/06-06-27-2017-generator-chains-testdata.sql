USE `ace_world`;

/* Remove potential old stuff */

DELETE FROM ace_weenie_class
WHERE weenieClassId in (893, 954, 1996, 4172);

DELETE FROM ace_object
WHERE aceObjectId in (893, 954, 1996, 4172);

/* Insert new stuff */
INSERT INTO ace_weenie_class
	(weenieClassId,
	weenieClassDescription)
VALUES 
	(893, 'drudgeskulkergen'),
	(954, 'drudgesneakergen'),
	(1996, 'lowaaluviangen'),
	(4172, 'drudgecampgen');
	
INSERT INTO ace_object
	(aceObjectId,
	aceObjectDescriptionFlags,
	weenieClassId,
	weenieHeaderFlags,
	physicsDescriptionFlag)
VALUES
	(893, 148, 893, 0, 32769),
	(954, 148, 893, 0, 32769),
	(1996, 148, 893, 0, 32769),
	(4172, 148, 893, 0, 32769);
	
INSERT INTO ace_object_properties_did
	(aceObjectId,
	didPropertyId,
	propertyValue)
VALUES 
	(893, 1, 33555051),
   (893, 8, 100667494),
	(954, 1, 33555051),
   (954, 8, 100667494),
   (1996, 1, 33555051),
   (1996, 8, 100667494),
	(4172, 1, 33555051),
   (4172, 8, 100667494);

INSERT INTO ace_object_properties_int
	(aceObjectId,
	intPropertyId,
	propertyValue)
VALUES 
	(893, 1, 0),      /* ItemType = 0 */
	(893, 81, 1),     /* MaxGeneratedObjects = 1 */
	(893, 100, 2),    /* GeneratorType = Absolute */
	(893, 104, 7),		/* ActivationCreateClass = 7 (wcid of Drudge Skulker) */
	(893, 142, 0),    /* GeneratorTimeType = Undef */
	(893, 9006, 100), /* GeneratorProbability = 100 */
	(954, 1, 0),      /* ItemType = 0 */
	(954, 81, 1),     /* MaxGeneratedObjects = 1 */
	(954, 100, 2),    /* GeneratorType = Absolute */
	(954, 104, 940),  /* ActivationCreateClass = 940 (wcid of Drudge Sneaker) */
	(954, 142, 0),    /* GeneratorTimeType = Undef */
	(954, 9006, 100), /* GeneratorProbability = 100 */
	(1996, 1, 0),      /* ItemType = 0 */
	(1996, 81, 10),    /* MaxGeneratedObjects = 10 */
	(1996, 100, 1),    /* GeneratorType = Relative */
	(1996, 104, 0),  /* ActivationCreateClass = 0 (linked generators) */
	(1996, 142, 0),    /* GeneratorTimeType = Undef */
	(1996, 9006, 100), /* GeneratorProbability = 100 */
	(4172, 1, 0),      /* ItemType = 0 */
	(4172, 81, 3),     /* MaxGeneratedObjects = 3 */
	(4172, 100, 2),    /* GeneratorType = Absolute */
	(4172, 104, 0),	 /* ActivationCreateClass = 0 (linked generators) */
	(4172, 142, 0),    /* GeneratorTimeType = Undef */
	(4172, 9006, 100); /* GeneratorProbability = 100 */ 

INSERT INTO ace_object_properties_string 
	(aceObjectId,
	strPropertyId,
	propertyValue)
VALUES 
	(893, 1, 'Drudge Skulker Generator'),
	(954, 1, 'Drudge Sneaker Generator'),
	(1996, 1, 'Low A Aluvian Generator'),
	(4172, 1, 'Drudge Camp Generator');
	
INSERT INTO ace_object_generator_link
	(aceObjectId,
	`index`,
	generatorId,
	generatorWeight)
VALUES
	(1996, 1, 4172, 100),
	(4172, 1, 893, 60),
	(4172, 2, 954, 40);
	
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
	(893, 1, 2847145993, 40.729305, 18.956450, 85.265892, 0.903499, 0.000000, 0.000000, 0.428589),
	(1996, 1, 2847080471, 50.559231, 157.634384, 94.218269, -0.347880, 0.000000, 0.000000, -0.937539),
	(4172, 1, 2847145993, 45.804615, 7.771394, 88.062149, 0.701760, 0.000000, 0.000000, 0.712413);
