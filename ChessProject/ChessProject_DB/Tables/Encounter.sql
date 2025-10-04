CREATE TABLE [dbo].[Encounter]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	TournamentID INT NOT NULL,
	Plyr_WhiteId INT NOT NULL,
	Plyr_BlackId INT NOT NULL,
	[Round] INT NOT NULL,
	[Result] NVARCHAR(50) NOT NULL
		CONSTRAINT DF_Encounter_Result DEFAULT ('not played'),

	CONSTRAINT FK_Encounter_Tournament FOREIGN KEY (TournamentID) REFERENCES Tournament(Id)
	
)
