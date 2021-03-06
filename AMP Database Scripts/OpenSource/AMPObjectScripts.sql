--USE [master]
--GO
--/****** Object:  Database [AMP]    Script Date: 10/12/2017 08:33:20 ******/
--CREATE DATABASE [AMP] ON  PRIMARY 
--( NAME = N'AMP', FILENAME = N'F:\mssql\data\AMP\AMP.mdf' , SIZE = 1572864KB , MAXSIZE = 5132288KB , FILEGROWTH = 524288KB )
-- LOG ON 
--( NAME = N'AMP_log', FILENAME = N'L:\mssql\Logs\AMP\AMP_log.ldf' , SIZE = 2621440KB , MAXSIZE = 20971520KB , FILEGROWTH = 524288KB )
--GO
ALTER DATABASE [AMP] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AMP].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AMP] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AMP] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AMP] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AMP] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AMP] SET ARITHABORT OFF 
GO
ALTER DATABASE [AMP] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [AMP] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [AMP] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AMP] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AMP] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [AMP] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AMP] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AMP] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AMP] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AMP] SET  DISABLE_BROKER 
GO
ALTER DATABASE [AMP] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AMP] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AMP] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AMP] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AMP] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AMP] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [AMP] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [AMP] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [AMP] SET  MULTI_USER 
GO
ALTER DATABASE [AMP] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [AMP] SET DB_CHAINING OFF 
GO
EXEC sys.sp_db_vardecimal_storage_format N'AMP', N'ON'

