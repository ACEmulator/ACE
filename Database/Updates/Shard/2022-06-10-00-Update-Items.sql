/* PropertyFloat.CriticalMultiplier - 30377 Wings of Rakhil */

UPDATE biota_properties_float bfloat
INNER JOIN biota ON bfloat.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_float wfloat ON wfloat.object_Id=biota.weenie_Class_Id
SET bfloat.value=wfloat.value
WHERE biota.weenie_Class_Id=30377 and bfloat.`type`=136 and wfloat.`type`=136;

/* PropertyFloat.CriticalMultiplier - 34991 Opal Repugnant Staff */

UPDATE biota_properties_float bfloat
INNER JOIN biota ON bfloat.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_float wfloat ON wfloat.object_Id=biota.weenie_Class_Id
SET bfloat.value=wfloat.value
WHERE biota.weenie_Class_Id=34991 and bfloat.`type`=136 and wfloat.`type`=136;

/* PropertyFloat.CriticalMultiplier - 35558 Ice Wand */

UPDATE biota_properties_float bfloat
INNER JOIN biota ON bfloat.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_float wfloat ON wfloat.object_Id=biota.weenie_Class_Id
SET bfloat.value=wfloat.value
WHERE biota.weenie_Class_Id=35558 and bfloat.`type`=136 and wfloat.`type`=136;

/* PropertyFloat.CriticalMultiplier - 70895 Chimeric Eye of the Quiddity */

UPDATE biota_properties_float bfloat
INNER JOIN biota ON bfloat.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_float wfloat ON wfloat.object_Id=biota.weenie_Class_Id
SET bfloat.value=wfloat.value
WHERE biota.weenie_Class_Id=70895 and bfloat.`type`=136 and wfloat.`type`=136;

/* PropertyFloat.SlayerDamageBonus - 35950 Tusker Paw Wand */

UPDATE biota_properties_float bfloat
INNER JOIN biota ON bfloat.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_float wfloat ON wfloat.object_Id=biota.weenie_Class_Id
SET bfloat.value=wfloat.value
WHERE biota.weenie_Class_Id=35950 and bfloat.`type`=138 and wfloat.`type`=138;
