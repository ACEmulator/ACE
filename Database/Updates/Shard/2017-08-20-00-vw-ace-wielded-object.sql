use ace_shard;
DELIMITER $$

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_ace_wielded_object` AS (
SELECT
 `aopiid`.`propertyValue`   AS `wielderId`
  , `aopiid`.`aceObjectId`   AS `aceObjectId`
  , `aopi`.`propertyValue`   AS `wieldedLocation`
FROM (`ace_object_properties_iid` `aopiid`
   JOIN `ace_object_properties_int` `aopi`
     ON (`aopiid`.`aceObjectId` = `aopi`.`aceObjectId`
         AND `aopi`.`intPropertyId` = 10))
WHERE `aopiid`.`iidPropertyId` = 3
ORDER BY `aopiid`.`propertyValue`,`aopi`.`propertyValue`)$$

DELIMITER ;