/****** Object:  User [AMPAdmin]    Script Date: 10/12/2017 08:33:25 ******/
CREATE USER [AMPAdmin] FOR LOGIN [AMPAdmin] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [AMPAdmin]
GO
ALTER ROLE [db_datareader] ADD MEMBER [AMPAdmin]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [AMPAdmin]
GO
/****** Object:  Schema [Component]    Script Date: 10/12/2017 08:33:33 ******/
CREATE SCHEMA [Component]
GO
/****** Object:  Schema [DFID\ma-robertson_admin]    Script Date: 10/12/2017 08:33:33 ******/
CREATE SCHEMA [DFID\ma-robertson_admin]
GO
/****** Object:  Schema [Location]    Script Date: 10/12/2017 08:33:33 ******/
CREATE SCHEMA [Location]
GO
/****** Object:  Schema [Project]    Script Date: 10/12/2017 08:33:33 ******/
CREATE SCHEMA [Project]
GO
/****** Object:  Schema [Results]    Script Date: 10/12/2017 08:33:33 ******/
CREATE SCHEMA [Results]
GO
/****** Object:  Schema [Shadow]    Script Date: 10/12/2017 08:33:34 ******/
CREATE SCHEMA [Shadow]
GO
/****** Object:  Schema [System]    Script Date: 10/12/2017 08:33:34 ******/
CREATE SCHEMA [System]
GO
/****** Object:  Schema [Tasks]    Script Date: 10/12/2017 08:33:34 ******/
CREATE SCHEMA [Tasks]
GO
/****** Object:  Schema [Workflow]    Script Date: 10/12/2017 08:33:34 ******/
CREATE SCHEMA [Workflow]
GO
/****** Object:  UserDefinedFunction [System].[RemoveNonStandardCharacters]    Script Date: 10/12/2017 08:33:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [System].[RemoveNonStandardCharacters](@String NVARCHAR(MAX))

RETURNS NVARCHAR(MAX)

AS
	BEGIN

		DECLARE @KeepValues AS VARCHAR(255)
		SET @KeepValues = '%[^-a-z0-9 .,&£$%*()+=#@:<>?\/]%'
		WHILE PATINDEX(@KeepValues, @String) > 0
			SET @String = STUFF(@String, PATINDEX(@KeepValues, @String), 1, '')

		RETURN @String
	END

GO
/****** Object:  Table [Component].[ComponentDates]    Script Date: 10/12/2017 08:33:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Component].[ComponentDates](
	[ComponentID] [varchar](10) NOT NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[OperationalStartDate] [datetime] NULL,
	[OperationalEndDate] [datetime] NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ComponentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Component].[ComponentMaster]    Script Date: 10/12/2017 08:33:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Component].[ComponentMaster](
	[ComponentID] [varchar](10) NOT NULL,
	[ComponentDescription] [varchar](max) NULL,
	[BudgetCentreID] [varchar](5) NULL,
	[ProjectID] [varchar](6) NULL,
	[AdminApprover] [varchar](7) NULL,
	[FundingMechanism] [varchar](255) NULL,
	[OperationalStatus] [varchar](255) NULL,
	[BenefittingCountry] [varchar](2) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[Approved] [varchar](5) NULL,
	[FundingArrangementValue] [varchar](20) NULL,
	[PartnerOrganisationValue] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[ComponentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Component].[DeliveryChain]    Script Date: 10/12/2017 08:33:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Component].[DeliveryChain](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ComponentID] [varchar](10) NULL,
	[ChainID] [varchar](50) NOT NULL,
	[ParentID] [int] NOT NULL,
	[ParentType] [varchar](1) NULL,
	[ChildID] [int] NOT NULL,
	[ChildType] [varchar](1) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[ParentNodeID] [int] NULL,
 CONSTRAINT [PK_DeliveryChain] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Component].[ImplementingOrganisation]    Script Date: 10/12/2017 08:33:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Component].[ImplementingOrganisation](
	[ComponentID] [varchar](10) NOT NULL,
	[LineNo] [int] NOT NULL,
	[ImplementingOrganisation] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ComponentID] ASC,
	[LineNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Component].[InputSectorCodes]    Script Date: 10/12/2017 08:33:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Component].[InputSectorCodes](
	[ComponentID] [varchar](10) NOT NULL,
	[LineNo] [int] NOT NULL,
	[InputSectorCode] [varchar](7) NULL,
	[Percentage] [int] NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ComponentID] ASC,
	[LineNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Component].[Markers]    Script Date: 10/12/2017 08:33:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Component].[Markers](
	[ComponentID] [varchar](10) NOT NULL,
	[PBA] [varchar](3) NOT NULL,
	[SWAP] [varchar](3) NOT NULL,
	[Status] [varchar](1) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ComponentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Component].[PartnerMaster]    Script Date: 10/12/2017 08:33:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Component].[PartnerMaster](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PartnerID] [int] NOT NULL,
	[PartnerName] [varchar](255) NOT NULL,
	[Status] [varchar](1) NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
	[UserID] [varchar](6) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Component].[zComponentMaster_PreCR1515045]    Script Date: 10/12/2017 08:33:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Component].[zComponentMaster_PreCR1515045](
	[ComponentID] [varchar](10) NOT NULL,
	[ComponentDescription] [varchar](max) NULL,
	[BudgetCentreID] [varchar](5) NULL,
	[ProjectID] [varchar](6) NULL,
	[AdminApprover] [varchar](7) NULL,
	[FundingMechanism] [varchar](255) NULL,
	[OperationalStatus] [varchar](255) NULL,
	[BenefittingCountry] [varchar](2) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[Approved] [varchar](5) NULL,
	[FundingArrangementValue] [varchar](20) NULL,
	[PartnerOrganisationValue] [varchar](20) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Component].[zDeliveryChain_PreCR1292793]    Script Date: 10/12/2017 08:33:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Component].[zDeliveryChain_PreCR1292793](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ComponentID] [varchar](10) NULL,
	[ChainID] [int] NOT NULL,
	[ParentID] [int] NOT NULL,
	[ParentType] [varchar](1) NULL,
	[ChildID] [int] NOT NULL,
	[ChildType] [varchar](1) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](255) NULL,
	[Status] [varchar](1) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [Location].[CountryCodes]    Script Date: 10/12/2017 08:33:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Location].[CountryCodes](
	[CountryCodeID] [nvarchar](2) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CountryCodeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Location].[GeoCodes]    Script Date: 10/12/2017 08:33:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Location].[GeoCodes](
	[GeoID] [bigint] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[LocationPoint] [varchar](max) NOT NULL,
	[CountryCodeID] [nvarchar](2) NULL,
	[Administrative] [nvarchar](255) NOT NULL,
	[Longitude] [float] NOT NULL,
	[Latitude] [float] NOT NULL,
	[PrecisionID] [int] NOT NULL,
	[LocationTypeID] [nvarchar](255) NOT NULL,
	[Confirmed] [bit] NOT NULL,
	[Deleted] [int] NOT NULL,
	[Percentage] [float] NULL,
	[CMP] [bit] NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[UserID] [varchar](255) NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
 CONSTRAINT [PK_ProjectLocations] PRIMARY KEY CLUSTERED 
(
	[GeoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Location].[LocationType]    Script Date: 10/12/2017 08:33:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Location].[LocationType](
	[LocationTypeID] [nvarchar](255) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Description] [varchar](1000) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LocationTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Location].[Precision]    Script Date: 10/12/2017 08:33:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Location].[Precision](
	[PrecisionID] [int] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Description] [varchar](1000) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PrecisionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[AuditedFinancialStatements]    Script Date: 10/12/2017 08:33:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[AuditedFinancialStatements](
	[ProjectID] [varchar](6) NOT NULL,
	[StatementID] [int] NOT NULL,
	[DueDate] [datetime] NULL,
	[PromptDate] [datetime] NULL,
	[ReceivedDate] [datetime] NULL,
	[PeriodFrom] [datetime] NULL,
	[PeriodTo] [datetime] NULL,
	[Value] [decimal](18, 0) NULL,
	[Currency] [varchar](3) NULL,
	[StatementType] [varchar](255) NULL,
	[reason_action] [varchar](1000) NULL,
	[Status] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[DocumentID] [varchar](12) NULL,
	[DocSource] [varchar](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[StatementID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[AuditedFinancialStatementsTemp]    Script Date: 10/12/2017 08:33:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[AuditedFinancialStatementsTemp](
	[ProjectID] [varchar](6) NOT NULL,
	[StatementID] [int] NOT NULL,
	[DueDate] [datetime] NULL,
	[PromptDate] [datetime] NULL,
	[ReceivedDate] [datetime] NULL,
	[PeriodFrom] [datetime] NULL,
	[PeriodTo] [datetime] NULL,
	[Value] [decimal](18, 0) NULL,
	[Currency] [varchar](3) NULL,
	[StatementType] [varchar](255) NULL,
	[reason_action] [varchar](1000) NULL,
	[Status] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[DocumentID] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[ConditionalityReview]    Script Date: 10/12/2017 08:33:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ConditionalityReview](
	[ProjectID] [varchar](6) NOT NULL,
	[DisbursementSuspended] [varchar](255) NULL,
	[cause] [varchar](1000) NULL,
	[date_suspended] [datetime] NULL,
	[consequences] [varchar](1000) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[Deferral]    Script Date: 10/12/2017 08:33:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[Deferral](
	[ProjectID] [varchar](6) NOT NULL,
	[DeferralTimescale] [varchar](3) NULL,
	[DeferralReason] [varchar](1000) NULL,
	[DeferralJustification] [varchar](1000) NULL,
	[AnnualReviewDate] [datetime] NULL,
	[PCRDueDate] [datetime] NULL,
	[LastUpdated] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[DeferralReason]    Script Date: 10/12/2017 08:33:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[DeferralReason](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DeferralReasons] [varchar](150) NULL,
	[IsActive] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[DSOMarkers]    Script Date: 10/12/2017 08:33:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[DSOMarkers](
	[ProjectID] [varchar](6) NOT NULL,
	[GrowGovTradeBaseServ] [varchar](255) NULL,
	[ClimateChange] [varchar](255) NULL,
	[ConflictHumaniterian] [varchar](255) NULL,
	[GlobalPartnerships] [varchar](255) NULL,
	[MoreEffectiveDoners] [varchar](255) NULL,
	[HighQualityAid] [varchar](255) NULL,
	[InternalEfficency] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[Evaluation]    Script Date: 10/12/2017 08:33:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[Evaluation](
	[EvaluationID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[EvaluationTypeID] [varchar](6) NOT NULL,
	[IfOther] [varchar](255) NULL,
	[ManagementOfEvaluation] [varchar](6) NULL,
	[EstimatedBudget] [decimal](13, 0) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[AdditionalInfo] [varchar](1000) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[EvaluationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[EvaluationDocuments]    Script Date: 10/12/2017 08:33:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[EvaluationDocuments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EvaluationID] [int] NOT NULL,
	[DocumentID] [varchar](12) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[Status] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[DocSource] [varchar](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Project].[EvaluationDocumentsTemp]    Script Date: 10/12/2017 08:33:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[EvaluationDocumentsTemp](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EvaluationID] [int] NOT NULL,
	[DocumentID] [varchar](7) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[Status] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Project].[Markers]    Script Date: 10/12/2017 08:33:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[Markers](
	[ProjectID] [varchar](6) NOT NULL,
	[GenderEquality] [varchar](255) NULL,
	[HIVAIDS] [varchar](255) NULL,
	[Biodiversity] [varchar](255) NULL,
	[Mitigation] [varchar](255) NULL,
	[Adaptation] [varchar](255) NULL,
	[Desertification] [varchar](255) NULL,
	[Status] [varchar](255) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[Disability] [varchar](255) NULL,
	[DisabilityPercentage] [int] NULL,
 CONSTRAINT [Project.Markers_pk] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[OverallRiskRating]    Script Date: 10/12/2017 08:33:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[OverallRiskRating](
	[OverallRiskRatingId] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[Comments] [varchar](1000) NULL,
	[RiskScore] [varchar](10) NULL,
	[UserID] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
 CONSTRAINT [PK_OverallRiskRating] PRIMARY KEY CLUSTERED 
(
	[OverallRiskRatingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[Performance]    Script Date: 10/12/2017 08:33:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[Performance](
	[ProjectID] [varchar](6) NOT NULL,
	[ARRequired] [varchar](3) NULL,
	[ARExemptJustification] [varchar](1000) NULL,
	[ARDueDate] [datetime] NULL,
	[ARPromptDate] [datetime] NULL,
	[ARDefferal] [varchar](3) NULL,
	[PCRRequired] [varchar](3) NULL,
	[PCRExemptJustification] [varchar](1000) NULL,
	[PCRDueDate] [datetime] NULL,
	[PCRPrompt] [datetime] NULL,
	[PCRDefferal] [varchar](3) NULL,
	[PCRDefferalJustification] [varchar](1000) NULL,
	[PCRAuthorised] [varchar](3) NULL,
	[ARExcemptReason] [varchar](1000) NULL,
	[PCRExcemptReason] [varchar](1000) NULL,
	[DefferalTimeScale] [int] NULL,
	[DeferralReason] [varchar](1000) NULL,
	[Status] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[ProjectBudgetCentreOrgUnit]    Script Date: 10/12/2017 08:33:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ProjectBudgetCentreOrgUnit](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[BudgetCentre] [varchar](5) NULL,
	[OrgUnit] [varchar](5) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[ProjectDates]    Script Date: 10/12/2017 08:33:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ProjectDates](
	[ProjectID] [varchar](6) NOT NULL,
	[Created_date] [datetime] NULL,
	[FinancialStartDate] [datetime] NULL,
	[FinancialEndDate] [datetime] NULL,
	[OperationalStartDate] [datetime] NULL,
	[OperationalEndDate] [datetime] NULL,
	[ActualStartDate] [datetime] NULL,
	[PromptCompletionDate] [datetime] NULL,
	[ESNApprovedDate] [datetime] NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[ActualEndDate] [datetime] NULL,
	[ArchiveDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[ProjectInfo]    Script Date: 10/12/2017 08:33:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ProjectInfo](
	[ProjectID] [varchar](6) NOT NULL,
	[OVIS] [varchar](max) NULL,
	[TeamMarker] [varchar](255) NULL,
	[RiskAtApproval] [varchar](255) NULL,
	[SpecificConditions] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Project].[ProjectMaster]    Script Date: 10/12/2017 08:33:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ProjectMaster](
	[ProjectID] [varchar](6) NOT NULL,
	[Title] [varchar](max) NULL,
	[Description] [varchar](max) NULL,
	[BudgetCentreID] [varchar](5) NULL,
	[Stage] [varchar](5) NULL,
	[Status] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Project].[Reports]    Script Date: 10/12/2017 08:33:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[Reports](
	[ProjectID] [varchar](6) NOT NULL,
	[ReportID] [int] NOT NULL,
	[ReportType] [varchar](1000) NULL,
	[DueDate] [datetime] NULL,
	[PromptDate] [datetime] NULL,
	[ReceivedDate] [datetime] NULL,
	[QuestNo] [int] NULL,
	[Status] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[ReportID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[ReviewARScore]    Script Date: 10/12/2017 08:33:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ReviewARScore](
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[OverallScore] [varchar](3) NULL,
	[Justification] [varchar](1000) NULL,
	[Progress] [varchar](1000) NULL,
	[OnTrackTime] [varchar](1) NULL,
	[OnTrackCost] [varchar](1) NULL,
	[OffTrackJustification] [varchar](1000) NULL,
	[Status] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[ReviewID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[ReviewDeferral]    Script Date: 10/12/2017 08:33:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ReviewDeferral](
	[DeferralID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[StageID] [varchar](5) NOT NULL,
	[DeferralTimescale] [varchar](3) NULL,
	[DeferralJustification] [varchar](500) NULL,
	[ApproverComment] [varchar](500) NULL,
	[Approver] [varchar](6) NULL,
	[Approved] [varchar](1) NULL,
	[Requester] [varchar](6) NULL,
	[PreviousReviewDate] [datetime] NULL,
	[LastUpdated] [datetime] NULL,
	[UpdatedBy] [varchar](10) NULL,
 CONSTRAINT [PK__ReviewDe__292D70C770D3A237] PRIMARY KEY CLUSTERED 
(
	[DeferralID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[ReviewDocuments]    Script Date: 10/12/2017 08:33:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ReviewDocuments](
	[ReviewDocumentsID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[DocumentID] [varchar](12) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[DocSource] [varchar](1) NULL,
 CONSTRAINT [PK_ReviewDocuments] PRIMARY KEY CLUSTERED 
(
	[ReviewDocumentsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Project].[ReviewDocumentsTemp]    Script Date: 10/12/2017 08:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ReviewDocumentsTemp](
	[ReviewDocumentsID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[DocumentID] [varchar](7) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Project].[ReviewExemption]    Script Date: 10/12/2017 08:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ReviewExemption](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[StageID] [varchar](5) NOT NULL,
	[ExemptionType] [varchar](50) NOT NULL,
	[ExemptionReason] [varchar](1000) NULL,
	[ApproverComment] [varchar](200) NULL,
	[Approver] [varchar](6) NULL,
	[Approved] [varchar](1) NULL,
	[SubmissionComment] [varchar](200) NULL,
	[Requester] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UpdatedBy] [varchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[ReviewMaster]    Script Date: 10/12/2017 08:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ReviewMaster](
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[ReviewType] [varchar](255) NULL,
	[ReviewDate] [datetime] NULL,
	[DeferralDate] [datetime] NULL,
	[RiskScore] [varchar](10) NULL,
	[Status] [varchar](6) NULL,
	[Approved] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[Approver] [varchar](6) NULL,
	[Requester] [varchar](6) NULL,
	[StageID] [varchar](5) NULL,
	[SubmissionComment] [varchar](500) NULL,
	[ApproveComment] [varchar](500) NULL,
	[ReviewScore] [decimal](18, 1) NULL,
	[DueDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[ReviewID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[ReviewOutputs]    Script Date: 10/12/2017 08:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ReviewOutputs](
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[OutputID] [int] NOT NULL,
	[OutputDescription] [varchar](500) NULL,
	[Weight] [int] NULL,
	[OutputScore] [varchar](3) NULL,
	[ImpactScore] [float] NULL,
	[Risk] [varchar](10) NULL,
	[Status] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[ReviewID] ASC,
	[OutputID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[ReviewPCRScore]    Script Date: 10/12/2017 08:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ReviewPCRScore](
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[FinalOutputScore] [varchar](3) NULL,
	[OutcomeScore] [varchar](3) NULL,
	[ProgressToImpact] [varchar](1000) NULL,
	[CompletedToTimescales] [varchar](1) NULL,
	[CompletedToCost] [varchar](1) NULL,
	[FailedJustification] [varchar](1000) NULL,
	[Status] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[ReviewID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[ReviewScore]    Script Date: 10/12/2017 08:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ReviewScore](
	[OutputScore] [varchar](3) NOT NULL,
	[Definition] [nvarchar](50) NOT NULL,
	[Weight] [int] NOT NULL,
 CONSTRAINT [PK_ReviewScore] PRIMARY KEY CLUSTERED 
(
	[OutputScore] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[ReviewStage]    Script Date: 10/12/2017 08:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[ReviewStage](
	[StageID] [varchar](5) NOT NULL,
	[StageDescription] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[StageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[RiskDocument]    Script Date: 10/12/2017 08:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[RiskDocument](
	[RiskRegisterID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[DocumentID] [varchar](12) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[DocSource] [varchar](1) NULL,
 CONSTRAINT [PK_RiskRegisterDocuments] PRIMARY KEY CLUSTERED 
(
	[RiskRegisterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Project].[RiskDocumentTemp]    Script Date: 10/12/2017 08:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[RiskDocumentTemp](
	[RiskRegisterID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[DocumentID] [varchar](7) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Project].[RiskRegister]    Script Date: 10/12/2017 08:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[RiskRegister](
	[RiskID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[RiskDescription] [varchar](max) NULL,
	[Owner] [varchar](6) NOT NULL,
	[RiskCategory] [int] NOT NULL,
	[RiskLikelihood] [int] NULL,
	[RiskImpact] [int] NULL,
	[GrossRisk] [varchar](10) NOT NULL,
	[MitigationStrategy] [varchar](max) NULL,
	[ResidualLikelihood] [int] NULL,
	[ResidualImpact] [int] NULL,
	[ResidualRisk] [varchar](10) NOT NULL,
	[Comments] [varchar](max) NULL,
	[ExternalOwner] [varchar](max) NULL,
	[Status] [varchar](1) NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
	[UserID] [varchar](6) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RiskID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Project].[Team]    Script Date: 10/12/2017 08:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[Team](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[TeamID] [varchar](255) NOT NULL,
	[RoleID] [varchar](255) NOT NULL,
	[Status] [varchar](255) NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[LastUpdated] [datetime] NOT NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Project].[TeamExternal]    Script Date: 10/12/2017 08:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Project].[TeamExternal](
	[ProjectID] [varchar](6) NOT NULL,
	[MemberID] [varchar](7) NOT NULL,
	[MemberDescription] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC,
	[MemberID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Results].[OutputIndicator]    Script Date: 10/12/2017 08:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Results].[OutputIndicator](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectOutputID] [int] NULL,
	[OutputIndicatorID] [int] NULL,
	[IndicatorDescription] [varchar](1000) NULL,
	[Source] [varchar](1000) NULL,
	[Units] [varchar](255) NULL,
	[Baseline] [varchar](255) NULL,
	[BaselineDate] [datetime] NULL,
	[Target] [varchar](255) NULL,
	[TargetDate] [datetime] NULL,
	[TargetAchieved] [varchar](255) NULL,
	[IsDRF] [varchar](1) NULL,
	[IsCHR] [varchar](1) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Results].[OutputIndicatorMilestones]    Script Date: 10/12/2017 08:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Results].[OutputIndicatorMilestones](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OutputIndicatorID] [int] NULL,
	[MilestoneID] [int] NULL,
	[From] [datetime] NULL,
	[To] [datetime] NULL,
	[Planned] [varchar](255) NULL,
	[Change] [varchar](255) NULL,
	[Achieved] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Results].[ProjectOutput]    Script Date: 10/12/2017 08:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Results].[ProjectOutput](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NULL,
	[OutputID] [int] NULL,
	[ProjectOutputDescription] [varchar](1000) NULL,
	[Assumption] [varchar](1000) NULL,
	[ImpactWeightingPercentage] [int] NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[Risk] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[AuditedFinancialStatements]    Script Date: 10/12/2017 08:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[AuditedFinancialStatements](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[Change_Status] [varchar](255) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[StatementID] [int] NOT NULL,
	[DueDate] [datetime] NULL,
	[PromptDate] [datetime] NULL,
	[ReceivedDate] [datetime] NULL,
	[PeriodFrom] [datetime] NULL,
	[PeriodTo] [datetime] NULL,
	[Value] [decimal](18, 0) NULL,
	[Currency] [varchar](3) NULL,
	[StatementType] [varchar](255) NULL,
	[reason_action] [varchar](1000) NULL,
	[Status] [varchar](255) NULL,
	[LastUpdated] [datetime] NOT NULL,
	[UserID] [varchar](6) NULL,
	[DocumentID] [varchar](12) NULL,
PRIMARY KEY CLUSTERED 
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[AuditedFinancialStatementsTemp]    Script Date: 10/12/2017 08:33:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[AuditedFinancialStatementsTemp](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[Change_Status] [varchar](255) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[StatementID] [int] NOT NULL,
	[DueDate] [datetime] NULL,
	[PromptDate] [datetime] NULL,
	[ReceivedDate] [datetime] NULL,
	[PeriodFrom] [datetime] NULL,
	[PeriodTo] [datetime] NULL,
	[Value] [decimal](18, 0) NULL,
	[Currency] [varchar](3) NULL,
	[StatementType] [varchar](255) NULL,
	[reason_action] [varchar](1000) NULL,
	[Status] [varchar](255) NULL,
	[LastUpdated] [datetime] NOT NULL,
	[UserID] [varchar](6) NULL,
	[DocumentID] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ComponentDates]    Script Date: 10/12/2017 08:33:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ComponentDates](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[Change_Status] [varchar](255) NOT NULL,
	[ComponentID] [varchar](10) NOT NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[OperationalStartDate] [datetime] NULL,
	[OperationalEndDate] [datetime] NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ComponentDeliveryChain]    Script Date: 10/12/2017 08:33:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ComponentDeliveryChain](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[ID] [int] NOT NULL,
	[ChangeStatus] [varchar](255) NOT NULL,
	[ComponentID] [varchar](10) NOT NULL,
	[ChainID] [varchar](50) NULL,
	[ParentID] [int] NULL,
	[ParentType] [varchar](1) NULL,
	[ChildID] [int] NULL,
	[ChildType] [varchar](1) NULL,
	[ParentNodeID] [int] NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
 CONSTRAINT [PK_DeliveryChain] PRIMARY KEY CLUSTERED 
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ComponentDeliveryChainBackup]    Script Date: 10/12/2017 08:33:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ComponentDeliveryChainBackup](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[ID] [int] NOT NULL,
	[ChangeStatus] [varchar](255) NOT NULL,
	[ComponentID] [varchar](10) NULL,
	[ChainID] [varchar](50) NULL,
	[ParentID] [int] NULL,
	[ParentType] [varchar](1) NULL,
	[ChildID] [int] NULL,
	[ChildType] [varchar](1) NULL,
	[ParentNodeID] [int] NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](255) NULL,
	[Status] [varchar](1) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ComponentMaster]    Script Date: 10/12/2017 08:33:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ComponentMaster](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[Change_Status] [varchar](255) NOT NULL,
	[ComponentID] [varchar](10) NOT NULL,
	[ComponentDescription] [varchar](max) NULL,
	[BudgetCentreID] [varchar](5) NULL,
	[ProjectID] [varchar](6) NULL,
	[AdminApprover] [varchar](7) NULL,
	[FundingMechanism] [varchar](255) NULL,
	[OperationalStatus] [varchar](255) NULL,
	[BenefittingCountry] [varchar](2) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[Approved] [varchar](5) NULL,
	[FundingArrangementValue] [varchar](20) NULL,
	[PartnerOrganisationValue] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[Evaluation]    Script Date: 10/12/2017 08:33:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[Evaluation](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[Change_Status] [varchar](255) NOT NULL,
	[EvaluationID] [int] NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[EvaluationTypeID] [varchar](6) NOT NULL,
	[IfOther] [varchar](255) NULL,
	[ManagementOfEvaluation] [varchar](6) NULL,
	[EstimatedBudget] [decimal](13, 0) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[AdditionalInfo] [varchar](1000) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ImplementingOrganisation]    Script Date: 10/12/2017 08:33:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ImplementingOrganisation](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[Change_Status] [varchar](255) NOT NULL,
	[ComponentID] [varchar](10) NOT NULL,
	[LineNo] [int] NOT NULL,
	[ImplementingOrganisation] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[InputSectorCodes]    Script Date: 10/12/2017 08:33:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[InputSectorCodes](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[Change_Status] [varchar](255) NOT NULL,
	[ComponentID] [varchar](10) NOT NULL,
	[LineNo] [int] NOT NULL,
	[InputSectorCode] [varchar](7) NULL,
	[Percentage] [int] NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[Markers]    Script Date: 10/12/2017 08:33:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[Markers](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[Change_Status] [varchar](255) NOT NULL,
	[ComponentID] [varchar](10) NOT NULL,
	[PBA] [varchar](3) NOT NULL,
	[SWAP] [varchar](3) NOT NULL,
	[Status] [varchar](1) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[Performance]    Script Date: 10/12/2017 08:33:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[Performance](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[Change_Status] [varchar](255) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[ARRequired] [varchar](3) NULL,
	[ARExemptJustification] [varchar](1000) NULL,
	[ARDueDate] [datetime] NULL,
	[ARPromptDate] [datetime] NULL,
	[ARDefferal] [varchar](3) NULL,
	[PCRRequired] [varchar](3) NULL,
	[PCRExemptJustification] [varchar](1000) NULL,
	[PCRDueDate] [datetime] NULL,
	[PCRPrompt] [datetime] NULL,
	[PCRDefferal] [varchar](3) NULL,
	[PCRDefferalJustification] [varchar](1000) NULL,
	[PCRAuthorised] [varchar](3) NULL,
	[ARExcemptReason] [varchar](1000) NULL,
	[PCRExcemptReason] [varchar](1000) NULL,
	[DefferalTimeScale] [int] NULL,
	[DeferralReason] [varchar](1000) NULL,
	[Status] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ProjectDates]    Script Date: 10/12/2017 08:33:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ProjectDates](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[Change_Status] [varchar](255) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[Created_date] [datetime] NULL,
	[FinancialStartDate] [datetime] NULL,
	[FinancialEndDate] [datetime] NULL,
	[OperationalStartDate] [datetime] NULL,
	[OperationalEndDate] [datetime] NULL,
	[ActualStartDate] [datetime] NULL,
	[PromptCompletionDate] [datetime] NULL,
	[ESNApprovedDate] [datetime] NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[ActualEndDate] [datetime] NULL,
	[ArchiveDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ProjectInfo]    Script Date: 10/12/2017 08:33:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ProjectInfo](
	[ProjectID] [varchar](6) NOT NULL,
	[Change_Status] [varchar](255) NOT NULL,
	[OVIS] [varchar](max) NULL,
	[TeamMarker] [varchar](255) NULL,
	[RiskAtApproval] [varchar](255) NULL,
	[SpecificConditions] [varchar](255) NULL,
	[LastUpdated] [datetime] NOT NULL,
	[UserID] [varchar](6) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ProjectMarkers]    Script Date: 10/12/2017 08:33:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ProjectMarkers](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[ChangeStatus] [varchar](255) NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[GenderEquality] [varchar](255) NULL,
	[HIVAIDS] [varchar](255) NULL,
	[Biodiversity] [varchar](255) NULL,
	[Mitigation] [varchar](255) NULL,
	[Adaptation] [varchar](255) NULL,
	[Desertification] [varchar](255) NULL,
	[Status] [varchar](255) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
 CONSTRAINT [Shadow.ProjectMarkers_pk] PRIMARY KEY CLUSTERED 
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ProjectMaster]    Script Date: 10/12/2017 08:33:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ProjectMaster](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[Change_Status] [varchar](255) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[Title] [varchar](max) NULL,
	[Description] [varchar](max) NULL,
	[BudgetCentreID] [varchar](5) NULL,
	[Stage] [varchar](5) NULL,
	[Status] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ReviewARScore]    Script Date: 10/12/2017 08:33:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ReviewARScore](
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[OverallScore] [varchar](3) NULL,
	[Justification] [varchar](1000) NULL,
	[Progress] [varchar](1000) NULL,
	[OnTrackTime] [varchar](1) NULL,
	[OnTrackCost] [varchar](1) NULL,
	[OffTrackJustification] [varchar](1000) NULL,
	[Status] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](20) NULL,
	[Change_Status] [varchar](10) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ReviewDeferral]    Script Date: 10/12/2017 08:33:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ReviewDeferral](
	[DeferralID] [int] NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[StageID] [varchar](5) NOT NULL,
	[DeferralTimescale] [varchar](3) NULL,
	[DeferralJustification] [varchar](500) NULL,
	[ApproverComment] [varchar](500) NULL,
	[Approver] [varchar](6) NULL,
	[Approved] [varchar](1) NULL,
	[Requester] [varchar](6) NULL,
	[PreviousReviewDate] [datetime] NULL,
	[LastUpdated] [datetime] NULL,
	[UpdatedBy] [varchar](10) NULL,
	[Change_Status] [varchar](10) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ReviewDocuments]    Script Date: 10/12/2017 08:33:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ReviewDocuments](
	[ReviewDocumentsID] [int] NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[DocumentID] [varchar](12) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[Change_Status] [varchar](10) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ReviewDocumentsTemp]    Script Date: 10/12/2017 08:33:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ReviewDocumentsTemp](
	[ReviewDocumentsID] [int] NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[DocumentID] [varchar](7) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[Change_Status] [varchar](10) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ReviewExemption]    Script Date: 10/12/2017 08:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ReviewExemption](
	[ID] [int] NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[StageID] [varchar](5) NOT NULL,
	[ExemptionType] [varchar](50) NOT NULL,
	[ExemptionReason] [varchar](1000) NULL,
	[ApproverComment] [varchar](200) NULL,
	[Approver] [varchar](6) NULL,
	[Approved] [varchar](1) NULL,
	[SubmissionComment] [varchar](200) NULL,
	[Requester] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UpdatedBy] [varchar](10) NULL,
	[Change_Status] [varchar](10) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ReviewMaster]    Script Date: 10/12/2017 08:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ReviewMaster](
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[ReviewType] [varchar](255) NULL,
	[ReviewDate] [datetime] NULL,
	[DeferralDate] [datetime] NULL,
	[RiskScore] [varchar](10) NULL,
	[Status] [varchar](6) NULL,
	[Approved] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[StageID] [varchar](5) NULL,
	[ApproveComment] [varchar](500) NULL,
	[Approver] [varchar](6) NULL,
	[SubmissionComment] [varchar](500) NULL,
	[Requester] [varchar](6) NULL,
	[ReviewScore] [decimal](18, 1) NULL,
	[Change_Status] [varchar](10) NULL,
	[DueDate] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ReviewOutputs]    Script Date: 10/12/2017 08:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ReviewOutputs](
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[OutputID] [int] NOT NULL,
	[OutputDescription] [varchar](500) NULL,
	[Weight] [int] NULL,
	[OutputScore] [varchar](3) NULL,
	[ImpactScore] [float] NULL,
	[Risk] [varchar](10) NULL,
	[Status] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[Change_Status] [varchar](10) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[ReviewPCRScore]    Script Date: 10/12/2017 08:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[ReviewPCRScore](
	[ProjectID] [varchar](6) NOT NULL,
	[ReviewID] [int] NOT NULL,
	[FinalOutputScore] [varchar](3) NULL,
	[OutcomeScore] [varchar](3) NULL,
	[ProgressToImpact] [varchar](1000) NULL,
	[CompletedToTimescales] [varchar](1) NULL,
	[CompletedToCost] [varchar](1) NULL,
	[FailedJustification] [varchar](1000) NULL,
	[Status] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[Change_Status] [varchar](10) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[RiskDocument]    Script Date: 10/12/2017 08:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[RiskDocument](
	[RiskRegisterID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[DocumentID] [varchar](12) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[Change_State] [varchar](1) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[RiskDocumentTemp]    Script Date: 10/12/2017 08:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[RiskDocumentTemp](
	[RiskRegisterID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[DocumentID] [varchar](7) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[Change_State] [varchar](1) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[RiskRegister]    Script Date: 10/12/2017 08:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[RiskRegister](
	[RiskID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[RiskDescription] [varchar](max) NULL,
	[Owner] [varchar](6) NOT NULL,
	[RiskCategory] [int] NOT NULL,
	[RiskLikelihood] [int] NULL,
	[RiskImpact] [int] NULL,
	[GrossRisk] [varchar](10) NOT NULL,
	[MitigationStrategy] [varchar](max) NULL,
	[ResidualLikelihood] [int] NULL,
	[ResidualImpact] [int] NULL,
	[ResidualRisk] [varchar](10) NOT NULL,
	[Comments] [varchar](max) NULL,
	[ExternalOwner] [varchar](max) NULL,
	[Status] [varchar](1) NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
	[UserID] [varchar](6) NOT NULL,
	[Change_State] [varchar](1) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[Team]    Script Date: 10/12/2017 08:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[Team](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[Change_Status] [varchar](255) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[TeamID] [varchar](255) NOT NULL,
	[RoleID] [varchar](255) NOT NULL,
	[Status] [varchar](255) NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[LastUpdated] [datetime] NOT NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Shadow].[WorkflowMaster]    Script Date: 10/12/2017 08:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Shadow].[WorkflowMaster](
	[Change_Status] [varchar](10) NULL,
	[WorkFlowID] [int] NOT NULL,
	[WorkFlowStepID] [int] NOT NULL,
	[TaskID] [int] NOT NULL,
	[StageID] [int] NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[ActionBy] [varchar](6) NULL,
	[ActionDate] [datetime] NULL,
	[Recipient] [varchar](6) NULL,
	[ActionComments] [varchar](500) NULL,
	[Status] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [System].[AdminUsers]    Script Date: 10/12/2017 08:33:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[AdminUsers](
	[AdminUserID] [varchar](6) NOT NULL,
	[Status] [varchar](255) NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
	[UserID] [varchar](6) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AdminUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[BenefitingCountry]    Script Date: 10/12/2017 08:33:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[BenefitingCountry](
	[BenefitingCountryID] [varchar](2) NOT NULL,
	[BenefitingCountryDescription] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[BenefitingCountryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[BudgetCentre]    Script Date: 10/12/2017 08:33:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[BudgetCentre](
	[BudgetCentreID] [varchar](5) NOT NULL,
	[BudgetCentreDescription] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[BudgetCentreID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[CodePerformance]    Script Date: 10/12/2017 08:33:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[CodePerformance](
	[LogID] [int] IDENTITY(1,1) NOT NULL,
	[MethodName] [varchar](255) NULL,
	[Description] [varchar](255) NULL,
	[Value] [varchar](6) NULL,
	[From] [datetime] NULL,
	[To] [datetime] NULL,
	[Result] [float] NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[ComponentDateHashtable]    Script Date: 10/12/2017 08:33:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[ComponentDateHashtable](
	[CheckSumValue] [int] NULL,
	[work_order] [varchar](25) NOT NULL,
	[date_from] [datetime] NOT NULL,
	[date_to] [datetime] NOT NULL,
	[start_date] [datetime] NULL,
	[end_date] [datetime] NULL,
	[last_update] [datetime] NULL,
	[UserID] [varchar](6) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [System].[ComponentDates]    Script Date: 10/12/2017 08:33:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[ComponentDates](
	[CheckSumValue] [int] NULL,
	[work_order] [varchar](25) NOT NULL,
	[date_from] [datetime] NOT NULL,
	[date_to] [datetime] NOT NULL,
	[start_date] [datetime] NULL,
	[end_date] [datetime] NULL,
	[last_update] [datetime] NULL,
	[UserID] [varchar](6) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [System].[ComponentHashtable]    Script Date: 10/12/2017 08:33:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[ComponentHashtable](
	[CheckSumValue] [int] NULL,
	[work_order] [varchar](25) NOT NULL,
	[description] [varchar](255) NOT NULL,
	[InputterID] [varchar](7) NULL,
	[QualityAssurerID] [varchar](7) NULL,
	[department] [varchar](25) NOT NULL,
	[project] [varchar](25) NOT NULL,
	[dfid_role] [varchar](25) NOT NULL,
	[admin_bud_approver] [varchar](25) NOT NULL,
	[funding_mechanism] [varchar](25) NOT NULL,
	[op_status] [varchar](25) NOT NULL,
	[benefitting_country] [varchar](25) NOT NULL,
	[last_update] [datetime] NOT NULL,
	[UserID] [varchar](6) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [System].[DateHashTable]    Script Date: 10/12/2017 08:33:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[DateHashTable](
	[CheckSumValue] [int] NULL,
	[project] [varchar](25) NOT NULL,
	[created_date] [datetime] NOT NULL,
	[date_from] [datetime] NOT NULL,
	[date_to] [datetime] NOT NULL,
	[start_date] [datetime] NULL,
	[end_date] [datetime] NULL,
	[actual_start] [datetime] NULL,
	[promp_compl] [datetime] NULL,
	[ies_date] [datetime] NULL,
	[last_update] [datetime] NOT NULL,
	[User_ID] [varchar](25) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [System].[ErrorLog]    Script Date: 10/12/2017 08:33:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[ErrorLog](
	[ErrorID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](10) NULL,
	[ErrorMessage] [varchar](max) NULL,
	[InnerException] [varchar](max) NULL,
	[StackTrace] [varchar](max) NULL,
	[CustomError] [varchar](max) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ErrorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [System].[EvaluationManagement]    Script Date: 10/12/2017 08:33:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[EvaluationManagement](
	[EvaluationManagementID] [varchar](6) NOT NULL,
	[EvaluationManagementDescription] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[EvaluationManagementID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[EvaluationType]    Script Date: 10/12/2017 08:33:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[EvaluationType](
	[EvaluationTypeID] [varchar](6) NOT NULL,
	[EvaluationDescription] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[EvaluationTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[ExemptionReason]    Script Date: 10/12/2017 08:33:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[ExemptionReason](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ExemptionID] [int] NULL,
	[ExemptionType] [varchar](3) NOT NULL,
	[ExemptionReason] [varchar](150) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[FundingArrangement]    Script Date: 10/12/2017 08:33:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[FundingArrangement](
	[FundingArrangementValue] [varchar](10) NOT NULL,
	[FundingArrangementType] [varchar](100) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[FundingArrangementValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[FundingMech]    Script Date: 10/12/2017 08:33:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[FundingMech](
	[FundingMechID] [varchar](25) NOT NULL,
	[FundingMechDescription] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[FundingMechID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[FundingMechToSector]    Script Date: 10/12/2017 08:33:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[FundingMechToSector](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SectorCode] [varchar](7) NULL,
	[FundingMech] [varchar](255) NULL,
	[Status] [varchar](6) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[InputSector]    Script Date: 10/12/2017 08:33:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[InputSector](
	[InputSectorCodeID] [varchar](7) NOT NULL,
	[InputSectorCodeDescription] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[InputSectorCodeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[Logging]    Script Date: 10/12/2017 08:33:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[Logging](
	[LogID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](10) NULL,
	[ViewName] [varchar](255) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[PartnerOrganisation]    Script Date: 10/12/2017 08:33:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[PartnerOrganisation](
	[PartnerOrganisationValue] [varchar](10) NOT NULL,
	[PartnerOrganisationType] [varchar](250) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[PartnerOrganisationValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[Portfolio]    Script Date: 10/12/2017 08:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[Portfolio](
	[PortfolioID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NULL,
	[Status] [varchar](10) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[PortfolioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[ProjectHashtable]    Script Date: 10/12/2017 08:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[ProjectHashtable](
	[CheckSumValue] [int] NULL,
	[project] [varchar](25) NOT NULL,
	[description] [varchar](255) NOT NULL,
	[Purpose] [varchar](8000) NULL,
	[department] [varchar](25) NOT NULL,
	[Stage] [varchar](25) NOT NULL,
	[Status] [varchar](25) NOT NULL,
	[last_update] [datetime] NOT NULL,
	[User] [varchar](6) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [System].[ProjectInfoTable]    Script Date: 10/12/2017 08:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[ProjectInfoTable](
	[CheckSumValue] [int] NULL,
	[project] [varchar](25) NOT NULL,
	[ovis] [varchar](1000) NULL,
	[team_marker] [varchar](100) NULL,
	[approval_risk] [varchar](10) NULL,
	[specific_conditions] [varchar](25) NULL,
	[GenderEquality] [varchar](25) NULL,
	[HIVAIDS] [varchar](25) NULL,
	[last_update] [datetime] NOT NULL,
	[User_ID] [varchar](6) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [System].[ProjectRole]    Script Date: 10/12/2017 08:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[ProjectRole](
	[ProjectRoleID] [varchar](255) NOT NULL,
	[ProjectRoleDescription] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProjectRoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[ProjectTeamHash]    Script Date: 10/12/2017 08:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[ProjectTeamHash](
	[CheckSumValue] [int] NULL,
	[ProjectID] [varchar](6) NULL,
	[name] [varchar](25) NOT NULL,
	[role] [varchar](25) NOT NULL,
	[Status] [varchar](1) NOT NULL,
	[date_from_fx] [datetime] NULL,
	[date_to_fx] [datetime] NULL,
	[last_update] [datetime] NULL,
	[User_ID] [varchar](25) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [System].[Risk]    Script Date: 10/12/2017 08:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[Risk](
	[RiskValue] [varchar](10) NOT NULL,
	[RiskTitle] [varchar](25) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[RiskValue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[RiskCategory]    Script Date: 10/12/2017 08:33:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[RiskCategory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RiskCategoryDescription] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[RiskImpact]    Script Date: 10/12/2017 08:33:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[RiskImpact](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RiskImpactDescription] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[RiskLikelihood]    Script Date: 10/12/2017 08:33:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[RiskLikelihood](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RiskLikelihoodDescription] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[Stage]    Script Date: 10/12/2017 08:33:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[Stage](
	[StageID] [varchar](5) NOT NULL,
	[StageDescription] [varchar](255) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[StageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [System].[UserLookUp]    Script Date: 10/12/2017 08:33:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[UserLookUp](
	[ResourceID] [varchar](6) NOT NULL,
	[UserName] [varchar](max) NULL,
	[UserLogon] [varchar](max) NULL,
	[LastUpdated] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[ResourceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [System].[Users]    Script Date: 10/12/2017 08:33:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [System].[Users](
	[UserID] [varchar](7) NOT NULL,
	[UserName] [varchar](1000) NULL,
	[Logon] [varchar](1000) NULL,
	[Status] [varchar](1) NULL,
	[LastUpdated] [datetime] NULL,
	[UserUpdated] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Workflow].[ProjectPlannedEndDates]    Script Date: 10/12/2017 08:33:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Workflow].[ProjectPlannedEndDates](
	[Identity] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[CurrentPlannedEndDate] [datetime] NOT NULL,
	[NewPlannedEndDate] [datetime] NOT NULL,
	[Status] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[WorkTaskID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Identity] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Workflow].[WorkflowDocuments]    Script Date: 10/12/2017 08:33:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Workflow].[WorkflowDocuments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WorkflowID] [int] NOT NULL,
	[DocumentID] [varchar](12) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[Status] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
	[DocSource] [varchar](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Workflow].[WorkflowDocumentsTemp]    Script Date: 10/12/2017 08:33:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Workflow].[WorkflowDocumentsTemp](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WorkflowID] [int] NOT NULL,
	[DocumentID] [varchar](7) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[Status] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Workflow].[WorkflowMaster]    Script Date: 10/12/2017 08:33:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Workflow].[WorkflowMaster](
	[WorkFlowID] [int] NOT NULL,
	[WorkFlowStepID] [int] NOT NULL,
	[TaskID] [int] NOT NULL,
	[StageID] [int] NOT NULL,
	[ProjectID] [varchar](6) NOT NULL,
	[ActionBy] [varchar](6) NULL,
	[ActionDate] [datetime] NULL,
	[Recipient] [varchar](6) NULL,
	[ActionComments] [varchar](500) NULL,
	[Status] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[WorkFlowID] ASC,
	[WorkFlowStepID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Workflow].[WorkflowStage]    Script Date: 10/12/2017 08:33:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Workflow].[WorkflowStage](
	[StageID] [int] NOT NULL,
	[Description] [varchar](500) NOT NULL,
	[Status] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[StageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Workflow].[WorkflowTask]    Script Date: 10/12/2017 08:33:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Workflow].[WorkflowTask](
	[TaskID] [int] NOT NULL,
	[Description] [varchar](500) NOT NULL,
	[Status] [varchar](255) NULL,
	[LastUpdate] [datetime] NULL,
	[UserID] [varchar](6) NULL,
PRIMARY KEY CLUSTERED 
(
	[TaskID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [Tasks].[vAMPAlerts]    Script Date: 10/12/2017 08:33:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [Tasks].[vAMPAlerts]
AS

Select 
	P.ProjectID as ProjectID,
	'Annual Review is due on ' + CAST(DAY(Perf.ARDueDate) AS VARCHAR(2)) + ' ' + DATENAME(MONTH, Perf.ARDueDate) + ' ' + CAST(YEAR(Perf.ARDueDate) AS VARCHAR(4)) AS [Message],
	'/Project/Reviews/' + p.ProjectID as [Path],
	Team.TeamID as 'UserID',
	'3' as Severity
from 
	Project.ProjectMaster P 
	INNER JOIN Project.Performance Perf ON p.ProjectID = perf.ProjectID
	INNER JOIN Project.Team Team ON p.ProjectID = Team.ProjectID
Where
	Perf.ARPromptDate <= GetDate() and 
	Perf.ARRequired = 'Yes' and 
	Perf.ARDueDate > '1900 JAN 01 :00:00:00' and
	Perf.ARDueDate >= GETDATE() and
	Team.ProjectID = p.ProjectID and
	Team.Status = 'A'
UNION ALL
Select 
	P.ProjectID as ProjectID,
	'Annual Review is overdue' AS [Message],
	'/Project/Reviews/' + p.ProjectID as [Path],
	Team.TeamID as 'UserID',
	'1' as Severity
from 
	Project.ProjectMaster P 
	INNER JOIN Project.Performance Perf ON p.ProjectID = perf.ProjectID
	INNER JOIN Project.Team Team ON p.ProjectID = Team.ProjectID
Where
	Perf.ARRequired = 'Yes' and 
	Perf.ARDueDate > '1900 JAN 01 :00:00:00' and
	Perf.ARDueDate < GETDATE() and
	Team.ProjectID = p.ProjectID and
	Team.Status = 'A'
UNION ALL
Select 
	P.ProjectID as ProjectID,
	'PCR is due on ' + CAST(DAY(Perf.PCRDueDate) AS VARCHAR(2)) + ' ' + DATENAME(MONTH, Perf.PCRDueDate) + ' ' + CAST(YEAR(Perf.PCRDueDate) AS VARCHAR(4)) AS [Message],
	'/Project/Reviews/' + p.ProjectID as [Path],
	Team.TeamID as 'UserID',
	'3' as Severity
from 
	Project.ProjectMaster P 
	INNER JOIN Project.Performance Perf ON p.ProjectID = perf.ProjectID
	INNER JOIN Project.Team Team ON p.ProjectID = Team.ProjectID
Where
	Perf.PCRPrompt <= GetDate() and 
	Perf.PCRRequired = 'Yes' and 
	Perf.PCRDueDate > '1900 JAN 01 :00:00:00' and
	Perf.PCRDueDate >= GETDATE() and
	Team.ProjectID = p.ProjectID and
	Team.Status = 'A'
UNION ALL
Select 
	P.ProjectID as ProjectID,
	'Project Completion Review is overdue' AS [Message],
	'/Project/Reviews/' + p.ProjectID as [Path],
	Team.TeamID as 'UserID',
	'1' as Severity
from 
	Project.ProjectMaster P 
	INNER JOIN Project.Performance Perf ON p.ProjectID = perf.ProjectID
	INNER JOIN Project.Team Team ON p.ProjectID = Team.ProjectID
Where
	Perf.PCRRequired = 'Yes' and 
	Perf.PCRDueDate > '1900 JAN 01 :00:00:00' and
	Perf.PCRDueDate < GETDATE() and
	Team.ProjectID = p.ProjectID and
	Team.Status = 'A'



GO
/****** Object:  View [Tasks].[vAMPTasks]    Script Date: 10/12/2017 08:33:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [Tasks].[vAMPTasks]                                                   
AS

SELECT P.Title,Z.* FROM 
(
Select 
       wf.ProjectID AS ProjectID, 
       wf.ActionBy AS Sender, 
       wf.Recipient AS Recipient, 
       wf.ActionDate AS ActionDate, 
       wf.ActionComments AS SenderComments, 
       CASE wt.Description
       WHEN 'Admin or Rapid Response' THEN 'Approve Admin or Rapid Response' ELSE wt.Description END AS TaskDescription,
       '/Workflow/Edit/' + wf.ProjectID + '/' + CAST(wf.TaskID AS varchar(2))  AS [Path]
from 
       Workflow.WorkflowMaster wf, 
       Workflow.WorkflowTask wt
Where
       wf.TaskID = wt.taskID and wf.status = 'A'
UNION ALL
Select
       ProjectID as ProjectID,
       UserID as Sender,
       Approver as Recipient,
       LastUpdated as ActionDate,
       SubmissionComment as SenderComments,
       'Approve ' + ReviewType as TaskDescription,
       '/Project/Reviews/' + ProjectID as [Path]
from 
       Project.ReviewMaster 
where 
       StageId = 1
UNION ALL
Select
       ProjectID as ProjectID,
       Requester as Sender,
       Approver as Recipient,
       LastUpdated as ActionDate,
       DeferralJustification as SenderComments,
       'Approve Annual Review Deferral' as TaskDescription,
       '/Project/Reviews/' + ProjectID as [Path]
from 
       Project.ReviewDeferral
where 
       StageId = 1
UNION ALL
Select
       ProjectID as ProjectID,
       Requester as Sender,
       Approver as Recipient,
       LastUpdated as ActionDate,
       SubmissionComment as SenderComments,
       'Approve ' + ExemptionType + ' Exemption' as TaskDescription,
       '/Project/Reviews/' + ProjectID as [Path]
from 
       Project.ReviewExemption
where 
       StageId = 1
)Z
INNER JOIN 
       Project.ProjectMaster P
ON 
       Z.ProjectID = P.ProjectID

GO
/****** Object:  View [Workflow].[V_ProjectClosure]    Script Date: 10/12/2017 08:33:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [Workflow].[V_ProjectClosure]
WITH SCHEMABINDING
AS
WITH Step1 AS
(
SELECT 
	ProjectID,
	ReviewDate AS ClosureDate,
	Approver
FROM
	[Project].[ReviewMaster]
WHERE
	ReviewType = 'Project Completion Review'
AND
	approved = '1'
AND
	Approver IS NOT NULL

UNION ALL 

SELECT 
	ProjectID,
	ActionDate AS ClosureDate,
	ActionBy AS Approver
FROM
	[Workflow].[WorkflowMaster]
WHERE
	TaskID = '1'
AND
	StageID = '2'
AND
	Status = 'C'
),

Step2 AS
(
SELECT
	ProjectId ,
	ClosureDate ,
	Approver ,
	ROW_NUMBER() OVER(PARTITION BY ProjectId ORDER BY ClosureDate DESC) AS [Ranking]
FROM
	Step1
)

SELECT
	ProjectID,
	ClosureDate,
	Approver
FROM 
	Step2
WHERE
	[Ranking] = 1

GO
/****** Object:  View [Workflow].[vHoDBudCentLookup]    Script Date: 10/12/2017 08:33:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





--CREATE VIEW [Workflow].[vHoDBudCentLookup]
--AS
--Select 
--	IDSQL.display_name_Forename_first AS 'DisplayName',
--	IDSQL.EMP_NO as 'EmpNo',
--	IDSQL.Current_Grade_Simple as 'Grade',
--	CASE IDSQL.Current_Grade_Simple
--		WHEN 'G1A' THEN 1
--		WHEN 'G2' THEN 2
--		WHEN 'G3' THEN 3
--		WHEN 'G5' THEN 4
--		WHEN 'A1' THEN 5
--		WHEN 'A2' THEN 6
--		ELSE 7
--	END as 'GradeRank',
--	BudCent.BudgetCentreID as 'BudetCentre', 
--	BudCent.BudgetCentreDescription as 'BudgetCentreDescription',
--	relvalue.rel_value as 'DivisionCode',
--	project.ProjectID
--from 
--	System.BudgetCentre BudCent
--JOIN 
--	[eksa87].[Agresso].[dbo].aglrelvalue relvalue on relvalue.att_value = BudCent.BudgetCentreID
--JOIN
--	[Identity].[IdentitySQL].[identitysql] IDSQL on IDSQL.DIVISION_CODE = relvalue.rel_value
--JOIN
--	Project.ProjectMaster project ON project.BudgetCentreID = BudCent.BudgetCentreID
--WHERE
--	relvalue.client = 'DF' AND
--	relvalue.rel_attr_id = '36' AND 
--	relvalue.rel_value NOT IN (
--'U0001',     
--'U0293',    
--'U0036',   
--'U0278',  
--'U0001', 
--'U0036',     
--'U0065',     
--'U0005',     
--'U0087',          
--'U0043',     
--'U0001',     
--'U0087',     
--'U0005',     
--'U0001',     
--'U0007',
--'U0070') AND     
--	IDSQL.IS_HOD = 'T' AND
--	IDSQL.IS_CURRENT = 'T' AND
--	Project.Status = 'A'
--DIVISION_CODE	DIVISION_NAME
--U0001     	Top Management Group
--U0293     	PGP Cabinet
--U0036     	Finance and Corporate Performance
--U0278     	Economic Development
--U0001     	Top Management Group
--U0036     	Finance and Corporate Performance
--U0065     	Stabilisation Unit
--U0005     	Middle East, Humanitarian and Conflict
--U0087     	Africa Regional
--U0014     	DFID Ethiopia * INCLUDE THIS ONE. DIRECTOR LEVEL HOD
--U0043     	Research and Evidence
--U0001     	Top Management Group
--U0087     	Africa Regional
--U0005     	Middle East, Humanitarian and Conflict
--U0001     	Top Management Group
--U0007     	AsCOT Director's Office

--David Hallam
--Pauline Hayes
--Rachael Turner




GO
ALTER TABLE [Component].[ComponentDates]  WITH CHECK ADD FOREIGN KEY([ComponentID])
REFERENCES [Component].[ComponentMaster] ([ComponentID])
GO
ALTER TABLE [Component].[ComponentMaster]  WITH CHECK ADD FOREIGN KEY([BenefittingCountry])
REFERENCES [System].[BenefitingCountry] ([BenefitingCountryID])
GO
ALTER TABLE [Component].[ComponentMaster]  WITH CHECK ADD FOREIGN KEY([BudgetCentreID])
REFERENCES [System].[BudgetCentre] ([BudgetCentreID])
GO
ALTER TABLE [Component].[ComponentMaster]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Component].[DeliveryChain]  WITH CHECK ADD FOREIGN KEY([ComponentID])
REFERENCES [Component].[ComponentMaster] ([ComponentID])
GO
ALTER TABLE [Component].[ImplementingOrganisation]  WITH CHECK ADD FOREIGN KEY([ComponentID])
REFERENCES [Component].[ComponentMaster] ([ComponentID])
GO
ALTER TABLE [Component].[InputSectorCodes]  WITH CHECK ADD FOREIGN KEY([ComponentID])
REFERENCES [Component].[ComponentMaster] ([ComponentID])
GO
ALTER TABLE [Component].[InputSectorCodes]  WITH CHECK ADD FOREIGN KEY([InputSectorCode])
REFERENCES [System].[InputSector] ([InputSectorCodeID])
GO
ALTER TABLE [Component].[Markers]  WITH CHECK ADD FOREIGN KEY([ComponentID])
REFERENCES [Component].[ComponentMaster] ([ComponentID])
GO
ALTER TABLE [Component].[Markers]  WITH CHECK ADD FOREIGN KEY([ComponentID])
REFERENCES [Component].[ComponentMaster] ([ComponentID])
GO
ALTER TABLE [Location].[GeoCodes]  WITH CHECK ADD FOREIGN KEY([CountryCodeID])
REFERENCES [Location].[CountryCodes] ([CountryCodeID])
GO
ALTER TABLE [Location].[GeoCodes]  WITH CHECK ADD FOREIGN KEY([LocationTypeID])
REFERENCES [Location].[LocationType] ([LocationTypeID])
GO
ALTER TABLE [Location].[GeoCodes]  WITH CHECK ADD FOREIGN KEY([PrecisionID])
REFERENCES [Location].[Precision] ([PrecisionID])
GO
ALTER TABLE [Location].[GeoCodes]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Location].[GeoCodes]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[AuditedFinancialStatements]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[ConditionalityReview]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[Deferral]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[Deferral]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[DSOMarkers]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[Evaluation]  WITH CHECK ADD FOREIGN KEY([EvaluationTypeID])
REFERENCES [System].[EvaluationType] ([EvaluationTypeID])
GO
ALTER TABLE [Project].[Evaluation]  WITH CHECK ADD FOREIGN KEY([ManagementOfEvaluation])
REFERENCES [System].[EvaluationManagement] ([EvaluationManagementID])
GO
ALTER TABLE [Project].[Evaluation]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[EvaluationDocuments]  WITH CHECK ADD FOREIGN KEY([EvaluationID])
REFERENCES [Project].[Evaluation] ([EvaluationID])
GO
ALTER TABLE [Project].[Markers]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[OverallRiskRating]  WITH CHECK ADD  CONSTRAINT [FK__OverallRi__RiskS__5804F8DB] FOREIGN KEY([RiskScore])
REFERENCES [System].[Risk] ([RiskValue])
GO
ALTER TABLE [Project].[OverallRiskRating] CHECK CONSTRAINT [FK__OverallRi__RiskS__5804F8DB]
GO
ALTER TABLE [Project].[Performance]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[ProjectBudgetCentreOrgUnit]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[ProjectDates]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[ProjectInfo]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[ProjectMaster]  WITH CHECK ADD FOREIGN KEY([BudgetCentreID])
REFERENCES [System].[BudgetCentre] ([BudgetCentreID])
GO
ALTER TABLE [Project].[ProjectMaster]  WITH CHECK ADD FOREIGN KEY([Stage])
REFERENCES [System].[Stage] ([StageID])
GO
ALTER TABLE [Project].[Reports]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[ReviewARScore]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[ReviewARScore]  WITH CHECK ADD FOREIGN KEY([ProjectID], [ReviewID])
REFERENCES [Project].[ReviewMaster] ([ProjectID], [ReviewID])
GO
ALTER TABLE [Project].[ReviewDeferral]  WITH CHECK ADD  CONSTRAINT [FK__ReviewDef__Proje__73B00EE2] FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[ReviewDeferral] CHECK CONSTRAINT [FK__ReviewDef__Proje__73B00EE2]
GO
ALTER TABLE [Project].[ReviewDeferral]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[ReviewDeferral]  WITH CHECK ADD  CONSTRAINT [FK__ReviewDef__Stage__72BBEAA9] FOREIGN KEY([StageID])
REFERENCES [Project].[ReviewStage] ([StageID])
GO
ALTER TABLE [Project].[ReviewDeferral] CHECK CONSTRAINT [FK__ReviewDef__Stage__72BBEAA9]
GO
ALTER TABLE [Project].[ReviewDeferral]  WITH CHECK ADD FOREIGN KEY([StageID])
REFERENCES [Project].[ReviewStage] ([StageID])
GO
ALTER TABLE [Project].[ReviewDocuments]  WITH CHECK ADD FOREIGN KEY([ProjectID], [ReviewID])
REFERENCES [Project].[ReviewMaster] ([ProjectID], [ReviewID])
GO
ALTER TABLE [Project].[ReviewExemption]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[ReviewExemption]  WITH CHECK ADD FOREIGN KEY([StageID])
REFERENCES [Project].[ReviewStage] ([StageID])
GO
ALTER TABLE [Project].[ReviewMaster]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[ReviewMaster]  WITH CHECK ADD FOREIGN KEY([RiskScore])
REFERENCES [System].[Risk] ([RiskValue])
GO
ALTER TABLE [Project].[ReviewMaster]  WITH CHECK ADD FOREIGN KEY([StageID])
REFERENCES [Project].[ReviewStage] ([StageID])
GO
ALTER TABLE [Project].[ReviewOutputs]  WITH CHECK ADD FOREIGN KEY([OutputScore])
REFERENCES [Project].[ReviewScore] ([OutputScore])
GO
ALTER TABLE [Project].[ReviewOutputs]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[ReviewOutputs]  WITH CHECK ADD FOREIGN KEY([ProjectID], [ReviewID])
REFERENCES [Project].[ReviewMaster] ([ProjectID], [ReviewID])
GO
ALTER TABLE [Project].[ReviewPCRScore]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[ReviewPCRScore]  WITH CHECK ADD FOREIGN KEY([ProjectID], [ReviewID])
REFERENCES [Project].[ReviewMaster] ([ProjectID], [ReviewID])
GO
ALTER TABLE [Project].[RiskDocument]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[RiskRegister]  WITH CHECK ADD FOREIGN KEY([GrossRisk])
REFERENCES [System].[Risk] ([RiskValue])
GO
ALTER TABLE [Project].[RiskRegister]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[RiskRegister]  WITH CHECK ADD FOREIGN KEY([ResidualRisk])
REFERENCES [System].[Risk] ([RiskValue])
GO
ALTER TABLE [Project].[RiskRegister]  WITH CHECK ADD FOREIGN KEY([RiskCategory])
REFERENCES [System].[RiskCategory] ([ID])
GO
ALTER TABLE [Project].[Team]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Project].[Team]  WITH CHECK ADD FOREIGN KEY([RoleID])
REFERENCES [System].[ProjectRole] ([ProjectRoleID])
GO
ALTER TABLE [Project].[Team]  WITH CHECK ADD FOREIGN KEY([RoleID])
REFERENCES [System].[ProjectRole] ([ProjectRoleID])
GO
ALTER TABLE [Project].[TeamExternal]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Results].[OutputIndicator]  WITH CHECK ADD FOREIGN KEY([ProjectOutputID])
REFERENCES [Results].[ProjectOutput] ([ID])
GO
ALTER TABLE [Results].[OutputIndicatorMilestones]  WITH CHECK ADD FOREIGN KEY([OutputIndicatorID])
REFERENCES [Results].[OutputIndicator] ([ID])
GO
ALTER TABLE [Results].[ProjectOutput]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [System].[FundingMechToSector]  WITH CHECK ADD FOREIGN KEY([SectorCode])
REFERENCES [System].[InputSector] ([InputSectorCodeID])
GO
ALTER TABLE [System].[Portfolio]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Workflow].[WorkflowMaster]  WITH CHECK ADD FOREIGN KEY([ProjectID])
REFERENCES [Project].[ProjectMaster] ([ProjectID])
GO
ALTER TABLE [Workflow].[WorkflowMaster]  WITH CHECK ADD FOREIGN KEY([StageID])
REFERENCES [Workflow].[WorkflowStage] ([StageID])
GO
ALTER TABLE [Workflow].[WorkflowMaster]  WITH CHECK ADD FOREIGN KEY([TaskID])
REFERENCES [Workflow].[WorkflowTask] ([TaskID])
GO
/****** Object:  StoredProcedure [System].[HourlyTasks]    Script Date: 10/12/2017 08:33:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [System].[HourlyTasks] 
	
AS
BEGIN
BEGIN TRAN



-- Sync sector description
UPDATE System.InputSector
SET 
	InputSectorCodeDescription = a.description
FROM 
	 EKSA87.AGRESSO.DBO.agldimvalue a
WHERE 
	a.attribute_id = ('54') AND a.client = 'DF'
AND 
	InputSectorCodeID = a.dim_value	
AND 
	InputSectorCodeDescription != a.description

-- Insert missing System Attributes
INSERT INTO System.BudgetCentre
SELECT dim_value, description, CASE Status WHEN 'N' THEN 'A' WHEN 'C' THEN 'C' END AS Status, GETDATE(), user_id FROM EKSA87.AGRESSO.DBO.agldimvalue WHERE attribute_id = ('C1') AND client = 'DF' 
AND dim_value NOT IN (SELECT BudgetCentreID FROM System.BudgetCentre)

--Sync description/status changes
UPDATE 
	System.BudgetCentre 
SET BudgetCentreDescription = description, Status =  CASE x.Status WHEN 'N' THEN 'A' WHEN 'C' THEN 'C' END, LastUpdated= GETDATE(), UserID = user_id
FROM EKSA87.AGRESSO.DBO.agldimvalue x 
WHERE 
	attribute_id = ('C1') 
AND 
	client = 'DF' 
AND 
	BudgetCentreID = x.dim_value
AND 
	BudgetCentreDescription ! = description

-- OR [Status] != x.status???


INSERT INTO System.FundingMech 
SELECT dim_value, description,CASE Status WHEN 'N' THEN 'A' WHEN 'C' THEN 'C' END AS Status, GETDATE(), user_id FROM EKSA87.AGRESSO.DBO.agldimvalue WHERE attribute_id = ('73') AND client = 'DF' and Status = 'N' 
AND dim_value NOT IN (SELECT FundingMechID FROM System.FundingMech)

--DELETE FROM [System].[FundingMechToSector]

--INSERT INTO [System].[FundingMechToSector]
--SELECT distinct input_sector, fund_type,'A', GETDATE(),'028984' FROM EKSA87.AGRESSO.[dbo].[ufundinputlink]
--WHERE input_sector IN (SELECT InputSectorCodeID FROM System.InputSector)


--DELETE FROM System.InputSector

INSERT INTO System.InputSector
SELECT dim_value, description, 
CASE Status WHEN 'N' THEN 'A' WHEN 'C' THEN 'C' END AS Status, GETDATE(), user_id FROM EKSA87.AGRESSO.DBO.agldimvalue WHERE attribute_id = ('54') AND client = 'DF'
AND dim_value NOT IN (SELECT InputSectorCodeID FROM System.InputSector)


--Insert missings
INSERT INTO System.Users
SELECT u.emp_no, u.display_name_forename_first, u.logon,
CASE u.IS_CURRENT
WHEN 'T' THEN 'A'
ELSE 'C' END AS Status,
GETDATE(), '028984'  FROM [identity].[identity].[identity] u WHERE 
 u.EMP_NO NOT IN (SELECT UserID FROM System.Users)
AND u.logon is not null


---------------------------------------------------------------------------------------------------------------
--Set people marked as leavers in IDENTITY to Status 'C' in AMP and use their Leaving Date as their End Date
---------------------------------------------------------------------------------------------------------------
UPDATE t
SET 
	t.Status = 'C',
	t.EndDate = i.LEAVING_DATE
FROM Project.Team AS t
INNER JOIN [Identity].IdentitySQL.identitysql i
ON t.TeamID = i.EMP_NO
Where i.IS_CURRENT = 'F' and t.Status = 'A'

---------------------------------------------------------------------------------------------------------------
--Tidy up the missing few from Identity. Set End Date as now.
---------------------------------------------------------------------------------------------------------------
UPDATE t
SET 
	t.Status = 'C',
	t.EndDate = GETDATE()
FROM Project.Team AS t
Where t.Status = 'C' and t.EndDate IS NULL

COMMIT
END
GO
/****** Object:  StoredProcedure [System].[Monitoring]    Script Date: 10/12/2017 08:33:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Create a New SQL Agent to run Execute this stored procedure every 10 minutes.

-- AMP Monitoring System
CREATE PROCEDURE [System].[Monitoring] 
      
AS
BEGIN


----------------------------------------------------------------------------------------------------------------------
-- Update Review dates to remove 1900
----------------------------------------------------------------------------------------------------------------------

--SELECT * FROM Project.Performance WHERE ARRequired='No' AND ARDueDate is not null and ARPromptDate is not null

--BEGIN TRAN
--UPDATE Project.Performance
--SET ARDueDate = null, ARPromptDate = null
--WHERE
--	ARRequired = 'No'

--UPDATE Project.Performance
--SET PCRDueDate = null, PCRPrompt = null
--WHERE
--	PCRRequired = 'No'


--UPDATE Project.Performance
--SET PCRDueDate = null, PCRPrompt = null
--WHERE
--	ProjectID IN (SELECT ProjectID FROM Project.ProjectMaster WHERE Stage = 7)


--UPDATE Project.Performance
--SET ARDueDate = null, ARPromptDate = null
--WHERE
--	ProjectID IN (SELECT ProjectID FROM Project.ProjectMaster WHERE Stage = 7)



--UPDATE Project.Performance
--SET PCRDueDate = null, PCRPrompt = null
--WHERE
--	ProjectID IN (SELECT ProjectID FROM Project.ProjectMaster WHERE Stage <5)


--UPDATE Project.Performance
--SET ARDueDate = null, ARPromptDate = null
--WHERE
--	ProjectID IN (SELECT ProjectID FROM Project.ProjectMaster WHERE Stage <5)
--COMMIT



---- Monitor changes to end dates
--BEGIN TRAN

--UPDATE 
--	Project.Performance 
--SET 
--	PCRDueDate = DATEADD(MONTH,3,pd.OperationalEndDate),
--	PCRPrompt = DATEADD(MONTH,-3,pd.OperationalEndDate)
--FROM 
--	Project.ProjectDates pd
--INNER JOIN 
--	Project.Performance p
--ON 
--	pd.ProjectID = p.ProjectID
--INNER JOIN 
--	Project.ProjectMaster pm
--ON
--	pd.ProjectID = pm.ProjectID
--WHERE 
--	p.PCRRequired = 'Yes'
--AND 
--	pm.Stage = 5
--AND 
--	DATEADD(MONTH,3,pd.OperationalEndDate) != p.PCRDueDate

--COMMIT

-- Monitor for projects going to stage 5 which need a AR
--BEGIN TRAN

--UPDATE 
--	Project.Performance 
--SET 
--	ARDueDate = DATEADD(MONTH,12,pd.ActualStartDate),
--	ARPromptDate = DATEADD(MONTH,9,pd.ActualStartDate)
----	ARRequired ='Yes'
--FROM 
--	Project.ProjectDates pd
--INNER JOIN 
--	Project.Performance p
--ON 
--	pd.ProjectID = p.ProjectID
--INNER JOIN 
--	Project.ProjectMaster pm
--ON
--	pd.ProjectID = pm.ProjectID
--WHERE 
--	pm.Stage = 5
--AND 
--	ARDueDate is null
--AND 
--	ARRequired = 'Yes'
--COMMIT

-- Monitor for projects going to stage 5 which need a AR
--BEGIN TRAN 
--UPDATE 
--	Project.Performance 
--SET 
--	PCRDueDate = DATEADD(MONTH,3,pd.OperationalEndDate),
--	PCRPrompt =  DATEADD(MONTH,-3,pd.OperationalEndDate),
--	PCRRequired = 'Yes'
--FROM 
--	Project.ProjectDates pd
--INNER JOIN 
--	Project.Performance p
--ON 
--	pd.ProjectID = p.ProjectID
--INNER JOIN 
--	Project.ProjectMaster pm
--ON
--	pd.ProjectID = pm.ProjectID
--WHERE 
--	pm.Stage = 5
--AND 
--	PCRDueDate is null
--AND 
--	PCRRequired = 'Yes'

--COMMIT
SELECT ''
END
GO
/****** Object:  StoredProcedure [System].[NightlyTasks]    Script Date: 10/12/2017 08:33:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [System].[NightlyTasks] 
      
AS
BEGIN
BEGIN TRAN
-- No Code...what do we want to sync nightly?
SELECT ''
COMMIT
END


GO
USE [master]
GO
ALTER DATABASE [AMP] SET  READ_WRITE 
GO
