-- phpMyAdmin SQL Dump
-- version 4.6.4
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Generation Time: Apr 28, 2017 at 10:45 AM
-- Server version: 10.0.15-MariaDB
-- PHP Version: 7.0.9

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `ace_world`
--
USE `ace_world`;

DROP TABLE IF EXISTS `portal_destination`;
DROP TABLE IF EXISTS `ace_portal_object`;

-- --------------------------------------------------------

--
-- Table structure for table `ace_portal_object`
--

CREATE TABLE `ace_portal_object` (
  `weenieClassId` smallint(5) UNSIGNED NOT NULL,
  `destLandblockId` int(10) UNSIGNED NOT NULL DEFAULT '0',
  `destX` float NOT NULL DEFAULT '0',
  `destY` float NOT NULL DEFAULT '0',
  `destZ` float NOT NULL DEFAULT '0',
  `destQX` float NOT NULL DEFAULT '0',
  `destQY` float NOT NULL DEFAULT '0',
  `destQZ` float NOT NULL DEFAULT '0',
  `destQW` float NOT NULL DEFAULT '0',
  `min_lvl` int(11) UNSIGNED NOT NULL DEFAULT '0',
  `max_lvl` int(11) UNSIGNED NOT NULL DEFAULT '0',
  `societyId` tinyint(3) UNSIGNED NOT NULL DEFAULT '0',
  `isTieable` tinyint(1) UNSIGNED NOT NULL DEFAULT '1',
  `isRecallable` tinyint(1) UNSIGNED NOT NULL DEFAULT '1',
  `isSummonable` tinyint(1) UNSIGNED NOT NULL DEFAULT '1'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `ace_portal_object`
--
ALTER TABLE `ace_portal_object`
  ADD PRIMARY KEY (`weenieClassId`);

--
-- Constraints for dumped tables
--

--
-- Constraints for table `ace_portal_object`
--
ALTER TABLE `ace_portal_object`
  ADD CONSTRAINT `FK_weenieClassId` FOREIGN KEY (`weenieClassId`) REFERENCES `ace_object` (`weenieClassId`);
