USE `ace_world`;

DELETE FROM ace_weenie_class
WHERE weenieClassId = 15759;

/* Insert new stuff */
INSERT INTO ace_weenie_class
	(weenieClassId,
	weenieClassDescription)
VALUES 
	(15759, 'linkitemgen10seconds');
	
DELETE FROM ace_object
WHERE aceObjectId = 70002;

INSERT INTO ace_object
	(aceObjectId,
	aceObjectDescriptionFlags,
	weenieClassId,
	weenieHeaderFlags,
	physicsDescriptionFlag)
VALUES
	(70002, 148, 15759, 0, 32769);
	
INSERT INTO ace_object_properties_did
	(aceObjectId,
	didPropertyId,
	propertyValue)
VALUES 
	(70002, 1, 33555051), /* CSetup */
    (70002, 8, 100667494); /* Icon */

INSERT INTO ace_object_properties_int
	(aceObjectId,
	intPropertyId,
	propertyValue)
VALUES 
	(70002, 1, 0),      /* ItemType = 0 */
	(70002, 81, 1),     /* MaxGeneratedObjects = 1 */
    (70002, 93, 1040),     /* PhysicsState = 1040 */
	(70002, 100, 2),    /* GeneratorType = Absolute */
	(70002, 104, 13239),		/* ActivationCreateClass */
	(70002, 142, 0),    /* GeneratorTimeType = Undef */
	(70002, 9006, 100); /* GeneratorProbability = 100 */ 

INSERT INTO ace_object_properties_bool 
	(aceObjectId,
	boolPropertyId,
	propertyValue)
VALUES 
	(70002, 11, True),
    (70002, 14, True);

INSERT INTO ace_object_properties_string 
	(aceObjectId,
	strPropertyId,
	propertyValue)
VALUES 
	(70002, 1, 'Academy Cap Generator');
	
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
	(70002, 1, 2130903478, 22.2098, -40.2234, 0.67375, 0.102269, 0.000000, 0.000000, -0.994757);

DELETE FROM ace_object
WHERE aceObjectId = 70003;

INSERT INTO ace_object
	(aceObjectId,
	aceObjectDescriptionFlags,
	weenieClassId,
	weenieHeaderFlags,
	physicsDescriptionFlag)
VALUES
	(70003, 148, 15759, 0, 32769);
	
INSERT INTO ace_object_properties_did
	(aceObjectId,
	didPropertyId,
	propertyValue)
VALUES 
	(70003, 1, 33555051),
    (70003, 8, 100667494);

INSERT INTO ace_object_properties_int
	(aceObjectId,
	intPropertyId,
	propertyValue)
VALUES 
	(70003, 1, 0),      /* ItemType = 0 */
	(70003, 81, 1),     /* MaxGeneratedObjects = 1 */
    (70003, 93, 1040),     
	(70003, 100, 2),    /* GeneratorType = Absolute */
	(70003, 104, 13240),		
	(70003, 142, 0),    /* GeneratorTimeType = Undef */
	(70003, 9006, 100); /* GeneratorProbability = 100 */ 

INSERT INTO ace_object_properties_bool 
	(aceObjectId,
	boolPropertyId,
	propertyValue)
VALUES 
	(70003, 11, True),
    (70003, 14, True);

INSERT INTO ace_object_properties_string 
	(aceObjectId,
	strPropertyId,
	propertyValue)
VALUES 
	(70003, 1, 'Academy Gauntlets Generator');
	
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
	(70003, 1, 2130903472, 18.3629, -21.0976, 0, -0.922891, 0.000000, 0.000000, 0.385062);

DELETE FROM ace_object
WHERE aceObjectId = 70004;

INSERT INTO ace_object
	(aceObjectId,
	aceObjectDescriptionFlags,
	weenieClassId,
	weenieHeaderFlags,
	physicsDescriptionFlag)
VALUES
	(70004, 148, 15759, 0, 32769);
	
INSERT INTO ace_object_properties_did
	(aceObjectId,
	didPropertyId,
	propertyValue)
VALUES 
	(70004, 1, 33555051),
    (70004, 8, 100667494);

INSERT INTO ace_object_properties_int
	(aceObjectId,
	intPropertyId,
	propertyValue)
VALUES 
	(70004, 1, 0),      /* ItemType = 0 */
	(70004, 81, 1),     /* MaxGeneratedObjects = 1 */
    (70004, 93, 1040),     
	(70004, 100, 2),    /* GeneratorType = Absolute */
	(70004, 104, 13241),		
	(70004, 142, 0),    /* GeneratorTimeType = Undef */
	(70004, 9006, 100); /* GeneratorProbability = 100 */ 

