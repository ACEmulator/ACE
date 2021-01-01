/* PropertyFloat.CriticalFrequency - 36552 Scepter of the Portal Currents */

UPDATE biota_properties_float bfloat
INNER JOIN biota ON bfloat.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_float wfloat ON wfloat.object_Id=biota.weenie_Class_Id
SET bfloat.value=wfloat.value
WHERE biota.weenie_Class_Id=36552 and bfloat.`type`=147 and wfloat.`type`=147;

/* PropertyFloat.CriticalFrequency - 43813 Sturdy Bloodstone Wand */

UPDATE biota_properties_float bfloat
INNER JOIN biota ON bfloat.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_float wfloat ON wfloat.object_Id=biota.weenie_Class_Id
SET bfloat.value=wfloat.value
WHERE biota.weenie_Class_Id=43813 and bfloat.`type`=147 and wfloat.`type`=147;

/* PropertyFloat.CriticalFrequency - 43814 Delicate Bloodstone Wand */

UPDATE biota_properties_float bfloat
INNER JOIN biota ON bfloat.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_float wfloat ON wfloat.object_Id=biota.weenie_Class_Id
SET bfloat.value=wfloat.value
WHERE biota.weenie_Class_Id=43814 and bfloat.`type`=147 and wfloat.`type`=147;
