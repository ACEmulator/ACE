USE ace_shard;

ALTER TABLE `biota_properties_palette` 
ADD COLUMN `order` TINYINT(3) UNSIGNED NULL DEFAULT NULL AFTER `length`;