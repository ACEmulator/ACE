USE `ace_world`;

ALTER TABLE `ace_object_inventory` 
CHANGE COLUMN `destinationType` `destinationType` INT(10) NOT NULL DEFAULT '0' ,
CHANGE COLUMN `palette` `palette` INT(10) NOT NULL DEFAULT '0' ,
CHANGE COLUMN `tryToBond` `tryToBond` INT(10) NULL DEFAULT '0' ;
