/* PropertyFloat.ResistanceModifier - 52744 Nether Society Orb.sql */

UPDATE biota_properties_float bfloat
INNER JOIN biota ON bfloat.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_float wfloat ON wfloat.object_Id=biota.weenie_Class_Id
SET bfloat.value=wfloat.value
WHERE biota.weenie_Class_Id=52744 and bfloat.`type`=157 and wfloat.`type`=157;
