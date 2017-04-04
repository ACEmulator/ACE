/*
SQLyog Community v12.4.1 (64 bit)
MySQL - 5.7.17-log : Database - ace_world
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
  `baseAceObjectId` int(10) unsigned NOT NULL,
  `weenieClassId` smallint(5) unsigned NOT NULL,
  `landblock` smallint(5) unsigned DEFAULT NULL,
  `cell` smallint(5) unsigned DEFAULT NULL,
  `posX` float DEFAULT NULL,
  `posY` float DEFAULT NULL,
  `posZ` float DEFAULT NULL,
  `qW` float DEFAULT NULL,
  `qX` float DEFAULT NULL,
  `qY` float DEFAULT NULL,
  `qZ` float DEFAULT NULL,
  PRIMARY KEY (`baseAceObjectId`),
  KEY `FK_ace_object__weenieClassId` (`weenieClassId`),
  CONSTRAINT `FK_ace_object__baseAceObjectId` FOREIGN KEY (`baseAceObjectId`) REFERENCES `base_ace_object` (`baseAceObjectId`),
  CONSTRAINT `FK_ace_object__weenieClassId` FOREIGN KEY (`weenieClassId`) REFERENCES `weenie_class` (`weenieClassId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_animation_changes` */

DROP TABLE IF EXISTS `ace_object_animation_changes`;

