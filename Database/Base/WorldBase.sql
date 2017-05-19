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
USE `ace_world`;

/*Table structure for table `ace_object` */

DROP TABLE IF EXISTS `ace_object`;

CREATE TABLE `ace_object` (
  `aceObjectId` int(10) unsigned NOT NULL,
  `weenieClassId` int(10) unsigned NOT NULL,
  `aceObjectDescriptionFlags` int(10) unsigned NOT NULL,
  `animationFrameId` int(10) unsigned DEFAULT NULL,
  `currentMotionState` text NOT NULL,
  `iconId` int(10) unsigned NOT NULL,
  `iconOverlayId` int(10) unsigned DEFAULT NULL,
  `iconUnderlayId` int(10) unsigned DEFAULT NULL,
  `modelTableId` int(10) unsigned DEFAULT NULL,
  `motionTableId` int(10) unsigned DEFAULT NULL,
  `physicsDescriptionFlag` int(10) unsigned NOT NULL,
  `playScript` smallint(5) unsigned DEFAULT NULL,
  `physicsTableId` int(10) unsigned DEFAULT NULL,
  `soundTableId` int(10) unsigned DEFAULT NULL,
  `weenieHeaderFlags` int(10) unsigned NOT NULL,
  `spellId` smallint(5) unsigned DEFAULT NULL,
  `defaultScript` int(10) unsigned DEFAULT NULL,
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
  CONSTRAINT `FK_ace_object_animation_changes__baseAceObjectId` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_palette_change` */

DROP TABLE IF EXISTS `ace_object_palette_change`;

CREATE TABLE `ace_object_palette_change` (
  `aceObjectId` int(10) unsigned NOT NULL,
  `subPaletteId` int(10) unsigned NOT NULL,
  `offset` smallint(5) unsigned NOT NULL,
  `length` smallint(5) unsigned zerofill NOT NULL,
  PRIMARY KEY (`aceObjectId`,`subPaletteId`,`offset`,`length`),
  CONSTRAINT `FK_ace_object_palette_data__baseAceObjectId` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_bigint` */

DROP TABLE IF EXISTS `ace_object_properties_bigint`;

CREATE TABLE `ace_object_properties_bigint` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT '0',
  `bigIntPropertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyValue` bigint(20) unsigned NOT NULL DEFAULT '0',
  UNIQUE KEY `ace_object__property_bigint_id` (`aceObjectId`,`bigIntPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_BigInt_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_bool` */

DROP TABLE IF EXISTS `ace_object_properties_bool`;

CREATE TABLE `ace_object_properties_bool` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT '0',
  `boolPropertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyValue` tinyint(1) NOT NULL DEFAULT '0',
  UNIQUE KEY `ace_object__property_bool_id` (`aceObjectId`,`boolPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Bool_Ace_object` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_double` */

DROP TABLE IF EXISTS `ace_object_properties_double`;

CREATE TABLE `ace_object_properties_double` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT '0',
  `dblPropertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyValue` double NOT NULL DEFAULT '0',
  UNIQUE KEY `ace_object__property_double_id` (`aceObjectId`,`dblPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Dbl_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_int` */

DROP TABLE IF EXISTS `ace_object_properties_int`;

CREATE TABLE `ace_object_properties_int` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT '0',
  `intPropertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyValue` int(10) unsigned NOT NULL DEFAULT '0',
  UNIQUE KEY `ace_object__property_int_id` (`aceObjectId`,`intPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Int_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_properties_string` */

DROP TABLE IF EXISTS `ace_object_properties_string`;

CREATE TABLE `ace_object_properties_string` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT '0',
  `strPropertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyValue` text NOT NULL,
  UNIQUE KEY `ace_object__property_string_id` (`aceObjectId`,`strPropertyId`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Str_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_texture_map_change` */

DROP TABLE IF EXISTS `ace_object_texture_map_change`;

CREATE TABLE `ace_object_texture_map_change` (
  `aceObjectId` int(10) unsigned NOT NULL,
  `index` tinyint(3) unsigned NOT NULL,
  `oldId` int(10) unsigned NOT NULL,
  `newId` int(10) unsigned NOT NULL,
  PRIMARY KEY (`aceObjectId`,`index`,`oldId`),
  CONSTRAINT `FK_ace_object_texture_map_changes__baseAceObjectId` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_poi` */

DROP TABLE IF EXISTS `ace_poi`;

CREATE TABLE `ace_poi` (
  `name` text NOT NULL,
  `positionId` int(10) unsigned NOT NULL,
  PRIMARY KEY (`name`(100)),
  UNIQUE KEY `idx_poi` (`positionId`),
  CONSTRAINT `fk_poi_position` FOREIGN KEY (`positionId`) REFERENCES `ace_position` (`positionId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_position` */

DROP TABLE IF EXISTS `ace_position`;

CREATE TABLE `ace_position` (
  `positionId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `aceObjectId` int(10) unsigned DEFAULT NULL,
  `positionType` smallint(5) unsigned NOT NULL,
  `landblock` int(10) unsigned NOT NULL,
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  `qW` float NOT NULL,
  `qX` float NOT NULL,
  `qY` float NOT NULL,
  `qZ` float NOT NULL,
  PRIMARY KEY (`positionId`),
  KEY `idx_aceObjectId` (`aceObjectId`),
  KEY `idx_landblock` (`landblock`),
  KEY `idxPostionType` (`positionType`),
  CONSTRAINT `fk_ap_ao` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`)
) ENGINE=InnoDB AUTO_INCREMENT=176111 DEFAULT CHARSET=utf8;

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
 `defaultScript` int(10) unsigned ,
 `itemType` int(10) unsigned 
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
 `defaultScript` int(10) unsigned ,
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
 `defaultScript` int(10) unsigned ,
 `name` text ,
 `ItemType` int(10) unsigned ,
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

/*View structure for view vw_ace_object */

/*!50001 DROP TABLE IF EXISTS `vw_ace_object` */;
/*!50001 DROP VIEW IF EXISTS `vw_ace_object` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_ace_object` AS select `ao`.`aceObjectId` AS `aceObjectId`,`aops`.`propertyValue` AS `name`,`ao`.`weenieClassId` AS `weenieClassId`,`awc`.`weenieClassDescription` AS `weenieClassDescription`,`ao`.`aceObjectDescriptionFlags` AS `aceObjectDescriptionFlags`,`ao`.`animationFrameId` AS `animationFrameId`,`ao`.`currentMotionState` AS `currentMotionState`,`ao`.`iconId` AS `iconId`,`ao`.`iconOverlayId` AS `iconOverlayId`,`ao`.`iconUnderlayId` AS `iconUnderlayId`,`ao`.`modelTableId` AS `modelTableId`,`ao`.`motionTableId` AS `motionTableId`,`ao`.`physicsDescriptionFlag` AS `physicsDescriptionFlag`,`ao`.`playScript` AS `playScript`,`ao`.`physicsTableId` AS `physicsTableId`,`ao`.`soundTableId` AS `soundTableId`,`ao`.`weenieHeaderFlags` AS `weenieHeaderFlags`,`ao`.`spellId` AS `spellId`,`ao`.`defaultScript` AS `defaultScript`,`aopi`.`propertyValue` AS `itemType` from (((`ace_object` `ao` join `ace_weenie_class` `awc` on((`ao`.`weenieClassId` = `awc`.`weenieClassId`))) join `ace_object_properties_string` `aops` on(((`ao`.`aceObjectId` = `aops`.`aceObjectId`) and (`aops`.`strPropertyId` = 1)))) join `ace_object_properties_int` `aopi` on(((`ao`.`aceObjectId` = `aopi`.`aceObjectId`) and (`aopi`.`intPropertyId` = 1)))) where (`ao`.`aceObjectId` <> `ao`.`weenieClassId`) */;

/*View structure for view vw_ace_weenie_class */

/*!50001 DROP TABLE IF EXISTS `vw_ace_weenie_class` */;
/*!50001 DROP VIEW IF EXISTS `vw_ace_weenie_class` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_ace_weenie_class` AS (select `ao`.`weenieClassId` AS `weenieClassId`,`ao`.`aceObjectId` AS `aceObjectId`,`aops`.`propertyValue` AS `name`,`awc`.`weenieClassDescription` AS `weenieClassDescription`,`ao`.`aceObjectDescriptionFlags` AS `aceObjectDescriptionFlags`,`ao`.`animationFrameId` AS `animationFrameId`,`ao`.`currentMotionState` AS `currentMotionState`,`ao`.`iconId` AS `iconId`,`ao`.`iconOverlayId` AS `iconOverlayId`,`ao`.`iconUnderlayId` AS `iconUnderlayId`,`ao`.`modelTableId` AS `modelTableId`,`ao`.`motionTableId` AS `motionTableId`,`ao`.`physicsDescriptionFlag` AS `physicsDescriptionFlag`,`ao`.`playScript` AS `playScript`,`ao`.`physicsTableId` AS `physicsTableId`,`ao`.`soundTableId` AS `soundTableId`,`ao`.`weenieHeaderFlags` AS `weenieHeaderFlags`,`ao`.`spellId` AS `spellId`,`ao`.`defaultScript` AS `defaultScript`,`aopi`.`propertyValue` AS `itemType` from (((`ace_object` `ao` join `ace_weenie_class` `awc` on((`ao`.`weenieClassId` = `awc`.`weenieClassId`))) join `ace_object_properties_string` `aops` on(((`ao`.`aceObjectId` = `aops`.`aceObjectId`) and (`aops`.`strPropertyId` = 1)))) join `ace_object_properties_int` `aopi` on(((`ao`.`aceObjectId` = `aopi`.`aceObjectId`) and (`aopi`.`intPropertyId` = 1)))) where (`ao`.`aceObjectId` = `ao`.`weenieClassId`)) */;

/*View structure for view vw_base_ace_object */

/*!50001 DROP TABLE IF EXISTS `vw_base_ace_object` */;
/*!50001 DROP VIEW IF EXISTS `vw_base_ace_object` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_base_ace_object` AS (select `ao`.`aceObjectId` AS `aceObjectId`,`ao`.`weenieClassId` AS `weenieClassId`,`ao`.`aceObjectDescriptionFlags` AS `aceObjectDescriptionFlags`,`ao`.`animationFrameId` AS `animationFrameId`,`ao`.`currentMotionState` AS `currentMotionState`,`ao`.`iconId` AS `iconId`,`ao`.`iconOverlayId` AS `iconOverlayId`,`ao`.`iconUnderlayId` AS `iconUnderlayId`,`ao`.`modelTableId` AS `modelTableId`,`ao`.`motionTableId` AS `motionTableId`,`ao`.`physicsDescriptionFlag` AS `physicsDescriptionFlag`,`ao`.`playScript` AS `playScript`,`ao`.`physicsTableId` AS `physicsTableId`,`ao`.`soundTableId` AS `soundTableId`,`ao`.`weenieHeaderFlags` AS `weenieHeaderFlags`,`ao`.`spellId` AS `spellId`,`ao`.`defaultScript` AS `defaultScript`,`aops`.`propertyValue` AS `name`,`aopi`.`propertyValue` AS `ItemType`,`awc`.`weenieClassDescription` AS `weenieClassDescription` from (((`ace_object` `ao` join `ace_weenie_class` `awc` on((`ao`.`weenieClassId` = `awc`.`weenieClassId`))) join `ace_object_properties_string` `aops` on(((`ao`.`aceObjectId` = `aops`.`aceObjectId`) and (`aops`.`strPropertyId` = 1)))) join `ace_object_properties_int` `aopi` on(((`ao`.`aceObjectId` = `aopi`.`aceObjectId`) and (`aopi`.`intPropertyId` = 1)))) where (`ao`.`aceObjectId` = `ao`.`weenieClassId`)) */;

/*View structure for view vw_teleport_location */

/*!50001 DROP TABLE IF EXISTS `vw_teleport_location` */;
/*!50001 DROP VIEW IF EXISTS `vw_teleport_location` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_teleport_location` AS (select `apoi`.`name` AS `name`,`ap`.`landblock` AS `landblock`,`ap`.`posX` AS `posX`,`ap`.`posY` AS `posY`,`ap`.`posZ` AS `posZ`,`ap`.`qW` AS `qW`,`ap`.`qX` AS `qX`,`ap`.`qY` AS `qY`,`ap`.`qZ` AS `qZ` from (`ace_poi` `apoi` join `ace_position` `ap` on((`apoi`.`positionId` = `ap`.`positionId`))) where (`ap`.`positionType` = 28)) */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
