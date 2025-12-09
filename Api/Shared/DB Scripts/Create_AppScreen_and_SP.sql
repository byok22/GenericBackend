-- Script: Create AppScreen table and stored procedures
-- Ajusta tipos y nombres de columnas según tu diseño real si es necesario

IF OBJECT_ID('dbo.AppScreen', 'U') IS NOT NULL
    DROP TABLE dbo.AppScreen;
GO

CREATE TABLE dbo.AppScreen (
    PKAppScreen INT IDENTITY(1,1) PRIMARY KEY,
    FKParentAppScreen INT NULL,
    ParentScreen NVARCHAR(250) NULL,
    Screen NVARCHAR(250) NOT NULL,
    Url NVARCHAR(500) NULL,
    SortOrder INT NULL,
    Icon NVARCHAR(250) NULL,
    FKUser INT NULL,
    Available BIT NOT NULL DEFAULT(1)
);
GO

-- Stored procedure: up_AddAppScreen
IF OBJECT_ID('dbo.up_AddAppScreen','P') IS NOT NULL
    DROP PROCEDURE dbo.up_AddAppScreen;
GO
CREATE PROCEDURE dbo.up_AddAppScreen
    @PKAppScreen INT = 0,
    @FKParentAppScreen INT = NULL,
    @Screen NVARCHAR(250) = NULL,
    @Url NVARCHAR(500) = NULL,
    @Sortorder INT = NULL,
    @Icon NVARCHAR(250) = NULL,
    @FKUser INT = NULL,
    @Available BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    IF @PKAppScreen IS NULL OR @PKAppScreen = 0
    BEGIN
        INSERT INTO dbo.AppScreen (FKParentAppScreen, Screen, Url, SortOrder, Icon, FKUser, Available)
        VALUES (@FKParentAppScreen, @Screen, @Url, @Sortorder, @Icon, @FKUser, @Available);

        SELECT SCOPE_IDENTITY() AS PKAppScreen, @FKParentAppScreen AS FKParentAppScreen, @Screen AS Screen, @Url AS Url, @Sortorder AS Sortorder, @Icon AS Icon, @FKUser AS FKUser, @Available AS Available;
    END
    ELSE
    BEGIN
        -- If PK provided, perform update style behavior and return row
        UPDATE dbo.AppScreen
        SET FKParentAppScreen = @FKParentAppScreen,
            Screen = @Screen,
            Url = @Url,
            SortOrder = @Sortorder,
            Icon = @Icon,
            FKUser = @FKUser,
            Available = @Available
        WHERE PKAppScreen = @PKAppScreen;

        SELECT @PKAppScreen AS PKAppScreen, @FKParentAppScreen AS FKParentAppScreen, @Screen AS Screen, @Url AS Url, @Sortorder AS Sortorder, @Icon AS Icon, @FKUser AS FKUser, @Available AS Available;
    END
END
GO

-- Stored procedure: up_ChgAppScreen (update)
IF OBJECT_ID('dbo.up_ChgAppScreen','P') IS NOT NULL
    DROP PROCEDURE dbo.up_ChgAppScreen;
GO
CREATE PROCEDURE dbo.up_ChgAppScreen
    @PKAppScreen INT,
    @FKParentAppScreen INT = NULL,
    @Screen NVARCHAR(250) = NULL,
    @Url NVARCHAR(500) = NULL,
    @Sortorder INT = NULL,
    @Icon NVARCHAR(250) = NULL,
    @FKUser INT = NULL,
    @Available BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.AppScreen
    SET FKParentAppScreen = @FKParentAppScreen,
        Screen = @Screen,
        Url = @Url,
        SortOrder = @Sortorder,
        Icon = @Icon,
        FKUser = @FKUser,
        Available = @Available
    WHERE PKAppScreen = @PKAppScreen;

    SELECT @PKAppScreen AS PKAppScreen;
END
GO

-- Stored procedure: up_RmvAppScreen
IF OBJECT_ID('dbo.up_RmvAppScreen','P') IS NOT NULL
    DROP PROCEDURE dbo.up_RmvAppScreen;
GO
CREATE PROCEDURE dbo.up_RmvAppScreen
    @PKAppScreen INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.AppScreen WHERE PKAppScreen = @PKAppScreen;

    SELECT @PKAppScreen AS PKAppScreen;
END
GO

-- Stored procedure: up_GetAppScreens (get all)
IF OBJECT_ID('dbo.up_GetAppScreens','P') IS NOT NULL
    DROP PROCEDURE dbo.up_GetAppScreens;
GO
CREATE PROCEDURE dbo.up_GetAppScreens
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.PKAppScreen,
        s.FKParentAppScreen,
        (p.Screen) AS ParentScreen,
        s.Screen,
        s.Url,
        s.SortOrder,
        s.Icon,
        s.FKUser AS FKUser,
        s.Available
    FROM dbo.AppScreen s
    LEFT JOIN dbo.AppScreen p ON p.PKAppScreen = s.FKParentAppScreen
    ORDER BY s.SortOrder;
END
GO

-- Stored procedure: Up_GetScreenById
IF OBJECT_ID('dbo.Up_GetScreenById','P') IS NOT NULL
    DROP PROCEDURE dbo.Up_GetScreenById;
GO
CREATE PROCEDURE dbo.Up_GetScreenById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.PKAppScreen,
        s.FKParentAppScreen,
        (p.Screen) AS ParentScreen,
        s.Screen,
        s.Url,
        s.SortOrder,
        s.Icon,
        s.FKUser AS FKUser,
        s.Available
    FROM dbo.AppScreen s
    LEFT JOIN dbo.AppScreen p ON p.PKAppScreen = s.FKParentAppScreen
    WHERE s.PKAppScreen = @Id;
END
GO

-- Stored procedure: Up_GetAppScreenByNtUser (example by FKUser)
IF OBJECT_ID('dbo.Up_GetAppScreenByNtUser','P') IS NOT NULL
    DROP PROCEDURE dbo.Up_GetAppScreenByNtUser;
GO
CREATE PROCEDURE dbo.Up_GetAppScreenByNtUser
    @ntUser NVARCHAR(250)
AS
BEGIN
    SET NOCOUNT ON;

    -- This example assumes FKUser maps to a user id; adapt join if needed
    SELECT
        s.PKAppScreen,
        s.FKParentAppScreen,
        s.Screen,
        s.Url,
        s.SortOrder,
        s.Icon,
        s.FKUser AS FKUser,
        s.Available
    FROM dbo.AppScreen s
    WHERE s.FKUser = (SELECT TOP 1 PKUser FROM dbo.[User] WHERE NTUser = @ntUser)
    ORDER BY s.SortOrder;
END
GO
