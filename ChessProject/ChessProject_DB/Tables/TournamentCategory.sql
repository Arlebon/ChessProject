CREATE TABLE [dbo].[TournamentCategory]
(
	TournamentId INT NOT NULL,
    CategoryId INT NOT NULL,
    PRIMARY KEY (TournamentId, CategoryId),
    FOREIGN KEY (TournamentId) REFERENCES Tournament(Id) ON DELETE CASCADE,
    FOREIGN KEY (CategoryId) REFERENCES Category(Id) ON DELETE CASCADE
)