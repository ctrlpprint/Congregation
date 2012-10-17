IF OBJECT_ID('[dbo].[Addresses]', 'U') IS NOT NULL  DROP TABLE [dbo].[Addresses]
GO

CREATE TABLE [dbo].[Addresses](
	[ID] [int] NOT NULL,
	[FamilyID] [int] NOT NULL,
	[StreetNumber] [nvarchar](25) NULL,
	[Street] [nvarchar](100) NULL,
	[Suburb] [nvarchar](100) NULL,
	[City] [nvarchar](100) NULL,
	[PostCode] [nvarchar](10) NULL,
	[Country] [nvarchar](100) NULL,
	[Lat] [decimal](11,7) NULL,
	[Lng] [decimal](11,7) NULL,
	[Location] AS (
		CASE
			WHEN [Lat] <> 0 AND [Lng] <> 0 THEN
				geography::STGeomFromText('POINT(' + 
					CONVERT(VARCHAR(100),[Lng]) + ' ' +  
					CONVERT(VARCHAR(100),[Lat]) + ')',4326)
			ELSE NULL
		END -- http://stackoverflow.com/a/5573131/424788
	),
	[IsDeleted] [bit] default(0) NULL,
	[UpdatedAt] [datetime] default(CURRENT_TIMESTAMP) NULL,
	[UpdatedBy] [nvarchar](100) default('Admin') NULL
 CONSTRAINT [PK_Addresses] PRIMARY KEY CLUSTERED ( [ID] ASC )
)
GO

IF OBJECT_ID('[dbo].[Contacts]', 'U') IS NOT NULL  DROP TABLE [dbo].[Contacts]
GO

CREATE TABLE [dbo].[Contacts](
	[ID] [int] NOT NULL,
	[Gender] [nvarchar](10) NULL,
	[FamilyID] [int] NULL,
	[FirstName] [nvarchar](100) NULL,
	[LastName] [nvarchar](100) NULL,
	[Mobile] [nvarchar](20) NULL,
	[MobileVisibility] [int] NULL,
	[MobileUsability] [int] NULL,
	[Email] [nvarchar](100) NULL,
	[EmailVisibility] [int] NULL,
	[EmailUsability] [int] NULL,
	[FacebookId] [nvarchar](50) NULL,
	[FacebookIdVisibility] [int] NULL,
	[IsDeleted] [bit] default(0) NULL,
	[UpdatedAt] [datetime] default(CURRENT_TIMESTAMP) NULL,
	[UpdatedBy] [nvarchar](100) default('Admin') NULL
 CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED ( [ID] ASC )
)
GO

IF OBJECT_ID('[dbo].[Families]', 'U') IS NOT NULL  DROP TABLE [dbo].[Families]
GO

CREATE TABLE [dbo].[Families](
	[ID] [int] NOT NULL,
	[AddressID] [int] NULL,
	[FamilyName] [nvarchar](100) NULL,
	[Children] [nvarchar](255) NULL,
	[Phone] [nvarchar](20) NULL,
	[ShowInDirectory] [bit] NULL,
	[IsDeleted] [bit] default(0) NULL,
	[UpdatedAt] [datetime] default(CURRENT_TIMESTAMP) NULL,
	[UpdatedBy] [nvarchar](100) default('Admin') NULL
 CONSTRAINT [PK_Families] PRIMARY KEY CLUSTERED ( [ID] ASC )
)
GO


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'GetNearestNeighbours') AND type in (N'P', N'PC'))
DROP PROCEDURE [GetNearestNeighbours]
GO

CREATE PROCEDURE [dbo].[GetNearestNeighbours]
	@AddressId int,
	@NumberOfNeighbours int
AS
BEGIN


DECLARE		@g GEOGRAPHY;

SELECT		@g = Location 
FROM		Addresses 
WHERE		ID = @AddressId

SELECT		TOP(@NumberOfNeighbours) 
			a.ID AddressId,
			a.FamilyId,
			Location.STDistance(@g) Distance 
FROM		Addresses a
WHERE		Location IS NOT NULL
AND			ID <> @AddressId
ORDER BY	Location.STDistance(@g);

-- NOTE: Column is computed so cannot index.
-- To index we'd use:
-- CREATE SPATIAL INDEX SIndx_Addresses_Location 
--   ON Addresses(Location);

END