INSERT INTO ace_object_properties_bool 
	(aceObjectId,
	boolPropertyId,
	propertyValue)
VALUES 
	(70004, 11, True),
    (70004, 14, True);

INSERT INTO ace_object_properties_string 
	(aceObjectId,
	strPropertyId,
	propertyValue)
VALUES 
	(70004, 1, 'Academy Leggings Generator');
	
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
	(70004, 1, 2130903478, 17.7939, -41.728, -0.002500013, -0.481744, 0.000000, 0.000000, -0.876312);

DELETE FROM ace_weenie_class
WHERE weenieClassId = 28282;

INSERT INTO ace_weenie_class
	(weenieClassId,
	weenieClassDescription)
VALUES 
	(28282, 'linkmonstergen10seconds');

DELETE FROM ace_object
WHERE aceObjectId = 70005;

INSERT INTO ace_object
	(aceObjectId,
	aceObjectDescriptionFlags,
	weenieClassId,
	weenieHeaderFlags,
	physicsDescriptionFlag)
VALUES
	(70005, 148, 28282, 0, 32769);
	
INSERT INTO ace_object_properties_did
	(aceObjectId,
	didPropertyId,
	propertyValue)
VALUES 
	(70005, 1, 33555051),
    (70005, 8, 100667494);

INSERT INTO ace_object_properties_int
	(aceObjectId,
	intPropertyId,
	propertyValue)
VALUES 
	(70005, 1, 0),      /* ItemType = 0 */
	(70005, 81, 1),     /* MaxGeneratedObjects = 1 */
    (70005, 93, 1040),
	(70005, 100, 2),    /* GeneratorType = Absolute */
	(70005, 104, 12698),		
	(70005, 142, 0),    /* GeneratorTimeType = Undef */
	(70005, 9006, 100); /* GeneratorProbability = 100 */ 

INSERT INTO ace_object_properties_bool 
	(aceObjectId,
	boolPropertyId,
	propertyValue)
VALUES 
	(70005, 11, True),
    (70005, 14, True);

INSERT INTO ace_object_properties_string 
	(aceObjectId,
	strPropertyId,
	propertyValue)
VALUES 
	(70005, 1, 'Sparring Golem Generator');
	
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
	(70005, 1, 2130903610, 60.9185, -20.011, 0.009000003, -0.7153111, 0.000000, 0.000000, -0.6988061);

DELETE FROM ace_object
WHERE aceObjectId = 70006;

INSERT INTO ace_object
	(aceObjectId,
	aceObjectDescriptionFlags,
	weenieClassId,
	weenieHeaderFlags,
	physicsDescriptionFlag)
VALUES
	(70006, 148, 15759, 0, 32769);
	
INSERT INTO ace_object_properties_did
	(aceObjectId,
	didPropertyId,
	propertyValue)
VALUES 
	(70006, 1, 33555051),
    (70006, 8, 100667494);

INSERT INTO ace_object_properties_int
	(aceObjectId,
	intPropertyId,
	propertyValue)
VALUES 
	(70006, 1, 0),      /* ItemType = 0 */
	(70006, 81, 1),     /* MaxGeneratedObjects = 1 */
    (70006, 93, 1040),     
	(70006, 100, 2),    /* GeneratorType = Absolute */
	(70006, 104, 5090),		
	(70006, 142, 0),    /* GeneratorTimeType = Undef */
	(70006, 9006, 100); /* GeneratorProbability = 100 */ 

INSERT INTO ace_object_properties_bool 
	(aceObjectId,
	boolPropertyId,
	propertyValue)
VALUES 
	(70006, 11, True),
    (70006, 14, True);

INSERT INTO ace_object_properties_string 
	(aceObjectId,
	strPropertyId,
	propertyValue)
VALUES 
	(70006, 1, 'Bruised Apple Generator');
	
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
	(70006, 1, 2130903469, 7.739, -30.103, 1.5, 0.702712, 0.000000, 0.000000, -0.711474);

DELETE FROM ace_object
WHERE aceObjectId = 70007;

INSERT INTO ace_object
	(aceObjectId,
	aceObjectDescriptionFlags,
	weenieClassId,
	weenieHeaderFlags,
	physicsDescriptionFlag)
