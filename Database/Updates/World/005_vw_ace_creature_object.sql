/*
SQLyog Community v12.4.1 (64 bit)
MySQL - 10.1.21-MariaDB-1~xenial : Database - ace_world
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`ace_world` /*!40100 DEFAULT CHARACTER SET latin1 */;

USE `ace_world`;

/*Drop old static creature view */

DROP TABLE IF EXISTS `vw_ace_creature_static`;

/*!50001 DROP VIEW IF EXISTS `vw_ace_creature_static` */;
/*!50001 DROP TABLE IF EXISTS `vw_ace_creature_static` */;

/*Data for the table `weenie_creature_data` */

insert  into `weenie_creature_data`(`weenieClassId`,`level`,`strength`,`endurance`,`coordination`,`quickness`,`focus`,`self`,`health`,`stamina`,`mana`,`baseExperience`,`luminance`,`lootTier`) values 
(35440,8,20,30,25,30,25,15,20,80,15,1000,0,1),
(35441,8,20,30,50,55,25,15,25,80,15,1000,0,1),
(35442,8,30,35,50,35,30,15,33,85,15,1000,0,1);

/*Table structure for table `vw_ace_creature_object` */

DROP TABLE IF EXISTS `vw_ace_creature_object`;

/*!50001 DROP VIEW IF EXISTS `vw_ace_creature_object` */;
/*!50001 DROP TABLE IF EXISTS `vw_ace_creature_object` */;

/*!50001 CREATE TABLE  `vw_ace_creature_object`(
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
 `level` int(10) unsigned ,
 `strength` int(10) unsigned ,
 `endurance` int(10) unsigned ,
 `coordination` int(10) unsigned ,
 `quickness` int(10) unsigned ,
 `focus` int(10) unsigned ,
 `self` int(10) unsigned ,
 `health` int(10) unsigned ,
 `stamina` int(10) unsigned ,
 `mana` int(10) unsigned ,
 `baseExperience` int(10) unsigned ,
 `luminance` tinyint(3) unsigned ,
 `lootTier` tinyint(3) unsigned 
)*/;

/*View structure for view vw_ace_creature_object */

/*!50001 DROP TABLE IF EXISTS `vw_ace_creature_object` */;
/*!50001 DROP VIEW IF EXISTS `vw_ace_creature_object` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`%` SQL SECURITY DEFINER VIEW `vw_ace_creature_object` AS (select `BAO`.`baseAceObjectId` AS `baseAceObjectId`,`BAO`.`name` AS `name`,`BAO`.`typeId` AS `typeId`,`BAO`.`paletteId` AS `paletteId`,`BAO`.`ammoType` AS `ammoType`,`BAO`.`blipColor` AS `blipColor`,`BAO`.`bitField` AS `bitField`,`BAO`.`burden` AS `burden`,`BAO`.`combatUse` AS `combatUse`,`BAO`.`cooldownDuration` AS `cooldownDuration`,`BAO`.`cooldownId` AS `cooldownId`,`BAO`.`effects` AS `effects`,`BAO`.`containersCapacity` AS `containersCapacity`,`BAO`.`header` AS `header`,`BAO`.`hookTypeId` AS `hookTypeId`,`BAO`.`iconId` AS `iconId`,`BAO`.`iconOverlayId` AS `iconOverlayId`,`BAO`.`iconUnderlayId` AS `iconUnderlayId`,`BAO`.`hookItemTypes` AS `hookItemTypes`,`BAO`.`itemsCapacity` AS `itemsCapacity`,`BAO`.`location` AS `location`,`BAO`.`materialType` AS `materialType`,`BAO`.`maxStackSize` AS `maxStackSize`,`BAO`.`maxStructure` AS `maxStructure`,`BAO`.`radar` AS `radar`,`BAO`.`pscript` AS `pscript`,`BAO`.`spellId` AS `spellId`,`BAO`.`stackSize` AS `stackSize`,`BAO`.`structure` AS `structure`,`BAO`.`targetTypeId` AS `targetTypeId`,`BAO`.`usability` AS `usability`,`BAO`.`useRadius` AS `useRadius`,`BAO`.`validLocations` AS `validLocations`,`BAO`.`value` AS `value`,`BAO`.`workmanship` AS `workmanship`,`BAO`.`animationFrameId` AS `animationFrameId`,`BAO`.`defaultScript` AS `defaultScript`,`BAO`.`defaultScriptIntensity` AS `defaultScriptIntensity`,`BAO`.`elasticity` AS `elasticity`,`BAO`.`friction` AS `friction`,`BAO`.`locationId` AS `locationId`,`BAO`.`modelTableId` AS `modelTableId`,`BAO`.`motionTableId` AS `motionTableId`,`BAO`.`objectScale` AS `objectScale`,`BAO`.`physicsBitField` AS `physicsBitField`,`BAO`.`physicsState` AS `physicsState`,`BAO`.`physicsTableId` AS `physicsTableId`,`BAO`.`soundTableId` AS `soundTableId`,`BAO`.`translucency` AS `translucency`,`WCD`.`weenieClassId` AS `weenieClassId`,`WCD`.`level` AS `level`,`WCD`.`strength` AS `strength`,`WCD`.`endurance` AS `endurance`,`WCD`.`coordination` AS `coordination`,`WCD`.`quickness` AS `quickness`,`WCD`.`focus` AS `focus`,`WCD`.`self` AS `self`,`WCD`.`health` AS `health`,`WCD`.`stamina` AS `stamina`,`WCD`.`mana` AS `mana`,`WCD`.`baseExperience` AS `baseExperience`,`WCD`.`luminance` AS `luminance`,`WCD`.`lootTier` AS `lootTier` from ((`weenie_creature_data` `WCD` join `weenie_class` `WC` on((`WCD`.`weenieClassId` = `WC`.`weenieClassId`))) join `base_ace_object` `BAO` on((`WC`.`baseAceObjectId` = `BAO`.`baseAceObjectId`)))) */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
