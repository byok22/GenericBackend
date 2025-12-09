/* ==================================================================================
   Script: Create Database
   Author: Kevin Torruco
   Date:   2025-12-09
   ================================================================================== */

USE [master]
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'TE_GenericProject')
BEGIN
    PRINT 'Creating database [TE_GenericProject]...'
    CREATE DATABASE [TE_GenericProject]
END
ELSE
BEGIN
    PRINT 'Database [TE_GenericProject] already exists.'
END
GO

-- Seleccionar la base de datos para los siguientes scripts
USE [TE_GenericProject]
GO

-- Crear el tipo de dato IdList que se usa en los SPs (por si acaso no se creó antes)
IF NOT EXISTS (SELECT * FROM sys.types WHERE is_table_type = 1 AND name = 'IdList')
BEGIN
    CREATE TYPE [dbo].[IdList] AS TABLE(
        [ID] [int] NOT NULL,
        PRIMARY KEY CLUSTERED 
    (
        [ID] ASC
    )WITH (IGNORE_DUP_KEY = OFF)
    )
END
GO
USE [TE_GenericProject]
GO

/****** Object:  Table [dbo].[CT_Roles]    Script Date: 12/09/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CT_Roles](
    [PKRole] [int] IDENTITY(1,1) NOT NULL,
    [Role] [varchar](20) NULL,
    [Available] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
    [PKRole] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[CT_Users]    Script Date: 12/09/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CT_Users](
    [PKUser] [int] IDENTITY(1,1) NOT NULL,
    [NTAccount] [varchar](50) NULL,
    [UserName] [varchar](250) NULL,
    [Email] [varchar](150) NULL,
    [FKRole] [int] NULL,
    [Available] [bit] NULL,
    [CreatedBy] [nchar](50) NULL,
    [UpdatedBy] [nchar](50) NULL,
    [CreatedAt] [datetime] NULL,
    [UpdatedAt] [datetime] NULL,
 CONSTRAINT [PK__CT_Users__593F5E2E7CC5DC87] PRIMARY KEY CLUSTERED 
(
    [PKUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[SC_AppScreen]    Script Date: 12/09/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SC_AppScreen](
    [PKAppScreen] [int] IDENTITY(1,1) NOT NULL,
    [FKParentAppScreen] [int] NOT NULL,
    [Screen] [varchar](200) NOT NULL,
    [Url] [varchar](250) NULL,
    [SortOrder] [int] NOT NULL,
    [Icon] [varchar](30) NOT NULL,
    [FKUser] [int] NOT NULL,
    [Available] [bit] NULL,
 CONSTRAINT [PK__SC_AppSc__03A56E3D8FC5272B] PRIMARY KEY CLUSTERED 
(
    [PKAppScreen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[SC_AppScreenRole]    Script Date: 12/09/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SC_AppScreenRole](
    [PKScreenRoles] [int] IDENTITY(1,1) NOT NULL,
    [FKScreen] [int] NOT NULL,
    [FKRoles] [int] NOT NULL,
 CONSTRAINT [PK__SC_Scree__73A3E28319F6AB5A] PRIMARY KEY CLUSTERED 
(
    [PKScreenRoles] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[SY_AppProperties]    Script Date: 12/09/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SY_AppProperties](
    [PKProperty] [int] NULL,
    [Property] [varchar](50) NULL,
    [Value] [varchar](250) NULL,
    [Available] [bit] NULL
) ON [PRIMARY]
GO

/****** Object:  StoredProcedure [dbo].[up_AddAppScreen]    Script Date: 12/09/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:       Kevin Torruco
-- Create date:  2025-12-09
-- Description:  Add App Screen
-- =============================================
CREATE PROCEDURE [dbo].[up_AddAppScreen]
    -- El C# envía @PKAppScreen, pero es un IDENTITY, así que lo ignoramos en el INSERT
    @PKAppScreen INT, 
    @FKParentAppScreen INT,
    @Screen VARCHAR(200),
    @Url VARCHAR(250),
    @Sortorder INT,
    @Icon VARCHAR(30),
    @FKUser INT,
    @Available BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    -- Insertar el nuevo registro
    INSERT INTO [dbo].[SC_AppScreen] (
        [FKParentAppScreen],
        [Screen],
        [Url],
        [SortOrder],
        [Icon],
        [FKUser],
        [Available] -- Asumimos que los nuevos registros siempre están disponibles
    )
    VALUES (
        @FKParentAppScreen,
        @Screen,
        @Url,
        @Sortorder,
        @Icon,
        @FKUser,
       @Available
    );

    -- Obtener el ID recién creado
    DECLARE @NewPKAppScreen INT = SCOPE_IDENTITY();

    -- Devolver el registro completo, como lo espera tu método AddAsync
    SELECT
        @NewPKAppScreen AS PKAppScreen,
        @FKParentAppScreen AS FKParentAppScreen,
        @Screen AS Screen,
        @Url AS Url,
        @Sortorder AS Sortorder,
        @Icon AS Icon,
        @FKUser AS FKUser,
        1 AS Available;
END
GO

USE [TE_GenericProject]
GO

/* ==================================================================================
   REGION: CT_Roles Stored Procedures
   ================================================================================== */

