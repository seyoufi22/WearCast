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
CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [IsDefault] bit NOT NULL,
    [IsDeleted] bit NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [FirstName] nvarchar(100) NOT NULL,
    [LastName] nvarchar(100) NOT NULL,
    [State] nvarchar(50) NOT NULL,
    [City] nvarchar(50) NOT NULL,
    [Street] nvarchar(200) NOT NULL,
    [BuildingNumber] nvarchar(20) NOT NULL,
    [EmailConfirmationCode] nvarchar(max) NULL,
    [EmailConfirmationCodeExpiration] datetime2 NULL,
    [ResetPasswordCode] nvarchar(max) NULL,
    [ResetPasswordCodeExpiration] datetime2 NULL,
    [IsDisabled] bit NOT NULL DEFAULT CAST(0 AS bit),
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] varchar(20) NOT NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);

CREATE TABLE [SellerApplications] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
    [Email] nvarchar(256) NOT NULL,
    [PhoneNumber] nvarchar(11) NOT NULL,
    [EmailConfirmationCode] nvarchar(max) NULL,
    [EmailConfirmationCodeExpiration] datetime2 NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [SellerName] nvarchar(100) NOT NULL,
    [CommercialRegisterNumber] nvarchar(20) NOT NULL,
    [TaxIdNumber] nvarchar(9) NOT NULL,
    [Description] nvarchar(500) NOT NULL,
    [LogoUrl] nvarchar(500) NOT NULL,
    [State] nvarchar(50) NOT NULL,
    [City] nvarchar(50) NOT NULL,
    [Street] nvarchar(200) NOT NULL,
    [BuildingNumber] nvarchar(20) NOT NULL,
    [CreatedOn] datetime2 NOT NULL,
    [Status] int NOT NULL,
    [RejectionReason] nvarchar(500) NULL,
    CONSTRAINT [PK_SellerApplications] PRIMARY KEY ([Id])
);

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Categories] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    [ImageUrl] nvarchar(255) NOT NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedById] nvarchar(450) NULL,
    [UpdatedOn] datetime2 NULL,
    [IsDeleted] bit NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Categories_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Categories_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id])
);

CREATE TABLE [Customers] (
    [Id] int NOT NULL IDENTITY,
    [ProfileImageUrl] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Customers_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Drivers] (
    [Id] int NOT NULL IDENTITY,
    [Status] tinyint NOT NULL,
    [ProfileImageUrl] nvarchar(500) NOT NULL DEFAULT N'',
    [NationalId] char(14) NOT NULL,
    [VehicleType] tinyint NOT NULL,
    [VehiclePlateNumber] nvarchar(20) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Drivers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Drivers_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Factories] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Factories] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Factories_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [RefreshTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [Id] int NOT NULL IDENTITY,
    [Token] nvarchar(max) NOT NULL,
    [ExpiresOn] datetime2 NOT NULL,
    [CreatedOn] datetime2 NOT NULL,
    [RevokedOn] datetime2 NULL,
    CONSTRAINT [PK_RefreshTokens] PRIMARY KEY ([UserId], [Id]),
    CONSTRAINT [FK_RefreshTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Sellers] (
    [Id] int NOT NULL IDENTITY,
    [SellerName] nvarchar(max) NOT NULL,
    [CommercialRegisterNumber] nvarchar(max) NOT NULL,
    [TaxIdNumber] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [LogoUrl] nvarchar(max) NOT NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Sellers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Sellers_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ShippingCompanies] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_ShippingCompanies] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ShippingCompanies_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [FixedProducts] (
    [Id] int NOT NULL IDENTITY,
    [CategoryId] int NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [Description] nvarchar(1000) NOT NULL,
    [TargetAudience] nvarchar(20) NOT NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedById] nvarchar(450) NULL,
    [UpdatedOn] datetime2 NULL,
    [IsDeleted] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [SizeDetails] nvarchar(max) NULL,
    CONSTRAINT [PK_FixedProducts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_FixedProducts_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_FixedProducts_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_FixedProducts_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [FixedProductColors] (
    [Id] int NOT NULL IDENTITY,
    [ProductId] int NOT NULL,
    [ColorName] nvarchar(50) NOT NULL,
    [ColorCode] nvarchar(20) NOT NULL,
    [ImageUrl] nvarchar(max) NOT NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedById] nvarchar(450) NULL,
    [UpdatedOn] datetime2 NULL,
    [IsDeleted] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [Sizes] nvarchar(max) NULL,
    CONSTRAINT [PK_FixedProductColors] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_FixedProductColors_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_FixedProductColors_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_FixedProductColors_FixedProducts_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [FixedProducts] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [FixedProductImages] (
    [Id] int NOT NULL IDENTITY,
    [ProductColorId] int NOT NULL,
    [ImageUrl] nvarchar(max) NOT NULL,
    [CreatedById] nvarchar(450) NOT NULL,
    [CreatedOn] datetime2 NOT NULL,
    [UpdatedById] nvarchar(450) NULL,
    [UpdatedOn] datetime2 NULL,
    [IsDeleted] bit NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_FixedProductImages] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_FixedProductImages_AspNetUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_FixedProductImages_AspNetUsers_UpdatedById] FOREIGN KEY ([UpdatedById]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_FixedProductImages_FixedProductColors_ProductColorId] FOREIGN KEY ([ProductColorId]) REFERENCES [FixedProductColors] ([Id]) ON DELETE NO ACTION
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'IsDefault', N'IsDeleted', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
    SET IDENTITY_INSERT [AspNetRoles] ON;
