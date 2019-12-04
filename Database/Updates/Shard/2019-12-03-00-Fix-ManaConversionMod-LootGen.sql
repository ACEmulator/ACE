delete bfloat from biota_properties_float bfloat
inner join biota_properties_int bint on bint.object_Id=bfloat.object_Id and bint.`type`=105 /* ItemWorkmanship */
where bfloat.`type`=144 /* ManaConversionMod */ and bfloat.value=0;
