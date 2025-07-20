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

CREATE TABLE [Games] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(100) NOT NULL,
    [Description] nvarchar(500) NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [Genre] int NOT NULL,
    [ReleaseDate] datetime2 NOT NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    CONSTRAINT [PK_Games] PRIMARY KEY ([Id])
);
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

CREATE TABLE [Promotions] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(100) NOT NULL,
    [DiscountPercentage] decimal(5,2) NOT NULL,
    [StartDate] datetime2 NOT NULL,
    [EndDate] datetime2 NOT NULL,
    [GameId] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    CONSTRAINT [PK_Promotions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Promotions_Games_GameId] FOREIGN KEY ([GameId]) REFERENCES [Games] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [OwnedGames] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [GameId] int NOT NULL,
    [PurchaseDate] datetime2 NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    [CreatedAt] datetime2 NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    CONSTRAINT [PK_OwnedGames] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_OwnedGames_Games_GameId] FOREIGN KEY ([GameId]) REFERENCES [Games] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_OwnedGames_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_OwnedGames_GameId] ON [OwnedGames] ([GameId]);
GO

CREATE INDEX [IX_OwnedGames_UserId] ON [OwnedGames] ([UserId]);
GO

CREATE INDEX [IX_Promotions_GameId] ON [Promotions] ([GameId]);
GO

CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250720164355_InitialCreate', N'8.0.17');
GO

COMMIT;
GO

