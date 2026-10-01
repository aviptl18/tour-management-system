USE [TourDB]
GO
/****** Object:  UserDefinedTableType [dbo].[TourOptionTvp]    Script Date: 11-03-2026 14:53:37 ******/
CREATE TYPE [dbo].[TourOptionTvp] AS TABLE(
	[OptionId] [int] NULL,
	[OptionName] [nvarchar](200) NOT NULL,
	[PriceApplied] [decimal](12, 2) NOT NULL,
	[Quantity] [int] NOT NULL
)
GO
/****** Object:  Table [dbo].[LocationImages]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocationImages](
	[ImageId] [int] IDENTITY(1,1) NOT NULL,
	[LocationId] [int] NOT NULL,
	[ImageUrl] [nvarchar](500) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ImageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Locations]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Locations](
	[LocationId] [int] IDENTITY(1,1) NOT NULL,
	[LocationName] [nvarchar](200) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LocationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OptionDefinition]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OptionDefinition](
	[OptionId] [int] IDENTITY(1,1) NOT NULL,
	[OptionName] [nvarchar](150) NOT NULL,
	[BasePrice] [decimal](10, 2) NOT NULL,
	[Currency] [nvarchar](10) NOT NULL,
	[Category] [nvarchar](100) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OptionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TourAllowedOption]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TourAllowedOption](
	[TourId] [int] NOT NULL,
	[OptionId] [int] NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_TourAllowedOption] PRIMARY KEY CLUSTERED 
(
	[TourId] ASC,
	[OptionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TourDraft]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TourDraft](
	[DraftId] [int] IDENTITY(1,1) NOT NULL,
	[TourTitle] [nvarchar](200) NOT NULL,
	[Overview] [nvarchar](max) NOT NULL,
	[LocationId] [int] NULL,
	[BasePrice] [decimal](18, 2) NULL,
	[DraftStep] [int] NOT NULL,
	[Status] [nvarchar](20) NOT NULL,
	[OriginalTourId] [int] NULL,
	[CreatedByAdminId] [int] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[Duration] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[DraftId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TourDraftImage]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TourDraftImage](
	[DraftImageId] [int] IDENTITY(1,1) NOT NULL,
	[DraftId] [int] NOT NULL,
	[LocationImageId] [int] NOT NULL,
	[IsCover] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[DraftImageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TourDraftPricingComponent]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TourDraftPricingComponent](
	[PricingComponentId] [int] IDENTITY(1,1) NOT NULL,
	[DraftId] [int] NOT NULL,
	[OptionId] [int] NULL,
	[ComponentName] [nvarchar](150) NOT NULL,
	[Price] [decimal](10, 2) NOT NULL,
	[IsBaseComponent] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PricingComponentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TourImages]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TourImages](
	[TourImageId] [int] IDENTITY(1,1) NOT NULL,
	[TourId] [int] NOT NULL,
	[ImageId] [int] NOT NULL,
	[IsCoverImage] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TourImageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TourPricingComponent]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TourPricingComponent](
	[PricingComponentId] [int] IDENTITY(1,1) NOT NULL,
	[TourId] [int] NOT NULL,
	[OptionId] [int] NULL,
	[ComponentName] [nvarchar](150) NOT NULL,
	[Price] [decimal](10, 2) NOT NULL,
	[IsBaseComponent] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NULL,
	[DisplayOrder] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PricingComponentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tours]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tours](
	[TourId] [int] IDENTITY(1,1) NOT NULL,
	[LocationId] [int] NOT NULL,
	[TourTitle] [nvarchar](100) NOT NULL,
	[Duration] [nvarchar](100) NULL,
	[Overview] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[BasePrice] [decimal](12, 2) NULL,
	[TotalPrice] [decimal](12, 2) NULL,
	[CoverImageId] [int] NULL,
	[IncludedTotal] [decimal](10, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[TourId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TourSelectedOption]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TourSelectedOption](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TourId] [int] NOT NULL,
	[OptionId] [int] NULL,
	[OptionName] [nvarchar](200) NULL,
	[PriceApplied] [decimal](12, 2) NULL,
	[Quantity] [int] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](150) NOT NULL,
	[Email] [nvarchar](150) NOT NULL,
	[PasswordHash] [nvarchar](500) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_Locations_LocationName]    Script Date: 11-03-2026 14:53:37 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_Locations_LocationName] ON [dbo].[Locations]
(
	[LocationName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_OptionDefinition_OptionName]    Script Date: 11-03-2026 14:53:37 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_OptionDefinition_OptionName] ON [dbo].[OptionDefinition]
(
	[OptionName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TourAllowedOption_OptionId]    Script Date: 11-03-2026 14:53:37 ******/
