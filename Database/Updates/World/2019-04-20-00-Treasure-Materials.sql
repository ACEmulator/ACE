SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `ace_world`
--

-- --------------------------------------------------------

--
-- Table structure for table `treasure_material_base`
--

DROP TABLE IF EXISTS `treasure_material_base`;
CREATE TABLE `treasure_material_base` (
  `id` int(10) UNSIGNED NOT NULL,
  `material_Code` int(10) UNSIGNED NOT NULL COMMENT 'Derived from PropertyInt.TsysMutationData',
  `tier` int(10) UNSIGNED NOT NULL COMMENT 'Loot Tier',
  `probability` float NOT NULL,
  `material_Id` int(10) UNSIGNED NOT NULL COMMENT 'MaterialType'
) ENGINE=MyISAM DEFAULT CHARSET=utf8;


--
-- Indexes for dumped tables
--

--
-- Indexes for table `treasure_material_base`
--
ALTER TABLE `treasure_material_base`
  ADD PRIMARY KEY (`id`),
  ADD KEY `tier` (`tier`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `treasure_material_base`
--
ALTER TABLE `treasure_material_base`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `ace_world`
--

-- --------------------------------------------------------

--
-- Table structure for table `treasure_material_groups`
--

DROP TABLE IF EXISTS `treasure_material_groups`;
CREATE TABLE `treasure_material_groups` (
  `id` int(10) UNSIGNED NOT NULL,
  `material_Group` int(10) UNSIGNED NOT NULL COMMENT 'MaterialType Group',
  `tier` int(10) UNSIGNED NOT NULL COMMENT 'Loot Tier',
  `probability` float NOT NULL,
  `material_Id` int(10) UNSIGNED NOT NULL COMMENT 'MaterialType'
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `treasure_material_groups`
--
ALTER TABLE `treasure_material_groups`
  ADD PRIMARY KEY (`id`),
  ADD KEY `tier` (`tier`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `treasure_material_groups`
--
ALTER TABLE `treasure_material_groups`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `ace_world`
--

-- --------------------------------------------------------

--
-- Table structure for table `treasure_material_color`
--

DROP TABLE IF EXISTS `treasure_material_color`;
CREATE TABLE `treasure_material_color` (
  `id` int(10) UNSIGNED NOT NULL,
  `material_Id` int(10) UNSIGNED NOT NULL,
  `color_Code` int(10) UNSIGNED NOT NULL,
  `palette_Template` int(10) UNSIGNED NOT NULL,
  `probability` float NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `treasure_material_color`
--
ALTER TABLE `treasure_material_color`
  ADD PRIMARY KEY (`id`),
  ADD KEY `material_Id` (`material_Id`),
  ADD KEY `tsys_Mutation_Color` (`color_Code`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `treasure_material_color`
--
ALTER TABLE `treasure_material_color`
  MODIFY `id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
