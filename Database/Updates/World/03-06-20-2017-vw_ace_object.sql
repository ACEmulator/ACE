DROP VIEW IF EXISTS `vw_ace_object`;
DELIMITER $$
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_ace_object` AS 
SELECT
   `ao`.`aceObjectId`                 AS `aceObjectId`
   , `aops`.`propertyValue`           AS `name`
   , `ao`.`weenieClassId`             AS `weenieClassId`
   , `ao`.`currentMotionState`        AS `currentMotionState`
   , `awc`.`weenieClassDescription`   AS `weenieClassDescription`
   , `ao`.`aceObjectDescriptionFlags` AS `aceObjectDescriptionFlags`
   , `ao`.`physicsDescriptionFlag`    AS `physicsDescriptionFlag`
   , `ao`.`weenieHeaderFlags`         AS `weenieHeaderFlags`
   , `aopi`.`propertyValue`           AS `itemType`
   , `ap`.`positionId`                AS `positionId`
   , `ap`.`positionType`              AS `positionType`
   , `ap`.`landblockRaw`              AS `LandblockRaw`
   , `ap`.`landblock`                 AS `landblock`   
   , `ap`.`posX`                      AS `posX`
   , `ap`.`posY`                      AS `posY`
   , `ap`.`posZ`                      AS `posZ`
   , `ap`.`qW`                        AS `qW`
   , `ap`.`qX`                        AS `qX`
   , `ap`.`qY`                        AS `qY`
   , `ap`.`qZ`                        AS `qZ`
FROM ((((`ace_object` `ao`
       JOIN `ace_weenie_class` `awc`
         ON ((`ao`.`weenieClassId` = `awc`.`weenieClassId`)))
      JOIN `ace_object_properties_string` `aops`
        ON (((`ao`.`aceObjectId` = `aops`.`aceObjectId`)
             AND (`aops`.`strPropertyId` = 1))))
     JOIN `ace_object_properties_int` `aopi`
       ON (((`ao`.`aceObjectId` = `aopi`.`aceObjectId`)
            AND (`aopi`.`intPropertyId` = 1))))
    JOIN `ace_position` `ap`
      ON (((`ao`.`aceObjectId` = `ap`.`aceObjectId`)
           AND (`ap`.`positionType` = 1))))$$

DELIMITER ;