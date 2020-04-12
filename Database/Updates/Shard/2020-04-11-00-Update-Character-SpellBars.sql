USE ace_shard;

ALTER TABLE `character_properties_spell_bar`
DROP FOREIGN KEY `wcid_spellbar`;
ALTER TABLE `character_properties_spell_bar`
DROP INDEX `wcid_spellbar_barId_spellId_uidx`;

UPDATE character_properties_spell_bar
SET spell_Bar_Number = spell_Bar_Number + 1,
    spell_Bar_Index = spell_Bar_Index + 1
WHERE id > 0;

ALTER TABLE `character_properties_spell_bar`
DROP COLUMN `id`,
DROP PRIMARY KEY,
ADD PRIMARY KEY (`character_Id`, `spell_Bar_Number`, `spell_Id`),
ADD INDEX `spellBar_idx` (`spell_Bar_Index` ASC);

ALTER TABLE `character_properties_spell_bar`
ADD CONSTRAINT `characterId_spellbar`
  FOREIGN KEY (`character_Id`)
  REFERENCES `character` (`id`)
  ON DELETE CASCADE;
