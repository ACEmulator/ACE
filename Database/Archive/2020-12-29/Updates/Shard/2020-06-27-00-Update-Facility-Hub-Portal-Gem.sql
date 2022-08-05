INSERT INTO biota_properties_d_i_d (object_Id, `type`, value)
SELECT biota.id, 27, 268435543 /* UseUserAnimation - Sanctuary */ FROM biota
LEFT JOIN biota_properties_d_i_d useUserAnimation ON useUserAnimation.object_Id=biota.id AND useUserAnimation.`type`=27
WHERE biota.weenie_Class_Id=49563 /* Facility Hub Portal Gem */ AND useUserAnimation.value IS NULL;
