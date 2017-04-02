DROP VIEW IF EXISTS `vw_character_positions`;
CREATE OR REPLACE VIEW `vw_character_positions` AS (
  SELECT CP.character_id, CP.positionType, CP.cell, CP.positionX, CP.positionY, CP.positionZ, CP.rotationX, CP.rotationY, CP.rotationZ, CP.rotationW
  FROM `character_position` CP
  INNER JOIN `character` CH ON CP.character_id = CH.guid)