/* PropertyInt.TargetType - 21066 Salvaged Peridot */

UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_int wint ON wint.object_Id=biota.weenie_Class_Id
SET bint.value=wint.value
WHERE biota.weenie_Class_Id=21066 and bint.`type`=94 and wint.`type`=94;

/* PropertyInt.TargetType - 21088 Salvaged Yellow Topaz */

UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_int wint ON wint.object_Id=biota.weenie_Class_Id
SET bint.value=wint.value
WHERE biota.weenie_Class_Id=21088 and bint.`type`=94 and wint.`type`=94;

/* PropertyInt.TargetType - 21089 Salvaged Zircon */

UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_int wint ON wint.object_Id=biota.weenie_Class_Id
SET bint.value=wint.value
WHERE biota.weenie_Class_Id=21089 and bint.`type`=94 and wint.`type`=94;

/* PropertyInt.TargetType - 36634 Foolproof Peridot */

UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_int wint ON wint.object_Id=biota.weenie_Class_Id
SET bint.value=wint.value
WHERE biota.weenie_Class_Id=36634 and bint.`type`=94 and wint.`type`=94;

/* PropertyInt.TargetType - 36635 Foolproof Yellow Topaz */

UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_int wint ON wint.object_Id=biota.weenie_Class_Id
SET bint.value=wint.value
WHERE biota.weenie_Class_Id=36635 and bint.`type`=94 and wint.`type`=94;

/* PropertyInt.TargetType - 36636 Foolproof Zircon */

UPDATE biota_properties_int bint
INNER JOIN biota ON bint.object_Id=biota.Id
INNER JOIN ace_world.weenie_properties_int wint ON wint.object_Id=biota.weenie_Class_Id
SET bint.value=wint.value
WHERE biota.weenie_Class_Id=36636 and bint.`type`=94 and wint.`type`=94;
