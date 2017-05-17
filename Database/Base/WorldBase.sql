/*
SQLyog Ultimate v12.4.1 (64 bit)
MySQL - 5.7.17-log : Database - ace_world
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
DROP DATABASE IF EXISTS ace_world;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`ace_world` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `ace_world`;

/*Table structure for table `ace_object` */

DROP TABLE IF EXISTS `ace_object`;

CREATE TABLE `ace_object` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL,
  `weenieClassId` INT(10) UNSIGNED NOT NULL,
  `aceObjectDescriptionFlags` INT(10) UNSIGNED NOT NULL,
  `animationFrameId` INT(10) UNSIGNED NOT NULL,
  `currentMotionState` TEXT NOT NULL,
  `iconId` INT(10) UNSIGNED NOT NULL,
  `iconOverlayId` INT(10) UNSIGNED NOT NULL,
  `iconUnderlayId` INT(10) UNSIGNED NOT NULL,
  `modelTableId` INT(10) UNSIGNED NOT NULL,
  `motionTableId` INT(10) UNSIGNED NOT NULL,
  `physicsDescriptionFlag` INT(10) UNSIGNED NOT NULL,
  `playScript` SMALLINT(5) UNSIGNED NOT NULL,
  `physicsTableId` INT(10) UNSIGNED NOT NULL,
  `soundTableId` INT(10) UNSIGNED NOT NULL,
  `weenieHeaderFlags` INT(10) UNSIGNED NOT NULL,
  `spellId` SMALLINT(5) UNSIGNED NOT NULL,
  `defaultScript` INT(10) UNSIGNED NOT NULL,
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

/*Table structure for table `ace_object_properties_bigint` */

DROP TABLE IF EXISTS `ace_object_properties_bigint`;

CREATE TABLE `ace_object_properties_bigint` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL DEFAULT '0',
  `bigIntPropertyId` SMALLINT(5) UNSIGNED NOT NULL DEFAULT '0',
  `propertyValue` BIGINT(20) UNSIGNED NOT NULL DEFAULT '0',
  UNIQUE KEY `ace_object__property_bigint_id` (`aceObjectId`,`bigIntPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_BigInt_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_bool` */

DROP TABLE IF EXISTS `ace_object_properties_bool`;

CREATE TABLE `ace_object_properties_bool` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL DEFAULT '0',
  `boolPropertyId` SMALLINT(5) UNSIGNED NOT NULL DEFAULT '0',
  `propertyValue` TINYINT(1) NOT NULL DEFAULT '0',
  UNIQUE KEY `ace_object__property_bool_id` (`aceObjectId`,`boolPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Bool_Ace_object` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_double` */

DROP TABLE IF EXISTS `ace_object_properties_double`;

CREATE TABLE `ace_object_properties_double` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL DEFAULT '0',
  `dblPropertyId` SMALLINT(5) NOT NULL DEFAULT '0',
  `propertyValue` DOUBLE NOT NULL DEFAULT '0',
  UNIQUE KEY `ace_object__property_double_id` (`aceObjectId`,`dblPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Dbl_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_int` */

DROP TABLE IF EXISTS `ace_object_properties_int`;

CREATE TABLE `ace_object_properties_int` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL DEFAULT '0',
  `intPropertyId` SMALLINT(5) UNSIGNED NOT NULL DEFAULT '0',
  `propertyValue` INT(10) UNSIGNED NOT NULL DEFAULT '0',
  UNIQUE KEY `ace_object__property_int_id` (`aceObjectId`,`intPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Int_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_string` */

DROP TABLE IF EXISTS `ace_object_properties_string`;

CREATE TABLE `ace_object_properties_string` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL DEFAULT '0',
  `strPropertyId` SMALLINT(5) UNSIGNED NOT NULL DEFAULT '0',
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
  `aceObjectId` INT(10) UNSIGNED DEFAULT NULL,
  `positionType` SMALLINT(5) UNSIGNED NOT NULL,
  `landblock` INT(10) UNSIGNED NOT NULL,
  `posX` FLOAT NOT NULL,
  `posY` FLOAT NOT NULL,
  `posZ` FLOAT NOT NULL,
  `qW` FLOAT NOT NULL,
  `qX` FLOAT NOT NULL,
  `qY` FLOAT NOT NULL,
  `qZ` FLOAT NOT NULL,
  PRIMARY KEY (`positionId`),
  KEY `idx_aceObjectId` (`aceObjectId`),
  KEY `idx_landblock` (`landblock`),
  KEY `idxPostionType` (`positionType`),
  CONSTRAINT `fk_ap_ao` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`)
) ENGINE=INNODB AUTO_INCREMENT=196669 DEFAULT CHARSET=utf8;

