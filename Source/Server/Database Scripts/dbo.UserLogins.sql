CREATE TABLE [dbo].[UserLogins] (
    [UserId]       INT           NOT NULL,
    [PasswordHash] VARCHAR (MAX) NOT NULL,
    CONSTRAINT [FK_UserLogins_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

