USE `ace_world`;

ALTER TABLE `ace_object_inventory` 
ADD COLUMN `shade` FLOAT NOT NULL DEFAULT '1' AFTER `palette`,
ADD COLUMN `tryToBond` TINYINT(4) NOT NULL DEFAULT '0' AFTER `shade`;
