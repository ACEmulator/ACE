insert into biota_properties_bool (object_Id, `type`, value)
select id, 66 /* IgnoreMagicArmor */, 1 from biota
left join biota_properties_bool bbool on bbool.object_Id=biota.id and `type`=66 /* IgnoreMagicArmor */
where biota.weenie_Class_Id=30316 and bbool.value is null;
