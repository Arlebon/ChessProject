CREATE TABLE [dbo].[TournamentUser]
(
	TournamentId INT NOT NULL,
	UserId INT NOT NULL,
	CONSTRAINT PK_TournamentUser PRIMARY KEY (TournamentId, UserId),
	CONSTRAINT FK_Tournament FOREIGN KEY (TournamentId) REFERENCES Tournament(Id),
	CONSTRAINT FK_User FOREIGN KEY (UserId) REFERENCES [User](Id)
)
