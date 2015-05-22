CREATE TABLE [dbo].[Participations] (
    [Id]       INT NOT NULL,
    [UserId]   INT NOT NULL,
    [BandId]   INT NOT NULL,
    [IsLeader] BIT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Participations_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

