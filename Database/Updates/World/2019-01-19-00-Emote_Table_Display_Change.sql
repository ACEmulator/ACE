USE `ace_world`;

ALTER TABLE `weenie_properties_emote_action` 
CHANGE COLUMN `display` `display` BIT(1) NULL DEFAULT NULL ;
