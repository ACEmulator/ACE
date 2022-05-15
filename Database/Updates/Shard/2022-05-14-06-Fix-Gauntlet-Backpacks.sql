/* PropertyInt.EncumbranceVal - 52734 Gauntlet Backpack */

UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_int wint ON wint.object_Id=biota.weenie_Class_Id
SET bint.value=wint.value
WHERE biota.weenie_Class_Id=52734 and bint.`type`=5 and wint.`type`=5;

/* PropertyInt.EncumbranceVal - 52735 Gauntlet Backpack */

UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_int wint ON wint.object_Id=biota.weenie_Class_Id
SET bint.value=wint.value
WHERE biota.weenie_Class_Id=52735 and bint.`type`=5 and wint.`type`=5;

/* PropertyInt.EncumbranceVal - 52736 Gauntlet Backpack */

UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_int wint ON wint.object_Id=biota.weenie_Class_Id
SET bint.value=wint.value
WHERE biota.weenie_Class_Id=52736 and bint.`type`=5 and wint.`type`=5;

