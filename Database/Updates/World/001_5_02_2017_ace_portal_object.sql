SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `ace_world`
--
USE `ace_world`;

DROP TABLE IF EXISTS `portal_destination`;
DROP TABLE IF EXISTS `ace_portal_object`;

-- --------------------------------------------------------

--
-- Table structure for table `ace_portal_object`
--

CREATE TABLE `ace_portal_object` (
  `weenieClassId` smallint(5) UNSIGNED NOT NULL,
  `destLandblockId` int(10) UNSIGNED NOT NULL DEFAULT '0',
  `destX` float NOT NULL DEFAULT '0',
  `destY` float NOT NULL DEFAULT '0',
  `destZ` float NOT NULL DEFAULT '0',
  `destQX` float NOT NULL DEFAULT '0',
  `destQY` float NOT NULL DEFAULT '0',
  `destQZ` float NOT NULL DEFAULT '0',
  `destQW` float NOT NULL DEFAULT '0',
  `min_lvl` int(11) UNSIGNED NOT NULL DEFAULT '0',
  `max_lvl` int(11) UNSIGNED NOT NULL DEFAULT '0',
  `societyId` tinyint(3) UNSIGNED NOT NULL DEFAULT '0',
  `isTieable` tinyint(1) UNSIGNED NOT NULL DEFAULT '1',
  `isRecallable` tinyint(1) UNSIGNED NOT NULL DEFAULT '1',
  `isSummonable` tinyint(1) UNSIGNED NOT NULL DEFAULT '1'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `ace_portal_object`
--
ALTER TABLE `ace_portal_object`
  ADD PRIMARY KEY (`weenieClassId`);

--
-- Constraints for dumped tables
--

--
-- Constraints for table `ace_portal_object`
--
ALTER TABLE `ace_portal_object`
  ADD CONSTRAINT `FK_weenieClassId` FOREIGN KEY (`weenieClassId`) REFERENCES `ace_object` (`weenieClassId`);

--
-- View structure for view `vw_ace_portal_object`
--
/*!50001 DROP TABLE IF EXISTS `vw_ace_portal_object` */;
/*!50001 DROP VIEW IF EXISTS `vw_ace_portal_object` */;

/*!50001 CREATE VIEW `vw_ace_portal_object`
 AS (select `ao`.`baseAceObjectId` AS `baseAceObjectId`,
            `ao`.`name` AS `name`,
			`ao`.`typeId` AS `typeId`,
			`ao`.`paletteId` AS `paletteId`,
			`ao`.`ammoType` AS `ammoType`,
			`ao`.`blipColor` AS `blipColor`,
			`ao`.`bitField` AS `bitField`,
			`ao`.`burden` AS `burden`,
			`ao`.`combatUse` AS `combatUse`,
			`ao`.`cooldownDuration` AS `cooldownDuration`,
			`ao`.`cooldownId` AS `cooldownId`,
			`ao`.`effects` AS `effects`,
			`ao`.`containersCapacity` AS `containersCapacity`,
			`ao`.`header` AS `header`,
			`ao`.`hookTypeId` AS `hookTypeId`,
			`ao`.`iconId` AS `iconId`,
			`ao`.`iconOverlayId` AS `iconOverlayId`,
			`ao`.`iconUnderlayId` AS `iconUnderlayId`,
			`ao`.`hookItemTypes` AS `hookItemTypes`,
			`ao`.`itemsCapacity` AS `itemsCapacity`,
			`ao`.`location` AS `location`,
			`ao`.`materialType` AS `materialType`,
			`ao`.`maxStackSize` AS `maxStackSize`,
			`ao`.`maxStructure` AS `maxStructure`,
			`ao`.`radar` AS `radar`,
			`ao`.`pscript` AS `pscript`,
			`ao`.`spellId` AS `spellId`,
			`ao`.`stackSize` AS `stackSize`,
			`ao`.`structure` AS `structure`,
			`ao`.`targetTypeId` AS `targetTypeId`,
			`ao`.`usability` AS `usability`,
			`ao`.`useRadius` AS `useRadius`,
			`ao`.`validLocations` AS `validLocations`,
			`ao`.`value` AS `value`,
			`ao`.`workmanship` AS `workmanship`,
			`ao`.`animationFrameId` AS `animationFrameId`,
			`ao`.`defaultScript` AS `defaultScript`,
			`ao`.`defaultScriptIntensity` AS `defaultScriptIntensity`,
			`ao`.`elasticity` AS `elasticity`,
			`ao`.`friction` AS `friction`,
			`ao`.`locationId` AS `locationId`,
			`ao`.`modelTableId` AS `modelTableId`,
			`ao`.`motionTableId` AS `motionTableId`,
			`ao`.`objectScale` AS `objectScale`,
			`ao`.`physicsBitField` AS `physicsBitField`,
			`ao`.`physicsState` AS `physicsState`,
			`ao`.`physicsTableId` AS `physicsTableId`,
			`ao`.`soundTableId` AS `soundTableId`,
			`ao`.`translucency` AS `translucency`,
			`ao`.`currentMotionState` AS `currentMotionState`,
			`ao`.`weenieClassId` AS `weenieClassId`,
			`ao`.`landblock` AS `landblock`,
			`ao`.`cell` AS `cell`,
			`ao`.`posX` AS `posX`,
			`ao`.`posY` AS `posY`,
			`ao`.`posZ` AS `posZ`,
			`ao`.`qW` AS `qW`,
			`ao`.`qX` AS `qX`,
			`ao`.`qY` AS `qY`,
			`ao`.`qZ` AS `qZ`,
			`apo`.`destLandblockId` AS `destLandblockId`,
			`apo`.`destX` AS `destX`,
			`apo`.`destY` AS `destY`,
			`apo`.`destZ` AS `destZ`,
			`apo`.`destQX` AS `destQX`,
			`apo`.`destQY` AS `destQY`,
			`apo`.`destQZ` AS `destQZ`,
			`apo`.`destQW` AS `destQW`,
			`apo`.`min_lvl` AS `min_lvl`,
			`apo`.`max_lvl` AS `max_lvl`,
			`apo`.`societyId` AS `societyId`,
			`apo`.`isTieable` AS `isTieable`,
			`apo`.`isRecallable` AS `isRecallable`,
			`apo`.`isSummonable` AS `isSummonable`
 from (`vw_ace_object` `ao` left outer join `ace_portal_object` `apo` on((`ao`.`weenieClassId` = `apo`.`weenieClassId`))) where typeId = 65536) */;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
