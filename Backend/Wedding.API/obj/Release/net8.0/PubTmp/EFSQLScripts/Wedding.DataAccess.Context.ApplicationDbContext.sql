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

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [FullName] nvarchar(max) NULL,
        [Gender] nvarchar(max) NULL,
        [BirthDate] datetime2 NULL,
        [AvatarUrl] nvarchar(max) NULL,
        [Country] nvarchar(max) NULL,
        [Address] nvarchar(max) NULL,
        [TaxNumber] nvarchar(max) NULL,
        [UpdateTime] datetime2 NULL,
        [CreateTime] datetime2 NULL,
        [LastLoginTime] datetime2 NULL,
        [SendClearEmail] bit NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [Companies] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Address] nvarchar(max) NOT NULL,
        [City] nvarchar(max) NOT NULL,
        [State] nvarchar(max) NOT NULL,
        [Country] nvarchar(max) NOT NULL,
        [PostalCode] nvarchar(max) NOT NULL,
        [Phone] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Website] nvarchar(max) NOT NULL,
        [FoundedDate] datetime2 NOT NULL,
        [LogoUrl] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Companies] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [EmailTemplates] (
        [Id] uniqueidentifier NOT NULL,
        [TemplateName] nvarchar(max) NOT NULL,
        [SenderName] nvarchar(max) NULL,
        [SenderEmail] nvarchar(max) NULL,
        [Category] nvarchar(max) NOT NULL,
        [SubjectLine] nvarchar(max) NOT NULL,
        [PreHeaderText] nvarchar(max) NULL,
        [PersonalizationTags] nvarchar(max) NULL,
        [BodyContent] nvarchar(max) NOT NULL,
        [FooterContent] nvarchar(max) NULL,
        [CallToAction] nvarchar(max) NULL,
        [Language] nvarchar(max) NULL,
        [RecipientType] nvarchar(max) NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [CreatedTime] datetime2 NULL,
        [UpdatedBy] nvarchar(max) NULL,
        [UpdatedTime] datetime2 NULL,
        [Status] int NOT NULL,
        CONSTRAINT [PK_EmailTemplates] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [Privacies] (
        [Id] uniqueidentifier NOT NULL,
        [Title] nvarchar(max) NOT NULL,
        [Content] nvarchar(max) NOT NULL,
        [LastUpdated] datetime2 NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Privacies] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [TermOfUses] (
        [Id] uniqueidentifier NOT NULL,
        [Title] nvarchar(max) NOT NULL,
        [Content] nvarchar(max) NOT NULL,
        [LastUpdated] datetime2 NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_TermOfUses] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [ActivityLogs] (
        [LogId] uniqueidentifier NOT NULL,
        [UserId] nvarchar(450) NOT NULL,
        [Action] nvarchar(max) NOT NULL,
        [Entity] nvarchar(max) NOT NULL,
        [TimeStamp] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_ActivityLogs] PRIMARY KEY ([LogId]),
        CONSTRAINT [FK_ActivityLogs_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [Balances] (
        [UserId] nvarchar(450) NOT NULL,
        [TotalBalance] float NOT NULL,
        [AvailableBalance] float NOT NULL,
        [PayoutBalance] float NOT NULL,
        [Currency] nvarchar(max) NOT NULL,
        [UpdatedTime] datetime2 NOT NULL,
        CONSTRAINT [PK_Balances] PRIMARY KEY ([UserId]),
        CONSTRAINT [FK_Balances_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [Customers] (
        [CustomerId] uniqueidentifier NOT NULL,
        [UserId] nvarchar(450) NOT NULL,
        [StripeAccountId] nvarchar(max) NULL,
        [IsAccepted] bit NULL,
        [AcceptedTime] datetime2 NULL,
        [AcceptedBy] nvarchar(max) NULL,
        [RejectedTime] datetime2 NULL,
        [RejectedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_Customers] PRIMARY KEY ([CustomerId]),
        CONSTRAINT [FK_Customers_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [Transactions] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] nvarchar(450) NOT NULL,
        [Type] int NOT NULL,
        [Amount] float NOT NULL,
        [Currency] nvarchar(max) NOT NULL,
        [CreatedTime] datetime2 NOT NULL,
        CONSTRAINT [PK_Transactions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Transactions_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [Cards] (
        [Id] uniqueidentifier NOT NULL,
        [CustomerId] uniqueidentifier NULL,
        [Code] nvarchar(max) NULL,
        [TotalCard] int NULL,
        [TotalRate] real NULL,
        [Version] int NULL,
        [TotalEarned] float NULL,
        [DeactivatedBy] nvarchar(max) NULL,
        [DeactivatedTime] datetime2 NULL,
        [ActivatedBy] nvarchar(max) NULL,
        [ActivatedTime] datetime2 NULL,
        [CreatedBy] nvarchar(max) NULL,
        [CreatedTime] datetime2 NULL,
        [MergedBy] nvarchar(max) NULL,
        [MergedTime] datetime2 NULL,
        [Status] int NOT NULL,
        CONSTRAINT [PK_Cards] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Cards_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([CustomerId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [CartHeaders] (
        [Id] uniqueidentifier NOT NULL,
        [CustomerId] uniqueidentifier NOT NULL,
        [TotalPrice] float NOT NULL,
        CONSTRAINT [PK_CartHeaders] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CartHeaders_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([CustomerId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [OrderHeaders] (
        [Id] uniqueidentifier NOT NULL,
        [CustomerId] uniqueidentifier NOT NULL,
        [OrderPrice] float NOT NULL,
        [PaymentIntentId] nvarchar(max) NULL,
        [StripeSessionId] nvarchar(max) NULL,
        [CreatedBy] nvarchar(max) NULL,
        [CreatedTime] datetime2 NULL,
        [UpdatedBy] nvarchar(max) NULL,
        [UpdatedTime] datetime2 NULL,
        [Status] int NOT NULL,
        CONSTRAINT [PK_OrderHeaders] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OrderHeaders_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([CustomerId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [CartDetails] (
        [Id] uniqueidentifier NOT NULL,
        [CartHeaderId] uniqueidentifier NOT NULL,
        [CardId] uniqueidentifier NOT NULL,
        [CardTitle] nvarchar(max) NULL,
        [CardPrice] float NOT NULL,
        CONSTRAINT [PK_CartDetails] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CartDetails_CartHeaders_CartHeaderId] FOREIGN KEY ([CartHeaderId]) REFERENCES [CartHeaders] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [OrderDetails] (
        [Id] uniqueidentifier NOT NULL,
        [CardId] uniqueidentifier NOT NULL,
        [CardTitle] nvarchar(max) NULL,
        [CardPrice] float NOT NULL,
        [OrderHeaderId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_OrderDetails] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OrderDetails_OrderHeaders_OrderHeaderId] FOREIGN KEY ([OrderHeaderId]) REFERENCES [OrderHeaders] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [OrdersStatus] (
        [Id] uniqueidentifier NOT NULL,
        [OrderHeaderId] uniqueidentifier NULL,
        [Status] int NOT NULL,
        [CreatedTime] datetime2 NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_OrdersStatus] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OrdersStatus_OrderHeaders_OrderHeaderId] FOREIGN KEY ([OrderHeaderId]) REFERENCES [OrderHeaders] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [CardManagements] (
        [CardId] uniqueidentifier NOT NULL,
        [UserId] nvarchar(450) NOT NULL,
        [InvitationId] uniqueidentifier NOT NULL,
        [AttendStatus] nvarchar(max) NOT NULL,
        [CreatedTime] datetime2 NOT NULL,
        CONSTRAINT [PK_CardManagements] PRIMARY KEY ([CardId]),
        CONSTRAINT [FK_CardManagements_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [Weddings] (
        [WeddingId] uniqueidentifier NOT NULL,
        [UserId] nvarchar(450) NOT NULL,
        [BrideName] nvarchar(max) NOT NULL,
        [GroomName] nvarchar(max) NOT NULL,
        [WeddingDate] datetime2 NOT NULL,
        [WeddingLocation] nvarchar(max) NOT NULL,
        [WeddingPhotoUrl] nvarchar(max) NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        [CardManagementCardId] uniqueidentifier NULL,
        CONSTRAINT [PK_Weddings] PRIMARY KEY ([WeddingId]),
        CONSTRAINT [FK_Weddings_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Weddings_CardManagements_CardManagementCardId] FOREIGN KEY ([CardManagementCardId]) REFERENCES [CardManagements] ([CardId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [Events] (
        [EventId] uniqueidentifier NOT NULL,
        [WeddingId] uniqueidentifier NOT NULL,
        [BrideName] nvarchar(max) NOT NULL,
        [GroomName] nvarchar(max) NOT NULL,
        [EventDate] datetime2 NOT NULL,
        [EventLocation] nvarchar(max) NOT NULL,
        [EventPhotoUrl] nvarchar(max) NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Events] PRIMARY KEY ([EventId]),
        CONSTRAINT [FK_Events_Weddings_WeddingId] FOREIGN KEY ([WeddingId]) REFERENCES [Weddings] ([WeddingId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [EventPhotos] (
        [EventPhotoId] uniqueidentifier NOT NULL,
        [EventId] uniqueidentifier NULL,
        [PhotoUrl] nvarchar(max) NOT NULL,
        [PhotoType] nvarchar(max) NOT NULL,
        [UploadedDate] datetime2 NOT NULL,
        CONSTRAINT [PK_EventPhotos] PRIMARY KEY ([EventPhotoId]),
        CONSTRAINT [FK_EventPhotos_Events_EventId] FOREIGN KEY ([EventId]) REFERENCES [Events] ([EventId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [GuestLists] (
        [GuestListId] uniqueidentifier NOT NULL,
        [GuestId] uniqueidentifier NOT NULL,
        [EventId] uniqueidentifier NOT NULL,
        [GuestName] nvarchar(max) NOT NULL,
        [AttendStatus] nvarchar(max) NOT NULL,
        [CheckinTime] datetime2 NOT NULL,
        [GuestGift] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_GuestLists] PRIMARY KEY ([GuestListId]),
        CONSTRAINT [FK_GuestLists_Events_EventId] FOREIGN KEY ([EventId]) REFERENCES [Events] ([EventId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [Guests] (
        [GuestId] uniqueidentifier NOT NULL,
        [GuestListId] uniqueidentifier NULL,
        [EventId] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Attend] nvarchar(max) NOT NULL,
        [Gift] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Guests] PRIMARY KEY ([GuestId]),
        CONSTRAINT [FK_Guests_Events_EventId] FOREIGN KEY ([EventId]) REFERENCES [Events] ([EventId]) ON DELETE CASCADE,
        CONSTRAINT [FK_Guests_GuestLists_GuestListId] FOREIGN KEY ([GuestListId]) REFERENCES [GuestLists] ([GuestListId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [InvitationHtmls] (
        [HtmlId] uniqueidentifier NOT NULL,
        [InvitationId] uniqueidentifier NULL,
        [HtmlContent] nvarchar(max) NOT NULL,
        [CreatedTime] datetime2 NOT NULL,
        [UpdateddTime] datetime2 NOT NULL,
        CONSTRAINT [PK_InvitationHtmls] PRIMARY KEY ([HtmlId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [Invitations] (
        [InvitationId] uniqueidentifier NOT NULL,
        [WeddingId] uniqueidentifier NULL,
        [TemplateId] uniqueidentifier NULL,
        [InvationLocation] datetime2 NOT NULL,
        [InvitationPhotoUrl] nvarchar(max) NOT NULL,
        [CustomerMessage] nvarchar(max) NOT NULL,
        [CustomerTextColor] nvarchar(max) NOT NULL,
        [ShareableLink] nvarchar(max) NOT NULL,
        [CreatedTime] datetime2 NOT NULL,
        CONSTRAINT [PK_Invitations] PRIMARY KEY ([InvitationId]),
        CONSTRAINT [FK_Invitations_Weddings_WeddingId] FOREIGN KEY ([WeddingId]) REFERENCES [Weddings] ([WeddingId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE TABLE [InvitationTemplates] (
        [TemplateId] uniqueidentifier NOT NULL,
        [TemplateName] nvarchar(max) NOT NULL,
        [BackgroundImageUrl] nvarchar(max) NOT NULL,
        [TextColor] nvarchar(max) NOT NULL,
        [TextFont] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [CreatedBy] datetime2 NOT NULL,
        [InvitationId] uniqueidentifier NULL,
        CONSTRAINT [PK_InvitationTemplates] PRIMARY KEY ([TemplateId]),
        CONSTRAINT [FK_InvitationTemplates_Invitations_InvitationId] FOREIGN KEY ([InvitationId]) REFERENCES [Invitations] ([InvitationId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] ON;
    EXEC(N'INSERT INTO [AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName])
    VALUES (N''8fa7c7bb-b4dc-480d-a660-e07a90855d5d'', N''Customer'', N''Customer'', N''Customer''),
    (N''8fa7c7bb-daa5-a660-bf02-82301a5eb327'', N''ADMIN'', N''ADMIN'', N''ADMIN'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
        SET IDENTITY_INSERT [AspNetRoles] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'Address', N'AvatarUrl', N'BirthDate', N'ConcurrencyStamp', N'Country', N'CreateTime', N'Email', N'EmailConfirmed', N'FullName', N'Gender', N'LastLoginTime', N'LockoutEnabled', N'LockoutEnd', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'SecurityStamp', N'SendClearEmail', N'TaxNumber', N'TwoFactorEnabled', N'UpdateTime', N'UserName') AND [object_id] = OBJECT_ID(N'[AspNetUsers]'))
        SET IDENTITY_INSERT [AspNetUsers] ON;
    EXEC(N'INSERT INTO [AspNetUsers] ([Id], [AccessFailedCount], [Address], [AvatarUrl], [BirthDate], [ConcurrencyStamp], [Country], [CreateTime], [Email], [EmailConfirmed], [FullName], [Gender], [LastLoginTime], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp], [SendClearEmail], [TaxNumber], [TwoFactorEnabled], [UpdateTime], [UserName])
    VALUES (N''TranThaiSon493'', 0, N''123 Admin St'', N''https://example.com/avatar.png'', ''1990-01-01T00:00:00.0000000'', N''aab60f6c-6cd2-4540-81c1-f7f3aee341e7'', N''Country'', NULL, N''admin@gmail.com'', CAST(1 AS bit), N''Admin User'', N''Male'', NULL, CAST(1 AS bit), NULL, N''ADMIN@GMAIL.COM'', N''ADMIN@GMAIL.COM'', N''AQAAAAIAAYagAAAAELitBjCGR6ZJvD548yisrGa1X/Xp2fcBcWd3V7fGA4jFbGk1H4p6GcBy2qw3Ea0N1Q=='', N''1234567890'', CAST(1 AS bit), N''c10472ca-2d66-4e50-8f6f-2823f1d6b186'', CAST(0 AS bit), N''123456789'', CAST(0 AS bit), ''2003-01-12T00:00:00.0000000'', N''admin@gmail.com'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'Address', N'AvatarUrl', N'BirthDate', N'ConcurrencyStamp', N'Country', N'CreateTime', N'Email', N'EmailConfirmed', N'FullName', N'Gender', N'LastLoginTime', N'LockoutEnabled', N'LockoutEnd', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'SecurityStamp', N'SendClearEmail', N'TaxNumber', N'TwoFactorEnabled', N'UpdateTime', N'UserName') AND [object_id] = OBJECT_ID(N'[AspNetUsers]'))
        SET IDENTITY_INSERT [AspNetUsers] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Address', N'City', N'Country', N'Description', N'Email', N'FoundedDate', N'LogoUrl', N'Name', N'Phone', N'PostalCode', N'State', N'Website') AND [object_id] = OBJECT_ID(N'[Companies]'))
        SET IDENTITY_INSERT [Companies] ON;
    EXEC(N'INSERT INTO [Companies] ([Id], [Address], [City], [Country], [Description], [Email], [FoundedDate], [LogoUrl], [Name], [Phone], [PostalCode], [State], [Website])
    VALUES (''81a76a7b-bff7-45c6-8495-887f744dfa30'', N''123 Main St'', N''Hometown'', N''Country'', N''ABC Corp is a leading company in XYZ industry.'', N''contact@abccorp.com'', ''2000-01-01T00:00:00.0000000'', N''http://www.abccorp.com/logo.png'', N''ABC Corp'', N''123-456-7890'', N''12345'', N''State'', N''http://www.abccorp.com'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Address', N'City', N'Country', N'Description', N'Email', N'FoundedDate', N'LogoUrl', N'Name', N'Phone', N'PostalCode', N'State', N'Website') AND [object_id] = OBJECT_ID(N'[Companies]'))
        SET IDENTITY_INSERT [Companies] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'BodyContent', N'CallToAction', N'Category', N'CreatedBy', N'CreatedTime', N'FooterContent', N'Language', N'PersonalizationTags', N'PreHeaderText', N'RecipientType', N'SenderEmail', N'SenderName', N'Status', N'SubjectLine', N'TemplateName', N'UpdatedBy', N'UpdatedTime') AND [object_id] = OBJECT_ID(N'[EmailTemplates]'))
        SET IDENTITY_INSERT [EmailTemplates] ON;
    EXEC(N'INSERT INTO [EmailTemplates] ([Id], [BodyContent], [CallToAction], [Category], [CreatedBy], [CreatedTime], [FooterContent], [Language], [PersonalizationTags], [PreHeaderText], [RecipientType], [SenderEmail], [SenderName], [Status], [SubjectLine], [TemplateName], [UpdatedBy], [UpdatedTime])
    VALUES (''21b64e11-aa33-4232-a7a6-6bf8bd7fee30'', N''<p>Hello {FirstName},</p><p>Click <a href="{ResetLink}">here</a> to reset your password.</p>'', N''<a href="{{ResetLink}}">Reset Password</a>'', N''Security'', NULL, NULL, N''<p>Contact us at cursusservicetts@gmail.com</p>'', N''English'', N''{FirstName}, {ResetLink}'', N''Reset your password to regain access.'', N''Customer'', N''cursusservicetts@gmail.com'', N''Wedding Team'', 1, N''Reset Your Password'', N''ChangePassword'', NULL, NULL),
    (''2366124b-a3e2-493a-8411-140fbb528d50'', N''<p>Thank you for registering your Wedding account. Click here to go back the page</p>'', N''<a href="{{Login}}">Login now</a>'', N''Verify'', NULL, NULL, N''<p>Contact us at cursusservicetts@gmail.com</p>'', N''English'', N''{FirstName}, {LinkLogin}'', N''User Account Verified!'', N''Customer'', N''cursusservicetts@gmail.com'', N''Wedding Team'', 1, N''Wedding Verify Email'', N''SendVerifyEmail'', NULL, NULL),
    (''3295d97c-35ec-4c6d-b531-e22a6675f4c1'', N''Dear [UserFullName],<br><br>Your account will be deleted after 14 days.'', N''<a href="https://weddinginvations.web.app/user/sign-in">Login</a>'', N''Remind Account'', NULL, NULL, N''<p>Contact us at cursusservicetts@gmail.com</p>'', N''English'', N''{FirstName}, {LastName}'', N''Hello!'', N''Customer'', N''cursusservicetts@gmail.com'', N''Wedding Team'', 1, N''Remind Delete Account!'', N''RemindDeleteAccount'', NULL, NULL),
    (''5ea756ce-7554-4e89-9f90-60c036671426'', N''Hi [UserFullName],<br><br>We received a request to reset your password. Click the link below to reset your password.'', N''https://weddinginvations.web.app/sign-in/verify-email?userId=user.Id&token=Uri.EscapeDataString(token)'', N''Security'', NULL, NULL, N''If you did not request a password reset, please ignore this email.'', N''English'', N''[UserFullName], [ResetPasswordLink]'', N''Reset your password to regain access'', N''Customer'', N''cursusservicetts@gmail.com'', N''Wedding Team'', 1, N''Reset Your Password'', N''ForgotPasswordEmail'', NULL, NULL),
    (''9e614fcb-7d9a-469e-a437-655022d596f4'', N''Dear [UserFullName],<br><br>You have completed our course program, you can take new courses to increase your knowledge and skills.'', N''<a href="https://weddinginvations.web.app/user/sign-in">Login</a>'', N''Course completed'', NULL, NULL, N''<p>Contact us at cursusservicetts@gmail.com</p>'', N''English'', N''{FirstName}, {LastName}'', N''Hello!'', N''Customer'', N''cursusservicetts@gmail.com'', N''Wedding Team'', 1, N''Congratulations on completing the course!'', N''CustomerCompleteCourse'', NULL, NULL),
    (''b7f68b99-036d-4e3b-b5ce-7825dc7e20b1'', CONCAT(CAST(N''Dear {FirstName} {LastName},<br><br>'' AS nvarchar(max)), nchar(13), nchar(10), nchar(13), nchar(10), N''                    This email confirms that your payout request has been processed successfully.'', nchar(13), nchar(10), N''                    <br>'', nchar(13), nchar(10), N''                    <strong>Payout Details:</strong>'', nchar(13), nchar(10), N''                    <ul>'', nchar(13), nchar(10), N''                    <li>Amount: {PayoutAmount}</li>'', nchar(13), nchar(10), N''                    <li>Transaction Date: {TransactionDate}</li> '', nchar(13), nchar(10), N''                    </ul>'', nchar(13), nchar(10), N''                    <br>'', nchar(13), nchar(10), N''                    You can view your payout history in your customer dashboard. '', nchar(13), nchar(10), N''                    <br> '', nchar(13), nchar(10), N''                    Thank you for being a valued Wedding customer!'', nchar(13), nchar(10), N''                    <br>''), N''<a href="https://weddinginvations.web.app/user/sign-in">Login</a>'', N''Payout'', NULL, NULL, N''<p>Contact us at cursusservicetts@gmail.com</p>'', N''English'', N''{FirstName}, {LastName}, {PayoutAmount}, {TransactionDate}'', N''Payout Successful!'', N''Customer'', N''cursusservicetts@gmail.com'', N''Wedding Team'', 1, N''Your Wedding Payout is Complete!'', N''NotifyCustomerPaymentReceived'', NULL, NULL),
    (''d01a70db-099a-41b6-a33e-b923d70aa8d9'', N''Dear [UserFullName],<br><br>Your account has been deleted.'', N''<a href="https://weddinginvations.web.app/user/sign-in">Login</a>'', N''Delete Account'', NULL, NULL, N''<p>Contact us at cursusservicetts@gmail.com</p>'', N''English'', N''{FirstName}, {LastName}'', N''Hello!'', N''Customer'', N''cursusservicetts@gmail.com'', N''Wedding Team'', 1, N''Delete Account!'', N''DeleteAccount'', NULL, NULL),
    (''d79a1288-3188-468c-b595-a9a206f6181f'', N''Dear [UserFullName],<br><br>Welcome to Wedding! We are excited to have you join our learning community.'', N''<a href="https://weddinginvations.web.app/user/sign-in">Login</a>'', N''Welcome'', NULL, NULL, N''<p>Contact us at cursusservicetts@gmail.com</p>'', N''English'', N''{FirstName}, {LastName}'', N''Thank you for signing up!'', N''Customer'', N''cursusservicetts@gmail.com'', N''Wedding Team'', 1, N''Welcome to Wedding!'', N''WelcomeEmail'', NULL, NULL)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'BodyContent', N'CallToAction', N'Category', N'CreatedBy', N'CreatedTime', N'FooterContent', N'Language', N'PersonalizationTags', N'PreHeaderText', N'RecipientType', N'SenderEmail', N'SenderName', N'Status', N'SubjectLine', N'TemplateName', N'UpdatedBy', N'UpdatedTime') AND [object_id] = OBJECT_ID(N'[EmailTemplates]'))
        SET IDENTITY_INSERT [EmailTemplates] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Content', N'IsActive', N'LastUpdated', N'Title') AND [object_id] = OBJECT_ID(N'[Privacies]'))
        SET IDENTITY_INSERT [Privacies] ON;
    EXEC(N'INSERT INTO [Privacies] ([Id], [Content], [IsActive], [LastUpdated], [Title])
    VALUES (''0c99a1e4-2102-478a-871a-044586f9750e'', N''These are the privacy for our service.'', CAST(1 AS bit), ''2024-10-30T11:10:31.9255270Z'', N''Privacy'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Content', N'IsActive', N'LastUpdated', N'Title') AND [object_id] = OBJECT_ID(N'[Privacies]'))
        SET IDENTITY_INSERT [Privacies] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Content', N'IsActive', N'LastUpdated', N'Title') AND [object_id] = OBJECT_ID(N'[TermOfUses]'))
        SET IDENTITY_INSERT [TermOfUses] ON;
    EXEC(N'INSERT INTO [TermOfUses] ([Id], [Content], [IsActive], [LastUpdated], [Title])
    VALUES (''4a5bc13b-2182-4002-82e5-30e62794aec6'', N''These are the terms of use for our service.'', CAST(1 AS bit), ''2024-10-30T11:10:31.9255303Z'', N''Terms of Use'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Content', N'IsActive', N'LastUpdated', N'Title') AND [object_id] = OBJECT_ID(N'[TermOfUses]'))
        SET IDENTITY_INSERT [TermOfUses] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[AspNetUserRoles]'))
        SET IDENTITY_INSERT [AspNetUserRoles] ON;
    EXEC(N'INSERT INTO [AspNetUserRoles] ([RoleId], [UserId])
    VALUES (N''8fa7c7bb-daa5-a660-bf02-82301a5eb327'', N''TranThaiSon493'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[AspNetUserRoles]'))
        SET IDENTITY_INSERT [AspNetUserRoles] OFF;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE INDEX [IX_ActivityLogs_UserId] ON [ActivityLogs] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE INDEX [IX_CardManagements_InvitationId] ON [CardManagements] ([InvitationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE INDEX [IX_CardManagements_UserId] ON [CardManagements] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE INDEX [IX_Cards_CustomerId] ON [Cards] ([CustomerId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE INDEX [IX_CartDetails_CartHeaderId] ON [CartDetails] ([CartHeaderId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE INDEX [IX_CartHeaders_CustomerId] ON [CartHeaders] ([CustomerId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE INDEX [IX_Customers_UserId] ON [Customers] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE INDEX [IX_EventPhotos_EventId] ON [EventPhotos] ([EventId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE INDEX [IX_Events_WeddingId] ON [Events] ([WeddingId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE INDEX [IX_GuestLists_EventId] ON [GuestLists] ([EventId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE INDEX [IX_GuestLists_GuestId] ON [GuestLists] ([GuestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE INDEX [IX_Guests_EventId] ON [Guests] ([EventId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE INDEX [IX_Guests_GuestListId] ON [Guests] ([GuestListId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE INDEX [IX_InvitationHtmls_InvitationId] ON [InvitationHtmls] ([InvitationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Invitations_TemplateId] ON [Invitations] ([TemplateId]) WHERE [TemplateId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE INDEX [IX_Invitations_WeddingId] ON [Invitations] ([WeddingId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE INDEX [IX_InvitationTemplates_InvitationId] ON [InvitationTemplates] ([InvitationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE INDEX [IX_OrderDetails_OrderHeaderId] ON [OrderDetails] ([OrderHeaderId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE INDEX [IX_OrderHeaders_CustomerId] ON [OrderHeaders] ([CustomerId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE INDEX [IX_OrdersStatus_OrderHeaderId] ON [OrdersStatus] ([OrderHeaderId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE INDEX [IX_Transactions_UserId] ON [Transactions] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE INDEX [IX_Weddings_CardManagementCardId] ON [Weddings] ([CardManagementCardId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    CREATE INDEX [IX_Weddings_UserId] ON [Weddings] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    ALTER TABLE [CardManagements] ADD CONSTRAINT [FK_CardManagements_Invitations_InvitationId] FOREIGN KEY ([InvitationId]) REFERENCES [Invitations] ([InvitationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    ALTER TABLE [GuestLists] ADD CONSTRAINT [FK_GuestLists_Guests_GuestId] FOREIGN KEY ([GuestId]) REFERENCES [Guests] ([GuestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    ALTER TABLE [InvitationHtmls] ADD CONSTRAINT [FK_InvitationHtmls_Invitations_InvitationId] FOREIGN KEY ([InvitationId]) REFERENCES [Invitations] ([InvitationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    ALTER TABLE [Invitations] ADD CONSTRAINT [FK_Invitations_InvitationTemplates_TemplateId] FOREIGN KEY ([TemplateId]) REFERENCES [InvitationTemplates] ([TemplateId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241030111032_Initial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241030111032_Initial', N'8.0.10');
END;
GO

COMMIT;
GO

