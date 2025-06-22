IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [Role] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250622160702_InitialCreate', N'8.0.17');
GO

COMMIT;
GO

