/* PropertyDataId.Spell - 30141 Hieroglyph of Healing Mastery */

UPDATE biota_properties_d_i_d bd_i_d
INNER JOIN biota ON bd_i_d.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_d_i_d wd_i_d ON wd_i_d.object_Id=biota.weenie_Class_Id
SET bd_i_d.value=wd_i_d.value
WHERE biota.weenie_Class_Id=30141 and bd_i_d.`type`=28 and wd_i_d.`type`=28;

/* PropertyString.LongDesc - 30141 Hieroglyph of Healing Mastery */

UPDATE biota_properties_string bstring
INNER JOIN biota ON bstring.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_string wstring ON wstring.object_Id=biota.weenie_Class_Id
SET bstring.value=wstring.value
WHERE biota.weenie_Class_Id=30141 and bstring.`type`=16 and wstring.`type`=16;
