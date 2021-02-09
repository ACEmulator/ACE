INSERT INTO biota_properties_int
SELECT biota.id, 159 /* WieldSkillType */, 1 FROM biota
INNER JOIN biota_properties_int wieldReq ON wieldReq.object_Id=biota.id AND wieldReq.`type`=158 /* WieldRequirements */
LEFT JOIN biota_properties_int wieldSkill ON wieldSkill.object_Id=biota.id AND wieldSkill.`type`=159 /* WieldSkillType */
WHERE wieldReq.value=7 /* Level */ AND wieldSkill.value IS NULL;