VALUES
	(70007, 148, 28282, 0, 32769);
	
INSERT INTO ace_object_properties_did
	(aceObjectId,
	didPropertyId,
	propertyValue)
VALUES 
	(70007, 1, 33555051),
    (70007, 8, 100667494);

INSERT INTO ace_object_properties_int
	(aceObjectId,
	intPropertyId,
	propertyValue)
VALUES 
	(70007, 1, 0),      /* ItemType = 0 */
	(70007, 81, 1),     /* MaxGeneratedObjects = 1 */
    (70007, 93, 1040),     
	(70007, 100, 2),    /* GeneratorType = Absolute */
	(70007, 104, 12698),		
	(70007, 142, 0),    /* GeneratorTimeType = Undef */
	(70007, 9006, 100); /* GeneratorProbability = 100 */ 

INSERT INTO ace_object_properties_bool 
	(aceObjectId,
	boolPropertyId,
	propertyValue)
VALUES 
	(70007, 11, True),
    (70007, 14, True);

INSERT INTO ace_object_properties_string 
	(aceObjectId,
	strPropertyId,
	propertyValue)
VALUES 
	(70007, 1, 'Sparring Golem Generator');
	
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
	(70007, 1, 2130903705, 86.856, -20.211, 0.009000003, -0.707107, 0.000000, 0.000000, -0.707107);

DELETE FROM ace_object
WHERE aceObjectId = 70008;

INSERT INTO ace_object
	(aceObjectId,
	aceObjectDescriptionFlags,
	weenieClassId,
	weenieHeaderFlags,
	physicsDescriptionFlag)
VALUES
	(70008, 148, 28282, 0, 32769);
	
INSERT INTO ace_object_properties_did
	(aceObjectId,
	didPropertyId,
	propertyValue)
VALUES 
	(70008, 1, 33555051),
    (70008, 8, 100667494);

INSERT INTO ace_object_properties_int
	(aceObjectId,
	intPropertyId,
	propertyValue)
VALUES 
	(70008, 1, 0),      /* ItemType = 0 */
	(70008, 81, 1),     /* MaxGeneratedObjects = 1 */
    (70008, 93, 1040),     
	(70008, 100, 2),    /* GeneratorType = Absolute */
	(70008, 104, 12698),		
	(70008, 142, 0),    /* GeneratorTimeType = Undef */
	(70008, 9006, 100); /* GeneratorProbability = 100 */ 

INSERT INTO ace_object_properties_bool 
	(aceObjectId,
	boolPropertyId,
	propertyValue)
VALUES 
	(70008, 11, True),
    (70008, 14, True);

INSERT INTO ace_object_properties_string 
	(aceObjectId,
	strPropertyId,
	propertyValue)
VALUES 
	(70008, 1, 'Sparring Golem Generator');
	
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
	(70008, 1, 2130903682, 83.7375, -6.96577, 0.009000003, 0.487769, 0.000000, 0.000000, 0.872973);

DELETE FROM ace_object
WHERE aceObjectId = 70009;

INSERT INTO ace_object
	(aceObjectId,
	aceObjectDescriptionFlags,
	weenieClassId,
	weenieHeaderFlags,
	physicsDescriptionFlag)
VALUES
	(70009, 148, 28282, 0, 32769);
	
INSERT INTO ace_object_properties_did
	(aceObjectId,
	didPropertyId,
	propertyValue)
VALUES 
	(70009, 1, 33555051),
    (70009, 8, 100667494);

INSERT INTO ace_object_properties_int
	(aceObjectId,
	intPropertyId,
	propertyValue)
VALUES 
	(70009, 1, 0),      /* ItemType = 0 */
	(70009, 81, 1),     /* MaxGeneratedObjects = 1 */
    (70009, 93, 1040),     
	(70009, 100, 2),    /* GeneratorType = Absolute */
	(70009, 104, 12698),	
	(70009, 142, 0),    /* GeneratorTimeType = Undef */
	(70009, 9006, 100); /* GeneratorProbability = 100 */ 

INSERT INTO ace_object_properties_bool 
	(aceObjectId,
	boolPropertyId,
	propertyValue)
VALUES 
	(70009, 11, True),
    (70009, 14, True);

INSERT INTO ace_object_properties_string 
	(aceObjectId,
	strPropertyId,
	propertyValue)
VALUES 
	(70009, 1, 'Sparring Golem Generator');
	
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
	(70009, 1, 2130903660, 69.9163, -11.2541, 0.009000003, -0.655506, 0.000000, 0.000000, -0.75519);

