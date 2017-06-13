/*
SQLyog Ultimate v12.4.2 (64 bit)
MySQL - 10.2.6-MariaDB : Database - ace_world
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`ace_world` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `ace_world`;

/*Table structure for table `ace_object` */

DROP TABLE IF EXISTS `ace_object`;

CREATE TABLE `ace_object` (
  `aceObjectId` int(10) unsigned NOT NULL,
  `aceObjectDescriptionFlags` int(10) unsigned NOT NULL,
  `weenieClassId` int(10) unsigned NOT NULL,
  `weenieHeaderFlags` int(10) unsigned NOT NULL,
  `physicsDescriptionFlag` int(10) unsigned NOT NULL,
  `currentMotionState` text DEFAULT NULL,
  PRIMARY KEY (`aceObjectId`),
  KEY `idx_weenie` (`weenieClassId`),
  CONSTRAINT `fk_weenie_ao` FOREIGN KEY (`weenieClassId`) REFERENCES `ace_weenie_class` (`weenieClassId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_animation_change` */

DROP TABLE IF EXISTS `ace_object_animation_change`;

CREATE TABLE `ace_object_animation_change` (
  `aceObjectId` int(10) unsigned NOT NULL,
  `index` tinyint(3) unsigned NOT NULL,
  `animationId` int(10) unsigned NOT NULL,
  PRIMARY KEY (`aceObjectId`,`index`),
  CONSTRAINT `FK_ace_object_animation_changes__baseAceObjectId` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_palette_change` */

DROP TABLE IF EXISTS `ace_object_palette_change`;

CREATE TABLE `ace_object_palette_change` (
  `aceObjectId` int(10) unsigned NOT NULL,
  `subPaletteId` int(10) unsigned NOT NULL,
  `offset` smallint(5) unsigned NOT NULL,
  `length` smallint(5) unsigned zerofill NOT NULL,
  PRIMARY KEY (`aceObjectId`,`subPaletteId`,`offset`,`length`),
  CONSTRAINT `FK_ace_object_palette_data__baseAceObjectId` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_attribute` */

DROP TABLE IF EXISTS `ace_object_properties_attribute`;

CREATE TABLE `ace_object_properties_attribute` (
  `aceObjectId` int(10) unsigned NOT NULL,
  `attributeId` smallint(4) unsigned NOT NULL,
  `attributeBase` smallint(4) unsigned NOT NULL DEFAULT 0,
  `attributeRanks` tinyint(2) unsigned NOT NULL DEFAULT 0,
  `attributeXpSpent` int(10) unsigned NOT NULL DEFAULT 0,
  PRIMARY KEY (`aceObjectId`,`attributeId`),
  UNIQUE KEY `ace_object__property_attribute_id` (`aceObjectId`,`attributeId`),
  CONSTRAINT `fk_Prop_Attribute_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_attribute2nd` */

DROP TABLE IF EXISTS `ace_object_properties_attribute2nd`;

CREATE TABLE `ace_object_properties_attribute2nd` (
  `aceObjectId` int(10) unsigned NOT NULL,
  `attribute2ndId` smallint(4) unsigned NOT NULL,
  `attribute2ndValue` mediumint(7) unsigned NOT NULL DEFAULT 0,
  `attribute2ndRanks` tinyint(2) unsigned NOT NULL DEFAULT 0,
  `attribute2ndXpSpent` int(10) unsigned NOT NULL DEFAULT 0,
  PRIMARY KEY (`aceObjectId`,`attribute2ndId`),
  UNIQUE KEY `ace_object__property_attribute2nd_id` (`aceObjectId`,`attribute2ndId`),
  CONSTRAINT `fk_Prop_Attribute2nd_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_bigint` */

DROP TABLE IF EXISTS `ace_object_properties_bigint`;

CREATE TABLE `ace_object_properties_bigint` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT 0,
  `bigIntPropertyId` smallint(5) unsigned NOT NULL DEFAULT 0,
  `propertyValue` bigint(20) unsigned NOT NULL DEFAULT 0,
  UNIQUE KEY `ace_object__property_bigint_id` (`aceObjectId`,`bigIntPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_BigInt_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_bool` */

DROP TABLE IF EXISTS `ace_object_properties_bool`;

CREATE TABLE `ace_object_properties_bool` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT 0,
  `boolPropertyId` smallint(5) unsigned NOT NULL DEFAULT 0,
  `propertyValue` tinyint(1) NOT NULL DEFAULT 0,
  UNIQUE KEY `ace_object__property_bool_id` (`aceObjectId`,`boolPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Bool_Ace_object` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_did` */

DROP TABLE IF EXISTS `ace_object_properties_did`;

CREATE TABLE `ace_object_properties_did` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT 0,
  `didPropertyId` smallint(5) unsigned NOT NULL DEFAULT 0,
  `propertyValue` int(10) unsigned NOT NULL DEFAULT 0,
  UNIQUE KEY `ace_object__property_did_id` (`aceObjectId`,`didPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Did_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_double` */

DROP TABLE IF EXISTS `ace_object_properties_double`;

CREATE TABLE `ace_object_properties_double` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT 0,
  `dblPropertyId` smallint(5) unsigned NOT NULL DEFAULT 0,
  `propertyValue` double NOT NULL DEFAULT 0,
  UNIQUE KEY `ace_object__property_double_id` (`aceObjectId`,`dblPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Dbl_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_iid` */

DROP TABLE IF EXISTS `ace_object_properties_iid`;

CREATE TABLE `ace_object_properties_iid` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT 0,
  `iidPropertyId` smallint(5) unsigned NOT NULL DEFAULT 0,
  `propertyValue` int(10) unsigned NOT NULL DEFAULT 0,
  UNIQUE KEY `ace_object__property_iid_id` (`aceObjectId`,`iidPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Iid_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_int` */

DROP TABLE IF EXISTS `ace_object_properties_int`;

CREATE TABLE `ace_object_properties_int` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT 0,
  `intPropertyId` smallint(5) unsigned NOT NULL DEFAULT 0,
  `propertyValue` int(10) unsigned NOT NULL DEFAULT 0,
  UNIQUE KEY `ace_object__property_int_id` (`aceObjectId`,`intPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Int_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_spell` */

DROP TABLE IF EXISTS `ace_object_properties_spell`;

CREATE TABLE `ace_object_properties_spell` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT 0,
  `spellId` int(10) unsigned NOT NULL DEFAULT 0,
  UNIQUE KEY `ace_object__property_spell_id` (`spellId`,`aceObjectId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Spell_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_string` */

DROP TABLE IF EXISTS `ace_object_properties_string`;

CREATE TABLE `ace_object_properties_string` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT 0,
  `strPropertyId` smallint(5) unsigned NOT NULL DEFAULT 0,
  `propertyValue` text NOT NULL,
  UNIQUE KEY `ace_object__property_string_id` (`aceObjectId`,`strPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Str_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_texture_map_change` */

DROP TABLE IF EXISTS `ace_object_texture_map_change`;

CREATE TABLE `ace_object_texture_map_change` (
  `aceObjectId` int(10) unsigned NOT NULL,
  `index` tinyint(3) unsigned NOT NULL,
  `oldId` int(10) unsigned NOT NULL,
  `newId` int(10) unsigned NOT NULL,
  PRIMARY KEY (`aceObjectId`,`index`,`oldId`),
  CONSTRAINT `FK_ace_object_texture_map_changes__baseAceObjectId` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_poi` */

DROP TABLE IF EXISTS `ace_poi`;

CREATE TABLE `ace_poi` (
  `name` text NOT NULL,
  `positionId` int(10) unsigned NOT NULL,
  PRIMARY KEY (`name`(100)),
  UNIQUE KEY `idx_poi` (`positionId`),
  CONSTRAINT `fk_poi_position` FOREIGN KEY (`positionId`) REFERENCES `ace_position` (`positionId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_portal_object` */

DROP TABLE IF EXISTS `ace_portal_object`;

CREATE TABLE `ace_portal_object` (
  `aceObjectId` int(10) unsigned NOT NULL,
  `positionId` int(10) unsigned NOT NULL,
  `min_lvl` int(11) unsigned NOT NULL DEFAULT 0,
  `max_lvl` int(11) unsigned NOT NULL DEFAULT 0,
  `societyId` tinyint(3) unsigned NOT NULL DEFAULT 0,
  `isTieable` tinyint(1) unsigned NOT NULL DEFAULT 1,
  `isRecallable` tinyint(1) unsigned NOT NULL DEFAULT 1,
  `isSummonable` tinyint(1) unsigned NOT NULL DEFAULT 1,
  PRIMARY KEY (`aceObjectId`),
  KEY `FK_apo2po` (`positionId`),
  CONSTRAINT `FK_apo2ao` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`),
  CONSTRAINT `FK_apo2po` FOREIGN KEY (`positionId`) REFERENCES `ace_position` (`positionId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_position` */

DROP TABLE IF EXISTS `ace_position`;

CREATE TABLE `ace_position` (
  `positionId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `aceObjectId` int(10) unsigned DEFAULT NULL,
  `positionType` smallint(5) unsigned NOT NULL,
  `landblockRaw` int(10) unsigned NOT NULL,
  `landblock` smallint(5) unsigned GENERATED ALWAYS AS (conv(left(hex(`landblockRaw`),4),16,10)) STORED,
  `cell` smallint(5) unsigned GENERATED ALWAYS AS (conv(lpad(substr(hex(`landblockRaw`),5,4),4,'0'),16,10)) STORED,
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  `qW` float NOT NULL,
  `qX` float NOT NULL,
  `qY` float NOT NULL,
  `qZ` float NOT NULL,
  PRIMARY KEY (`positionId`),
  KEY `idx_aceObjectId` (`aceObjectId`),
  KEY `idx_landblock` (`landblockRaw`),
  KEY `idxPostionType` (`positionType`),
  CONSTRAINT `fk_ap_ao` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`),
  CONSTRAINT `fk_position_ao` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=178436 DEFAULT CHARSET=utf8;

/*Table structure for table `ace_weenie_class` */

DROP TABLE IF EXISTS `ace_weenie_class`;

CREATE TABLE `ace_weenie_class` (
  `weenieClassId` int(10) unsigned NOT NULL,
  `weenieClassDescription` text NOT NULL,
  PRIMARY KEY (`weenieClassId`),
  UNIQUE KEY `idx_weenieName` (`weenieClassDescription`(100))
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `vw_ace_object` */

DROP TABLE IF EXISTS `vw_ace_object`;

/*!50001 DROP VIEW IF EXISTS `vw_ace_object` */;
/*!50001 DROP TABLE IF EXISTS `vw_ace_object` */;

/*!50001 CREATE TABLE  `vw_ace_object`(
 `aceObjectId` int(10) unsigned ,
 `name` text ,
 `weenieClassId` int(10) unsigned ,
 `currentMotionState` text ,
 `weenieClassDescription` text ,
 `aceObjectDescriptionFlags` int(10) unsigned ,
 `physicsDescriptionFlag` int(10) unsigned ,
 `weenieHeaderFlags` int(10) unsigned ,
 `itemType` int(10) unsigned ,
 `positionId` int(10) unsigned ,
 `positionType` smallint(5) unsigned ,
 `LandblockRaw` int(10) unsigned ,
 `landblock` smallint(5) unsigned ,
 `cell` smallint(5) unsigned ,
 `posX` float ,
 `posY` float ,
 `posZ` float ,
 `qW` float ,
 `qX` float ,
 `qY` float ,
 `qZ` float 
)*/;

/*Table structure for table `vw_ace_portal_object` */

DROP TABLE IF EXISTS `vw_ace_portal_object`;

/*!50001 DROP VIEW IF EXISTS `vw_ace_portal_object` */;
/*!50001 DROP TABLE IF EXISTS `vw_ace_portal_object` */;

/*!50001 CREATE TABLE  `vw_ace_portal_object`(
 `aceObjectId` int(10) unsigned ,
 `name` text ,
 `weenieClassId` int(10) unsigned ,
 `weenieClassDescription` text ,
 `aceObjectDescriptionFlags` int(10) unsigned ,
 `physicsDescriptionFlag` int(10) unsigned ,
 `weenieHeaderFlags` int(10) unsigned ,
 `itemType` int(10) unsigned ,
 `positionId` int(10) unsigned ,
 `positionType` smallint(5) unsigned ,
 `landblockRaw` int(10) unsigned ,
 `landblock` smallint(5) unsigned ,
 `currentMotionState` text ,
 `cell` smallint(5) unsigned ,
 `posX` float ,
 `posY` float ,
 `posZ` float ,
 `qW` float ,
 `qX` float ,
 `qY` float ,
 `qZ` float ,
 `min_lvl` int(11) unsigned ,
 `max_lvl` int(11) unsigned ,
 `societyId` tinyint(3) unsigned ,
 `isTieable` tinyint(1) unsigned ,
 `isRecallable` tinyint(1) unsigned ,
 `isSummonable` tinyint(1) unsigned ,
 `destPositionId` int(10) unsigned ,
 `destLandblockId` int(10) unsigned ,
 `destLandblock` smallint(5) unsigned ,
 `destCell` smallint(5) unsigned ,
 `destX` float ,
 `destY` float ,
 `destZ` float ,
 `destQW` float ,
 `destQX` float ,
 `destQY` float ,
 `destQZ` float 
)*/;

/*Table structure for table `vw_teleport_location` */

DROP TABLE IF EXISTS `vw_teleport_location`;

/*!50001 DROP VIEW IF EXISTS `vw_teleport_location` */;
/*!50001 DROP TABLE IF EXISTS `vw_teleport_location` */;

/*!50001 CREATE TABLE  `vw_teleport_location`(
 `name` text ,
 `landblock` int(10) unsigned ,
 `posX` float ,
 `posY` float ,
 `posZ` float ,
 `qW` float ,
 `qX` float ,
 `qY` float ,
 `qZ` float 
)*/;

/*View structure for view vw_ace_object */

/*!50001 DROP TABLE IF EXISTS `vw_ace_object` */;
/*!50001 DROP VIEW IF EXISTS `vw_ace_object` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_ace_object` AS select `ao`.`aceObjectId` AS `aceObjectId`,`aops`.`propertyValue` AS `name`,`ao`.`weenieClassId` AS `weenieClassId`,`ao`.`currentMotionState` AS `currentMotionState`,`awc`.`weenieClassDescription` AS `weenieClassDescription`,`ao`.`aceObjectDescriptionFlags` AS `aceObjectDescriptionFlags`,`ao`.`physicsDescriptionFlag` AS `physicsDescriptionFlag`,`ao`.`weenieHeaderFlags` AS `weenieHeaderFlags`,`aopi`.`propertyValue` AS `itemType`,`ap`.`positionId` AS `positionId`,`ap`.`positionType` AS `positionType`,`ap`.`landblockRaw` AS `LandblockRaw`,`ap`.`landblock` AS `landblock`,`ap`.`cell` AS `cell`,`ap`.`posX` AS `posX`,`ap`.`posY` AS `posY`,`ap`.`posZ` AS `posZ`,`ap`.`qW` AS `qW`,`ap`.`qX` AS `qX`,`ap`.`qY` AS `qY`,`ap`.`qZ` AS `qZ` from ((((`ace_object` `ao` join `ace_weenie_class` `awc` on(`ao`.`weenieClassId` = `awc`.`weenieClassId`)) join `ace_object_properties_string` `aops` on(`ao`.`aceObjectId` = `aops`.`aceObjectId` and `aops`.`strPropertyId` = 1)) join `ace_object_properties_int` `aopi` on(`ao`.`aceObjectId` = `aopi`.`aceObjectId` and `aopi`.`intPropertyId` = 1)) join `ace_position` `ap` on(`ao`.`aceObjectId` = `ap`.`aceObjectId` and `ap`.`positionType` = 1)) */;

/*View structure for view vw_ace_portal_object */

/*!50001 DROP TABLE IF EXISTS `vw_ace_portal_object` */;
/*!50001 DROP VIEW IF EXISTS `vw_ace_portal_object` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_ace_portal_object` AS (select `ao`.`aceObjectId` AS `aceObjectId`,`ao`.`name` AS `name`,`ao`.`weenieClassId` AS `weenieClassId`,`ao`.`weenieClassDescription` AS `weenieClassDescription`,`ao`.`aceObjectDescriptionFlags` AS `aceObjectDescriptionFlags`,`ao`.`physicsDescriptionFlag` AS `physicsDescriptionFlag`,`ao`.`weenieHeaderFlags` AS `weenieHeaderFlags`,`ao`.`itemType` AS `itemType`,`ao`.`positionId` AS `positionId`,`ao`.`positionType` AS `positionType`,`ao`.`LandblockRaw` AS `landblockRaw`,`ao`.`landblock` AS `landblock`,`ao`.`currentMotionState` AS `currentMotionState`,`ao`.`cell` AS `cell`,`ao`.`posX` AS `posX`,`ao`.`posY` AS `posY`,`ao`.`posZ` AS `posZ`,`ao`.`qW` AS `qW`,`ao`.`qX` AS `qX`,`ao`.`qY` AS `qY`,`ao`.`qZ` AS `qZ`,`apo`.`min_lvl` AS `min_lvl`,`apo`.`max_lvl` AS `max_lvl`,`apo`.`societyId` AS `societyId`,`apo`.`isTieable` AS `isTieable`,`apo`.`isRecallable` AS `isRecallable`,`apo`.`isSummonable` AS `isSummonable`,`ap`.`positionId` AS `destPositionId`,`ap`.`landblockRaw` AS `destLandblockId`,`ap`.`landblock` AS `destLandblock`,`ap`.`cell` AS `destCell`,`ap`.`posX` AS `destX`,`ap`.`posY` AS `destY`,`ap`.`posZ` AS `destZ`,`ap`.`qW` AS `destQW`,`ap`.`qX` AS `destQX`,`ap`.`qY` AS `destQY`,`ap`.`qZ` AS `destQZ` from ((`vw_ace_object` `ao` left join `ace_portal_object` `apo` on(`ao`.`aceObjectId` = `apo`.`aceObjectId`)) left join `ace_position` `ap` on(`apo`.`aceObjectId` = `ap`.`aceObjectId` and `ap`.`positionType` = 2)) where `ao`.`itemType` = 65536) */;

/*View structure for view vw_teleport_location */

/*!50001 DROP TABLE IF EXISTS `vw_teleport_location` */;
/*!50001 DROP VIEW IF EXISTS `vw_teleport_location` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_teleport_location` AS (select `apoi`.`name` AS `name`,`ap`.`landblockRaw` AS `landblock`,`ap`.`posX` AS `posX`,`ap`.`posY` AS `posY`,`ap`.`posZ` AS `posZ`,`ap`.`qW` AS `qW`,`ap`.`qX` AS `qX`,`ap`.`qY` AS `qY`,`ap`.`qZ` AS `qZ` from (`ace_poi` `apoi` join `ace_position` `ap` on(`apoi`.`positionId` = `ap`.`positionId`)) where `ap`.`positionType` = 28) */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
