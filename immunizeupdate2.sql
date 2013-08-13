use TEMP_DSSHRS
go

update SpecialStudies .immunize set  pen1vgiven = null ,pe1vngiven=null where cardavail = 'yes' and 
pen1vgiven is not null and pe1vngiven is not null  and 
round between 20091 and 20103

go

update SpecialStudies .immunize set pen2vgiven = null ,pe2vngiven=null  where cardavail = 'yes' and 
pen2vgiven is not null and ( pe2vngiven is not null) and 
round between 20091 and 20103

go

update SpecialStudies .immunize set pen3vgiven = null ,pe3vngiven=null  where cardavail ='yes' and 
pen3vgiven is not null and ( pe3vngiven is not null) and 
round between 20091 and 20103
go


update SpecialStudies .immunize set  bcg1vngive = null,bcg1vgiven=null where cardavail = 'yes' and 
bcg1vngive is not null and ( bcg1vgiven is not null) and 
round between 20091 and 20103


go
update SpecialStudies .immunize set pol0vgivn = null, pol0vngivn = null where cardavail = 'yes' and 
pol0vgivn is not null and ( pol0vngivn is not null) and 
round between 20101 and 20103


go
update SpecialStudies .immunize set pol1vgiven = null, pol1vngivn = null where cardavail = 'yes' and 
pol1vgiven is not null and ( pol1vngivn is not null) and 
round between 20091 and 20103

go
update SpecialStudies .immunize set pol2vgivn = null, pol2vngivn = null  where cardavail = 'yes' and 
pol2vgivn is not null and ( pol2vngivn is not null) and 
round between 20091 and 20103
go


update SpecialStudies .immunize set pol3vgivn = null, pol3vngivn = null  where cardavail = 'yes' and 
pol3vgivn is not null and ( pol3vngivn is not null) and 
round between 20091 and 20103


go
update SpecialStudies .immunize set measvgiven = null, meavngiven = null where cardavail = 'yes' and 
measvgiven is not null and ( meavngiven is not null) and 
round between 20091 and 20103