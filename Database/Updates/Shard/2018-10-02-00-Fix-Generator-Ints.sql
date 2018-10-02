USE `ace_shard`;

ALTER TABLE `biota_properties_generator` 
CHANGE COLUMN `init_Create` `init_Create` INT(10) NOT NULL DEFAULT '1' COMMENT 'Number of object to generate initially' ,
CHANGE COLUMN `max_Create` `max_Create` INT(10) NOT NULL DEFAULT '1' COMMENT 'Maximum amount of objects to generate' ;
