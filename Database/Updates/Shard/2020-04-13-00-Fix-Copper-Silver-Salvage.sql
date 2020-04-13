insert into biota_properties_d_i_d (object_Id, `type`, value)
select bint.object_Id, 37 /* ItemSkillLimit */, bint.value from biota_properties_int bint
left join biota_properties_d_i_d bdid on bdid.object_Id=bint.object_Id and bdid.`type`=37 /* ItemSkillLimit */
where bint.`type`=159 /* WieldSkillType */
and (bint.value=6 /* MeleeDefense */ or bint.value=7) /* MissileDefense */ and bdid.value is null;
