USE [master]
GO
/****** Object:  Database [WeatherBotDatabase]    Script Date: 2/27/2025 7:05:54 PM ******/
CREATE DATABASE [WeatherBotDatabase]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'WheaterBotDatabase', FILENAME = N'C:\Users\Qamel\WheaterBotDatabase.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'WheaterBotDatabase_log', FILENAME = N'C:\Users\Qamel\WheaterBotDatabase_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [WeatherBotDatabase] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [WeatherBotDatabase].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [WeatherBotDatabase] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [WeatherBotDatabase] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [WeatherBotDatabase] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [WeatherBotDatabase] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [WeatherBotDatabase] SET ARITHABORT OFF 
GO
ALTER DATABASE [WeatherBotDatabase] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [WeatherBotDatabase] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [WeatherBotDatabase] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [WeatherBotDatabase] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [WeatherBotDatabase] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [WeatherBotDatabase] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [WeatherBotDatabase] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [WeatherBotDatabase] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [WeatherBotDatabase] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [WeatherBotDatabase] SET  DISABLE_BROKER 
GO
ALTER DATABASE [WeatherBotDatabase] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [WeatherBotDatabase] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [WeatherBotDatabase] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [WeatherBotDatabase] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [WeatherBotDatabase] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [WeatherBotDatabase] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [WeatherBotDatabase] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [WeatherBotDatabase] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [WeatherBotDatabase] SET  MULTI_USER 
GO
ALTER DATABASE [WeatherBotDatabase] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [WeatherBotDatabase] SET DB_CHAINING OFF 
GO
ALTER DATABASE [WeatherBotDatabase] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [WeatherBotDatabase] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [WeatherBotDatabase] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [WeatherBotDatabase] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [WeatherBotDatabase] SET QUERY_STORE = OFF
GO
USE [WeatherBotDatabase]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2/27/2025 7:05:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[TelegramId] [bigint] NOT NULL,
	[TelegramName] [nvarchar](255) NULL,
	[CreatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TelegramId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WeatherHistory]    Script Date: 2/27/2025 7:05:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WeatherHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NULL,
	[RequestedCity] [nvarchar](255) NOT NULL,
	[RequestTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[WeatherHistory] ADD  DEFAULT (getdate()) FOR [RequestTime]
GO
ALTER TABLE [dbo].[WeatherHistory]  WITH CHECK ADD  CONSTRAINT [FK_WeatherHistory_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([TelegramId])
GO
ALTER TABLE [dbo].[WeatherHistory] CHECK CONSTRAINT [FK_WeatherHistory_User]
GO
USE [master]
GO
ALTER DATABASE [WeatherBotDatabase] SET  READ_WRITE 
GO
