/* ensure PetDevices PetClass are synced with base weenie */
insert into biota_properties_int (object_Id, `type`, value)
select biota.id, 266 /* PetClass */, wint.value from ace_shard.biota biota
inner join ace_world.weenie_properties_int wint on wint.object_Id=biota.weenie_Class_Id and wint.`type`=266 /* PetClass */
where weenie_Type=70 /* PetDevice */
on duplicate key update biota_properties_int.value=biota_properties_int.value;

/* ensure PetDevices SummoningMastery are synced with base weenie */
insert into biota_properties_int (object_Id, `type`, value)
select biota.id, 362 /* SummoningMastery */, wint.value from ace_shard.biota biota
inner join ace_world.weenie_properties_int wint on wint.object_Id=biota.weenie_Class_Id and wint.`type`=362 /* SummoningMastery */
where weenie_Type=70 /* PetDevice */
on duplicate key update biota_properties_int.value=biota_properties_int.value;
