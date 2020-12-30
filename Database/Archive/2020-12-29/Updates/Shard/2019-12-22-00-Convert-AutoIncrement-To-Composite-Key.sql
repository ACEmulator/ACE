USE ace_shard;

ALTER TABLE `biota_properties_attribute` 
DROP COLUMN `id`,
DROP PRIMARY KEY,
ADD PRIMARY KEY (`object_Id`, `type`),
DROP INDEX `wcid_attribute_type_uidx` ;
;

ALTER TABLE `biota_properties_attribute_2nd` 
DROP COLUMN `id`,
DROP PRIMARY KEY,
ADD PRIMARY KEY (`object_Id`, `type`),
DROP INDEX `wcid_attribute2nd_type_uidx` ;
;

ALTER TABLE `biota_properties_book` 
DROP COLUMN `id`,
DROP PRIMARY KEY,
ADD PRIMARY KEY (`object_Id`),
DROP INDEX `wcid_bookdata_uidx` ;
;

ALTER TABLE `biota_properties_bool` 
DROP COLUMN `id`,
DROP PRIMARY KEY,
ADD PRIMARY KEY (`object_Id`, `type`),
DROP INDEX `wcid_bool_type_uidx` ;
;

ALTER TABLE `biota_properties_d_i_d` 
DROP COLUMN `id`,
DROP PRIMARY KEY,
ADD PRIMARY KEY (`object_Id`, `type`),
DROP INDEX `wcid_did_type_uidx` ;
;

ALTER TABLE `biota_properties_event_filter` 
DROP COLUMN `id`,
DROP PRIMARY KEY,
ADD PRIMARY KEY (`object_Id`, `event`),
DROP INDEX `wcid_eventfilter_type_uidx` ;
;

ALTER TABLE `biota_properties_float` 
DROP COLUMN `id`,
DROP PRIMARY KEY,
ADD PRIMARY KEY (`object_Id`, `type`),
DROP INDEX `wcid_float_type_uidx` ;
;

ALTER TABLE `biota_properties_i_i_d` 
DROP COLUMN `id`,
DROP PRIMARY KEY,
ADD PRIMARY KEY (`object_Id`, `type`),
DROP INDEX `wcid_iid_type_uidx` ;
;

ALTER TABLE `biota_properties_int` 
DROP COLUMN `id`,
DROP PRIMARY KEY,
ADD PRIMARY KEY (`object_Id`, `type`),
DROP INDEX `wcid_int_type_uidx` ;
;

ALTER TABLE `biota_properties_int64` 
DROP COLUMN `id`,
DROP PRIMARY KEY,
ADD PRIMARY KEY (`object_Id`, `type`),
DROP INDEX `wcid_int64_type_uidx` ;
;

ALTER TABLE `biota_properties_position` 
DROP COLUMN `id`,
DROP PRIMARY KEY,
ADD PRIMARY KEY (`object_Id`, `position_Type`),
DROP INDEX `wcid_position_type_uidx` ;
;

ALTER TABLE `biota_properties_skill` 
DROP COLUMN `id`,
DROP PRIMARY KEY,
ADD PRIMARY KEY (`object_Id`, `type`),
DROP INDEX `wcid_skill_type_uidx` ;
;

ALTER TABLE `biota_properties_spell_book` 
DROP COLUMN `id`,
DROP PRIMARY KEY,
ADD PRIMARY KEY (`object_Id`, `spell`),
DROP INDEX `wcid_spellbook_type_uidx` ;
;

ALTER TABLE `biota_properties_string` 
DROP COLUMN `id`,
DROP PRIMARY KEY,
ADD PRIMARY KEY (`object_Id`, `type`),
DROP INDEX `wcid_string_type_uidx` ;
;



ALTER TABLE `character_properties_fill_comp_book` 
DROP COLUMN `id`,
DROP PRIMARY KEY,
ADD PRIMARY KEY (`character_Id`, `spell_Component_Id`),
DROP INDEX `wcid_fillcompbook_type_uidx` ;
;

ALTER TABLE `character_properties_friend_list` 
DROP COLUMN `id`,
DROP PRIMARY KEY,
ADD PRIMARY KEY (`character_Id`, `friend_Id`),
DROP INDEX `wcid_friend_uidx` ;
;

ALTER TABLE `character_properties_quest_registry` 
DROP COLUMN `id`,
DROP PRIMARY KEY,
ADD PRIMARY KEY (`character_Id`, `quest_Name`),
DROP INDEX `wcid_questbook_name_uidx` ;
;

ALTER TABLE `character_properties_title_book` 
DROP COLUMN `id`,
DROP PRIMARY KEY,
ADD PRIMARY KEY (`character_Id`, `title_Id`),
DROP INDEX `wcid_titlebook_type_uidx` ;
;

ALTER TABLE `house_permission` 
DROP COLUMN `id`,
DROP PRIMARY KEY,
ADD PRIMARY KEY (`house_Id`, `player_Guid`),
DROP INDEX `biota_Id_house_Id_player_Guid_uidx` ;
;
