INSERT INTO biota_properties_int (object_Id, `type`, value)
SELECT biota.id, 28, 0
FROM biota
LEFT JOIN biota_properties_int bint ON biota.id=bint.object_Id and bint.`type`=28
WHERE biota.weenie_Type=2 AND bint.value is null;
