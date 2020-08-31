CREATE TABLE [dbo].[Schedule]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[MovieId] INT NOT NULL,
	[RoomName] NVARCHAR(100) NOT NULL,
	[StartTime] DATE NOT NULL,
	[EndTime] DATE NOT NULL,
	[SiteState] NVARCHAR(200) NOT NULL,
	[State] INT NOT NULL, 
    CONSTRAINT [FK_Schedule_ToMovie] FOREIGN KEY ([MovieId]) REFERENCES [Movie]([Id])
)
