USE [master]
GO
/****** Object:  Database [MyInventory]    Script Date: 11/6/2024 4:35:13 PM ******/
CREATE DATABASE [MyInventory]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MyInventory', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.HUYBLACK\MSSQL\DATA\MyInventory.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'MyInventory_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.HUYBLACK\MSSQL\DATA\MyInventory_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [MyInventory] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MyInventory].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MyInventory] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MyInventory] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MyInventory] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MyInventory] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MyInventory] SET ARITHABORT OFF 
GO
ALTER DATABASE [MyInventory] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [MyInventory] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MyInventory] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MyInventory] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MyInventory] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MyInventory] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MyInventory] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MyInventory] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MyInventory] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MyInventory] SET  ENABLE_BROKER 
GO
ALTER DATABASE [MyInventory] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MyInventory] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MyInventory] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MyInventory] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MyInventory] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MyInventory] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [MyInventory] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MyInventory] SET RECOVERY FULL 
GO
ALTER DATABASE [MyInventory] SET  MULTI_USER 
GO
ALTER DATABASE [MyInventory] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MyInventory] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MyInventory] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MyInventory] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [MyInventory] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [MyInventory] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'MyInventory', N'ON'
GO
ALTER DATABASE [MyInventory] SET QUERY_STORE = ON
GO
ALTER DATABASE [MyInventory] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [MyInventory]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 11/6/2024 4:35:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 11/6/2024 4:35:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 11/6/2024 4:35:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 11/6/2024 4:35:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 11/6/2024 4:35:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 11/6/2024 4:35:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 11/6/2024 4:35:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 11/6/2024 4:35:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 11/6/2024 4:35:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](100) NULL,
	[SupplierId] [int] NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 11/6/2024 4:35:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[OrderDate] [datetime2](7) NOT NULL,
	[SupplierId] [int] NOT NULL,
	[OrderName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetail]    Script Date: 11/6/2024 4:35:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetail](
	[OrderDetailId] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[UnitPrice] [decimal](18, 2) NOT NULL,
	[Quantity] [int] NOT NULL,
	[OrderDetailName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_OrderDetail] PRIMARY KEY CLUSTERED 
(
	[OrderDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 11/6/2024 4:35:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[StockQuantity] [int] NOT NULL,
	[ReorderLevel] [int] NOT NULL,
	[CategoryID] [int] NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Supplier]    Script Date: 11/6/2024 4:35:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Supplier](
	[SupplierId] [int] IDENTITY(1,1) NOT NULL,
	[SupplierName] [nvarchar](100) NOT NULL,
	[ContactInfo] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED 
(
	[SupplierId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241009094109_DbInit', N'8.0.10')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241016064951_AddAuthentication', N'8.0.10')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241017162318_dborder', N'8.0.10')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241106090442_DbInit', N'8.0.10')
GO
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([CategoryId], [CategoryName], [Description], [SupplierId]) VALUES (1, N'Laptop Accessories', N'All accessories related to laptops, including bags, stands, and cooling pads', 1)
INSERT [dbo].[Category] ([CategoryId], [CategoryName], [Description], [SupplierId]) VALUES (2, N'Gaming Laptops', N'High-performance laptops designed for gaming', 2)
INSERT [dbo].[Category] ([CategoryId], [CategoryName], [Description], [SupplierId]) VALUES (3, N'Business Laptops', N'Laptops optimized for professional use', 3)
INSERT [dbo].[Category] ([CategoryId], [CategoryName], [Description], [SupplierId]) VALUES (4, N'Laptop Chargers', N'Chargers and power adapters for various laptop models', 4)
INSERT [dbo].[Category] ([CategoryId], [CategoryName], [Description], [SupplierId]) VALUES (5, N'Laptop Parts', N'Spare parts like batteries, screens, and motherboards for laptops', 5)
INSERT [dbo].[Category] ([CategoryId], [CategoryName], [Description], [SupplierId]) VALUES (6, N'Ultrabooks', N'Thin and light laptops with high performance', 6)
INSERT [dbo].[Category] ([CategoryId], [CategoryName], [Description], [SupplierId]) VALUES (7, N'2-in-1 Laptops', N'Laptops that can convert into tablets', 7)
INSERT [dbo].[Category] ([CategoryId], [CategoryName], [Description], [SupplierId]) VALUES (8, N'Laptop Bags', N'Specialized bags for carrying laptops securely', 8)
INSERT [dbo].[Category] ([CategoryId], [CategoryName], [Description], [SupplierId]) VALUES (9, N'Laptop Stands', N'Ergonomic stands designed for better laptop usage', 9)
INSERT [dbo].[Category] ([CategoryId], [CategoryName], [Description], [SupplierId]) VALUES (10, N'Laptop Cooling Pads', N'Cooling pads designed to reduce laptop overheating', 10)
SET IDENTITY_INSERT [dbo].[Category] OFF
GO
SET IDENTITY_INSERT [dbo].[Order] ON 

INSERT [dbo].[Order] ([OrderId], [OrderDate], [SupplierId], [OrderName]) VALUES (1, CAST(N'2024-11-01T00:00:00.0000000' AS DateTime2), 1, N'Order for Laptop Accessories')
INSERT [dbo].[Order] ([OrderId], [OrderDate], [SupplierId], [OrderName]) VALUES (2, CAST(N'2024-11-02T00:00:00.0000000' AS DateTime2), 2, N'Order for Gaming Laptops')
INSERT [dbo].[Order] ([OrderId], [OrderDate], [SupplierId], [OrderName]) VALUES (3, CAST(N'2024-11-03T00:00:00.0000000' AS DateTime2), 3, N'Order for Business Laptops')
INSERT [dbo].[Order] ([OrderId], [OrderDate], [SupplierId], [OrderName]) VALUES (4, CAST(N'2024-11-04T00:00:00.0000000' AS DateTime2), 4, N'Order for Laptop Chargers')
INSERT [dbo].[Order] ([OrderId], [OrderDate], [SupplierId], [OrderName]) VALUES (5, CAST(N'2024-11-05T00:00:00.0000000' AS DateTime2), 5, N'Order for Laptop Parts')
INSERT [dbo].[Order] ([OrderId], [OrderDate], [SupplierId], [OrderName]) VALUES (6, CAST(N'2024-11-06T00:00:00.0000000' AS DateTime2), 6, N'Order for Ultrabooks')
INSERT [dbo].[Order] ([OrderId], [OrderDate], [SupplierId], [OrderName]) VALUES (7, CAST(N'2024-11-07T00:00:00.0000000' AS DateTime2), 7, N'Order for 2-in-1 Laptops')
INSERT [dbo].[Order] ([OrderId], [OrderDate], [SupplierId], [OrderName]) VALUES (8, CAST(N'2024-11-08T00:00:00.0000000' AS DateTime2), 8, N'Order for Laptop Stands')
INSERT [dbo].[Order] ([OrderId], [OrderDate], [SupplierId], [OrderName]) VALUES (9, CAST(N'2024-11-09T00:00:00.0000000' AS DateTime2), 9, N'Order for Cooling Pads')
INSERT [dbo].[Order] ([OrderId], [OrderDate], [SupplierId], [OrderName]) VALUES (10, CAST(N'2024-11-10T00:00:00.0000000' AS DateTime2), 10, N'Order for Laptop Sleeves')
SET IDENTITY_INSERT [dbo].[Order] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderDetail] ON 

INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (6, 1, 2, CAST(29.99 AS Decimal(18, 2)), 2, N'Laptop Bag A Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (7, 1, 2, CAST(39.99 AS Decimal(18, 2)), 3, N'Laptop Bag B Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (8, 1, 21, CAST(119.99 AS Decimal(18, 2)), 2, N'Laptop Docking Station Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (9, 1, 22, CAST(29.99 AS Decimal(18, 2)), 3, N'USB-C Hub for Laptops Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (10, 2, 3, CAST(1499.99 AS Decimal(18, 2)), 1, N'Gaming Laptop X1 Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (11, 2, 4, CAST(1799.99 AS Decimal(18, 2)), 1, N'Gaming Laptop X2 Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (12, 2, 23, CAST(79.99 AS Decimal(18, 2)), 4, N'Portable SSD 500GB Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (13, 2, 24, CAST(149.99 AS Decimal(18, 2)), 2, N'Portable SSD 1TB Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (14, 3, 5, CAST(899.99 AS Decimal(18, 2)), 2, N'Business Laptop Pro Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (15, 3, 6, CAST(649.99 AS Decimal(18, 2)), 3, N'Business Laptop Lite Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (16, 3, 25, CAST(12.99 AS Decimal(18, 2)), 5, N'Laptop Screen Protector Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (17, 3, 26, CAST(14.99 AS Decimal(18, 2)), 3, N'Wireless Mouse Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (18, 4, 7, CAST(19.99 AS Decimal(18, 2)), 5, N'Laptop Charger A Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (19, 4, 8, CAST(24.99 AS Decimal(18, 2)), 4, N'Laptop Charger B Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (20, 4, 27, CAST(29.99 AS Decimal(18, 2)), 5, N'Wireless Keyboard Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (21, 4, 28, CAST(39.99 AS Decimal(18, 2)), 4, N'Laptop Stand with Storage Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (22, 5, 9, CAST(49.99 AS Decimal(18, 2)), 6, N'Laptop Battery Pack Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (23, 5, 10, CAST(99.99 AS Decimal(18, 2)), 3, N'Laptop Screen 15" Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (24, 5, 29, CAST(69.99 AS Decimal(18, 2)), 2, N'External Laptop Battery Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (25, 5, 30, CAST(49.99 AS Decimal(18, 2)), 3, N'Bluetooth Speaker for Laptop Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (26, 6, 11, CAST(1299.99 AS Decimal(18, 2)), 1, N'Ultrabook Slim Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (27, 6, 12, CAST(1399.99 AS Decimal(18, 2)), 2, N'Ultrabook Pro Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (28, 6, 31, CAST(19.99 AS Decimal(18, 2)), 4, N'Laptop Anti-Theft Lock Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (29, 7, 13, CAST(799.99 AS Decimal(18, 2)), 4, N'2-in-1 Convertible Laptop Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (30, 7, 14, CAST(849.99 AS Decimal(18, 2)), 3, N'2-in-1 Hybrid Laptop Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (31, 8, 15, CAST(19.99 AS Decimal(18, 2)), 8, N'Laptop Stand Adjustable Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (32, 8, 16, CAST(29.99 AS Decimal(18, 2)), 5, N'Laptop Stand Deluxe Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (33, 8, 17, CAST(24.99 AS Decimal(18, 2)), 3, N'Cooling Pad Pro Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (34, 8, 18, CAST(19.99 AS Decimal(18, 2)), 4, N'Cooling Pad Slim Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (35, 9, 19, CAST(34.99 AS Decimal(18, 2)), 2, N'Laptop Cooling Stand Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (36, 9, 20, CAST(15.99 AS Decimal(18, 2)), 6, N'Laptop Sleeve A Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (37, 10, 21, CAST(119.99 AS Decimal(18, 2)), 2, N'Laptop Docking Station Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (38, 10, 22, CAST(29.99 AS Decimal(18, 2)), 3, N'USB-C Hub for Laptops Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (39, 10, 23, CAST(79.99 AS Decimal(18, 2)), 4, N'Portable SSD 500GB Order Detail')
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [UnitPrice], [Quantity], [OrderDetailName]) VALUES (40, 10, 24, CAST(149.99 AS Decimal(18, 2)), 2, N'Portable SSD 1TB Order Detail')
SET IDENTITY_INSERT [dbo].[OrderDetail] OFF
GO
SET IDENTITY_INSERT [dbo].[Product] ON 

INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (2, N'Laptop Bag A', N'A stylish and durable laptop bag', CAST(29.99 AS Decimal(18, 2)), 50, 10, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (3, N'Laptop Bag B', N'Water-resistant laptop bag with multiple compartments', CAST(39.99 AS Decimal(18, 2)), 30, 5, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (4, N'Gaming Laptop X1', N'High-performance gaming laptop with RGB keyboard', CAST(1499.99 AS Decimal(18, 2)), 20, 5, 2)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (5, N'Gaming Laptop X2', N'Premium gaming laptop with advanced graphics', CAST(1799.99 AS Decimal(18, 2)), 15, 3, 2)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (6, N'Business Laptop Pro', N'Business laptop with a professional design and security features', CAST(899.99 AS Decimal(18, 2)), 40, 8, 3)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (7, N'Business Laptop Lite', N'Affordable and efficient business laptop', CAST(649.99 AS Decimal(18, 2)), 60, 12, 3)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (8, N'Laptop Charger A', N'Compatible charger for multiple laptop models', CAST(19.99 AS Decimal(18, 2)), 100, 20, 4)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (9, N'Laptop Charger B', N'Universal laptop charger with fast charging support', CAST(24.99 AS Decimal(18, 2)), 80, 15, 4)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (10, N'Laptop Battery Pack', N'High-quality replacement battery for various laptop models', CAST(49.99 AS Decimal(18, 2)), 35, 7, 5)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (11, N'Laptop Screen 15"', N'Replacement screen for 15-inch laptops', CAST(99.99 AS Decimal(18, 2)), 25, 5, 5)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (12, N'Ultrabook Slim', N'Ultra-thin and lightweight laptop with powerful performance', CAST(1299.99 AS Decimal(18, 2)), 10, 2, 6)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (13, N'Ultrabook Pro', N'Premium ultrabook with fast SSD and long battery life', CAST(1399.99 AS Decimal(18, 2)), 12, 3, 6)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (14, N'2-in-1 Convertible Laptop', N'Laptop that transforms into a tablet with touch screen', CAST(799.99 AS Decimal(18, 2)), 45, 9, 7)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (15, N'2-in-1 Hybrid Laptop', N'Laptop-tablet hybrid for maximum versatility', CAST(849.99 AS Decimal(18, 2)), 38, 8, 7)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (16, N'Laptop Stand Adjustable', N'Adjustable laptop stand for ergonomic use', CAST(19.99 AS Decimal(18, 2)), 150, 30, 8)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (17, N'Laptop Stand Deluxe', N'Premium adjustable laptop stand with cooling feature', CAST(29.99 AS Decimal(18, 2)), 60, 15, 8)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (18, N'Cooling Pad Pro', N'Cooling pad with multiple fans for better heat dissipation', CAST(24.99 AS Decimal(18, 2)), 120, 25, 9)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (19, N'Cooling Pad Slim', N'Slim and portable laptop cooling pad', CAST(19.99 AS Decimal(18, 2)), 80, 16, 9)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (20, N'Laptop Cooling Stand', N'Laptop stand with built-in cooling system', CAST(34.99 AS Decimal(18, 2)), 55, 11, 9)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (21, N'Laptop Sleeve A', N'Soft and padded sleeve for 13-inch laptops', CAST(15.99 AS Decimal(18, 2)), 70, 14, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (22, N'Laptop Sleeve B', N'Stylish sleeve for 15-inch laptops with extra compartments', CAST(22.99 AS Decimal(18, 2)), 45, 9, 1)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (23, N'Laptop Docking Station', N'Docking station for multiple monitor support', CAST(119.99 AS Decimal(18, 2)), 30, 6, 10)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (24, N'USB-C Hub for Laptops', N'USB-C hub with multiple ports for connectivity', CAST(29.99 AS Decimal(18, 2)), 100, 20, 10)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (25, N'Portable SSD 500GB', N'500GB portable SSD for extra storage and fast data transfer', CAST(79.99 AS Decimal(18, 2)), 75, 15, 5)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (26, N'Portable SSD 1TB', N'1TB portable SSD with fast read/write speeds', CAST(149.99 AS Decimal(18, 2)), 60, 12, 5)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (27, N'Laptop Screen Protector', N'Screen protector to prevent scratches and dust', CAST(12.99 AS Decimal(18, 2)), 90, 18, 5)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (28, N'Wireless Mouse', N'Ergonomic wireless mouse for laptop use', CAST(14.99 AS Decimal(18, 2)), 150, 30, 8)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (29, N'Wireless Keyboard', N'Compact wireless keyboard designed for laptop use', CAST(29.99 AS Decimal(18, 2)), 100, 20, 8)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (30, N'Laptop Stand with Storage', N'Laptop stand with built-in storage compartments', CAST(39.99 AS Decimal(18, 2)), 55, 11, 8)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (31, N'External Laptop Battery', N'External battery for extended laptop usage', CAST(69.99 AS Decimal(18, 2)), 40, 8, 5)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (32, N'Bluetooth Speaker for Laptop', N'Portable Bluetooth speaker for laptop audio', CAST(49.99 AS Decimal(18, 2)), 60, 12, 8)
INSERT [dbo].[Product] ([ProductID], [ProductName], [Description], [Price], [StockQuantity], [ReorderLevel], [CategoryID]) VALUES (33, N'Laptop Anti-Theft Lock', N'Security lock to protect your laptop from theft', CAST(19.99 AS Decimal(18, 2)), 80, 16, 5)
SET IDENTITY_INSERT [dbo].[Product] OFF
GO
SET IDENTITY_INSERT [dbo].[Supplier] ON 

INSERT [dbo].[Supplier] ([SupplierId], [SupplierName], [ContactInfo]) VALUES (1, N'Supplier 1', N'contact1@example.com')
INSERT [dbo].[Supplier] ([SupplierId], [SupplierName], [ContactInfo]) VALUES (2, N'Supplier 2', N'contact2@example.com')
INSERT [dbo].[Supplier] ([SupplierId], [SupplierName], [ContactInfo]) VALUES (3, N'Supplier 3', N'contact3@example.com')
INSERT [dbo].[Supplier] ([SupplierId], [SupplierName], [ContactInfo]) VALUES (4, N'Supplier 4', N'contact4@example.com')
INSERT [dbo].[Supplier] ([SupplierId], [SupplierName], [ContactInfo]) VALUES (5, N'Supplier 5', N'contact5@example.com')
INSERT [dbo].[Supplier] ([SupplierId], [SupplierName], [ContactInfo]) VALUES (6, N'Supplier 6', N'contact6@example.com')
INSERT [dbo].[Supplier] ([SupplierId], [SupplierName], [ContactInfo]) VALUES (7, N'Supplier 7', N'contact7@example.com')
INSERT [dbo].[Supplier] ([SupplierId], [SupplierName], [ContactInfo]) VALUES (8, N'Supplier 8', N'contact8@example.com')
INSERT [dbo].[Supplier] ([SupplierId], [SupplierName], [ContactInfo]) VALUES (9, N'Supplier 9', N'contact9@example.com')
INSERT [dbo].[Supplier] ([SupplierId], [SupplierName], [ContactInfo]) VALUES (10, N'Supplier 10', N'contact10@example.com')
INSERT [dbo].[Supplier] ([SupplierId], [SupplierName], [ContactInfo]) VALUES (11, N'Supplier 11', N'contact11@example.com')
INSERT [dbo].[Supplier] ([SupplierId], [SupplierName], [ContactInfo]) VALUES (12, N'Supplier 12', N'contact12@example.com')
INSERT [dbo].[Supplier] ([SupplierId], [SupplierName], [ContactInfo]) VALUES (13, N'Supplier 13', N'contact13@example.com')
INSERT [dbo].[Supplier] ([SupplierId], [SupplierName], [ContactInfo]) VALUES (14, N'Supplier 14', N'contact14@example.com')
INSERT [dbo].[Supplier] ([SupplierId], [SupplierName], [ContactInfo]) VALUES (15, N'Supplier 15', N'contact15@example.com')
INSERT [dbo].[Supplier] ([SupplierId], [SupplierName], [ContactInfo]) VALUES (16, N'Supplier 16', N'contact16@example.com')
INSERT [dbo].[Supplier] ([SupplierId], [SupplierName], [ContactInfo]) VALUES (17, N'Supplier 17', N'contact17@example.com')
INSERT [dbo].[Supplier] ([SupplierId], [SupplierName], [ContactInfo]) VALUES (18, N'Supplier 18', N'contact18@example.com')
INSERT [dbo].[Supplier] ([SupplierId], [SupplierName], [ContactInfo]) VALUES (19, N'Supplier 19', N'contact19@example.com')
INSERT [dbo].[Supplier] ([SupplierId], [SupplierName], [ContactInfo]) VALUES (20, N'Supplier 20', N'contact20@example.com')
SET IDENTITY_INSERT [dbo].[Supplier] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 11/6/2024 4:35:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 11/6/2024 4:35:14 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 11/6/2024 4:35:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 11/6/2024 4:35:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 11/6/2024 4:35:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 11/6/2024 4:35:14 PM ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 11/6/2024 4:35:14 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Category_SupplierId]    Script Date: 11/6/2024 4:35:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_Category_SupplierId] ON [dbo].[Category]
(
	[SupplierId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Order_SupplierId]    Script Date: 11/6/2024 4:35:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_Order_SupplierId] ON [dbo].[Order]
(
	[SupplierId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderDetail_OrderId]    Script Date: 11/6/2024 4:35:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_OrderDetail_OrderId] ON [dbo].[OrderDetail]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderDetail_ProductId]    Script Date: 11/6/2024 4:35:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_OrderDetail_ProductId] ON [dbo].[OrderDetail]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Product_CategoryID]    Script Date: 11/6/2024 4:35:14 PM ******/
CREATE NONCLUSTERED INDEX [IX_Product_CategoryID] ON [dbo].[Product]
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Order] ADD  DEFAULT (N'') FOR [OrderName]
GO
ALTER TABLE [dbo].[OrderDetail] ADD  DEFAULT (N'') FOR [OrderDetailName]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Category]  WITH CHECK ADD  CONSTRAINT [FK_Category_Supplier_SupplierId] FOREIGN KEY([SupplierId])
REFERENCES [dbo].[Supplier] ([SupplierId])
GO
ALTER TABLE [dbo].[Category] CHECK CONSTRAINT [FK_Category_Supplier_SupplierId]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Supplier_SupplierId] FOREIGN KEY([SupplierId])
REFERENCES [dbo].[Supplier] ([SupplierId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Supplier_SupplierId]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetail_Order_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK_OrderDetail_Order_OrderId]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetail_Product_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK_OrderDetail_Product_ProductId]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Category_CategoryID] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Category] ([CategoryId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Category_CategoryID]
GO
USE [master]
GO
ALTER DATABASE [MyInventory] SET  READ_WRITE 
GO
