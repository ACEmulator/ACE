ALTER TABLE weenie_palette_changes DROP PRIMARY KEY, ADD PRIMARY KEY(`weenieClassId`, `subPaletteId`, `offset`, `length`);

ALTER TABLE weenie_texture_map_changes DROP PRIMARY KEY, ADD PRIMARY KEY(`weenieClassId`, `index`, `oldId`);

ALTER TABLE ace_object_palette_changes DROP PRIMARY KEY, ADD PRIMARY KEY(`baseAceObjectId`, `subPaletteId`, `offset`, `length`);

ALTER TABLE ace_object_texture_map_changes DROP PRIMARY KEY, ADD PRIMARY KEY(`baseAceObjectId`, `index`, `oldId`);
