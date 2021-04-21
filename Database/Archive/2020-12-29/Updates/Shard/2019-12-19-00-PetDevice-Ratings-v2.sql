/*========================================*/
/* v1 (null) -> v2 */
/* adjusts to new ratings formula */
/*========================================*/

USE ace_shard;

UPDATE biota_properties_int bint
INNER JOIN biota ON biota.id=bint.object_Id AND biota.weenie_Type=70 /* PetDevice */
LEFT JOIN biota_properties_int version ON version.object_Id=bint.object_Id AND version.`type`=124
SET bint.value = ROUND(bint.value * 0.8)
WHERE version.value IS NULL and bint.`type` >= 370 /* GearDamage */ AND bint.`type` <= 375 /* GearCritDamageResist */;

INSERT INTO biota_properties_int (object_Id, `type`, value)
SELECT biota.id, 124, 2 FROM biota
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
WHERE biota.weenie_Type=70 /* PetDevice */ AND version.value IS NULL;
