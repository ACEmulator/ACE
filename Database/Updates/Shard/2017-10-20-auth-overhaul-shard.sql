
ALTER TABLE `ace_object`   
  ADD COLUMN `userModified` TINYINT(1) DEFAULT 0 NOT NULL COMMENT 'flag indicating whether or not this has record has been altered since deployment' AFTER `weenieClassId`;

DROP VIEW IF EXISTS `vw_ace_character`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `vw_ace_character` AS 
SELECT
  `ao`.`aceObjectId`               AS `guid`,
  `aopiidacc`.`propertyValue`      AS `subscriptionId`,
  `aops`.`propertyValue`           AS `NAME`,
  `aopb`.`propertyValue`           AS `deleted`,
  `aopbi`.`propertyValue`          AS `deleteTime`,
  `ao`.`weenieClassId`             AS `weenieClassId`,
  `awc`.`weenieClassDescription`   AS `weenieClassDescription`,
  `ao`.`aceObjectDescriptionFlags` AS `aceObjectDescriptionFlags`,
  `ao`.`physicsDescriptionFlag`    AS `physicsDescriptionFlag`,
  `ao`.`weenieHeaderFlags`         AS `weenieHeaderFlags`,
  `aopi`.`propertyValue`           AS `itemType`,
  `aopd`.`propertyValue`           AS `loginTimestamp`
FROM (((((((`ace_object` `ao`
         JOIN `ace_weenie_class` `awc`
           ON (`ao`.`weenieClassId` = `awc`.`weenieClassId`))
        JOIN `ace_object_properties_string` `aops`
          ON (`ao`.`aceObjectId` = `aops`.`aceObjectId`
              AND `aops`.`strPropertyId` = 1))
       JOIN `ace_object_properties_bool` `aopb`
         ON (`ao`.`aceObjectId` = `aopb`.`aceObjectId`
             AND `aopb`.`boolPropertyId` = 9001))
      JOIN `ace_object_properties_int` `aopi`
        ON (`ao`.`aceObjectId` = `aopi`.`aceObjectId`
            AND `aopi`.`intPropertyId` = 1))
     JOIN `ace_object_properties_bigint` `aopbi`
       ON (`ao`.`aceObjectId` = `aopbi`.`aceObjectId`
           AND `aopbi`.`bigIntPropertyId` = 9001))
    JOIN `ace_object_properties_iid` `aopiidacc`
      ON (`ao`.`aceObjectId` = `aopiidacc`.`aceObjectId`
          AND `aopiidacc`.`iidPropertyId` = 9001))
   LEFT JOIN `ace_object_properties_double` `aopd`
     ON (`ao`.`aceObjectId` = `aopd`.`aceObjectId`
         AND `aopd`.`dblPropertyId` = 48));