INSERT INTO [AspNetRoles] ([Id], [ConcurrencyStamp], [IsDefault], [IsDeleted], [Name], [NormalizedName])
VALUES (N'03205b5f-c1bd-411b-a31d-dcaff6ca0005', N'7eff9a9d-07d2-4bda-926d-7b1ecd47d3b8', CAST(0 AS bit), CAST(0 AS bit), N'Factory', N'FACTORY'),
(N'6635cd46-2b74-4e0c-8872-1fed3faa60cd', N'd40b1bc4-6525-47f6-a80a-4695ccc08f1e', CAST(0 AS bit), CAST(0 AS bit), N'Seller', N'SELLER'),
(N'91756452-0c83-4c86-8129-88698116ee37', N'fe83a0d2-cc41-4305-bcea-799fe7af0de2', CAST(0 AS bit), CAST(0 AS bit), N'Customer', N'CUSTOMER'),
(N'9766e049-f467-48f3-8b66-c1715ac1fcec', N'58e49111-c6bb-4826-87be-2888c51416f3', CAST(0 AS bit), CAST(0 AS bit), N'ShippingCompany', N'SHIPPINGCOMPANY'),
(N'99fef310-8a33-4639-aa90-09c8b372bf03', N'6d4214a5-fc02-401f-a42c-70bfa1fbd731', CAST(0 AS bit), CAST(0 AS bit), N'Driver', N'DRIVER'),
(N'efc6a646-310b-4571-9b0e-bdfebe29f921', N'ed345b9e-2f7a-4b76-8d25-476cd4cb10db', CAST(0 AS bit), CAST(0 AS bit), N'SuperAdmin', N'SUPERADMIN');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConcurrencyStamp', N'IsDefault', N'IsDeleted', N'Name', N'NormalizedName') AND [object_id] = OBJECT_ID(N'[AspNetRoles]'))
    SET IDENTITY_INSERT [AspNetRoles] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'ConcurrencyStamp', N'Email', N'EmailConfirmationCode', N'EmailConfirmationCodeExpiration', N'EmailConfirmed', N'FirstName', N'LastName', N'LockoutEnabled', N'LockoutEnd', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'ResetPasswordCode', N'ResetPasswordCodeExpiration', N'SecurityStamp', N'TwoFactorEnabled', N'UserName', N'BuildingNumber', N'City', N'State', N'Street') AND [object_id] = OBJECT_ID(N'[AspNetUsers]'))
    SET IDENTITY_INSERT [AspNetUsers] ON;
