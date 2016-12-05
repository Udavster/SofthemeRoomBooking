CREATE TABLE [dbo].[EquipmentRooms]
(
	id INT IDENTITY PRIMARY KEY,
	id_room INT NOT NULL,
	id_equipment INT NOT NULL,
	quantity INT NOT NULL
	CONSTRAINT fk_ERtoRoom FOREIGN KEY (id_room)
		REFERENCES Rooms(id),
	CONSTRAINT fk_ERtoEquipment FOREIGN KEY (id_equipment)
		REFERENCES Equipment(id)
);

GO
