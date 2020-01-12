USE `ace_world`;

ALTER TABLE `recipe_mods_bool` 
ADD COLUMN `index` TINYINT(5) NOT NULL AFTER `recipe_Mod_Id`;

ALTER TABLE `recipe_mods_d_i_d` 
ADD COLUMN `index` TINYINT(5) NOT NULL AFTER `recipe_Mod_Id`;

ALTER TABLE `recipe_mods_float` 
ADD COLUMN `index` TINYINT(5) NOT NULL AFTER `recipe_Mod_Id`;

ALTER TABLE `recipe_mods_i_i_d` 
ADD COLUMN `index` TINYINT(5) NOT NULL AFTER `recipe_Mod_Id`;

ALTER TABLE `recipe_mods_int` 
ADD COLUMN `index` TINYINT(5) NOT NULL AFTER `recipe_Mod_Id`;

ALTER TABLE `recipe_mods_string` 
ADD COLUMN `index` TINYINT(5) NOT NULL AFTER `recipe_Mod_Id`;

ALTER TABLE `recipe_requirements_bool` 
ADD COLUMN `index` TINYINT(5) NOT NULL AFTER `recipe_Id`;

ALTER TABLE `recipe_requirements_d_i_d` 
ADD COLUMN `index` TINYINT(5) NOT NULL AFTER `recipe_Id`;

ALTER TABLE `recipe_requirements_float` 
ADD COLUMN `index` TINYINT(5) NOT NULL AFTER `recipe_Id`;

ALTER TABLE `recipe_requirements_i_i_d` 
ADD COLUMN `index` TINYINT(5) NOT NULL AFTER `recipe_Id`;

ALTER TABLE `recipe_requirements_int` 
ADD COLUMN `index` TINYINT(5) NOT NULL AFTER `recipe_Id`;

ALTER TABLE `recipe_requirements_string` 
ADD COLUMN `index` TINYINT(5) NOT NULL AFTER `recipe_Id`;
