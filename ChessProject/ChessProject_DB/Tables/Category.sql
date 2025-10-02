CREATE TABLE [dbo].[Category]
(
	Id INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL UNIQUE
        CHECK (Name IN ('junior', 'senior', 'veteran'))
)

