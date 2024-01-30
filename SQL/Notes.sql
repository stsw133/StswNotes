﻿CREATE TABLE [dbo].[Notes]
(
	[ID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] VARCHAR(100) NOT NULL,
	[Author] VARCHAR(100) NOT NULL,
	[Document] VARBINARY(MAX) NOT NULL,
	[CreateDT] DATETIME NOT NULL DEFAULT GETDATE()
)
