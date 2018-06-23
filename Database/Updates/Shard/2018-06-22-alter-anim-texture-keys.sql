alter table biota_properties_anim_part add column `order` tinyint(3) unsigned;
alter table biota_properties_texture_map add column `order` tinyint(3) unsigned;

alter table biota_properties_anim_part drop key `object_Id_index_uidx`, add unique key `object_Id_index_uidx`(`object_Id`,`index`,`animation_Id`);
alter table biota_properties_texture_map drop key `object_Id_index_oldId_uidx`, add unique key `object_Id_index_oldId_uidx`(`object_Id`,`index`,`old_Id`, `new_Id`);

