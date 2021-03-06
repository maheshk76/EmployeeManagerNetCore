USE [EmpDbContext]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 22-04-2021 10:08:27 ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employees]    Script Date: 22-04-2021 10:08:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[EmployeeId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
	[LastName] [nvarchar](max) NOT NULL,
	[JoiningDate] [datetime2](7) NOT NULL,
	[Salary] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeToProjects]    Script Date: 22-04-2021 10:08:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmployeeToProjects](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [int] NOT NULL,
	[ProjectId] [int] NOT NULL,
 CONSTRAINT [PK_EmployeeToProjects] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Projects]    Script Date: 22-04-2021 10:08:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Projects](
	[ProjectId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Cost] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Projects] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmployeeToProjects]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeToProjects_Employees_EmployeeId] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employees] ([EmployeeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EmployeeToProjects] CHECK CONSTRAINT [FK_EmployeeToProjects_Employees_EmployeeId]
GO
ALTER TABLE [dbo].[EmployeeToProjects]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeToProjects_Projects_ProjectId] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([ProjectId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EmployeeToProjects] CHECK CONSTRAINT [FK_EmployeeToProjects_Projects_ProjectId]
GO
/****** Object:  StoredProcedure [dbo].[GetData]    Script Date: 22-04-2021 10:08:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[GetData]
    @search nvarchar(max) =NULL,
    @sdate nvarchar(max)=NULL,
    @edate nvarchar(max)=NULL,
	@pagesize int = 5,
	@pagenum int = 1,
	@possiblerows int output
	as
	Begin
    set NoCount on;
    DECLARE @SkipRows int = (@pagenum - 1) * @pagesize;

	--a variable that stores total records without pagination--
	select @possiblerows=COUNT(w.EmployeeId) from EmployeeToProjects w 
inner join Employees e  on w.EmployeeId=e.EmployeeId
inner join Projects a on w.ProjectId=a.ProjectId
where 
	(@search='' or e.FirstName like '%'+@search+'%' or e.LastName like '%'+@search+'%' or a.Name like '%'+@search+'%')
	and (@sdate=''  or e.JoiningDate>=@sdate)
	and (@edate=''  or e.JoiningDate<=@edate)

		
select w.EmployeeId,w.Id as EmpToProjectId,w.ProjectId,e.FirstName+' '+e.LastName as FullName,a.Name as ProjectName,e.JoiningDate,a.Cost
from EmployeeToProjects w 
inner join Employees e  on w.EmployeeId=e.EmployeeId
inner join Projects a on w.ProjectId=a.ProjectId
where 
	(@search='' or e.FirstName like '%'+@search+'%' or e.LastName like '%'+@search+'%' or a.Name like '%'+@search+'%')
	and (@sdate=''  or e.JoiningDate>=@sdate)
	and (@edate=''  or e.JoiningDate<=@edate)

order by e.JoiningDate asc,e.FirstName asc 
OFFSET @SkipRows ROWS 
    FETCH NEXT @pagesize ROWS ONLY
return

End
GO
