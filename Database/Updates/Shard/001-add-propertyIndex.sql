ALTER TABLE `ace_world`.`ace_object_properties_int`   
ADD COLUMN `propertyIndex` TINYINT(3) UNSIGNED DEFAULT 0 NOT NULL AFTER `intPropertyId`;

ALTER TABLE `ace_world`.`ace_object_properties_bigint`   
ADD COLUMN `propertyIndex` TINYINT(3) UNSIGNED DEFAULT 0 NOT NULL AFTER `bigIntPropertyId`;

ALTER TABLE `ace_world`.`ace_object_properties_bool`   
ADD COLUMN `propertyIndex` TINYINT(3) UNSIGNED DEFAULT 0 NOT NULL AFTER `boolPropertyId`;

ALTER TABLE `ace_world`.`ace_object_properties_did`   
ADD COLUMN `propertyIndex` TINYINT(3) UNSIGNED DEFAULT 0 NOT NULL AFTER `didPropertyId`;

ALTER TABLE `ace_world`.`ace_object_properties_double`   
ADD COLUMN `propertyIndex` TINYINT(3) UNSIGNED DEFAULT 0 NOT NULL AFTER `dblPropertyId`;

ALTER TABLE `ace_world`.`ace_object_properties_iid`   
ADD COLUMN `propertyIndex` TINYINT(3) UNSIGNED DEFAULT 0 NOT NULL AFTER `iidPropertyId`;

ALTER TABLE `ace_world`.`ace_object_properties_string`   
ADD COLUMN `propertyIndex` TINYINT(3) UNSIGNED DEFAULT 0 NOT NULL AFTER `strPropertyId`;


