CREATE DATABASE  IF NOT EXISTS `ace_shard` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `ace_shard`;
-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: localhost    Database: ace_shard
-- ------------------------------------------------------
-- Server version	5.7.19-log

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
-- Table structure for table `ace_contract_tracker`
--

DROP TABLE IF EXISTS `ace_contract_tracker`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_contract_tracker` (
  `aceObjectId` int(10) unsigned NOT NULL,
  `contractId` int(10) unsigned NOT NULL,
  `version` int(10) unsigned NOT NULL,
  `stage` int(10) unsigned NOT NULL,
  `timeWhenDone` bigint(20) unsigned NOT NULL,
  `timeWhenRepeats` bigint(20) unsigned NOT NULL,
  `deleteContract` int(10) unsigned NOT NULL,
  `setAsDisplayContract` int(10) unsigned NOT NULL,
  PRIMARY KEY (`aceObjectId`,`contractId`),
  KEY `ace_contract_aceObject` (`aceObjectId`),
  CONSTRAINT `fk_contract_ace_object` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_contract_tracker`
--

LOCK TABLES `ace_contract_tracker` WRITE;
/*!40000 ALTER TABLE `ace_contract_tracker` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_contract_tracker` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_object`
--

DROP TABLE IF EXISTS `ace_object`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_object` (
  `aceObjectId` int(10) unsigned NOT NULL,
  `aceObjectDescriptionFlags` int(10) unsigned NOT NULL,
  `weenieClassId` int(10) unsigned NOT NULL,
  `userModified` tinyint(1) NOT NULL DEFAULT '0' COMMENT 'flag indicating whether or not this has record has been altered since deployment',
  `weenieHeaderFlags` int(10) unsigned DEFAULT NULL,
  `weenieHeaderFlags2` int(10) unsigned DEFAULT NULL,
  `physicsDescriptionFlag` int(10) unsigned DEFAULT NULL,
  `currentMotionState` text,
  PRIMARY KEY (`aceObjectId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_object`
--

LOCK TABLES `ace_object` WRITE;
/*!40000 ALTER TABLE `ace_object` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_object` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_object_animation_change`
--

DROP TABLE IF EXISTS `ace_object_animation_change`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_object_animation_change` (
  `aceObjectId` int(10) unsigned NOT NULL,
  `index` tinyint(3) unsigned NOT NULL,
  `animationId` int(10) unsigned NOT NULL,
  PRIMARY KEY (`aceObjectId`,`index`),
  CONSTRAINT `FK_ace_object_animation_changes__baseAceObjectId` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_object_animation_change`
--

LOCK TABLES `ace_object_animation_change` WRITE;
/*!40000 ALTER TABLE `ace_object_animation_change` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_object_animation_change` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_object_palette_change`
--

DROP TABLE IF EXISTS `ace_object_palette_change`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_object_palette_change` (
  `aceObjectId` int(10) unsigned NOT NULL,
  `subPaletteId` int(10) unsigned NOT NULL,
  `offset` smallint(5) unsigned NOT NULL,
  `length` smallint(5) unsigned zerofill NOT NULL,
  PRIMARY KEY (`aceObjectId`,`subPaletteId`,`offset`,`length`),
  CONSTRAINT `FK_ace_object_palette_data__baseAceObjectId` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_object_palette_change`
--

LOCK TABLES `ace_object_palette_change` WRITE;
/*!40000 ALTER TABLE `ace_object_palette_change` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_object_palette_change` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_object_properties_attribute`
--

DROP TABLE IF EXISTS `ace_object_properties_attribute`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_object_properties_attribute` (
  `aceObjectId` int(10) unsigned NOT NULL,
  `attributeId` smallint(4) unsigned NOT NULL,
  `attributeBase` smallint(4) unsigned NOT NULL DEFAULT '0',
  `attributeRanks` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `attributeXpSpent` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`aceObjectId`,`attributeId`),
  UNIQUE KEY `ace_object__property_attribute_id` (`aceObjectId`,`attributeId`),
  CONSTRAINT `fk_Prop_Attribute_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_object_properties_attribute`
--

LOCK TABLES `ace_object_properties_attribute` WRITE;
/*!40000 ALTER TABLE `ace_object_properties_attribute` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_object_properties_attribute` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_object_properties_attribute2nd`
--

DROP TABLE IF EXISTS `ace_object_properties_attribute2nd`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_object_properties_attribute2nd` (
  `aceObjectId` int(10) unsigned NOT NULL,
  `attribute2ndId` smallint(4) unsigned NOT NULL,
  `attribute2ndValue` mediumint(7) unsigned NOT NULL DEFAULT '0',
  `attribute2ndRanks` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `attribute2ndXpSpent` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`aceObjectId`,`attribute2ndId`),
  UNIQUE KEY `ace_object__property_attribute2nd_id` (`aceObjectId`,`attribute2ndId`),
  CONSTRAINT `fk_Prop_Attribute2nd_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_object_properties_attribute2nd`
--

LOCK TABLES `ace_object_properties_attribute2nd` WRITE;
/*!40000 ALTER TABLE `ace_object_properties_attribute2nd` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_object_properties_attribute2nd` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_object_properties_bigint`
--

DROP TABLE IF EXISTS `ace_object_properties_bigint`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_object_properties_bigint` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT '0',
  `bigIntPropertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyIndex` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `propertyValue` bigint(20) unsigned NOT NULL DEFAULT '0',
  UNIQUE KEY `ace_object__property_bigint_id` (`aceObjectId`,`bigIntPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_BigInt_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_object_properties_bigint`
--

LOCK TABLES `ace_object_properties_bigint` WRITE;
/*!40000 ALTER TABLE `ace_object_properties_bigint` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_object_properties_bigint` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_object_properties_book`
--

DROP TABLE IF EXISTS `ace_object_properties_book`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_object_properties_book` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT '0',
  `page` int(10) unsigned NOT NULL DEFAULT '0',
  `authorName` varchar(255) NOT NULL,
  `authorAccount` varchar(255) NOT NULL,
  `authorId` int(10) unsigned NOT NULL DEFAULT '0',
  `ignoreAuthor` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `pageText` text NOT NULL,
  PRIMARY KEY (`aceObjectId`,`page`),
  UNIQUE KEY `ace_object__property_book_id` (`aceObjectId`,`page`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Book_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_object_properties_book`
--

LOCK TABLES `ace_object_properties_book` WRITE;
/*!40000 ALTER TABLE `ace_object_properties_book` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_object_properties_book` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_object_properties_bool`
--

DROP TABLE IF EXISTS `ace_object_properties_bool`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_object_properties_bool` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT '0',
  `boolPropertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyIndex` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `propertyValue` tinyint(1) NOT NULL DEFAULT '0',
  UNIQUE KEY `ace_object__property_bool_id` (`aceObjectId`,`boolPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Bool_Ace_object` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_object_properties_bool`
--

LOCK TABLES `ace_object_properties_bool` WRITE;
/*!40000 ALTER TABLE `ace_object_properties_bool` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_object_properties_bool` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_object_properties_did`
--

DROP TABLE IF EXISTS `ace_object_properties_did`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_object_properties_did` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT '0',
  `didPropertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyIndex` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `propertyValue` int(10) unsigned NOT NULL DEFAULT '0',
  UNIQUE KEY `ace_object__property_did_id` (`aceObjectId`,`didPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Did_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_object_properties_did`
--

LOCK TABLES `ace_object_properties_did` WRITE;
/*!40000 ALTER TABLE `ace_object_properties_did` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_object_properties_did` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_object_properties_double`
--

DROP TABLE IF EXISTS `ace_object_properties_double`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_object_properties_double` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT '0',
  `dblPropertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyIndex` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `propertyValue` double NOT NULL DEFAULT '0',
  UNIQUE KEY `ace_object__property_double_id` (`aceObjectId`,`dblPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Dbl_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_object_properties_double`
--

LOCK TABLES `ace_object_properties_double` WRITE;
/*!40000 ALTER TABLE `ace_object_properties_double` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_object_properties_double` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_object_properties_iid`
--

DROP TABLE IF EXISTS `ace_object_properties_iid`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_object_properties_iid` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT '0',
  `iidPropertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyIndex` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `propertyValue` int(10) unsigned NOT NULL DEFAULT '0',
  UNIQUE KEY `ace_object__property_iid_id` (`aceObjectId`,`iidPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  KEY `aopiid_propertyValue` (`propertyValue`),
  CONSTRAINT `fk_Prop_Iid_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_object_properties_iid`
--

LOCK TABLES `ace_object_properties_iid` WRITE;
/*!40000 ALTER TABLE `ace_object_properties_iid` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_object_properties_iid` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_object_properties_int`
--

DROP TABLE IF EXISTS `ace_object_properties_int`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_object_properties_int` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT '0',
  `intPropertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyIndex` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `propertyValue` int(10) NOT NULL DEFAULT '0',
  UNIQUE KEY `ace_object__property_int_id` (`aceObjectId`,`intPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Int_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_object_properties_int`
--

LOCK TABLES `ace_object_properties_int` WRITE;
/*!40000 ALTER TABLE `ace_object_properties_int` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_object_properties_int` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_object_properties_skill`
--

DROP TABLE IF EXISTS `ace_object_properties_skill`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_object_properties_skill` (
  `aceObjectId` int(10) unsigned NOT NULL,
  `skillId` tinyint(1) unsigned NOT NULL,
  `skillStatus` tinyint(1) unsigned NOT NULL,
  `skillPoints` smallint(2) unsigned NOT NULL DEFAULT '0',
  `skillXpSpent` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`aceObjectId`,`skillId`),
  CONSTRAINT `fk_Prop_Skill_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_object_properties_skill`
--

LOCK TABLES `ace_object_properties_skill` WRITE;
/*!40000 ALTER TABLE `ace_object_properties_skill` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_object_properties_skill` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_object_properties_spell`
--

DROP TABLE IF EXISTS `ace_object_properties_spell`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_object_properties_spell` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT '0',
  `spellId` int(10) unsigned NOT NULL DEFAULT '0',
  UNIQUE KEY `ace_object__property_spell_id` (`spellId`,`aceObjectId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Spell_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_object_properties_spell`
--

LOCK TABLES `ace_object_properties_spell` WRITE;
/*!40000 ALTER TABLE `ace_object_properties_spell` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_object_properties_spell` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_object_properties_spellbar_positions`
--

DROP TABLE IF EXISTS `ace_object_properties_spellbar_positions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_object_properties_spellbar_positions` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT '0',
  `spellId` int(10) unsigned NOT NULL DEFAULT '0',
  `spellBarId` int(10) unsigned NOT NULL DEFAULT '0',
  `spellBarPositionId` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`aceObjectId`,`spellId`,`spellBarId`),
  CONSTRAINT `fk_sb_ao` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_object_properties_spellbar_positions`
--

LOCK TABLES `ace_object_properties_spellbar_positions` WRITE;
/*!40000 ALTER TABLE `ace_object_properties_spellbar_positions` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_object_properties_spellbar_positions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_object_properties_string`
--

DROP TABLE IF EXISTS `ace_object_properties_string`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_object_properties_string` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT '0',
  `strPropertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyIndex` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `propertyValue` text NOT NULL,
  UNIQUE KEY `ace_object__property_string_id` (`aceObjectId`,`strPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Str_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_object_properties_string`
--

LOCK TABLES `ace_object_properties_string` WRITE;
/*!40000 ALTER TABLE `ace_object_properties_string` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_object_properties_string` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_object_texture_map_change`
--

DROP TABLE IF EXISTS `ace_object_texture_map_change`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_object_texture_map_change` (
  `aceObjectId` int(10) unsigned NOT NULL,
  `index` tinyint(3) unsigned NOT NULL,
  `oldId` int(10) unsigned NOT NULL,
  `newId` int(10) unsigned NOT NULL,
  PRIMARY KEY (`aceObjectId`,`index`,`oldId`),
  CONSTRAINT `FK_ace_object_texture_map_changes__baseAceObjectId` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_object_texture_map_change`
--

LOCK TABLES `ace_object_texture_map_change` WRITE;
/*!40000 ALTER TABLE `ace_object_texture_map_change` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_object_texture_map_change` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_position`
--

DROP TABLE IF EXISTS `ace_position`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_position` (
  `positionId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `aceObjectId` int(10) unsigned NOT NULL,
  `positionType` smallint(5) unsigned NOT NULL,
  `landblockRaw` int(10) unsigned NOT NULL,
  `landblock` int(5) unsigned GENERATED ALWAYS AS ((`landblockRaw` >> 16)) VIRTUAL,
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  `qW` float NOT NULL,
  `qX` float NOT NULL,
  `qY` float NOT NULL,
  `qZ` float NOT NULL,
  PRIMARY KEY (`positionId`),
  KEY `idx_aceObjectId` (`aceObjectId`),
  KEY `idxPostionType` (`positionType`),
  KEY `idx_landblock_raw` (`landblockRaw`),
  KEY `idx_landblock` (`landblock`),
  CONSTRAINT `fk_position_ao` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_position`
--

LOCK TABLES `ace_position` WRITE;
/*!40000 ALTER TABLE `ace_position` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_position` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_weenie_class`
--

DROP TABLE IF EXISTS `ace_weenie_class`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_weenie_class` (
  `weenieClassId` int(10) unsigned NOT NULL,
  `weenieClassDescription` text NOT NULL,
  PRIMARY KEY (`weenieClassId`),
  UNIQUE KEY `idx_weenieName` (`weenieClassDescription`(100))
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_weenie_class`
--

LOCK TABLES `ace_weenie_class` WRITE;
/*!40000 ALTER TABLE `ace_weenie_class` DISABLE KEYS */;
INSERT INTO `ace_weenie_class` VALUES (1,'human'),(4,'admin'),(1149,'allegiance');
/*!40000 ALTER TABLE `ace_weenie_class` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `vw_ace_character`
--

DROP TABLE IF EXISTS `vw_ace_character`;
/*!50001 DROP VIEW IF EXISTS `vw_ace_character`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `vw_ace_character` AS SELECT 
 1 AS `guid`,
 1 AS `subscriptionId`,
 1 AS `NAME`,
 1 AS `deleted`,
 1 AS `deleteTime`,
 1 AS `weenieClassId`,
 1 AS `weenieClassDescription`,
 1 AS `aceObjectDescriptionFlags`,
 1 AS `physicsDescriptionFlag`,
 1 AS `weenieHeaderFlags`,
 1 AS `itemType`,
 1 AS `loginTimestamp`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `vw_ace_inventory_object`
--

DROP TABLE IF EXISTS `vw_ace_inventory_object`;
/*!50001 DROP VIEW IF EXISTS `vw_ace_inventory_object`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `vw_ace_inventory_object` AS SELECT 
 1 AS `containerId`,
 1 AS `aceObjectId`,
 1 AS `placement`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `vw_ace_object`
--

DROP TABLE IF EXISTS `vw_ace_object`;
/*!50001 DROP VIEW IF EXISTS `vw_ace_object`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `vw_ace_object` AS SELECT 
 1 AS `aceObjectId`,
 1 AS `name`,
 1 AS `weenieClassId`,
 1 AS `currentMotionState`,
 1 AS `weenieClassDescription`,
 1 AS `aceObjectDescriptionFlags`,
 1 AS `physicsDescriptionFlag`,
 1 AS `weenieHeaderFlags`,
 1 AS `itemType`,
 1 AS `positionId`,
 1 AS `positionType`,
 1 AS `LandblockRaw`,
 1 AS `landblock`,
 1 AS `posX`,
 1 AS `posY`,
 1 AS `posZ`,
 1 AS `qW`,
 1 AS `qX`,
 1 AS `qY`,
 1 AS `qZ`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `vw_ace_wielded_object`
--

DROP TABLE IF EXISTS `vw_ace_wielded_object`;
/*!50001 DROP VIEW IF EXISTS `vw_ace_wielded_object`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `vw_ace_wielded_object` AS SELECT 
 1 AS `wielderId`,
 1 AS `aceObjectId`,
 1 AS `wieldedLocation`*/;
