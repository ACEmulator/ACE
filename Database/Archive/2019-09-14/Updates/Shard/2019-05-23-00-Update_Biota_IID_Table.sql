USE `ace_shard`;

ALTER TABLE `biota_properties_i_i_d`
ADD INDEX `type_value_idx` (`type` ASC, `value` ASC);
;
