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

CREATE TABLE `UserInfo` (
    `Id` char(36) NOT NULL,
    `UserId` int NOT NULL,
    `Password` longtext NULL,
    CONSTRAINT `PK_UserInfo` PRIMARY KEY (`Id`)
);

CREATE TABLE `GroupAssignment` (
    `Id` char(36) NOT NULL,
    `UserId` int NOT NULL,
    `GroupId` char(36) NOT NULL,
    CONSTRAINT `PK_GroupAssignment` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_GroupAssignment_Group_GroupId` FOREIGN KEY (`GroupId`) REFERENCES `Group` (`Id`) ON DELETE CASCADE
);

CREATE INDEX `IX_GroupAssignment_GroupId` ON `GroupAssignment` (`GroupId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20190923164348_InitialTaskBerryCreate', '2.2.6-servicing-10079');

