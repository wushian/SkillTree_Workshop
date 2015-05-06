CREATE TABLE [dbo].[Photo] (
    [ID]          UNIQUEIDENTIFIER NOT NULL,
    [ArticleID]   UNIQUEIDENTIFIER NOT NULL,
    [FileName]    NVARCHAR (128)   NOT NULL,
    [Description] NVARCHAR (128)   NULL,
    [CreateUser]  UNIQUEIDENTIFIER NOT NULL,
    [CreateDate]  DATETIME         CONSTRAINT [DF_Photo_CreateDate] DEFAULT (getdate()) NOT NULL,
    [UpdateUser]  UNIQUEIDENTIFIER NULL,
    [UpdateDate]  DATETIME         CONSTRAINT [DF_Photo_UpdateDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Photo] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Photo_Article] FOREIGN KEY ([ArticleID]) REFERENCES [dbo].[Article] ([ID])
);

