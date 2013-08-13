SELECT [educationID]
      ,[xid]
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
      ,[fieldworker]
      ,[rec_status]
      ,[errflag]
      ,[errdate]
      ,[transit_id]
      ,case when ([everenr] is null) or (ltrim(rtrim([everenr]))='') then 'Q' else [everenr] end as [everenr]
      ,case when ([whyenrol] is null) or (ltrim(rtrim([whyenrol]))='') then 'Q' else [whyenrol] end as [whyenrol]
      ,case when ([enrol] is null) or (ltrim(rtrim([enrol]))='') then 'Q' else [enrol] end as [enrol]
      ,case when ([othreas] is null) or (ltrim(rtrim([othreas]))='') then 'Q' else [othreas] end as [othreas]
      ,case when ([engread] is null) or (ltrim(rtrim([engread]))='') then 'Q' else [engread] end as [engread]
      ,case when ([engwrite] is null) or (ltrim(rtrim([engwrite]))='') then 'Q' else [engwrite] end as [engwrite]
      ,case when ([engspk] is null) or (ltrim(rtrim([engspk]))='') then 'Q' else [engspk] end as [engspk]
      ,case when ([kisread] is null) or (ltrim(rtrim([kisread]))='') then 'Q' else [kisread] end as [kisread]
      ,case when ([kiswrite] is null) or (ltrim(rtrim([kiswrite]))='') then 'Q' else [kiswrite] end as [kiswrite]
      ,case when ([kisspk] is null) or (ltrim(rtrim([kisspk]))='') then 'Q' else [kisspk] end as [kisspk]
      ,case when ([year] is null) or (ltrim(rtrim([year]))='') then 'Q' else [year] end as [year]
      --,case when ([kisread] is null) or (ltrim(rtrim([kisread]))='') then 'Q' else [kisread] end as [kisread]
  FROM 
  --update 
  [TEMP_DSSHRS].[SpecialStudies].[education]
  --set
  -- [everenr]=case when ([everenr] is null) or (ltrim(rtrim([everenr]))='') then 'Q' else [everenr] end 
  --    , [whyenrol]=case when ([whyenrol] is null) or (ltrim(rtrim([whyenrol]))='') then 'Q' else [whyenrol] end 
  --    ,[enrol]=case when ([enrol] is null) or (ltrim(rtrim([enrol]))='') then 'Q' else [enrol] end  
  --    ,[othreas]=case when ([othreas] is null) or (ltrim(rtrim([othreas]))='') then 'Q' else [othreas] end 
  --    ,[engread]=case when ([engread] is null) or (ltrim(rtrim([engread]))='') then 'Q' else [engread] end 
  --    ,[engwrite]=case when ([engwrite] is null) or (ltrim(rtrim([engwrite]))='') then 'Q' else [engwrite] end 
  --    ,[engspk]=case when ([engspk] is null) or (ltrim(rtrim([engspk]))='') then 'Q' else [engspk] end 
  --    ,[kisread]=case when ([kisread] is null) or (ltrim(rtrim([kisread]))='') then 'Q' else [kisread] end 
  --    ,[kiswrite]=case when ([kiswrite] is null) or (ltrim(rtrim([kiswrite]))='') then 'Q' else [kiswrite] end 
  --    ,[kisspk]=case when ([kisspk] is null) or (ltrim(rtrim([kisspk]))='') then 'Q' else [kisspk] end 
  --    ,[year]=case when ([year] is null) or (ltrim(rtrim([year]))='') then 'Q' else [year] end 
  
  where 
  not (rec_status like '%x%')
  and
   year=''
GO