/****** Object:  StoredProcedure [dbo].[up_GetAllRoles]    Script Date: 12/09/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:       Kevin Torruco
-- Create date:  2025-12-09
-- Description:  Get All Roles
-- =============================================
CREATE PROCEDURE [dbo].[up_GetAllRoles] 
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        CTR.[PKRole] 
        ,CTR.[Role]
        ,CTR.[Available]
     FROM 
        [dbo].[CT_Roles] CTR WITH(NOLOCK)
END
GO

/****** Object:  StoredProcedure [dbo].[up_GetRoleById]    Script Date: 12/09/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:       Kevin Torruco
-- Create date:  2025-12-09
-- Description:  Get Role By ID
-- =============================================
CREATE PROCEDURE [dbo].[up_GetRoleById] 
    @PKRole INTEGER
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        CTR.[PKRole]
        ,CTR.[Role]
        ,CTR.[Available]
    FROM 
        [dbo].[CT_Roles] CTR WITH(NOLOCK)
    WHERE
        PKRole = @PKRole
END
GO

/****** Object:  StoredProcedure [dbo].[up_InsertRole]    Script Date: 12/09/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:       Kevin Torruco
-- Create date:  2025-12-09
-- Description:  Insert Role
-- =============================================
CREATE PROCEDURE [dbo].[up_InsertRole]
    @Role VARCHAR(20)
    ,@Available BIT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO [dbo].[CT_Roles]
            ([Role]
            ,[Available])
     VALUES 
            (@Role
            ,@Available)
    
    SELECT
            CTR.PKRole        
            ,CTR.Role AS RoleName
            ,CTR.Available
    FROM
            [dbo].[CT_Roles] CTR WITH(NOLOCK)
    WHERE PKRole = SCOPE_IDENTITY()
END
GO

/****** Object:  StoredProcedure [dbo].[up_UpdateRole]    Script Date: 12/09/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:       Kevin Torruco
-- Create date:  2025-12-09
-- Description:  Update Role
-- =============================================
CREATE PROCEDURE [dbo].[up_UpdateRole] 
    @PKRole INTEGER
    ,@Role VARCHAR(20)
    ,@Available BIT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE CTR
    SET 
        CTR.[Role] = @Role
        ,CTR.[Available] = @Available
    FROM 
        [dbo].[CT_Roles] CTR WITH(NOLOCK)
    WHERE 
        PKRole = @PKRole
END
GO

/****** Object:  StoredProcedure [dbo].[up_DeleteRol]    Script Date: 12/09/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:       Kevin Torruco    
-- Create date:  2025-12-09
-- Description:  Delete Role
-- =============================================
CREATE PROCEDURE [dbo].[up_DeleteRol] 
    @PKRole INTEGER
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM 
        [dbo].[CT_Roles]
    WHERE 
        PKRole = @PKRole
END
GO

/* ==================================================================================
   REGION: CT_Users Stored Procedures
   ================================================================================== */

