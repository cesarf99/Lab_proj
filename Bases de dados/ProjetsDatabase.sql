CREATE DATABASE HCILabProject;
USE HCILabProject;

Create table Project(
	ID_Project INTEGER NOT NULL AUTO_INCREMENT,
    Title VARCHAR(1000) NOT NULL,
    Description VARCHAR(2000) NULL,
	Financer VARCHAR(100) NULL,
	Email VARCHAR(100)  NULL,
	Country	VARCHAR(100) NULL,
	Url VARCHAR(1000) NULL,
    Tipo VARCHAR(100) NULL,
    GrantNumber VARCHAR(100) NULL,
    Putcode LONG NULL,
    PRIMARY KEY(ID_Project)
);

Create table ConnectionResearcher(
	ID INTEGER NOT NULL AUTO_INCREMENT,
    OrcidID VARCHAR(50) NOT NULL ,
    ProjectID INTEGER NOT NULL,
    PRIMARY KEY(ID),
    FOREIGN KEY(ProjectID) References Project(ID_Project)
);

CREATE TABLE ConnectionPublication(
	ID INTEGER NOT NULL AUTO_INCREMENT,
    ProjectID INTEGER NOT NULL,
    Doi VARCHAR(100) NOT NULL,
    PRIMARY KEY(ID),
    FOREIGN KEY(ProjectID) References Project(ID_Project)
);




