UPDATE biota_properties_int bint
INNER JOIN biota ON biota.id=bint.object_Id
SET value=524296 /* ItemUseable - SourceContainedTargetContained */
WHERE biota.weenie_Class_Id=21073 AND bint.`type`=16;

INSERT INTO biota_properties_int (object_Id, `type`, value)
SELECT biota.id, 94, 35215 /* TargetType - Jewelry, Misc, Gem, RedirectableItemEnchantmentTarget */
FROM biota
LEFT JOIN biota_properties_int bint ON biota.id=bint.object_Id AND bint.`type`=94
WHERE biota.weenie_Class_Id=21073 AND bint.value IS NULL;

UPDATE biota_properties_string bstr
INNER JOIN biota ON biota.id=bstr.object_Id
SET value='Apply this material to a treasure-generated item in order to remove that item\'s "Retained" status.'
WHERE biota.weenie_Class_Id=21073 AND bstr.`type`=14; /* Use */
