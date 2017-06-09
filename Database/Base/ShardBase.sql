/*
SQLyog Ultimate
MySQL - 5.7.17-log : Database - ace_shard
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`ace_shard` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `ace_shard`;

/*Table structure for table `ace_object` */

DROP TABLE IF EXISTS `ace_object`;

CREATE TABLE `ace_object` (
  `aceObjectId` int(10) unsigned NOT NULL,
  `aceObjectDescriptionFlags` int(10) unsigned NOT NULL,
  `weenieClassId` int(10) unsigned NOT NULL,
  `weenieHeaderFlags` int(10) unsigned NOT NULL,
  `physicsDescriptionFlag` int(10) unsigned NOT NULL,
  `currentMotionState` text,
  PRIMARY KEY (`aceObjectId`),
  KEY `idx_weenie` (`weenieClassId`)
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
  `attributeId` smallint(5) unsigned NOT NULL,
  `attributeBase` smallint(5) unsigned NOT NULL DEFAULT '0',
  `attributeRanks` smallint(5) unsigned NOT NULL DEFAULT '0',
  `attributeXpSpent` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`aceObjectId`,`attributeId`),
  UNIQUE KEY `ace_object__property_attribute_id` (`aceObjectId`,`attributeId`),
  CONSTRAINT `fk_Prop_Attribute_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_attribute2nd` */

DROP TABLE IF EXISTS `ace_object_properties_attribute2nd`;

CREATE TABLE `ace_object_properties_attribute2nd` (
  `aceObjectId` int(10) unsigned NOT NULL,
  `attribute2ndId` smallint(5) unsigned NOT NULL,
  `attribute2ndValue` smallint(5) unsigned NOT NULL DEFAULT '0',
  `attribute2ndRanks` smallint(5) unsigned NOT NULL DEFAULT '0',
  `attribute2ndXpSpent` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`aceObjectId`,`attribute2ndId`),
  UNIQUE KEY `ace_object__property_attribute2nd_id` (`aceObjectId`,`attribute2ndId`),
  CONSTRAINT `fk_Prop_Attribute2nd_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_bigint` */

DROP TABLE IF EXISTS `ace_object_properties_bigint`;

