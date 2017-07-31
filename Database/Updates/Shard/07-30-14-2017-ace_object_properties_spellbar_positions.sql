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
USE `ace_shard`;

/*Table structure for table `ace_object_properties_spellbar_positions` */

DROP TABLE IF EXISTS `ace_object_properties_spellbar_positions`;

CREATE TABLE `ace_object_properties_spellbar_positions` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT 0,
  `spellId` int(10) unsigned NOT NULL DEFAULT 0,
  `spellBarId` int(10) unsigned NOT NULL DEFAULT 0,
  `spellBarPositionId` int(10) unsigned NOT NULL DEFAULT 0,
  PRIMARY KEY (`aceObjectId`,`spellId`,`spellBarId`),
  CONSTRAINT `fk_sb_ao` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
