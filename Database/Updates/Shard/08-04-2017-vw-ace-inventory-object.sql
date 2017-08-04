/*
SQLyog Ultimate
MySQL - 5.7.17-log : Database - ace_shard
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

USE `ace_shard`;

/*Table structure for table `vw_ace_inventory_object` */

DROP TABLE IF EXISTS `vw_ace_inventory_object`;

/*!50001 DROP VIEW IF EXISTS `vw_ace_inventory_object` */;
/*!50001 DROP TABLE IF EXISTS `vw_ace_inventory_object` */;

/*!50001 CREATE TABLE  `vw_ace_inventory_object`(
 `containerId` int(10) unsigned ,
 `aceObjectId` int(10) unsigned ,
 `placement` int(10) unsigned 
)*/;

/*View structure for view vw_ace_inventory_object */

/*!50001 DROP TABLE IF EXISTS `vw_ace_inventory_object` */;
/*!50001 DROP VIEW IF EXISTS `vw_ace_inventory_object` */;

/*!50001 CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW `vw_ace_inventory_object` AS (select `aopiid`.`propertyValue` AS `containerId`,`aopiid`.`aceObjectId` AS `aceObjectId`,`aopi`.`propertyValue` AS `placement` from (`ace_object_properties_iid` `aopiid` join `ace_object_properties_int` `aopi` on(((`aopiid`.`aceObjectId` = `aopi`.`aceObjectId`) and (`aopi`.`intPropertyId` = 65)))) where (`aopiid`.`iidPropertyId` = 2) order by `aopiid`.`propertyValue`,`aopi`.`propertyValue`) */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
