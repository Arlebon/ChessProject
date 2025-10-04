CREATE TABLE [dbo].[Tournament]
(
	Id INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Location NVARCHAR(200) NULL,

    MinPlayers INT NOT NULL CHECK (MinPlayers BETWEEN 2 AND 32)
        CHECK (MinPlayers < MaxPlayers),
    MaxPlayers INT NOT NULL CHECK (MaxPlayers BETWEEN 2 AND 32),

    CurrentPlayers INT CHECK (CurrentPlayers < MaxPlayers),

    MinElo INT NULL CHECK (MinElo BETWEEN 0 AND 3000)
        CHECK (MinElo < MaxElo),
    MaxElo INT NULL CHECK (MaxElo BETWEEN 0 AND 3000),

    [Status] int NOT NULL DEFAULT(0),

    CurrentRound INT NOT NULL DEFAULT 0,
    WomenOnly BIT NOT NULL DEFAULT 0,

    RegistrationDeadline DATE NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME()
        
)
