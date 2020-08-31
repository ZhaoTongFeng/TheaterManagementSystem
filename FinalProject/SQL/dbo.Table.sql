CREATE TABLE [dbo].[Movie] (
    [Id]           INT             NOT NULL,
    [Name]         NVARCHAR (100)  NOT NULL,
    [Length]       INT             NOT NULL,
    [Type]         NVARCHAR (100)  NOT NULL,
    [Director]     NVARCHAR (1000) NOT NULL,
    [Scriptwriter] NVARCHAR (1000) NOT NULL,
    [Actor]        NVARCHAR (1000) NOT NULL,
    [Country]      NVARCHAR (100)  NOT NULL,
    [Language]     NVARCHAR (100)  NOT NULL,
    [Price]        INT             NOT NULL,
    [Introduce]    NVARCHAR (1000) NOT NULL,
    [img_path]     NVARCHAR (100)  NOT NULL,
    [State]        INT             NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

