/* PropertyString.LongDesc - 69982 Night Club Shirt */

UPDATE biota_properties_string bstring
INNER JOIN biota ON bstring.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_string wstring ON wstring.object_Id=biota.weenie_Class_Id
SET bstring.value=wstring.value
WHERE biota.weenie_Class_Id=69982 and bstring.`type`=16 and wstring.`type`=16;
