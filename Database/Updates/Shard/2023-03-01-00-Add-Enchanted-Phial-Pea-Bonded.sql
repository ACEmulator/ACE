INSERT INTO biota_properties_int
SELECT biota.id, 33 /* Bonded */, 1 /* Bonded */ FROM biota
LEFT JOIN biota_properties_int bint ON bint.object_Id=biota.id AND bint.`type`=33 /* Bonded */
WHERE biota.weenie_Class_Id=36732 /* Enchanted Platinum Phial Pea */ AND bint.value IS NULL;
