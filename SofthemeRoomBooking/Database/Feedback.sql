CREATE TABLE [dbo].[Feedback]
(
	id INT IDENTITY PRIMARY KEY,
	email NVARCHAR(256) NOT NULL, --decided to leave it like that
	message NVARCHAR(500) NOT NULL,
	name NVARCHAR(50) NOT NULL,
	surname NVARCHAR(50) NOT NULL,
	created SMALLDATETIME NOT NULL
);

GO
