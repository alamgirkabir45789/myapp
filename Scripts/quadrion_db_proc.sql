USE [quadrion_db]
GO
/****** Object:  StoredProcedure [dbo].[prcGetCurrentYearLeaveSummary]    Script Date: 10/25/2023 5:12:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER proc [dbo].[prcGetCurrentYearLeaveSummary]
@Name varchar(100)
as 
Begin 
	select Name,Reason,ApprovedFromDate,ApprovedToDate,IsAccepted,ContactNo,Designation,Email,LeaveType,ApprovedTotalDay,TotalDay,FromTime,ToTime,IsDays,IsHours, 
	ISNULL((Select Sum(TotalDay) From LeaveRegisters where IsAccepted=1 and LeaveType='SICK' and Name=@Name and DATEPART(YEAR, FromDate)= DATEPART(YEAR, GETDATE()) and DATEPART(YEAR, ToDate)= DATEPART(YEAR, GETDATE()) Group BY LeaveType ),0) as TotalSickLeaveTaken ,
	ISNULL((Select Sum(TotalDay) From LeaveRegisters where IsAccepted=1 and LeaveType='EARN' and Name=@Name and DATEPART(YEAR, FromDate)= DATEPART(YEAR, GETDATE()) and DATEPART(YEAR, ToDate)= DATEPART(YEAR, GETDATE()) Group BY LeaveType ),0) as TotalEarnLeaveTaken ,
	ISNULL((Select Sum(TotalDay) From LeaveRegisters where IsAccepted=1 and LeaveType='ANUAL' and Name=@Name and DATEPART(YEAR, FromDate)= DATEPART(YEAR, GETDATE()) and DATEPART(YEAR, ToDate)= DATEPART(YEAR, GETDATE()) Group BY LeaveType),0) as TotalAnualLeaveTaken ,
	ISNULL((Select Sum(TotalDay) From LeaveRegisters where IsAccepted=1 and LeaveType='CASUAL' and Name=@Name and DATEPART(YEAR, FromDate)= DATEPART(YEAR, GETDATE()) and DATEPART(YEAR, ToDate)= DATEPART(YEAR, GETDATE()) Group BY LeaveType),0) as TotalCasualLeaveTaken ,
	ISNULL((Select Sum(TotalDay) From LeaveRegisters where IsAccepted=1 and LeaveType='MATERNITY' and Name=@Name and DATEPART(YEAR, FromDate)= DATEPART(YEAR, GETDATE()) and DATEPART(YEAR, ToDate)= DATEPART(YEAR, GETDATE()) Group BY LeaveType),0) as TotalMaternityLeaveTaken ,
	ISNULL((Select Sum(TotalDay) From LeaveRegisters where IsAccepted=1 and LeaveType='HALFDAY' and Name=@Name and DATEPART(YEAR, FromDate)= DATEPART(YEAR, GETDATE()) and DATEPART(YEAR, ToDate)= DATEPART(YEAR, GETDATE()) Group BY LeaveType),0) as TotalHalfDayLeaveTaken ,
	ISNULL((Select Sum(TotalDay) From LeaveRegisters where IsAccepted=1 and LeaveType='SHORTTERM' and Name=@Name and DATEPART(YEAR, FromDate)= DATEPART(YEAR, GETDATE()) and DATEPART(YEAR, ToDate)= DATEPART(YEAR, GETDATE()) Group BY LeaveType),0) as TotalShortTermLeaveTaken ,

	
	(Select top 1 TotalDay From EmpLeaveTypes where LeaveTypeName='SICK') as TotalSickLeave,
	(Select top 1 TotalDay From EmpLeaveTypes where LeaveTypeName='EARN') as TotalEarnLeave,
	(Select top 1 TotalDay From EmpLeaveTypes where LeaveTypeName='ANUAL') as TotalAnualLeave,
	(Select top 1 TotalDay From EmpLeaveTypes where LeaveTypeName='CASUAL') as TotalCasualLeave	,
	(Select top 1 TotalDay From EmpLeaveTypes where LeaveTypeName='MATERNITY') as TotalMaternityLeave	,
	(Select top 1 TotalDay From EmpLeaveTypes where LeaveTypeName='HALFDAY') as TotalHalfDayLeave	,
	(Select top 1 TotalDay From EmpLeaveTypes where LeaveTypeName='SHORTTERM') as TotalShortTermLeave 
	from LeaveRegisters where Name=@Name and DATEPART(YEAR, FromDate)= DATEPART(YEAR, GETDATE()) and DATEPART(YEAR, ToDate)= DATEPART(YEAR, GETDATE())
	
End
