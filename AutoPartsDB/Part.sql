CREATE TABLE [dbo].[Part]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ImportId] [nvarchar](max) NULL,
	[Brand] [nvarchar](max) NULL,
	[Number] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Details] [nvarchar](max) NULL,
	[Size] [nvarchar](max) NULL,
	[Weight] [nvarchar](max) NULL,
	[Quantity] [nvarchar](max) NULL,
	[Price] [nvarchar](max) NULL,
	[Supplier] [nvarchar](max) NULL,
	[DeliveryTime] [nvarchar](max) NULL
)
