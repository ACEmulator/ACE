INSERT INTO biota_properties_int
SELECT biota.id, 19 /* Value */, 0 FROM biota
LEFT JOIN biota_properties_int bint ON bint.object_Id=biota.id AND bint.`type`= 19 /* Value */
WHERE biota.weenie_Class_Id = 45728 /* Geraine's Tome (2) */ AND bint.value IS NULL;

INSERT INTO biota_properties_int
SELECT biota.id, 33 /* Bonded */, 1 /* Bonded */ FROM biota
LEFT JOIN biota_properties_int bint ON bint.object_Id=biota.id AND bint.`type`= 33 /* Bonded */
WHERE biota.weenie_Class_Id = 45728 /* Geraine's Tome (2) */ AND bint.value IS NULL;

INSERT INTO biota_properties_int
SELECT biota.id, 114 /* Attuned */, 1 /* Attuned */ FROM biota
LEFT JOIN biota_properties_int bint ON bint.object_Id=biota.id AND bint.`type`= 114 /* Attuned */
WHERE biota.weenie_Class_Id = 45728 /* Geraine's Tome (2) */ AND bint.value IS NULL;

INSERT INTO biota_properties_int
SELECT biota.id, 19 /* Value */, 0 FROM biota
LEFT JOIN biota_properties_int bint ON bint.object_Id=biota.id AND bint.`type`= 19 /* Value */
WHERE biota.weenie_Class_Id = 45729 /* Geraine's Tome (5) */ AND bint.value IS NULL;

INSERT INTO biota_properties_int
SELECT biota.id, 33 /* Bonded */, 1 /* Bonded */ FROM biota
LEFT JOIN biota_properties_int bint ON bint.object_Id=biota.id AND bint.`type`= 33 /* Bonded */
WHERE biota.weenie_Class_Id = 45729 /* Geraine's Tome (5) */ AND bint.value IS NULL;

INSERT INTO biota_properties_int
SELECT biota.id, 114 /* Attuned */, 1 /* Attuned */ FROM biota
LEFT JOIN biota_properties_int bint ON bint.object_Id=biota.id AND bint.`type`= 114 /* Attuned */
WHERE biota.weenie_Class_Id = 45729 /* Geraine's Tome (5) */ AND bint.value IS NULL;

INSERT INTO biota_properties_int
SELECT biota.id, 19 /* Value */, 0 FROM biota
LEFT JOIN biota_properties_int bint ON bint.object_Id=biota.id AND bint.`type`= 19 /* Value */
WHERE biota.weenie_Class_Id = 45730 /* Geraine's Tome (4) */ AND bint.value IS NULL;

INSERT INTO biota_properties_int
SELECT biota.id, 33 /* Bonded */, 1 /* Bonded */ FROM biota
LEFT JOIN biota_properties_int bint ON bint.object_Id=biota.id AND bint.`type`= 33 /* Bonded */
WHERE biota.weenie_Class_Id = 45730 /* Geraine's Tome (4) */ AND bint.value IS NULL;

INSERT INTO biota_properties_int
SELECT biota.id, 114 /* Attuned */, 1 /* Attuned */ FROM biota
LEFT JOIN biota_properties_int bint ON bint.object_Id=biota.id AND bint.`type`= 114 /* Attuned */
WHERE biota.weenie_Class_Id = 45730 /* Geraine's Tome (4) */ AND bint.value IS NULL;

INSERT INTO biota_properties_int
SELECT biota.id, 19 /* Value */, 0 FROM biota
LEFT JOIN biota_properties_int bint ON bint.object_Id=biota.id AND bint.`type`= 19 /* Value */
WHERE biota.weenie_Class_Id = 45732 /* Geraine's Tome (7) */ AND bint.value IS NULL;

INSERT INTO biota_properties_int
SELECT biota.id, 33 /* Bonded */, 1 /* Bonded */ FROM biota
LEFT JOIN biota_properties_int bint ON bint.object_Id=biota.id AND bint.`type`= 33 /* Bonded */
WHERE biota.weenie_Class_Id = 45732 /* Geraine's Tome (7) */ AND bint.value IS NULL;

INSERT INTO biota_properties_int
SELECT biota.id, 114 /* Attuned */, 1 /* Attuned */ FROM biota
LEFT JOIN biota_properties_int bint ON bint.object_Id=biota.id AND bint.`type`= 114 /* Attuned */
WHERE biota.weenie_Class_Id = 45732 /* Geraine's Tome (7) */ AND bint.value IS NULL;

INSERT INTO biota_properties_int
SELECT biota.id, 19 /* Value */, 0 FROM biota
LEFT JOIN biota_properties_int bint ON bint.object_Id=biota.id AND bint.`type`= 19 /* Value */
WHERE biota.weenie_Class_Id = 45734 /* Geraine's Tome (3) */ AND bint.value IS NULL;

INSERT INTO biota_properties_int
SELECT biota.id, 33 /* Bonded */, 1 /* Bonded */ FROM biota
LEFT JOIN biota_properties_int bint ON bint.object_Id=biota.id AND bint.`type`= 33 /* Bonded */
WHERE biota.weenie_Class_Id = 45734 /* Geraine's Tome (3) */ AND bint.value IS NULL;

INSERT INTO biota_properties_int
SELECT biota.id, 114 /* Attuned */, 1 /* Attuned */ FROM biota
LEFT JOIN biota_properties_int bint ON bint.object_Id=biota.id AND bint.`type`= 114 /* Attuned */
WHERE biota.weenie_Class_Id = 45734 /* Geraine's Tome (3) */ AND bint.value IS NULL;
