DROP TABLE IF EXISTS `accesslevel`;

CREATE TABLE IF NOT EXISTS `accesslevel` (
  `level` int(10) unsigned NOT NULL DEFAULT '0',
  `name` varchar(45) NOT NULL,
  `prefix` varchar(45) DEFAULT '',
  PRIMARY KEY (`level`),
  UNIQUE KEY `level` (`level`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

INSERT INTO `accesslevel` (`level`,`name`,`prefix`) VALUES (0,'Player','');
INSERT INTO `accesslevel` (`level`,`name`,`prefix`) VALUES (1,'Advocate','');
INSERT INTO `accesslevel` (`level`,`name`,`prefix`) VALUES (2,'Sentinel','Sentinel');
INSERT INTO `accesslevel` (`level`,`name`,`prefix`) VALUES (3,'Envoy','Envoy');
INSERT INTO `accesslevel` (`level`,`name`,`prefix`) VALUES (4,'Developer','');
INSERT INTO `accesslevel` (`level`,`name`,`prefix`) VALUES (5,'Admin','Admin');

ALTER TABLE `account` 
ADD COLUMN `accesslevel` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `account`,
ADD INDEX `accesslevel_idx` (`accesslevel` ASC);
ALTER TABLE `account` 
ADD CONSTRAINT `accesslevel`
  FOREIGN KEY (`accesslevel`)
  REFERENCES `accesslevel` (`level`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;
  