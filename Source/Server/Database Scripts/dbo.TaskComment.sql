CREATE TABLE [dbo].[TaskComments] (
    [Id]             INT           NOT NULL,
    [TaskId]         INT           NOT NULL,
    [CommenterId]	 INT		   NOT NULL,
    [ParentCommentId]    INT NOT NULL,
    [Comment] VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TaskComments_Tasks] FOREIGN KEY ([TaskId]) REFERENCES [dbo].[Tasks] ([Id]),
    CONSTRAINT [FK_TaskComments_Users] FOREIGN KEY ([CommenterId]) REFERENCES [dbo].[Users] ([Id]),
    CONSTRAINT [FK_TaskComments_TaskComments] FOREIGN KEY ([ParentCommentId]) REFERENCES [dbo].[TaskComments] ([Id]),
);

