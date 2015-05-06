CREATE TABLE [dbo].[Article] (
    [ID]          UNIQUEIDENTIFIER NOT NULL,
    [CategoryID]  UNIQUEIDENTIFIER NOT NULL,
    [Subject]     NVARCHAR (100)   NOT NULL,
    [Summary]     NVARCHAR (1024)  NOT NULL,
    [ContentText] NVARCHAR (MAX)   NOT NULL,
    [IsPublish]   BIT              CONSTRAINT [DF_Article_IsPublish] DEFAULT ((0)) NOT NULL,
    [PublishDate] DATETIME         NOT NULL,
    [ViewCount]   INT              CONSTRAINT [DF_Article_ViewCount] DEFAULT ((0)) NOT NULL,
    [CreateUser]  UNIQUEIDENTIFIER NOT NULL,
    [CreateDate]  DATETIME         CONSTRAINT [DF_Article_CreateDate] DEFAULT (getdate()) NOT NULL,
    [UpdateUser]  UNIQUEIDENTIFIER NULL,
    [UpdateDate]  DATETIME         CONSTRAINT [DF_Article_UpdateDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Article] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Article_Category] FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[Category] ([ID])
);