/****** Object:  StoredProcedure [dbo].[up_GetAllUsers]    Script Date: 12/09/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:       Kevin Torruco
-- Create date:  2025-12-09
-- =============================================
CREATE PROCEDURE [dbo].[up_GetAllUsers]
AS
BEGIN
    SET NOCOUNT ON;
  SELECT  [PKUser]  Id
      ,[NTAccount] NTAccount
      ,[UserName] UserName
      ,[Email]
      ,R.Role       
      ,U.[Available]
  FROM [dbo].[CT_Users] U (NOLOCK) 
  left join CT_Roles R  (NOLOCK) on U.FKRole = R.PKRole 
END;
GO

/****** Object:  StoredProcedure [dbo].[up_GetUserById]    Script Date: 12/09/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:       Kevin Torruco
-- Create date:  2025-12-09
-- =============================================
CREATE PROCEDURE [dbo].[up_GetUserById]
@Id INT = 0
AS
BEGIN
    SET NOCOUNT ON;
  SELECT  [PKUser]  Id
      ,[NTAccount] NTUser
      ,[UserName] UserName
      ,[Email]
      ,R.Role       
      ,U.[Available]
  FROM [dbo].[CT_Users] U (NOLOCK) 
  left join CT_Roles R  (NOLOCK) on U.FKRole = R.PKRole 
  where PKUser = @Id
END;
GO

/****** Object:  StoredProcedure [dbo].[up_GetUserByNTUser]    Script Date: 12/09/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:       Kevin Torruco
-- Create date:  2025-12-09
-- =============================================
CREATE PROCEDURE [dbo].[up_GetUserByNTUser]
@NTUser varchar(50) = ''
AS
BEGIN
    SET NOCOUNT ON;
  SELECT  [PKUser]  Id
      ,[NTAccount] NTUser
      ,[UserName] UserName
      ,[Email]
      ,R.Role       
      ,U.[Available]
  FROM [dbo].[CT_Users] U (NOLOCK) 
  left join CT_Roles R  (NOLOCK) on U.FKRole = R.PKRole 
  where NTAccount = @NTUser
END;
GO

/****** Object:  StoredProcedure [dbo].[up_AddUser]    Script Date: 12/09/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:       Kevin Torruco
-- Create date:  2025-12-09
-- =============================================
CREATE PROCEDURE [dbo].[up_AddUser]
    @NTUser NVARCHAR(255),
    @UserName NVARCHAR(255) = NULL, 
    @Email NVARCHAR(255) = NULL,
    @Role NVARCHAR(255),
    @Available BIT  ,
    @CreatedBy  NVARCHAR(255) = ''
AS
BEGIN
    DECLARE @NewId INT;
    DECLARE @Message NVARCHAR(255);
    DECLARE @fkRole INT = 0

    BEGIN TRY
        SET @fkRole = TRY_CAST(@Role AS INT);

        INSERT INTO [dbo].[CT_Users]
            ([NTAccount]                
            ,[UserName]
            ,[Email]
            ,[FKRole]
            ,[Available]
            ,CreatedBy
            )
        VALUES
             (@NTUser 
            ,@UserName 
            ,@Email 
            ,@fkRole 
            ,@Available
            ,@CreatedBy)
        
        SET @NewId = SCOPE_IDENTITY();
        SET @Message = 'User created successfully';

        -- Return the new record
        SELECT 
               [PKUser] AS Id
              ,NTAccount 
              ,[UserName]
              ,[Email]
              ,CAST([FKRole] AS VARCHAR(50))  AS Role  
              ,[Available]
        FROM [dbo].[CT_Users]
        WHERE [PKUser] = @NewId;
    END TRY
    BEGIN CATCH
        -- Error handling
        SELECT NULL AS Id, ERROR_MESSAGE() AS Message;
    END CATCH
END;
GO

/****** Object:  StoredProcedure [dbo].[up_UpdateUser]    Script Date: 12/09/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:       Kevin Torruco
-- Create date:  2025-12-09
-- =============================================
CREATE PROCEDURE [dbo].[up_UpdateUser] 
    @Id                INT
    ,@NTUser        VARCHAR(50)
    ,@UserName        VARCHAR(250)
    ,@Email            VARCHAR(150)
    ,@FKRole        INT
    ,@Available        BIT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE CTU
    SET 
        CTU.[NTAccount]        =    @NTUser 
        ,CTU.[Email]        =    @Email
        ,CTU.[FKRole]        =    @FKRole    
        ,CTU.[UserName]        =    @UserName
        ,CTU.[Available]    =    @Available
    FROM 
        [dbo].CT_Users CTU WITH(NOLOCK)
     WHERE 
        PKUser = @Id
END
GO

/****** Object:  StoredProcedure [dbo].[up_DeleteUser]    Script Date: 12/09/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:       Kevin Torruco
-- Create date:  2025-12-09
-- =============================================
CREATE PROCEDURE [dbo].[up_DeleteUser] 
    @PKUser            INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM 
        [dbo].[CT_Users]
    WHERE 
        PKUser = @PKUser 
END
GO

/* ==================================================================================
   REGION: SC_AppScreen & Roles Stored Procedures
   ================================================================================== */

