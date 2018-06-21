USE ace_shard;

DROP TABLE IF EXISTS `config_properties_string`;
CREATE TABLE IF NOT EXISTS `config_properties_string`(
	`key`	        VARCHAR(255) PRIMARY KEY,
    `value`         TEXT NOT NULL,
    `description`   TEXT
);

DROP TABLE IF EXISTS `config_properties_long`;
CREATE TABLE IF NOT EXISTS `config_properties_long`(
	`key`	        VARCHAR(255) PRIMARY KEY,
    `value`         BIGINT NOT NULL,
    `description`   TEXT
);

DROP TABLE IF EXISTS `config_properties_double`;
CREATE TABLE IF NOT EXISTS `config_properties_double`(
	`key`	        VARCHAR(255) PRIMARY KEY,
    `value`	        REAL NOT NULL,
    `description`   TEXT
);

DROP TABLE IF EXISTS `config_properties_boolean`;
CREATE TABLE IF NOT EXISTS `config_properties_boolean`(
	`key`	        VARCHAR(255) PRIMARY KEY,
    `value`         BIT(1) NOT NULL,
    `description`   TEXT
);
