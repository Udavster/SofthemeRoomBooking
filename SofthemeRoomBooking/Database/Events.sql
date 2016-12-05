CREATE TABLE [dbo].[Events]
(
	id INT IDENTITY PRIMARY KEY,
	title NVARCHAR(100),
	description NVARCHAR(500),
	id_user NVARCHAR(128) NOT NULL,
 	nickname NVARCHAR(150), --what we forgot WWF
 	id_room INT NOT NULL,
 	start SMALLDATETIME NOT NULL,
 	finish SMALLDATETIME NOT NULL, --turns out that 'end' is a keyword
 	publicity BIT NOT NULL,
 	CONSTRAINT fk_EventRoom FOREIGN KEY (id_room)
 		REFERENCES Rooms(id),
 	CONSTRAINT fk_EventCreator FOREIGN KEY (id_user)
 		REFERENCES AspNetUsers(id)
);

GO
