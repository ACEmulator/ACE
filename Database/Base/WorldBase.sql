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

/*Table structure for table `ace_weenie_class` */

DROP TABLE IF EXISTS `ace_weenie_class`;

CREATE TABLE `ace_weenie_class` (
  `weenieClassId` INT(10) UNSIGNED NOT NULL,
  `weenieClassDescription` TEXT NOT NULL,
  PRIMARY KEY (`weenieClassId`),
  UNIQUE KEY `idx_weenieName` (`weenieClassDescription`(100))
) ENGINE=INNODB DEFAULT CHARSET=utf8;


/*Table structure for table `ace_object` */

DROP TABLE IF EXISTS `ace_object`;

CREATE TABLE `ace_object` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL,
  `aceObjectDescriptionFlags` INT(10) UNSIGNED NOT NULL,
  `weenieClassId` INT(10) UNSIGNED NOT NULL,
  `weenieHeaderFlags` INT(10) UNSIGNED NOT NULL,
  `physicsDescriptionFlag` INT(10) UNSIGNED NOT NULL,
  `currentMotionState` TEXT DEFAULT NULL,
  PRIMARY KEY (`aceObjectId`),
  KEY `idx_weenie` (`weenieClassId`),
  CONSTRAINT `fk_weenie_ao` FOREIGN KEY (`weenieClassId`) REFERENCES `ace_weenie_class` (`weenieClassId`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_animation_change` */

DROP TABLE IF EXISTS `ace_object_animation_change`;

CREATE TABLE `ace_object_animation_change` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL,
  `index` TINYINT(3) UNSIGNED NOT NULL,
  `animationId` INT(10) UNSIGNED NOT NULL,
  PRIMARY KEY (`aceObjectId`,`index`),
  CONSTRAINT `FK_ace_object_animation_changes__baseAceObjectId` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_palette_change` */

DROP TABLE IF EXISTS `ace_object_palette_change`;

CREATE TABLE `ace_object_palette_change` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL,
  `subPaletteId` INT(10) UNSIGNED NOT NULL,
  `offset` SMALLINT(5) UNSIGNED NOT NULL,
  `length` SMALLINT(5) UNSIGNED ZEROFILL NOT NULL,
  PRIMARY KEY (`aceObjectId`,`subPaletteId`,`offset`,`length`),
  CONSTRAINT `FK_ace_object_palette_data__baseAceObjectId` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_attribute` */

DROP TABLE IF EXISTS `ace_object_properties_attribute`;

CREATE TABLE `ace_object_properties_attribute` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL,
  `attributeId` SMALLINT(4) UNSIGNED NOT NULL,
  `attributeBase` SMALLINT(4) UNSIGNED NOT NULL DEFAULT 0,
  `attributeRanks` TINYINT(2) UNSIGNED NOT NULL DEFAULT 0,
  `attributeXpSpent` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  PRIMARY KEY (`aceObjectId`,`attributeId`),
  UNIQUE KEY `ace_object__property_attribute_id` (`aceObjectId`,`attributeId`),
  CONSTRAINT `fk_Prop_Attribute_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_attribute2nd` */

DROP TABLE IF EXISTS `ace_object_properties_attribute2nd`;

CREATE TABLE `ace_object_properties_attribute2nd` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL,
  `attribute2ndId` SMALLINT(4) UNSIGNED NOT NULL,
  `attribute2ndValue` MEDIUMINT(7) UNSIGNED NOT NULL DEFAULT 0,
  `attribute2ndRanks` TINYINT(2) UNSIGNED NOT NULL DEFAULT 0,
  `attribute2ndXpSpent` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  PRIMARY KEY (`aceObjectId`,`attribute2ndId`),
  UNIQUE KEY `ace_object__property_attribute2nd_id` (`aceObjectId`,`attribute2ndId`),
  CONSTRAINT `fk_Prop_Attribute2nd_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_bigint` */

DROP TABLE IF EXISTS `ace_object_properties_bigint`;

CREATE TABLE `ace_object_properties_bigint` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `bigIntPropertyId` SMALLINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `propertyValue` BIGINT(20) UNSIGNED NOT NULL DEFAULT 0,
  UNIQUE KEY `ace_object__property_bigint_id` (`aceObjectId`,`bigIntPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_BigInt_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_bool` */

DROP TABLE IF EXISTS `ace_object_properties_bool`;

CREATE TABLE `ace_object_properties_bool` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `boolPropertyId` SMALLINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `propertyValue` TINYINT(1) NOT NULL DEFAULT 0,
  UNIQUE KEY `ace_object__property_bool_id` (`aceObjectId`,`boolPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Bool_Ace_object` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_did` */

DROP TABLE IF EXISTS `ace_object_properties_did`;

CREATE TABLE `ace_object_properties_did` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `didPropertyId` SMALLINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `propertyValue` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  UNIQUE KEY `ace_object__property_did_id` (`aceObjectId`,`didPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Did_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_double` */

DROP TABLE IF EXISTS `ace_object_properties_double`;

CREATE TABLE `ace_object_properties_double` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `dblPropertyId` SMALLINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `propertyValue` DOUBLE NOT NULL DEFAULT 0,
  UNIQUE KEY `ace_object__property_double_id` (`aceObjectId`,`dblPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Dbl_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_iid` */

DROP TABLE IF EXISTS `ace_object_properties_iid`;

CREATE TABLE `ace_object_properties_iid` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `iidPropertyId` SMALLINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `propertyValue` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  UNIQUE KEY `ace_object__property_iid_id` (`aceObjectId`,`iidPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Iid_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_int` */

DROP TABLE IF EXISTS `ace_object_properties_int`;

CREATE TABLE `ace_object_properties_int` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `intPropertyId` SMALLINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `propertyValue` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  UNIQUE KEY `ace_object__property_int_id` (`aceObjectId`,`intPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Int_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_spell` */

DROP TABLE IF EXISTS `ace_object_properties_spell`;

CREATE TABLE `ace_object_properties_spell` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `spellId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  UNIQUE KEY `ace_object__property_spell_id` (`spellId`,`aceObjectId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Spell_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_string` */

DROP TABLE IF EXISTS `ace_object_properties_string`;

CREATE TABLE `ace_object_properties_string` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `strPropertyId` SMALLINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `propertyValue` TEXT NOT NULL,
  UNIQUE KEY `ace_object__property_string_id` (`aceObjectId`,`strPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Str_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_texture_map_change` */

DROP TABLE IF EXISTS `ace_object_texture_map_change`;

CREATE TABLE `ace_object_texture_map_change` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL,
  `index` TINYINT(3) UNSIGNED NOT NULL,
  `oldId` INT(10) UNSIGNED NOT NULL,
  `newId` INT(10) UNSIGNED NOT NULL,
  PRIMARY KEY (`aceObjectId`,`index`,`oldId`),
  CONSTRAINT `FK_ace_object_texture_map_changes__baseAceObjectId` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_poi` */

DROP TABLE IF EXISTS `ace_poi`;

CREATE TABLE `ace_poi` (
  `name` TEXT NOT NULL,
  `positionId` INT(10) UNSIGNED NOT NULL,
  PRIMARY KEY (`name`(100)),
  UNIQUE KEY `idx_poi` (`positionId`),
  CONSTRAINT `fk_poi_position` FOREIGN KEY (`positionId`) REFERENCES `ace_position` (`positionId`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_portal_object` */

DROP TABLE IF EXISTS `ace_portal_object`;

CREATE TABLE `ace_portal_object` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL,
  `positionId` INT(10) UNSIGNED NOT NULL,
  `min_lvl` INT(11) UNSIGNED NOT NULL DEFAULT 0,
  `max_lvl` INT(11) UNSIGNED NOT NULL DEFAULT 0,
  `societyId` TINYINT(3) UNSIGNED NOT NULL DEFAULT 0,
  `isTieable` TINYINT(1) UNSIGNED NOT NULL DEFAULT 1,
  `isRecallable` TINYINT(1) UNSIGNED NOT NULL DEFAULT 1,
  `isSummonable` TINYINT(1) UNSIGNED NOT NULL DEFAULT 1,
  PRIMARY KEY (`aceObjectId`),
  KEY `FK_apo2po` (`positionId`),
  CONSTRAINT `FK_apo2ao` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`),
  CONSTRAINT `FK_apo2po` FOREIGN KEY (`positionId`) REFERENCES `ace_position` (`positionId`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_position` */

DROP TABLE IF EXISTS `ace_position`;

CREATE TABLE `ace_position` (
  `positionId` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `aceObjectId` INT(10) UNSIGNED DEFAULT NULL,
  `positionType` SMALLINT(5) UNSIGNED NOT NULL,
  `landblockRaw` INT(10) UNSIGNED NOT NULL,
  `landblock` SMALLINT(5) UNSIGNED GENERATED ALWAYS AS (CONV(LEFT(HEX(`landblockRaw`),4),16,10)) STORED,
  `cell` SMALLINT(5) UNSIGNED GENERATED ALWAYS AS (CONV(LPAD(SUBSTR(HEX(`landblockRaw`),5,4),4,'0'),16,10)) STORED,
  `posX` FLOAT NOT NULL,
  `posY` FLOAT NOT NULL,
  `posZ` FLOAT NOT NULL,
  `qW` FLOAT NOT NULL,
  `qX` FLOAT NOT NULL,
  `qY` FLOAT NOT NULL,
  `qZ` FLOAT NOT NULL,
  PRIMARY KEY (`positionId`),
  KEY `idx_aceObjectId` (`aceObjectId`),
  KEY `idx_landblock` (`landblockRaw`),
  KEY `idxPostionType` (`positionType`),
  CONSTRAINT `fk_ap_ao` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`),
  CONSTRAINT `fk_position_ao` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=INNODB AUTO_INCREMENT=178436 DEFAULT CHARSET=utf8;

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