INSERT INTO [AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmationCode], [EmailConfirmationCodeExpiration], [EmailConfirmed], [FirstName], [LastName], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [ResetPasswordCode], [ResetPasswordCodeExpiration], [SecurityStamp], [TwoFactorEnabled], [UserName], [BuildingNumber], [City], [State], [Street])
VALUES (N'019bcd04-0443-7450-b883-59b89ff9671d', 0, N'F8E241D5-2EDA-47F8-99A6-A808F5819AC1', N'Customer@WearCast.com', NULL, NULL, CAST(1 AS bit), N'WearCast', N'Customer', CAST(0 AS bit), NULL, N'CUSTOMER@WEARCAST.COM', N'CUSTOMER@WEARCAST.COM', N'AQAAAAIAAYagAAAAENtP0eLKD5M6MtJQhAYAkh8I/D48s2S55lIDj5p6tg55DJPRkJ5XtwF0SV8JQEbIiw==', '01000000002', CAST(1 AS bit), NULL, NULL, N'B3CC00839EFE45119752D661DE818BE8', CAST(0 AS bit), N'Customer@WearCast.com', N'15', N'Smouha', N'Alexandria', N'Victor Emmanuel'),
(N'5c821809-f53e-44e2-b02e-2e62cd16bea4', 0, N'd732650c-c26b-4314-abaf-fe20261baff3', N'Driver@WearCast.com', NULL, NULL, CAST(1 AS bit), N'WearCast', N'Driver', CAST(0 AS bit), NULL, N'DRIVER@WEARCAST.COM', N'DRIVER@WEARCAST.COM', N'AQAAAAIAAYagAAAAEOfnRtOAu0967IvwEju3Ipt/fsN4Ijh/WV3u6VwVLOAvnG8H0VWtHeRlpXjV+ab7Ew==', '01000000006', CAST(1 AS bit), NULL, NULL, N'1CED46537D60492E9165593104B35D2E', CAST(0 AS bit), N'Driver@WearCast.com', N'30', N'Heliopolis', N'Cairo', N'El Hegaz'),
(N'b40b3bad-4c09-41e9-96f5-2edf3fbf663d', 0, N'877425a6-9985-41e5-b951-769647829b08', N'ShippingCompany@WearCast.com', NULL, NULL, CAST(1 AS bit), N'WearCast', N'ShippingCompany', CAST(0 AS bit), NULL, N'SHIPPINGCOMPANY@WEARCAST.COM', N'SHIPPINGCOMPANY@WEARCAST.COM', N'AQAAAAIAAYagAAAAEKHTSGVpzJslVzx24dE/yPJcfm1xJh/yqYRK0PIy+lzcaIKvMrxCWeKpXS0WFAhxcw==', '01000000005', CAST(1 AS bit), NULL, NULL, N'1CED46537D60492E9165593104B35D2E', CAST(0 AS bit), N'ShippingCompany@WearCast.com', N'5', N'Maadi', N'Cairo', N'Road 9'),
(N'f0b32453-dd0d-479a-a91b-72bbf6f9eaff', 0, N'4e57ffa9-4c71-4b97-998a-27a8d2a3bb04', N'Seller@WearCast.com', NULL, NULL, CAST(1 AS bit), N'WearCast', N'Seller', CAST(0 AS bit), NULL, N'SELLER@WEARCAST.COM', N'SELLER@WEARCAST.COM', N'AQAAAAIAAYagAAAAELmYP6pkEqOiZgdvd0Yn2JDvFDgxCXsbgiudGwDrJv5I2z/1IHwW/LIoaf1ifvbvSQ==', '01000000003', CAST(1 AS bit), NULL, NULL, N'1548E5815A804885901BF82B2B0616D3', CAST(0 AS bit), N'Seller@WearCast.com', N'20', N'Dokki', N'Giza', N'Tahrir St'),
(N'f3a0803c-b8b8-4bad-9a16-ea5a4902ff01', 0, N'78f0f4bb-725f-4803-b34e-c5b8774cc0e4', N'Factory@WearCast.com', NULL, NULL, CAST(1 AS bit), N'WearCast', N'Factory', CAST(0 AS bit), NULL, N'FACTORY@WEARCAST.COM', N'FACTORY@WEARCAST.COM', N'AQAAAAIAAYagAAAAEGKazKpWma09E61sO8iJHmYGmYhSecRzxwy/FTbnNXWyBkzZIqijb9fU7nj2lYSGRg==', '01000000004', CAST(1 AS bit), NULL, NULL, N'EC614A6CDF134E3288ED241B1AB33608', CAST(0 AS bit), N'Factory@WearCast.com', N'50', N'10th of Ramadan', N'Sharqia', N'Industrial Zone'),
(N'fe83a0d2-cc41-4305-bcea-799fe7af0de2', 0, N'5e7bb6ec-e063-4e6f-a929-1fbe81b3c4d0', N'SuperAdmin@WearCast.com', NULL, NULL, CAST(1 AS bit), N'WearCast', N'SuperAdmin', CAST(0 AS bit), NULL, N'SUPERADMIN@WEARCAST.COM', N'SUPERADMIN@WEARCAST.COM', N'AQAAAAIAAYagAAAAEJy9nrrle8KVO2FW2/9w58i8W0N0n6qom+sC3vbkI/aI2rUI3P27ChXEuDS8A/RNGw==', '01000000001', CAST(1 AS bit), NULL, NULL, N'DAE8F8342FB84409A3CF6B6BE8802BC8', CAST(0 AS bit), N'SuperAdmin@WearCast.com', N'10', N'Nasr City', N'Cairo', N'Makram Ebeid');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AccessFailedCount', N'ConcurrencyStamp', N'Email', N'EmailConfirmationCode', N'EmailConfirmationCodeExpiration', N'EmailConfirmed', N'FirstName', N'LastName', N'LockoutEnabled', N'LockoutEnd', N'NormalizedEmail', N'NormalizedUserName', N'PasswordHash', N'PhoneNumber', N'PhoneNumberConfirmed', N'ResetPasswordCode', N'ResetPasswordCodeExpiration', N'SecurityStamp', N'TwoFactorEnabled', N'UserName', N'BuildingNumber', N'City', N'State', N'Street') AND [object_id] = OBJECT_ID(N'[AspNetUsers]'))
    SET IDENTITY_INSERT [AspNetUsers] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClaimType', N'ClaimValue', N'RoleId') AND [object_id] = OBJECT_ID(N'[AspNetRoleClaims]'))
    SET IDENTITY_INSERT [AspNetRoleClaims] ON;
