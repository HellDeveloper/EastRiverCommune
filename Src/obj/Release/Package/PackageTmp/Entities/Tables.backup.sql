USE [EastRiverCommune]
GO
/****** Object:  ForeignKey [FK_Commodity_Category]    Script Date: 06/18/2015 01:55:44 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Commodity_Category]') AND parent_object_id = OBJECT_ID(N'[dbo].[Commodity]'))
ALTER TABLE [dbo].[Commodity] DROP CONSTRAINT [FK_Commodity_Category]
GO
/****** Object:  ForeignKey [FK_Activity_Category]    Script Date: 06/18/2015 01:55:44 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Activity_Category]') AND parent_object_id = OBJECT_ID(N'[dbo].[Activity]'))
ALTER TABLE [dbo].[Activity] DROP CONSTRAINT [FK_Activity_Category]
GO
/****** Object:  ForeignKey [FK_OrderItem_Commodity]    Script Date: 06/18/2015 01:55:45 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_Commodity]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK_OrderItem_Commodity]
GO
/****** Object:  ForeignKey [FK_OrderItem_OrderList]    Script Date: 06/18/2015 01:55:45 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_OrderList]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK_OrderItem_OrderList]
GO
/****** Object:  Table [dbo].[OrderItem]    Script Date: 06/18/2015 01:55:45 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_Commodity]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK_OrderItem_Commodity]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_OrderList]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK_OrderItem_OrderList]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_OrderItem_Enable]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [DF_OrderItem_Enable]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_OrderItem_WhenModify]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [DF_OrderItem_WhenModify]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_OrderItem_Price]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [DF_OrderItem_Price]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_OrderItem_Count]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [DF_OrderItem_Count]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_OrderItem_Total]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [DF_OrderItem_Total]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND type in (N'U'))
DROP TABLE [dbo].[OrderItem]
GO
/****** Object:  Table [dbo].[Activity]    Script Date: 06/18/2015 01:55:44 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Activity_Category]') AND parent_object_id = OBJECT_ID(N'[dbo].[Activity]'))
ALTER TABLE [dbo].[Activity] DROP CONSTRAINT [FK_Activity_Category]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Activity]') AND type in (N'U'))
DROP TABLE [dbo].[Activity]
GO
/****** Object:  Table [dbo].[Commodity]    Script Date: 06/18/2015 01:55:44 ******/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Commodity_Category]') AND parent_object_id = OBJECT_ID(N'[dbo].[Commodity]'))
ALTER TABLE [dbo].[Commodity] DROP CONSTRAINT [FK_Commodity_Category]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Commodity_Enable]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Commodity] DROP CONSTRAINT [DF_Commodity_Enable]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Commodity_WhenModify]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Commodity] DROP CONSTRAINT [DF_Commodity_WhenModify]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Commodity_Price]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Commodity] DROP CONSTRAINT [DF_Commodity_Price]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Commodity]') AND type in (N'U'))
DROP TABLE [dbo].[Commodity]
GO
/****** Object:  Table [dbo].[Directory]    Script Date: 06/18/2015 01:55:43 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Directory_Enable]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Directory] DROP CONSTRAINT [DF_Directory_Enable]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Directory_WhenModify]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Directory] DROP CONSTRAINT [DF_Directory_WhenModify]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Directory]') AND type in (N'U'))
DROP TABLE [dbo].[Directory]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 06/18/2015 01:55:42 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Category_Enable]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Category] DROP CONSTRAINT [DF_Category_Enable]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Category_WhenModify]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Category] DROP CONSTRAINT [DF_Category_WhenModify]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Category]') AND type in (N'U'))
DROP TABLE [dbo].[Category]
GO
/****** Object:  Table [dbo].[OrderList]    Script Date: 06/18/2015 01:55:42 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_OrderList_Enable]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[OrderList] DROP CONSTRAINT [DF_OrderList_Enable]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_OrderList_WhenModify]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[OrderList] DROP CONSTRAINT [DF_OrderList_WhenModify]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderList]') AND type in (N'U'))
DROP TABLE [dbo].[OrderList]
GO
/****** Object:  Table [dbo].[Temp]    Script Date: 06/18/2015 01:55:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Temp]') AND type in (N'U'))
DROP TABLE [dbo].[Temp]
GO
/****** Object:  Table [dbo].[Temp]    Script Date: 06/18/2015 01:55:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Temp]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Temp](
	[ID] [nvarchar](25) COLLATE Chinese_PRC_CS_AS NOT NULL,
	[Enable] [bit] NULL,
	[WhenModify] [datetime] NULL,
	[Type] [nvarchar](50) COLLATE Chinese_PRC_CS_AS NULL,
	[Who] [nvarchar](50) COLLATE Chinese_PRC_CS_AS NULL,
	[Data] [nvarchar](max) COLLATE Chinese_PRC_CS_AS NULL,
 CONSTRAINT [PK_Temp] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[OrderList]    Script Date: 06/18/2015 01:55:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderList]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[OrderList](
	[ID] [nvarchar](25) COLLATE Chinese_PRC_CS_AS NOT NULL,
	[Enable] [bit] NOT NULL CONSTRAINT [DF_OrderList_Enable]  DEFAULT ((1)),
	[WhenModify] [datetime] NOT NULL CONSTRAINT [DF_OrderList_WhenModify]  DEFAULT (sysdatetime()),
	[DeliveryDate] [date] NOT NULL,
	[DeliveryManner] [nvarchar](50) COLLATE Chinese_PRC_CS_AS NULL,
	[Man] [nvarchar](50) COLLATE Chinese_PRC_CS_AS NULL,
	[Phone] [nvarchar](50) COLLATE Chinese_PRC_CS_AS NULL,
	[Address] [nvarchar](50) COLLATE Chinese_PRC_CS_AS NULL,
	[Remark] [nvarchar](50) COLLATE Chinese_PRC_CS_AS NULL,
 CONSTRAINT [PK_OrderList] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderList', N'COLUMN',N'DeliveryDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提货日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderList', @level2type=N'COLUMN',@level2name=N'DeliveryDate'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderList', N'COLUMN',N'DeliveryManner'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'提货方式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderList', @level2type=N'COLUMN',@level2name=N'DeliveryManner'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderList', N'COLUMN',N'Man'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'联系人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderList', @level2type=N'COLUMN',@level2name=N'Man'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderList', N'COLUMN',N'Phone'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'联系人电话号码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderList', @level2type=N'COLUMN',@level2name=N'Phone'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderList', N'COLUMN',N'Address'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收货地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderList', @level2type=N'COLUMN',@level2name=N'Address'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderList', N'COLUMN',N'Remark'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderList', @level2type=N'COLUMN',@level2name=N'Remark'
GO
/****** Object:  Table [dbo].[Category]    Script Date: 06/18/2015 01:55:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Category]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Category](
	[ID] [nvarchar](25) COLLATE Chinese_PRC_CS_AS NOT NULL,
	[Enable] [bit] NOT NULL CONSTRAINT [DF_Category_Enable]  DEFAULT ((1)),
	[WhenModify] [datetime] NOT NULL CONSTRAINT [DF_Category_WhenModify]  DEFAULT (sysdatetime()),
	[Name] [nvarchar](50) COLLATE Chinese_PRC_CS_AS NOT NULL,
	[Type] [int] NOT NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Category', N'COLUMN',N'Type'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0:Commodity  1:Activity' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Category', @level2type=N'COLUMN',@level2name=N'Type'
GO
INSERT [dbo].[Category] ([ID], [Enable], [WhenModify], [Name], [Type]) VALUES (N'1', 1, CAST(0x0000A49A000A08C8 AS DateTime), N'优惠套餐', 0)
INSERT [dbo].[Category] ([ID], [Enable], [WhenModify], [Name], [Type]) VALUES (N'2', 1, CAST(0x0000A49A000A187A AS DateTime), N'肉类', 0)
INSERT [dbo].[Category] ([ID], [Enable], [WhenModify], [Name], [Type]) VALUES (N'3', 1, CAST(0x0000A49A000A2AEA AS DateTime), N'海鲜', 0)
INSERT [dbo].[Category] ([ID], [Enable], [WhenModify], [Name], [Type]) VALUES (N'4', 1, CAST(0x0000A49A000A3741 AS DateTime), N'蔬菜', 0)
INSERT [dbo].[Category] ([ID], [Enable], [WhenModify], [Name], [Type]) VALUES (N'5', 1, CAST(0x0000A49A000A40D2 AS DateTime), N'主食', 0)
INSERT [dbo].[Category] ([ID], [Enable], [WhenModify], [Name], [Type]) VALUES (N'6', 1, CAST(0x0000A49A000A4B64 AS DateTime), N'调料', 0)
INSERT [dbo].[Category] ([ID], [Enable], [WhenModify], [Name], [Type]) VALUES (N'7', 1, CAST(0x0000A49A000A5C04 AS DateTime), N'工具', 0)
INSERT [dbo].[Category] ([ID], [Enable], [WhenModify], [Name], [Type]) VALUES (N'8', 1, CAST(0x0000A49A000A6C28 AS DateTime), N'水果', 0)
/****** Object:  Table [dbo].[Directory]    Script Date: 06/18/2015 01:55:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Directory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Directory](
	[ID] [nvarchar](25) COLLATE Chinese_PRC_CS_AS NOT NULL,
	[Enable] [bit] NOT NULL CONSTRAINT [DF_Directory_Enable]  DEFAULT ((1)),
	[WhenModify] [datetime] NOT NULL CONSTRAINT [DF_Directory_WhenModify]  DEFAULT (sysdatetime()),
	[Name] [nvarchar](25) COLLATE Chinese_PRC_CS_AS NOT NULL,
	[Path] [nvarchar](max) COLLATE Chinese_PRC_CS_AS NULL,
	[Description] [nvarchar](max) COLLATE Chinese_PRC_CS_AS NULL,
 CONSTRAINT [PK_Directory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Directory', N'COLUMN',N'Name'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Directory', @level2type=N'COLUMN',@level2name=N'Name'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Directory', N'COLUMN',N'Path'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Directory', @level2type=N'COLUMN',@level2name=N'Path'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Directory', N'COLUMN',N'Description'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Directory', @level2type=N'COLUMN',@level2name=N'Description'
GO
INSERT [dbo].[Directory] ([ID], [Enable], [WhenModify], [Name], [Path], [Description]) VALUES (N'Commodity', 1, CAST(0x0000A4AB0180EE42 AS DateTime), N'Commodity', N'E:/Image/Commodity/', N'商品图片')
/****** Object:  Table [dbo].[Commodity]    Script Date: 06/18/2015 01:55:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Commodity]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Commodity](
	[ID] [nvarchar](25) COLLATE Chinese_PRC_CS_AS NOT NULL,
	[Enable] [bit] NOT NULL CONSTRAINT [DF_Commodity_Enable]  DEFAULT ((1)),
	[WhenModify] [datetime] NOT NULL CONSTRAINT [DF_Commodity_WhenModify]  DEFAULT (sysdatetime()),
	[Name] [nvarchar](50) COLLATE Chinese_PRC_CS_AS NULL,
	[CategoryID] [nvarchar](25) COLLATE Chinese_PRC_CS_AS NULL,
	[Price] [decimal](10, 2) NOT NULL CONSTRAINT [DF_Commodity_Price]  DEFAULT ((0)),
	[Unit] [nvarchar](10) COLLATE Chinese_PRC_CS_AS NULL,
	[PictruePath] [nvarchar](max) COLLATE Chinese_PRC_CS_AS NULL,
	[Description] [nvarchar](max) COLLATE Chinese_PRC_CS_AS NULL,
 CONSTRAINT [PK_Commodity] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
INSERT [dbo].[Commodity] ([ID], [Enable], [WhenModify], [Name], [CategoryID], [Price], [Unit], [PictruePath], [Description]) VALUES (N'1', 1, CAST(0x0000A4A200746C63 AS DateTime), N'牛肉', N'2', CAST(10.00 AS Decimal(10, 2)), N'斤', NULL, NULL)
INSERT [dbo].[Commodity] ([ID], [Enable], [WhenModify], [Name], [CategoryID], [Price], [Unit], [PictruePath], [Description]) VALUES (N'10', 1, CAST(0x0000A4A200746C63 AS DateTime), N'白萝卜', N'4', CAST(16.00 AS Decimal(10, 2)), N'斤', NULL, NULL)
INSERT [dbo].[Commodity] ([ID], [Enable], [WhenModify], [Name], [CategoryID], [Price], [Unit], [PictruePath], [Description]) VALUES (N'11', 1, CAST(0x0000A4A200746C63 AS DateTime), N'黑萝卜', N'4', CAST(16.00 AS Decimal(10, 2)), N'个', NULL, NULL)
INSERT [dbo].[Commodity] ([ID], [Enable], [WhenModify], [Name], [CategoryID], [Price], [Unit], [PictruePath], [Description]) VALUES (N'12', 1, CAST(0x0000A4A200746C63 AS DateTime), N'小黑菜', N'4', CAST(16.00 AS Decimal(10, 2)), N'颗', NULL, NULL)
INSERT [dbo].[Commodity] ([ID], [Enable], [WhenModify], [Name], [CategoryID], [Price], [Unit], [PictruePath], [Description]) VALUES (N'13', 1, CAST(0x0000A4A200746C63 AS DateTime), N'大黑菜', N'4', CAST(16.00 AS Decimal(10, 2)), N'颗', NULL, NULL)
INSERT [dbo].[Commodity] ([ID], [Enable], [WhenModify], [Name], [CategoryID], [Price], [Unit], [PictruePath], [Description]) VALUES (N'14', 1, CAST(0x0000A4A200746C63 AS DateTime), N'香蕉', N'8', CAST(7.00 AS Decimal(10, 2)), NULL, NULL, NULL)
INSERT [dbo].[Commodity] ([ID], [Enable], [WhenModify], [Name], [CategoryID], [Price], [Unit], [PictruePath], [Description]) VALUES (N'15', 1, CAST(0x0000A4A200746C63 AS DateTime), N'苹果', N'8', CAST(7.00 AS Decimal(10, 2)), NULL, NULL, NULL)
INSERT [dbo].[Commodity] ([ID], [Enable], [WhenModify], [Name], [CategoryID], [Price], [Unit], [PictruePath], [Description]) VALUES (N'16', 1, CAST(0x0000A4A200746C63 AS DateTime), N'流连忘返', N'8', CAST(7.00 AS Decimal(10, 2)), NULL, NULL, NULL)
INSERT [dbo].[Commodity] ([ID], [Enable], [WhenModify], [Name], [CategoryID], [Price], [Unit], [PictruePath], [Description]) VALUES (N'2', 1, CAST(0x0000A4A200746C63 AS DateTime), N'羊肉', N'2', CAST(11.00 AS Decimal(10, 2)), N'千克', NULL, NULL)
INSERT [dbo].[Commodity] ([ID], [Enable], [WhenModify], [Name], [CategoryID], [Price], [Unit], [PictruePath], [Description]) VALUES (N'3', 1, CAST(0x0000A4A200746C63 AS DateTime), N'猪肉', N'2', CAST(12.00 AS Decimal(10, 2)), N'千克', NULL, NULL)
INSERT [dbo].[Commodity] ([ID], [Enable], [WhenModify], [Name], [CategoryID], [Price], [Unit], [PictruePath], [Description]) VALUES (N'4', 1, CAST(0x0000A4A200746C63 AS DateTime), N'鸡肉', N'2', CAST(12.00 AS Decimal(10, 2)), N'千克', NULL, NULL)
INSERT [dbo].[Commodity] ([ID], [Enable], [WhenModify], [Name], [CategoryID], [Price], [Unit], [PictruePath], [Description]) VALUES (N'5', 1, CAST(0x0000A4A200746C63 AS DateTime), N'大白菜', N'4', CAST(13.00 AS Decimal(10, 2)), N'盒', NULL, NULL)
INSERT [dbo].[Commodity] ([ID], [Enable], [WhenModify], [Name], [CategoryID], [Price], [Unit], [PictruePath], [Description]) VALUES (N'6', 1, CAST(0x0000A4A200746C63 AS DateTime), N'小白菜', N'4', CAST(14.00 AS Decimal(10, 2)), N'盒', NULL, NULL)
INSERT [dbo].[Commodity] ([ID], [Enable], [WhenModify], [Name], [CategoryID], [Price], [Unit], [PictruePath], [Description]) VALUES (N'7', 1, CAST(0x0000A4A200746C63 AS DateTime), N'中白菜', N'4', CAST(15.00 AS Decimal(10, 2)), N'盒', NULL, NULL)
INSERT [dbo].[Commodity] ([ID], [Enable], [WhenModify], [Name], [CategoryID], [Price], [Unit], [PictruePath], [Description]) VALUES (N'8', 1, CAST(0x0000A4A200746C63 AS DateTime), N'番茄', N'4', CAST(15.00 AS Decimal(10, 2)), N'个', NULL, NULL)
INSERT [dbo].[Commodity] ([ID], [Enable], [WhenModify], [Name], [CategoryID], [Price], [Unit], [PictruePath], [Description]) VALUES (N'9', 1, CAST(0x0000A4A200746C63 AS DateTime), N'红萝卜', N'4', CAST(16.00 AS Decimal(10, 2)), N'个', NULL, NULL)
/****** Object:  Table [dbo].[Activity]    Script Date: 06/18/2015 01:55:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Activity]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Activity](
	[ID] [nvarchar](25) COLLATE Chinese_PRC_CS_AS NOT NULL,
	[Enable] [bit] NOT NULL,
	[WhenModify] [datetime] NOT NULL,
	[Title] [nvarchar](50) COLLATE Chinese_PRC_CS_AS NOT NULL,
	[CategoryID] [nvarchar](25) COLLATE Chinese_PRC_CS_AS NULL,
	[Content] [nvarchar](max) COLLATE Chinese_PRC_CS_AS NULL,
	[Address] [nvarchar](255) COLLATE Chinese_PRC_CS_AS NOT NULL,
	[RegistBeginDate] [datetime] NOT NULL,
	[RegistEndDate] [datetime] NOT NULL,
	[Price] [decimal](10, 2) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[Days] [int] NOT NULL,
 CONSTRAINT [PK_Activity] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Activity', N'COLUMN',N'Title'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'标题' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Activity', @level2type=N'COLUMN',@level2name=N'Title'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Activity', N'COLUMN',N'CategoryID'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'分类' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Activity', @level2type=N'COLUMN',@level2name=N'CategoryID'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Activity', N'COLUMN',N'Content'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'内容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Activity', @level2type=N'COLUMN',@level2name=N'Content'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Activity', N'COLUMN',N'Address'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Activity', @level2type=N'COLUMN',@level2name=N'Address'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Activity', N'COLUMN',N'RegistBeginDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'报名开始时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Activity', @level2type=N'COLUMN',@level2name=N'RegistBeginDate'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Activity', N'COLUMN',N'RegistEndDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'报名结束日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Activity', @level2type=N'COLUMN',@level2name=N'RegistEndDate'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Activity', N'COLUMN',N'Price'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'费用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Activity', @level2type=N'COLUMN',@level2name=N'Price'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Activity', N'COLUMN',N'StartDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开始日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Activity', @level2type=N'COLUMN',@level2name=N'StartDate'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Activity', N'COLUMN',N'Days'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'活动天数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Activity', @level2type=N'COLUMN',@level2name=N'Days'
GO
/****** Object:  Table [dbo].[OrderItem]    Script Date: 06/18/2015 01:55:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderItem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[OrderItem](
	[ID] [nvarchar](25) COLLATE Chinese_PRC_CS_AS NOT NULL,
	[Enable] [bit] NOT NULL CONSTRAINT [DF_OrderItem_Enable]  DEFAULT ((1)),
	[WhenModify] [datetime] NOT NULL CONSTRAINT [DF_OrderItem_WhenModify]  DEFAULT (sysdatetime()),
	[OrderListID] [nvarchar](25) COLLATE Chinese_PRC_CS_AS NULL,
	[CommodityID] [nvarchar](25) COLLATE Chinese_PRC_CS_AS NULL,
	[Price] [decimal](10, 2) NOT NULL CONSTRAINT [DF_OrderItem_Price]  DEFAULT ((0)),
	[Count] [int] NOT NULL CONSTRAINT [DF_OrderItem_Count]  DEFAULT ((1)),
	[Total] [decimal](10, 2) NOT NULL CONSTRAINT [DF_OrderItem_Total]  DEFAULT ((0)),
 CONSTRAINT [PK_OrderItem] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'Count'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'个数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'Count'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderItem', N'COLUMN',N'Total'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'单价×数量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderItem', @level2type=N'COLUMN',@level2name=N'Total'
GO
/****** Object:  ForeignKey [FK_Commodity_Category]    Script Date: 06/18/2015 01:55:44 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Commodity_Category]') AND parent_object_id = OBJECT_ID(N'[dbo].[Commodity]'))
ALTER TABLE [dbo].[Commodity]  WITH CHECK ADD  CONSTRAINT [FK_Commodity_Category] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Category] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Commodity_Category]') AND parent_object_id = OBJECT_ID(N'[dbo].[Commodity]'))
ALTER TABLE [dbo].[Commodity] CHECK CONSTRAINT [FK_Commodity_Category]
GO
/****** Object:  ForeignKey [FK_Activity_Category]    Script Date: 06/18/2015 01:55:44 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Activity_Category]') AND parent_object_id = OBJECT_ID(N'[dbo].[Activity]'))
ALTER TABLE [dbo].[Activity]  WITH CHECK ADD  CONSTRAINT [FK_Activity_Category] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Category] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Activity_Category]') AND parent_object_id = OBJECT_ID(N'[dbo].[Activity]'))
ALTER TABLE [dbo].[Activity] CHECK CONSTRAINT [FK_Activity_Category]
GO
/****** Object:  ForeignKey [FK_OrderItem_Commodity]    Script Date: 06/18/2015 01:55:45 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_Commodity]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_Commodity] FOREIGN KEY([CommodityID])
REFERENCES [dbo].[Commodity] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_Commodity]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_Commodity]
GO
/****** Object:  ForeignKey [FK_OrderItem_OrderList]    Script Date: 06/18/2015 01:55:45 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_OrderList]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_OrderList] FOREIGN KEY([OrderListID])
REFERENCES [dbo].[OrderList] ([ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_OrderList]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_OrderList]
GO
