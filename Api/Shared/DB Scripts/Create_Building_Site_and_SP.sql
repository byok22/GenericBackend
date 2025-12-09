-- Script: Create Site and Building tables and stored procedures
-- Includes BasicFieldsModels: CreatedAt, UpdatedAt, CreatedBy, UpdatedBy

-- Drop tables if exist (caution in production)
IF OBJECT_ID('dbo.Building', 'U') IS NOT NULL
    DROP TABLE dbo.Building;
IF OBJECT_ID('dbo.Site', 'U') IS NOT NULL
    DROP TABLE dbo.Site;
GO

CREATE TABLE dbo.Site (
    PKSite INT IDENTITY(1,1) PRIMARY KEY,
    SiteName NVARCHAR(250) NOT NULL,
    Available BIT NOT NULL DEFAULT(1),
    CreatedAt DATETIME NOT NULL DEFAULT(GETDATE()),
    UpdatedAt DATETIME NOT NULL DEFAULT(GETDATE()),
    CreatedBy NVARCHAR(250) NULL,
    UpdatedBy NVARCHAR(250) NULL
);

CREATE TABLE dbo.Building (
    PKBuilding INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(250) NOT NULL,
    Description NVARCHAR(1000) NULL,
    FKSite INT NOT NULL,
    Available BIT NOT NULL DEFAULT(1),
    CreatedAt DATETIME NOT NULL DEFAULT(GETDATE()),
    UpdatedAt DATETIME NOT NULL DEFAULT(GETDATE()),
    CreatedBy NVARCHAR(250) NULL,
    UpdatedBy NVARCHAR(250) NULL,
    CONSTRAINT FK_Building_Site FOREIGN KEY (FKSite) REFERENCES dbo.Site(PKSite)
);
GO

-- up_AddSite
IF OBJECT_ID('dbo.up_AddSite','P') IS NOT NULL
    DROP PROCEDURE dbo.up_AddSite;
GO
CREATE PROCEDURE dbo.up_AddSite
    @PKSite INT = 0,
    @SiteName NVARCHAR(250) = NULL,
    @Available BIT = 1,
    @CreatedBy NVARCHAR(250) = NULL,
    @UpdatedBy NVARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @PKSite IS NULL OR @PKSite = 0
    BEGIN
        INSERT INTO dbo.Site (SiteName, Available, CreatedBy, UpdatedBy, CreatedAt, UpdatedAt)
        VALUES (@SiteName, @Available, @CreatedBy, @UpdatedBy, GETDATE(), GETDATE());
        SELECT SCOPE_IDENTITY() AS PKSite, @SiteName AS SiteName, @Available AS Available, GETDATE() AS CreatedAt, GETDATE() AS UpdatedAt, @CreatedBy AS CreatedBy, @UpdatedBy AS UpdatedBy;
    END
    ELSE
    BEGIN
        UPDATE dbo.Site SET SiteName = @SiteName, Available = @Available, UpdatedBy = @UpdatedBy, UpdatedAt = GETDATE() WHERE PKSite = @PKSite;
        SELECT @PKSite AS PKSite, @SiteName AS SiteName, @Available AS Available, GETDATE() AS CreatedAt, GETDATE() AS UpdatedAt, @CreatedBy AS CreatedBy, @UpdatedBy AS UpdatedBy;
    END
END
GO

-- up_GetSites
IF OBJECT_ID('dbo.up_GetSites','P') IS NOT NULL
    DROP PROCEDURE dbo.up_GetSites;
GO
CREATE PROCEDURE dbo.up_GetSites
AS
BEGIN
    SET NOCOUNT ON;
    SELECT PKSite, SiteName, Available, CreatedAt, UpdatedAt, CreatedBy, UpdatedBy FROM dbo.Site ORDER BY SiteName;
END
GO

-- Up_GetSiteById
IF OBJECT_ID('dbo.Up_GetSiteById','P') IS NOT NULL
    DROP PROCEDURE dbo.Up_GetSiteById;
GO
CREATE PROCEDURE dbo.Up_GetSiteById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT PKSite, SiteName, Available, CreatedAt, UpdatedAt, CreatedBy, UpdatedBy FROM dbo.Site WHERE PKSite = @Id;
END
GO

-- up_ChgSite
IF OBJECT_ID('dbo.up_ChgSite','P') IS NOT NULL
    DROP PROCEDURE dbo.up_ChgSite;
GO
CREATE PROCEDURE dbo.up_ChgSite
    @PKSite INT,
    @SiteName NVARCHAR(250),
    @Available BIT,
    @UpdatedBy NVARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Site SET SiteName = @SiteName, Available = @Available, UpdatedBy = @UpdatedBy, UpdatedAt = GETDATE() WHERE PKSite = @PKSite;
    SELECT @PKSite AS PKSite, 'Updated' AS Message;
END
GO