CREATE TABLE `ace_object_animation_changes` (
  `baseAceObjectId` int(10) unsigned NOT NULL,
  `index` tinyint(3) unsigned NOT NULL,
  `animationId` int(10) unsigned NOT NULL,
  PRIMARY KEY (`baseAceObjectId`,`index`),
  CONSTRAINT `FK_ace_object_animation_changes__baseAceObjectId` FOREIGN KEY (`baseAceObjectId`) REFERENCES `ace_object` (`baseAceObjectId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_palette_changes` */

DROP TABLE IF EXISTS `ace_object_palette_changes`;

CREATE TABLE `ace_object_palette_changes` (
  `baseAceObjectId` int(10) unsigned NOT NULL,
  `subPaletteId` int(10) unsigned NOT NULL,
  `offset` smallint(5) unsigned NOT NULL,
  `length` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`baseAceObjectId`,`subPaletteId`),
  CONSTRAINT `FK_ace_object_palette_data__baseAceObjectId` FOREIGN KEY (`baseAceObjectId`) REFERENCES `ace_object` (`baseAceObjectId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `ace_object_texture_map_changes` */

DROP TABLE IF EXISTS `ace_object_texture_map_changes`;

CREATE TABLE `ace_object_texture_map_changes` (
  `baseAceObjectId` int(10) unsigned NOT NULL,
  `index` tinyint(3) unsigned NOT NULL,
  `oldId` int(10) unsigned NOT NULL,
  `newId` int(10) unsigned NOT NULL,
  PRIMARY KEY (`baseAceObjectId`,`index`),
  CONSTRAINT `FK_ace_object_texture_map_changes__baseAceObjectId` FOREIGN KEY (`baseAceObjectId`) REFERENCES `ace_object` (`baseAceObjectId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `base_ace_object` */

DROP TABLE IF EXISTS `base_ace_object`;

CREATE TABLE `base_ace_object` (
  `baseAceObjectId` int(10) unsigned NOT NULL,
  `name` text NOT NULL,
  `typeId` int(10) unsigned NOT NULL,
  `paletteId` int(10) unsigned NOT NULL DEFAULT '0',
  `ammoType` int(10) unsigned NOT NULL DEFAULT '0',
  `blipColor` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `bitField` int(10) unsigned NOT NULL DEFAULT '0',
  `burden` int(10) unsigned NOT NULL DEFAULT '0',
  `combatUse` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `cooldownDuration` double NOT NULL DEFAULT '0',
  `cooldownId` int(10) unsigned NOT NULL DEFAULT '0',
  `effects` int(10) unsigned NOT NULL DEFAULT '0',
  `containersCapacity` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `header` int(10) unsigned NOT NULL DEFAULT '0',
  `hookTypeId` int(10) unsigned NOT NULL DEFAULT '0',
  `iconId` int(10) unsigned NOT NULL DEFAULT '0',
  `iconOverlayId` int(10) unsigned NOT NULL DEFAULT '0',
  `iconUnderlayId` int(10) unsigned NOT NULL DEFAULT '0',
  `hookItemTypes` int(10) unsigned NOT NULL DEFAULT '0',
  `itemsCapacity` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `location` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `materialType` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `maxStackSize` smallint(5) unsigned NOT NULL DEFAULT '0',
  `maxStructure` smallint(5) unsigned NOT NULL DEFAULT '0',
  `radar` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `pscript` smallint(5) unsigned NOT NULL DEFAULT '0',
  `spellId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `stackSize` smallint(5) unsigned NOT NULL DEFAULT '0',
  `structure` smallint(5) unsigned NOT NULL DEFAULT '0',
  `targetTypeId` int(10) unsigned NOT NULL DEFAULT '0',
  `usability` int(10) unsigned NOT NULL DEFAULT '0',
  `useRadius` float NOT NULL DEFAULT '0',
  `validLocations` int(10) unsigned NOT NULL DEFAULT '0',
  `value` int(10) unsigned NOT NULL DEFAULT '0',
  `workmanship` float NOT NULL DEFAULT '0',
  `animationFrameId` int(10) unsigned NOT NULL DEFAULT '0',
  `defaultScript` int(10) unsigned NOT NULL DEFAULT '0',
  `defaultScriptIntensity` float NOT NULL DEFAULT '0',
  `elasticity` float NOT NULL DEFAULT '0',
  `friction` float NOT NULL DEFAULT '0',
  `locationId` int(10) unsigned NOT NULL DEFAULT '0',
  `modelTableId` int(10) unsigned NOT NULL DEFAULT '0',
  `motionTableId` int(10) unsigned NOT NULL DEFAULT '0',
  `objectScale` float NOT NULL DEFAULT '0',
  `physicsBitField` int(10) unsigned NOT NULL DEFAULT '0',
  `physicsState` int(10) unsigned NOT NULL DEFAULT '0',
  `physicsTableId` int(10) unsigned NOT NULL DEFAULT '0',
  `soundTableId` int(10) unsigned NOT NULL DEFAULT '0',
  `translucency` float NOT NULL DEFAULT '0',
  PRIMARY KEY (`baseAceObjectId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `teleport_location` */

DROP TABLE IF EXISTS `teleport_location`;

CREATE TABLE `teleport_location` (
  `location` varchar(100) NOT NULL DEFAULT '',
  `cell` int(10) unsigned NOT NULL DEFAULT '0',
  `x` float NOT NULL DEFAULT '0',
  `y` float NOT NULL DEFAULT '0',
  `z` float NOT NULL DEFAULT '0',
  `qx` float NOT NULL DEFAULT '0',
  `qy` float NOT NULL DEFAULT '0',
  `qz` float NOT NULL DEFAULT '0',
  `qw` float NOT NULL DEFAULT '0',
  PRIMARY KEY (`location`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `weenie_animation_changes` */

DROP TABLE IF EXISTS `weenie_animation_changes`;

CREATE TABLE `weenie_animation_changes` (
  `weenieClassId` smallint(5) unsigned NOT NULL,
  `index` tinyint(3) unsigned NOT NULL,
  `animationId` int(10) unsigned NOT NULL,
  PRIMARY KEY (`weenieClassId`,`index`),
  CONSTRAINT `FK_weenie_animation_changes__weenieClassId` FOREIGN KEY (`weenieClassId`) REFERENCES `weenie_class` (`weenieClassId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `weenie_class` */

DROP TABLE IF EXISTS `weenie_class`;

CREATE TABLE `weenie_class` (
  `weenieClassId` smallint(5) unsigned NOT NULL,
  `baseAceObjectId` int(10) unsigned NOT NULL,
  PRIMARY KEY (`weenieClassId`),
  KEY `FK_weenie_class__baseAceObjectId` (`baseAceObjectId`),
  CONSTRAINT `FK_weenie_class__baseAceObjectId` FOREIGN KEY (`baseAceObjectId`) REFERENCES `base_ace_object` (`baseAceObjectId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `weenie_creature_data` */

DROP TABLE IF EXISTS `weenie_creature_data`;

CREATE TABLE `weenie_creature_data` (
  `weenieClassId` smallint(5) unsigned NOT NULL,
  `level` int(10) unsigned NOT NULL,
  `strength` int(10) unsigned NOT NULL,
  `endurance` int(10) unsigned NOT NULL,
  `coordination` int(10) unsigned NOT NULL,
  `quickness` int(10) unsigned NOT NULL,
  `focus` int(10) unsigned NOT NULL,
  `self` int(10) unsigned NOT NULL,
  `health` int(10) unsigned NOT NULL,
  `stamina` int(10) unsigned NOT NULL,
  `mana` int(10) unsigned NOT NULL,
  `baseExperience` int(10) unsigned NOT NULL,
  `luminance` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `lootTier` tinyint(3) unsigned NOT NULL DEFAULT '1',
  PRIMARY KEY (`weenieClassId`),
  CONSTRAINT `FK_weenie_creature_data__weenieClassId` FOREIGN KEY (`weenieClassId`) REFERENCES `weenie_class` (`weenieClassId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `weenie_palette_changes` */

DROP TABLE IF EXISTS `weenie_palette_changes`;

CREATE TABLE `weenie_palette_changes` (
  `weenieClassId` smallint(5) unsigned NOT NULL,
  `subPaletteId` int(10) unsigned NOT NULL,
  `offset` smallint(5) unsigned NOT NULL,
  `length` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`weenieClassId`,`subPaletteId`),
  CONSTRAINT `FK_weenie_palette_data__weenieClassId` FOREIGN KEY (`weenieClassId`) REFERENCES `weenie_class` (`weenieClassId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `weenie_texture_map_changes` */

DROP TABLE IF EXISTS `weenie_texture_map_changes`;

CREATE TABLE `weenie_texture_map_changes` (
  `weenieClassId` smallint(5) unsigned NOT NULL,
  `index` tinyint(3) unsigned NOT NULL,
  `oldId` int(10) unsigned NOT NULL,
  `newId` int(10) unsigned NOT NULL,
  PRIMARY KEY (`weenieClassId`,`index`),
  CONSTRAINT `FK_weenie_texture_map_changes__weenieClassId` FOREIGN KEY (`weenieClassId`) REFERENCES `weenie_class` (`weenieClassId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `vw_ace_object` */

DROP TABLE IF EXISTS `vw_ace_object`;

/*!50001 DROP VIEW IF EXISTS `vw_ace_object` */;
/*!50001 DROP TABLE IF EXISTS `vw_ace_object` */;

/*!50001 CREATE TABLE  `vw_ace_object`(
 `baseAceObjectId` int(10) unsigned ,
 `name` text ,
 `typeId` int(10) unsigned ,
 `paletteId` int(10) unsigned ,
 `ammoType` int(10) unsigned ,
 `blipColor` tinyint(3) unsigned ,
 `bitField` int(10) unsigned ,
 `burden` int(10) unsigned ,
 `combatUse` tinyint(3) unsigned ,
 `cooldownDuration` double ,
 `cooldownId` int(10) unsigned ,
 `effects` int(10) unsigned ,
 `containersCapacity` tinyint(3) unsigned ,
 `header` int(10) unsigned ,
 `hookTypeId` int(10) unsigned ,
 `iconId` int(10) unsigned ,
 `iconOverlayId` int(10) unsigned ,
 `iconUnderlayId` int(10) unsigned ,
 `hookItemTypes` int(10) unsigned ,
 `itemsCapacity` tinyint(3) unsigned ,
 `location` tinyint(3) unsigned ,
 `materialType` tinyint(3) unsigned ,
 `maxStackSize` smallint(5) unsigned ,
 `maxStructure` smallint(5) unsigned ,
 `radar` tinyint(3) unsigned ,
 `pscript` smallint(5) unsigned ,
 `spellId` smallint(5) unsigned ,
 `stackSize` smallint(5) unsigned ,
 `structure` smallint(5) unsigned ,
 `targetTypeId` int(10) unsigned ,
 `usability` int(10) unsigned ,
 `useRadius` float ,
 `validLocations` int(10) unsigned ,
 `value` int(10) unsigned ,
 `workmanship` float ,
 `animationFrameId` int(10) unsigned ,
 `defaultScript` int(10) unsigned ,
 `defaultScriptIntensity` float ,
 `elasticity` float ,
 `friction` float ,
 `locationId` int(10) unsigned ,
 `modelTableId` int(10) unsigned ,
 `motionTableId` int(10) unsigned ,
 `objectScale` float ,
 `physicsBitField` int(10) unsigned ,
 `physicsState` int(10) unsigned ,
 `physicsTableId` int(10) unsigned ,
 `soundTableId` int(10) unsigned ,
 `translucency` float ,
 `weenieClassId` smallint(5) unsigned ,
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

/*View structure for view vw_ace_object */

/*!50001 DROP TABLE IF EXISTS `vw_ace_object` */;
/*!50001 DROP VIEW IF EXISTS `vw_ace_object` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_ace_object` AS (select `bao`.`baseAceObjectId` AS `baseAceObjectId`,`bao`.`name` AS `name`,`bao`.`typeId` AS `typeId`,`bao`.`paletteId` AS `paletteId`,`bao`.`ammoType` AS `ammoType`,`bao`.`blipColor` AS `blipColor`,`bao`.`bitField` AS `bitField`,`bao`.`burden` AS `burden`,`bao`.`combatUse` AS `combatUse`,`bao`.`cooldownDuration` AS `cooldownDuration`,`bao`.`cooldownId` AS `cooldownId`,`bao`.`effects` AS `effects`,`bao`.`containersCapacity` AS `containersCapacity`,`bao`.`header` AS `header`,`bao`.`hookTypeId` AS `hookTypeId`,`bao`.`iconId` AS `iconId`,`bao`.`iconOverlayId` AS `iconOverlayId`,`bao`.`iconUnderlayId` AS `iconUnderlayId`,`bao`.`hookItemTypes` AS `hookItemTypes`,`bao`.`itemsCapacity` AS `itemsCapacity`,`bao`.`location` AS `location`,`bao`.`materialType` AS `materialType`,`bao`.`maxStackSize` AS `maxStackSize`,`bao`.`maxStructure` AS `maxStructure`,`bao`.`radar` AS `radar`,`bao`.`pscript` AS `pscript`,`bao`.`spellId` AS `spellId`,`bao`.`stackSize` AS `stackSize`,`bao`.`structure` AS `structure`,`bao`.`targetTypeId` AS `targetTypeId`,`bao`.`usability` AS `usability`,`bao`.`useRadius` AS `useRadius`,`bao`.`validLocations` AS `validLocations`,`bao`.`value` AS `value`,`bao`.`workmanship` AS `workmanship`,`bao`.`animationFrameId` AS `animationFrameId`,`bao`.`defaultScript` AS `defaultScript`,`bao`.`defaultScriptIntensity` AS `defaultScriptIntensity`,`bao`.`elasticity` AS `elasticity`,`bao`.`friction` AS `friction`,`bao`.`locationId` AS `locationId`,`bao`.`modelTableId` AS `modelTableId`,`bao`.`motionTableId` AS `motionTableId`,`bao`.`objectScale` AS `objectScale`,`bao`.`physicsBitField` AS `physicsBitField`,`bao`.`physicsState` AS `physicsState`,`bao`.`physicsTableId` AS `physicsTableId`,`bao`.`soundTableId` AS `soundTableId`,`bao`.`translucency` AS `translucency`,`ao`.`weenieClassId` AS `weenieClassId`,`ao`.`landblock` AS `landblock`,`ao`.`cell` AS `cell`,`ao`.`posX` AS `posX`,`ao`.`posY` AS `posY`,`ao`.`posZ` AS `posZ`,`ao`.`qW` AS `qW`,`ao`.`qX` AS `qX`,`ao`.`qY` AS `qY`,`ao`.`qZ` AS `qZ` from (`ace_object` `ao` join `base_ace_object` `bao` on((`ao`.`baseAceObjectId` = `bao`.`baseAceObjectId`)))) */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
