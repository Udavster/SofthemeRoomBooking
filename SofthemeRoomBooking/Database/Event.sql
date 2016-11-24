CREATE TABLE [dbo].[Event]
(
	id INT IDENTITY PRIMARY KEY,
	title NVARCHAR(100),
	description NVARCHAR(500),
	id_user INT,
	start SMALLDATETIME,
	finish SMALLDATETIME, --turns out that 'end' is a keyword
	publicity BIT
)
