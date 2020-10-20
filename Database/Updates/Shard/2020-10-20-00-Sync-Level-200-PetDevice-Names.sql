UPDATE biota_properties_string bstr
INNER JOIN biota ON bstr.object_Id=biota.Id
INNER JOIN biota_properties_int bint ON bint.object_Id=biota.id AND bint.`type`=369 /* UseRequiresLevel */
INNER JOIN ace_world.weenie_properties_string wstr ON wstr.object_id=biota.weenie_Class_Id AND wstr.`type`=1 /* Name */ 
SET bstr.value=wstr.value
WHERE biota.weenie_Type=70 /* PetDevice */ AND bint.value=185 AND bstr.`type`=1 /* Name */;
