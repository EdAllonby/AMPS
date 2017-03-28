CREATE TABLE [dbo].[Participations] (
    [Id]       INT NOT NULL,
    [UserId]   INT NOT NULL,
    [BandId]   INT NOT NULL,
    [IsLeader] BIT NOT NULL,
	[CreatedDate]  DATETIME2 (7) NOT NULL,
    [UpdatedDate]  DATETIME2 (7) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Participations_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

