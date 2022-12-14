USE [csproj]
GO
/****** Object:  Table [dbo].[Klienci]    Script Date: 17.09.2022 16:57:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Klienci](
	[id] [int] NOT NULL,
	[imie] [char](20) NOT NULL,
	[nazwisko] [char](20) NOT NULL,
 CONSTRAINT [PK_Klienci] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Motorniczy]    Script Date: 17.09.2022 16:57:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Motorniczy](
	[id] [int] NOT NULL,
	[imie] [char](20) NOT NULL,
	[nazwisko] [char](20) NOT NULL,
	[id_tramwaju] [int] NOT NULL,
 CONSTRAINT [PK_Motorniczy] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Przystanki]    Script Date: 17.09.2022 16:57:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Przystanki](
	[id] [int] NOT NULL,
	[nazwa] [char](20) NOT NULL,
 CONSTRAINT [PK_Przystanki] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Przystanki_na_trasie]    Script Date: 17.09.2022 16:57:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Przystanki_na_trasie](
	[id_trasy] [int] NOT NULL,
	[id_przystanku] [int] NOT NULL,
	[numer_na_trasie] [int] NOT NULL,
 CONSTRAINT [PK_Przystanki_na_trasie] PRIMARY KEY CLUSTERED 
(
	[id_trasy] ASC,
	[id_przystanku] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tramwaje]    Script Date: 17.09.2022 16:57:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tramwaje](
	[id] [int] NOT NULL,
	[numer] [int] NOT NULL,
	[data_ostatniego_przeglądu] [date] NOT NULL,
	[id_trasy] [int] NOT NULL,
 CONSTRAINT [PK_Tramwaje] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Trasy]    Script Date: 17.09.2022 16:57:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Trasy](
	[id] [int] NOT NULL,
	[nazwa] [char](20) NOT NULL,
 CONSTRAINT [PK_Trasy] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Motorniczy]  WITH CHECK ADD  CONSTRAINT [FK_Motorniczy_Tramwaje] FOREIGN KEY([id_tramwaju])
REFERENCES [dbo].[Tramwaje] ([id])
GO
ALTER TABLE [dbo].[Motorniczy] CHECK CONSTRAINT [FK_Motorniczy_Tramwaje]
GO
ALTER TABLE [dbo].[Przystanki_na_trasie]  WITH CHECK ADD  CONSTRAINT [FK_Przystanki_na_trasie_Przystanki] FOREIGN KEY([id_przystanku])
REFERENCES [dbo].[Przystanki] ([id])
GO
ALTER TABLE [dbo].[Przystanki_na_trasie] CHECK CONSTRAINT [FK_Przystanki_na_trasie_Przystanki]
GO
ALTER TABLE [dbo].[Przystanki_na_trasie]  WITH CHECK ADD  CONSTRAINT [FK_Przystanki_na_trasie_Trasy] FOREIGN KEY([id_trasy])
REFERENCES [dbo].[Trasy] ([id])
GO
ALTER TABLE [dbo].[Przystanki_na_trasie] CHECK CONSTRAINT [FK_Przystanki_na_trasie_Trasy]
GO
ALTER TABLE [dbo].[Tramwaje]  WITH CHECK ADD  CONSTRAINT [FK_Tramwaje_Trasy] FOREIGN KEY([id_trasy])
REFERENCES [dbo].[Trasy] ([id])
GO
ALTER TABLE [dbo].[Tramwaje] CHECK CONSTRAINT [FK_Tramwaje_Trasy]
GO
