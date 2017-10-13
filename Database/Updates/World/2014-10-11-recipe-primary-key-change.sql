ALTER TABLE `ace_world`.`ace_recipe`   
  CHANGE `recipeId` `recipeGuid` BINARY(16) NOT NULL COMMENT 'surrogate key';
