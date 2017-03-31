-- --------------------------------------------------------
-- Server version:               10.1.21-MariaDB-1~xenial - mariadb.org binary distribution
-- Server OS:                    debian-linux-gnu
-- HeidiSQL Version:             9.4.0.5125
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for table ace_world.ace_creature_generators
CREATE TABLE IF NOT EXISTS `ace_creature_generators` (
  `generatorid` int(10) unsigned NOT NULL,
  `name` text NOT NULL,
  PRIMARY KEY (`generatorid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Generators (=groups) for all type of creatures: Drudges, Eaters, Banderlings, etc.';

-- Dumping data for table ace_world.ace_creature_generators: ~2 rows (approximately)
/*!40000 ALTER TABLE `ace_creature_generators` DISABLE KEYS */;
INSERT INTO `ace_creature_generators` (`generatorid`, `name`) VALUES
	(3, 'Drduges (Black)'),
	(8, 'Drduges (Normal)');
/*!40000 ALTER TABLE `ace_creature_generators` ENABLE KEYS */;

-- Dumping structure for table ace_world.ace_creature_generator_data
CREATE TABLE IF NOT EXISTS `ace_creature_generator_data` (
  `generatorid` int(10) unsigned NOT NULL,
  `weenieClassId` smallint(5) unsigned NOT NULL,
  `probability` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`generatorid`,`weenieClassId`),
  KEY `FKace_creature_generator_data__weenieClassId` (`weenieClassId`),
  CONSTRAINT `FKace_creature_generator_data__generatorId` FOREIGN KEY (`generatorid`) REFERENCES `ace_creature_generators` (`generatorid`),
  CONSTRAINT `FKace_creature_generator_data__weenieClassId` FOREIGN KEY (`weenieClassId`) REFERENCES `weenie_class` (`weenieClassId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Creature templates that all belong into one generator: i.e. all normal drudges. Probability of spawning this particular creature from this group.';

-- Dumping data for table ace_world.ace_creature_generator_data: ~3 rows (approximately)
/*!40000 ALTER TABLE `ace_creature_generator_data` DISABLE KEYS */;
INSERT INTO `ace_creature_generator_data` (`generatorid`, `weenieClassId`, `probability`) VALUES
	(8, 35440, 50),
	(8, 35441, 40),
	(8, 42437, 10);
/*!40000 ALTER TABLE `ace_creature_generator_data` ENABLE KEYS */;

-- Dumping structure for table ace_world.ace_creature_generator_locations
CREATE TABLE IF NOT EXISTS `ace_creature_generator_locations` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `generatorId` int(10) unsigned NOT NULL,
  `quantity` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `landblock` smallint(5) unsigned DEFAULT NULL,
  `cell` smallint(5) unsigned DEFAULT NULL,
  `posX` float DEFAULT NULL,
  `posY` float DEFAULT NULL,
  `posZ` float DEFAULT NULL,
  `qW` float DEFAULT NULL,
  `qX` float DEFAULT NULL,
  `qY` float DEFAULT NULL,
  `qZ` float DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `FKace_creature_generator_locations__generatorId` (`generatorId`),
  CONSTRAINT `FKace_creature_generator_locations__generatorId` FOREIGN KEY (`generatorId`) REFERENCES `ace_creature_generators` (`generatorid`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1 COMMENT='Locations for the random generated creatures. Specify the quantitiy of how many creatures from this generator should be spawned around this location.';

-- Dumping data for table ace_world.ace_creature_generator_locations: ~1 rows (approximately)
/*!40000 ALTER TABLE `ace_creature_generator_locations` DISABLE KEYS */;
INSERT INTO `ace_creature_generator_locations` (`id`, `generatorId`, `quantity`, `landblock`, `cell`, `posX`, `posY`, `posZ`, `qW`, `qX`, `qY`, `qZ`) VALUES
	(1, 8, 3, 4344, 25, 82.1302, 9.98661, 94.005, 0.983917, 0, 0, 0.178627);
/*!40000 ALTER TABLE `ace_creature_generator_locations` ENABLE KEYS */;

-- Dumping structure for table ace_world.ace_creature_static_locations
CREATE TABLE IF NOT EXISTS `ace_creature_static_locations` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `weenieClassId` smallint(5) unsigned NOT NULL,
  `landblock` smallint(5) unsigned DEFAULT NULL,
  `cell` smallint(5) unsigned DEFAULT NULL,
  `posX` float DEFAULT NULL,
  `posY` float DEFAULT NULL,
  `posZ` float DEFAULT NULL,
  `qW` float DEFAULT NULL,
  `qX` float DEFAULT NULL,
  `qY` float DEFAULT NULL,
  `qZ` float DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `FKace_creature_static_locations__weenieClassId` (`weenieClassId`),
  CONSTRAINT `FKace_creature_static_locations__weenieClassId` FOREIGN KEY (`weenieClassId`) REFERENCES `weenie_class` (`weenieClassId`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1 COMMENT='Location for an exact - not random - creature to spawn: i.e. the 3 (?) water golems on Mayoi beach would be in here.';

-- Dumping data for table ace_world.ace_creature_static_locations: ~1 rows (approximately)
/*!40000 ALTER TABLE `ace_creature_static_locations` DISABLE KEYS */;
INSERT INTO `ace_creature_static_locations` (`id`, `weenieClassId`, `landblock`, `cell`, `posX`, `posY`, `posZ`, `qW`, `qX`, `qY`, `qZ`) VALUES
	(2, 35442, 43443, 15, 47.7673, 154.315, 94.005, 0.278842, 0, 0, 0.960337);
/*!40000 ALTER TABLE `ace_creature_static_locations` ENABLE KEYS */;

-- Dumping structure for view ace_world.vw_ace_creature_static
-- Creating temporary table to overcome VIEW dependency errors
CREATE TABLE `vw_ace_creature_static` (
	`baseAceObjectId` INT(10) UNSIGNED NOT NULL,
	`name` TEXT NOT NULL COLLATE 'utf8_general_ci',
	`typeId` INT(10) UNSIGNED NOT NULL,
	`paletteId` INT(10) UNSIGNED NOT NULL,
	`ammoType` INT(10) UNSIGNED NOT NULL,
	`blipColor` TINYINT(3) UNSIGNED NOT NULL,
	`bitField` INT(10) UNSIGNED NOT NULL,
	`burden` INT(10) UNSIGNED NOT NULL,
	`combatUse` TINYINT(3) UNSIGNED NOT NULL,
	`cooldownDuration` DOUBLE NOT NULL,
	`cooldownId` INT(10) UNSIGNED NOT NULL,
	`effects` INT(10) UNSIGNED NOT NULL,
	`containersCapacity` TINYINT(3) UNSIGNED NOT NULL,
	`header` INT(10) UNSIGNED NOT NULL,
	`hookTypeId` INT(10) UNSIGNED NOT NULL,
	`iconId` INT(10) UNSIGNED NOT NULL,
	`iconOverlayId` INT(10) UNSIGNED NOT NULL,
	`iconUnderlayId` INT(10) UNSIGNED NOT NULL,
	`hookItemTypes` INT(10) UNSIGNED NOT NULL,
	`itemsCapacity` TINYINT(3) UNSIGNED NOT NULL,
	`location` TINYINT(3) UNSIGNED NOT NULL,
	`materialType` TINYINT(3) UNSIGNED NOT NULL,
	`maxStackSize` SMALLINT(5) UNSIGNED NOT NULL,
	`maxStructure` SMALLINT(5) UNSIGNED NOT NULL,
	`radar` TINYINT(3) UNSIGNED NOT NULL,
	`pscript` SMALLINT(5) UNSIGNED NOT NULL,
	`spellId` SMALLINT(5) UNSIGNED NOT NULL,
	`stackSize` SMALLINT(5) UNSIGNED NOT NULL,
	`structure` SMALLINT(5) UNSIGNED NOT NULL,
	`targetTypeId` INT(10) UNSIGNED NOT NULL,
	`usability` INT(10) UNSIGNED NOT NULL,
	`useRadius` FLOAT NOT NULL,
	`validLocations` INT(10) UNSIGNED NOT NULL,
	`value` INT(10) UNSIGNED NOT NULL,
	`workmanship` FLOAT NOT NULL,
	`animationFrameId` INT(10) UNSIGNED NOT NULL,
	`defaultScript` INT(10) UNSIGNED NOT NULL,
	`defaultScriptIntensity` FLOAT NOT NULL,
	`elasticity` FLOAT NOT NULL,
	`friction` FLOAT NOT NULL,
	`locationId` INT(10) UNSIGNED NOT NULL,
	`modelTableId` INT(10) UNSIGNED NOT NULL,
	`motionTableId` INT(10) UNSIGNED NOT NULL,
	`objectScale` FLOAT NOT NULL,
	`physicsBitField` INT(10) UNSIGNED NOT NULL,
	`physicsState` INT(10) UNSIGNED NOT NULL,
	`physicsTableId` INT(10) UNSIGNED NOT NULL,
	`soundTableId` INT(10) UNSIGNED NOT NULL,
	`translucency` FLOAT NOT NULL,
	`weenieClassId` SMALLINT(5) UNSIGNED NOT NULL,
	`landblock` SMALLINT(5) UNSIGNED NULL,
	`cell` SMALLINT(5) UNSIGNED NULL,
	`posX` FLOAT NULL,
	`posY` FLOAT NULL,
	`posZ` FLOAT NULL,
	`qW` FLOAT NULL,
	`qX` FLOAT NULL,
	`qY` FLOAT NULL,
	`qZ` FLOAT NULL
) ENGINE=MyISAM;

-- Dumping structure for view ace_world.vw_ace_creature_static
-- Removing temporary table and create final VIEW structure
DROP TABLE IF EXISTS `vw_ace_creature_static`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`%` SQL SECURITY DEFINER VIEW `vw_ace_creature_static` AS (select `BAO`.`baseAceObjectId` AS `baseAceObjectId`,`BAO`.`name` AS `name`,`BAO`.`typeId` AS `typeId`,`BAO`.`paletteId` AS `paletteId`,`BAO`.`ammoType` AS `ammoType`,`BAO`.`blipColor` AS `blipColor`,`BAO`.`bitField` AS `bitField`,`BAO`.`burden` AS `burden`,`BAO`.`combatUse` AS `combatUse`,`BAO`.`cooldownDuration` AS `cooldownDuration`,`BAO`.`cooldownId` AS `cooldownId`,`BAO`.`effects` AS `effects`,`BAO`.`containersCapacity` AS `containersCapacity`,`BAO`.`header` AS `header`,`BAO`.`hookTypeId` AS `hookTypeId`,`BAO`.`iconId` AS `iconId`,`BAO`.`iconOverlayId` AS `iconOverlayId`,`BAO`.`iconUnderlayId` AS `iconUnderlayId`,`BAO`.`hookItemTypes` AS `hookItemTypes`,`BAO`.`itemsCapacity` AS `itemsCapacity`,`BAO`.`location` AS `location`,`BAO`.`materialType` AS `materialType`,`BAO`.`maxStackSize` AS `maxStackSize`,`BAO`.`maxStructure` AS `maxStructure`,`BAO`.`radar` AS `radar`,`BAO`.`pscript` AS `pscript`,`BAO`.`spellId` AS `spellId`,`BAO`.`stackSize` AS `stackSize`,`BAO`.`structure` AS `structure`,`BAO`.`targetTypeId` AS `targetTypeId`,`BAO`.`usability` AS `usability`,`BAO`.`useRadius` AS `useRadius`,`BAO`.`validLocations` AS `validLocations`,`BAO`.`value` AS `value`,`BAO`.`workmanship` AS `workmanship`,`BAO`.`animationFrameId` AS `animationFrameId`,`BAO`.`defaultScript` AS `defaultScript`,`BAO`.`defaultScriptIntensity` AS `defaultScriptIntensity`,`BAO`.`elasticity` AS `elasticity`,`BAO`.`friction` AS `friction`,`BAO`.`locationId` AS `locationId`,`BAO`.`modelTableId` AS `modelTableId`,`BAO`.`motionTableId` AS `motionTableId`,`BAO`.`objectScale` AS `objectScale`,`BAO`.`physicsBitField` AS `physicsBitField`,`BAO`.`physicsState` AS `physicsState`,`BAO`.`physicsTableId` AS `physicsTableId`,`BAO`.`soundTableId` AS `soundTableId`,`BAO`.`translucency` AS `translucency`,`ACSL`.`weenieClassId` AS `weenieClassId`,`ACSL`.`landblock` AS `landblock`,`ACSL`.`cell` AS `cell`,`ACSL`.`posX` AS `posX`,`ACSL`.`posY` AS `posY`,`ACSL`.`posZ` AS `posZ`,`ACSL`.`qW` AS `qW`,`ACSL`.`qX` AS `qX`,`ACSL`.`qY` AS `qY`,`ACSL`.`qZ` AS `qZ` from ((`ace_creature_static_locations` `ACSL` join `weenie_class` `WC` on((`WC`.`weenieClassId` = `ACSL`.`weenieClassId`))) join `base_ace_object` `BAO` on((`WC`.`baseAceObjectId` = `BAO`.`baseAceObjectId`))));

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
