CREATE TABLE [dbo].[TaskCategories] (
    [Id]       INT          NOT NULL,
    [Category] VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

insert into TaskCategories
values (1, 'Guitar'), (2, 'Bass'), (3, 'Drums'), (4, 'Vocals'), (5,'Synth'), (6, 'Mixing'), (7, 'Project'), (8, 'Other')