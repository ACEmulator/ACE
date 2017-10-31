
DROP TABLE IF EXISTS `ace_object_properties_skill`;
DROP VIEW IF EXISTS `vw_ace_inventory_object`;
DROP VIEW IF EXISTS `vw_ace_wielded_object`;

CREATE TABLE `ace_object_properties_skill` (
  `aceObjectId` INT(10) UNSIGNED NOT NULL,
  `skillId` TINYINT(1) UNSIGNED NOT NULL,
  `skillStatus` TINYINT(1) UNSIGNED NOT NULL,
  `skillPoints` SMALLINT(2) UNSIGNED NOT NULL DEFAULT 0,
  `skillXpSpent` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  PRIMARY KEY (`aceObjectId`,`skillId`),
  CONSTRAINT `fk_Prop_Skill_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8;

DELIMITER $$

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_ace_inventory_object` AS (
SELECT
  `aopiid`.`propertyValue` AS `containerId`,
  `aopiid`.`aceObjectId`   AS `aceObjectId`,
  `aopi`.`propertyValue`   AS `placement`
FROM (`ace_object_properties_iid` `aopiid`
   JOIN `ace_object_properties_int` `aopi`
     ON (`aopiid`.`aceObjectId` = `aopi`.`aceObjectId`
         AND `aopi`.`intPropertyId` = 65))
WHERE `aopiid`.`iidPropertyId` = 2
ORDER BY `aopiid`.`propertyValue`,`aopi`.`propertyValue`)$$

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_ace_wielded_object` AS (
SELECT
  `aopiid`.`propertyValue` AS `wielderId`,
  `aopiid`.`aceObjectId`   AS `aceObjectId`,
  `aopi`.`propertyValue`   AS `wieldedLocation`
FROM (`ace_object_properties_iid` `aopiid`
   JOIN `ace_object_properties_int` `aopi`
     ON (`aopiid`.`aceObjectId` = `aopi`.`aceObjectId`
         AND `aopi`.`intPropertyId` = 10))
WHERE `aopiid`.`iidPropertyId` = 3
ORDER BY `aopiid`.`propertyValue`,`aopi`.`propertyValue`)$$

DELIMITER ;