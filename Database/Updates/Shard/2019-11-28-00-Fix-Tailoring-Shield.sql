update biota_properties_int bint
inner join biota on biota.id=bint.object_Id and biota.weenie_Class_Id=44863
set bint.value=6 /* Vestements */
where bint.`type`=94 and bint.value=4; /* Clothing */
