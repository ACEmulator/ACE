USE ace_shard;

UPDATE character_properties_spell_bar
SET spell_Bar_Number = spell_Bar_Number + 1,
    spell_Bar_Index = spell_Bar_Index + 1
WHERE id > 0;
