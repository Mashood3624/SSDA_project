-- MySQL dump 10.13  Distrib 8.0.23, for Win64 (x86_64)
--
-- Host: localhost    Database: ssda
-- ------------------------------------------------------
-- Server version	8.0.23

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `results`
--

DROP TABLE IF EXISTS `results`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `results` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(45) NOT NULL,
  `Course` varchar(45) NOT NULL,
  `Date` varchar(45) NOT NULL,
  `Detection` varchar(45) DEFAULT NULL,
  `Emotion` varchar(45) DEFAULT NULL,
  `Start_Time` varchar(45) DEFAULT NULL,
  `Time` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=38 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `results`
--

LOCK TABLES `results` WRITE;
/*!40000 ALTER TABLE `results` DISABLE KEYS */;
INSERT INTO `results` VALUES (1,'Mashood','SSDA','24/05/2021','0',NULL,NULL,NULL),(2,'Mashood','SSDA','24/05/2021','True',NULL,NULL,NULL),(3,'Mashood','SSDA','24/05/2021','0',NULL,NULL,NULL),(4,'Mashood','SSDA','24/05/2021','True',NULL,NULL,NULL),(5,'Mashood','SSDA','24/05/2021','0',NULL,NULL,NULL),(6,'Mashood','SSDA','24/05/2021','True',NULL,NULL,NULL),(7,'Mashood','SSDA','24/05/2021','0','',NULL,NULL),(8,'Mashood','SSDA','24/05/2021','True','sad',NULL,NULL),(9,'mashood','SSDA','24/05/2021','0','',NULL,NULL),(10,'mashood','SSDA','24/05/2021','True','sad',NULL,NULL),(11,'mashood','SSDA','24/05/2021','True','neutral',NULL,NULL),(12,'mashood','SSDA','24/05/2021','0','',NULL,NULL),(13,'mashood','SSDA','24/05/2021','True','sad',NULL,NULL),(14,'mashood','SSDA','24/05/2021','True','neutral',NULL,NULL),(15,'mashood','SSDA','24/05/2021','True','sad',NULL,NULL),(16,'mashood','SSDA','24/05/2021','0','',NULL,NULL),(17,'mashood','SSDA','25/05/2021','0','',NULL,NULL),(19,'mashood','SQE','07/06/2021','True','',NULL,NULL),(20,'mashood','SQE','07/06/2021','True','',NULL,NULL),(21,'mashood','SQE','07/06/2021','True','',NULL,NULL),(22,'mashood','SQE','07/06/2021','True','',NULL,NULL),(23,'mashood','SQE','07/06/2021','True','',NULL,NULL),(24,'mashood','SQE','07/06/2021','0','',NULL,NULL),(25,'mashood','SQE','07/06/2021','0','',NULL,NULL),(26,'mashood','SQE','07/06/2021','0','',NULL,NULL),(27,'mashood','SQE','07/06/2021','0','',NULL,NULL),(28,'mashood','SQE','07/06/2021','0','',NULL,NULL),(29,'mashood','AAI','07/06/2021','True','neutral','22:10:56','30'),(30,'mashood','AAI','07/06/2021','True','angry','22:10:56','60'),(31,'mashood','AAI','07/06/2021','0','','22:10:56','90'),(32,'mashood','AAI','07/06/2021','0','','22:10:56','120'),(33,'mashood','AAI','07/06/2021','0','','22:10:56','150'),(34,'mashood','AAI','07/06/2021','0','','22:10:56','180'),(35,'mashood','AAI','07/06/2021','True','0','22:10:56','210'),(36,'mashood','AAI','07/06/2021','0','','22:10:56','240'),(37,'mashood','AAI','07/06/2021','True','neutral','22:10:56','270');
/*!40000 ALTER TABLE `results` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-06-15 19:40:53
