/*
SQLyog Community v12.4.1 (64 bit)
MySQL - 5.7.17-log : Database - ace_character
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`ace_character` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `ace_character`;

/*Table structure for table `character` */

DROP TABLE IF EXISTS `character`;

CREATE TABLE `character` (
  `guid` int(10) unsigned NOT NULL DEFAULT '0',
  `accountId` int(10) unsigned NOT NULL DEFAULT '0',
  `name` varchar(32) NOT NULL DEFAULT '',
  `templateOption` tinyint(1) unsigned NOT NULL,
  `startArea` tinyint(1) unsigned NOT NULL,
  `birth` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `deleteTime` bigint(20) unsigned NOT NULL DEFAULT '0',
  `deleted` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `totalLogins` int(10) unsigned NOT NULL DEFAULT '0',
  `characterOptions1` int(10) unsigned NOT NULL DEFAULT '0',
  `characterOptions2` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `character_appearance` */

DROP TABLE IF EXISTS `character_appearance`;

CREATE TABLE `character_appearance` (
  `id` int(10) unsigned NOT NULL,
  `eyes` tinyint(1) unsigned NOT NULL,
  `nose` tinyint(1) unsigned NOT NULL,
  `mouth` tinyint(1) unsigned NOT NULL,
  `eyeColor` tinyint(1) unsigned NOT NULL,
  `hairColor` tinyint(1) unsigned NOT NULL,
  `hairStyle` tinyint(1) unsigned NOT NULL,
  `hairHue` double NOT NULL,
  `skinHue` double NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `FK_character_appearance_id_character_guid` FOREIGN KEY (`id`) REFERENCES `character` (`guid`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `character_friends` */

DROP TABLE IF EXISTS `character_friends`;

CREATE TABLE `character_friends` (
  `id` int(10) unsigned NOT NULL,
  `friendId` int(10) unsigned NOT NULL,
  PRIMARY KEY (`id`,`friendId`),
  KEY `FK_character_friends_friendId_character_guid` (`friendId`),
  CONSTRAINT `FK_character_friends_friendId_character_guid` FOREIGN KEY (`friendId`) REFERENCES `character` (`guid`),
  CONSTRAINT `FK_character_friends_id_character_guid` FOREIGN KEY (`id`) REFERENCES `character` (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `character_position` */

DROP TABLE IF EXISTS `character_position`;

CREATE TABLE `character_position` (
  `character_id` int(10) unsigned NOT NULL,
  `positionType` tinyint(3) unsigned NOT NULL,
  `cell` int(10) unsigned NOT NULL DEFAULT '0',
  `positionX` float NOT NULL DEFAULT '0',
  `positionY` float NOT NULL DEFAULT '0',
  `positionZ` float NOT NULL DEFAULT '0',
  `rotationX` float NOT NULL DEFAULT '0',
  `rotationY` float NOT NULL DEFAULT '0',
  `rotationZ` float NOT NULL DEFAULT '0',
  `rotationW` float NOT NULL DEFAULT '0',
  PRIMARY KEY (`character_id`,`positionType`),
  CONSTRAINT `FK_character_position_id_character_guid` FOREIGN KEY (`character_id`) REFERENCES `character` (`guid`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `character_properties_bigint` */

DROP TABLE IF EXISTS `character_properties_bigint`;

CREATE TABLE `character_properties_bigint` (
  `guid` int(10) unsigned NOT NULL DEFAULT '0',
  `propertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyValue` bigint(20) unsigned NOT NULL DEFAULT '0',
  UNIQUE KEY `character_guid__property_id` (`guid`,`propertyId`),
  KEY `guid` (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `character_properties_bool` */

DROP TABLE IF EXISTS `character_properties_bool`;

CREATE TABLE `character_properties_bool` (
  `guid` int(10) unsigned NOT NULL DEFAULT '0',
  `propertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyValue` tinyint(1) NOT NULL DEFAULT '0',
  UNIQUE KEY `character_guid__property_id` (`guid`,`propertyId`),
  KEY `guid` (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `character_properties_double` */

DROP TABLE IF EXISTS `character_properties_double`;

CREATE TABLE `character_properties_double` (
  `guid` int(10) unsigned NOT NULL DEFAULT '0',
  `propertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyValue` double NOT NULL DEFAULT '0',
  UNIQUE KEY `character_guid__property_id` (`guid`,`propertyId`),
  KEY `guid` (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `character_properties_int` */

DROP TABLE IF EXISTS `character_properties_int`;

CREATE TABLE `character_properties_int` (
  `guid` int(10) unsigned NOT NULL DEFAULT '0',
  `propertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyValue` int(10) unsigned NOT NULL DEFAULT '0',
  UNIQUE KEY `character_guid__property_id` (`guid`,`propertyId`),
  KEY `guid` (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `character_properties_string` */

DROP TABLE IF EXISTS `character_properties_string`;

CREATE TABLE `character_properties_string` (
  `guid` int(10) unsigned NOT NULL DEFAULT '0',
  `propertyId` smallint(5) unsigned NOT NULL DEFAULT '0',
  `propertyValue` text NOT NULL,
  UNIQUE KEY `character_guid__property_id` (`guid`,`propertyId`),
  KEY `guid` (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `character_skills` */

DROP TABLE IF EXISTS `character_skills`;

CREATE TABLE `character_skills` (
  `id` int(10) unsigned NOT NULL,
  `skillId` tinyint(1) unsigned NOT NULL,
  `skillStatus` tinyint(1) unsigned NOT NULL,
  `skillPoints` smallint(2) unsigned NOT NULL DEFAULT '0',
  `skillXpSpent` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`,`skillId`),
  CONSTRAINT `FK_character_skills_id_character_guid` FOREIGN KEY (`id`) REFERENCES `character` (`guid`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `character_startup_gear` */

DROP TABLE IF EXISTS `character_startup_gear`;

CREATE TABLE `character_startup_gear` (
  `id` int(10) unsigned NOT NULL,
  `headgearStyle` int(10) unsigned NOT NULL,
  `headgearColor` tinyint(1) unsigned NOT NULL,
  `headgearHue` double NOT NULL,
  `shirtStyle` tinyint(1) unsigned NOT NULL,
  `shirtColor` tinyint(1) unsigned NOT NULL,
  `shirtHue` double NOT NULL,
  `pantsStyle` tinyint(1) unsigned NOT NULL,
  `pantsColor` tinyint(1) unsigned NOT NULL,
  `pantsHue` double NOT NULL,
  `footwearStyle` tinyint(1) unsigned NOT NULL,
  `footwearColor` tinyint(1) unsigned NOT NULL,
  `footwearHue` double NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `FK_character_startupgear_id_character_guid` FOREIGN KEY (`id`) REFERENCES `character` (`guid`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*Table structure for table `character_stats` */

DROP TABLE IF EXISTS `character_stats`;

CREATE TABLE `character_stats` (
  `id` int(10) unsigned NOT NULL,
  `strength` tinyint(1) unsigned NOT NULL,
  `strengthXpSpent` int(10) unsigned NOT NULL DEFAULT '0',
  `strengthRanks` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `endurance` tinyint(1) unsigned NOT NULL,
  `enduranceXpSpent` int(10) unsigned NOT NULL DEFAULT '0',
  `enduranceRanks` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `coordination` tinyint(1) unsigned NOT NULL,
  `coordinationXpSpent` int(10) unsigned NOT NULL DEFAULT '0',
  `coordinationRanks` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `quickness` tinyint(1) unsigned NOT NULL,
  `quicknessXpSpent` int(10) unsigned NOT NULL DEFAULT '0',
  `quicknessRanks` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `focus` tinyint(1) unsigned NOT NULL,
  `focusXpSpent` int(10) unsigned NOT NULL DEFAULT '0',
  `focusRanks` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `self` tinyint(1) unsigned NOT NULL,
  `selfXpSpent` int(10) unsigned NOT NULL DEFAULT '0',
  `healthXpSpent` int(10) unsigned NOT NULL DEFAULT '0',
  `staminaXpSpent` int(10) unsigned NOT NULL DEFAULT '0',
  `manaXpSpent` int(10) unsigned NOT NULL DEFAULT '0',
  `selfRanks` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `healthRanks` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `healthCurrent` smallint(2) unsigned NOT NULL DEFAULT '0',
  `staminaRanks` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `staminaCurrent` smallint(2) unsigned NOT NULL DEFAULT '0',
  `manaRanks` tinyint(2) unsigned NOT NULL DEFAULT '0',
  `manaCurrent` smallint(2) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  CONSTRAINT `FK_character_stats_id_character_guid` FOREIGN KEY (`id`) REFERENCES `character` (`guid`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*!50106 set global event_scheduler = 1*/;

/* Event structure for event `check_for_characters_to_delete` */

/*!50106 DROP EVENT IF EXISTS `check_for_characters_to_delete`*/;

DELIMITER $$

/*!50106 CREATE DEFINER=`root`@`localhost` EVENT `check_for_characters_to_delete` ON SCHEDULE EVERY 1 MINUTE STARTS '2017-02-23 10:03:22' ON COMPLETION NOT PRESERVE ENABLE DO BEGIN
		SET SQL_SAFE_UPDATES = 0;
		UPDATE `character` SET deleted = 1 WHERE deleted = 0 AND NOT deleteTime = 0 AND unix_timestamp() > deleteTime;
		SET SQL_SAFE_UPDATES = 1;
    END */$$
DELIMITER ;

/*Table structure for table `vw_character_positions` */

DROP TABLE IF EXISTS `vw_character_positions`;

/*!50001 DROP VIEW IF EXISTS `vw_character_positions` */;
/*!50001 DROP TABLE IF EXISTS `vw_character_positions` */;

/*!50001 CREATE TABLE  `vw_character_positions`(
 `character_id` int(10) unsigned ,
 `positionType` tinyint(3) unsigned ,
 `cell` int(10) unsigned ,
 `positionX` float ,
 `positionY` float ,
 `positionZ` float ,
 `rotationX` float ,
 `rotationY` float ,
 `rotationZ` float ,
 `rotationW` float 
)*/;

/*View structure for view vw_character_positions */

/*!50001 DROP TABLE IF EXISTS `vw_character_positions` */;
/*!50001 DROP VIEW IF EXISTS `vw_character_positions` */;

/*!50001 CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_character_positions` AS (select `cp`.`character_id` AS `character_id`,`cp`.`positionType` AS `positionType`,`cp`.`cell` AS `cell`,`cp`.`positionX` AS `positionX`,`cp`.`positionY` AS `positionY`,`cp`.`positionZ` AS `positionZ`,`cp`.`rotationX` AS `rotationX`,`cp`.`rotationY` AS `rotationY`,`cp`.`rotationZ` AS `rotationZ`,`cp`.`rotationW` AS `rotationW` from (`character_position` `cp` join `character` `ch` on((`cp`.`character_id` = `ch`.`guid`)))) */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
