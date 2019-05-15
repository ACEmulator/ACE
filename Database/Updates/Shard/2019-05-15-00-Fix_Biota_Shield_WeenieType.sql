USE `ace_shard`;

UPDATE biota
INNER JOIN biota_properties_int AS itemType ON biota.id=itemType.object_Id
INNER JOIN biota_properties_int AS validLocations ON biota.id=validLocations.object_Id 
INNER JOIN biota_properties_int AS combatUse ON biota.id=combatUse.object_Id 
SET biota.weenie_Type = 1
WHERE biota.id > 0
AND itemType.type = 1 and itemType.value = 2 
AND validLocations.type = 9 and validLocations.value = 2097152
AND combatUse.type = 51 and combatUse.value = 4
AND biota.weenie_Type <> 1
AND biota.weenie_Type <> 40;
