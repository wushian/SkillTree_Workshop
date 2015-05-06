CREATE TABLE [dbo].[SystemUser] (
    [ID]         UNIQUEIDENTIFIER NOT NULL,
    [Name]       NVARCHAR (50)    NOT NULL,
    [Account]    NVARCHAR (50)    NOT NULL,
    [Password]   NVARCHAR (100)   NOT NULL,
    [Salt]       NVARCHAR (50)    NOT NULL,
    [Email]      NVARCHAR (256)   NOT NULL,
    [CreateUser] UNIQUEIDENTIFIER NOT NULL,
    [CreateDate] DATETIME         CONSTRAINT [DF_SystemUser_CreateDate] DEFAULT (getdate()) NOT NULL,
    [UpdateUser] UNIQUEIDENTIFIER NULL,
    [UpdateDate] DATETIME         CONSTRAINT [DF_SystemUser_UpdateDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_SystemUser] PRIMARY KEY CLUSTERED ([ID] ASC)
);

