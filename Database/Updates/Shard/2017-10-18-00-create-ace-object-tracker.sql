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

/*Table structure for table `ace_contract_tracker` */

DROP TABLE IF EXISTS `ace_contract_tracker`;

CREATE TABLE `ace_contract_tracker` (
  `aceObjectId` int(10) unsigned NOT NULL,
  `contractId` int(10) unsigned NOT NULL,
  `version` int(10) unsigned NOT NULL,
  `stage` int(10) unsigned NOT NULL,
  `timeWhenDone` bigint(20) unsigned NOT NULL,
  `timeWhenRepeats` bigint(20) unsigned NOT NULL,
  `deleteContract` int(10) unsigned NOT NULL,
  `setAsDisplayContract` int(10) unsigned NOT NULL,
  PRIMARY KEY (`aceObjectId`,`contractId`),
  KEY `ace_contract_aceObject` (`aceObjectId`),
  CONSTRAINT `fk_contract_ace_object` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
