ALTER TABLE `biota_properties_book` 
CHANGE COLUMN `max_Num_Pages` `max_Num_Pages` INT(10) NOT NULL DEFAULT '0' COMMENT 'Maximum number of pages per book' ,
CHANGE COLUMN `max_Num_Chars_Per_Page` `max_Num_Chars_Per_Page` INT(10) NOT NULL DEFAULT '0' COMMENT 'Maximum number of characters per page' ;

ALTER TABLE `biota_properties_create_list` 
CHANGE COLUMN `stack_Size` `stack_Size` INT(10) NOT NULL DEFAULT '0' COMMENT 'Stack Size of object to create (-1 = infinite)' ;

ALTER TABLE `biota_properties_emote` 
CHANGE COLUMN `probability` `probability` FLOAT NOT NULL DEFAULT '0' COMMENT 'Probability of this EmoteSet being chosen' ;

ALTER TABLE `biota_properties_emote_action` 
CHANGE COLUMN `delay` `delay` FLOAT NOT NULL DEFAULT '0' COMMENT 'Time to wait before EmoteAction starts execution' ,
CHANGE COLUMN `extent` `extent` FLOAT NOT NULL DEFAULT '0' COMMENT '?' ;

ALTER TABLE `biota_properties_generator` 
CHANGE COLUMN `probability` `probability` FLOAT NOT NULL DEFAULT '0' ,
CHANGE COLUMN `init_Create` `init_Create` INT(10) NOT NULL DEFAULT '0' COMMENT 'Number of object to generate initially' ,
CHANGE COLUMN `max_Create` `max_Create` INT(10) NOT NULL DEFAULT '0' COMMENT 'Maximum amount of objects to generate' ,
CHANGE COLUMN `when_Create` `when_Create` INT(10) UNSIGNED NOT NULL DEFAULT '0' COMMENT 'When to generate the weenie object' ,
CHANGE COLUMN `where_Create` `where_Create` INT(10) UNSIGNED NOT NULL DEFAULT '0' COMMENT 'Where to generate the weenie object' ;

ALTER TABLE `biota_properties_spell_book` 
CHANGE COLUMN `probability` `probability` FLOAT NOT NULL DEFAULT '0' COMMENT 'Chance to cast this spell' ;
