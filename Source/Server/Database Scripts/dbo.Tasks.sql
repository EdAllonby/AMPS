CREATE TABLE [dbo].[Tasks] (
    [Id]             INT           NOT NULL,
    [BandId]         INT           NOT NULL,
    [Title]          VARCHAR (200) NOT NULL,
    [Description]    VARCHAR (MAX) NOT NULL,
    [AssignedUserId] INT           NULL,
    [IsCompleted]    BIT           NOT NULL,
    [Points]         INT           NOT NULL,
    [JamId]          INT           NULL,
    [TaskCategoryId] INT           NOT NULL,
	[CreatedDate]  DATETIME2 (7) NOT NULL,
    [UpdatedDate]  DATETIME2 (7) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Tasks_Bands] FOREIGN KEY ([BandId]) REFERENCES [dbo].[Bands] ([Id]),
    CONSTRAINT [FK_Tasks_Users] FOREIGN KEY ([AssignedUserId]) REFERENCES [dbo].[Users] ([Id]),
    CONSTRAINT [FK_Tasks_Jams] FOREIGN KEY ([JamId]) REFERENCES [dbo].[Jams] ([Id]),
    CONSTRAINT [FK_Tasks_TaskCategories] FOREIGN KEY ([TaskCategoryId]) REFERENCES [dbo].[TaskCategories] ([Id])
);

