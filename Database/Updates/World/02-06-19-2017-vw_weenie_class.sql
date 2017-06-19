DELIMITER $$

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_ace_weenie_class` AS 
SELECT
   `ao`.`aceObjectId`                 AS `aceObjectId`
   , `aops`.`propertyValue`           AS `name`
   , `ao`.`weenieClassId`             AS `weenieClassId`
   , `awc`.`weenieClassDescription`   AS `weenieClassDescription`
   , `aopi`.`propertyValue`           AS `itemType`
FROM ((((`ace_object` `ao`
       JOIN `ace_weenie_class` `awc`
         ON ((`ao`.`weenieClassId` = `awc`.`weenieClassId`)))
      JOIN `ace_object_properties_string` `aops`
        ON (((`ao`.`aceObjectId` = `aops`.`aceObjectId`)
             AND (`aops`.`strPropertyId` = 1))))
     JOIN `ace_object_properties_int` `aopi`
       ON (((`ao`.`aceObjectId` = `aopi`.`aceObjectId`)
            AND (`aopi`.`intPropertyId` = 1))))
    LEFT JOIN `ace_position` `ap`
      ON (((`ao`.`aceObjectId` = `ap`.`aceObjectId`)
           AND (`ap`.`positionType` = 1))))
WHERE ISNULL(`ap`.`aceObjectId`)$$

DELIMITER ;