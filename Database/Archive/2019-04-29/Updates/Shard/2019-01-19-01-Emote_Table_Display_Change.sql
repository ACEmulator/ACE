USE `ace_shard`;

ALTER TABLE `biota_properties_emote_action` 
CHANGE COLUMN `display` `display` BIT(1) NULL DEFAULT NULL ;