/*Table structure for table `ace_weenie_class` */

DROP TABLE IF EXISTS `ace_weenie_class`;

CREATE TABLE `ace_weenie_class` (
  `weenieClassId` INT(10) UNSIGNED NOT NULL,
  `weenieClassDescription` TEXT NOT NULL,
  PRIMARY KEY (`weenieClassId`),
  UNIQUE KEY `idx_weenieName` (`weenieClassDescription`(100))
) ENGINE=INNODB DEFAULT CHARSET=utf8;

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
 `animationFrameId` int(10) unsigned ,
 `currentMotionState` text ,
 `iconId` int(10) unsigned ,
 `iconOverlayId` int(10) unsigned ,
 `iconUnderlayId` int(10) unsigned ,
 `modelTableId` int(10) unsigned ,
 `motionTableId` int(10) unsigned ,
 `physicsDescriptionFlag` int(10) unsigned ,
 `playScript` smallint(5) unsigned ,
 `physicsTableId` int(10) unsigned ,
 `soundTableId` int(10) unsigned ,
 `weenieHeaderFlags` int(10) unsigned ,
 `spellId` smallint(5) unsigned ,
 `defaultScript` int(10) unsigned 
)*/;

/*Table structure for table `vw_ace_weenie_class` */

DROP TABLE IF EXISTS `vw_ace_weenie_class`;

/*!50001 DROP VIEW IF EXISTS `vw_ace_weenie_class` */;
/*!50001 DROP TABLE IF EXISTS `vw_ace_weenie_class` */;

/*!50001 CREATE TABLE  `vw_ace_weenie_class`(
 `weenieClassId` int(10) unsigned ,
 `weenieClassDescription` text ,
 `aceObjectDescriptionFlags` int(10) unsigned ,
 `animationFrameId` int(10) unsigned ,
 `currentMotionState` text ,
 `iconId` int(10) unsigned ,
 `iconOverlayId` int(10) unsigned ,
 `iconUnderlayId` int(10) unsigned ,
 `modelTableId` int(10) unsigned ,
 `motionTableId` int(10) unsigned ,
 `physicsDescriptionFlag` int(10) unsigned ,
 `playScript` smallint(5) unsigned ,
 `physicsTableId` int(10) unsigned ,
 `soundTableId` int(10) unsigned ,
 `weenieHeaderFlags` int(10) unsigned ,
 `spellId` smallint(5) unsigned ,
 `defaultScript` int(10) unsigned 
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

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_ace_object` AS select `ao`.`aceObjectId` AS `aceObjectId`,`aops`.`propertyValue` AS `name`,`ao`.`weenieClassId` AS `weenieClassId`,`awc`.`weenieClassDescription` AS `weenieClassDescription`,`ao`.`aceObjectDescriptionFlags` AS `aceObjectDescriptionFlags`,`ao`.`animationFrameId` AS `animationFrameId`,`ao`.`currentMotionState` AS `currentMotionState`,`ao`.`iconId` AS `iconId`,`ao`.`iconOverlayId` AS `iconOverlayId`,`ao`.`iconUnderlayId` AS `iconUnderlayId`,`ao`.`modelTableId` AS `modelTableId`,`ao`.`motionTableId` AS `motionTableId`,`ao`.`physicsDescriptionFlag` AS `physicsDescriptionFlag`,`ao`.`playScript` AS `playScript`,`ao`.`physicsTableId` AS `physicsTableId`,`ao`.`soundTableId` AS `soundTableId`,`ao`.`weenieHeaderFlags` AS `weenieHeaderFlags`,`ao`.`spellId` AS `spellId`,`ao`.`defaultScript` AS `defaultScript` from ((`ace_object` `ao` join `ace_weenie_class` `awc` on((`ao`.`weenieClassId` = `awc`.`weenieClassId`))) join `ace_object_properties_string` `aops` on(((`ao`.`aceObjectId` = `aops`.`aceObjectId`) and (`aops`.`strPropertyId` = 1)))) where (`ao`.`aceObjectId` <> `ao`.`weenieClassId`) */;

