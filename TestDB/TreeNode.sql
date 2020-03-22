CREATE TABLE [dbo].[TreeNode] (
    [Id]          INT  IDENTITY (1, 1) NOT NULL,
    [ParentId]    INT  NULL,
    [Name]        TEXT NOT NULL,
    [OrderId]     INT  NOT NULL,
    [IsDirectory] BIT  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);