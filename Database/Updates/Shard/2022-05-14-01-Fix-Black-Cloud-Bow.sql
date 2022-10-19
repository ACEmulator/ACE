/* PropertyInt.ItemXpStyle - 30304 Black Cloud Bow */

UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_int wint ON wint.object_Id=biota.weenie_Class_Id
SET bint.value=wint.value
WHERE biota.weenie_Class_Id=30304 and bint.`type`=320 and wint.`type`=320;
