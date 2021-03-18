/* PropertyFloat.CriticalFrequency - 51899 Casting Stone */

UPDATE biota_properties_float bfloat
INNER JOIN biota ON bfloat.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_float wfloat ON wfloat.object_Id=biota.weenie_Class_Id
SET bfloat.value=wfloat.value
WHERE biota.weenie_Class_Id=51899 and bfloat.`type`=147 and wfloat.`type`=147;