-- up_RmvSite
IF OBJECT_ID('dbo.up_RmvSite','P') IS NOT NULL
    DROP PROCEDURE dbo.up_RmvSite;
GO
CREATE PROCEDURE dbo.up_RmvSite
    @PKSite INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.Site WHERE PKSite = @PKSite;
    SELECT @PKSite AS PKSite, 'Deleted' AS Message;
END
GO

-- up_AddBuilding
IF OBJECT_ID('dbo.up_AddBuilding','P') IS NOT NULL
    DROP PROCEDURE dbo.up_AddBuilding;
GO
CREATE PROCEDURE dbo.up_AddBuilding
    @PKBuilding INT = 0,
    @Name NVARCHAR(250) = NULL,
    @Description NVARCHAR(1000) = NULL,
    @FKSite INT = NULL,
    @Available BIT = 1,
    @CreatedBy NVARCHAR(250) = NULL,
    @UpdatedBy NVARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @PKBuilding IS NULL OR @PKBuilding = 0
    BEGIN
        INSERT INTO dbo.Building (Name, Description, FKSite, Available, CreatedBy, UpdatedBy, CreatedAt, UpdatedAt)
        VALUES (@Name, @Description, @FKSite, @Available, @CreatedBy, @UpdatedBy, GETDATE(), GETDATE());
        SELECT SCOPE_IDENTITY() AS PKBuilding, @Name AS Name, @Description AS Description, @FKSite AS FKSite, @Available AS Available, GETDATE() AS CreatedAt, GETDATE() AS UpdatedAt, @CreatedBy AS CreatedBy, @UpdatedBy AS UpdatedBy;
    END
    ELSE
    BEGIN
        UPDATE dbo.Building SET Name = @Name, Description = @Description, FKSite = @FKSite, Available = @Available, UpdatedBy = @UpdatedBy, UpdatedAt = GETDATE() WHERE PKBuilding = @PKBuilding;
        SELECT @PKBuilding AS PKBuilding, @Name AS Name, @Description AS Description, @FKSite AS FKSite, @Available AS Available, GETDATE() AS CreatedAt, GETDATE() AS UpdatedAt, @CreatedBy AS CreatedBy, @UpdatedBy AS UpdatedBy;
    END
END
GO

-- up_GetBuildings
IF OBJECT_ID('dbo.up_GetBuildings','P') IS NOT NULL
    DROP PROCEDURE dbo.up_GetBuildings;
GO
CREATE PROCEDURE dbo.up_GetBuildings
AS
BEGIN
    SET NOCOUNT ON;
    SELECT b.PKBuilding, b.Name, b.Description, b.FKSite, b.Available, b.CreatedAt, b.UpdatedAt, b.CreatedBy, b.UpdatedBy FROM dbo.Building b ORDER BY b.Name;
END
GO

-- Up_GetBuildingById
IF OBJECT_ID('dbo.Up_GetBuildingById','P') IS NOT NULL
    DROP PROCEDURE dbo.Up_GetBuildingById;
GO
CREATE PROCEDURE dbo.Up_GetBuildingById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT PKBuilding, Name, Description, FKSite, Available, CreatedAt, UpdatedAt, CreatedBy, UpdatedBy FROM dbo.Building WHERE PKBuilding = @Id;
END
GO

-- up_GetBuildingsBySite
IF OBJECT_ID('dbo.up_GetBuildingsBySite','P') IS NOT NULL
    DROP PROCEDURE dbo.up_GetBuildingsBySite;
GO
CREATE PROCEDURE dbo.up_GetBuildingsBySite
    @SiteId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT PKBuilding, Name, Description, FKSite, Available, CreatedAt, UpdatedAt, CreatedBy, UpdatedBy FROM dbo.Building WHERE FKSite = @SiteId ORDER BY Name;
END
GO

-- up_ChgBuilding
IF OBJECT_ID('dbo.up_ChgBuilding','P') IS NOT NULL
    DROP PROCEDURE dbo.up_ChgBuilding;
GO
CREATE PROCEDURE dbo.up_ChgBuilding
    @PKBuilding INT,
    @Name NVARCHAR(250),
    @Description NVARCHAR(1000),
    @FKSite INT,
    @Available BIT,
    @UpdatedBy NVARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Building SET Name = @Name, Description = @Description, FKSite = @FKSite, Available = @Available, UpdatedBy = @UpdatedBy, UpdatedAt = GETDATE() WHERE PKBuilding = @PKBuilding;
    SELECT @PKBuilding AS PKBuilding, 'Updated' AS Message;
END
GO

-- up_RmvBuilding
IF OBJECT_ID('dbo.up_RmvBuilding','P') IS NOT NULL
    DROP PROCEDURE dbo.up_RmvBuilding;
GO
CREATE PROCEDURE dbo.up_RmvBuilding
    @PKBuilding INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.Building WHERE PKBuilding = @PKBuilding;
    SELECT @PKBuilding AS PKBuilding, 'Deleted' AS Message;
END
GO
