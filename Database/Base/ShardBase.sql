/*
SQLyog Ultimate
MySQL - 10.2.6-MariaDB : Database - ace_shard
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`ace_shard` /*!40100 DEFAULT CHARACTER SET latin1 */;

USE `ace_shard`;

/*Table structure for table `ace_object` */

DROP TABLE IF EXISTS `ace_object`;

CREATE TABLE `ace_object` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL,
  `aceObjectDescriptionFlags` INT(10) UNSIGNED NOT NULL,
  `weenieClassId` INT(10) UNSIGNED NOT NULL,
  `weenieHeaderFlags` INT(10) UNSIGNED NOT NULL,
  `physicsDescriptionFlag` INT(10) UNSIGNED NOT NULL,
  `currentMotionState` TEXT DEFAULT NULL,
  PRIMARY KEY (`aceObjectId`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_animation_change` */

DROP TABLE IF EXISTS `ace_object_animation_change`;

CREATE TABLE `ace_object_animation_change` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL,
  `index` TINYINT(3) UNSIGNED NOT NULL,
  `animationId` INT(10) UNSIGNED NOT NULL,
  PRIMARY KEY (`aceObjectId`,`index`),
  CONSTRAINT `FK_ace_object_animation_changes__baseAceObjectId` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_palette_change` */

DROP TABLE IF EXISTS `ace_object_palette_change`;

CREATE TABLE `ace_object_palette_change` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL,
  `subPaletteId` INT(10) UNSIGNED NOT NULL,
  `offset` SMALLINT(5) UNSIGNED NOT NULL,
  `length` SMALLINT(5) UNSIGNED ZEROFILL NOT NULL,
  PRIMARY KEY (`aceObjectId`,`subPaletteId`,`offset`,`length`),
  CONSTRAINT `FK_ace_object_palette_data__baseAceObjectId` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`)
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
  CONSTRAINT `fk_Prop_BigInt_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_bool` */

DROP TABLE IF EXISTS `ace_object_properties_bool`;

CREATE TABLE `ace_object_properties_bool` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `boolPropertyId` SMALLINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `propertyValue` TINYINT(1) NOT NULL DEFAULT 0,
  UNIQUE KEY `ace_object__property_bool_id` (`aceObjectId`,`boolPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Bool_Ace_object` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_did` */

DROP TABLE IF EXISTS `ace_object_properties_did`;

CREATE TABLE `ace_object_properties_did` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `didPropertyId` SMALLINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `propertyValue` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  UNIQUE KEY `ace_object__property_did_id` (`aceObjectId`,`didPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Did_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_double` */

DROP TABLE IF EXISTS `ace_object_properties_double`;

CREATE TABLE `ace_object_properties_double` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `dblPropertyId` SMALLINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `propertyValue` DOUBLE NOT NULL DEFAULT 0,
  UNIQUE KEY `ace_object__property_double_id` (`aceObjectId`,`dblPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Dbl_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_iid` */

DROP TABLE IF EXISTS `ace_object_properties_iid`;

CREATE TABLE `ace_object_properties_iid` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `iidPropertyId` SMALLINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `propertyValue` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  UNIQUE KEY `ace_object__property_iid_id` (`aceObjectId`,`iidPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Iid_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_int` */

DROP TABLE IF EXISTS `ace_object_properties_int`;

CREATE TABLE `ace_object_properties_int` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `intPropertyId` SMALLINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `propertyValue` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  UNIQUE KEY `ace_object__property_int_id` (`aceObjectId`,`intPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Int_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_skill` */

DROP TABLE IF EXISTS `ace_object_properties_skill`;

CREATE TABLE `ace_object_properties_skill` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL,
  `skillId` TINYINT(1) UNSIGNED NOT NULL,
  `skillStatus` TINYINT(1) UNSIGNED NOT NULL,
  `skillPoints` SMALLINT(2) UNSIGNED NOT NULL DEFAULT 0,
  `skillXpSpent` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  PRIMARY KEY (`aceObjectId`,`skillId`),
  CONSTRAINT `fk_Prop_Skill_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_spell` */

DROP TABLE IF EXISTS `ace_object_properties_spell`;

CREATE TABLE `ace_object_properties_spell` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `spellId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  UNIQUE KEY `ace_object__property_spell_id` (`spellId`,`aceObjectId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Spell_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_string` */

DROP TABLE IF EXISTS `ace_object_properties_string`;

CREATE TABLE `ace_object_properties_string` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `strPropertyId` SMALLINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `propertyValue` TEXT NOT NULL,
  UNIQUE KEY `ace_object__property_string_id` (`aceObjectId`,`strPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Str_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_texture_map_change` */

DROP TABLE IF EXISTS `ace_object_texture_map_change`;

CREATE TABLE `ace_object_texture_map_change` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL,
  `index` TINYINT(3) UNSIGNED NOT NULL,
  `oldId` INT(10) UNSIGNED NOT NULL,
  `newId` INT(10) UNSIGNED NOT NULL,
  PRIMARY KEY (`aceObjectId`,`index`,`oldId`),
  CONSTRAINT `FK_ace_object_texture_map_changes__baseAceObjectId` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_poi` */

DROP TABLE IF EXISTS `ace_poi`;

CREATE TABLE `ace_poi` (
  `name` TEXT NOT NULL,
  `positionId` INT(10) UNSIGNED NOT NULL,
  PRIMARY KEY (`name`(100)),
  UNIQUE KEY `idx_poi` (`positionId`),
  CONSTRAINT `fk_poi_position` FOREIGN KEY (`positionId`) REFERENCES `ace_position` (`positionId`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_position` */

DROP TABLE IF EXISTS `ace_position`;

CREATE TABLE `ace_position` (
  `positionId` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `aceObjectId` INT(10) UNSIGNED NOT NULL,
  `positionType` SMALLINT(5) UNSIGNED NOT NULL,
  `landblockRaw` INT(10) UNSIGNED NOT NULL,
  `landblock` INT(5) UNSIGNED GENERATED ALWAYS AS (CONV(LEFT(HEX(`landblockRaw`),4),16,10)) STORED,
  `cell` INT(5) UNSIGNED GENERATED ALWAYS AS (CONV(LPAD(SUBSTR(HEX(`landblockRaw`),5,4),4,'0'),16,10)) STORED,
  `posX` FLOAT NOT NULL,
  `posY` FLOAT NOT NULL,
  `posZ` FLOAT NOT NULL,
  `qW` FLOAT NOT NULL,
  `qX` FLOAT NOT NULL,
  `qY` FLOAT NOT NULL,
  `qZ` FLOAT NOT NULL,
  PRIMARY KEY (`positionId`),
  KEY `idx_aceObjectId` (`aceObjectId`),
  KEY `idxPostionType` (`positionType`),
  KEY `idx_landblock_raw` (`landblockRaw`),
  KEY `idx_landblock` (`landblock`),
  KEY `idx_cell` (`cell`),
  CONSTRAINT `fk_ap_ao` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`),
  CONSTRAINT `fk_position_ao` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=43272 DEFAULT CHARSET=utf8;

/*Table structure for table `ace_weenie_class` */

DROP TABLE IF EXISTS `ace_weenie_class`;

CREATE TABLE `ace_weenie_class` (
  `weenieClassId` int(10) unsigned NOT NULL,
  `weenieClassDescription` text NOT NULL,
  PRIMARY KEY (`weenieClassId`),
  UNIQUE KEY `idx_weenieName` (`weenieClassDescription`(100))
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `character_friends` */

DROP TABLE IF EXISTS `character_friends`;

CREATE TABLE `character_friends` (
  `id` int(10) unsigned NOT NULL,
  `friendId` int(10) unsigned NOT NULL,
  PRIMARY KEY (`id`,`friendId`),
  KEY `FK_character_friends_friendId_character_guid` (`friendId`)
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

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_ace_character` AS select `ao`.`aceObjectId` AS `guid`,`aopiidacc`.`propertyValue` AS `accountId`,`aops`.`propertyValue` AS `NAME`,`aopb`.`propertyValue` AS `deleted`,`aopbi`.`propertyValue` AS `deleteTime`,`ao`.`weenieClassId` AS `weenieClassId`,`awc`.`weenieClassDescription` AS `weenieClassDescription`,`ao`.`aceObjectDescriptionFlags` AS `aceObjectDescriptionFlags`,`ao`.`physicsDescriptionFlag` AS `physicsDescriptionFlag`,`ao`.`weenieHeaderFlags` AS `weenieHeaderFlags`,`aopi`.`propertyValue` AS `itemType` from ((((((`ace_object` `ao` join `ace_weenie_class` `awc` on(`ao`.`weenieClassId` = `awc`.`weenieClassId`)) join `ace_object_properties_string` `aops` on(`ao`.`aceObjectId` = `aops`.`aceObjectId` and `aops`.`strPropertyId` = 1)) join `ace_object_properties_bool` `aopb` on(`ao`.`aceObjectId` = `aopb`.`aceObjectId` and `aopb`.`boolPropertyId` = 9001)) join `ace_object_properties_int` `aopi` on(`ao`.`aceObjectId` = `aopi`.`aceObjectId` and `aopi`.`intPropertyId` = 1)) join `ace_object_properties_bigint` `aopbi` on(`ao`.`aceObjectId` = `aopbi`.`aceObjectId` and `aopbi`.`bigIntPropertyId` = 9001)) join `ace_object_properties_iid` `aopiidacc` on(`ao`.`aceObjectId` = `aopiidacc`.`aceObjectId` and `aopiidacc`.`iidPropertyId` = 9001)) */;

/*View structure for view vw_ace_object */

/*!50001 DROP TABLE IF EXISTS `vw_ace_object` */;
/*!50001 DROP VIEW IF EXISTS `vw_ace_object` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_ace_object` AS select `ao`.`aceObjectId` AS `aceObjectId`,`aops`.`propertyValue` AS `name`,`ao`.`weenieClassId` AS `weenieClassId`,`awc`.`weenieClassDescription` AS `weenieClassDescription`,`ao`.`aceObjectDescriptionFlags` AS `aceObjectDescriptionFlags`,`ao`.`physicsDescriptionFlag` AS `physicsDescriptionFlag`,`ao`.`weenieHeaderFlags` AS `weenieHeaderFlags`,`aopi`.`propertyValue` AS `itemType`,`ap`.`positionId` AS `positionId`,`ap`.`positionType` AS `positionType`,`ap`.`landblockRaw` AS `LandblockRaw`,`ap`.`posX` AS `posX`,`ap`.`posY` AS `posY`,`ap`.`posZ` AS `posZ`,`ap`.`qW` AS `qW`,`ap`.`qX` AS `qX`,`ap`.`qY` AS `qY`,`ap`.`qZ` AS `qZ` from ((((`ace_object` `ao` join `ace_weenie_class` `awc` on(`ao`.`weenieClassId` = `awc`.`weenieClassId`)) join `ace_object_properties_string` `aops` on(`ao`.`aceObjectId` = `aops`.`aceObjectId` and `aops`.`strPropertyId` = 1)) join `ace_object_properties_int` `aopi` on(`ao`.`aceObjectId` = `aopi`.`aceObjectId` and `aopi`.`intPropertyId` = 1)) join `ace_position` `ap` on(`ao`.`aceObjectId` = `ap`.`aceObjectId` and `ap`.`positionType` = 1)) */;

/*View structure for view vw_teleport_location */

/*!50001 DROP TABLE IF EXISTS `vw_teleport_location` */;
/*!50001 DROP VIEW IF EXISTS `vw_teleport_location` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_teleport_location` AS (select `apoi`.`name` AS `name`,`ap`.`landblockRaw` AS `landblock`,`ap`.`posX` AS `posX`,`ap`.`posY` AS `posY`,`ap`.`posZ` AS `posZ`,`ap`.`qW` AS `qW`,`ap`.`qX` AS `qX`,`ap`.`qY` AS `qY`,`ap`.`qZ` AS `qZ` from (`ace_poi` `apoi` join `ace_position` `ap` on(`apoi`.`positionId` = `ap`.`positionId`)) where `ap`.`positionType` = 28) */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
