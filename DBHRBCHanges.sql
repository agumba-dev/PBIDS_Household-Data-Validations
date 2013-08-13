use HRB
go
BEGIN TRY
begin transaction alltr
	begin transaction ctaker
		alter table  DSS.Migrations
			add  
			[observationid] [varchar](200) NULL,
			[entry_date] [datetime] NULL
			--[transit_id] [int] IDENTITY(1,1) NOT NULL
	

	begin transaction pamen
		alter table  DSS.pregoutcome
			add  
			[observationid] [varchar](200) NULL,
			[entry_date] [datetime] NULL

	

	begin transaction  cln
		alter table  dss.Consents
			add  
			[entry_date] [datetime] NULL

	

	begin transaction ent
		alter table DSS.individual
			add  
			[observationid] [varchar](200) NULL
			--[entry_date] [datetime] NULL
	

	begin transaction der

		alter table DSS.Events_Episodes
			add  
			[observationid] [varchar](200) NULL,
			[entry_date] [datetime] NULL
		
commit transaction ctaker
commit transaction pamen
commit transaction cln
commit transaction ent
commit transaction der

commit transaction alltr

END TRY

BEGIN CATCH
    rollback transaction ctaker
	rollback transaction pamen
	rollback transaction cln
	rollback transaction ent
	rollback transaction der
	rollback transaction alltr
END CATCH