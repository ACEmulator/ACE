-- MySQL dump 10.13  Distrib 5.7.12, for Win64 (x86_64)
--
-- Host: localhost    Database: ace_world
-- ------------------------------------------------------
-- Server version	5.7.17-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

CREATE DATABASE /*!32312 IF NOT EXISTS*/`ace_world` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `ace_auth`;

--
-- Table structure for table `ace_creature_generator_data`
--

DROP TABLE IF EXISTS `ace_creature_generator_data`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_creature_generator_data` (
  `generatorid` int(10) unsigned NOT NULL,
  `weenieClassId` smallint(5) unsigned NOT NULL,
  `probability` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`generatorid`,`weenieClassId`),
  KEY `FKace_creature_generator_data__weenieClassId` (`weenieClassId`),
  CONSTRAINT `FKace_creature_generator_data__generatorId` FOREIGN KEY (`generatorid`) REFERENCES `ace_creature_generators` (`generatorid`),
  CONSTRAINT `FKace_creature_generator_data__weenieClassId` FOREIGN KEY (`weenieClassId`) REFERENCES `weenie_class` (`weenieClassId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Creature templates that all belong into one generator: i.e. all normal drudges. Probability of spawning this particular creature from this group.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ace_creature_generator_locations`
--

DROP TABLE IF EXISTS `ace_creature_generator_locations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_creature_generator_locations` (
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
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Locations for the random generated creatures. Specify the quantitiy of how many creatures from this generator should be spawned around this location.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ace_creature_generators`
--

DROP TABLE IF EXISTS `ace_creature_generators`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_creature_generators` (
  `generatorid` int(10) unsigned NOT NULL,
  `name` text NOT NULL,
  PRIMARY KEY (`generatorid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Generators (=groups) for all type of creatures: Drudges, Eaters, Banderlings, etc.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ace_creature_static_locations`
--

DROP TABLE IF EXISTS `ace_creature_static_locations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_creature_static_locations` (
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
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Location for an exact - not random - creature to spawn: i.e. the 3 (?) water golems on Mayoi beach would be in here.';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ace_object`
--

DROP TABLE IF EXISTS `ace_object`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_object` (
  `baseAceObjectId` int(10) unsigned NOT NULL,
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
  PRIMARY KEY (`baseAceObjectId`),
  KEY `FK_ace_object__weenieClassId` (`weenieClassId`),
  CONSTRAINT `FK_ace_object__baseAceObjectId` FOREIGN KEY (`baseAceObjectId`) REFERENCES `base_ace_object` (`baseAceObjectId`),
  CONSTRAINT `FK_ace_object__weenieClassId` FOREIGN KEY (`weenieClassId`) REFERENCES `weenie_class` (`weenieClassId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ace_object_animation_changes`
--

DROP TABLE IF EXISTS `ace_object_animation_changes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_object_animation_changes` (
  `baseAceObjectId` int(10) unsigned NOT NULL,
  `index` tinyint(3) unsigned NOT NULL,
  `animationId` int(10) unsigned NOT NULL,
  PRIMARY KEY (`baseAceObjectId`,`index`),
  CONSTRAINT `FK_ace_object_animation_changes__baseAceObjectId` FOREIGN KEY (`baseAceObjectId`) REFERENCES `ace_object` (`baseAceObjectId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ace_object_palette_changes`
--

DROP TABLE IF EXISTS `ace_object_palette_changes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_object_palette_changes` (
  `baseAceObjectId` int(10) unsigned NOT NULL,
  `subPaletteId` int(10) unsigned NOT NULL,
  `offset` smallint(5) unsigned NOT NULL,
  `length` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`baseAceObjectId`,`subPaletteId`,`offset`,`length`),
  CONSTRAINT `FK_ace_object_palette_data__baseAceObjectId` FOREIGN KEY (`baseAceObjectId`) REFERENCES `ace_object` (`baseAceObjectId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ace_object_texture_map_changes`
--

DROP TABLE IF EXISTS `ace_object_texture_map_changes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_object_texture_map_changes` (
  `baseAceObjectId` int(10) unsigned NOT NULL,
  `index` tinyint(3) unsigned NOT NULL,
  `oldId` int(10) unsigned NOT NULL,
  `newId` int(10) unsigned NOT NULL,
  PRIMARY KEY (`baseAceObjectId`,`index`,`oldId`),
  CONSTRAINT `FK_ace_object_texture_map_changes__baseAceObjectId` FOREIGN KEY (`baseAceObjectId`) REFERENCES `ace_object` (`baseAceObjectId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `base_ace_object`
--

