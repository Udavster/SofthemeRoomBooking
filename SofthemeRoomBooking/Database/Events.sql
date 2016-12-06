CREATE TABLE [dbo].[Events](
	[Id] INT IDENTITY PRIMARY KEY,
	[Title] NVARCHAR(100),
	[Description] NVARCHAR(500),
	[Id_user] NVARCHAR(128) NOT NULL,
	[Nickname] NVARCHAR(150), --what we forgot WWF
	[Id_room] INT NOT NULL,
	[Start] SMALLDATETIME NOT NULL,
	[Finish] SMALLDATETIME NOT NULL, --turns out that 'end' is a keyword
	[Publicity] BIT NOT NULL,
	CONSTRAINT fk_EventRoom FOREIGN KEY ([Id_room])
		REFERENCES Rooms([Id]),
	CONSTRAINT fk_EventCreator FOREIGN KEY ([Id_user])
		REFERENCES AspNetUsers(id)
)

GO
