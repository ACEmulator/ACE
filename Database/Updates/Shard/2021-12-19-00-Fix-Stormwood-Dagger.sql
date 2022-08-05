/* PropertyInt.AttackType - 53323 Stormwood Dagger */

UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_int wint ON wint.object_Id=biota.weenie_Class_Id
SET bint.value=wint.value
WHERE biota.weenie_Class_Id=53323 and bint.`type`=47 and wint.`type`=47;


/* PropertyInt.AttackType - 72006 Stormwood Dagger */

UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_int wint ON wint.object_Id=biota.weenie_Class_Id
SET bint.value=wint.value
WHERE biota.weenie_Class_Id=72006 and bint.`type`=47 and wint.`type`=47;
