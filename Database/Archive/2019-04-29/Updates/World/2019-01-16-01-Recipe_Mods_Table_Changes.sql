USE `ace_world`;

ALTER TABLE `recipe_mods_bool` 
CHANGE COLUMN `unknown_1` `source` INT(10) NOT NULL ;

ALTER TABLE `recipe_mods_d_i_d` 
CHANGE COLUMN `unknown_1` `source` INT(10) NOT NULL ;

ALTER TABLE `recipe_mods_float` 
CHANGE COLUMN `unknown_1` `source` INT(10) NOT NULL ;

ALTER TABLE `recipe_mods_i_i_d` 
CHANGE COLUMN `unknown_1` `source` INT(10) NOT NULL ;

ALTER TABLE `recipe_mods_int` 
CHANGE COLUMN `unknown_1` `source` INT(10) NOT NULL ;

ALTER TABLE `recipe_mods_string` 
CHANGE COLUMN `unknown_1` `source` INT(10) NOT NULL ;