SET character_set_client = @saved_cs_client;

--
-- Final view structure for view `vw_ace_character`
--

/*!50001 DROP VIEW IF EXISTS `vw_ace_character`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `vw_ace_character` AS select `ao`.`aceObjectId` AS `guid`,`aopiidacc`.`propertyValue` AS `subscriptionId`,`aops`.`propertyValue` AS `NAME`,`aopb`.`propertyValue` AS `deleted`,`aopbi`.`propertyValue` AS `deleteTime`,`ao`.`weenieClassId` AS `weenieClassId`,`awc`.`weenieClassDescription` AS `weenieClassDescription`,`ao`.`aceObjectDescriptionFlags` AS `aceObjectDescriptionFlags`,`ao`.`physicsDescriptionFlag` AS `physicsDescriptionFlag`,`ao`.`weenieHeaderFlags` AS `weenieHeaderFlags`,`aopi`.`propertyValue` AS `itemType`,`aopd`.`propertyValue` AS `loginTimestamp` from (((((((`ace_object` `ao` join `ace_weenie_class` `awc` on((`ao`.`weenieClassId` = `awc`.`weenieClassId`))) join `ace_object_properties_string` `aops` on(((`ao`.`aceObjectId` = `aops`.`aceObjectId`) and (`aops`.`strPropertyId` = 1)))) join `ace_object_properties_bool` `aopb` on(((`ao`.`aceObjectId` = `aopb`.`aceObjectId`) and (`aopb`.`boolPropertyId` = 9001)))) join `ace_object_properties_int` `aopi` on(((`ao`.`aceObjectId` = `aopi`.`aceObjectId`) and (`aopi`.`intPropertyId` = 1)))) join `ace_object_properties_bigint` `aopbi` on(((`ao`.`aceObjectId` = `aopbi`.`aceObjectId`) and (`aopbi`.`bigIntPropertyId` = 9001)))) join `ace_object_properties_iid` `aopiidacc` on(((`ao`.`aceObjectId` = `aopiidacc`.`aceObjectId`) and (`aopiidacc`.`iidPropertyId` = 9001)))) left join `ace_object_properties_double` `aopd` on(((`ao`.`aceObjectId` = `aopd`.`aceObjectId`) and (`aopd`.`dblPropertyId` = 48)))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `vw_ace_inventory_object`
--

/*!50001 DROP VIEW IF EXISTS `vw_ace_inventory_object`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `vw_ace_inventory_object` AS (select `aopiid`.`propertyValue` AS `containerId`,`aopiid`.`aceObjectId` AS `aceObjectId`,`aopi`.`propertyValue` AS `placement` from (`ace_object_properties_iid` `aopiid` join `ace_object_properties_int` `aopi` on(((`aopiid`.`aceObjectId` = `aopi`.`aceObjectId`) and (`aopi`.`intPropertyId` = 65)))) where (`aopiid`.`iidPropertyId` = 2) order by `aopiid`.`propertyValue`,`aopi`.`propertyValue`) */;
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
/*!50001 VIEW `vw_ace_object` AS select `ao`.`aceObjectId` AS `aceObjectId`,`aops`.`propertyValue` AS `name`,`ao`.`weenieClassId` AS `weenieClassId`,`ao`.`currentMotionState` AS `currentMotionState`,`awc`.`weenieClassDescription` AS `weenieClassDescription`,`ao`.`aceObjectDescriptionFlags` AS `aceObjectDescriptionFlags`,`ao`.`physicsDescriptionFlag` AS `physicsDescriptionFlag`,`ao`.`weenieHeaderFlags` AS `weenieHeaderFlags`,`aopi`.`propertyValue` AS `itemType`,`ap`.`positionId` AS `positionId`,`ap`.`positionType` AS `positionType`,`ap`.`landblockRaw` AS `LandblockRaw`,`ap`.`landblock` AS `landblock`,`ap`.`posX` AS `posX`,`ap`.`posY` AS `posY`,`ap`.`posZ` AS `posZ`,`ap`.`qW` AS `qW`,`ap`.`qX` AS `qX`,`ap`.`qY` AS `qY`,`ap`.`qZ` AS `qZ` from ((((`ace_object` `ao` join `ace_weenie_class` `awc` on((`ao`.`weenieClassId` = `awc`.`weenieClassId`))) join `ace_object_properties_string` `aops` on(((`ao`.`aceObjectId` = `aops`.`aceObjectId`) and (`aops`.`strPropertyId` = 1)))) join `ace_object_properties_int` `aopi` on(((`ao`.`aceObjectId` = `aopi`.`aceObjectId`) and (`aopi`.`intPropertyId` = 1)))) join `ace_position` `ap` on(((`ao`.`aceObjectId` = `ap`.`aceObjectId`) and (`ap`.`positionType` = 1)))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `vw_ace_wielded_object`
--

/*!50001 DROP VIEW IF EXISTS `vw_ace_wielded_object`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `vw_ace_wielded_object` AS (select `aopiid`.`propertyValue` AS `wielderId`,`aopiid`.`aceObjectId` AS `aceObjectId`,`aopi`.`propertyValue` AS `wieldedLocation` from (`ace_object_properties_iid` `aopiid` join `ace_object_properties_int` `aopi` on(((`aopiid`.`aceObjectId` = `aopi`.`aceObjectId`) and (`aopi`.`intPropertyId` = 10)))) where (`aopiid`.`iidPropertyId` = 3) order by `aopiid`.`propertyValue`,`aopi`.`propertyValue`) */;
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

-- Dump completed on 2017-10-29 16:05:06
