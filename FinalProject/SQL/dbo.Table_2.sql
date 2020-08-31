CREATE TABLE [dbo].[Order]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[ScheduleID] INT NOT NULL,
	[SiteID] INT NOT NULL,
	[PaymentInfo] NVARCHAR(1000) NOT NULL,
	[State] INT NOT NULL, 
    CONSTRAINT [FK_Order_ToSchedule] FOREIGN KEY ([ScheduleID]) REFERENCES [Schedule]([Id])
)
