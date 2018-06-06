DROP DATABASE IF EXISTS ace_config;

CREATE DATABASE ace_config DEFAULT CHARACTER SET utf8;

USE ace_config;

CREATE TABLE `string_stat`(
	`key`	varchar(45) PRIMARY KEY,
    `value` varchar(45)
);

CREATE TABLE `integer_stat`(
	`key`	varchar(45) PRIMARY KEY,
    `value` int(10)
);

CREATE TABLE `float_stat`(
	`key`	varchar(45) PRIMARY KEY,
    `value`	float(8)
);

CREATE TABLE `bool_stat`(
	`key`	varchar(45) PRIMARY KEY,
    `value` bool
);