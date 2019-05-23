USE `ace_shard`;

ALTER TABLE `biota_properties_i_i_d`
ADD COLUMN `type_value_combined` BIGINT(10) GENERATED ALWAYS AS ((`type` << 32) + `value`) VIRTUAL AFTER `value`/*,*/
/*ADD INDEX `type_value_combined_idx` (`type_value_combined` ASC);*/
;
