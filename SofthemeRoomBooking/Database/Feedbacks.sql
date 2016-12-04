CREATE TABLE [dbo].[Feedbacks]
(
	id INT IDENTITY PRIMARY KEY,
	email NVARCHAR(256),
	message NVARCHAR(500),
	name NVARCHAR(50),
	surname NVARCHAR(50),
	created SMALLDATETIME
)
