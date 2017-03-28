CREATE TABLE [dbo].[Jams] (
    [Id]       INT           NOT NULL,
    [BandId]   INT           NOT NULL,
    [EndDate]  DATETIME2 (7) NOT NULL,
    [IsActive] BIT           NOT NULL,
	[CreatedDate]  DATETIME2 (7) NOT NULL,
    [UpdatedDate]  DATETIME2 (7) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Jams_Bands] FOREIGN KEY ([BandId]) REFERENCES [dbo].[Bands] ([Id])
);

