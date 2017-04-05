-- --------------------------------------------------------
-- HeidiSQL Version:             9.4.0.5125
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping data for table ace_world.ace_creature_generators: ~2 rows (approximately)
/*!40000 ALTER TABLE `ace_creature_generators` DISABLE KEYS */;
INSERT INTO `ace_creature_generators` (`generatorid`, `name`) VALUES
	(3, 'Drudges (Black)'),
	(8, 'Drudges (Normal)');
/*!40000 ALTER TABLE `ace_creature_generators` ENABLE KEYS */;

-- Dumping data for table ace_world.ace_creature_generator_data: ~2 rows (approximately)
/*!40000 ALTER TABLE `ace_creature_generator_data` DISABLE KEYS */;
INSERT INTO `ace_creature_generator_data` (`generatorid`, `weenieClassId`, `probability`) VALUES
	(8, 35440, 50),
	(8, 35441, 40),
	(8, 35442, 10);
/*!40000 ALTER TABLE `ace_creature_generator_data` ENABLE KEYS */;

-- Dumping data for table ace_world.ace_creature_generator_locations: ~0 rows (approximately)
/*!40000 ALTER TABLE `ace_creature_generator_locations` DISABLE KEYS */;
INSERT INTO `ace_creature_generator_locations` (`id`, `generatorId`, `quantity`, `landblock`, `cell`, `posX`, `posY`, `posZ`, `qW`, `qX`, `qY`, `qZ`) VALUES
	(1, 8, 3, 43443, 25, 82.1302, 9.98661, 94.005, 0.983917, 0, 0, 0.178627);
/*!40000 ALTER TABLE `ace_creature_generator_locations` ENABLE KEYS */;

-- Dumping data for table ace_world.ace_creature_static_locations: ~1 rows (approximately)
/*!40000 ALTER TABLE `ace_creature_static_locations` DISABLE KEYS */;
INSERT INTO `ace_creature_static_locations` (`id`, `weenieClassId`, `landblock`, `cell`, `posX`, `posY`, `posZ`, `qW`, `qX`, `qY`, `qZ`) VALUES
	(1, 35442, 43443, 15, 47.7673, 154.315, 94.005, 0.278842, 0, 0, 0.960337);
/*!40000 ALTER TABLE `ace_creature_static_locations` ENABLE KEYS */;

/*Data for the table `base_ace_object` */

insert  into `base_ace_object`(`baseAceObjectId`,`name`,`typeId`,`paletteId`,`ammoType`,`blipColor`,`bitField`,`burden`,`combatUse`,`cooldownDuration`,`cooldownId`,`effects`,`containersCapacity`,`header`,`hookTypeId`,`iconId`,`iconOverlayId`,`iconUnderlayId`,`hookItemTypes`,`itemsCapacity`,`location`,`materialType`,`maxStackSize`,`maxStructure`,`radar`,`pscript`,`spellId`,`stackSize`,`structure`,`targetTypeId`,`usability`,`useRadius`,`validLocations`,`value`,`workmanship`,`animationFrameId`,`defaultScript`,`defaultScriptIntensity`,`elasticity`,`friction`,`locationId`,`modelTableId`,`motionTableId`,`objectScale`,`physicsBitField`,`physicsState`,`physicsTableId`,`soundTableId`,`translucency`) values 
(35437,'Drudge Robber',16,67112812,0,0,20,0,0,0,0,0,255,8388630,0,100667445,0,0,0,255,0,0,0,0,2,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,33556445,150994952,0,104451,1032,872415258,536870919,0),
(35440,'Drudge Skulker',16,67112812,0,0,20,0,0,0,0,0,255,8388630,0,100667445,0,0,0,255,0,0,0,0,2,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,33556445,150994952,0.95,104579,1032,872415258,536870919,0),
(35441,'Drudge Slinker',16,67112812,0,0,20,0,0,0,0,0,255,8388630,0,100667445,0,0,0,255,0,0,0,0,2,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,33556445,150994952,0.95,104579,1032,872415258,536870919,0),
(35442,'Drudge Sneaker',16,67112812,0,0,20,0,0,0,0,0,255,8388630,0,100667445,0,0,0,255,0,0,0,0,2,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,33556445,150994952,1.2,104579,1032,872415258,536870919,0);

/*Data for the table `weenie_animation_changes` */

insert  into `weenie_animation_changes`(`weenieClassId`,`index`,`animationId`) values 
(35441,3,16784258),
(35441,6,16784261),
(35442,9,16784289),
(35442,12,16784289);

/*Data for the table `weenie_class` */

insert  into `weenie_class`(`weenieClassId`,`baseAceObjectId`) values 
(35437,35437),
(35440,35440),
(35441,35441),
(35442,35442);

/*Data for the table `weenie_palette_changes` */

insert  into `weenie_palette_changes`(`weenieClassId`,`subPaletteId`,`offset`,`length`) values 
(35437,67112815,0,2048),
(35440,67112817,0,2048),
(35441,67112815,0,2048),
(35442,67112812,0,2048);

/*Data for the table `weenie_texture_map_changes` */

insert  into `weenie_texture_map_changes`(`weenieClassId`,`index`,`oldId`,`newId`) values 
(35441,3,83892453,83892454),
(35441,6,83892453,83892454),
(35442,9,83892467,83892468),
(35442,12,83892467,83892468);

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
