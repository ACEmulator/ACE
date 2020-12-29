/* Fix existing heritage specific casino keys */

UPDATE biota_properties_string
SET `value` = 'keygambler'
WHERE (object_Id > 0 AND `type` = 13) AND (`value` = 'keygambleralu' OR `value` = 'keygamblergha' OR `value` = 'keygamblersho');

/* Fix existing exquisite casino keys */

UPDATE biota_properties_string
SET `value` = 'exquisitekey'
WHERE object_Id > 0 AND `type` = 13 AND (`value` = 'keygamblerexquisite' OR `value` = 'exquisitechest');
