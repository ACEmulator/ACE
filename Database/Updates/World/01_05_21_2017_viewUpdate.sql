/*
SQLyog Ultimate v12.4.1 (64 bit)
MySQL - 10.1.23-MariaDB : Database - ace_world
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
USE `ace_world`;

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
 `itemType` int(10) unsigned ,
 `positionId` int(10) unsigned ,
 `positionType` smallint(5) unsigned ,
 `landblock` int(10) unsigned ,
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

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_ace_object` AS select `ao`.`aceObjectId` AS `aceObjectId`,`aops`.`propertyValue` AS `name`,`ao`.`weenieClassId` AS `weenieClassId`,`awc`.`weenieClassDescription` AS `weenieClassDescription`,`ao`.`aceObjectDescriptionFlags` AS `aceObjectDescriptionFlags`,`ao`.`animationFrameId` AS `animationFrameId`,`ao`.`currentMotionState` AS `currentMotionState`,`ao`.`iconId` AS `iconId`,`ao`.`iconOverlayId` AS `iconOverlayId`,`ao`.`iconUnderlayId` AS `iconUnderlayId`,`ao`.`modelTableId` AS `modelTableId`,`ao`.`motionTableId` AS `motionTableId`,`ao`.`physicsDescriptionFlag` AS `physicsDescriptionFlag`,`ao`.`playScript` AS `playScript`,`ao`.`physicsTableId` AS `physicsTableId`,`ao`.`soundTableId` AS `soundTableId`,`ao`.`weenieHeaderFlags` AS `weenieHeaderFlags`,`ao`.`spellId` AS `spellId`,`ao`.`defaultScript` AS `defaultScript`,`aopi`.`propertyValue` AS `itemType`,`ap`.`positionId` AS `positionId`,`ap`.`positionType` AS `positionType`,`ap`.`landblock` AS `landblock`,`ap`.`posX` AS `posX`,`ap`.`posY` AS `posY`,`ap`.`posZ` AS `posZ`,`ap`.`qW` AS `qW`,`ap`.`qX` AS `qX`,`ap`.`qY` AS `qY`,`ap`.`qZ` AS `qZ` from ((((`ace_object` `ao` join `ace_weenie_class` `awc` on((`ao`.`weenieClassId` = `awc`.`weenieClassId`))) join `ace_object_properties_string` `aops` on(((`ao`.`aceObjectId` = `aops`.`aceObjectId`) and (`aops`.`strPropertyId` = 1)))) join `ace_object_properties_int` `aopi` on(((`ao`.`aceObjectId` = `aopi`.`aceObjectId`) and (`aopi`.`intPropertyId` = 1)))) join `ace_position` `ap` on(((`ao`.`aceObjectId` = `ap`.`aceObjectId`) and (`ap`.`positionType` = 1)))) */;

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
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
