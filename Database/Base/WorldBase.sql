CREATE DATABASE  IF NOT EXISTS `ace_world` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `ace_world`;
-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: localhost    Database: ace_world
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
-- Table structure for table `ace_content`
--

DROP TABLE IF EXISTS `ace_content`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_content` (
  `contentGuid` binary(16) NOT NULL,
  `contentName` text NOT NULL,
  `contentType` int(3) unsigned DEFAULT NULL COMMENT 'ACE.Entity.Enum.ContentType',
  `userModified` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`contentGuid`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_content`
--

LOCK TABLES `ace_content` WRITE;
/*!40000 ALTER TABLE `ace_content` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_content` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_content_landblock`
--

DROP TABLE IF EXISTS `ace_content_landblock`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_content_landblock` (
  `contentLandblockGuid` binary(16) NOT NULL,
  `contentGuid` binary(16) NOT NULL,
  `landblockId` int(10) unsigned NOT NULL COMMENT '0x####0000.  lower word should be all 0s.',
  `comment` text,
  PRIMARY KEY (`contentLandblockGuid`),
  KEY `contentGuid` (`contentGuid`,`landblockId`),
  CONSTRAINT `ace_content_landblock_ibfk_1` FOREIGN KEY (`contentGuid`) REFERENCES `ace_content` (`contentGuid`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_content_landblock`
--

LOCK TABLES `ace_content_landblock` WRITE;
/*!40000 ALTER TABLE `ace_content_landblock` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_content_landblock` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_content_link`
--

DROP TABLE IF EXISTS `ace_content_link`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_content_link` (
  `contentGuid1` binary(16) NOT NULL,
  `contentGuid2` binary(16) NOT NULL,
  PRIMARY KEY (`contentGuid1`,`contentGuid2`),
  KEY `ace_content_link_ibfk_2` (`contentGuid2`),
  CONSTRAINT `ace_content_link_ibfk_1` FOREIGN KEY (`contentGuid1`) REFERENCES `ace_content` (`contentGuid`) ON DELETE CASCADE,
  CONSTRAINT `ace_content_link_ibfk_2` FOREIGN KEY (`contentGuid2`) REFERENCES `ace_content` (`contentGuid`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_content_link`
--

LOCK TABLES `ace_content_link` WRITE;
/*!40000 ALTER TABLE `ace_content_link` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_content_link` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_content_resource`
--

DROP TABLE IF EXISTS `ace_content_resource`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_content_resource` (
  `contentResourceGuid` binary(16) NOT NULL,
  `contentGuid` binary(16) NOT NULL,
  `name` text NOT NULL,
  `resourceUri` text NOT NULL,
  `comment` text,
  PRIMARY KEY (`contentResourceGuid`),
  KEY `ace_content_resource_ibfk_1` (`contentGuid`),
  CONSTRAINT `ace_content_resource_ibfk_1` FOREIGN KEY (`contentGuid`) REFERENCES `ace_content` (`contentGuid`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_content_resource`
--

LOCK TABLES `ace_content_resource` WRITE;
/*!40000 ALTER TABLE `ace_content_resource` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_content_resource` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_content_weenie`
--

DROP TABLE IF EXISTS `ace_content_weenie`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_content_weenie` (
  `contentWeenieGuid` binary(16) NOT NULL,
  `contentGuid` binary(16) NOT NULL,
  `weenieId` int(10) unsigned NOT NULL,
  `comment` text,
  PRIMARY KEY (`contentWeenieGuid`),
  KEY `ace_content_weenie_ibfk_1` (`contentGuid`),
  KEY `ace_content_weenie_ibfk_2` (`weenieId`),
  CONSTRAINT `ace_content_weenie_ibfk_1` FOREIGN KEY (`contentGuid`) REFERENCES `ace_content` (`contentGuid`) ON DELETE CASCADE,
  CONSTRAINT `ace_content_weenie_ibfk_2` FOREIGN KEY (`weenieId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_content_weenie`
--

LOCK TABLES `ace_content_weenie` WRITE;
/*!40000 ALTER TABLE `ace_content_weenie` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_content_weenie` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_landblock`
--

DROP TABLE IF EXISTS `ace_landblock`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_landblock` (
  `instanceId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `landblock` int(5) GENERATED ALWAYS AS ((`landblockRaw` >> 16)) VIRTUAL,
  `weenieClassId` int(10) unsigned NOT NULL,
  `preassignedGuid` int(10) unsigned DEFAULT NULL,
  `landblockRaw` int(10) unsigned NOT NULL,
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  `qW` float NOT NULL,
  `qX` float NOT NULL,
  `qY` float NOT NULL,
  `qZ` float NOT NULL,
  PRIMARY KEY (`instanceId`),
  UNIQUE KEY `instanceId_UNIQUE` (`instanceId`),
  UNIQUE KEY `preassignedGuid_UNIQUE` (`preassignedGuid`),
  KEY `fk_lb_weenie_idx` (`weenieClassId`),
  KEY `fk_lb_idx` (`landblock`),
  CONSTRAINT `fk_weenie_lb` FOREIGN KEY (`weenieClassId`) REFERENCES `ace_weenie_class` (`weenieClassId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_landblock`
--

LOCK TABLES `ace_landblock` WRITE;
/*!40000 ALTER TABLE `ace_landblock` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_landblock` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_object`
--

DROP TABLE IF EXISTS `ace_object`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_object` (
  `aceObjectId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `aceObjectDescriptionFlags` int(10) unsigned NOT NULL,
  `weenieClassId` int(10) unsigned NOT NULL,
  `userModified` tinyint(1) NOT NULL DEFAULT '0' COMMENT 'flag indicating whether or not this has record has been altered since deployment',
  `weenieHeaderFlags` int(10) unsigned DEFAULT NULL,
  `weenieHeaderFlags2` int(10) unsigned DEFAULT NULL,
  `physicsDescriptionFlag` int(10) unsigned DEFAULT NULL,
  `currentMotionState` text,
  PRIMARY KEY (`aceObjectId`),
  KEY `idx_weenie` (`weenieClassId`),
  CONSTRAINT `fk_weenie_ao` FOREIGN KEY (`weenieClassId`) REFERENCES `ace_weenie_class` (`weenieClassId`) ON DELETE CASCADE
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
-- Table structure for table `ace_object_generator_link`
--

DROP TABLE IF EXISTS `ace_object_generator_link`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_object_generator_link`
--

LOCK TABLES `ace_object_generator_link` WRITE;
/*!40000 ALTER TABLE `ace_object_generator_link` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_object_generator_link` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_object_inventory`
--

DROP TABLE IF EXISTS `ace_object_inventory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_object_inventory` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT '0',
  `destinationType` tinyint(5) NOT NULL DEFAULT '0',
  `weenieClassId` int(10) unsigned NOT NULL DEFAULT '0',
  `stackSize` int(10) NOT NULL DEFAULT '1',
  `palette` tinyint(5) NOT NULL DEFAULT '0',
  KEY `fk_Inventory_AceObject_idx` (`aceObjectId`),
  KEY `fk_Inventory_Weenie_idx` (`weenieClassId`),
  CONSTRAINT `fk_Inventory_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE,
  CONSTRAINT `fk_Inventory_Weenie` FOREIGN KEY (`weenieClassId`) REFERENCES `ace_weenie_class` (`weenieClassId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_object_inventory`
--

LOCK TABLES `ace_object_inventory` WRITE;
/*!40000 ALTER TABLE `ace_object_inventory` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_object_inventory` ENABLE KEYS */;
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
-- Table structure for table `ace_poi`
--

