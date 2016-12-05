CREATE TABLE [dbo].[EquipmentRooms](
	[Id] INT IDENTITY PRIMARY KEY,
	[Id_room] INT NOT NULL,
	[Id_equipment] INT NOT NULL,
	[Quantity] INT NOT NULL
	CONSTRAINT fk_ERtoRoom FOREIGN KEY ([Id_room])
		REFERENCES Rooms([Id]),
	CONSTRAINT fk_ERtoEquipment FOREIGN KEY ([Id_equipment])
		REFERENCES Equipment([Id])
)
