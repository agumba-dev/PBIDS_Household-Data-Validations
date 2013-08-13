update [TEMP_DSSHRS].[SpecialStudies].[education]
set rec_status='xx'
  where rec_status='i' and transit_id not in
(SELECT max(transit_id)
  FROM [TEMP_DSSHRS].[SpecialStudies].[education]
  where rec_status='i'
  group by
  --[educationID]
      [xid]
      ,[individid]
      ,[seq]
      ,[observeid]
      ,[vill]
      ,[date]
      ,[status]
      ,[everenr]
      ,[enrol]
      ,[whyenrol]
      ,[othreas]
      ,[edulevel]
      ,[grade]
      ,[eduyrs]
      ,[engread]
      ,[engwrite]
      ,[engspk]
      ,[kisread]
      ,[kiswrite]
      ,[kisspk]
      ,[year]
      ,[glocid]
      )
      
   
     
GO