CREATE TABLE `ace_object_properties_bigint` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT '0',
  `bigIntPropertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyValue` bigint(20) unsigned NOT NULL DEFAULT '0',
  UNIQUE KEY `ace_object__property_bigint_id` (`aceObjectId`,`bigIntPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_BigInt_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_bool` */

DROP TABLE IF EXISTS `ace_object_properties_bool`;

CREATE TABLE `ace_object_properties_bool` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT '0',
  `boolPropertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyValue` tinyint(1) NOT NULL DEFAULT '0',
  UNIQUE KEY `ace_object__property_bool_id` (`aceObjectId`,`boolPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Bool_Ace_object` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_did` */

DROP TABLE IF EXISTS `ace_object_properties_did`;

CREATE TABLE `ace_object_properties_did` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT '0',
  `didPropertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyValue` int(10) unsigned NOT NULL DEFAULT '0',
  UNIQUE KEY `ace_object__property_did_id` (`aceObjectId`,`didPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Did_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_double` */

DROP TABLE IF EXISTS `ace_object_properties_double`;

CREATE TABLE `ace_object_properties_double` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT '0',
  `dblPropertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyValue` double NOT NULL DEFAULT '0',
  UNIQUE KEY `ace_object__property_double_id` (`aceObjectId`,`dblPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Dbl_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_iid` */

DROP TABLE IF EXISTS `ace_object_properties_iid`;

CREATE TABLE `ace_object_properties_iid` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT '0',
  `iidPropertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyValue` int(10) unsigned NOT NULL DEFAULT '0',
  UNIQUE KEY `ace_object__property_iid_id` (`aceObjectId`,`iidPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Iid_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_int` */

DROP TABLE IF EXISTS `ace_object_properties_int`;

CREATE TABLE `ace_object_properties_int` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT '0',
  `intPropertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyValue` int(10) unsigned NOT NULL DEFAULT '0',
  UNIQUE KEY `ace_object__property_int_id` (`aceObjectId`,`intPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Int_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_skill` */

DROP TABLE IF EXISTS `ace_object_properties_skill`;

CREATE TABLE `ace_object_properties_skill` (
  `aceObjectId` int(10) unsigned NOT NULL,
  `skillId` smallint(5) unsigned NOT NULL,
  `skillStatus` smallint(5) unsigned NOT NULL,
  `skillPoints` smallint(5) unsigned NOT NULL DEFAULT '0',
  `skillXpSpent` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`aceObjectId`,`skillId`),
  CONSTRAINT `fk_Prop_Skill_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_spell` */

DROP TABLE IF EXISTS `ace_object_properties_spell`;

CREATE TABLE `ace_object_properties_spell` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT '0',
  `spellId` int(10) unsigned NOT NULL DEFAULT '0',
  UNIQUE KEY `ace_object__property_spell_id` (`spellId`,`aceObjectId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Spell_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_string` */

DROP TABLE IF EXISTS `ace_object_properties_string`;

CREATE TABLE `ace_object_properties_string` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT '0',
  `strPropertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
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

/*Table structure for table `ace_position` */

DROP TABLE IF EXISTS `ace_position`;

CREATE TABLE `ace_position` (
  `positionId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `aceObjectId` int(10) unsigned NOT NULL,
  `positionType` smallint(5) unsigned NOT NULL,
  `landblockRaw` int(10) unsigned NOT NULL,
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  `qW` float NOT NULL,
  `qX` float NOT NULL,
  `qY` float NOT NULL,
  `qZ` float NOT NULL,
  `landblock` int(5) unsigned GENERATED ALWAYS AS (conv(left(hex(`landblockRaw`),4),16,10)) STORED,
  `cell` int(5) unsigned GENERATED ALWAYS AS (conv(lpad(substr(hex(`landblockRaw`),5,4),4,'0'),16,10)) STORED,
  PRIMARY KEY (`positionId`),
  KEY `idx_aceObjectId` (`aceObjectId`),
  KEY `idxPostionType` (`positionType`),
  KEY `idx_landblock_raw` (`landblockRaw`),
  KEY `idx_landblock` (`landblock`),
  KEY `idx_cell` (`cell`),
  CONSTRAINT `fk_position_ao` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

/*Table structure for table `ace_weenie_class` */

DROP TABLE IF EXISTS `ace_weenie_class`;

CREATE TABLE `ace_weenie_class` (
  `weenieClassId` int(10) unsigned NOT NULL,
  `weenieClassDescription` text NOT NULL,
  PRIMARY KEY (`weenieClassId`),
  UNIQUE KEY `idx_weenieName` (`weenieClassDescription`(100))
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `vw_ace_character` */

DROP TABLE IF EXISTS `vw_ace_character`;

/*!50001 DROP VIEW IF EXISTS `vw_ace_character` */;
/*!50001 DROP TABLE IF EXISTS `vw_ace_character` */;

/*!50001 CREATE TABLE  `vw_ace_character`(
 `guid` int(10) unsigned ,
 `accountId` int(10) unsigned ,
 `NAME` text ,
 `deleted` tinyint(1) ,
 `deleteTime` bigint(20) unsigned ,
 `weenieClassId` int(10) unsigned ,
 `weenieClassDescription` text ,
 `aceObjectDescriptionFlags` int(10) unsigned ,
 `physicsDescriptionFlag` int(10) unsigned ,
 `weenieHeaderFlags` int(10) unsigned ,
 `itemType` int(10) unsigned 
)*/;

/*Table structure for table `vw_ace_object` */

DROP TABLE IF EXISTS `vw_ace_object`;

/*!50001 DROP VIEW IF EXISTS `vw_ace_object` */;
/*!50001 DROP TABLE IF EXISTS `vw_ace_object` */;

/*!50001 CREATE TABLE  `vw_ace_object`(
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
 `LandblockRaw` int(10) unsigned ,
 `posX` float ,
 `posY` float ,
 `posZ` float ,
 `qW` float ,
 `qX` float ,
 `qY` float ,
 `qZ` float 
)*/;

/*Table structure for table `vw_ace_weenie_class` */

DROP TABLE IF EXISTS `vw_ace_weenie_class`;

/*!50001 DROP VIEW IF EXISTS `vw_ace_weenie_class` */;
/*!50001 DROP TABLE IF EXISTS `vw_ace_weenie_class` */;

/*!50001 CREATE TABLE  `vw_ace_weenie_class`(
 `weenieClassId` int(10) unsigned ,
 `aceObjectId` int(10) unsigned ,
 `name` text ,
 `weenieClassDescription` text ,
 `currentMotionState` text ,
 `physicsDescriptionFlag` int(10) unsigned ,
 `itemType` int(10) unsigned 
)*/;

/*Table structure for table `vw_base_ace_object` */

DROP TABLE IF EXISTS `vw_base_ace_object`;

/*!50001 DROP VIEW IF EXISTS `vw_base_ace_object` */;
/*!50001 DROP TABLE IF EXISTS `vw_base_ace_object` */;

/*!50001 CREATE TABLE  `vw_base_ace_object`(
 `aceObjectId` int(10) unsigned ,
 `weenieClassId` int(10) unsigned ,
 `aceObjectDescriptionFlags` int(10) unsigned ,
 `currentMotionState` text ,
 `physicsDescriptionFlag` int(10) unsigned ,
 `weenieHeaderFlags` int(10) unsigned ,
 `name` text ,
 `itemType` int(10) unsigned ,
 `weenieClassDescription` text 
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

/*View structure for view vw_ace_character */

/*!50001 DROP TABLE IF EXISTS `vw_ace_character` */;
/*!50001 DROP VIEW IF EXISTS `vw_ace_character` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_ace_character` AS select `ao`.`aceObjectId` AS `guid`,`aopiidacc`.`propertyValue` AS `accountId`,`aops`.`propertyValue` AS `NAME`,`aopb`.`propertyValue` AS `deleted`,`aopbi`.`propertyValue` AS `deleteTime`,`ao`.`weenieClassId` AS `weenieClassId`,`awc`.`weenieClassDescription` AS `weenieClassDescription`,`ao`.`aceObjectDescriptionFlags` AS `aceObjectDescriptionFlags`,`ao`.`physicsDescriptionFlag` AS `physicsDescriptionFlag`,`ao`.`weenieHeaderFlags` AS `weenieHeaderFlags`,`aopi`.`propertyValue` AS `itemType` from ((((((`ace_object` `ao` join `ace_weenie_class` `awc` on((`ao`.`weenieClassId` = `awc`.`weenieClassId`))) join `ace_object_properties_string` `aops` on(((`ao`.`aceObjectId` = `aops`.`aceObjectId`) and (`aops`.`strPropertyId` = 1)))) join `ace_object_properties_bool` `aopb` on(((`ao`.`aceObjectId` = `aopb`.`aceObjectId`) and (`aopb`.`boolPropertyId` = 9001)))) join `ace_object_properties_int` `aopi` on(((`ao`.`aceObjectId` = `aopi`.`aceObjectId`) and (`aopi`.`intPropertyId` = 1)))) join `ace_object_properties_bigint` `aopbi` on(((`ao`.`aceObjectId` = `aopbi`.`aceObjectId`) and (`aopbi`.`bigIntPropertyId` = 9001)))) join `ace_object_properties_iid` `aopiidacc` on(((`ao`.`aceObjectId` = `aopiidacc`.`aceObjectId`) and (`aopiidacc`.`iidPropertyId` = 9001)))) */;

/*View structure for view vw_ace_object */

/*!50001 DROP TABLE IF EXISTS `vw_ace_object` */;
/*!50001 DROP VIEW IF EXISTS `vw_ace_object` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_ace_object` AS select `ao`.`aceObjectId` AS `aceObjectId`,`aops`.`propertyValue` AS `name`,`ao`.`weenieClassId` AS `weenieClassId`,`awc`.`weenieClassDescription` AS `weenieClassDescription`,`ao`.`aceObjectDescriptionFlags` AS `aceObjectDescriptionFlags`,`ao`.`physicsDescriptionFlag` AS `physicsDescriptionFlag`,`ao`.`weenieHeaderFlags` AS `weenieHeaderFlags`,`aopi`.`propertyValue` AS `itemType`,`ap`.`positionId` AS `positionId`,`ap`.`positionType` AS `positionType`,`ap`.`landblockRaw` AS `LandblockRaw`,`ap`.`posX` AS `posX`,`ap`.`posY` AS `posY`,`ap`.`posZ` AS `posZ`,`ap`.`qW` AS `qW`,`ap`.`qX` AS `qX`,`ap`.`qY` AS `qY`,`ap`.`qZ` AS `qZ` from ((((`ace_object` `ao` join `ace_weenie_class` `awc` on((`ao`.`weenieClassId` = `awc`.`weenieClassId`))) join `ace_object_properties_string` `aops` on(((`ao`.`aceObjectId` = `aops`.`aceObjectId`) and (`aops`.`strPropertyId` = 1)))) join `ace_object_properties_int` `aopi` on(((`ao`.`aceObjectId` = `aopi`.`aceObjectId`) and (`aopi`.`intPropertyId` = 1)))) join `ace_position` `ap` on(((`ao`.`aceObjectId` = `ap`.`aceObjectId`) and (`ap`.`positionType` = 1)))) */;

/*View structure for view vw_ace_weenie_class */

/*!50001 DROP TABLE IF EXISTS `vw_ace_weenie_class` */;
/*!50001 DROP VIEW IF EXISTS `vw_ace_weenie_class` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_ace_weenie_class` AS (select `ao`.`weenieClassId` AS `weenieClassId`,`ao`.`aceObjectId` AS `aceObjectId`,`aops`.`propertyValue` AS `name`,`awc`.`weenieClassDescription` AS `weenieClassDescription`,`ao`.`currentMotionState` AS `currentMotionState`,`ao`.`physicsDescriptionFlag` AS `physicsDescriptionFlag`,`aopi`.`propertyValue` AS `itemType` from (((`ace_object` `ao` join `ace_weenie_class` `awc` on((`ao`.`weenieClassId` = `awc`.`weenieClassId`))) join `ace_object_properties_string` `aops` on(((`ao`.`aceObjectId` = `aops`.`aceObjectId`) and (`aops`.`strPropertyId` = 1)))) join `ace_object_properties_int` `aopi` on(((`ao`.`aceObjectId` = `aopi`.`aceObjectId`) and (`aopi`.`intPropertyId` = 1)))) where (`ao`.`aceObjectId` = `ao`.`weenieClassId`)) */;

/*View structure for view vw_base_ace_object */

/*!50001 DROP TABLE IF EXISTS `vw_base_ace_object` */;
/*!50001 DROP VIEW IF EXISTS `vw_base_ace_object` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_base_ace_object` AS (select `ao`.`aceObjectId` AS `aceObjectId`,`ao`.`weenieClassId` AS `weenieClassId`,`ao`.`aceObjectDescriptionFlags` AS `aceObjectDescriptionFlags`,`ao`.`currentMotionState` AS `currentMotionState`,`ao`.`physicsDescriptionFlag` AS `physicsDescriptionFlag`,`ao`.`weenieHeaderFlags` AS `weenieHeaderFlags`,`aops`.`propertyValue` AS `name`,`aopi`.`propertyValue` AS `itemType`,`awc`.`weenieClassDescription` AS `weenieClassDescription` from (((`ace_object` `ao` join `ace_weenie_class` `awc` on((`ao`.`weenieClassId` = `awc`.`weenieClassId`))) join `ace_object_properties_string` `aops` on(((`ao`.`aceObjectId` = `aops`.`aceObjectId`) and (`aops`.`strPropertyId` = 1)))) join `ace_object_properties_int` `aopi` on(((`ao`.`aceObjectId` = `aopi`.`aceObjectId`) and (`aopi`.`intPropertyId` = 1)))) where (`ao`.`aceObjectId` = `ao`.`weenieClassId`)) */;

/*View structure for view vw_teleport_location */

/*!50001 DROP TABLE IF EXISTS `vw_teleport_location` */;
/*!50001 DROP VIEW IF EXISTS `vw_teleport_location` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_teleport_location` AS (select `apoi`.`name` AS `name`,`ap`.`landblockRaw` AS `landblock`,`ap`.`posX` AS `posX`,`ap`.`posY` AS `posY`,`ap`.`posZ` AS `posZ`,`ap`.`qW` AS `qW`,`ap`.`qX` AS `qX`,`ap`.`qY` AS `qY`,`ap`.`qZ` AS `qZ` from (`ace_poi` `apoi` join `ace_position` `ap` on((`apoi`.`positionId` = `ap`.`positionId`))) where (`ap`.`positionType` = 2)) */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
