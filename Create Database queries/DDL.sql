-- Database Notebook
CREATE DATABASE Notebook COLLATE Persian_100_CI_AS_KS_WS;
go

use Notebook;

-- Table Users
CREATE TABLE Users(
	id Int IDENTITY(1,1) NOT NULL,
	username Nvarchar(25) NOT NULL,
	password Text NULL,
	CONSTRAINT PK_Users PRIMARY KEY (id),
	CONSTRAINT username UNIQUE CLUSTERED (username)
);

-- Table Notes
CREATE TABLE Notes(
	id Int IDENTITY(1,1) NOT NULL,
	writerID Int NOT NULL,
	title Nvarchar(50) NULL,
	text Text NULL,
	CONSTRAINT PK_Notes PRIMARY KEY (id, writerID),
	CONSTRAINT 
		writes FOREIGN KEY (writerID) 
		REFERENCES Users (id) 
		ON UPDATE NO ACTION 
		ON DELETE CASCADE
);

-- Table Logs
CREATE TABLE Logs(
	id Int IDENTITY(1,1) NOT NULL,
	writerID Int NOT NULL,
	noteID Int NOT NULL,
	time Datetime NULL,
	CONSTRAINT PK_Logs PRIMARY KEY (id),
	CONSTRAINT newNote 
		FOREIGN KEY (noteID, writerID) 
		REFERENCES Notes (id, writerID) 
		ON UPDATE NO ACTION 
		ON DELETE NO ACTION
);