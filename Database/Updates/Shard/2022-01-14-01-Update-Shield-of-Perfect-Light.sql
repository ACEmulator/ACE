/* Shield of Perfect Light */

UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_int wint ON wint.object_Id=biota.weenie_Class_Id
SET bint.value=wint.value
WHERE biota.weenie_Class_Id=35295 and bint.`type`=28 and wint.`type`=28;

UPDATE biota_properties_float bfloat
INNER JOIN biota ON bfloat.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_float wfloat ON wfloat.object_Id=biota.weenie_Class_Id
SET bfloat.value=wfloat.value
WHERE biota.weenie_Class_Id=35295 and 
((bfloat.`type`=13 and wfloat.`type`=13) 
or (bfloat.`type`=14 and wfloat.`type`=14)
or (bfloat.`type`=15 and wfloat.`type`=15)
or (bfloat.`type`=16 and wfloat.`type`=16)
or (bfloat.`type`=17 and wfloat.`type`=17)
or (bfloat.`type`=18 and wfloat.`type`=18)
or (bfloat.`type`=19 and wfloat.`type`=19)
or (bfloat.`type`=159 and wfloat.`type`=159)
);
