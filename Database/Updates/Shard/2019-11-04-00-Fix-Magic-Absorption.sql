INSERT INTO biota_properties_float (object_Id, `type`, value)
SELECT did.object_Id, 159, 0.25 FROM biota_properties_d_i_d did
WHERE did.`type`=50 and did.value=0x60030AD
ON DUPLICATE KEY UPDATE biota_properties_float.value=0.25;
