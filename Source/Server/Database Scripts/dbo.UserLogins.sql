CREATE TABLE [dbo].[UserLogins] (
    [UserId]       INT           NOT NULL,
    [PasswordHash] VARCHAR (MAX) NOT NULL,
	[CreatedDate]  DATETIME2 (7) NOT NULL,
    [UpdatedDate]  DATETIME2 (7) NULL,
    CONSTRAINT [FK_UserLogins_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