DELETE FROM ace_object
WHERE aceObjectId = 70010;

INSERT INTO ace_object
	(aceObjectId,
	aceObjectDescriptionFlags,
	weenieClassId,
	weenieHeaderFlags,
	physicsDescriptionFlag)
VALUES
	(70010, 148, 28282, 0, 32769);
	
INSERT INTO ace_object_properties_did
	(aceObjectId,
	didPropertyId,
	propertyValue)
VALUES 
	(70010, 1, 33555051),
    (70010, 8, 100667494);

INSERT INTO ace_object_properties_int
	(aceObjectId,
	intPropertyId,
	propertyValue)
VALUES 
	(70010, 1, 0),      /* ItemType = 0 */
	(70010, 81, 1),     /* MaxGeneratedObjects = 1 */
    (70010, 93, 1040),     
	(70010, 100, 2),    /* GeneratorType = Absolute */
	(70010, 104, 12698),		
	(70010, 142, 0),    /* GeneratorTimeType = Undef */
	(70010, 9006, 100); /* GeneratorProbability = 100 */ 

INSERT INTO ace_object_properties_bool 
	(aceObjectId,
	boolPropertyId,
	propertyValue)
VALUES 
	(70010, 11, True),
    (70010, 14, True);

INSERT INTO ace_object_properties_string 
	(aceObjectId,
	strPropertyId,
	propertyValue)
VALUES 
	(70010, 1, 'Sparring Golem Generator');
	
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
	(70010, 1, 2130903662, 69.3977, -28.0201, 0.009000003, -0.691234, 0.000000, 0.000000, -0.722631);

DELETE FROM ace_object
WHERE aceObjectId = 70011;

INSERT INTO ace_object
	(aceObjectId,
	aceObjectDescriptionFlags,
	weenieClassId,
	weenieHeaderFlags,
	physicsDescriptionFlag)
VALUES
	(70011, 148, 28282, 0, 32769);
	
INSERT INTO ace_object_properties_did
	(aceObjectId,
	didPropertyId,
	propertyValue)
VALUES 
	(70011, 1, 33555051),
    (70011, 8, 100667494);

INSERT INTO ace_object_properties_int
	(aceObjectId,
	intPropertyId,
	propertyValue)
VALUES 
	(70011, 1, 0),      /* ItemType = 0 */
	(70011, 81, 1),     /* MaxGeneratedObjects = 1 */
    (70011, 93, 1040),     
	(70011, 100, 2),    /* GeneratorType = Absolute */
	(70011, 104, 12698),		
	(70011, 142, 0),    /* GeneratorTimeType = Undef */
	(70011, 9006, 100); /* GeneratorProbability = 100 */ 

INSERT INTO ace_object_properties_bool 
	(aceObjectId,
	boolPropertyId,
	propertyValue)
VALUES 
	(70011, 11, True),
    (70011, 14, True);

INSERT INTO ace_object_properties_string 
	(aceObjectId,
	strPropertyId,
	propertyValue)
VALUES 
	(70011, 1, 'Sparring Golem Generator');
	
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
	(70011, 1, 2130903684, 79.3977, -28.0201, 0.009000003, -0.691234, 0.000000, 0.000000, -0.722631); 

DELETE FROM ace_object
WHERE aceObjectId = 70012;

INSERT INTO ace_object
	(aceObjectId,
	aceObjectDescriptionFlags,
	weenieClassId,
	weenieHeaderFlags,
	physicsDescriptionFlag)
VALUES
	(70012, 148, 28282, 0, 32769);
	
INSERT INTO ace_object_properties_did
	(aceObjectId,
	didPropertyId,
	propertyValue)
VALUES 
	(70012, 1, 33555051),
    (70012, 8, 100667494);

INSERT INTO ace_object_properties_int
	(aceObjectId,
	intPropertyId,
	propertyValue)
VALUES 
	(70012, 1, 0),      /* ItemType = 0 */
	(70012, 81, 1),     /* MaxGeneratedObjects = 1 */
    (70012, 93, 1040),     
	(70012, 100, 2),    /* GeneratorType = Absolute */
	(70012, 104, 12698),		
	(70012, 142, 0),    /* GeneratorTimeType = Undef */
	(70012, 9006, 100); /* GeneratorProbability = 100 */ 

INSERT INTO ace_object_properties_bool 
	(aceObjectId,
	boolPropertyId,
	propertyValue)
