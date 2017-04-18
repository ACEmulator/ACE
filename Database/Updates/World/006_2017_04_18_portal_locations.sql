CREATE TABLE `portal_destination` (
  `weenieClassId` smallint(5) unsigned NOT NULL,
  `cell` int(10) unsigned NOT NULL DEFAULT '0',
  `x` float NOT NULL DEFAULT '0',
  `y` float NOT NULL DEFAULT '0',
  `z` float NOT NULL DEFAULT '0',
  `qx` float NOT NULL DEFAULT '0',
  `qy` float NOT NULL DEFAULT '0',
  `qz` float NOT NULL DEFAULT '0',
  `qw` float NOT NULL DEFAULT '0',
  `min_lvl` int(11) UNSIGNED NOT NULL DEFAULT '0',
  `max_lvl` int(11) UNSIGNED NOT NULL DEFAULT '0',
  PRIMARY KEY (`weenieClassId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

INSERT INTO `portal_destination` (`weenieClassId`, `cell`, `x`, `y`, `z`, `qx`, `qy`, `qz`, `qw`, `min_lvl`, `max_lvl`) VALUES 
  (2068,27132180, 10, -40, 0.005, 0, 0, -0.0375863, 0.999293,0,0),
  (2069,2847080450, 13.2, 35.4, 94.005, 0, 0, -0.283647, 0.958929, 0, 0)
