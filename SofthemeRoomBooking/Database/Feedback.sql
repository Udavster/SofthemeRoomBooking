CREATE TABLE [dbo].[Feedback](
	[Id] INT IDENTITY PRIMARY KEY,
	[Email] NVARCHAR(256) NOT NULL, --decided to leave it like that
	[Message] NVARCHAR(500) NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,
	[Surname] NVARCHAR(50) NOT NULL,
	[Created] SMALLDATETIME NOT NULL
) 
