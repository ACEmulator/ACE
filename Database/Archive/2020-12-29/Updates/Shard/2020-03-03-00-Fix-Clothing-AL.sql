UPDATE biota_properties_int bint
INNER JOIN biota on bint.object_Id=biota.id and biota.weenie_Type=2 /* Clothing */
INNER JOIN biota_properties_int bint2 on bint2.object_Id=biota.id and bint2.`type`=9 /* ValidLocations */ and bint2.value & 0x42 /* ChestWear | UpperLegWear */ != 0
INNER JOIN ace_world.weenie_properties_int wint on wint.object_Id=biota.weenie_Class_Id and wint.`type`=28 /* ArmorLevel */
SET bint.value=0
WHERE bint.`type`=28 /* ArmorLevel */ and bint.value > 0 and (wint.value is null or wint.value = 0);