DROP TABLE IF EXISTS `ace_poi`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_poi` (
  `name` text NOT NULL,
  `weenieClassId` int(10) unsigned NOT NULL,
  PRIMARY KEY (`name`(100)),
  KEY `fk_poi_weenie_ao_idx` (`weenieClassId`),
  CONSTRAINT `fk_poi_weenie_ao` FOREIGN KEY (`weenieClassId`) REFERENCES `ace_weenie_class` (`weenieClassId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_poi`
--

LOCK TABLES `ace_poi` WRITE;
/*!40000 ALTER TABLE `ace_poi` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_poi` ENABLE KEYS */;
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
-- Table structure for table `ace_recipe`
--

DROP TABLE IF EXISTS `ace_recipe`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_recipe` (
  `recipeGuid` binary(16) NOT NULL COMMENT 'surrogate key',
  `recipeType` tinyint(3) unsigned NOT NULL COMMENT 'see RecipeType enum in code',
  `userModified` tinyint(1) NOT NULL DEFAULT '0' COMMENT 'flag indicating whether or not this has record has been altered since deployment',
  `sourceWcid` int(10) unsigned NOT NULL COMMENT 'the object being used',
  `targetWcid` int(10) unsigned NOT NULL COMMENT 'the target of use',
  `skillId` smallint(6) unsigned DEFAULT NULL COMMENT 'skill required for the formula, if any',
  `skillDifficulty` smallint(6) unsigned DEFAULT NULL COMMENT 'skill value required for 50% success',
  `partialFailDifficulty` smallint(6) unsigned DEFAULT NULL COMMENT 'skill value for a partial botch (dyed clothing)',
  `successMessage` text,
  `failMessage` text,
  `alternateMessage` text,
  `resultFlags` int(10) unsigned DEFAULT NULL COMMENT 'bitmask of what happens.  see RecipeResults enum in code',
  `successItem1Wcid` int(10) unsigned DEFAULT NULL,
  `successItem2Wcid` int(10) unsigned DEFAULT NULL,
  `failureItem1Wcid` int(10) unsigned DEFAULT NULL,
  `failureItem2Wcid` int(10) unsigned DEFAULT NULL,
  `healingAttribute` smallint(6) unsigned DEFAULT NULL COMMENT 'used by recipeType = Healing. health = 64, stam = 128, mana = 256. if null, will default to health. source enum: ACE.Entity.Enum.Ability',
  PRIMARY KEY (`recipeGuid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ace_recipe`
--

LOCK TABLES `ace_recipe` WRITE;
/*!40000 ALTER TABLE `ace_recipe` DISABLE KEYS */;
/*!40000 ALTER TABLE `ace_recipe` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ace_weenie_class`
--

