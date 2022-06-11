/* PropertyInt.ItemSpellcraft - 71446 Exquisite Lense.sql */

UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_int wint ON wint.object_Id=biota.weenie_Class_Id
SET bint.value=wint.value
WHERE biota.weenie_Class_Id=71446 and bint.`type`=106 and wint.`type`=106;
