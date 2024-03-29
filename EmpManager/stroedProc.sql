DROP proc if exists [dbo].[GetData]
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
END