DROP TABLE IF EXISTS `ace_weenie_class`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_weenie_class` (
  `weenieClassId` int(10) unsigned NOT NULL AUTO_INCREMENT,
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
/*!40000 ALTER TABLE `ace_weenie_class` ENABLE KEYS */;
UNLOCK TABLES;

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
-- Temporary view structure for view `vw_ace_weenie_class`
--

DROP TABLE IF EXISTS `vw_ace_weenie_class`;
/*!50001 DROP VIEW IF EXISTS `vw_ace_weenie_class`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `vw_ace_weenie_class` AS SELECT 
 1 AS `aceObjectId`,
 1 AS `name`,
 1 AS `weenieClassId`,
 1 AS `weenieClassDescription`,
 1 AS `itemType`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `vw_teleport_location`
--

DROP TABLE IF EXISTS `vw_teleport_location`;
/*!50001 DROP VIEW IF EXISTS `vw_teleport_location`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `vw_teleport_location` AS SELECT 
 1 AS `name`,
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
-- Temporary view structure for view `vw_weenie_search`
--

DROP TABLE IF EXISTS `vw_weenie_search`;
/*!50001 DROP VIEW IF EXISTS `vw_weenie_search`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `vw_weenie_search` AS SELECT 
 1 AS `aceObjectId`,
 1 AS `userModified`,
 1 AS `weenieClassId`,
 1 AS `weenieClassDescription`,
 1 AS `name`,
 1 AS `itemType`,
 1 AS `weenieType`*/;
SET character_set_client = @saved_cs_client;

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
-- Final view structure for view `vw_ace_weenie_class`
--

/*!50001 DROP VIEW IF EXISTS `vw_ace_weenie_class`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `vw_ace_weenie_class` AS select `ao`.`aceObjectId` AS `aceObjectId`,`aops`.`propertyValue` AS `name`,`ao`.`weenieClassId` AS `weenieClassId`,`awc`.`weenieClassDescription` AS `weenieClassDescription`,`aopi`.`propertyValue` AS `itemType` from ((((`ace_object` `ao` join `ace_weenie_class` `awc` on((`ao`.`weenieClassId` = `awc`.`weenieClassId`))) join `ace_object_properties_string` `aops` on(((`ao`.`aceObjectId` = `aops`.`aceObjectId`) and (`aops`.`strPropertyId` = 1)))) join `ace_object_properties_int` `aopi` on(((`ao`.`aceObjectId` = `aopi`.`aceObjectId`) and (`aopi`.`intPropertyId` = 1)))) left join `ace_position` `ap` on(((`ao`.`aceObjectId` = `ap`.`aceObjectId`) and (`ap`.`positionType` = 1)))) where isnull(`ap`.`aceObjectId`) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `vw_teleport_location`
--

/*!50001 DROP VIEW IF EXISTS `vw_teleport_location`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `vw_teleport_location` AS (select `apoi`.`name` AS `name`,`ap`.`landblockRaw` AS `landblock`,`ap`.`posX` AS `posX`,`ap`.`posY` AS `posY`,`ap`.`posZ` AS `posZ`,`ap`.`qW` AS `qW`,`ap`.`qX` AS `qX`,`ap`.`qY` AS `qY`,`ap`.`qZ` AS `qZ` from (`ace_poi` `apoi` join `ace_position` `ap` on((`apoi`.`weenieClassId` = `ap`.`aceObjectId`))) where (`ap`.`positionType` = 2)) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `vw_weenie_search`
--

/*!50001 DROP VIEW IF EXISTS `vw_weenie_search`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `vw_weenie_search` AS (select `ao`.`aceObjectId` AS `aceObjectId`,`ao`.`userModified` AS `userModified`,`ao`.`weenieClassId` AS `weenieClassId`,`wc`.`weenieClassDescription` AS `weenieClassDescription`,`names`.`propertyValue` AS `name`,`itemtype`.`propertyValue` AS `itemType`,`weenietype`.`propertyValue` AS `weenieType` from ((((`ace_object` `ao` left join `ace_weenie_class` `wc` on((`ao`.`aceObjectId` = `wc`.`weenieClassId`))) left join `ace_object_properties_string` `names` on(((`ao`.`aceObjectId` = `names`.`aceObjectId`) and (`names`.`strPropertyId` = 1)))) left join `ace_object_properties_int` `weenietype` on(((`ao`.`aceObjectId` = `weenietype`.`aceObjectId`) and (`weenietype`.`intPropertyId` = 9007)))) left join `ace_object_properties_int` `itemtype` on(((`ao`.`aceObjectId` = `itemtype`.`aceObjectId`) and (`itemtype`.`intPropertyId` = 1)))) where (`ao`.`aceObjectId` = `ao`.`weenieClassId`)) */;
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

-- Dump completed on 2017-10-29 16:05:24