VALUES 
	(70012, 11, True),
    (70012, 14, True);

INSERT INTO ace_object_properties_string 
	(aceObjectId,
	strPropertyId,
	propertyValue)
VALUES 
	(70012, 1, 'Sparring Golem Generator');
	
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
	(70012, 1, 2130903684, 83.6883, -31.0056, 0.009000003, 0.853479, 0.000000, 0.000000, 0.521127); 

DELETE FROM ace_object
WHERE aceObjectId = 70013;

INSERT INTO ace_object
	(aceObjectId,
	aceObjectDescriptionFlags,
	weenieClassId,
	weenieHeaderFlags,
	physicsDescriptionFlag)
VALUES
	(70013, 148, 28282, 0, 32769);
	
INSERT INTO ace_object_properties_did
	(aceObjectId,
	didPropertyId,
	propertyValue)
VALUES 
	(70013, 1, 33555051),
    (70013, 8, 100667494);

INSERT INTO ace_object_properties_int
	(aceObjectId,
	intPropertyId,
	propertyValue)
VALUES 
	(70013, 1, 0),      /* ItemType = 0 */
	(70013, 81, 1),     /* MaxGeneratedObjects = 1 */
    (70013, 93, 1040),     
	(70013, 100, 2),    /* GeneratorType = Absolute */
	(70013, 104, 12698),		
	(70013, 142, 0),    /* GeneratorTimeType = Undef */
	(70013, 9006, 100); /* GeneratorProbability = 100 */ 

INSERT INTO ace_object_properties_bool 
	(aceObjectId,
	boolPropertyId,
	propertyValue)
VALUES 
	(70013, 11, True),
    (70013, 14, True);

INSERT INTO ace_object_properties_string 
	(aceObjectId,
	strPropertyId,
	propertyValue)
VALUES 
	(70013, 1, 'Sparring Golem Generator');
	
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
	(70013, 1, 2130903705, 87.9597, -15.8886, 0.009000003, 0.5688431, 0.000000, 0.000000, 0.8224461);

DELETE FROM ace_object
WHERE aceObjectId = 70014;

INSERT INTO ace_object
	(aceObjectId,
	aceObjectDescriptionFlags,
	weenieClassId,
	weenieHeaderFlags,
	physicsDescriptionFlag)
VALUES
	(70014, 148, 28282, 0, 32769);
	
INSERT INTO ace_object_properties_did
	(aceObjectId,
	didPropertyId,
	propertyValue)
VALUES 
	(70014, 1, 33555051),
    (70014, 8, 100667494);

INSERT INTO ace_object_properties_int
	(aceObjectId,
	intPropertyId,
	propertyValue)
VALUES 
	(70014, 1, 0),      /* ItemType = 0 */
	(70014, 81, 1),     /* MaxGeneratedObjects = 1 */
    (70014, 93, 1040),    
	(70014, 100, 2),    /* GeneratorType = Absolute */
	(70014, 104, 12698),		
	(70014, 142, 0),    /* GeneratorTimeType = Undef */
	(70014, 9006, 100); /* GeneratorProbability = 100 */ 

INSERT INTO ace_object_properties_bool 
	(aceObjectId,
	boolPropertyId,
	propertyValue)
VALUES 
	(70014, 11, True),
    (70014, 14, True);

INSERT INTO ace_object_properties_string 
	(aceObjectId,
	strPropertyId,
	propertyValue)
VALUES 
	(70014, 1, 'Sparring Golem Generator');
	
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
	(70014, 1, 2130903683, 77.3078, -20.2622, 0.009000003, -0.695954, 0.000000, 0.000000, -0.718086); 

DELETE FROM ace_weenie_class
WHERE weenieClassId = 1542;

INSERT INTO ace_weenie_class
	(weenieClassId,
	weenieClassDescription)
VALUES 
	(1542, 'linkitemgen');

DELETE FROM ace_object
WHERE aceObjectId = 2012229714;

DELETE FROM ace_object
WHERE aceObjectId = 2012229730;

DELETE FROM ace_object
WHERE aceObjectId = 2012229709;

DELETE FROM ace_object
WHERE aceObjectId = 70015;

INSERT INTO ace_object
	(aceObjectId,
	aceObjectDescriptionFlags,
	weenieClassId,
	weenieHeaderFlags,
	physicsDescriptionFlag)
VALUES
	(70015, 148, 1542, 0, 32769);
	
