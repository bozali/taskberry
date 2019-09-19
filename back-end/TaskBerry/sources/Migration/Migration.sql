CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(95) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
);

CREATE TABLE `Group` (
    `Id` char(36) NOT NULL,
    `Name` varchar(50) NOT NULL,
    `Description` longtext NULL,
    CONSTRAINT `PK_Group` PRIMARY KEY (`Id`)
);

CREATE TABLE `Task` (
    `Id` char(36) NOT NULL,
    `Title` varchar(96) NULL,
    `Description` longtext NULL,
    `Status` int NOT NULL,
    `Type` int NOT NULL,
    `OwnerId` varchar(36) NULL,
    `AssigneeId` int NULL,
    `Row` int NOT NULL,
    CONSTRAINT `PK_Task` PRIMARY KEY (`Id`)
);

CREATE TABLE `User` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `FirstName` varchar(50) NOT NULL,
    `LastName` varchar(50) NOT NULL,
    `Email` varchar(62) NOT NULL,
    CONSTRAINT `PK_User` PRIMARY KEY (`Id`)
);

CREATE TABLE `GroupAssignment` (
    `Id` char(36) NOT NULL,
    `UserId` int NOT NULL,
    `GroupId` char(36) NOT NULL,
    CONSTRAINT `PK_GroupAssignment` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_GroupAssignment_Group_GroupId` FOREIGN KEY (`GroupId`) REFERENCES `Group` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_GroupAssignment_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `UserInfo` (
    `Id` char(36) NOT NULL,
    `UserId` int NOT NULL,
    `Password` longtext NULL,
    CONSTRAINT `PK_UserInfo` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_UserInfo_User_UserId` FOREIGN KEY (`UserId`) REFERENCES `User` (`Id`) ON DELETE CASCADE
);

CREATE INDEX `IX_GroupAssignment_GroupId` ON `GroupAssignment` (`GroupId`);

CREATE INDEX `IX_GroupAssignment_UserId` ON `GroupAssignment` (`UserId`);

CREATE INDEX `IX_UserInfo_UserId` ON `UserInfo` (`UserId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20190919174749_InitialCreate', '2.2.6-servicing-10079');

