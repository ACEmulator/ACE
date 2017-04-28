-- MySQL dump 10.13  Distrib 5.7.12, for Win64 (x86_64)
--
-- Host: localhost    Database: ace_character
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

CREATE DATABASE /*!32312 IF NOT EXISTS*/`ace_character` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `ace_character`;

--
-- Table structure for table `character`
--

DROP TABLE IF EXISTS `character`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character` (
  `guid` int(10) unsigned NOT NULL DEFAULT '0',
  `accountId` int(10) unsigned NOT NULL DEFAULT '0',
  `name` varchar(32) NOT NULL DEFAULT '',
  `templateOption` tinyint(1) unsigned NOT NULL,
  `startArea` tinyint(1) unsigned NOT NULL,
  `birth` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `deleteTime` bigint(20) unsigned NOT NULL DEFAULT '0',
  `deleted` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `totalLogins` int(10) unsigned NOT NULL DEFAULT '0',
  `characterOptions1` int(10) unsigned NOT NULL DEFAULT '0',
  `characterOptions2` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `character_appearance`
--

DROP TABLE IF EXISTS `character_appearance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_appearance` (
  `id` int(10) unsigned NOT NULL,
  `eyes` tinyint(1) unsigned NOT NULL,
  `nose` tinyint(1) unsigned NOT NULL,
  `mouth` tinyint(1) unsigned NOT NULL,
  `eyeColor` tinyint(1) unsigned NOT NULL,
  `hairColor` tinyint(1) unsigned NOT NULL,
  `hairStyle` tinyint(1) unsigned NOT NULL,
  `hairHue` double NOT NULL,
  `skinHue` double NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `FK_character_appearance_id_character_guid` FOREIGN KEY (`id`) REFERENCES `character` (`guid`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `character_friends`
--

DROP TABLE IF EXISTS `character_friends`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_friends` (
  `id` int(10) unsigned NOT NULL,
  `friendId` int(10) unsigned NOT NULL,
  PRIMARY KEY (`id`,`friendId`),
  KEY `FK_character_friends_friendId_character_guid` (`friendId`),
  CONSTRAINT `FK_character_friends_friendId_character_guid` FOREIGN KEY (`friendId`) REFERENCES `character` (`guid`),
  CONSTRAINT `FK_character_friends_id_character_guid` FOREIGN KEY (`id`) REFERENCES `character` (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `character_position`
--

DROP TABLE IF EXISTS `character_position`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_position` (
  `character_id` int(10) unsigned NOT NULL,
  `positionType` tinyint(3) unsigned NOT NULL,
  `cell` int(10) unsigned NOT NULL DEFAULT '0',
  `positionX` float NOT NULL DEFAULT '0',
  `positionY` float NOT NULL DEFAULT '0',
  `positionZ` float NOT NULL DEFAULT '0',
  `rotationX` float NOT NULL DEFAULT '0',
  `rotationY` float NOT NULL DEFAULT '0',
  `rotationZ` float NOT NULL DEFAULT '0',
  `rotationW` float NOT NULL DEFAULT '0',
  PRIMARY KEY (`character_id`,`positionType`),
  CONSTRAINT `FK_character_position_id_character_guid` FOREIGN KEY (`character_id`) REFERENCES `character` (`guid`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `character_properties_bigint`
--

DROP TABLE IF EXISTS `character_properties_bigint`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_properties_bigint` (
  `guid` int(10) unsigned NOT NULL DEFAULT '0',
  `propertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyValue` bigint(20) unsigned NOT NULL DEFAULT '0',
  UNIQUE KEY `character_guid__property_id` (`guid`,`propertyId`),
  KEY `guid` (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `character_properties_bool`
--

DROP TABLE IF EXISTS `character_properties_bool`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_properties_bool` (
  `guid` int(10) unsigned NOT NULL DEFAULT '0',
  `propertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyValue` tinyint(1) NOT NULL DEFAULT '0',
  UNIQUE KEY `character_guid__property_id` (`guid`,`propertyId`),
  KEY `guid` (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `character_properties_double`
--

DROP TABLE IF EXISTS `character_properties_double`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_properties_double` (
  `guid` int(10) unsigned NOT NULL DEFAULT '0',
  `propertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyValue` double NOT NULL DEFAULT '0',
  UNIQUE KEY `character_guid__property_id` (`guid`,`propertyId`),
  KEY `guid` (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `character_properties_int`
--

DROP TABLE IF EXISTS `character_properties_int`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_properties_int` (
  `guid` int(10) unsigned NOT NULL DEFAULT '0',
  `propertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyValue` int(10) unsigned NOT NULL DEFAULT '0',
  UNIQUE KEY `character_guid__property_id` (`guid`,`propertyId`),
  KEY `guid` (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `character_properties_string`
--

DROP TABLE IF EXISTS `character_properties_string`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_properties_string` (
  `guid` int(10) unsigned NOT NULL DEFAULT '0',
  `propertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyValue` text NOT NULL,
  UNIQUE KEY `character_guid__property_id` (`guid`,`propertyId`),
  KEY `guid` (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `character_skills`
--

DROP TABLE IF EXISTS `character_skills`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_skills` (
  `id` int(10) unsigned NOT NULL,
  `skillId` tinyint(1) unsigned NOT NULL,
  `skillStatus` tinyint(1) unsigned NOT NULL,
  `skillPoints` smallint(2) unsigned NOT NULL DEFAULT '0',
  `skillXpSpent` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`,`skillId`),
  CONSTRAINT `FK_character_skills_id_character_guid` FOREIGN KEY (`id`) REFERENCES `character` (`guid`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `character_startup_gear`
--

DROP TABLE IF EXISTS `character_startup_gear`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_startup_gear` (
  `id` int(10) unsigned NOT NULL,
  `headgearStyle` int(10) unsigned NOT NULL,
  `headgearColor` tinyint(1) unsigned NOT NULL,
  `headgearHue` double NOT NULL,
  `shirtStyle` tinyint(1) unsigned NOT NULL,
  `shirtColor` tinyint(1) unsigned NOT NULL,
  `shirtHue` double NOT NULL,
  `pantsStyle` tinyint(1) unsigned NOT NULL,
  `pantsColor` tinyint(1) unsigned NOT NULL,
  `pantsHue` double NOT NULL,
  `footwearStyle` tinyint(1) unsigned NOT NULL,
  `footwearColor` tinyint(1) unsigned NOT NULL,
  `footwearHue` double NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `FK_character_startupgear_id_character_guid` FOREIGN KEY (`id`) REFERENCES `character` (`guid`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `character_stats`
--

DROP TABLE IF EXISTS `character_stats`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `character_stats` (
  `id` int(10) unsigned NOT NULL,
  `strength` tinyint(1) unsigned NOT NULL,
  `strengthXpSpent` int(10) unsigned NOT NULL DEFAULT '0',
  `strengthRanks` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `endurance` tinyint(1) unsigned NOT NULL,
  `enduranceXpSpent` int(10) unsigned NOT NULL DEFAULT '0',
  `enduranceRanks` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `coordination` tinyint(1) unsigned NOT NULL,
  `coordinationXpSpent` int(10) unsigned NOT NULL DEFAULT '0',
  `coordinationRanks` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `quickness` tinyint(1) unsigned NOT NULL,
  `quicknessXpSpent` int(10) unsigned NOT NULL DEFAULT '0',
  `quicknessRanks` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `focus` tinyint(1) unsigned NOT NULL,
  `focusXpSpent` int(10) unsigned NOT NULL DEFAULT '0',
  `focusRanks` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `self` tinyint(1) unsigned NOT NULL,
  `selfXpSpent` int(10) unsigned NOT NULL DEFAULT '0',
  `healthXpSpent` int(10) unsigned NOT NULL DEFAULT '0',
  `staminaXpSpent` int(10) unsigned NOT NULL DEFAULT '0',
  `manaXpSpent` int(10) unsigned NOT NULL DEFAULT '0',
  `selfRanks` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `healthRanks` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `healthCurrent` smallint(2) unsigned NOT NULL DEFAULT '0',
  `staminaRanks` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `staminaCurrent` smallint(2) unsigned NOT NULL DEFAULT '0',
  `manaRanks` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `manaCurrent` smallint(2) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  CONSTRAINT `FK_character_stats_id_character_guid` FOREIGN KEY (`id`) REFERENCES `character` (`guid`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Temporary view structure for view `vw_character_positions`
--

DROP TABLE IF EXISTS `vw_character_positions`;
/*!50001 DROP VIEW IF EXISTS `vw_character_positions`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `vw_character_positions` AS SELECT 
 1 AS `character_id`,
 1 AS `positionType`,
 1 AS `cell`,
 1 AS `positionX`,
 1 AS `positionY`,
 1 AS `positionZ`,
 1 AS `rotationX`,
 1 AS `rotationY`,
 1 AS `rotationZ`,
 1 AS `rotationW`*/;
SET character_set_client = @saved_cs_client;

--
-- Final view structure for view `vw_character_positions`
--

/*!50001 DROP VIEW IF EXISTS `vw_character_positions`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `vw_character_positions` AS (select `cp`.`character_id` AS `character_id`,`cp`.`positionType` AS `positionType`,`cp`.`cell` AS `cell`,`cp`.`positionX` AS `positionX`,`cp`.`positionY` AS `positionY`,`cp`.`positionZ` AS `positionZ`,`cp`.`rotationX` AS `rotationX`,`cp`.`rotationY` AS `rotationY`,`cp`.`rotationZ` AS `rotationZ`,`cp`.`rotationW` AS `rotationW` from (`character_position` `cp` join `character` `ch` on((`cp`.`character_id` = `ch`.`guid`)))) */;
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

-- Dump completed on 2017-04-25  1:22:33
