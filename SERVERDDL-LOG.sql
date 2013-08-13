/****** Object:  DdlTrigger [dss_server_log]    Script Date: 11/16/2011 11:37:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE TRIGGER [dss_server_log] 
on all server 
FOR DDL_EVENTS 
AS
DECLARE @data XML;
SET @data = EVENTDATA();
INSERT dsshrs.dbo.ddl_log 
   (PostTime, DB_User, Event, TSQL,EVENTDATAX) 
   VALUES 
   (GETDATE(), 
   CONVERT(nvarchar(100), current_USER), 
   @data.value('(/EVENT_INSTANCE/EventType)[1]', 'nvarchar(100)'), 
   @data.value('(/EVENT_INSTANCE/TSQLCommand)[1]', 'nvarchar(2000)'),@data) ;





GO

SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO

ENABLE TRIGGER [dss_server_log] ON ALL SERVER
GO