DROP TABLE IF EXISTS `base_ace_object`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `base_ace_object` (
  `baseAceObjectId` int(10) unsigned NOT NULL,
  `name` text NOT NULL,
  `typeId` int(10) unsigned NOT NULL,
  `paletteId` int(10) unsigned NOT NULL DEFAULT '0',
  `ammoType` int(10) unsigned NOT NULL DEFAULT '0',
  `blipColor` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `bitField` int(10) unsigned NOT NULL DEFAULT '0',
  `burden` int(10) unsigned NOT NULL DEFAULT '0',
  `combatUse` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `cooldownDuration` double NOT NULL DEFAULT '0',
  `cooldownId` int(10) unsigned NOT NULL DEFAULT '0',
  `effects` int(10) unsigned NOT NULL DEFAULT '0',
  `containersCapacity` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `header` int(10) unsigned NOT NULL DEFAULT '0',
  `hookTypeId` int(10) unsigned NOT NULL DEFAULT '0',
  `iconId` int(10) unsigned NOT NULL DEFAULT '0',
  `iconOverlayId` int(10) unsigned NOT NULL DEFAULT '0',
  `iconUnderlayId` int(10) unsigned NOT NULL DEFAULT '0',
  `hookItemTypes` int(10) unsigned NOT NULL DEFAULT '0',
  `itemsCapacity` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `location` int(10) unsigned NOT NULL DEFAULT '0',
  `materialType` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `maxStackSize` smallint(5) unsigned NOT NULL DEFAULT '0',
  `maxStructure` smallint(5) unsigned NOT NULL DEFAULT '0',
  `radar` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `pscript` smallint(5) unsigned NOT NULL DEFAULT '0',
  `spellId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `stackSize` smallint(5) unsigned NOT NULL DEFAULT '0',
  `structure` smallint(5) unsigned NOT NULL DEFAULT '0',
  `targetTypeId` int(10) unsigned NOT NULL DEFAULT '0',
  `usability` int(10) unsigned NOT NULL DEFAULT '0',
  `useRadius` float NOT NULL DEFAULT '0',
  `validLocations` int(10) unsigned NOT NULL DEFAULT '0',
  `value` int(10) unsigned NOT NULL DEFAULT '0',
  `workmanship` float NOT NULL DEFAULT '0',
  `animationFrameId` int(10) unsigned NOT NULL DEFAULT '0',
  `defaultScript` int(10) unsigned NOT NULL DEFAULT '0',
  `defaultScriptIntensity` float NOT NULL DEFAULT '0',
  `elasticity` float NOT NULL DEFAULT '0',
  `friction` float NOT NULL DEFAULT '0',
  `locationId` int(10) unsigned NOT NULL DEFAULT '0',
  `modelTableId` int(10) unsigned NOT NULL DEFAULT '0',
  `motionTableId` int(10) unsigned NOT NULL DEFAULT '0',
  `objectScale` float NOT NULL DEFAULT '0',
  `physicsBitField` int(10) unsigned NOT NULL DEFAULT '0',
  `physicsState` int(10) unsigned NOT NULL DEFAULT '0',
  `physicsTableId` int(10) unsigned NOT NULL DEFAULT '0',
  `soundTableId` int(10) unsigned NOT NULL DEFAULT '0',
  `translucency` float NOT NULL DEFAULT '0',
  `currentMotionState` text NOT NULL,
  PRIMARY KEY (`baseAceObjectId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `portal_destination`
--

DROP TABLE IF EXISTS `portal_destination`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `portal_destination` (
  `weenieClassId` smallint(5) unsigned NOT NULL,
  `cell` int(10) unsigned NOT NULL DEFAULT '0',
  `x` float NOT NULL DEFAULT '0',
  `y` float NOT NULL DEFAULT '0',
  `z` float NOT NULL DEFAULT '0',
  `qx` float NOT NULL DEFAULT '0',
  `qy` float NOT NULL DEFAULT '0',
  `qz` float NOT NULL DEFAULT '0',
  `qw` float NOT NULL DEFAULT '0',
  `min_lvl` int(11) unsigned NOT NULL DEFAULT '0',
  `max_lvl` int(11) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`weenieClassId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `teleport_location`
--

DROP TABLE IF EXISTS `teleport_location`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `teleport_location` (
  `location` varchar(100) NOT NULL DEFAULT '',
  `cell` int(10) unsigned NOT NULL DEFAULT '0',
  `x` float NOT NULL DEFAULT '0',
  `y` float NOT NULL DEFAULT '0',
  `z` float NOT NULL DEFAULT '0',
  `qx` float NOT NULL DEFAULT '0',
  `qy` float NOT NULL DEFAULT '0',
  `qz` float NOT NULL DEFAULT '0',
  `qw` float NOT NULL DEFAULT '0',
  PRIMARY KEY (`location`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Temporary view structure for view `vw_ace_creature_object`
--

DROP TABLE IF EXISTS `vw_ace_creature_object`;
/*!50001 DROP VIEW IF EXISTS `vw_ace_creature_object`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `vw_ace_creature_object` AS SELECT 
 1 AS `baseAceObjectId`,
 1 AS `name`,
 1 AS `typeId`,
 1 AS `paletteId`,
 1 AS `ammoType`,
 1 AS `blipColor`,
 1 AS `bitField`,
 1 AS `burden`,
 1 AS `combatUse`,
 1 AS `cooldownDuration`,
 1 AS `cooldownId`,
 1 AS `effects`,
 1 AS `containersCapacity`,
 1 AS `header`,
 1 AS `hookTypeId`,
 1 AS `iconId`,
 1 AS `iconOverlayId`,
 1 AS `iconUnderlayId`,
 1 AS `hookItemTypes`,
 1 AS `itemsCapacity`,
 1 AS `location`,
 1 AS `materialType`,
 1 AS `maxStackSize`,
 1 AS `maxStructure`,
 1 AS `radar`,
 1 AS `pscript`,
 1 AS `spellId`,
 1 AS `stackSize`,
 1 AS `structure`,
 1 AS `targetTypeId`,
 1 AS `usability`,
 1 AS `useRadius`,
 1 AS `validLocations`,
 1 AS `value`,
 1 AS `workmanship`,
 1 AS `animationFrameId`,
 1 AS `defaultScript`,
 1 AS `defaultScriptIntensity`,
 1 AS `elasticity`,
 1 AS `friction`,
 1 AS `locationId`,
 1 AS `modelTableId`,
 1 AS `motionTableId`,
 1 AS `objectScale`,
 1 AS `physicsBitField`,
 1 AS `physicsState`,
 1 AS `physicsTableId`,
 1 AS `soundTableId`,
 1 AS `translucency`,
 1 AS `currentMotionState`,
 1 AS `weenieClassId`,
 1 AS `level`,
 1 AS `strength`,
 1 AS `endurance`,
 1 AS `coordination`,
 1 AS `quickness`,
 1 AS `focus`,
 1 AS `self`,
 1 AS `health`,
 1 AS `stamina`,
 1 AS `mana`,
 1 AS `baseExperience`,
 1 AS `luminance`,
 1 AS `lootTier`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `vw_ace_object`
--

DROP TABLE IF EXISTS `vw_ace_object`;
/*!50001 DROP VIEW IF EXISTS `vw_ace_object`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `vw_ace_object` AS SELECT 
 1 AS `baseAceObjectId`,
 1 AS `name`,
 1 AS `typeId`,
 1 AS `paletteId`,
 1 AS `ammoType`,
 1 AS `blipColor`,
 1 AS `bitField`,
 1 AS `burden`,
 1 AS `combatUse`,
 1 AS `cooldownDuration`,
 1 AS `cooldownId`,
 1 AS `effects`,
 1 AS `containersCapacity`,
 1 AS `header`,
 1 AS `hookTypeId`,
 1 AS `iconId`,
 1 AS `iconOverlayId`,
 1 AS `iconUnderlayId`,
 1 AS `hookItemTypes`,
 1 AS `itemsCapacity`,
 1 AS `location`,
 1 AS `materialType`,
 1 AS `maxStackSize`,
 1 AS `maxStructure`,
 1 AS `radar`,
 1 AS `pscript`,
 1 AS `spellId`,
 1 AS `stackSize`,
 1 AS `structure`,
 1 AS `targetTypeId`,
 1 AS `usability`,
 1 AS `useRadius`,
 1 AS `validLocations`,
 1 AS `value`,
 1 AS `workmanship`,
 1 AS `animationFrameId`,
 1 AS `defaultScript`,
 1 AS `defaultScriptIntensity`,
 1 AS `elasticity`,
 1 AS `friction`,
 1 AS `locationId`,
 1 AS `modelTableId`,
 1 AS `motionTableId`,
 1 AS `objectScale`,
 1 AS `physicsBitField`,
 1 AS `physicsState`,
 1 AS `physicsTableId`,
 1 AS `soundTableId`,
 1 AS `translucency`,
 1 AS `currentMotionState`,
 1 AS `weenieClassId`,
 1 AS `landblock`,
 1 AS `cell`,
 1 AS `posX`,
 1 AS `posY`,
 1 AS `posZ`,
 1 AS `qW`,
 1 AS `qX`,
 1 AS `qY`,
 1 AS `qZ`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `weenie_animation_changes`
--

DROP TABLE IF EXISTS `weenie_animation_changes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_animation_changes` (
  `weenieClassId` smallint(5) unsigned NOT NULL,
  `index` tinyint(3) unsigned NOT NULL,
  `animationId` int(10) unsigned NOT NULL,
  PRIMARY KEY (`weenieClassId`,`index`),
  CONSTRAINT `FK_weenie_animation_changes__weenieClassId` FOREIGN KEY (`weenieClassId`) REFERENCES `weenie_class` (`weenieClassId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie_class`
--

DROP TABLE IF EXISTS `weenie_class`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_class` (
  `weenieClassId` smallint(5) unsigned NOT NULL,
  `baseAceObjectId` int(10) unsigned NOT NULL,
  PRIMARY KEY (`weenieClassId`),
  KEY `FK_weenie_class__baseAceObjectId` (`baseAceObjectId`),
  CONSTRAINT `FK_weenie_class__baseAceObjectId` FOREIGN KEY (`baseAceObjectId`) REFERENCES `base_ace_object` (`baseAceObjectId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie_creature_data`
--

DROP TABLE IF EXISTS `weenie_creature_data`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_creature_data` (
  `weenieClassId` smallint(5) unsigned NOT NULL,
  `level` int(10) unsigned NOT NULL,
  `strength` int(10) unsigned NOT NULL,
  `endurance` int(10) unsigned NOT NULL,
  `coordination` int(10) unsigned NOT NULL,
  `quickness` int(10) unsigned NOT NULL,
  `focus` int(10) unsigned NOT NULL,
  `self` int(10) unsigned NOT NULL,
  `health` int(10) unsigned NOT NULL,
  `stamina` int(10) unsigned NOT NULL,
  `mana` int(10) unsigned NOT NULL,
  `baseExperience` int(10) unsigned NOT NULL,
  `luminance` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `lootTier` tinyint(3) unsigned NOT NULL DEFAULT '1',
  PRIMARY KEY (`weenieClassId`),
  CONSTRAINT `FK_weenie_creature_data__weenieClassId` FOREIGN KEY (`weenieClassId`) REFERENCES `weenie_class` (`weenieClassId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie_palette_changes`
--

DROP TABLE IF EXISTS `weenie_palette_changes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_palette_changes` (
  `weenieClassId` smallint(5) unsigned NOT NULL,
  `subPaletteId` int(10) unsigned NOT NULL,
  `offset` smallint(5) unsigned NOT NULL,
  `length` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`weenieClassId`,`subPaletteId`,`offset`,`length`),
  CONSTRAINT `FK_weenie_palette_data__weenieClassId` FOREIGN KEY (`weenieClassId`) REFERENCES `weenie_class` (`weenieClassId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie_texture_map_changes`
--

DROP TABLE IF EXISTS `weenie_texture_map_changes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_texture_map_changes` (
  `weenieClassId` smallint(5) unsigned NOT NULL,
  `index` tinyint(3) unsigned NOT NULL,
  `oldId` int(10) unsigned NOT NULL,
  `newId` int(10) unsigned NOT NULL,
  PRIMARY KEY (`weenieClassId`,`index`,`oldId`),
  CONSTRAINT `FK_weenie_texture_map_changes__weenieClassId` FOREIGN KEY (`weenieClassId`) REFERENCES `weenie_class` (`weenieClassId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Final view structure for view `vw_ace_creature_object`
--

/*!50001 DROP VIEW IF EXISTS `vw_ace_creature_object`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `vw_ace_creature_object` AS (select `bao`.`baseAceObjectId` AS `baseAceObjectId`,`bao`.`name` AS `name`,`bao`.`typeId` AS `typeId`,`bao`.`paletteId` AS `paletteId`,`bao`.`ammoType` AS `ammoType`,`bao`.`blipColor` AS `blipColor`,`bao`.`bitField` AS `bitField`,`bao`.`burden` AS `burden`,`bao`.`combatUse` AS `combatUse`,`bao`.`cooldownDuration` AS `cooldownDuration`,`bao`.`cooldownId` AS `cooldownId`,`bao`.`effects` AS `effects`,`bao`.`containersCapacity` AS `containersCapacity`,`bao`.`header` AS `header`,`bao`.`hookTypeId` AS `hookTypeId`,`bao`.`iconId` AS `iconId`,`bao`.`iconOverlayId` AS `iconOverlayId`,`bao`.`iconUnderlayId` AS `iconUnderlayId`,`bao`.`hookItemTypes` AS `hookItemTypes`,`bao`.`itemsCapacity` AS `itemsCapacity`,`bao`.`location` AS `location`,`bao`.`materialType` AS `materialType`,`bao`.`maxStackSize` AS `maxStackSize`,`bao`.`maxStructure` AS `maxStructure`,`bao`.`radar` AS `radar`,`bao`.`pscript` AS `pscript`,`bao`.`spellId` AS `spellId`,`bao`.`stackSize` AS `stackSize`,`bao`.`structure` AS `structure`,`bao`.`targetTypeId` AS `targetTypeId`,`bao`.`usability` AS `usability`,`bao`.`useRadius` AS `useRadius`,`bao`.`validLocations` AS `validLocations`,`bao`.`value` AS `value`,`bao`.`workmanship` AS `workmanship`,`bao`.`animationFrameId` AS `animationFrameId`,`bao`.`defaultScript` AS `defaultScript`,`bao`.`defaultScriptIntensity` AS `defaultScriptIntensity`,`bao`.`elasticity` AS `elasticity`,`bao`.`friction` AS `friction`,`bao`.`locationId` AS `locationId`,`bao`.`modelTableId` AS `modelTableId`,`bao`.`motionTableId` AS `motionTableId`,`bao`.`objectScale` AS `objectScale`,`bao`.`physicsBitField` AS `physicsBitField`,`bao`.`physicsState` AS `physicsState`,`bao`.`physicsTableId` AS `physicsTableId`,`bao`.`soundTableId` AS `soundTableId`,`bao`.`translucency` AS `translucency`,`bao`.`currentMotionState` AS `currentMotionState`,`wcd`.`weenieClassId` AS `weenieClassId`,`wcd`.`level` AS `level`,`wcd`.`strength` AS `strength`,`wcd`.`endurance` AS `endurance`,`wcd`.`coordination` AS `coordination`,`wcd`.`quickness` AS `quickness`,`wcd`.`focus` AS `focus`,`wcd`.`self` AS `self`,`wcd`.`health` AS `health`,`wcd`.`stamina` AS `stamina`,`wcd`.`mana` AS `mana`,`wcd`.`baseExperience` AS `baseExperience`,`wcd`.`luminance` AS `luminance`,`wcd`.`lootTier` AS `lootTier` from ((`weenie_creature_data` `wcd` join `weenie_class` `wc` on((`wcd`.`weenieClassId` = `wc`.`weenieClassId`))) join `base_ace_object` `bao` on((`wc`.`baseAceObjectId` = `bao`.`baseAceObjectId`)))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `vw_ace_object`
--

/*!50001 DROP VIEW IF EXISTS `vw_ace_object`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `vw_ace_object` AS (select `bao`.`baseAceObjectId` AS `baseAceObjectId`,`bao`.`name` AS `name`,`bao`.`typeId` AS `typeId`,`bao`.`paletteId` AS `paletteId`,`bao`.`ammoType` AS `ammoType`,`bao`.`blipColor` AS `blipColor`,`bao`.`bitField` AS `bitField`,`bao`.`burden` AS `burden`,`bao`.`combatUse` AS `combatUse`,`bao`.`cooldownDuration` AS `cooldownDuration`,`bao`.`cooldownId` AS `cooldownId`,`bao`.`effects` AS `effects`,`bao`.`containersCapacity` AS `containersCapacity`,`bao`.`header` AS `header`,`bao`.`hookTypeId` AS `hookTypeId`,`bao`.`iconId` AS `iconId`,`bao`.`iconOverlayId` AS `iconOverlayId`,`bao`.`iconUnderlayId` AS `iconUnderlayId`,`bao`.`hookItemTypes` AS `hookItemTypes`,`bao`.`itemsCapacity` AS `itemsCapacity`,`bao`.`location` AS `location`,`bao`.`materialType` AS `materialType`,`bao`.`maxStackSize` AS `maxStackSize`,`bao`.`maxStructure` AS `maxStructure`,`bao`.`radar` AS `radar`,`bao`.`pscript` AS `pscript`,`bao`.`spellId` AS `spellId`,`bao`.`stackSize` AS `stackSize`,`bao`.`structure` AS `structure`,`bao`.`targetTypeId` AS `targetTypeId`,`bao`.`usability` AS `usability`,`bao`.`useRadius` AS `useRadius`,`bao`.`validLocations` AS `validLocations`,`bao`.`value` AS `value`,`bao`.`workmanship` AS `workmanship`,`bao`.`animationFrameId` AS `animationFrameId`,`bao`.`defaultScript` AS `defaultScript`,`bao`.`defaultScriptIntensity` AS `defaultScriptIntensity`,`bao`.`elasticity` AS `elasticity`,`bao`.`friction` AS `friction`,`bao`.`locationId` AS `locationId`,`bao`.`modelTableId` AS `modelTableId`,`bao`.`motionTableId` AS `motionTableId`,`bao`.`objectScale` AS `objectScale`,`bao`.`physicsBitField` AS `physicsBitField`,`bao`.`physicsState` AS `physicsState`,`bao`.`physicsTableId` AS `physicsTableId`,`bao`.`soundTableId` AS `soundTableId`,`bao`.`translucency` AS `translucency`,`bao`.`currentMotionState` AS `currentMotionState`,`ao`.`weenieClassId` AS `weenieClassId`,`ao`.`landblock` AS `landblock`,`ao`.`cell` AS `cell`,`ao`.`posX` AS `posX`,`ao`.`posY` AS `posY`,`ao`.`posZ` AS `posZ`,`ao`.`qW` AS `qW`,`ao`.`qX` AS `qX`,`ao`.`qY` AS `qY`,`ao`.`qZ` AS `qZ` from (`ace_object` `ao` join `base_ace_object` `bao` on((`ao`.`baseAceObjectId` = `bao`.`baseAceObjectId`)))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2017-04-25  1:19:57

-- MySQL dump 10.13  Distrib 5.7.12, for Win64 (x86_64)
--
-- Host: localhost    Database: ace_world
-- ------------------------------------------------------
-- Server version	5.7.17-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Dumping data for table `portal_destination`
--

LOCK TABLES `portal_destination` WRITE;
/*!40000 ALTER TABLE `portal_destination` DISABLE KEYS */;
INSERT INTO `portal_destination` VALUES (1090,2207318070,156,132,124.005,0,0,0,0,0,0),(1092,2558263312,36.456,178.58,27.058,0,0,0,0.998,0,0),(1096,3662938154,135.1,42.7,34,0,0,-0.258819,-0.965926,0,0),(1099,32047533,120.1,-130.1,-12,0,0,0,271.2,0,0),(1100,3264872499,155.892,48.389,69.973,0,0,0,-0.98,0,0),(1101,2103443501,121.804,108.905,50.005,0,0,0,-0.256,0,0),(1113,3147759664,122.3,176.6,55.1,0,0,0,110,0,0),(1120,33162060,80,-59,6.005,0,0,0,-0.346,0,0),(1123,31981930,-0.6,-38.5,0.005,0,0,0,-0.997,0,0),(1124,32965104,40,-140,0.005,0,0,0,1,0,0),(1126,33227270,92,-20,0.005,0,0,0,-0.707,0,0),(1309,31785486,80.915,-69.263,0,0,0,-0.0191975,0.999816,0,0),(1310,3629908031,187.164,145.655,13.602,0,0,0,1,0,0),(1339,31130079,60,-60,0.005,0,0,0,-0.927,0,0),(1340,2123431969,102.705,15.752,76.005,0,0,0,0.64,0,0),(1345,30998784,3,-49.9,0,0,0,0,-0.707,0,0),(1346,2086993962,143.336,24.174,7.964,0,0,0,0.743,0,0),(1512,30147045,70.5,-71,12.005,0,0,0,1,10,0),(1513,2174091295,84,154.963,178.918,0,0,0,0,0,0),(1590,29819169,39.729,-49.79,0,0,0,0,-0.022,0,0),(1591,2518286341,23,115.099,278,0,0,0,-0.979,0,0),(1592,29884689,20,-60,0,0,0,0,-0.025,0,0),(1593,2535849995,31.7,66.2,140.5,0,0,0,-0.965,0,0),(1595,29950509,71.5,-30.6,0.005,0,0,-0.992713,0.120503,0,0),(1596,3282763809,108.9,5.61,116,0,0,0,180.5,0,0),(1902,2863136785,48,24,5.61,0,0,0,153.7,0,0),(2068,27132180,10,-40,0.005,0,0,-0.0375863,0.999293,5,0),(2069,2847080450,13.2,35.4,94.005,0,0,-0.283647,0.958929,0,0),(2072,28705024,0.24,-0.7,0,0,0,-0.999963,0.00855181,0,0),(2073,3629973517,34.577,116,12.1,0,0,-0.99773,0.0673361,0,0),(2088,28901663,0,-10,0,0,0,0,-0.687,1,0),(2339,27984183,37.63,-29.989,0,0,0,0,0,0,0),(2340,2485977108,65.248,73.353,136.454,0,0,0,0.325,0,0),(3630,26149468,70.547,-77.66,0.005,0,0,0,0,0,0),(3631,2156396559,36,156,348.005,0,0,0,0,0,0),(4160,25428299,60,-80,0,0,0,-0.722967,0.690882,0,0),(4161,3612409875,69.673,62.982,38.005,0,0,-0.488149,0.87276,0,0),(4162,25362863,60,-50,0.005,0,0,0,0,1,12),(4163,2557935654,97.717,127.997,31.34,0,0,0,-0.74,0,0),(4164,25297275,39.257,-29.65,0.005,0,0,0,-0.717,0,0),(4165,2692284454,101.903,127.983,24.005,0,0,0,0.99,0,0),(4204,25756449,139.792,-66.582,6.005,0,0,0,-0.999,10,0),(4205,2707357730,110.8,25.399,35.2,0,0,0,-0.861,0,0),(4917,21299560,91.773,-26.584,0.005,0,0,0,0.708,0,0),(4918,2287599638,62.134,140.233,9.691,0,0,0,-0.574,0,0),(4929,23527847,109.999,-39.974,6.005,0,0,0,0,0,0),(4930,2471952398,24.488,126.451,40.005,0,0,0,0.525,0,0),(4935,23265708,140,-40,0,0,0,0,-0.707107,0,0),(4936,2863726624,93.391,180.593,123.788,0,0,0,-0.76,0,0),(4937,23200210,60,-10,12.005,0,0,-0.999574,-0.0291995,0,0),(4938,2813526046,91.1482,143.157,30.005,0,0,0.711473,-0.702713,0,0),(4939,22741358,30,0,6,0,0,0,-0.707,1,20),(4940,2321088574,169.451,136.038,20.246,0,0,0,0.254,0,0),(4943,22610322,179.978,-52.863,0.005,0,0,0,0,40,0),(4944,2404843537,60,12,34.005,0,0,0,0,0,0),(4947,22675886,10,-90,0,0,0,0,-0.707,1,20),(4948,2574843938,108,27.19,26,0,0,0,-1,0,0),(4955,22282622,30,-10,-6,0,0,0,1,1,20),(4956,3629383705,84.8,2.1,31.83,0,0,0,1,0,0),(4965,22217448,45.7,-43,-17.995,0,0,-0.5,-0.866025,1,20),(4966,3629514768,35.6,189.2,30,0,0,-0.998931,-0.0462346,0,0),(4971,21234100,30.018,-49.985,12.005,0,0,0,1,20,0),(4972,2154102805,55.596,97.963,80.005,0,0,0,-0.094,0,0),(4975,22937959,79.86,-170.198,0.005,0,0,0,-0.7,20,0),(5198,22413714,49.1,-61.9,0.005,0,0,0,0,1,2),(5199,2036727866,170.313,35.817,50.005,0,0,0,0.718,0,0),(5200,22479175,0,-20,0.005,0,0,0,0,0,0),(5201,2053373974,52.489,121.714,-0.095,0,0,0,0.96,0,0),(5202,22348184,20,-10,12,0,0,0,-1,1,20),(5203,2524839994,171.393,32.249,30.005,0,0,0,-0.707,0,0),(5505,21037478,70,-170,0.005,0,0,0,0,0,0),(5506,2705391622,11.199,136.163,110.005,0,0,0,0,0,0),(5509,21692947,80,-120,0,0,0,0,0,8,0),(5510,2655584277,59.348,107.692,84.95,0,0,0,0,0,0),(5511,21627201,-1.218,-47.664,0.005,0,0,0,0,15,0),(5512,2273902633,134.024,12.796,73.408,0,0,0,0,0,0),(5515,20840719,110.35,-129.723,-29.995,0,0,0,0,150,0),(5517,20775637,60,-10,-18,0,0,0,0,15,0),(5518,2206662912,35.861,98.068,79.66,0,0,0,0,0,0),(5604,20709889,10,-40,6,0,0,0,-0.909,0,0),(5605,2577793055,75.79,163.765,52.005,0,0,0,0.982,0,0),(6088,18678054,30,-60,6,0,0,0,0,0,0),(6090,18809126,30,-60,6,0,0,0,0,0,0),(6091,18874662,30,-60,6,0,0,0,0,0,0),(6092,18940198,30,-60,6.005,0,0,0.0124046,0.999923,0,0),(6093,19005734,30,-60,6,0,0,0,0,0,0),(6094,19071270,30,-60,6,0,0,0,0,0,0),(6095,19136806,30,-60,6,0,0,0,0,0,0),(6096,19202342,30,-60,6.005,0,0,0.0124045,0.999923,0,0),(6097,19267878,30,-60,6,0,0,0,0,0,0),(6099,19398950,30,-60,6,0,0,0,0,0,0),(6100,19464486,30,-60,6,0,0,0,0,0,0),(6101,19530022,30,-60,6,0,0,0,1,0,0),(6103,19661094,30,-60,6,0,0,0,0,0,0),(6104,19726630,30,-60,6,0,0,0,0,0,0),(6105,19792166,30,-60,6,0,0,0,0,0,0),(6106,19857702,30,-60,6,0,0,0,0,0,0),(6108,19988774,30,-60,6,0,0,0,0,0,0),(6109,20054310,30,-60,6,0,0,0,0,0,0),(6111,20185382,30,-60,6,0,0,0,0,0,0),(6112,20250918,30,-60,6,0,0,0,0,0,0),(6114,18613002,90,-200,0,0,0,0,0,0,0),(6115,2551644181,69.9,96.8,390.421,0,0,0,-0.707,0,0),(6795,17826529,40,-250,24.005,0,0,0,0,100,0),(6796,474808354,98.8,45.5,0.655,0,0,0,-0.707,0,0),(7194,288620581,99.809,107.91,42.005,0,0,0,0,0,0),(7206,17236326,60,-30,0.005,0,0,0,0,0,0),(7207,2388000791,56.038,152.038,15.344,0,0,0,0,0,0),(7243,322437182,180,132,98.005,0,0,0,0,0,0),(7244,17105781,100,-210,0,0,0,0,0,20,0),(7291,49480343,100,-330,0,0,0,0,0,15,0),(7294,255393852,175.849,84,38.659,0,0,0,0,0,0),(7319,49348946,140,-130,0,0,0,0,0,0,0),(7320,2636578870,155,139.3,250.363,0,0,0,0,0,0),(8367,46793287,130,-220,0.005,0,0,0,0,0,0),(8368,2539585584,133.148,184.745,9.7,0,0,0,0,0,0),(9296,44106086,260,0,-35.994,0,0,0,0,32,0),(9297,372572181,55.539,104.579,63.976,0,0,0,0,0,0),(9320,43843846,50,-50,0.005,0,0,0,-1,15,0),(9321,43843852,100,0,0.005,0,0,0,-1,30,0),(9322,43843840,0,0,0,0,0,0,-1,0,0),(9323,2474377275,183,48,36.009,0,0,0,-1,0,0),(12150,61014529,70.032,-592.154,0.005,0,0,0,0,80,0),(12152,60949086,310,-10,12,0,0,0,0,80,0),(12153,2654863393,108.573,13.193,87.398,0,0,0,0,0,0),(19131,645988381,77.7,108.1,240,0,0,-0.85,-0.52,0,0),(19133,3460366343,12.6,152.8,55.06,0,0,-0.84,-0.54,0,0),(19135,3862822946,96.96,37.72,74.57,0,0,-1,0,0,0),(19137,3164536860,94.5,74.5,48.351,0,0,0,0.602,0,0),(19715,1414988229,160.001,-89.987,0.005,0,0,-0.707,-0.707,0,0),(19716,1415053919,10,-110,6.005,0,0,0,1,0,0),(19717,1415184700,10,-50,6,0,0,0,1,0,0),(19718,1415119283,7.6,-100,6,0,0,0,1,0,0),(19726,1415250209,50,-50,0,0,0,0,0,20,0),(19727,2273706006,60,132,154.005,0,0,0,0,0,0),(23032,23855548,49.206,-31.935,0.005,0,0,0,0.707,0,0),(24434,1631912814,140.042,-182.837,0.005,0,0,0,0,60,80),(24436,1665598318,140.042,-182.837,0.005,0,0,0,0,60,0),(25494,1632174814,29.977,-16.228,0.005,0,0,0,0,0,0),(25495,1632240739,90.066,-120.64,0.005,0,0,0,-0.01,0,0),(25511,2504065079,156,156,40.005,0,0,0,0,0,0),(27694,1699414330,80,-90,0.005,0,0,0,1,0,0),(27695,3495165992,98.2248,185.695,238.005,0,0,-0.155874,0.987777,0,0),(28267,24772919,29.6039,-10.1276,0.01,0,0,0,1,10,0),(28268,3663134748,92.4422,76.7921,14.005,0,0,-0.156434,-0.987688,0,0),(29334,2248344259,119,-141,0.0065,0,0,1,0,0,0),(29337,3663003677,84.8,99,20,0,0,0,1,0,0),(29338,2847146009,84,7.1,94.01,0,0,-0.08,1,0,0),(29339,869859336,7.5,184.35,52.01,0,0,0,-1,0,0),(29340,2103705613,31.9,104.6,11.95,0,0,-0.82,0.58,0,0),(29433,1573319,90,-8,0.005,0,0,0,-1,50,0),(29434,2156789773,36,108,124.005,0,0,0,0,0,0),(29435,1638880,90.201,-129.36,12.005,0,0,0,1,80,0),(29439,1769838,80,-10,0,0,0,0,-1,65,0),(29440,2106589220,108,74.036,124.005,0,0,0,0,0,0),(31061,2248344094,50,-54,0.0065,0,0,-1,0,0,0),(32993,8454440,80,-10,0.005,0,0,0,0,0,0),(37163,12124820,110.034,-19.989,0.005,0,0,0,0,180,0),(42811,1691680779,30,50,78.01,0,0,-0.54,0.84,0,0),(42812,1236729889,100.1,20.8,238.61,0,0,-0.81,-0.59,0,0),(42813,3681878075,186,65,36,0,0,0.65,-0.75,0,0),(42814,2695102501,96.3,119.85,59.95,0,0,-0.71,0.71,0,0),(42815,3465805877,151.05,112.61,17.42,0,0,-0.35,-0.94,0,0),(42816,3229614087,11.72,155.56,33.03,0,0,-0.92,-0.4,0,0),(42817,3381395496,113.67,190.26,22,0,0,-0.71,-0.71,0,0),(42818,3147759680,169.36,168.25,54.01,0,0,-0.82,0.58,0,0),(42819,3332964361,46.81,4.22,42.01,0,0,0,1,0,0),(42820,2847146009,84,7.1,0,0,0,-0.08,1,0,0),(42821,2724200508,182.92,87.93,20,0,0,-0.93,-0.36,0,0),(42822,2672033818,90,24.55,36.55,0,0,-0.62,-0.78,0,0),(42823,2404909115,183.85,60.18,9.33,0,0,-0.71,0.71,0,0),(42824,2103705613,31.9,104.6,11.95,0,0,-0.82,0.58,0,0),(42825,565182487,48.19,165.89,0,0,0,-1,-0.08,0,0),(42826,733282364,178.96,86.57,0,0,0,-0.94,0.35,0,0),(42827,263782409,43,8.6,0,0,0,-0.2,-0.98,0,0),(42828,2513829939,146.9,71.3,99.76,0,0,-0.68,-0.73,0,0),(42829,2272002056,2,186.9,18,0,0,-0.71,-0.71,0,0),(42830,2240282668,120.36,95.47,90.05,0,0,0,1,0,0),(42831,2156920851,64.86,55.69,124.01,0,0,-0.37,-0.93,0,0),(42832,2471165985,108.3,6.1,18.14,0,0,-0.26,-0.96,0,0),(42833,2535587898,168.35,24.62,102.01,0,0,-0.39,-0.92,0,0),(42834,2541420556,25.81,73.85,0,0,0,-0.37,0.93,0,0),(42835,869859336,7.5,184.35,52.01,0,0,0,-1,0,0),(42836,397541418,132.62,25.81,44.01,0,0,-0.06,1,0,0),(42838,2719875098,83,38,560.36,0,0,0,1,0,0),(42840,3663003677,84.8,99,20,0,0,0,1,0,0),(42841,4133027845,23.741,107.236,20,0,0,-0.691,-0.722,0,0),(42842,3862036513,107.42,10.76,29.91,0,0,-0.77,-0.64,0,0),(42843,3862822946,96.96,37.72,74.57,0,0,-1,0,0,0),(42844,3694919697,59.72,10.77,18.05,0,0,-0.93,-0.36,0,0),(42845,3460366343,12.6,152.8,55.06,0,0,-0.84,-0.54,0,0),(42846,3863871535,138.3,161.9,20.04,0,0,-0.38,0.92,0,0),(42847,3027173406,75.2,124.1,34.69,0,0,0,1,0,0),(42848,3122069561,181.2,3.2,167.6,0,0,-0.53,-0.85,0,0),(42849,3378184193,14.8,0.3,12,0,0,-0.37,0.93,0,0),(42850,3894542378,132.7,37.9,20.11,0,0,-0.5,-0.87,0,0),(42851,2315387410,60.014,-91.413,6.005,-1,0,0,0,0,0),(42852,459147,70,-80,0.005,0,0,0,-1,0,0),(42998,669777941,57.456,101.009,80.005,0,0,0,0,0,0),(43000,498466853,98.5,98.1,120.01,0,0,-0.59,0.81,0,0),(43001,645988381,77.7,108.1,240,0,0,-0.85,-0.52,0,0),(43002,4135714867,145.7,49.85,58.01,0,0,-0.88,-0.47,0,0),(43003,1520173060,23.5,77.1,6.01,0,0,-1,0,0,0),(43004,4062445594,81.8,33,0,0,0,-0.97,0.24,0,0),(43065,459075,70,-60,0.005,0,0,-1,0,0,0),(43066,459059,60,-70,0.005,0,0,0.707107,-0.707107,0,0),(43067,459094,80,-70,0.005,0,0,0.707107,0.707107,0,0);
/*!40000 ALTER TABLE `portal_destination` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `teleport_location`
--

LOCK TABLES `teleport_location` WRITE;
/*!40000 ALTER TABLE `teleport_location` DISABLE KEYS */;
INSERT INTO `teleport_location` VALUES ('Aerlinthe',3135766557,84,105,26,0,0,-1,0),('Ahurenga',263782409,43,8.6,0,0,0,-0.2,-0.98),('Al-Arqas',2404909115,183.85,60.18,9.33,0,0,-0.71,0.71),('Al-Jalima',2240282668,120.36,95.47,90.05,0,0,0,1),('Arwic',3332964361,46.81,4.22,42.01,0,0,0,1),('Ayan Baqur',288620581,99.81,107.91,42.01,0,0,-0.7,0.71),('Baishi',3460366343,12.6,152.8,55.06,0,0,-0.84,-0.54),('Bandit Castle',3184525318,16.9,120.5,115.1,0,0,-0.71,0.71),('Beach Fort',1121845260,25,84.5,0,0,0,-0.73,-0.68),('Bluespire',565182487,48.19,165.89,0,0,0,-1,-0.08),('Candeth Keep',722534461,189.14,98.8,48.01,0,0,-0.37,-0.93),('Caul',151257096,11.4,188.6,87.52,0,0,-0.09,-1),('Cragstone',3147759680,169.36,168.25,54.01,0,0,-0.82,0.58),('Crater',2429550855,95.52,84,277.2,0,0,-0.71,-0.71),('Dryreach',3681878075,186,65,36,0,0,0.65,-0.75),('Eastham',3465805877,151.05,112.61,17.42,0,0,-0.35,-0.94),('Fort Tethana',645988381,77.7,108.1,240,0,0,-0.85,-0.52),('Freehold',4062445594,81.8,33,0,0,0,-0.97,0.24),('Glenden Wood',2695102501,96.3,119.85,59.95,0,0,-0.71,0.71),('Greenspire',733282364,178.96,86.57,0,0,0,-0.94,0.35),('Hebian-to',3863871535,138.3,161.9,20.04,0,0,-0.38,0.92),('Holtburg',2847146009,84,7.1,94.01,0,0,-0.08,1),('Kara',3122069561,181.2,3.2,167.6,0,0,-0.53,-0.85),('Khayyaban',2672033818,90,24.55,36.55,0,0,-0.62,-0.78),('Kryst',3894542378,132.7,37.9,20.11,0,0,-0.5,-0.87),('Lin',3694919697,59.72,10.77,18.05,0,0,-0.93,-0.36),('Linvak Tukal',2719875098,83,38,560.36,0,0,0,1),('Lytelthorpe',3229614087,11.72,155.56,33.03,0,0,-0.92,-0.4),('Mayoi',3862036513,107.42,10.76,29.91,0,0,-0.77,-0.64),('Nanto',3862822946,96.96,37.72,74.57,0,0,-1,0),('Neydisa',2513829939,146.9,71.3,99.76,0,0,-0.68,-0.73),('Outpost',1520173060,23.5,77.1,6.01,0,0,-1,0),('Plateau',1236729889,100.1,20.8,238.61,0,0,-0.81,-0.59),('Qalabar',2535587898,168.35,24.62,102.01,0,0,-0.39,-0.92),('Redspire',397541418,132.62,25.81,44.01,0,0,-0.06,1),('Refuge',4135714867,145.7,49.85,58.01,0,0,-0.88,-0.47),('Rithwic',3381395496,113.67,190.26,22,0,0,-0.71,-0.71),('Samsur',2541420556,25.81,73.85,0,0,0,-0.37,0.93),('Sawato',3378184193,14.8,0.3,12,0,0,-0.37,0.93),('Shoushi',3663003677,84.8,99,20,0,0,0,1),('Stonehold',1691680779,30,50,78.01,0,0,-0.54,0.84),('Timaru',498466853,98.5,98.1,120.01,0,0,-0.59,0.81),('Tou-Tou',4133224491,126.39,54.15,20,0,0,-0.37,0.93),('Tufa',2272002056,2,186.9,18,0,0,-0.71,-0.71),('Underground',32047533,120,-130,-12,0,0,-0.7,-0.71),('Uziz',2724200508,182.92,87.93,20,0,0,-0.93,-0.36),('Wai Jhou',1060175903,83.1,156.19,2.78,0,0,0.84,0.54),('Xarabydun',2471165985,108.3,6.1,18.14,0,0,-0.26,-0.96),('Yanshi',3027173406,75.2,124.1,34.69,0,0,0,1),('Yaraq',2103705613,31.9,104.6,11.95,0,0,-0.82,0.58),('Zaikhal',2156920851,64.86,55.69,124.01,0,0,-0.37,-0.93);
/*!40000 ALTER TABLE `teleport_location` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2017-04-26  0:25:22
