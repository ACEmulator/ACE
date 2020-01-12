USE `ace_world`;

ALTER TABLE `points_of_interest` 
DROP FOREIGN KEY `wcid_poi`;
ALTER TABLE `points_of_interest` 
DROP INDEX `wcid_poi` ;
;
