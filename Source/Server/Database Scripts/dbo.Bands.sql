CREATE TABLE [dbo].[Bands] (
    [Id]   INT           NOT NULL,
    [Name] VARCHAR (200) NOT NULL,
	[CreatedDate]  DATETIME2 (7) NOT NULL,
    [UpdatedDate]  DATETIME2 (7) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

