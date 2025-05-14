-- MySQL Administrator dump 1.4
--
-- ------------------------------------------------------
-- Server version	5.0.18-nt


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


--
-- Create schema posisdb
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ posisdb;
USE posisdb;

--
-- Table structure for table `posisdb`.`category`
--

DROP TABLE IF EXISTS `category`;
CREATE TABLE `category` (
  `CategoryNo` int(10) unsigned NOT NULL auto_increment,
  `CategoryName` varchar(45) NOT NULL default '',
  `Description` varchar(150) NOT NULL default '',
  PRIMARY KEY  (`CategoryNo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `posisdb`.`category`
--

/*!40000 ALTER TABLE `category` DISABLE KEYS */;
INSERT INTO `category` (`CategoryNo`,`CategoryName`,`Description`) VALUES 
 (1,'Bags','All bags will belong to this category'),
 (2,'SHOES','SHOES'),
 (3,'Gadget','Gadget');
/*!40000 ALTER TABLE `category` ENABLE KEYS */;


--
-- Table structure for table `posisdb`.`payment`
--

DROP TABLE IF EXISTS `payment`;
CREATE TABLE `payment` (
  `paymentNo` int(10) unsigned NOT NULL auto_increment,
  `InvoiceNo` int(10) unsigned NOT NULL default '0',
  `Cash` double NOT NULL default '0',
  `PChange` double NOT NULL default '0',
  PRIMARY KEY  (`paymentNo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `posisdb`.`payment`
--

/*!40000 ALTER TABLE `payment` DISABLE KEYS */;
INSERT INTO `payment` (`paymentNo`,`InvoiceNo`,`Cash`,`PChange`) VALUES 
 (1,100100001,2000,500),
 (2,100100002,3000,151),
 (3,100100003,2000,0),
 (4,100100004,20000,0),
 (5,100100005,2000,500),
 (6,100100006,10000,0),
 (7,100100007,2000,0),
 (8,100100008,2000,0),
 (9,100100009,2000,0),
 (10,100100009,2000,0),
 (11,100100010,2000,0),
 (12,100100011,2000,0),
 (13,100100012,2000,0),
 (14,100100013,2000,0),
 (15,100100014,200000,0),
 (16,100100015,20000,0),
 (17,100100016,2000,0);
/*!40000 ALTER TABLE `payment` ENABLE KEYS */;


--
-- Table structure for table `posisdb`.`product`
--

DROP TABLE IF EXISTS `product`;
CREATE TABLE `product` (
  `ProductNo` int(10) unsigned NOT NULL auto_increment,
  `ProductCode` varchar(45) NOT NULL default '',
  `Description` varchar(200) NOT NULL default '',
  `Barcode` varchar(50) NOT NULL default '',
  `UnitPrice` double NOT NULL default '0',
  `StocksOnHand` int(10) unsigned NOT NULL default '0',
  `ReorderLevel` int(10) unsigned NOT NULL default '0',
  `CategoryNo` int(10) unsigned NOT NULL default '0',
  PRIMARY KEY  (`ProductNo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `posisdb`.`product`
--

/*!40000 ALTER TABLE `product` DISABLE KEYS */;
INSERT INTO `product` (`ProductNo`,`ProductCode`,`Description`,`Barcode`,`UnitPrice`,`StocksOnHand`,`ReorderLevel`,`CategoryNo`) VALUES 
 (1,'PBAG','Pack Bag Jansport Large','133242342',1500,16,5,1),
 (2,'NSHOE','NIKE SHOES','133242341',2000,15,2,2),
 (3,'LTOP','Toshiba Laptop','133242345',20000,14,1,3);
/*!40000 ALTER TABLE `product` ENABLE KEYS */;


--
-- Table structure for table `posisdb`.`staff`
--

DROP TABLE IF EXISTS `staff`;
CREATE TABLE `staff` (
  `StaffID` int(10) unsigned NOT NULL auto_increment,
  `Lastname` varchar(45) NOT NULL default '',
  `Firstname` varchar(45) NOT NULL default '',
  `MI` varchar(1) NOT NULL default '',
  `Street` varchar(45) NOT NULL default '',
  `Barangay` varchar(45) NOT NULL default '',
  `City` varchar(45) NOT NULL default '',
  `Province` varchar(45) NOT NULL default '',
  `ContactNo` varchar(45) NOT NULL default '',
  `Username` varchar(45) NOT NULL default '',
  `Role` varchar(45) NOT NULL default '',
  `UPassword` varchar(45) NOT NULL default '',
  PRIMARY KEY  (`StaffID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `posisdb`.`staff`
--

/*!40000 ALTER TABLE `staff` DISABLE KEYS */;
INSERT INTO `staff` (`StaffID`,`Lastname`,`Firstname`,`MI`,`Street`,`Barangay`,`City`,`Province`,`ContactNo`,`Username`,`Role`,`UPassword`) VALUES 
 (1,'System','Admin','','Purok B','Balindog','Kidapawan','North Cotabato','09466903269','admin','Admin','admin'),
 (2,'Rojas','Nick','E','Test','Test','Test','Test','0846890823','nick','Cashier','1234');
/*!40000 ALTER TABLE `staff` ENABLE KEYS */;


--
-- Table structure for table `posisdb`.`stockin`
--

DROP TABLE IF EXISTS `stockin`;
CREATE TABLE `stockin` (
  `StockInNo` int(10) unsigned NOT NULL auto_increment,
  `ProductNo` int(10) unsigned NOT NULL default '0',
  `Quantity` double NOT NULL default '0',
  `DateIn` varchar(45) NOT NULL default '',
  PRIMARY KEY  (`StockInNo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `posisdb`.`stockin`
--

/*!40000 ALTER TABLE `stockin` DISABLE KEYS */;
INSERT INTO `stockin` (`StockInNo`,`ProductNo`,`Quantity`,`DateIn`) VALUES 
 (1,1,20,'12/31/2013'),
 (2,2,20,'09/30/2015'),
 (3,2,10,'09/30/2015'),
 (4,3,10,'09/30/2015'),
 (5,2,10,'09/30/2015'),
 (6,3,5,'09/30/2015');
/*!40000 ALTER TABLE `stockin` ENABLE KEYS */;


--
-- Table structure for table `posisdb`.`transactiondetails`
--

DROP TABLE IF EXISTS `transactiondetails`;
CREATE TABLE `transactiondetails` (
  `TDetailNo` int(10) unsigned NOT NULL auto_increment,
  `InvoiceNo` varchar(50) NOT NULL default '',
  `ProductNo` int(10) unsigned NOT NULL default '0',
  `ItemPrice` double NOT NULL default '0',
  `Quantity` int(10) unsigned NOT NULL default '0',
  `Discount` double NOT NULL default '0',
  PRIMARY KEY  (`TDetailNo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `posisdb`.`transactiondetails`
--

/*!40000 ALTER TABLE `transactiondetails` DISABLE KEYS */;
INSERT INTO `transactiondetails` (`TDetailNo`,`InvoiceNo`,`ProductNo`,`ItemPrice`,`Quantity`,`Discount`) VALUES 
 (1,'100100000',1,1500,1,0),
 (2,'100100001',1,1500,1,0),
 (3,'100100002',1,1350,1,150),
 (4,'100100003',2,2000,1,0),
 (5,'100100004',3,20000,1,0),
 (6,'100100005',1,1500,1,0),
 (7,'100100006',2,2000,5,0),
 (8,'100100007',2,2000,1,0),
 (9,'100100008',2,2000,1,0),
 (10,'100100009',2,2000,1,0),
 (11,'100100009',2,2000,1,0),
 (12,'100100010',2,2000,1,0),
 (13,'100100011',2,2000,1,0),
 (14,'100100012',2,2000,1,0),
 (15,'100100013',2,2000,1,0),
 (16,'100100014',2,2000,100,0),
 (17,'100100015',2,2000,10,0),
 (18,'100100016',2,2000,1,0);
/*!40000 ALTER TABLE `transactiondetails` ENABLE KEYS */;


--
-- Table structure for table `posisdb`.`transactions`
--

DROP TABLE IF EXISTS `transactions`;
CREATE TABLE `transactions` (
  `InvoiceNo` int(10) unsigned NOT NULL auto_increment,
  `TDate` varchar(45) NOT NULL default '',
  `TTime` varchar(45) NOT NULL default '',
  `NonVatTotal` double NOT NULL default '0',
  `VatAmount` double NOT NULL default '0',
  `TotalAmount` varchar(45) NOT NULL default '',
  `StaffID` int(11) NOT NULL default '0',
  PRIMARY KEY  (`InvoiceNo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `posisdb`.`transactions`
--

/*!40000 ALTER TABLE `transactions` DISABLE KEYS */;
INSERT INTO `transactions` (`InvoiceNo`,`TDate`,`TTime`,`NonVatTotal`,`VatAmount`,`TotalAmount`,`StaffID`) VALUES 
 (100100000,'09/30/2015','04:57:30',1500,0,'1,500.00',1),
 (100100001,'09/30/2015','04:59:36',1500,0,'1,500.00',1),
 (100100002,'09/30/2015','05:01:24',2849,0,'2,849.00',1),
 (100100003,'09/30/2015','05:17:00',1760,240,'2,000.00',1),
 (100100004,'09/30/2015','05:21:54',17600,2400,'20,000.00',1),
 (100100005,'02/09/2016','05:43:26',1320,180,'1,500.00',1),
 (100100006,'03/01/2016','08:53:39',8800,1200,'10,000.00',1),
 (100100007,'03/05/2016','07:09:40',1760,240,'2,000.00',1),
 (100100008,'03/14/2016','07:56:49',1760,240,'2,000.00',1),
 (100100009,'03/25/2016','10:06:33',1760,240,'2,000.00',1),
 (100100010,'03/25/2016','10:08:58',1760,240,'2,000.00',1),
 (100100011,'03/25/2016','10:09:12',1760,240,'2,000.00',1),
 (100100012,'04/18/2016','01:11:02',1760,240,'2,000.00',1),
 (100100013,'05/08/2016','10:36:57',1760,240,'2,000.00',1),
 (100100014,'05/13/2016','10:30:51',176000,24000,'200,000.00',1),
 (100100015,'05/13/2016','10:36:54',17600,2400,'20,000.00',1);
INSERT INTO `transactions` (`InvoiceNo`,`TDate`,`TTime`,`NonVatTotal`,`VatAmount`,`TotalAmount`,`StaffID`) VALUES 
 (100100016,'05/15/2016','05:31:06',1760,240,'2,000.00',1);
/*!40000 ALTER TABLE `transactions` ENABLE KEYS */;


--
-- Table structure for table `posisdb`.`vatsetting`
--

DROP TABLE IF EXISTS `vatsetting`;
CREATE TABLE `vatsetting` (
  `VatNo` int(10) unsigned NOT NULL auto_increment,
  `VatPercent` double NOT NULL default '0',
  PRIMARY KEY  (`VatNo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `posisdb`.`vatsetting`
--

/*!40000 ALTER TABLE `vatsetting` DISABLE KEYS */;
INSERT INTO `vatsetting` (`VatNo`,`VatPercent`) VALUES 
 (1,12);
/*!40000 ALTER TABLE `vatsetting` ENABLE KEYS */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
