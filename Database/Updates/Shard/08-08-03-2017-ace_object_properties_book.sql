/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `ace_shard`
--

-- --------------------------------------------------------

--
-- Table structure for table `ace_object_properties_book`
--

CREATE TABLE `ace_object_properties_book` (
  `aceObjectId` int(10) UNSIGNED NOT NULL,
  `page` int(10) UNSIGNED NOT NULL,
  `authorName` varchar(255) NOT NULL,
  `authorAccount` varchar(255) NOT NULL,
  `authorId` int(10) UNSIGNED NOT NULL,
  `pageText` text NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `ace_object_properties_book`
--
ALTER TABLE `ace_object_properties_book`
  ADD UNIQUE KEY `ace_objectId__page` (`aceObjectId`,`page`) USING BTREE,
  ADD KEY `aceObjectId` (`aceObjectId`) USING BTREE;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
