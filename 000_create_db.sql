USE [master]
GO
/****** Object:  Database [3m_dashboard_db]    Script Date: 12/12/2020 8:41:56 PM ******/
CREATE DATABASE [3m_dashboard_db]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'3m_dashboard_db', FILENAME = N'/var/opt/mssql/data/3m_dashboard_db.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'3m_dashboard_db_log', FILENAME = N'/var/opt/mssql/data/3m_dashboard_db_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [3m_dashboard_db] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [3m_dashboard_db].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [3m_dashboard_db] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [3m_dashboard_db] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [3m_dashboard_db] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [3m_dashboard_db] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [3m_dashboard_db] SET ARITHABORT OFF 
GO
ALTER DATABASE [3m_dashboard_db] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [3m_dashboard_db] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [3m_dashboard_db] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [3m_dashboard_db] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [3m_dashboard_db] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [3m_dashboard_db] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [3m_dashboard_db] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [3m_dashboard_db] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [3m_dashboard_db] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [3m_dashboard_db] SET  DISABLE_BROKER 
GO
ALTER DATABASE [3m_dashboard_db] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [3m_dashboard_db] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [3m_dashboard_db] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [3m_dashboard_db] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [3m_dashboard_db] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [3m_dashboard_db] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [3m_dashboard_db] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [3m_dashboard_db] SET RECOVERY FULL 
GO
ALTER DATABASE [3m_dashboard_db] SET  MULTI_USER 
GO
ALTER DATABASE [3m_dashboard_db] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [3m_dashboard_db] SET DB_CHAINING OFF 
GO
ALTER DATABASE [3m_dashboard_db] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [3m_dashboard_db] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [3m_dashboard_db] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'3m_dashboard_db', N'ON'
GO
ALTER DATABASE [3m_dashboard_db] SET QUERY_STORE = OFF
GO
USE [3m_dashboard_db]
GO
/****** Object:  StoredProcedure [dbo].[sp_get_active_process]    Script Date: 12/12/2020 8:41:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_get_active_process]
	@date datetime = null
AS
BEGIN

Select * From  (values 
 ('Guillotine'    ,'TEST-ORD-01','TEST-PRO-01','TEST-DESC-01',CAST(RAND()*(50-20)+20 as int), CAST(RAND()*(2000-1500)+1500 as int))
,('Blister'       ,'TEST-ORD-01','TEST-PRO-01','TEST-DESC-01',CAST(RAND()*(50-20)+20 as int), CAST(RAND()*(2000-1500)+1500 as int))
,('Bundler'       ,'TEST-ORD-01','TEST-PRO-01','TEST-DESC-01',CAST(RAND()*(50-20)+20 as int), CAST(RAND()*(2000-1500)+1500 as int))
,('3x3 Wrapper'   ,'TEST-ORD-01','TEST-PRO-01','TEST-DESC-01',CAST(RAND()*(50-20)+20 as int), CAST(RAND()*(2000-1500)+1500 as int))
,('Multi Wrapper' ,'TEST-ORD-01','TEST-PRO-01','TEST-DESC-01',CAST(RAND()*(50-20)+20 as int), CAST(RAND()*(2000-1500)+1500 as int))
) A(machine,[order],product,product_desc,completion,rate_hr);

END
GO
/****** Object:  StoredProcedure [dbo].[sp_get_output]    Script Date: 12/12/2020 8:41:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_get_output]
	@date datetime = null
AS
BEGIN

Select * From  (values 
 (CAST(RAND()*(100-20)+20 as int), CAST(RAND()*(100-20)+20 as int), CAST(RAND()*(100-20)+20 as int), CAST(RAND()*(100-20)+20 as int))
) A(oee,availability,performance, quality);

END
GO
/****** Object:  StoredProcedure [dbo].[sp_get_production_target_output]    Script Date: 12/12/2020 8:41:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_get_production_target_output]
	@date datetime = null
AS
BEGIN

Select * From  (values 
 ('Guillotine',CAST(RAND()*(2000-1500)+1500 as int), CAST(RAND()*(2000-1500)+1500 as int))
,('Blister',CAST(RAND()*(2000-1500)+1500 as int), CAST(RAND()*(2000-1500)+1500 as int))
,('Bundler',CAST(RAND()*(2000-1500)+1500 as int), CAST(RAND()*(2000-1500)+1500 as int))
,('3x3 Wrapper',CAST(RAND()*(2000-1500)+1500 as int), CAST(RAND()*(2000-1500)+1500 as int))
,('Multi Wrapper',CAST(RAND()*(2000-1500)+1500 as int), CAST(RAND()*(2000-1500)+1500 as int))
) A(category,Value1,Value2);

END
GO
/****** Object:  StoredProcedure [dbo].[sp_get_quality]    Script Date: 12/12/2020 8:41:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_get_quality]
	@date datetime = null
AS
BEGIN

Select * From  (values 
 ('Guillotine'    ,CAST(RAND()*(50-20)+20 as int),CAST(RAND()*(20-10)+10 as int))
,('Blister'       ,CAST(RAND()*(50-20)+20 as int),CAST(RAND()*(20-10)+10 as int))
,('Bundler'       ,CAST(RAND()*(50-20)+20 as int),CAST(RAND()*(20-10)+10 as int))
,('3x3 Wrapper'   ,CAST(RAND()*(50-20)+20 as int),CAST(RAND()*(20-10)+10 as int))
,('Multi Wrapper' ,CAST(RAND()*(50-20)+20 as int),CAST(RAND()*(20-10)+10 as int))
) A(category,qulity,defect);


END
GO
/****** Object:  StoredProcedure [dbo].[sp_get_runtime_losses]    Script Date: 12/12/2020 8:41:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_get_runtime_losses]
	@date datetime = null
AS
BEGIN

Select * From  (values 
 ('Guillotine'    ,CAST(RAND()*(50-20)+20 as int),CAST(RAND()*(50-20)+20 as int),CAST(RAND()*(50-20)+20 as int),CAST(RAND()*(50-20)+20 as int))
,('Blister'       ,CAST(RAND()*(50-20)+20 as int),CAST(RAND()*(50-20)+20 as int),CAST(RAND()*(50-20)+20 as int),CAST(RAND()*(50-20)+20 as int))
,('Bundler'       ,CAST(RAND()*(50-20)+20 as int),CAST(RAND()*(50-20)+20 as int),CAST(RAND()*(50-20)+20 as int),CAST(RAND()*(50-20)+20 as int))
,('3x3 Wrapper'   ,CAST(RAND()*(50-20)+20 as int),CAST(RAND()*(50-20)+20 as int),CAST(RAND()*(50-20)+20 as int),CAST(RAND()*(50-20)+20 as int))
,('Multi Wrapper' ,CAST(RAND()*(50-20)+20 as int),CAST(RAND()*(50-20)+20 as int),CAST(RAND()*(50-20)+20 as int),CAST(RAND()*(50-20)+20 as int))
) A(category,Value1,Value2,Value3,Value4);


END
GO
USE [master]
GO
ALTER DATABASE [3m_dashboard_db] SET  READ_WRITE 
GO
----------------------------------------------------------------
USE [master]
GO
ALTER DATABASE [3m_dashboard_db] SET  READ_WRITE 
GO
CREATE LOGIN cim WITH PASSWORD = '4dev@cim';  
GO
ALTER AUTHORIZATION ON DATABASE::[3m_dashboard_db] TO [cim]
GO
