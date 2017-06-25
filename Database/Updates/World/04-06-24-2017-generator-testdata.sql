USE `ace_world`;

/* Remove potential old stuff */
/*
DELETE FROM ace_object
WHERE aceObjectId = 893;

DELETE FROM ace_object_properties_int
WHERE aceObjectId = 893;

DELETE FROM ace_object_properties_string
WHERE aceObjectId = 893;

DELETE FROM ace_position
WHERE aceObjectId = 893;
*/
DELETE FROM ace_weenie_class
WHERE weenieClassId = 893;

/* Insert new stuff */
INSERT INTO ace_weenie_class
	(weenieClassId,
	weenieClassDescription)
VALUES 
	(893, 'drudgeskulkergen');
	
INSERT INTO ace_object
	(aceObjectId,
	aceObjectDescriptionFlags,
	weenieClassId,
	weenieHeaderFlags,
	physicsDescriptionFlag)
VALUES
	(893, 148, 893, 0, 32769);
	
INSERT INTO ace_object_properties_did
	(aceObjectId,
	didPropertyId,
	propertyValue)
VALUES 
	(893, 1, 33555051),
    (893, 8, 100667494);

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
	(893, 9006, 100); /* GeneratorProbability = 100 */ 

INSERT INTO ace_object_properties_string 
	(aceObjectId,
	strPropertyId,
	propertyValue)
VALUES 
	(893, 1, 'Drudge Skulker Generator');
	
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
	(893, 1, 2847080471, 50.559231, 157.634384, 94.218269, -0.347880, 0.000000, 0.000000, -0.937539);
    /* (893, 3, 2847080463, 47.767307, 154.314651, 94.005005, 0.278842, 0.000000, 0.000000, 0.960337); /* Position close to Holtburg Lifestone */