INSERT INTO [AspNetRoleClaims] ([Id], [ClaimType], [ClaimValue], [RoleId])
VALUES (1, N'permissions', N'categorys:read', N'efc6a646-310b-4571-9b0e-bdfebe29f921'),
(2, N'permissions', N'categorys:add', N'efc6a646-310b-4571-9b0e-bdfebe29f921'),
(3, N'permissions', N'categorys:update', N'efc6a646-310b-4571-9b0e-bdfebe29f921'),
(4, N'permissions', N'categorys:delete', N'efc6a646-310b-4571-9b0e-bdfebe29f921');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ClaimType', N'ClaimValue', N'RoleId') AND [object_id] = OBJECT_ID(N'[AspNetRoleClaims]'))
    SET IDENTITY_INSERT [AspNetRoleClaims] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[AspNetUserRoles]'))
    SET IDENTITY_INSERT [AspNetUserRoles] ON;
INSERT INTO [AspNetUserRoles] ([RoleId], [UserId])
VALUES (N'91756452-0c83-4c86-8129-88698116ee37', N'019bcd04-0443-7450-b883-59b89ff9671d'),
(N'99fef310-8a33-4639-aa90-09c8b372bf03', N'5c821809-f53e-44e2-b02e-2e62cd16bea4'),
(N'9766e049-f467-48f3-8b66-c1715ac1fcec', N'b40b3bad-4c09-41e9-96f5-2edf3fbf663d'),
(N'6635cd46-2b74-4e0c-8872-1fed3faa60cd', N'f0b32453-dd0d-479a-a91b-72bbf6f9eaff'),
(N'03205b5f-c1bd-411b-a31d-dcaff6ca0005', N'f3a0803c-b8b8-4bad-9a16-ea5a4902ff01'),
(N'efc6a646-310b-4571-9b0e-bdfebe29f921', N'fe83a0d2-cc41-4305-bcea-799fe7af0de2');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[AspNetUserRoles]'))
    SET IDENTITY_INSERT [AspNetUserRoles] OFF;

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);

