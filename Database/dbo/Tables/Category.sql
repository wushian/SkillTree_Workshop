CREATE TABLE [dbo].[Category] (
    [ID]         UNIQUEIDENTIFIER NOT NULL,
    [Name]       NVARCHAR (20)    NOT NULL,
    [CreateUser] UNIQUEIDENTIFIER NOT NULL,
    [CreateDate] DATETIME         CONSTRAINT [DF_Category_CreateDate] DEFAULT (getdate()) NOT NULL,
    [UpdateUser] UNIQUEIDENTIFIER NULL,
    [UpdateDate] DATETIME         CONSTRAINT [DF_Category_UpdateDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED ([ID] ASC)
);

