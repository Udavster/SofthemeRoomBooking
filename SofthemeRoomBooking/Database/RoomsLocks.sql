﻿CREATE TABLE [dbo].[RoomsLocks]
(
	[Id] INT IDENTITY PRIMARY KEY,
	[IdRoom] INT NOT NULL,
	[IdUser] NVARCHAR(128) NOT NULL,
	[Start] SMALLDATETIME NOT NULL,
	[Finish] SMALLDATETIME,
	CONSTRAINT fk_RoomRoomLock FOREIGN KEY ([IdRoom])
		REFERENCES Rooms([Id]),
	CONSTRAINT fk_UsersRoomLock FOREIGN KEY ([IdUser])
		REFERENCES AspNetUsers([Id]),
)