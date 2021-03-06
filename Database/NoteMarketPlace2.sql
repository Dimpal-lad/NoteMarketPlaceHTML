USE [master]
GO
/****** Object:  Database [NotesMarketPlace]    Script Date: 10-04-2021 15:36:42 ******/
CREATE DATABASE [NotesMarketPlace]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'NotesMarketPlace1', FILENAME = N'D:\SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\NotesMarketPlace1.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'NotesMarketPlace1_log', FILENAME = N'D:\SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\NotesMarketPlace1_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [NotesMarketPlace] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [NotesMarketPlace].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [NotesMarketPlace] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET ARITHABORT OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [NotesMarketPlace] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [NotesMarketPlace] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET  DISABLE_BROKER 
GO
ALTER DATABASE [NotesMarketPlace] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [NotesMarketPlace] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET RECOVERY FULL 
GO
ALTER DATABASE [NotesMarketPlace] SET  MULTI_USER 
GO
ALTER DATABASE [NotesMarketPlace] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [NotesMarketPlace] SET DB_CHAINING OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [NotesMarketPlace] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [NotesMarketPlace] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [NotesMarketPlace] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'NotesMarketPlace', N'ON'
GO
ALTER DATABASE [NotesMarketPlace] SET QUERY_STORE = OFF
GO
USE [NotesMarketPlace]
GO
/****** Object:  Table [dbo].[tblCountries]    Script Date: 10-04-2021 15:36:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCountries](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[CountryCode] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_tblCountries] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblDownloads]    Script Date: 10-04-2021 15:36:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblDownloads](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[Seller] [int] NOT NULL,
	[Downloader] [int] NOT NULL,
	[IsSellerHasAllowedDownload] [bit] NOT NULL,
	[AttachmentPath] [varchar](max) NULL,
	[IsAttachmentDownloaded] [bit] NOT NULL,
	[AttachmentDownloadedDate] [datetime] NULL,
	[IsPaid] [bit] NOT NULL,
	[PurchasedPrice] [decimal](18, 0) NULL,
	[NoteTitle] [varchar](100) NOT NULL,
	[NoteCategory] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_tblDownloads] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblNoteCategories]    Script Date: 10-04-2021 15:36:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblNoteCategories](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_tblNoteCategories] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblNoteTypes]    Script Date: 10-04-2021 15:36:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblNoteTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_tblNoteTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblReferenceData]    Script Date: 10-04-2021 15:36:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblReferenceData](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Value] [varchar](100) NOT NULL,
	[Datavalue] [varchar](100) NOT NULL,
	[RefCategory] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_tblReferenceData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSellerNotes]    Script Date: 10-04-2021 15:36:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSellerNotes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SellerID] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[ActionedBy] [int] NULL,
	[AdminRemarks] [varchar](max) NULL,
	[PublishedDate] [datetime] NULL,
	[Title] [varchar](100) NOT NULL,
	[Category] [int] NOT NULL,
	[DisplayPicture] [varchar](500) NULL,
	[NoteType] [int] NULL,
	[NumberofPages] [int] NULL,
	[Description] [varchar](max) NOT NULL,
	[UniversityName] [varchar](200) NULL,
	[Country] [int] NULL,
	[Course] [varchar](100) NULL,
	[CourseCode] [varchar](100) NULL,
	[Professor] [varchar](100) NULL,
	[IsPaid] [bit] NOT NULL,
	[SellingPrice] [decimal](18, 0) NULL,
	[NotesPreview] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_tblSellerNotes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSellerNotesAttachments]    Script Date: 10-04-2021 15:36:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSellerNotesAttachments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[FileName] [varchar](100) NOT NULL,
	[FilePath] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_tblSellerNotesAttachments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSellerNotesReportedIssues]    Script Date: 10-04-2021 15:36:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSellerNotesReportedIssues](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[ReportedByID] [int] NOT NULL,
	[AgainstDownloadID] [int] NOT NULL,
	[Remarks] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_tblSellerNotesReportedIssues] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSellerNotesReviews]    Script Date: 10-04-2021 15:36:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSellerNotesReviews](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[ReviewedByID] [int] NOT NULL,
	[AgainstDownloadsID] [int] NOT NULL,
	[Ratings] [decimal](18, 0) NOT NULL,
	[Comments] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_tblSellerNotesReviews] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSystemConfigurations]    Script Date: 10-04-2021 15:36:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSystemConfigurations](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Key] [varchar](100) NOT NULL,
	[Value] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_tblSystemConfigurations] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblUserProfile]    Script Date: 10-04-2021 15:36:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblUserProfile](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[DOB] [datetime] NULL,
	[Gender] [int] NULL,
	[SecondaryEmailAddress] [varchar](100) NULL,
	[CountryCode] [varchar](5) NOT NULL,
	[PhoneNumber] [varchar](20) NOT NULL,
	[ProfilePicture] [varchar](500) NULL,
	[AddressLine1] [varchar](100) NULL,
	[AddressLine2] [varchar](100) NULL,
	[City] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[ZipCode] [varchar](50) NULL,
	[Country] [varchar](50) NULL,
	[University] [varchar](100) NULL,
	[College] [varchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_tblUserProfile] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblUserRoles]    Script Date: 10-04-2021 15:36:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblUserRoles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_tblUserRoles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblUsers]    Script Date: 10-04-2021 15:36:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblUsers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[EmailID] [varchar](100) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[IsEmailVarified] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_tblUsers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tblCountries] ON 

INSERT [dbo].[tblCountries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'India', N'91', CAST(N'2021-03-15T10:41:40.000' AS DateTime), 56, NULL, NULL, 1)
INSERT [dbo].[tblCountries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (8, N'USA', N'1', CAST(N'2021-03-30T10:34:16.000' AS DateTime), 57, NULL, NULL, 1)
INSERT [dbo].[tblCountries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (9, N'Japan', N'81', CAST(N'2021-04-07T12:06:16.000' AS DateTime), 56, NULL, NULL, 1)
INSERT [dbo].[tblCountries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (10, N'Australia', N'61', CAST(N'2021-03-15T10:41:40.000' AS DateTime), 57, NULL, NULL, 1)
INSERT [dbo].[tblCountries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (11, N'France', N'33', CAST(N'2021-04-07T13:50:32.457' AS DateTime), 56, CAST(N'2021-04-07T13:51:09.117' AS DateTime), 56, 0)
SET IDENTITY_INSERT [dbo].[tblCountries] OFF
GO
SET IDENTITY_INSERT [dbo].[tblDownloads] ON 

INSERT [dbo].[tblDownloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2, 49, 54, 55, 1, N'/Images/54/49/Attachment/Attachment1_16002021.pdf', 1, CAST(N'2021-03-22T15:48:15.060' AS DateTime), 0, NULL, N'CPU', N'CE', CAST(N'2021-03-22T15:48:15.060' AS DateTime), 55, NULL, 55)
INSERT [dbo].[tblDownloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (3, 45, 55, 55, 0, NULL, 0, CAST(N'2021-03-22T15:50:23.510' AS DateTime), 1, CAST(1200 AS Decimal(18, 0)), N'DMBI', N'CE', CAST(N'2021-03-22T15:50:23.510' AS DateTime), 55, NULL, 55)
INSERT [dbo].[tblDownloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (4, 48, 54, 54, 1, N'/Images/54/48/Attachment/Attachment1_16432021.pdf', 1, CAST(N'2021-03-22T16:02:12.663' AS DateTime), 0, NULL, N'Maths', N'CE', CAST(N'2021-03-22T16:02:12.663' AS DateTime), 54, NULL, 54)
INSERT [dbo].[tblDownloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (5, 55, 54, 54, 1, N'/Images/54/55/Attachment/Attachment1_18552021.pdf', 1, CAST(N'2021-03-22T18:15:40.540' AS DateTime), 0, CAST(0 AS Decimal(18, 0)), N'Tarang', N'MCA', CAST(N'2021-03-22T18:15:40.540' AS DateTime), 54, NULL, 54)
INSERT [dbo].[tblDownloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (7, 50, 54, 54, 1, N'/Images/54/50/Attachment/Attachment1_16062021.pdf', 1, CAST(N'2021-03-22T18:24:23.383' AS DateTime), 0, CAST(0 AS Decimal(18, 0)), N'Sangharsh', N'MBA', CAST(N'2021-03-22T18:24:23.383' AS DateTime), 54, CAST(N'2021-03-29T17:38:22.523' AS DateTime), 54)
INSERT [dbo].[tblDownloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (11, 33, 54, 54, 0, NULL, 0, CAST(N'2021-03-22T18:27:41.993' AS DateTime), 1, CAST(123 AS Decimal(18, 0)), N'hjsd', N'CE', CAST(N'2021-03-22T18:27:41.993' AS DateTime), 54, NULL, 54)
INSERT [dbo].[tblDownloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (12, 42, 54, 54, 0, NULL, 0, CAST(N'2021-03-22T18:41:20.653' AS DateTime), 1, CAST(450 AS Decimal(18, 0)), N'Science', N'CE', CAST(N'2021-03-22T18:41:20.653' AS DateTime), 54, NULL, 54)
INSERT [dbo].[tblDownloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (13, 41, 54, 54, 0, NULL, 0, CAST(N'2021-03-23T09:38:39.723' AS DateTime), 1, CAST(234 AS Decimal(18, 0)), N'dgfjdngfh', N'CE', CAST(N'2021-03-23T09:38:39.723' AS DateTime), 54, NULL, 54)
INSERT [dbo].[tblDownloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (15, 29, 54, 54, 0, NULL, 0, CAST(N'2021-03-23T09:50:46.887' AS DateTime), 1, CAST(123 AS Decimal(18, 0)), N'bdhbfjd', N'MBA', CAST(N'2021-03-23T09:50:46.887' AS DateTime), 54, NULL, 54)
INSERT [dbo].[tblDownloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (19, 55, 54, 55, 1, N'/Images/54/55/Attachment/Attachment1_18552021.pdf', 1, CAST(N'2021-03-23T11:27:35.263' AS DateTime), 0, CAST(0 AS Decimal(18, 0)), N'Tarang', N'MCA', CAST(N'2021-03-23T11:27:35.263' AS DateTime), 55, NULL, 55)
INSERT [dbo].[tblDownloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (20, 31, 54, 55, 1, N'/Images/54/31/Attachment/Attachment1_04042021.pdf', 0, CAST(N'2021-03-23T11:52:11.980' AS DateTime), 1, CAST(123 AS Decimal(18, 0)), N'hjsd', N'CE', CAST(N'2021-03-23T11:52:11.980' AS DateTime), 55, NULL, 54)
INSERT [dbo].[tblDownloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (24, 40, 54, 55, 1, N'/Images/54/40/Attachment/Attachment1_06392021.pdf', 0, CAST(N'2021-03-23T12:54:59.517' AS DateTime), 1, CAST(2344 AS Decimal(18, 0)), N'Science', N'CE', CAST(N'2021-03-23T12:54:59.517' AS DateTime), 55, NULL, 54)
INSERT [dbo].[tblDownloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (25, 34, 54, 54, 1, N'/Images/54/34/Attachment/Attachment1_04192021.pdf', 0, CAST(N'2021-03-23T15:21:23.503' AS DateTime), 1, CAST(22 AS Decimal(18, 0)), N'jfbd', N'CE', CAST(N'2021-03-23T15:21:23.503' AS DateTime), 54, CAST(N'2021-03-29T17:35:08.100' AS DateTime), 54)
INSERT [dbo].[tblDownloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (26, 39, 54, 55, 1, N'/Images/54/39/Attachment/Attachment1_06162021.pdf', 0, CAST(N'2021-03-23T16:42:26.727' AS DateTime), 1, CAST(456 AS Decimal(18, 0)), N'ufjgnfdkgmdl', N'CE', CAST(N'2021-03-23T16:42:26.727' AS DateTime), 55, NULL, 54)
INSERT [dbo].[tblDownloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (27, 31, 54, 55, 0, NULL, 0, CAST(N'2021-03-23T17:01:45.437' AS DateTime), 1, CAST(123 AS Decimal(18, 0)), N'hjsd', N'CE', CAST(N'2021-03-23T17:01:45.437' AS DateTime), 55, NULL, 55)
INSERT [dbo].[tblDownloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (28, 34, 54, 55, 1, N'/Images/54/34/Attachment/Attachment1_04192021.pdf', 0, CAST(N'2021-03-26T09:03:00.947' AS DateTime), 1, CAST(22 AS Decimal(18, 0)), N'jfbd', N'CE', CAST(N'2021-03-26T09:03:00.947' AS DateTime), 55, NULL, 54)
INSERT [dbo].[tblDownloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (30, 32, 54, 55, 0, NULL, 0, CAST(N'2021-03-26T10:01:19.350' AS DateTime), 1, CAST(123 AS Decimal(18, 0)), N'hjsd', N'CE', CAST(N'2021-03-26T10:01:19.350' AS DateTime), 55, NULL, 55)
INSERT [dbo].[tblDownloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (31, 45, 55, 54, 1, N'/Images/55/45/Attachment/Attachment1_15312021.pdf', 0, CAST(N'2021-03-26T10:25:49.670' AS DateTime), 1, CAST(1200 AS Decimal(18, 0)), N'DMBI', N'CE', CAST(N'2021-03-26T10:25:49.670' AS DateTime), 54, NULL, 55)
INSERT [dbo].[tblDownloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (32, 45, 55, 54, 1, N'/Images/55/45/Attachment/Attachment1_15312021.pdf', 0, CAST(N'2021-03-26T11:08:46.487' AS DateTime), 1, CAST(1200 AS Decimal(18, 0)), N'DMBI', N'CE', CAST(N'2021-03-26T11:08:46.490' AS DateTime), 54, NULL, 55)
INSERT [dbo].[tblDownloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (33, 46, 55, 54, 0, NULL, 0, CAST(N'2021-03-26T11:28:33.790' AS DateTime), 1, CAST(450 AS Decimal(18, 0)), N'Physics', N'CE', CAST(N'2021-03-26T11:28:33.790' AS DateTime), 54, NULL, 54)
INSERT [dbo].[tblDownloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (34, 40, 54, 54, 0, NULL, 0, CAST(N'2021-03-29T15:41:24.263' AS DateTime), 1, CAST(2344 AS Decimal(18, 0)), N'Science', N'CE', CAST(N'2021-03-29T15:41:24.263' AS DateTime), 54, NULL, 54)
INSERT [dbo].[tblDownloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (35, 33, 54, 55, 0, NULL, 0, CAST(N'2021-03-29T19:00:45.500' AS DateTime), 1, CAST(123 AS Decimal(18, 0)), N'hjsd', N'CE', CAST(N'2021-03-29T19:00:45.500' AS DateTime), 55, NULL, 55)
INSERT [dbo].[tblDownloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachmentDownloaded], [AttachmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (36, 50, 54, 64, 1, N'/Images/54/50/Attachment/Attachment1_16062021.pdf', 1, CAST(N'2021-04-09T11:23:12.673' AS DateTime), 0, CAST(0 AS Decimal(18, 0)), N'Sangharsh', N'MBA', CAST(N'2021-04-09T11:23:12.673' AS DateTime), 64, NULL, NULL)
SET IDENTITY_INSERT [dbo].[tblDownloads] OFF
GO
SET IDENTITY_INSERT [dbo].[tblNoteCategories] ON 

INSERT [dbo].[tblNoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'CE', N'Computer Engineering', CAST(N'2021-03-30T10:34:16.000' AS DateTime), 57, NULL, NULL, 1)
INSERT [dbo].[tblNoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'IT', N'Information Technology', CAST(N'2021-03-15T10:41:40.000' AS DateTime), 57, NULL, NULL, 1)
INSERT [dbo].[tblNoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, N'MCA', N'Master of Computer Application', CAST(N'2021-04-05T16:53:24.000' AS DateTime), 57, NULL, NULL, 1)
INSERT [dbo].[tblNoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4, N'MBA', N'Master of Business Administration Interesting', CAST(N'2021-04-06T17:48:09.000' AS DateTime), 57, CAST(N'2021-04-07T10:59:17.707' AS DateTime), 56, 1)
INSERT [dbo].[tblNoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (5, N'History', N'This is Interesting', CAST(N'2021-04-07T10:39:47.990' AS DateTime), 56, NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[tblNoteCategories] OFF
GO
SET IDENTITY_INSERT [dbo].[tblNoteTypes] ON 

INSERT [dbo].[tblNoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'Handwritten', N'This is Handwritten Book', CAST(N'2021-03-15T10:41:40.000' AS DateTime), 57, NULL, NULL, 1)
INSERT [dbo].[tblNoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'University Book', N'This is University Book', CAST(N'2021-04-05T16:53:24.000' AS DateTime), 57, NULL, NULL, 1)
INSERT [dbo].[tblNoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, N'StoryBook', N'This is a Story Book', CAST(N'2021-03-30T10:34:16.000' AS DateTime), 56, NULL, NULL, 1)
INSERT [dbo].[tblNoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4, N'History Book', N'Know History', CAST(N'2021-04-07T12:06:16.090' AS DateTime), 56, CAST(N'2021-04-07T12:06:45.703' AS DateTime), 56, 0)
SET IDENTITY_INSERT [dbo].[tblNoteTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[tblReferenceData] ON 

INSERT [dbo].[tblReferenceData] ([ID], [Value], [Datavalue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'Draft', N'Draft', N'Notes Status', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tblReferenceData] ([ID], [Value], [Datavalue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'Submitted For Review', N'Submitted For Review', N'Notes Status', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tblReferenceData] ([ID], [Value], [Datavalue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, N'In Review', N'In Review', N'Notes Status', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tblReferenceData] ([ID], [Value], [Datavalue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4, N'Published', N'Approved', N'Notes Status', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tblReferenceData] ([ID], [Value], [Datavalue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (5, N'Rejected', N'Rejected', N'Notes Status', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tblReferenceData] ([ID], [Value], [Datavalue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (6, N'Removed', N'Removed', N'Notes Status', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tblReferenceData] ([ID], [Value], [Datavalue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (7, N'Male', N'M', N'Gender', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tblReferenceData] ([ID], [Value], [Datavalue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (9, N'Female', N'F', N'Gender', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tblReferenceData] ([ID], [Value], [Datavalue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (10, N'Other', N'other', N'Gender', NULL, NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[tblReferenceData] OFF
GO
SET IDENTITY_INSERT [dbo].[tblSellerNotes] ON 

INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (6, 54, 1, NULL, NULL, NULL, N'a', 1, NULL, NULL, NULL, N'ghvhjbj', NULL, 1, NULL, NULL, NULL, 0, CAST(123 AS Decimal(18, 0)), NULL, CAST(N'2021-03-02T13:41:22.623' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (7, 54, 1, NULL, NULL, NULL, N'a', 1, NULL, NULL, NULL, N'hsbkjdfgnfd', NULL, 1, NULL, NULL, NULL, 0, CAST(123 AS Decimal(18, 0)), NULL, CAST(N'2021-03-02T13:45:48.897' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (8, 54, 1, NULL, NULL, NULL, N'a', 1, NULL, NULL, NULL, N'hsbkjdfgnfd', NULL, NULL, NULL, NULL, NULL, 0, CAST(123 AS Decimal(18, 0)), NULL, CAST(N'2021-03-02T13:46:47.880' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (9, 54, 1, NULL, NULL, NULL, N'a', 1, NULL, NULL, NULL, N'hsbkjdfgnfd', NULL, NULL, NULL, NULL, NULL, 0, CAST(123 AS Decimal(18, 0)), NULL, CAST(N'2021-03-02T13:49:01.153' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (10, 54, 1, NULL, NULL, NULL, N'hjsd', 1, NULL, NULL, NULL, N'ghvhjb', NULL, NULL, NULL, NULL, NULL, 1, CAST(123 AS Decimal(18, 0)), NULL, CAST(N'2021-03-02T18:07:41.993' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (11, 54, 1, NULL, NULL, NULL, N'a', 1, NULL, NULL, NULL, N'fchgvjh', NULL, NULL, NULL, NULL, NULL, 1, CAST(123 AS Decimal(18, 0)), NULL, CAST(N'2021-03-03T08:19:23.617' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (12, 54, 1, NULL, NULL, NULL, N'a', 1, NULL, NULL, NULL, N'vnb m', NULL, NULL, NULL, NULL, NULL, 1, CAST(123 AS Decimal(18, 0)), NULL, CAST(N'2021-03-03T08:31:29.347' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (13, 54, 1, NULL, NULL, NULL, N'hjsd', 1, NULL, NULL, NULL, N'jnfdkjg', NULL, NULL, NULL, NULL, NULL, 1, CAST(1323 AS Decimal(18, 0)), NULL, CAST(N'2021-03-03T09:44:49.650' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (14, 54, 1, NULL, NULL, NULL, N'hjsd', 1, NULL, NULL, NULL, N'hgfhjbdf', NULL, NULL, NULL, NULL, NULL, 1, CAST(1323 AS Decimal(18, 0)), NULL, CAST(N'2021-03-03T10:02:27.793' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (15, 54, 1, NULL, NULL, NULL, N'hjsd', 1, NULL, NULL, NULL, N'dfxghvjv', NULL, NULL, NULL, NULL, NULL, 1, CAST(123 AS Decimal(18, 0)), NULL, CAST(N'2021-03-03T10:14:27.267' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (16, 54, 1, NULL, NULL, NULL, N'hjsd', 1, NULL, NULL, NULL, N'dfxghvjv', NULL, NULL, NULL, NULL, NULL, 1, CAST(123 AS Decimal(18, 0)), NULL, CAST(N'2021-03-03T10:16:21.133' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (17, 54, 1, NULL, NULL, NULL, N'yuhufbjds', 1, NULL, NULL, NULL, N'erjgntrkjhntk', NULL, NULL, NULL, NULL, NULL, 1, CAST(123 AS Decimal(18, 0)), NULL, CAST(N'2021-03-03T10:29:52.470' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (18, 54, 1, NULL, NULL, NULL, N'hjsd', 1, N'\Images\Screenshot (3)03472021.png', 2, NULL, N'jfgnkjfdng', NULL, NULL, NULL, NULL, NULL, 1, CAST(123 AS Decimal(18, 0)), NULL, CAST(N'2021-03-03T10:47:04.347' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (19, 54, 3, NULL, NULL, NULL, N'hjsd', 1, N'D:\.Net\Practise\Practise\Images\Screenshot (28)03022021.png', 3, 67, N'hdfjgdfk', NULL, 1, NULL, NULL, NULL, 1, CAST(123 AS Decimal(18, 0)), NULL, CAST(N'2021-03-03T11:02:36.223' AS DateTime), NULL, CAST(N'2021-03-14T12:42:22.937' AS DateTime), NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (20, 54, 1, NULL, NULL, NULL, N'hjsd', 1, N'D:\.Net\Practise\Practise\Images\54\20\Screenshot (6)03092021.png', 1, NULL, N'hjrbgjkdf', NULL, NULL, NULL, NULL, NULL, 1, CAST(123 AS Decimal(18, 0)), NULL, CAST(N'2021-03-03T11:09:28.117' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (21, 54, 5, 56, N'It nee some deatails', NULL, N'hjsd', 1, NULL, 2, NULL, N'dbhfjg', NULL, NULL, NULL, NULL, NULL, 1, CAST(123 AS Decimal(18, 0)), NULL, CAST(N'2021-03-03T11:16:19.373' AS DateTime), NULL, CAST(N'2021-04-02T16:24:19.227' AS DateTime), NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (23, 54, 2, NULL, NULL, NULL, N'hjsd', 1, N'D:\.Net\Practise\Practise\Images\54\23\Dimpalreceipt03092021.pdf', 1, NULL, N'gvvjhb', NULL, NULL, NULL, NULL, NULL, 1, CAST(123 AS Decimal(18, 0)), NULL, CAST(N'2021-03-03T12:09:23.717' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (24, 54, 1, NULL, NULL, NULL, N'hjsd', 1, N'D:\.Net\Practise\Practise\Images\54\24\Dimpal Gate03012021.pdf', 3, NULL, N'dxgfvhg', NULL, NULL, NULL, NULL, NULL, 1, CAST(123 AS Decimal(18, 0)), NULL, CAST(N'2021-03-03T16:00:13.337' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (25, 54, 1, NULL, NULL, NULL, N'hjsd', 1, NULL, NULL, NULL, N'ghvnb m', NULL, NULL, NULL, NULL, NULL, 1, CAST(123 AS Decimal(18, 0)), NULL, CAST(N'2021-03-03T18:16:28.260' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (26, 54, 1, NULL, NULL, NULL, N'fvbfdkj', 1, N'D:\.Net\Practise\Practise\Images\54\26\Screenshot (6)04582021.png', NULL, 5, N'dbfgjfdkgnmk', N'gyfdjgbfjkd', 1, N'yegfeus', N'123', N'fdfgdghjm', 1, CAST(123 AS Decimal(18, 0)), NULL, CAST(N'2021-03-04T09:56:32.307' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (28, 54, 4, 56, NULL, CAST(N'2021-03-04T16:11:27.000' AS DateTime), N'hjsd', 4, N'\Images\54\28\Screenshot (7)04272021.png', NULL, 5, N'ydshbfjd', N'gyfdjgbfjkd', 1, N'yegfeus', N'12', N'fdfgdghjm', 1, CAST(123 AS Decimal(18, 0)), NULL, CAST(N'2021-03-04T10:27:20.990' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (29, 54, 4, 56, NULL, CAST(N'2021-03-14T11:18:51.000' AS DateTime), N'bdhbfjd', 4, N'\Images\54\29\Screenshot (1)04362021.png', NULL, 5, N'fgcghvhjbj', N'gyfdjgbfjkd', 9, N'yegfeus', N'4', N'fgfdgf', 1, CAST(123 AS Decimal(18, 0)), N'\Images\54\37\Dimpalreceipt04002021.pdf', CAST(N'2021-03-04T10:36:07.543' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (30, 54, 4, 56, NULL, CAST(N'2021-03-15T15:36:15.000' AS DateTime), N'jfbd', 1, N'\Images\54\30\Screenshot (15)04222021.png', 3, 3, N'dghhbjhj', N'gyfdjgbfjkd', 8, N'v bn mn m', N'43', N'fgfdgf', 0, NULL, N'\Images\54\37\Dimpalreceipt04002021.pdf', CAST(N'2021-03-04T11:21:55.890' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (31, 54, 4, 56, NULL, CAST(N'2021-03-16T12:00:04.000' AS DateTime), N'hjsd', 1, N'\Images\54\31\Screenshot (15)04032021.png', 1, 5, N'hbgjdfngkfdj', N'gyfdjgbfjkd', 1, N'v bn mn m', N'12', N'fgfdgf', 1, CAST(123 AS Decimal(18, 0)), N'\Images\54\37\Dimpalreceipt04002021.pdf', CAST(N'2021-03-04T16:03:43.107' AS DateTime), NULL, CAST(N'2021-03-14T12:40:44.720' AS DateTime), NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (32, 54, 4, 56, NULL, CAST(N'2021-03-15T15:36:15.000' AS DateTime), N'hjsd', 1, N'/DefaultImage/DN_.jpg', 2, 5, N'hbgjdfngkfdj', N'gyfdjgbfjkd', 1, N'v bn mn m', N'12', N'fgfdgf', 1, CAST(123 AS Decimal(18, 0)), N'\Images\54\37\Dimpalreceipt04002021.pdf', CAST(N'2021-03-04T16:06:49.710' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (33, 54, 4, 56, NULL, CAST(N'2021-03-16T12:00:04.000' AS DateTime), N'hjsd', 1, N'\Images\54\33\Screenshot (14)04112021.png', 3, 3, N'tjhfghnghjn', N'gyfdjgbfjkd', 1, N'yegfeus', N'12', N'fgfdgf', 1, CAST(123 AS Decimal(18, 0)), N'\Images\54\40\Dimpal Gate06392021.pdf', CAST(N'2021-03-04T16:11:27.147' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (34, 54, 4, 56, NULL, CAST(N'2021-03-04T16:11:27.000' AS DateTime), N'jfbd', 1, N'/DefaultImage/DN_.jpg', 2, 5, N'dgfjghjmhg', N'gyfdjgbfjkd', 1, N'yegfeus', N'4', N'fdfgdghjm', 1, CAST(22 AS Decimal(18, 0)), N'\Images\54\40\Dimpal Gate06392021.pdf', CAST(N'2021-03-04T16:18:48.077' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (35, 54, 4, 56, NULL, CAST(N'2021-03-16T12:00:04.000' AS DateTime), N'a', 1, N'/DefaultImage/DN_.jpg', 1, 5, N'sdgfhg', N'gyfdjgbfjkd', 8, N'yegfeus', N'12', N'fgfdgf', 1, CAST(1323 AS Decimal(18, 0)), N'\Images\54\40\Dimpal Gate06392021.pdf', NULL, NULL, CAST(N'2021-03-14T11:18:51.917' AS DateTime), NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (36, 54, 4, 56, NULL, CAST(N'2021-03-14T11:18:51.000' AS DateTime), N'jfbd', 1, N'/DefaultImage/DN_.jpg', NULL, 5, N'hfbjdkngdkf', N'gyfdjgbfjkd', 8, N'yegfeus', N'43', N'fgfdgf', 1, CAST(67 AS Decimal(18, 0)), N'\Images\54\40\Dimpal Gate06392021.pdf', CAST(N'2021-03-04T16:40:16.523' AS DateTime), NULL, CAST(N'2021-03-14T11:22:38.287' AS DateTime), NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (37, 54, 4, 56, NULL, CAST(N'2021-03-31T19:06:44.310' AS DateTime), N'hjsd', 1, N'\Images\54\37\Screenshot (2)04592021.png', NULL, 5, N'jksgbfdkjgnfd', N'gyfdjgbfjkd', 1, N'yegfeus', N'4', N'fgfdgf', 1, CAST(1323 AS Decimal(18, 0)), N'\Images\54\37\Dimpalreceipt04002021.pdf', CAST(N'2021-03-04T16:59:18.523' AS DateTime), NULL, CAST(N'2021-03-31T19:04:45.343' AS DateTime), NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (38, 54, 1, NULL, NULL, NULL, N'yuhufbjds', 1, N'/DefaultImage/DN_.jpg', 2, 3, N'gyaefshbf', N'gyfdjgbfjkd', 1, N'yegfeus', N'43', N'fgfdgf', 1, CAST(67 AS Decimal(18, 0)), N'\Images\54\40\Dimpal Gate06392021.pdf', CAST(N'2021-03-04T17:06:20.457' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (39, 54, 4, 56, NULL, CAST(N'2021-03-15T15:36:15.000' AS DateTime), N'ufjgnfdkgmdl', 1, N'/DefaultImage/DN_.jpg', 1, 5, N'hugkdfjnhlkmghlkfbvhjxb', N'Kharel', 1, NULL, NULL, NULL, 1, CAST(456 AS Decimal(18, 0)), N'\Images\54\42\Dimpal Gate10272021.pdf', CAST(N'2021-03-04T17:06:20.000' AS DateTime), NULL, CAST(N'2021-03-14T11:09:37.057' AS DateTime), NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (40, 54, 4, 56, NULL, CAST(N'2021-03-16T12:00:04.000' AS DateTime), N'Science', 1, N'/DefaultImage/DN_.jpg', 3, 3, N'hewbfkfdjhnflk', NULL, NULL, N'yegfeus', NULL, N'Keyur', 1, CAST(2344 AS Decimal(18, 0)), N'\Images\54\40\Dimpal Gate06392021.pdf', CAST(N'2021-03-04T17:06:20.000' AS DateTime), NULL, CAST(N'2021-03-14T11:05:26.540' AS DateTime), NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (41, 54, 4, 56, NULL, CAST(N'2021-03-16T12:00:04.000' AS DateTime), N'dgfjdngfh', 1, N'/DefaultImage/DN_.jpg', 2, NULL, N'sbgkjhnnjkhg', NULL, NULL, NULL, NULL, NULL, 1, CAST(234 AS Decimal(18, 0)), N'\Images\54\42\Dimpal Gate10272021.pdf', CAST(N'2021-03-06T18:12:10.823' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (42, 54, 4, 56, NULL, CAST(N'2021-03-15T15:36:15.000' AS DateTime), N'Science', 1, N'/DefaultImage/DN_.jpg', 2, NULL, N'hgfvhjnb,m', NULL, NULL, NULL, NULL, NULL, 1, CAST(450 AS Decimal(18, 0)), N'\Images\54\42\Dimpal Gate10272021.pdf', CAST(N'2021-03-10T12:27:14.820' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (43, 54, 4, 56, N'It need changes', CAST(N'2021-04-04T18:59:10.767' AS DateTime), N'Science', 1, N'D:/.Net/Practise/Practise/DefaultImage/4.jpg', NULL, NULL, N'hgfvhjnb,m', NULL, NULL, NULL, NULL, NULL, 1, CAST(450 AS Decimal(18, 0)), N'\Images\54\42\Dimpal Gate10272021.pdf', CAST(N'2021-03-10T12:27:14.820' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (44, 54, 2, NULL, NULL, NULL, N'dgfjdngfh', 1, NULL, NULL, NULL, N'sbgkjhnnjkhg', NULL, NULL, NULL, NULL, NULL, 1, CAST(234 AS Decimal(18, 0)), NULL, CAST(N'2021-03-06T18:12:10.823' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (45, 55, 4, 56, NULL, CAST(N'2021-03-14T11:18:51.000' AS DateTime), N'DMBI', 1, N'\Images\55\45\Screenshot (15)15312021.png', 3, 120, N'this is important ', N'Kharel', 1, N'Computer', N'23', N'Sanjay', 1, CAST(1200 AS Decimal(18, 0)), N'\Images\55\45\Dimpalreceipt15312021.pdf', CAST(N'2021-03-15T15:31:21.707' AS DateTime), NULL, CAST(N'2021-04-05T12:03:42.137' AS DateTime), 56, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (46, 55, 4, 56, NULL, CAST(N'2021-03-14T11:18:51.000' AS DateTime), N'Physics', 1, N'\Images\55\46\Screenshot (14)15342021.png', 2, 500, N'hjdsfbdjkhntuihyjr uhgituhjtihjg', N'GEC Rajkot', 8, N'hjbsdgiduhnfk', N'124', N'Swati', 1, CAST(450 AS Decimal(18, 0)), N'\Images\55\46\Dimpal Gate15342021.pdf', CAST(N'2021-03-15T15:34:07.480' AS DateTime), NULL, CAST(N'2021-04-05T12:03:43.170' AS DateTime), 56, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (47, 55, 1, 56, N'Work on that', NULL, N'Science', 4, N'D:/.Net/Practise/Practise/DefaultImage/4.jpg', NULL, 120, N'jbhasvfurgn jfhgitjhoytij ihjihjmytl', N'Kharel', 9, N'Computer', N'23', N'Keyur', 1, CAST(490 AS Decimal(18, 0)), N'\Images\55\47\Dimpalreceipt15362021.pdf', CAST(N'2021-03-15T15:36:15.743' AS DateTime), NULL, CAST(N'2021-04-02T16:25:02.207' AS DateTime), NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (48, 54, 4, 56, NULL, CAST(N'2021-03-31T19:06:44.000' AS DateTime), N'Maths', 1, N'/DefaultImage/DN_.jpg', NULL, 400, N'bk jnvdkfgnl kjnglkjmhg', N'GEC Rajkot', 8, N'Maths', N'43', N'Keyur', 0, NULL, N'\Images\55\46\Dimpal Gate15342021.pdf', CAST(N'2021-03-16T11:43:18.677' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (49, 54, 6, 56, N'Inappropriate', CAST(N'2021-03-04T16:11:27.000' AS DateTime), N'CPU', 1, N'/DefaultImage/DN_.jpg', NULL, NULL, N'gujfdngdjkfnbcg jbsfdjkngfkngf', N'Gec bhavnagar', 8, N'Computer', NULL, NULL, 0, NULL, N'\Images\55\46\Dimpal Gate15342021.pdf', CAST(N'2021-03-16T12:00:04.793' AS DateTime), NULL, CAST(N'2021-04-05T10:19:41.203' AS DateTime), NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (50, 54, 4, 56, N'', CAST(N'2021-03-04T16:11:27.000' AS DateTime), N'Sangharsh', 4, N'/DefaultImage/DN_.jpg', NULL, 120, N'jsngidfjhn kjnhglhk', NULL, NULL, NULL, NULL, NULL, 0, CAST(0 AS Decimal(18, 0)), N'\Images\55\47\Dimpalreceipt15362021.pdf', CAST(N'2021-03-16T12:06:13.507' AS DateTime), NULL, CAST(N'2021-04-05T10:27:09.730' AS DateTime), NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (51, 54, 1, NULL, NULL, NULL, N'Science', 3, N'D:/.Net/Practise/Practise/DefaultImage/4.jpg', 1, 67, N'jfnvmfnb g,c', NULL, 8, NULL, NULL, NULL, 0, CAST(0 AS Decimal(18, 0)), N'\Images\55\45\Dimpalreceipt15312021.pdf', CAST(N'2021-03-16T15:38:14.137' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (52, 54, 1, NULL, NULL, NULL, N'Sangath', 3, NULL, 2, NULL, N'hbgjdnhfkh', NULL, NULL, NULL, NULL, NULL, 0, CAST(0 AS Decimal(18, 0)), N'\Images\55\45\Dimpalreceipt15312021.pdf', CAST(N'2021-03-17T17:49:26.270' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (53, 54, 1, NULL, NULL, NULL, N'Science', 3, N'/Images/54/53/Screenshot (2)17542021.png', NULL, NULL, N'fyubkj jbkjnk iuhikn', NULL, NULL, NULL, NULL, NULL, 0, CAST(0 AS Decimal(18, 0)), N'\Images\55\45\Dimpalreceipt15312021.pdf', CAST(N'2021-03-17T17:54:13.433' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (54, 54, 2, NULL, NULL, NULL, N'Jagat', 4, N'/Images/54/54/18462021.png', NULL, 450, N'jsdhbfjd jrghtihjoify uhgrihjklhm jrghtijulky jugntfkhmlkxadxe', N'GEC Rajkot', 10, N'Maths', N'43', N'Sanjay', 0, CAST(0 AS Decimal(18, 0)), N'\Images\55\45\Dimpalreceipt15312021.pdf', CAST(N'2021-03-18T15:46:36.987' AS DateTime), NULL, CAST(N'2021-03-31T19:04:06.553' AS DateTime), NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (55, 54, 4, 56, NULL, CAST(N'2021-03-31T19:06:44.000' AS DateTime), N'Tarang', 3, N'/Images/54/55/18542021.png', 2, 67, N'hjbddfhg jthi cytsbdfjdf uhgkhmgfstcgsf hghjgbdfk', N'Kharel', 10, N'hjbsdgiduhnfk', N'4', N'Sanjay', 0, CAST(0 AS Decimal(18, 0)), N'\Images\55\46\Dimpal Gate15342021.pdf', CAST(N'2021-03-18T15:54:43.297' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (61, 55, 5, 56, N'Check  content properly', NULL, N'Account', 3, N'/Images/55/61/02222021.png', NULL, 90, N'hdsh kjncf kjnh gvdashd jbvfj', N'GEC Rajkot', 10, N'Maths', N'123', N'Swati', 1, CAST(300 AS Decimal(18, 0)), N'/Images/55/61/02222021.pdf', CAST(N'2021-04-02T16:22:50.883' AS DateTime), NULL, CAST(N'2021-04-02T16:25:52.597' AS DateTime), NULL, 1)
INSERT [dbo].[tblSellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (62, 55, 2, NULL, NULL, NULL, N'Chemistry', 2, N'/Images/55/62/08092021.jpg', NULL, 500, N'This is hard to learn', N'VGEC', 10, N'Chemical', N'20', N'Komal', 1, CAST(600 AS Decimal(18, 0)), N'/Images/55/62/08092021.pdf', CAST(N'2021-04-08T18:09:06.417' AS DateTime), NULL, CAST(N'2021-04-08T18:09:49.150' AS DateTime), NULL, 1)
SET IDENTITY_INSERT [dbo].[tblSellerNotes] OFF
GO
SET IDENTITY_INSERT [dbo].[tblSellerNotesAttachments] ON 

INSERT [dbo].[tblSellerNotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, 28, N'D:\.Net\Practise\Practise\Images\54\28\Attachment\Attachment1_04272021.pdf;', N'D:\.Net\Practise\Practise\Images\54\28\Attachment\Attachment1_04272021.pdf', CAST(N'2021-03-04T10:27:48.077' AS DateTime), 54, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, 29, N'Attachment1_04362021.pdf;', N'/Images/54/29/Attachment/Attachment1_04362021.pdf', CAST(N'2021-03-04T10:36:14.617' AS DateTime), 54, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4, 30, N'Attachment1_04222021.pdf;', N'/Images/54/30/Attachment/Attachment1_04222021.pdf', CAST(N'2021-03-04T11:22:02.333' AS DateTime), 54, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (5, 31, N'Attachment1_04042021.pdf;', N'/Images/54/31/Attachment/Attachment1_04042021.pdf', CAST(N'2021-03-04T16:03:59.077' AS DateTime), 54, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (6, 33, N'Attachment1_04122021.pdf;', N'/Images/54/33/Attachment/Attachment1_04122021.pdf', CAST(N'2021-03-04T16:12:07.653' AS DateTime), 54, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (7, 34, N'Attachment1_04192021.pdf;', N'/Images/54/34/Attachment/Attachment1_04192021.pdf', CAST(N'2021-03-04T16:19:10.087' AS DateTime), 54, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (8, 35, N'Attachment1_04262021.pdf;', N'/Images/54/35/Attachment/Attachment1_04262021.pdf', CAST(N'2021-03-04T16:25:34.883' AS DateTime), 54, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (9, 36, N'Attachment1_04432021.pdf;', N'/Images/54/36/Attachment/Attachment1_04432021.pdf', CAST(N'2021-03-04T16:42:50.357' AS DateTime), 54, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (10, 37, N'Attachment1_04002021.pdf;', N'/Images/54/37/Attachment/Attachment1_04002021.pdf', CAST(N'2021-03-04T16:59:53.273' AS DateTime), 54, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (11, 38, N'Attachment1_04062021.pdf;', N'/Images/54/38/Attachment/Attachment1_04062021.pdf', CAST(N'2021-03-04T17:06:26.233' AS DateTime), 54, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (12, 39, N'Attachment1_06162021.pdf;', N'/Images/54/39/Attachment/Attachment1_06162021.pdf', CAST(N'2021-03-06T15:16:20.560' AS DateTime), 54, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (13, 40, N'Attachment1_06392021.pdf;', N'/Images/54/40/Attachment/Attachment1_06392021.pdf', CAST(N'2021-03-06T15:38:36.007' AS DateTime), 54, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (14, 42, N'Attachment1_10272021.pdf;', N'/Images/54/42/Attachment/Attachment1_10272021.pdf', CAST(N'2021-03-10T12:27:31.773' AS DateTime), 54, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (15, 45, N'Attachment1_15312021.pdf;', N'/Images/55/45/Attachment/Attachment1_15312021.pdf', CAST(N'2021-03-15T15:31:21.837' AS DateTime), 55, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (16, 46, N'Attachment1_15342021.pdf;', N'/Images/55/46/Attachment/Attachment1_15342021.pdf', CAST(N'2021-03-15T15:34:07.523' AS DateTime), 55, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (17, 47, N'Attachment1_15362021.pdf;', N'/Images/55/47/Attachment/Attachment1_15362021.pdf', CAST(N'2021-03-15T15:36:15.777' AS DateTime), 55, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (18, 48, N'Attachment1_16432021.pdf;', N'/Images/54/48/Attachment/Attachment1_16432021.pdf', CAST(N'2021-03-16T11:43:18.757' AS DateTime), 54, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (19, 49, N'Attachment1_16002021.pdf;', N'/Images/54/49/Attachment/Attachment1_16002021.pdf', CAST(N'2021-03-16T12:00:04.837' AS DateTime), 54, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (20, 50, N'Attachment1_16062021.pdf;', N'/Images/54/50/Attachment/Attachment1_16062021.pdf', CAST(N'2021-03-16T12:06:13.897' AS DateTime), 54, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (21, 51, N'Attachment1_16382021.pdf;', N'/Images/54/51/Attachment/Attachment1_16382021.pdf', CAST(N'2021-03-16T15:38:23.127' AS DateTime), 54, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (22, 53, N'Attachment1_17012021.pdf;', N'/Images/54/53/Attachment/Attachment1_17012021.pdf', CAST(N'2021-03-17T18:01:53.163' AS DateTime), 54, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (23, 54, N'Attachment1_18482021.pdf;', N'/Images/54/54/Attachment/Attachment1_18482021.pdf', CAST(N'2021-03-18T15:48:10.603' AS DateTime), 54, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (24, 55, N'Attachment1_18552021.pdf;', N'/Images/54/55/Attachment/Attachment1_18552021.pdf', CAST(N'2021-03-18T15:55:01.783' AS DateTime), 54, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (26, 61, N'Attachment1_02222021.pdf;', N'/Images/55/61/Attachment/Attachment1_02222021.pdf', CAST(N'2021-04-02T16:22:51.130' AS DateTime), 55, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotesAttachments] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (27, 62, N'Attachment1_08092021.pdf;', N'/Images/55/62/Attachment/Attachment1_08092021.pdf', CAST(N'2021-04-08T18:09:06.737' AS DateTime), 55, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[tblSellerNotesAttachments] OFF
GO
SET IDENTITY_INSERT [dbo].[tblSellerNotesReportedIssues] ON 

INSERT [dbo].[tblSellerNotesReportedIssues] ([ID], [NoteID], [ReportedByID], [AgainstDownloadID], [Remarks], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1, 34, 54, 25, N'It need changes', CAST(N'2021-04-07T16:27:03.430' AS DateTime), 54, CAST(N'2021-04-07T16:29:45.930' AS DateTime), 54)
INSERT [dbo].[tblSellerNotesReportedIssues] ([ID], [NoteID], [ReportedByID], [AgainstDownloadID], [Remarks], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2, 31, 55, 20, N'Not good', CAST(N'2021-04-07T17:26:00.103' AS DateTime), 55, CAST(N'2021-04-08T17:45:32.083' AS DateTime), 55)
SET IDENTITY_INSERT [dbo].[tblSellerNotesReportedIssues] OFF
GO
SET IDENTITY_INSERT [dbo].[tblSellerNotesReviews] ON 

INSERT [dbo].[tblSellerNotesReviews] ([ID], [NoteID], [ReviewedByID], [AgainstDownloadsID], [Ratings], [Comments], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4, 34, 54, 25, CAST(5 AS Decimal(18, 0)), N'Very Good', CAST(N'2021-03-25T19:35:20.300' AS DateTime), NULL, CAST(N'2021-03-25T20:12:37.737' AS DateTime), 54, 1)
INSERT [dbo].[tblSellerNotesReviews] ([ID], [NoteID], [ReviewedByID], [AgainstDownloadsID], [Ratings], [Comments], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (7, 34, 55, 25, CAST(3 AS Decimal(18, 0)), N'good', CAST(N'2021-03-26T09:30:46.130' AS DateTime), 54, NULL, NULL, 1)
INSERT [dbo].[tblSellerNotesReviews] ([ID], [NoteID], [ReviewedByID], [AgainstDownloadsID], [Ratings], [Comments], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (8, 45, 54, 3, CAST(2 AS Decimal(18, 0)), N'Work on that', CAST(N'2021-03-27T13:41:24.920' AS DateTime), 55, CAST(N'2021-04-05T17:45:20.537' AS DateTime), 54, 1)
SET IDENTITY_INSERT [dbo].[tblSellerNotesReviews] OFF
GO
SET IDENTITY_INSERT [dbo].[tblSystemConfigurations] ON 

INSERT [dbo].[tblSystemConfigurations] ([ID], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (61, N'Support email address', N'dnlad22@gmail.com', CAST(N'2021-04-08T16:52:00.637' AS DateTime), 57, CAST(N'2021-04-08T16:52:00.637' AS DateTime), NULL, 1)
INSERT [dbo].[tblSystemConfigurations] ([ID], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (62, N'Password', N'jkqobpmshhmlgumw', CAST(N'2021-04-08T16:52:01.137' AS DateTime), 57, CAST(N'2021-04-08T16:52:01.137' AS DateTime), NULL, 1)
INSERT [dbo].[tblSystemConfigurations] ([ID], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (63, N'Support phone number', N'9974178984', CAST(N'2021-04-08T16:52:01.147' AS DateTime), 57, CAST(N'2021-04-08T16:52:01.147' AS DateTime), NULL, 1)
INSERT [dbo].[tblSystemConfigurations] ([ID], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (64, N'Email Address(es)', N'ladp7201@gmail.com', CAST(N'2021-04-08T16:52:01.157' AS DateTime), 57, CAST(N'2021-04-08T16:52:01.157' AS DateTime), NULL, 1)
INSERT [dbo].[tblSystemConfigurations] ([ID], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (65, N'Facebook URL', N'www.facebook.com', CAST(N'2021-04-08T16:52:01.163' AS DateTime), 57, CAST(N'2021-04-08T16:52:01.163' AS DateTime), NULL, 1)
INSERT [dbo].[tblSystemConfigurations] ([ID], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (66, N'Twitter URL', N'www.twitter.com', CAST(N'2021-04-08T16:52:01.173' AS DateTime), 57, CAST(N'2021-04-08T16:52:01.173' AS DateTime), NULL, 1)
INSERT [dbo].[tblSystemConfigurations] ([ID], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (67, N'Linkedin URL', N'www.linkedin.com', CAST(N'2021-04-08T16:52:01.207' AS DateTime), 57, CAST(N'2021-04-08T16:52:01.207' AS DateTime), NULL, 1)
INSERT [dbo].[tblSystemConfigurations] ([ID], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (68, N'Default image for notes', N'/DefaultImage/DN_.jpg', CAST(N'2021-04-08T16:52:01.217' AS DateTime), 57, CAST(N'2021-04-08T16:52:01.217' AS DateTime), NULL, 1)
INSERT [dbo].[tblSystemConfigurations] ([ID], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (69, N'Default profile picture', N'/DefaultImage/DP_.jpg', CAST(N'2021-04-08T16:52:03.133' AS DateTime), 57, CAST(N'2021-04-08T16:52:03.133' AS DateTime), NULL, 1)
SET IDENTITY_INSERT [dbo].[tblSystemConfigurations] OFF
GO
SET IDENTITY_INSERT [dbo].[tblUserProfile] ON 

INSERT [dbo].[tblUserProfile] ([ID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [CountryCode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (5, 54, CAST(N'2000-01-22T00:00:00.000' AS DateTime), 9, NULL, N'1', N'9974178984', N'\Images\54\Screenshot (2)14412021.png', N'Endhal', N'Navsari', N'Navsari', N'Gujarat', N'478', N'1', N'GTU', N'GEC Bhavnagar', CAST(N'2021-03-14T18:41:13.433' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[tblUserProfile] ([ID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [CountryCode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (6, 55, CAST(N'2007-12-07T00:00:00.000' AS DateTime), 7, NULL, N'1', N'9974178984', N'/DefaultImage/DP_.jpg', N'Endhal', N'Navsari', N'Navsari', N'Gujarat', N'478', N'1', N'GTU', N'GEC Bhavnagar', CAST(N'2021-03-15T10:47:40.757' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[tblUserProfile] ([ID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [CountryCode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (7, 56, NULL, NULL, N'abc@gmail.com', N'1', N'8487059832', N'/DefaultImage/DP_.jpg', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2021-04-04T14:05:47.303' AS DateTime), 56, CAST(N'2021-04-04T14:34:59.743' AS DateTime), 56)
INSERT [dbo].[tblUserProfile] ([ID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [CountryCode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (9, 60, NULL, NULL, NULL, N'1', N'9879294644', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2021-04-06T17:48:13.047' AS DateTime), 57, CAST(N'2021-04-06T18:38:41.880' AS DateTime), 57)
SET IDENTITY_INSERT [dbo].[tblUserProfile] OFF
GO
SET IDENTITY_INSERT [dbo].[tblUserRoles] ON 

INSERT [dbo].[tblUserRoles] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'Member', N'Normal User', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tblUserRoles] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (2, N'Admin', N'Admin User', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tblUserRoles] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, N'SuperAdmin', N'SuperAdmin User', NULL, NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[tblUserRoles] OFF
GO
SET IDENTITY_INSERT [dbo].[tblUsers] ON 

INSERT [dbo].[tblUsers] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVarified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (54, 1, N'dimpal', N'lad', N'dimpallad22@gmail.com', N'P5zKu1CZIXUMjLJza8b6bIPnW1eJeQ5jPvCfB3bQumc=', 1, CAST(N'2021-02-27T11:44:10.853' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblUsers] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVarified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (55, 1, N'rishi', N'lad', N'ladrishi07@gmail.com', N'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', 1, CAST(N'2021-03-15T10:41:40.670' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblUsers] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVarified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (56, 2, N'Padmaben', N'Lad', N'ladp7201@gmail.com', N'VF/CcHF+oCWLsbMzDPdiIancizRhalNS1xCO1VE/v58=', 1, CAST(N'2021-03-30T10:34:16.483' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblUsers] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVarified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (57, 3, N'Narendra', N'Lad', N'nlad2510@gmail.com', N'ilhG30ROjwqBfx1LIdDQLwbgTMT889/gyF8KsdIndyA=', 1, CAST(N'2021-04-05T16:53:24.853' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[tblUsers] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVarified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (60, 2, N'Pranav', N'Lad', N'kikanibrijal059@gmail.com', N'YEXV63xPyPq3PXScIb2ggWpO/nSn5cPy4yBvaJ9yoEM=', 1, CAST(N'2021-04-06T17:48:09.563' AS DateTime), 57, CAST(N'2021-04-06T18:38:41.880' AS DateTime), 57, 0)
INSERT [dbo].[tblUsers] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVarified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (64, 1, N'Pranav', N'Lad', N'pranavlad200731@gmail.com', N'hR0xulOWwMDYheTNHWpYR+4w3CQduxdboTL72NHncio=', 1, CAST(N'2021-04-08T19:10:15.677' AS DateTime), NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[tblUsers] OFF
GO
ALTER TABLE [dbo].[tblCountries]  WITH CHECK ADD  CONSTRAINT [FK_tblCountries_tblUsers] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[tblUsers] ([ID])
GO
ALTER TABLE [dbo].[tblCountries] CHECK CONSTRAINT [FK_tblCountries_tblUsers]
GO
ALTER TABLE [dbo].[tblDownloads]  WITH CHECK ADD  CONSTRAINT [FK_tblDownloads_tblSellerNotes] FOREIGN KEY([NoteID])
REFERENCES [dbo].[tblSellerNotes] ([ID])
GO
ALTER TABLE [dbo].[tblDownloads] CHECK CONSTRAINT [FK_tblDownloads_tblSellerNotes]
GO
ALTER TABLE [dbo].[tblDownloads]  WITH CHECK ADD  CONSTRAINT [FK_tblDownloads_tblUsers] FOREIGN KEY([Seller])
REFERENCES [dbo].[tblUsers] ([ID])
GO
ALTER TABLE [dbo].[tblDownloads] CHECK CONSTRAINT [FK_tblDownloads_tblUsers]
GO
ALTER TABLE [dbo].[tblDownloads]  WITH CHECK ADD  CONSTRAINT [FK_tblDownloads_tblUsers1] FOREIGN KEY([Downloader])
REFERENCES [dbo].[tblUsers] ([ID])
GO
ALTER TABLE [dbo].[tblDownloads] CHECK CONSTRAINT [FK_tblDownloads_tblUsers1]
GO
ALTER TABLE [dbo].[tblNoteCategories]  WITH CHECK ADD  CONSTRAINT [FK_tblNoteCategories_tblUsers] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[tblUsers] ([ID])
GO
ALTER TABLE [dbo].[tblNoteCategories] CHECK CONSTRAINT [FK_tblNoteCategories_tblUsers]
GO
ALTER TABLE [dbo].[tblNoteTypes]  WITH CHECK ADD  CONSTRAINT [FK_tblNoteTypes_tblNoteTypes] FOREIGN KEY([ID])
REFERENCES [dbo].[tblNoteTypes] ([ID])
GO
ALTER TABLE [dbo].[tblNoteTypes] CHECK CONSTRAINT [FK_tblNoteTypes_tblNoteTypes]
GO
ALTER TABLE [dbo].[tblNoteTypes]  WITH CHECK ADD  CONSTRAINT [FK_tblNoteTypes_tblUsers] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[tblUsers] ([ID])
GO
ALTER TABLE [dbo].[tblNoteTypes] CHECK CONSTRAINT [FK_tblNoteTypes_tblUsers]
GO
ALTER TABLE [dbo].[tblSellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_tblSellerNotes_tblCountries] FOREIGN KEY([Country])
REFERENCES [dbo].[tblCountries] ([ID])
GO
ALTER TABLE [dbo].[tblSellerNotes] CHECK CONSTRAINT [FK_tblSellerNotes_tblCountries]
GO
ALTER TABLE [dbo].[tblSellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_tblSellerNotes_tblNoteCategories] FOREIGN KEY([Category])
REFERENCES [dbo].[tblNoteCategories] ([ID])
GO
ALTER TABLE [dbo].[tblSellerNotes] CHECK CONSTRAINT [FK_tblSellerNotes_tblNoteCategories]
GO
ALTER TABLE [dbo].[tblSellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_tblSellerNotes_tblNoteTypes] FOREIGN KEY([NoteType])
REFERENCES [dbo].[tblNoteTypes] ([ID])
GO
ALTER TABLE [dbo].[tblSellerNotes] CHECK CONSTRAINT [FK_tblSellerNotes_tblNoteTypes]
GO
ALTER TABLE [dbo].[tblSellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_tblSellerNotes_tblReferenceData] FOREIGN KEY([Status])
REFERENCES [dbo].[tblReferenceData] ([ID])
GO
ALTER TABLE [dbo].[tblSellerNotes] CHECK CONSTRAINT [FK_tblSellerNotes_tblReferenceData]
GO
ALTER TABLE [dbo].[tblSellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_tblSellerNotes_tblUsers] FOREIGN KEY([SellerID])
REFERENCES [dbo].[tblUsers] ([ID])
GO
ALTER TABLE [dbo].[tblSellerNotes] CHECK CONSTRAINT [FK_tblSellerNotes_tblUsers]
GO
ALTER TABLE [dbo].[tblSellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_tblSellerNotes_tblUsers1] FOREIGN KEY([ActionedBy])
REFERENCES [dbo].[tblUsers] ([ID])
GO
ALTER TABLE [dbo].[tblSellerNotes] CHECK CONSTRAINT [FK_tblSellerNotes_tblUsers1]
GO
ALTER TABLE [dbo].[tblSellerNotesAttachments]  WITH CHECK ADD  CONSTRAINT [FK_tblSellerNotesAttachments_tblSellerNotes] FOREIGN KEY([NoteID])
REFERENCES [dbo].[tblSellerNotes] ([ID])
GO
ALTER TABLE [dbo].[tblSellerNotesAttachments] CHECK CONSTRAINT [FK_tblSellerNotesAttachments_tblSellerNotes]
GO
ALTER TABLE [dbo].[tblSellerNotesReportedIssues]  WITH CHECK ADD  CONSTRAINT [FK_tblSellerNotesReportedIssues_tblDownloads] FOREIGN KEY([AgainstDownloadID])
REFERENCES [dbo].[tblDownloads] ([ID])
GO
ALTER TABLE [dbo].[tblSellerNotesReportedIssues] CHECK CONSTRAINT [FK_tblSellerNotesReportedIssues_tblDownloads]
GO
ALTER TABLE [dbo].[tblSellerNotesReportedIssues]  WITH CHECK ADD  CONSTRAINT [FK_tblSellerNotesReportedIssues_tblSellerNotes] FOREIGN KEY([NoteID])
REFERENCES [dbo].[tblSellerNotes] ([ID])
GO
ALTER TABLE [dbo].[tblSellerNotesReportedIssues] CHECK CONSTRAINT [FK_tblSellerNotesReportedIssues_tblSellerNotes]
GO
ALTER TABLE [dbo].[tblSellerNotesReportedIssues]  WITH CHECK ADD  CONSTRAINT [FK_tblSellerNotesReportedIssues_tblUsers] FOREIGN KEY([ReportedByID])
REFERENCES [dbo].[tblUsers] ([ID])
GO
ALTER TABLE [dbo].[tblSellerNotesReportedIssues] CHECK CONSTRAINT [FK_tblSellerNotesReportedIssues_tblUsers]
GO
ALTER TABLE [dbo].[tblSellerNotesReviews]  WITH CHECK ADD  CONSTRAINT [FK_tblSellerNotesReviews_tblDownloads] FOREIGN KEY([AgainstDownloadsID])
REFERENCES [dbo].[tblDownloads] ([ID])
GO
ALTER TABLE [dbo].[tblSellerNotesReviews] CHECK CONSTRAINT [FK_tblSellerNotesReviews_tblDownloads]
GO
ALTER TABLE [dbo].[tblSellerNotesReviews]  WITH CHECK ADD  CONSTRAINT [FK_tblSellerNotesReviews_tblSellerNotes] FOREIGN KEY([NoteID])
REFERENCES [dbo].[tblSellerNotes] ([ID])
GO
ALTER TABLE [dbo].[tblSellerNotesReviews] CHECK CONSTRAINT [FK_tblSellerNotesReviews_tblSellerNotes]
GO
ALTER TABLE [dbo].[tblSellerNotesReviews]  WITH CHECK ADD  CONSTRAINT [FK_tblSellerNotesReviews_tblUsers] FOREIGN KEY([ReviewedByID])
REFERENCES [dbo].[tblUsers] ([ID])
GO
ALTER TABLE [dbo].[tblSellerNotesReviews] CHECK CONSTRAINT [FK_tblSellerNotesReviews_tblUsers]
GO
ALTER TABLE [dbo].[tblUserProfile]  WITH CHECK ADD  CONSTRAINT [FK_tblUserProfile_tblReferenceData] FOREIGN KEY([Gender])
REFERENCES [dbo].[tblReferenceData] ([ID])
GO
ALTER TABLE [dbo].[tblUserProfile] CHECK CONSTRAINT [FK_tblUserProfile_tblReferenceData]
GO
ALTER TABLE [dbo].[tblUserProfile]  WITH CHECK ADD  CONSTRAINT [FK_tblUserProfile_tblUsers] FOREIGN KEY([UserID])
REFERENCES [dbo].[tblUsers] ([ID])
GO
ALTER TABLE [dbo].[tblUserProfile] CHECK CONSTRAINT [FK_tblUserProfile_tblUsers]
GO
ALTER TABLE [dbo].[tblUsers]  WITH CHECK ADD  CONSTRAINT [FK_tblUsers_tblUserRoles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[tblUserRoles] ([ID])
GO
ALTER TABLE [dbo].[tblUsers] CHECK CONSTRAINT [FK_tblUsers_tblUserRoles]
GO
USE [master]
GO
ALTER DATABASE [NotesMarketPlace] SET  READ_WRITE 
GO
