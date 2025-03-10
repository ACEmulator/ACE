/* PropertyFloat.CriticalMultiplier - 40425 Renegade Kalindan of the Mountains */

UPDATE biota_properties_float bfloat
INNER JOIN biota ON bfloat.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_float wfloat ON wfloat.object_Id=biota.weenie_Class_Id
SET bfloat.value=wfloat.value
WHERE biota.weenie_Class_Id=40425 and bfloat.`type`=136 and wfloat.`type`=136;


/* PropertyFloat.CriticalMultiplier - 88177 Renegade Kalindan of the Chase */

UPDATE biota_properties_float bfloat
INNER JOIN biota ON bfloat.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_float wfloat ON wfloat.object_Id=biota.weenie_Class_Id
SET bfloat.value=wfloat.value
WHERE biota.weenie_Class_Id=88177 and bfloat.`type`=136 and wfloat.`type`=136;


/* PropertyFloat.CriticalMultiplier - 88178 Renegade Kalindan of the Forests */

UPDATE biota_properties_float bfloat
INNER JOIN biota ON bfloat.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_float wfloat ON wfloat.object_Id=biota.weenie_Class_Id
SET bfloat.value=wfloat.value
WHERE biota.weenie_Class_Id=88178 and bfloat.`type`=136 and wfloat.`type`=136;


/* PropertyFloat.CriticalMultiplier - 88179 Renegade Kalindan of the Heights */

UPDATE biota_properties_float bfloat
INNER JOIN biota ON bfloat.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_float wfloat ON wfloat.object_Id=biota.weenie_Class_Id
SET bfloat.value=wfloat.value
WHERE biota.weenie_Class_Id=88179 and bfloat.`type`=136 and wfloat.`type`=136;


/* PropertyFloat.CriticalMultiplier - 88248 Renegade Kalindan of the Vortex */

UPDATE biota_properties_float bfloat
INNER JOIN biota ON bfloat.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_float wfloat ON wfloat.object_Id=biota.weenie_Class_Id
SET bfloat.value=wfloat.value
WHERE biota.weenie_Class_Id=88248 and bfloat.`type`=136 and wfloat.`type`=136;


/* PropertyFloat.CriticalMultiplier - 88249 Renegade Kalindan of the Rivers */

UPDATE biota_properties_float bfloat
INNER JOIN biota ON bfloat.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_float wfloat ON wfloat.object_Id=biota.weenie_Class_Id
SET bfloat.value=wfloat.value
WHERE biota.weenie_Class_Id=88249 and bfloat.`type`=136 and wfloat.`type`=136;