/****** Object:  StoredProcedure [dbo].[up_GetAppScreens]    Script Date: 12/09/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:       Kevin Torruco
-- Create date:  2025-12-09
-- =============================================
CREATE PROCEDURE [dbo].[up_GetAppScreens]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
       A. [PKAppScreen],
       A. [FKParentAppScreen],
        ISNULL( B.Screen,'') [ParentScreen],
        A.[Screen],
        A.[Url],
        A.[SortOrder],
        A.[Icon],
        A.[FKUser],
        A.[Available]
    FROM
        [dbo].[SC_AppScreen] A
        LEFT join SC_AppScreen B on A.FKParentAppScreen = B.PKAppScreen
  
    ORDER BY
        [SortOrder], [Screen];
END
GO

/****** Object:  StoredProcedure [dbo].[up_GetAppScreenById]    Script Date: 12/09/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:       Kevin Torruco
-- Create date:  2025-12-09
-- =============================================
CREATE PROCEDURE [dbo].[up_GetAppScreenById]
    @PKAppScreen INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        [PKAppScreen],
        [FKParentAppScreen],
        [Screen],
        [Url],
        [SortOrder],
        [Icon],
        [FKUser],
        [Available]
    FROM
        [dbo].[SC_AppScreen]
    WHERE
        [PKAppScreen] = @PKAppScreen
        AND [Available] = 1;
END
GO

/****** Object:  StoredProcedure [dbo].[Up_GetAppScreenByNtUser]    Script Date: 12/09/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:       Kevin Torruco
-- Create date:  2025-12-09
-- Description:  Get AppScreens By NtUser
-- =============================================
CREATE PROCEDURE [dbo].[Up_GetAppScreenByNtUser] 
    @ntUser varchar(150)
AS
BEGIN
    SET NOCOUNT ON;

      SELECT
        [PKAppScreen],
        [FKParentAppScreen],
        [Screen],
        [Url],
        [SortOrder],
        [Icon],
        [FKUser],
        A.[Available]
      
    FROM
        [dbo].[SC_AppScreen] A
        inner join SC_AppScreenRole S on A.PKAppScreen = S.FKScreen
        inner join  CT_Users U on U.FKRole = S.FKRoles
       
   
    WHERE
        U.NTAccount = @ntUser and  A.[Available] = 1
    ORDER BY
        [SortOrder], [Screen]; 
END
GO

/****** Object:  StoredProcedure [dbo].[up_ChgAppScreen]    Script Date: 12/09/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:       Kevin Torruco
-- Create date:  2025-12-09
-- =============================================
CREATE PROCEDURE [dbo].[up_ChgAppScreen]
    @PKAppScreen INT,
    @FKParentAppScreen INT,
    @Screen VARCHAR(200),
    @Url VARCHAR(250),
    @Sortorder INT,
    @Icon VARCHAR(30),
    @FKUser INT,
    @Available bit
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [dbo].[SC_AppScreen]
    SET
        [FKParentAppScreen] = @FKParentAppScreen,
        [Screen] = @Screen,
        [Url] = @Url,
        [SortOrder] = @Sortorder,
        [Icon] = @Icon,
        [FKUser] = @FKUser,
        [Available] = @Available 
      
    WHERE
        [PKAppScreen] = @PKAppScreen;

    SELECT @PKAppScreen AS PKAppScreen;
END
GO

/****** Object:  StoredProcedure [dbo].[up_RmvAppScreen]    Script Date: 12/09/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:       Kevin Torruco
-- Create date:  2025-12-09
-- =============================================
CREATE PROCEDURE [dbo].[up_RmvAppScreen]
    @PKAppScreen INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE [dbo].[SC_AppScreen]
    SET
        [Available] = 0 -- Soft delete
    WHERE
        [PKAppScreen] = @PKAppScreen;
    SELECT @PKAppScreen AS PKAppScreen;
END
GO

/****** Object:  StoredProcedure [dbo].[up_GetAppScreenRolesByRoleID]    Script Date: 12/09/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:       Kevin Torruco
-- Create date:  2025-12-09
-- =============================================
CREATE PROCEDURE [dbo].[up_GetAppScreenRolesByRoleID]
    @FKRoles INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        asr.PKScreenRoles,
        asr.FKScreen,
        asr.FKRoles,
        s.Screen AS ScreenName, 
        s.Url AS ScreenPath      
    FROM 
        dbo.SC_AppScreenRole asr WITH(NOLOCK)
    INNER JOIN 
        dbo.SC_AppScreen s WITH(NOLOCK) ON asr.FKScreen = s.PKAppScreen 
    WHERE 
        asr.FKRoles = @FKRoles;
END
GO

/****** Object:  StoredProcedure [dbo].[up_SyncAppScreenRoles]    Script Date: 12/09/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:       Kevin Torruco
-- Create date:  2025-12-09
-- =============================================
CREATE PROCEDURE [dbo].[up_SyncAppScreenRoles]
    @FKRoles INT,
    @ScreenIDs dbo.IdList READONLY
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        -- 1. BORRAR permisos que ya no están
        DELETE ASR
        FROM dbo.SC_AppScreenRole AS ASR
        WHERE ASR.FKRoles = @FKRoles
        AND NOT EXISTS (
            SELECT 1 FROM @ScreenIDs AS S WHERE S.ID = ASR.FKScreen
        );

        -- 2. INSERTAR nuevos permisos
        INSERT INTO dbo.SC_AppScreenRole (FKScreen, FKRoles)
        SELECT 
            S.ID, 
            @FKRoles
        FROM @ScreenIDs AS S
        WHERE NOT EXISTS (
            SELECT 1 FROM dbo.SC_AppScreenRole AS ASR WHERE ASR.FKRoles = @FKRoles AND ASR.FKScreen = S.ID
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION;
        THROW; 
    END CATCH;
END
GO

/* ==================================================================================
   REGION: SY_AppProperties (Generated basics)
   ================================================================================== */

/****** Object:  StoredProcedure [dbo].[up_GetAllAppProperties]    Script Date: 12/09/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:       Kevin Torruco
-- Create date:  2025-12-09
-- Description:  Get All System Properties
-- =============================================
CREATE PROCEDURE [dbo].[up_GetAllAppProperties]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        [PKProperty],
        [Property],
        [Value],
        [Available]
    FROM [dbo].[SY_AppProperties] WITH(NOLOCK)
END
GO

/****** Object:  StoredProcedure [dbo].[up_UpdateAppProperty]    Script Date: 12/09/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:       Kevin Torruco
-- Create date:  2025-12-09
-- Description:  Update System Property
-- =============================================
CREATE PROCEDURE [dbo].[up_UpdateAppProperty]
    @Property VARCHAR(50),
    @Value VARCHAR(250)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE [dbo].[SY_AppProperties]
    SET [Value] = @Value
    WHERE [Property] = @Property;
END
GO