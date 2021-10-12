UPDATE
    `biota_properties_int` bint
INNER JOIN `biota` ON bint.`object_Id` = `biota`.`Id`
INNER JOIN `ace_world`.`weenie_properties_int` wint
ON
    wint.`object_Id` = `biota`.`weenie_Class_Id`
SET
    `bint`.`value` = wint.`value`
WHERE
    biota.`weenie_Type` = 70 /* PetDevice */ AND bint.`type` = 266 /* PetClass */ AND wint.`type` = 266;