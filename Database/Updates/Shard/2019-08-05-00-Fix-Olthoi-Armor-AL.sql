/* 37187 Olthoi Alduressa Gauntlets */
UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.id
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
SET bint.value = bint.value - 83 /* 308 -> 225 */
WHERE biota.weenie_Class_Id=37187 AND bint.`type`=28 AND version.value IS NULL;

INSERT INTO biota_properties_int (object_Id, `type`, value)
SELECT biota.id, 124, 2 FROM biota
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
WHERE biota.weenie_Class_Id=37187 AND version.value IS NULL;

/* 37188 Olthoi Amuli Gauntlets */
UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.id
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
SET bint.value = bint.value - 20 /* 225 -> 205 */
WHERE biota.weenie_Class_Id=37188 AND bint.`type`=28 AND version.value IS NULL;

INSERT INTO biota_properties_int (object_Id, `type`, value)
SELECT biota.id, 124, 2 FROM biota
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
WHERE biota.weenie_Class_Id=37188 AND version.value IS NULL;

/* 37189 Olthoi Celdon Gauntlets */
UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.id
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
SET bint.value = bint.value - 76 /* 301 -> 225 */
WHERE biota.weenie_Class_Id=37189 AND bint.`type`=28 AND version.value IS NULL;

INSERT INTO biota_properties_int (object_Id, `type`, value)
SELECT biota.id, 124, 2 FROM biota
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
WHERE biota.weenie_Class_Id=37189 AND version.value IS NULL;

/* 37190 Olthoi Koujia Gauntlets */
UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.id
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
SET bint.value = bint.value + 115 /* 110 -> 225 */
WHERE biota.weenie_Class_Id=37190 AND bint.`type`=28 AND version.value IS NULL;

INSERT INTO biota_properties_int (object_Id, `type`, value)
SELECT biota.id, 124, 2 FROM biota
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
WHERE biota.weenie_Class_Id=37190 AND version.value IS NULL;

/* 37192 Olthoi Celdon Girth */
UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.id
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
SET bint.value = bint.value - 253 /* 478 -> 225 */
WHERE biota.weenie_Class_Id=37192 AND bint.`type`=28 AND version.value IS NULL;

INSERT INTO biota_properties_int (object_Id, `type`, value)
SELECT biota.id, 124, 2 FROM biota
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
WHERE biota.weenie_Class_Id=37192 AND version.value IS NULL;

/* 37195 Olthoi Alduressa Helm */
UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.id
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
SET bint.value = bint.value - 55 /* 280 -> 225 */
WHERE biota.weenie_Class_Id=37195 AND bint.`type`=28 AND version.value IS NULL;

INSERT INTO biota_properties_int (object_Id, `type`, value)
SELECT biota.id, 124, 2 FROM biota
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
WHERE biota.weenie_Class_Id=37195 AND version.value IS NULL;

/* 37196 Olthoi Amuli Helm */
UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.id
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
SET bint.value = bint.value - 83 /* 288 -> 205 */
WHERE biota.weenie_Class_Id=37196 AND bint.`type`=28 AND version.value IS NULL;

INSERT INTO biota_properties_int (object_Id, `type`, value)
SELECT biota.id, 124, 2 FROM biota
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
WHERE biota.weenie_Class_Id=37196 AND version.value IS NULL;

/* 37197 Olthoi Celdon Helm */
UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.id
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
SET bint.value = bint.value - 77 /* 302 -> 225 */
WHERE biota.weenie_Class_Id=37197 AND bint.`type`=28 AND version.value IS NULL;

INSERT INTO biota_properties_int (object_Id, `type`, value)
SELECT biota.id, 124, 2 FROM biota
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
WHERE biota.weenie_Class_Id=37197 AND version.value IS NULL;

/* 37198 Olthoi Koujia Kabuton */
UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.id
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
SET bint.value = bint.value + 115 /* 110 -> 225 */
WHERE biota.weenie_Class_Id=37198 AND bint.`type`=28 AND version.value IS NULL;

INSERT INTO biota_properties_int (object_Id, `type`, value)
SELECT biota.id, 124, 2 FROM biota
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
WHERE biota.weenie_Class_Id=37198 AND version.value IS NULL;

/* 37200 Olthoi Alduressa Leggings */
UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.id
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
SET bint.value = bint.value - 479 /* 704 -> 225 */
WHERE biota.weenie_Class_Id=37200 AND bint.`type`=28 AND version.value IS NULL;

INSERT INTO biota_properties_int (object_Id, `type`, value)
SELECT biota.id, 124, 2 FROM biota
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
WHERE biota.weenie_Class_Id=37200 AND version.value IS NULL;

/* 37201 Olthoi Amuli Leggings */
UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.id
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
SET bint.value = bint.value - 53 /* 258 -> 205 */
WHERE biota.weenie_Class_Id=37201 AND bint.`type`=28 AND version.value IS NULL;

INSERT INTO biota_properties_int (object_Id, `type`, value)
SELECT biota.id, 124, 2 FROM biota
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
WHERE biota.weenie_Class_Id=37201 AND version.value IS NULL;

/* 37202 Olthoi Celdon Leggings */
UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.id
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
SET bint.value = bint.value - 85 /* 310 -> 225 */
WHERE biota.weenie_Class_Id=37202 AND bint.`type`=28 AND version.value IS NULL;

