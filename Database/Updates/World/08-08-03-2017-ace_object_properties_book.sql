--
-- Table structure for table `ace_object_properties_book`
--

DROP TABLE IF EXISTS `ace_object_properties_book`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `ace_object_properties_book` (
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT '0',
  `page` int(10) unsigned NOT NULL DEFAULT '0',
  `authorName` varchar(255) NOT NULL,
  `authorAccount` varchar(255) NOT NULL,
  `authorId` int(10) unsigned NOT NULL DEFAULT '0',
  `ignoreAuthor` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `pageText` text NOT NULL,
  PRIMARY KEY (`aceObjectId`,`page`),
  UNIQUE KEY `ace_object__property_book_id` (`aceObjectId`,`page`),
  KEY `aceObjectId` (`aceObjectId`),
  CONSTRAINT `fk_Prop_Book_AceObject` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;
