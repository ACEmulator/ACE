/* PropertyInt.WieldRequirements - 46828 Purified Mouryou Katana */

UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_int wint ON wint.object_Id=biota.weenie_Class_Id
SET bint.value=wint.value
WHERE biota.weenie_Class_Id=46828 and bint.`type`=158 and wint.`type`=158;

/* PropertyInt.WieldRequirements - 46829 Purified Mouryou Nanjou-tachi */

UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_int wint ON wint.object_Id=biota.weenie_Class_Id
SET bint.value=wint.value
WHERE biota.weenie_Class_Id=46829 and bint.`type`=158 and wint.`type`=158;

/* PropertyInt.WieldRequirements - 46832 Purified Mouryou Nekode */

UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_int wint ON wint.object_Id=biota.weenie_Class_Id
SET bint.value=wint.value
WHERE biota.weenie_Class_Id=46832 and bint.`type`=158 and wint.`type`=158;