INSERT INTO biota_properties_int (object_Id, `type`, value)
SELECT biota.id, 124, 2 FROM biota
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
WHERE biota.weenie_Class_Id=37202 AND version.value IS NULL;

/* 37203 Olthoi Koujia Leggings */
UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.id
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
SET bint.value = bint.value + 115 /* 110 -> 225 */
WHERE biota.weenie_Class_Id=37203 AND bint.`type`=28 AND version.value IS NULL;

INSERT INTO biota_properties_int (object_Id, `type`, value)
SELECT biota.id, 124, 2 FROM biota
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
WHERE biota.weenie_Class_Id=37203 AND version.value IS NULL;

/* 37205 Olthoi Celdon Sleeves */
UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.id
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
SET bint.value = bint.value - 459 /* 684 -> 225 */
WHERE biota.weenie_Class_Id=37205 AND bint.`type`=28 AND version.value IS NULL;

INSERT INTO biota_properties_int (object_Id, `type`, value)
SELECT biota.id, 124, 2 FROM biota
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
WHERE biota.weenie_Class_Id=37205 AND version.value IS NULL;

/* 37206 Olthoi Koujia Sleeves */
UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.id
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
SET bint.value = bint.value + 115 /* 110 -> 225 */
WHERE biota.weenie_Class_Id=37206 AND bint.`type`=28 AND version.value IS NULL;

INSERT INTO biota_properties_int (object_Id, `type`, value)
SELECT biota.id, 124, 2 FROM biota
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
WHERE biota.weenie_Class_Id=37206 AND version.value IS NULL;

/* 37207 Olthoi Alduressa Boots */
UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.id
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
SET bint.value = bint.value - 499 /* 724 -> 225 */
WHERE biota.weenie_Class_Id=37207 AND bint.`type`=28 AND version.value IS NULL;

INSERT INTO biota_properties_int (object_Id, `type`, value)
SELECT biota.id, 124, 2 FROM biota
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
WHERE biota.weenie_Class_Id=37207 AND version.value IS NULL;

/* 37208 Olthoi Amuli Sollerets */
UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.id
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
SET bint.value = bint.value - 95 /* 300 -> 205 */
WHERE biota.weenie_Class_Id=37208 AND bint.`type`=28 AND version.value IS NULL;

INSERT INTO biota_properties_int (object_Id, `type`, value)
SELECT biota.id, 124, 2 FROM biota
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
WHERE biota.weenie_Class_Id=37208 AND version.value IS NULL;

/* 37209 Olthoi Celdon Sollerets */
UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.id
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
SET bint.value = bint.value - 60 /* 285 -> 225 */
WHERE biota.weenie_Class_Id=37209 AND bint.`type`=28 AND version.value IS NULL;

INSERT INTO biota_properties_int (object_Id, `type`, value)
SELECT biota.id, 124, 2 FROM biota
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
WHERE biota.weenie_Class_Id=37209 AND version.value IS NULL;

/* 37211 Olthoi Koujia Sollerets */
UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.id
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
SET bint.value = bint.value - 105 /* 330 -> 225 */
WHERE biota.weenie_Class_Id=37211 AND bint.`type`=28 AND version.value IS NULL;

INSERT INTO biota_properties_int (object_Id, `type`, value)
SELECT biota.id, 124, 2 FROM biota
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
WHERE biota.weenie_Class_Id=37211 AND version.value IS NULL;

/* 37214 Olthoi Celdon Breastplate */
UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.id
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
SET bint.value = bint.value - 34 /* 259 -> 225 */
WHERE biota.weenie_Class_Id=37214 AND bint.`type`=28 AND version.value IS NULL;

INSERT INTO biota_properties_int (object_Id, `type`, value)
SELECT biota.id, 124, 2 FROM biota
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
WHERE biota.weenie_Class_Id=37214 AND version.value IS NULL;

/* 37215 Olthoi Koujia Breastplate */
UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.id
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
SET bint.value = bint.value + 115 /* 110 -> 225 */
WHERE biota.weenie_Class_Id=37215 AND bint.`type`=28 AND version.value IS NULL;

INSERT INTO biota_properties_int (object_Id, `type`, value)
SELECT biota.id, 124, 2 FROM biota
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
WHERE biota.weenie_Class_Id=37215 AND version.value IS NULL;

/* 37217 Olthoi Alduressa Coat */
UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.id
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
SET bint.value = bint.value - 429 /* 654 -> 225 */
WHERE biota.weenie_Class_Id=37217 AND bint.`type`=28 AND version.value IS NULL;

INSERT INTO biota_properties_int (object_Id, `type`, value)
SELECT biota.id, 124, 2 FROM biota
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
WHERE biota.weenie_Class_Id=37217 AND version.value IS NULL;

/* 37299 Olthoi Amuli Coat */
UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.id
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
SET bint.value = bint.value - 67 /* 272 -> 205 */
WHERE biota.weenie_Class_Id=37299 AND bint.`type`=28 AND version.value IS NULL;

INSERT INTO biota_properties_int (object_Id, `type`, value)
SELECT biota.id, 124, 2 FROM biota
LEFT JOIN biota_properties_int version ON version.object_Id=biota.id AND version.`type`=124
WHERE biota.weenie_Class_Id=37299 AND version.value IS NULL;