CREATE NONCLUSTERED INDEX [IX_TourAllowedOption_OptionId] ON [dbo].[TourAllowedOption]
(
	[OptionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TourAllowedOption_TourId]    Script Date: 11-03-2026 14:53:37 ******/
CREATE NONCLUSTERED INDEX [IX_TourAllowedOption_TourId] ON [dbo].[TourAllowedOption]
(
	[TourId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TourPricingComponent_Tour_DisplayOrder]    Script Date: 11-03-2026 14:53:37 ******/
CREATE NONCLUSTERED INDEX [IX_TourPricingComponent_Tour_DisplayOrder] ON [dbo].[TourPricingComponent]
(
	[TourId] ASC,
	[DisplayOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Locations] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Locations] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[OptionDefinition] ADD  DEFAULT ('INR') FOR [Currency]
GO
ALTER TABLE [dbo].[OptionDefinition] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[OptionDefinition] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[TourAllowedOption] ADD  DEFAULT ((0)) FOR [IsDefault]
GO
ALTER TABLE [dbo].[TourAllowedOption] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[TourDraft] ADD  DEFAULT ((0)) FOR [DraftStep]
GO
ALTER TABLE [dbo].[TourDraft] ADD  DEFAULT ('Draft') FOR [Status]
GO
ALTER TABLE [dbo].[TourDraft] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[TourDraftImage] ADD  DEFAULT ((0)) FOR [IsCover]
GO
ALTER TABLE [dbo].[TourDraftImage] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[TourDraftPricingComponent] ADD  DEFAULT ((0)) FOR [IsBaseComponent]
GO
ALTER TABLE [dbo].[TourDraftPricingComponent] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[TourImages] ADD  DEFAULT ((0)) FOR [IsCoverImage]
GO
ALTER TABLE [dbo].[TourPricingComponent] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[TourPricingComponent] ADD  DEFAULT ((1)) FOR [DisplayOrder]
GO
ALTER TABLE [dbo].[Tours] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[TourSelectedOption] ADD  DEFAULT ((1)) FOR [Quantity]
GO
ALTER TABLE [dbo].[TourSelectedOption] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[LocationImages]  WITH CHECK ADD  CONSTRAINT [FK_LocationImages_Locations] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Locations] ([LocationId])
GO
ALTER TABLE [dbo].[LocationImages] CHECK CONSTRAINT [FK_LocationImages_Locations]
GO
ALTER TABLE [dbo].[TourAllowedOption]  WITH CHECK ADD  CONSTRAINT [FK_TourAllowedOption_Option] FOREIGN KEY([OptionId])
REFERENCES [dbo].[OptionDefinition] ([OptionId])
GO
ALTER TABLE [dbo].[TourAllowedOption] CHECK CONSTRAINT [FK_TourAllowedOption_Option]
GO
ALTER TABLE [dbo].[TourAllowedOption]  WITH CHECK ADD  CONSTRAINT [FK_TourAllowedOption_Tour] FOREIGN KEY([TourId])
REFERENCES [dbo].[Tours] ([TourId])
GO
ALTER TABLE [dbo].[TourAllowedOption] CHECK CONSTRAINT [FK_TourAllowedOption_Tour]
GO
ALTER TABLE [dbo].[TourDraft]  WITH CHECK ADD  CONSTRAINT [FK_TourDraft_User] FOREIGN KEY([CreatedByAdminId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[TourDraft] CHECK CONSTRAINT [FK_TourDraft_User]
GO
ALTER TABLE [dbo].[TourDraftImage]  WITH CHECK ADD  CONSTRAINT [FK_TourDraftImage_Draft] FOREIGN KEY([DraftId])
REFERENCES [dbo].[TourDraft] ([DraftId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TourDraftImage] CHECK CONSTRAINT [FK_TourDraftImage_Draft]
GO
ALTER TABLE [dbo].[TourDraftImage]  WITH CHECK ADD  CONSTRAINT [FK_TourDraftImage_LocationImage] FOREIGN KEY([LocationImageId])
REFERENCES [dbo].[LocationImages] ([ImageId])
GO
ALTER TABLE [dbo].[TourDraftImage] CHECK CONSTRAINT [FK_TourDraftImage_LocationImage]
GO
ALTER TABLE [dbo].[TourDraftPricingComponent]  WITH CHECK ADD  CONSTRAINT [FK_DraftPricing_Draft] FOREIGN KEY([DraftId])
REFERENCES [dbo].[TourDraft] ([DraftId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TourDraftPricingComponent] CHECK CONSTRAINT [FK_DraftPricing_Draft]
GO
ALTER TABLE [dbo].[TourDraftPricingComponent]  WITH CHECK ADD  CONSTRAINT [FK_DraftPricing_Option] FOREIGN KEY([OptionId])
REFERENCES [dbo].[OptionDefinition] ([OptionId])
GO
ALTER TABLE [dbo].[TourDraftPricingComponent] CHECK CONSTRAINT [FK_DraftPricing_Option]
GO
ALTER TABLE [dbo].[TourImages]  WITH CHECK ADD FOREIGN KEY([ImageId])
REFERENCES [dbo].[LocationImages] ([ImageId])
GO
ALTER TABLE [dbo].[TourImages]  WITH CHECK ADD FOREIGN KEY([TourId])
REFERENCES [dbo].[Tours] ([TourId])
GO
ALTER TABLE [dbo].[TourPricingComponent]  WITH CHECK ADD  CONSTRAINT [FK_TourPricingComponent_Option] FOREIGN KEY([OptionId])
REFERENCES [dbo].[OptionDefinition] ([OptionId])
GO
ALTER TABLE [dbo].[TourPricingComponent] CHECK CONSTRAINT [FK_TourPricingComponent_Option]
GO
ALTER TABLE [dbo].[TourPricingComponent]  WITH CHECK ADD  CONSTRAINT [FK_TourPricingComponent_Tours] FOREIGN KEY([TourId])
REFERENCES [dbo].[Tours] ([TourId])
GO
ALTER TABLE [dbo].[TourPricingComponent] CHECK CONSTRAINT [FK_TourPricingComponent_Tours]
GO
ALTER TABLE [dbo].[Tours]  WITH CHECK ADD FOREIGN KEY([LocationId])
REFERENCES [dbo].[Locations] ([LocationId])
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Roles]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Users]
GO
ALTER TABLE [dbo].[TourDraft]  WITH CHECK ADD  CONSTRAINT [CK_TourDraft_Status] CHECK  (([Status]='Updated' OR [Status]='Cancelled' OR [Status]='Published' OR [Status]='Complete' OR [Status]='Draft'))
GO
ALTER TABLE [dbo].[TourDraft] CHECK CONSTRAINT [CK_TourDraft_Status]
GO
ALTER TABLE [dbo].[TourPricingComponent]  WITH CHECK ADD  CONSTRAINT [CK_DisplayOrder_Positive] CHECK  (([DisplayOrder]>(0)))
GO
ALTER TABLE [dbo].[TourPricingComponent] CHECK CONSTRAINT [CK_DisplayOrder_Positive]
GO
/****** Object:  StoredProcedure [dbo].[GetActiveOptions]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetActiveOptions]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        OptionId,
        OptionName,
        BasePrice,
        Currency,
        Category,
        IsActive   -- ✅ ADD THIS
    FROM dbo.OptionDefinition
    WHERE IsActive = 1
    ORDER BY Category, OptionName;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Location_GetImagesByLocationId]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Location_GetImagesByLocationId]
	@LocationId INT
AS
BEGIN
	SELECT ImageId, LocationId,ImageUrl
	FROM LocationImages
	WHERE LocationId = @LocationId;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_AddAllowedOptionToTour]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_AddAllowedOptionToTour]
(
	@TourId INT,
	@OptionId INT,
	@IsDefault BIT
)
AS
BEGIN
	SET NOCOUNT ON;

	-- validate option

	IF NOT EXISTS(
		SELECT 1 FROM dbo.OptionDefinition
		WHERE OptionId = @OptionId
			AND IsActive = 1
	)
	BEGIN
		RAISERROR('Option does not exist or is inactive.', 16, 1);
		RETURN;
	END

	-- if already exists

	IF EXISTS(
		SELECT 1 FROM dbo.TourAllowedOption
		WHERE TourId = @TourId
			AND OptionId = @OptionId
	)
	BEGIN
		-- if existing is default, never downgrade

		IF EXISTS(
			SELECT 1 FROM dbo.TourAllowedOption
			WHERE TourId = @TourId
				AND OptionId = @OptionId
				AND IsDefault = 1
		)
		BEGIN
			RETURN;
		END

		-- if existing is optional and default requested - upgrade

		UPDATE dbo.TourAllowedOption
		SET IsDefault = 1
		WHERE TourId = @TourId
			AND OptionId = @OptionId;

		RETURN;
	END

	-- insert new

	INSERT INTO dbo.TourAllowedOption
	(
		TourId,
		OptionId,
		IsDefault,
		CreatedDate
	)
	VALUES
	(
		@TourId,
		@OptionId,
		@IsDefault,
		GETDATE()
	);
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_AddDraftImage]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_AddDraftImage]
	@DraftId INT,
	@LocationImageId INT,
	@IsCover BIT
AS
BEGIN
	INSERT INTO dbo.TourDraftImage
	(
		DraftId,
		LocationImageId,
		IsCover
	)
	VALUES
	(
		@DraftId,
		@LocationImageId,
		@IsCover
	);
END
GO
/****** Object:  StoredProcedure [dbo].[usp_AddDraftPricingComponent]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_AddDraftPricingComponent]
	@DraftId INT,
	@OptionId INT = NULL,
	@ComponentName NVARCHAR(150),
	@Price DECIMAL(10,2),
	@IsBaseComponent BIT

AS
BEGIN
	INSERT INTO dbo.TourDraftPricingComponent
	(		
		DraftId,
        OptionId,
        ComponentName,
        Price,
        IsBaseComponent
	)
	VALUES
	(
		@DraftId,
        @OptionId,
        @ComponentName,
        @Price,
        @IsBaseComponent
	);
END
GO
/****** Object:  StoredProcedure [dbo].[usp_AddLocation]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_AddLocation]
	@LocationName NVARCHAR(200)

AS 
BEGIN
	IF EXISTS ( SELECT 1 FROM Locations WHERE LocationName = @LocationName)
	BEGIN
		RAISERROR('Location already exists', 16, 1);
		RETURN;
	END

	INSERT INTO Locations (LocationName, IsActive, CreatedDate)
	VALUES (@LocationName, 1, GETDATE());
END
GO
/****** Object:  StoredProcedure [dbo].[usp_AddOption]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_AddOption]
(
	@OptionName NVARCHAR(100),
	@BasePrice DECIMAL(10,2),
	@Currency NVARCHAR(10),
	@Category NVARCHAR(100)
)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.OptionDefinition
	(
		OptionName,
		BasePrice,
		Currency,
		Category
	)
	VALUES
	(
		@OptionName,
		@BasePrice,
		@Currency,
		@Category
	);
END
GO
/****** Object:  StoredProcedure [dbo].[usp_AddTours]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_AddTours]
    @LocationId INT,
    @TourTitle NVARCHAR(100),
    @Duration NVARCHAR(100),
    @BasePrice DECIMAL(12,2),
    @Overview NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Tours
    (
        LocationId,
        TourTitle,
        Duration,
        BasePrice,
        Overview,
        CreatedDate
    )
    VALUES
    (
        @LocationId,
        @TourTitle,
        @Duration,
        @BasePrice,
        @Overview,
        SYSUTCDATETIME()
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS NewTourId;
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_AddTourWithPricing]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_AddTourWithPricing]
(
    @TourTitle NVARCHAR(200),
    @Overview NVARCHAR(MAX),
    @LocationId INT,
    @Duration NVARCHAR(100),
    @BasePrice DECIMAL(12,2),
    @TotalPrice DECIMAL(12,2),
    @Options dbo.TourOptionTvp READONLY
)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO dbo.Tours
        (
            TourTitle,
            Overview,
            LocationId,
            Duration,
            BasePrice,
            TotalPrice,
            CreatedDate
        )
        VALUES
        (
            @TourTitle,
            @Overview,
            @LocationId,
            @Duration,
            @BasePrice,
            @TotalPrice,
            SYSUTCDATETIME()
        );

        DECLARE @NewTourId INT = SCOPE_IDENTITY();

        INSERT INTO dbo.TourSelectedOption
        (
            TourId,
            OptionId,
            OptionName,
            PriceApplied,
            Quantity,
            CreatedAt
        )
        SELECT
            @NewTourId,
            OptionId,
            OptionName,
            PriceApplied,
            Quantity,
            SYSUTCDATETIME()
        FROM @Options;

        COMMIT TRANSACTION;

        -- ✅ THIS IS THE FIX
        SELECT @NewTourId AS NewTourId;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[usp_CreateTourDraft]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_CreateTourDraft]
    @CreatedByAdminId INT,
    @DraftId INT OUTPUT
AS
BEGIN
    INSERT INTO dbo.TourDraft
    (
        TourTitle,
        Overview,
        DraftStep,
        CreatedByAdminId,
        Status
    )
    VALUES
    (
        '',
        '',
        0,
        @CreatedByAdminId,
        'Draft'
    );

    SET @DraftId = SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[usp_CreateTourDraftFromLive]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_CreateTourDraftFromLive]
(
    @TourId INT,
    @AdminId INT,
    @DraftId INT OUTPUT
)
AS
BEGIN

SET NOCOUNT ON;

BEGIN TRY

BEGIN TRANSACTION;

-- VALIDATE TOUR EXISTS

IF NOT EXISTS
(
    SELECT 1 
    FROM Tours
    WHERE TourId = @TourId
)
BEGIN
    RAISERROR('Tour not found.',16,1);
END


-- CREATE DRAFT ROW

INSERT INTO TourDraft
(
    TourTitle,
    Overview,
    LocationId,
    Duration,
    BasePrice,
    DraftStep,
    Status,
    OriginalTourId,
    CreatedByAdminId,
    CreatedAt
)
SELECT
    TourTitle,
    Overview,
    LocationId,
    Duration,
    BasePrice,
    3,
    'Draft',
    TourId,
    @AdminId,
    GETDATE()
FROM Tours
WHERE TourId = @TourId;


SET @DraftId = SCOPE_IDENTITY();


-- SAFETY CHECK

IF @DraftId IS NULL
BEGIN
    RAISERROR('Failed to create draft.',16,1);
END


-- CLONE IMAGES

INSERT INTO TourDraftImage
(
    DraftId,
    LocationImageId,
    IsCover,
    CreatedAt
)
SELECT
    @DraftId,
    ImageId,
    IsCoverImage,
    GETDATE()
FROM TourImages
WHERE TourId = @TourId;


-- CLONE PRICING COMPONENTS

INSERT INTO TourDraftPricingComponent
(
    DraftId,
    OptionId,
    ComponentName,
    Price,
    IsBaseComponent,
    CreatedAt
)
SELECT 
    @DraftId,
    OptionId,
    ComponentName,
    Price,
    IsBaseComponent,
    GETDATE()
FROM TourPricingComponent
WHERE TourId = @TourId;


COMMIT TRANSACTION;

END TRY

BEGIN CATCH

ROLLBACK TRANSACTION;
THROW;

END CATCH

END
GO
/****** Object:  StoredProcedure [dbo].[usp_DeactivateLocation]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_DeactivateLocation]
	@LocationId INT

AS
BEGIN
	IF EXISTS (SELECT 1 FROM Tours WHERE LocationId = @LocationId)
	BEGIN
		RAISERROR('Cannot deactivate location used by tours', 16, 1);
		RETURN;
	END

	UPDATE Locations
	SET IsActive = 0
	WHERE LocationId = @LocationId;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_DeleteDraftImages]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_DeleteDraftImages]
	@DraftId INT
AS
BEGIN
	DELETE FROM dbo.TourDraftImage
	WHERE DraftId = @DraftId;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_DeleteDraftPricingComponents]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_DeleteDraftPricingComponents]
	@DraftId INT
AS
BEGIN
	DELETE FROM dbo.TourDraftPricingComponent
	WHERE DraftId = @DraftId;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_DeleteTour]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_DeleteTour]
	@TourId INT
AS 
BEGIN
	DELETE FROM Tours
	WHERE TourId = @TourId;
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_EnsureBaseComponent]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_EnsureBaseComponent]
	@DraftId INT

AS
BEGIN
	IF NOT EXISTS(
		SELECT 1 FROM dbo.TourDraftPricingComponent
		WHERE DraftId = @DraftId
		AND IsBaseComponent = 1
	)
	BEGIN
		INSERT INTO dbo.TourDraftPricingComponent
		(
			DraftId,
			ComponentName,
			Price,
			IsBaseComponent
		)
		VALUES
		(
			@DraftId,
			'Base Price',
			0,
			1
		);
	END
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetActiveDraftForTour]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetActiveDraftForTour]
(
	@TourId INT
)
AS
BEGIN

SET NOCOUNT ON;

SELECT TOP 1 DraftId
FROM TourDraft
WHERE OriginalTourId = @TourId
AND Status IN ('Draft','Complete')
ORDER BY CreatedAt DESC

END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetAllLocations]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetAllLocations]
	@IncludeInactive BIT = 0

AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		LocationId,
		LocationName,
		IsActive,
		CreatedDate
	FROM Locations
	WHERE (@IncludeInactive = 1 OR IsActive = 1)
	ORDER BY LocationName ASC;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetAllOptions]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetAllOptions]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		OptionId,
		OptionName,
		BasePrice,
		Currency,
		Category,
		IsActive,
		CreatedDate
	FROM dbo.OptionDefinition
	ORDER BY Category, OptionName;
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_GetAllowedOptionsForTour]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetAllowedOptionsForTour]
(
	@TourId INT
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		o.OptionId,
		o.OptionName,
		o.BasePrice,
		o.Currency,
		o.Category,
		tao.IsDefault,
		tao.CreatedDate
	FROM dbo.TourAllowedOption AS tao
	INNER JOIN dbo.OptionDefinition AS o
		ON o.OptionId = tao.OptionId
	WHERE 
		tao.TourId = @TourId
		AND o.IsActive = 1
	ORDER BY
		tao.IsDefault DESC,
		o.Category,
		o.OptionName;
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_GetAllTours]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetAllTours]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        T.TourId,
        T.TourTitle,
        T.Duration,
        T.Overview,

        -- Location
        T.LocationId,
        L.LocationName,

        -- Pricing (new, correct fields)
        T.BasePrice,
        T.TotalPrice,

        -- Cover image via resolved pointer
        LI.ImageUrl AS CoverImageUrl

    FROM Tours T

    INNER JOIN Locations L
        ON T.LocationId = L.LocationId

    LEFT JOIN TourImages TI
        ON T.CoverImageId = TI.TourImageId

    LEFT JOIN LocationImages LI
        ON TI.ImageId = LI.ImageId

    ORDER BY T.TourId DESC;
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_GetAvailableOptionalOptionsForTour]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetAvailableOptionalOptionsForTour]
(
	@TourId INT
)
AS
BEGIN
	SET NOCOUNT ON;


	SELECT
		o.OptionId,
		o.OptionName,
		o.BasePrice,
		o.Currency,
		o.Category
	FROM dbo.OptionDefinition o
	WHERE
		o.IsActive = 1
		AND NOT EXISTS(
			SELECT 1 FROM dbo.TourAllowedOption tao
			WHERE tao.TourId = @TourId
				AND tao.OptionId = o.OptionId
		)
	ORDER BY
		o.Category,
		o.OptionName
END;	
GO
/****** Object:  StoredProcedure [dbo].[usp_GetDraftImages]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetDraftImages]
	@DraftId INT

AS
BEGIN
	SELECT *
	FROM dbo.TourDraftImage
	WHERE DraftId = @DraftId;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetDraftPricingComponents]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetDraftPricingComponents]
	@DraftId INT

AS
BEGIN
	SELECT * FROM dbo.TourDraftPricingComponent
	WHERE DraftId = @DraftId
	ORDER BY IsBaseComponent DESC, PricingComponentId ASC;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetOptionById]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetOptionById]
	@OptionId INT

AS
BEGIN
	SELECT
		OptionId,
		OptionName,
		BasePrice,
		Currency,
		Category,
		IsActive
	FROM OptionDefinition
	WHERE OptionId = @OptionId
		AND IsActive = 1
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetTourById]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetTourById]
	@TourId INT
AS 
BEGIN
	SELECT
		T.TourId,
		T.LocationId,
		T.TourTitle,
		T.Duration,
		T.Price,
		T.Overview,
		T.ImageUrl,
		T.CreatedDate
	FROM Tours T 
	WHERE T.TourId = @TourId;
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_GetTourDraftById]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetTourDraftById]
    @DraftId INT,
    @CreatedByAdminId INT
AS
BEGIN
    SELECT *
    FROM dbo.TourDraft
    WHERE DraftId = @DraftId
      AND CreatedByAdminId = @CreatedByAdminId
      AND Status IN ('Draft','Complete');
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GetUserByEmail]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetUserByEmail]
(
	@Email NVARCHAR(150)
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		UserId,
		FullName,
		Email,
		PasswordHash, 
		IsActive
	FROM dbo.Users
	WHERE Email = @Email;
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_GetUserRoles]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetUserRoles]
(
	@UserId INT
)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		R.RoleName
	FROM dbo.UserRoles UR
	INNER JOIN dbo.Roles R
		ON UR.RoleId = R.RoleId
	WHERE UR.UserId = @UserId;
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_GetUserTourDetails]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetUserTourDetails]
    @TourId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- =====================================================
    -- 1️⃣ CORE TOUR DETAILS (USER-SAFE WITH FULL PRICING)
    -- =====================================================
    SELECT
        t.TourId,
        t.TourTitle,
        l.LocationName,
        t.Overview,
        t.Duration,
        t.BasePrice,
        t.IncludedTotal,
        t.TotalPrice
    FROM Tours t
    INNER JOIN Locations l
        ON t.LocationId = l.LocationId
    WHERE t.TourId = @TourId;


    -- =====================================================
    -- 2️⃣ TOUR IMAGES (ORDERED: COVER FIRST)
    -- =====================================================
    SELECT
        li.ImageUrl,
        ti.IsCoverImage
    FROM TourImages ti
    INNER JOIN LocationImages li
        ON ti.ImageId = li.ImageId
    WHERE ti.TourId = @TourId
    ORDER BY
        ti.IsCoverImage DESC,
        ti.TourImageId ASC;


    -- =====================================================
    -- 3️⃣ ALLOWED ADD-ONS (NAMES ONLY)
    -- =====================================================
    SELECT
        od.OptionName
    FROM TourAllowedOption tao
    INNER JOIN OptionDefinition od
        ON tao.OptionId = od.OptionId
    WHERE tao.TourId = @TourId;

END
GO
/****** Object:  StoredProcedure [dbo].[usp_LoginUser]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_LoginUser]
(
	@Email NVARCHAR(150)
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		UserId,
		FullName,
		Email,
		PasswordHash
	FROM dbo.Users
	WHERE Email = @Email
	AND IsActive = 1;
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_PublishTour]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_PublishTour]
	@DraftId INT,
	@AdminId INT,
	@NewTourId INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		BEGIN TRANSACTION;

		-- Validate draft is ready

		IF NOT EXISTS(
			SELECT 1 FROM TourDraft
			WHERE DraftId = @DraftId
				AND Status = 'Complete'
				AND DraftStep = 3
		)
		BEGIN 
			RAISERROR('Draft is not ready for publish.', 16, 1);
		END

		-- Ensure CoverImage exists

		IF NOT EXISTS (
			SELECT 1 FROM TourDraftImage
			WHERE DraftId = @DraftId
				AND IsCover = 1
		)
		BEGIN 
			RAISERROR('Cover image is Required.', 16, 1);
		END

		-- Create Live Tour

		INSERT INTO Tours
		(
			TourTitle,
			Overview,
			LocationId,
			Duration,
			CreatedDate
		)
		SELECT 
			TourTitle,
			Overview,
			LocationId,
			Duration,
			GETDATE()
		FROM TourDraft
		WHERE DraftId = @DraftId

		SET @NewTourId = SCOPE_IDENTITY();

		-- Copy Images to Live Tour

		INSERT INTO TourImages(TourId, ImageId, IsCoverImage)
		SELECT 
			@NewTourId,
			LocationImageId,
			IsCover
		FROM TourDraftImage
		WHERE DraftId = @DraftId;

		-- Save CoverImageId in Tours table
		
		UPDATE T
		SET CoverImageId = TI.TourImageId
		FROM Tours T
		JOIN TourImages TI ON TI.TourId = T.TourId
		WHERE T.TourId = @NewTourId
		AND TI.IsCoverImage = 1;

		-- Copy Pricing component 

		INSERT INTO TourPricingComponent
		(
			TourId,
			OptionId,
			ComponentName,
			Price,
			IsBaseComponent,
			DisplayOrder
		)
		SELECT
			@NewTourId,
			OptionId,
			ComponentName,
			Price,
			IsBaseComponent,
			ROW_NUMBER() OVER ( ORDER BY PricingComponentId)
		FROM TourDraftPricingComponent
		WHERE DraftId = @DraftId;

		-- Calculate Pricing Total

		DECLARE @BasePrice DECIMAL(10,2);
		DECLARE @IncludedTotal DECIMAL(10,2);

		-- base price

		SELECT @BasePrice = Price
		FROM TourPricingComponent
		WHERE TourId = @NewTourId
			AND IsBaseComponent = 1;

		-- included component total

		SELECT @IncludedTotal = ISNULL(SUM(Price), 0)
		FROM TourPricingComponent
		WHERE TourId = @NewTourId
			AND IsBaseComponent = 0
			AND OptionId IS NOT NULL;

		-- Update Tour Price Fields

		UPDATE Tours
		SET BasePrice = @BasePrice,
			IncludedTotal = @IncludedTotal,
			TotalPrice = @BasePrice + @IncludedTotal
		WHERE TourId = @NewTourId;

		-- Store included options

		INSERT INTO TourAllowedOption(TourId, OptionId, IsDefault)
		SELECT 
			@NewTourId,
			OptionId,
			1
		FROM TourPricingComponent
		WHERE TourId = @NewTourId
			AND OptionId IS NOT NULL
			AND IsBaseComponent = 0;

		-- Update Draft Status

		UPDATE TourDraft
		SET Status = 'Published',
			OriginalTourId = @NewTourId
		WHERE DraftId = @DraftId;

		-- Commit

		COMMIT TRANSACTION;

	END TRY

	BEGIN CATCH
		ROLLBACK TRANSACTION;
		THROW;
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[usp_RegisterUser]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_RegisterUser]
(
	@FullName NVARCHAR(150),
	@Email NVARCHAR(150),
	@PasswordHash NVARCHAR(500),
	@RoleName NVARCHAR(50) = 'User'
)
AS
BEGIN
	SET NOCOUNT ON;

	-- Prevent duplicate email

	IF EXISTS (SELECT 1 FROM dbo.Users WHERE Email = @Email)
	BEGIN
		RAISERROR('Email Already Exist.', 16, 1);
		RETURN;
	END	

	-- Insert User

	INSERT INTO dbo.Users(FullName, Email, PasswordHash)
	VALUES (@FullName, @Email, @PasswordHash);

	DECLARE @UserId INT = SCOPE_IDENTITY();

	-- Assign Role

	INSERT INTO dbo.UserRoles(UserId,RoleId)
	SELECT @UserId, RoleId
	FROM dbo.Roles
	WHERE RoleName = @RoleName;
END;
GO
/****** Object:  StoredProcedure [dbo].[usp_RemoveAllowedOptionFromTour]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_RemoveAllowedOptionFromTour]
(
	@TourId INT,
	@OptionId INT
)
AS
BEGIN
	SET NOCOUNT ON;

	-- check if option is default

	IF EXISTS(
		SELECT 1 FROM dbo.TourAllowedOption
		WHERE TourId = @TourId
			AND OptionId = @OptionId
			AND IsDefault = 1
	)
	BEGIN 
		RAISERROR('Included options cannot be removed from configuration.', 16, 1);
		RETURN;
	END

	-- remove only optional addons

	DELETE FROM dbo.TourAllowedOption
	WHERE TourId = @TourId
		AND OptionId = @OptionId
		AND IsDefault = 0;
END;



GO
/****** Object:  StoredProcedure [dbo].[usp_RemoveDraftPricingComponent]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_RemoveDraftPricingComponent]
    @PricingComponentId INT
AS
BEGIN
    DELETE FROM dbo.TourDraftPricingComponent
    WHERE PricingComponentId = @PricingComponentId
END
GO
/****** Object:  StoredProcedure [dbo].[usp_SetDraftStep]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_SetDraftStep]
    @DraftId INT,
    @Step INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE TourDraft
    SET DraftStep = @Step,
        UpdatedAt = GETUTCDATE()
    WHERE DraftId = @DraftId;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_SetOptionActiveStatus]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_SetOptionActiveStatus]
    @OptionId INT,
    @IsActive BIT
AS
BEGIN
    UPDATE OptionDefinition
    SET IsActive = @IsActive
    WHERE OptionId = @OptionId;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateDraftLocation]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_UpdateDraftLocation]
	@DraftId INT,
	@AdminId INT,
	@LocationId INT
AS
BEGIN
	UPDATE dbo.TourDraft
	SET LocationId = @LocationId,
		DraftStep = CASE WHEN DraftStep < 2 THEN 2 ELSE DraftStep END,
		UpdatedAt = SYSUTCDATETIME()
	WHERE DraftId = @DraftId
		AND CreatedByAdminId = @AdminId
		AND Status = 'Draft';
END
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateDraftPricingComponentPrice]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_UpdateDraftPricingComponentPrice]
	@ComponentId INT,
	@Price DECIMAL(10,2)
AS
BEGIN
	UPDATE TourDraftPricingComponent
	SET Price = @Price
	WHERE PricingComponentId = @ComponentId
END
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateDraftStatus]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_UpdateDraftStatus]
	@DraftId INT,
	@Status NVARCHAR(20)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE TourDraft
	SET Status = @Status,
		UpdatedAt = GETUTCDATE()
	WHERE DraftId = @DraftId;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateLocation]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_UpdateLocation]
	@LocationId INT,
	@LocationName NVARCHAR(200),
	@IsActive BIT 

AS
BEGIN
	IF EXISTS(
		SELECT 1
		FROM Locations
		WHERE LocationName = @LocationName
		AND LocationId <> LocationId
	)
	BEGIN
		RAISERROR('Location name already in use', 16, 1);
		RETURN;
	END

	UPDATE Locations
		SET 
			LocationName = @LocationName,
			IsActive = @IsActive
		WHERE LocationId = @LocationId;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateOption]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_UpdateOption]
(
	@OptionId INT,
	@OptionName NVARCHAR(150),
	@BasePrice DECIMAL(10,2),
	@Currency NVARCHAR(10),
	@Category NVARCHAR(100)
)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.OptionDefinition
	SET
		OptionName = @OptionName,
		BasePrice = @BasePrice,
		Currency = @Currency,
		Category = @Category
	WHERE OptionId = @OptionId;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateTour]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_UpdateTour]
	@TourId INT,
	@LocationId INT,
	@TourTitle NVARCHAR(100),
	@Duration NVARCHAR(100),
	@Price DECIMAL(10,2),
	@Overview NVARCHAR(Max),
	@ImageUrl NVARCHAR(500)

AS 
BEGIN
	UPDATE Tours
	SET
		LocationId = @LocationId,
		TourTitle = @TourTitle,
		Duration = @Duration,
		Price = @Price,
		Overview = @Overview,
		ImageUrl = @ImageUrl
	WHERE TourId = @TourId;
END;																																																						
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateTourDraftStep1]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_UpdateTourDraftStep1]
    @DraftId INT,
    @CreatedByAdminId INT,
    @TourTitle NVARCHAR(200),
    @Overview NVARCHAR(MAX)
AS
BEGIN
    UPDATE dbo.TourDraft
    SET
        TourTitle = @TourTitle,
        Overview = @Overview,
        DraftStep = 1,
        UpdatedAt = SYSUTCDATETIME()
    WHERE DraftId = @DraftId
      AND CreatedByAdminId = @CreatedByAdminId
      AND Status = 'Draft';
END
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateTourDraftStep3]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_UpdateTourDraftStep3]
	@DraftId INT,
	@Duration NVARCHAR(100)
AS
BEGIN
	UPDATE TourDraft
	SET Duration = @Duration,
		DraftStep = CASE WHEN DraftStep < 3 THEN 3 ELSE DraftStep END,
		UpdatedAt = GETUTCDATE()
	WHERE DraftId = @DraftId
END
GO
/****** Object:  StoredProcedure [dbo].[usp_UpdateTourFromDraft]    Script Date: 11-03-2026 14:53:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_UpdateTourFromDraft]
(
 @DraftId INT,
 @AdminId INT
)
AS
BEGIN

SET NOCOUNT ON;

-- STEP 1: Validate draft BEFORE transaction

IF NOT EXISTS
(
    SELECT 1
    FROM TourDraft
    WHERE DraftId = @DraftId
    AND Status = 'Complete'
    AND DraftStep = 3
)
BEGIN
    THROW 50001, 'Draft not ready.', 1;
END


DECLARE @TourId INT;

SELECT @TourId = OriginalTourId
FROM TourDraft
WHERE DraftId = @DraftId;


BEGIN TRY

    BEGIN TRANSACTION;

    -- Update Tour Core Data

    UPDATE T
    SET
        T.TourTitle = D.TourTitle,
        T.Overview = D.Overview,
        T.LocationId = D.LocationId,
        T.Duration = D.Duration
    FROM Tours T
    JOIN TourDraft D
        ON D.DraftId = @DraftId
    WHERE T.TourId = @TourId;


    -- Replace Images

    DELETE FROM TourImages
    WHERE TourId = @TourId;

    INSERT INTO TourImages
    (
        TourId,
        ImageId,
        IsCoverImage
    )
    SELECT
        @TourId,
        LocationImageId,
        IsCover
    FROM TourDraftImage
    WHERE DraftId = @DraftId;


    -- Update CoverImageId

    UPDATE T
    SET CoverImageId = TI.TourImageId
    FROM Tours T
    JOIN TourImages TI
        ON TI.TourId = T.TourId
    WHERE T.TourId = @TourId
    AND TI.IsCoverImage = 1;


    -- Replace Pricing Components

    DELETE FROM TourPricingComponent
    WHERE TourId = @TourId;

    INSERT INTO TourPricingComponent
    (
        TourId,
        OptionId,
        ComponentName,
        Price,
        IsBaseComponent,
        DisplayOrder
    )
    SELECT
        @TourId,
        OptionId,
        ComponentName,
        Price,
        IsBaseComponent,
        ROW_NUMBER() OVER (ORDER BY PricingComponentId)
    FROM TourDraftPricingComponent
    WHERE DraftId = @DraftId;


    -- Recalculate price

    DECLARE @BasePrice DECIMAL(10,2);
    DECLARE @IncludedTotal DECIMAL(10,2);

    SELECT @BasePrice = Price
    FROM TourPricingComponent
    WHERE TourId = @TourId
    AND IsBaseComponent = 1;

    SELECT @IncludedTotal = ISNULL(SUM(Price),0)
    FROM TourPricingComponent
    WHERE TourId = @TourId
    AND OptionId IS NOT NULL
    AND IsBaseComponent = 0;

    UPDATE Tours
    SET
        BasePrice = @BasePrice,
        IncludedTotal = @IncludedTotal,
        TotalPrice = @BasePrice + @IncludedTotal
    WHERE TourId = @TourId;


    -- Remove all included options

    DELETE FROM TourAllowedOption
    WHERE TourId = @TourId
    AND IsDefault = 1;


    -- Convert optional options to included if they exist in Step 3

    UPDATE tao
    SET IsDefault = 1
    FROM TourAllowedOption tao
    JOIN TourPricingComponent pc
        ON pc.OptionId = tao.OptionId
    WHERE tao.TourId = @TourId
    AND pc.TourId = @TourId
    AND pc.OptionId IS NOT NULL
    AND pc.IsBaseComponent = 0
    AND tao.IsDefault = 0;


    -- Insert remaining included options

    INSERT INTO TourAllowedOption
    (
        TourId,
        OptionId,
        IsDefault
    )
    SELECT
        @TourId,
        pc.OptionId,
        1
    FROM TourPricingComponent pc
    WHERE pc.TourId = @TourId
    AND pc.OptionId IS NOT NULL
    AND pc.IsBaseComponent = 0
    AND NOT EXISTS
    (
        SELECT 1
        FROM TourAllowedOption tao
        WHERE tao.TourId = @TourId
        AND tao.OptionId = pc.OptionId
    );


    -- Mark draft updated

    UPDATE TourDraft
    SET Status = 'Updated'
    WHERE DraftId = @DraftId;


    COMMIT TRANSACTION;

END TRY
BEGIN CATCH

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;

END CATCH

END
GO