INSERT INTO ace_object_properties_did
	(aceObjectId,
	didPropertyId,
	propertyValue)
VALUES 
	(70015, 1, 33555051),
    (70015, 8, 100667494);

INSERT INTO ace_object_properties_int
	(aceObjectId,
	intPropertyId,
	propertyValue)
VALUES 
	(70015, 1, 0),      /* ItemType = 0 */
	(70015, 81, 1),     /* MaxGeneratedObjects = 1 */
    (70015, 93, 1040),     
	(70015, 100, 2),    /* GeneratorType = Absolute */
	(70015, 104, 30989),		
	(70015, 142, 0),    /* GeneratorTimeType = Undef */
	(70015, 9006, 100); /* GeneratorProbability = 100 */ 

INSERT INTO ace_object_properties_bool 
	(aceObjectId,
	boolPropertyId,
	propertyValue)
VALUES 
	(70015, 11, True),
    (70015, 14, True);

INSERT INTO ace_object_properties_string 
	(aceObjectId,
	strPropertyId,
	propertyValue)
VALUES 
	(70015, 1, 'Treasure Chest Generator');
	
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
	(70015, 1, 2130903620, 57.6229, -42.4067, 0, -0.3798201, 0.000000, 0.000000, -0.9250603); 

DELETE FROM ace_object
WHERE aceObjectId = 70016;

INSERT INTO ace_object
	(aceObjectId,
	aceObjectDescriptionFlags,
	weenieClassId,
	weenieHeaderFlags,
	physicsDescriptionFlag)
VALUES
	(70016, 148, 1542, 0, 32769);
	
INSERT INTO ace_object_properties_did
	(aceObjectId,
	didPropertyId,
	propertyValue)
VALUES 
	(70016, 1, 33555051),
    (70016, 8, 100667494);

INSERT INTO ace_object_properties_int
	(aceObjectId,
	intPropertyId,
	propertyValue)
VALUES 
	(70016, 1, 0),      /* ItemType = 0 */
	(70016, 81, 1),     /* MaxGeneratedObjects = 1 */
    (70016, 93, 1040),     
	(70016, 100, 2),    /* GeneratorType = Absolute */
	(70016, 104, 30989),		
	(70016, 142, 0),    /* GeneratorTimeType = Undef */
	(70016, 9006, 100); /* GeneratorProbability = 100 */ 

INSERT INTO ace_object_properties_bool 
	(aceObjectId,
	boolPropertyId,
	propertyValue)
VALUES 
	(70016, 11, True),
    (70016, 14, True);

INSERT INTO ace_object_properties_string 
	(aceObjectId,
	strPropertyId,
	propertyValue)
VALUES 
	(70016, 1, 'Treasure Chest Generator');
	
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
	(70016, 1, 2130903601, 57.7551, 2.18029, 0, 0.9210611, 0.000000, 0.000000, 0.3894181);

DELETE FROM ace_object
WHERE aceObjectId = 70017;

INSERT INTO ace_object
	(aceObjectId,
	aceObjectDescriptionFlags,
	weenieClassId,
	weenieHeaderFlags,
	physicsDescriptionFlag)
VALUES
	(70017, 148, 1542, 0, 32769);
	
INSERT INTO ace_object_properties_did
	(aceObjectId,
	didPropertyId,
	propertyValue)
VALUES 
	(70017, 1, 33555051),
    (70017, 8, 100667494);

INSERT INTO ace_object_properties_int
	(aceObjectId,
	intPropertyId,
	propertyValue)
VALUES 
	(70017, 1, 0),      /* ItemType = 0 */
	(70017, 81, 1),     /* MaxGeneratedObjects = 1 */
    (70017, 93, 1040),     
	(70017, 100, 2),    /* GeneratorType = Absolute */
	(70017, 104, 30989),		
	(70017, 142, 0),    /* GeneratorTimeType = Undef */
	(70017, 9006, 100); /* GeneratorProbability = 100 */ 

INSERT INTO ace_object_properties_bool 
	(aceObjectId,
	boolPropertyId,
	propertyValue)
VALUES 
	(70017, 11, True),
    (70017, 14, True);

INSERT INTO ace_object_properties_string 
	(aceObjectId,
	strPropertyId,
	propertyValue)
VALUES 
	(70017, 1, 'Treasure Chest Generator');
	
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
	(70017, 1, 2130903715, 92.4067, -42.3771, 0, 0.385543, 0.000000, 0.000000, -0.9226899);
