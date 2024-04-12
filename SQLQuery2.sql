USE [Airport]
GO

/****** Object:  Table [dbo].[Airplanes]    Script Date: 12/6/2023 4:45:55 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Airplanes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FlightNumber] [nchar](10) NOT NULL,
	[Brand] [nvarchar](50) NOT NULL,
	[PreviousTerminal] [int] NULL,
	[CurrentTerminal] [int] NULL,
	[ImgPath] [nvarchar](max) NOT NULL,
	[CanTakeOff] [bit] NOT NULL,
 CONSTRAINT [PK_Airplanes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Terminals](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Number] [int] NOT NULL,
	[DurationInTerminal] [int] NOT NULL,
	[IsOccupied] [bit] NOT NULL,
	[PositionX] [int] NOT NULL,
	[PositionY] [int] NOT NULL,
 CONSTRAINT [PK_Terminals] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Flights](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AirplaneId] [int] NOT NULL,
	[TerminalId] [int] NOT NULL,
 CONSTRAINT [PK_Flights] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Flights]  WITH CHECK ADD  CONSTRAINT [FK_Flights_Airplanes] FOREIGN KEY([AirplaneId])
REFERENCES [dbo].[Airplanes] ([Id])
GO

ALTER TABLE [dbo].[Flights] CHECK CONSTRAINT [FK_Flights_Airplanes]
GO

ALTER TABLE [dbo].[Flights]  WITH CHECK ADD  CONSTRAINT [FK_Flights_Terminals] FOREIGN KEY([TerminalId])
REFERENCES [dbo].[Terminals] ([Id])
GO

ALTER TABLE [dbo].[Flights] CHECK CONSTRAINT [FK_Flights_Terminals]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FlightsHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FlightId] [int] NOT NULL,
	[ActualArrivalTime] [datetime] NOT NULL,
	[ActualDepartureTime] [datetime] NOT NULL,
 CONSTRAINT [PK_FlightsHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[FlightsHistory]  WITH CHECK ADD  CONSTRAINT [FK_FlightsHistory_Flights] FOREIGN KEY([FlightId])
REFERENCES [dbo].[Flights] ([Id])
GO

ALTER TABLE [dbo].[FlightsHistory] CHECK CONSTRAINT [FK_FlightsHistory_Flights]
GO

/****** Object:  Table [dbo].[ScheduledFlights]    Script Date: 12/6/2023 4:54:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ScheduledFlights](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FlightId] [int] NOT NULL,
	[ScheduledArrivalTime] [datetime] NOT NULL,
	[ScheduledDepartureTime] [datetime] NOT NULL,
 CONSTRAINT [PK_ScheduledFlights] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ScheduledFlights]  WITH CHECK ADD  CONSTRAINT [FK_ScheduledFlights_Flights] FOREIGN KEY([FlightId])
REFERENCES [dbo].[Flights] ([Id])
GO

ALTER TABLE [dbo].[ScheduledFlights] CHECK CONSTRAINT [FK_ScheduledFlights_Flights]
GO

INSERT INTO [dbo].[Terminals]
           ([Number]
           ,[DurationInTerminal]
           ,[IsOccupied]
           ,[PositionX]
           ,[PositionY])
     VALUES
	 (1, 0, 0, 1165, 100),
	 (2, 0, 0, 1030, 185),
	 (3, 0, 0, 900, 270),
	 (4, 0, 0, 610, 320),
	 (5, 0, 0, 280, 465),
	 (6, 0, 0, 405, 655),
	 (7, 0, 0, 725, 655),
	 (8, 0, 0, 900, 465),
	 (9, 0, 0, 380, 190)

GO