/*View structure for view vw_ace_weenie_class */

/*!50001 DROP TABLE IF EXISTS `vw_ace_weenie_class` */;
/*!50001 DROP VIEW IF EXISTS `vw_ace_weenie_class` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_ace_weenie_class` AS select `ao`.`weenieClassId` AS `weenieClassId`,`awc`.`weenieClassDescription` AS `weenieClassDescription`,`ao`.`aceObjectDescriptionFlags` AS `aceObjectDescriptionFlags`,`ao`.`animationFrameId` AS `animationFrameId`,`ao`.`currentMotionState` AS `currentMotionState`,`ao`.`iconId` AS `iconId`,`ao`.`iconOverlayId` AS `iconOverlayId`,`ao`.`iconUnderlayId` AS `iconUnderlayId`,`ao`.`modelTableId` AS `modelTableId`,`ao`.`motionTableId` AS `motionTableId`,`ao`.`physicsDescriptionFlag` AS `physicsDescriptionFlag`,`ao`.`playScript` AS `playScript`,`ao`.`physicsTableId` AS `physicsTableId`,`ao`.`soundTableId` AS `soundTableId`,`ao`.`weenieHeaderFlags` AS `weenieHeaderFlags`,`ao`.`spellId` AS `spellId`,`ao`.`defaultScript` AS `defaultScript` from (((select `ace_world`.`ace_object`.`aceObjectId` AS `aceObjectId`,`ace_world`.`ace_object`.`weenieClassId` AS `weenieClassId`,`ace_world`.`ace_object`.`aceObjectDescriptionFlags` AS `aceObjectDescriptionFlags`,`ace_world`.`ace_object`.`animationFrameId` AS `animationFrameId`,`ace_world`.`ace_object`.`currentMotionState` AS `currentMotionState`,`ace_world`.`ace_object`.`iconId` AS `iconId`,`ace_world`.`ace_object`.`iconOverlayId` AS `iconOverlayId`,`ace_world`.`ace_object`.`iconUnderlayId` AS `iconUnderlayId`,`ace_world`.`ace_object`.`modelTableId` AS `modelTableId`,`ace_world`.`ace_object`.`motionTableId` AS `motionTableId`,`ace_world`.`ace_object`.`physicsDescriptionFlag` AS `physicsDescriptionFlag`,`ace_world`.`ace_object`.`playScript` AS `playScript`,`ace_world`.`ace_object`.`physicsTableId` AS `physicsTableId`,`ace_world`.`ace_object`.`soundTableId` AS `soundTableId`,`ace_world`.`ace_object`.`weenieHeaderFlags` AS `weenieHeaderFlags`,`ace_world`.`ace_object`.`spellId` AS `spellId`,`ace_world`.`ace_object`.`defaultScript` AS `defaultScript` from `ace_world`.`ace_object` where (`ace_world`.`ace_object`.`aceObjectId` = `ace_world`.`ace_object`.`weenieClassId`))) `ao` join `ace_world`.`ace_weenie_class` `awc` on((`ao`.`weenieClassId` = `awc`.`weenieClassId`))) */;

/*View structure for view vw_teleport_location */

/*!50001 DROP TABLE IF EXISTS `vw_teleport_location` */;
/*!50001 DROP VIEW IF EXISTS `vw_teleport_location` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_teleport_location` AS (select `apoi`.`name` AS `name`,`ap`.`landblock` AS `landblock`,`ap`.`posX` AS `posX`,`ap`.`posY` AS `posY`,`ap`.`posZ` AS `posZ`,`ap`.`qW` AS `qW`,`ap`.`qX` AS `qX`,`ap`.`qY` AS `qY`,`ap`.`qZ` AS `qZ` from (`ace_poi` `apoi` join `ace_position` `ap` on((`apoi`.`positionId` = `ap`.`positionId`))) where (`ap`.`positionType` = 28)) */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
