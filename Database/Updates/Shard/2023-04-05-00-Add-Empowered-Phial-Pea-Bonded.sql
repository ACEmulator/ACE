INSERT INTO biota_properties_int
SELECT biota.id, 33 /* Bonded */, 1 /* Bonded */ FROM biota
LEFT JOIN biota_properties_int bint ON bint.object_Id=biota.id AND bint.`type`=33 /* Bonded */
WHERE biota.weenie_Class_Id=36729 /* Enchanted Copper Phial Pea */ AND bint.value IS NULL;

INSERT INTO biota_properties_int
SELECT biota.id, 33 /* Bonded */, 1 /* Bonded */ FROM biota
LEFT JOIN biota_properties_int bint ON bint.object_Id=biota.id AND bint.`type`=33 /* Bonded */
WHERE biota.weenie_Class_Id=36730 /* Enchanted Gold Phial Pea */ AND bint.value IS NULL;

INSERT INTO biota_properties_int
SELECT biota.id, 33 /* Bonded */, 1 /* Bonded */ FROM biota
LEFT JOIN biota_properties_int bint ON bint.object_Id=biota.id AND bint.`type`=33 /* Bonded */
WHERE biota.weenie_Class_Id=36731 /* Enchanted Iron Phial Pea */ AND bint.value IS NULL;

INSERT INTO biota_properties_int
SELECT biota.id, 33 /* Bonded */, 1 /* Bonded */ FROM biota
LEFT JOIN biota_properties_int bint ON bint.object_Id=biota.id AND bint.`type`=33 /* Bonded */
WHERE biota.weenie_Class_Id=36733 /* Empowered Platinum Phial Pea */ AND bint.value IS NULL;

INSERT INTO biota_properties_int
SELECT biota.id, 33 /* Bonded */, 1 /* Bonded */ FROM biota
LEFT JOIN biota_properties_int bint ON bint.object_Id=biota.id AND bint.`type`=33 /* Bonded */
WHERE biota.weenie_Class_Id=36734 /* Enchanted Pyreal Phial Pea */ AND bint.value IS NULL;

INSERT INTO biota_properties_int
SELECT biota.id, 33 /* Bonded */, 1 /* Bonded */ FROM biota
LEFT JOIN biota_properties_int bint ON bint.object_Id=biota.id AND bint.`type`=33 /* Bonded */
WHERE biota.weenie_Class_Id=36735 /* Enchanted Silver Phial Pea */ AND bint.value IS NULL;