CREATE UNIQUE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]) WHERE [NormalizedEmail] IS NOT NULL;

CREATE UNIQUE INDEX [IX_AspNetUsers_Email] ON [AspNetUsers] ([Email]) WHERE [Email] IS NOT NULL;

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

CREATE INDEX [IX_Categories_CreatedById] ON [Categories] ([CreatedById]);

CREATE UNIQUE INDEX [IX_Categories_Name] ON [Categories] ([Name]);

CREATE INDEX [IX_Categories_UpdatedById] ON [Categories] ([UpdatedById]);

CREATE UNIQUE INDEX [IX_Customers_UserId] ON [Customers] ([UserId]);

CREATE UNIQUE INDEX [IX_Drivers_NationalId] ON [Drivers] ([NationalId]);

CREATE UNIQUE INDEX [IX_Drivers_UserId] ON [Drivers] ([UserId]);

CREATE UNIQUE INDEX [IX_Factories_UserId] ON [Factories] ([UserId]);

CREATE INDEX [IX_FixedProductColors_CreatedById] ON [FixedProductColors] ([CreatedById]);

CREATE INDEX [IX_FixedProductColors_ProductId] ON [FixedProductColors] ([ProductId]);

CREATE INDEX [IX_FixedProductColors_UpdatedById] ON [FixedProductColors] ([UpdatedById]);

CREATE INDEX [IX_FixedProductImages_CreatedById] ON [FixedProductImages] ([CreatedById]);

CREATE INDEX [IX_FixedProductImages_ProductColorId] ON [FixedProductImages] ([ProductColorId]);

CREATE INDEX [IX_FixedProductImages_UpdatedById] ON [FixedProductImages] ([UpdatedById]);

CREATE INDEX [IX_FixedProducts_CategoryId] ON [FixedProducts] ([CategoryId]);

CREATE INDEX [IX_FixedProducts_CreatedById] ON [FixedProducts] ([CreatedById]);

CREATE INDEX [IX_FixedProducts_UpdatedById] ON [FixedProducts] ([UpdatedById]);

CREATE UNIQUE INDEX [IX_SellerApplications_Email] ON [SellerApplications] ([Email]) WHERE [Status] != 3;

CREATE UNIQUE INDEX [IX_SellerApplications_PhoneNumber] ON [SellerApplications] ([PhoneNumber]) WHERE [Status] != 3;

CREATE UNIQUE INDEX [IX_Sellers_UserId] ON [Sellers] ([UserId]);

CREATE UNIQUE INDEX [IX_ShippingCompanies_UserId] ON [ShippingCompanies] ([UserId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260306192253_init', N'10.0.3');

COMMIT;
GO

