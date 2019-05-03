USE `ace_shard`;

UPDATE biota
INNER JOIN biota_properties_int on biota.id = biota_properties_int.object_Id
SET biota.weenie_Type = 1
WHERE biota.id > 0 AND biota.weenie_Type = 2 AND biota_properties_int.`type` = 1 AND biota_properties_int.`value` = 8;
