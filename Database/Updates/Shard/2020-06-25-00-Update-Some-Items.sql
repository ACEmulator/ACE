/* PropertyInt.ClothingPriority - 25840 Snarl's Jerkin */

UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_int wint ON wint.object_Id=biota.weenie_Class_Id
SET bint.value=wint.value
WHERE biota.weenie_Class_Id=25840 and bint.`type`=4 and wint.`type`=4;

/* PropertyFloat.SlayerDamageBonus - 35377 Replica BloodScorch */

UPDATE biota_properties_float bfloat
INNER JOIN biota ON bfloat.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_float wfloat ON wfloat.object_Id=biota.weenie_Class_Id
SET bfloat.value=wfloat.value
WHERE biota.weenie_Class_Id=35377 and bfloat.`type`=138 and wfloat.`type`=138;

/* PropertyFloat.CriticalFrequency - 35377 Replica BloodScorch */

UPDATE biota_properties_float bfloat
INNER JOIN biota ON bfloat.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_float wfloat ON wfloat.object_Id=biota.weenie_Class_Id
SET bfloat.value=wfloat.value
WHERE biota.weenie_Class_Id=35377 and bfloat.`type`=147 and wfloat.`type`=147;

/* PropertyFloat.ProcSpellRate - 35377 Replica BloodScorch */

UPDATE biota_properties_float bfloat
INNER JOIN biota ON bfloat.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_float wfloat ON wfloat.object_Id=biota.weenie_Class_Id
SET bfloat.value=wfloat.value
WHERE biota.weenie_Class_Id=35377 and bfloat.`type`=156 and wfloat.`type`=156;

/* PropertyFloat.SlayerDamageBonus - 40652 Great BloodScorch */

UPDATE biota_properties_float bfloat
INNER JOIN biota ON bfloat.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_float wfloat ON wfloat.object_Id=biota.weenie_Class_Id
SET bfloat.value=wfloat.value
WHERE biota.weenie_Class_Id=40652 and bfloat.`type`=138 and wfloat.`type`=138;

/* PropertyFloat.CriticalFrequency - 40652 Great BloodScorch */

UPDATE biota_properties_float bfloat
INNER JOIN biota ON bfloat.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_float wfloat ON wfloat.object_Id=biota.weenie_Class_Id
SET bfloat.value=wfloat.value
WHERE biota.weenie_Class_Id=40652 and bfloat.`type`=147 and wfloat.`type`=147;

/* PropertyFloat.ProcSpellRate - 40652 Great BloodScorch */

UPDATE biota_properties_float bfloat
INNER JOIN biota ON bfloat.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_float wfloat ON wfloat.object_Id=biota.weenie_Class_Id
SET bfloat.value=wfloat.value
WHERE biota.weenie_Class_Id=40652 and bfloat.`type`=156 and wfloat.`type`=156;
