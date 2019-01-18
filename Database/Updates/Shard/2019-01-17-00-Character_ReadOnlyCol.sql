USE `ace_shard`;

ALTER TABLE `ace_shard`.`character` 
ADD COLUMN `is_Read_Only` BIT(1) NOT NULL DEFAULT 0 AFTER `default_Hair_Texture`;
