Imports System.Data
Imports System.Data.SqlClient
Public Class clsDsshrs_Residency_Val

#Region "variables and constructor"
    Private globalvariables As clsGlobalVariables = clsGlobalVariables.getObject
    Private da As clsDataAccess = clsDataAccess.getObject
    Private startEvents As String() = {"BIR", "ENT", "ENU", "TRI"}
    Private endEvents As String() = {"DTH", "TRO", "EXT"}
    Private Sub New()

    End Sub
    Public Sub New(ByVal validationtype As mhrsSyncValidationTypes)
        da.validationtype = validationtype
    End Sub
#End Region
#Region "procedures"
    
    Friend Function validaterec(ByVal Residencyrecord As DataRow, ByVal tablename As String) As Boolean
        'reset errror column

        Dim Village As String = da.getrecordsCompound(tablename, Residencyrecord).Trim
        Dim isValidrecord As Boolean = True
        Dim round As String = da.getrecordsRound(tablename, Residencyrecord).Trim
        'ResidencyID	1	uniqueidentifier
        'individid	2	varchar
        If Not clsUserDefinedFunctions.isValidIndividid(Residencyrecord("individid").ToString) Then
            Me.da.saveError(Residencyrecord("ResidencyID").ToString.Trim, tablename, "invalid individid", "", Now(), "", Village, round)
            isValidrecord = False
        End If
        ' If Not isValidrecord Then
        Select Case (IsDBNull(Residencyrecord("eeventtype")) Or IsDBNull(Residencyrecord("edate")) Or IsDBNull(Residencyrecord("eobserveid")))
            Case True
                If Me.validateStartofEpisode(Residencyrecord, tablename, Village, round) Then
                    isValidrecord = False
                End If
            Case False
                If Me.validateStartofEpisode(Residencyrecord, tablename, Village, round) Then
                    isValidrecord = False
                End If
                If Me.validateEndofEpisode(Residencyrecord, tablename, Village, round) Then
                    isValidrecord = False
                End If
        End Select
        'End If
        Return isValidrecord
    End Function
#End Region
#Region "Validation functions"
    Private Function validateStartofEpisode(ByVal Residencyrecord As DataRow, ByVal tablename As String, ByVal village As String, ByVal round As String) As Boolean
        Dim hasError As Boolean = False
        'locationid	3	varchar
        If Not clsUserDefinedFunctions.isValidLocationid(Residencyrecord("locationid").ToString) Then
            Me.da.saveError(Residencyrecord("ResidencyID").ToString.Trim, tablename, "invalid locationid", "", Now(), "", village, round)
            hasError = True
        End If
        'seventtype	4	char
        If Not isStartEvent(Residencyrecord("seventtype").ToString) Then
            Me.da.saveError(Residencyrecord("ResidencyID").ToString.Trim, tablename, "invalid seventtype", "", Now(), "", village, round)
            hasError = True
        End If
        'sdate	5	datetime
        If CDate(Residencyrecord("sdate")) > Now() Then
            Me.da.saveError(Residencyrecord("ResidencyID").ToString.Trim, tablename, "invalid date sdate", "", Now(), "", village, round)
            hasError = True
        End If
        'sdate	5	datetime
       
        'sobserveid	6	varchar
        If Not clsUserDefinedFunctions.isValidObservationid(Residencyrecord("sobserveid").ToString) Then
            Me.da.saveError(Residencyrecord("ResidencyID").ToString.Trim, tablename, "invalid sobserveid", "", Now(), "", village, round)
            hasError = True
        End If
        'perform startevent reference validations
        If Me.validateStartEvent(Residencyrecord, tablename, village, round) Then
            hasError = True
        End If
        Return hasError
    End Function
    Private Function validateEndofEpisode(ByVal Residencyrecord As DataRow, ByVal tablename As String, ByVal village As String, ByVal round As String) As Boolean
        Dim hasError As Boolean = False
        'Check if the record exists in MainDB
        'ResidencyID	1	uniqueidentifier
        
        'eeventtype	8	char
        If Not isEndEvent(Residencyrecord("eeventtype").ToString) Then
            Me.da.saveError(Residencyrecord("ResidencyID").ToString.Trim, tablename, "invalid eeventtype", "", Now(), "", village, round)
            hasError = True
        End If
        'edate	9	datetime

        If CDate(Residencyrecord("edate")) > Now() Then
            Me.da.saveError(Residencyrecord("ResidencyID").ToString.Trim, tablename, "invalid date edate", "", Now(), "", village, round)
            hasError = True
        End If
        If CDate(Residencyrecord("sdate")) > CDate(Residencyrecord("edate")) Then
            Me.da.saveError(Residencyrecord("ResidencyID").ToString.Trim, tablename, "sdate is greater than edate", "", Now(), "", village, round)
            hasError = True
        End If
        'eobserveid	10	varchar
        If Not clsUserDefinedFunctions.isValidObservationid(Residencyrecord("eobserveid").ToString) Then
            Me.da.saveError(Residencyrecord("ResidencyID").ToString.Trim, tablename, "invalid eobserveid", "", Now(), "", village, round)
            hasError = True
        End If
        'efieldworker	11	varchar

        'Other
        If Not ((Residencyrecord("seventtype").ToString.ToUpper.Trim = "BIR")) Then
            If CDate(Residencyrecord("sdate")) = CDate(Residencyrecord("edate")) Then
                Me.da.saveError(Residencyrecord("ResidencyID").ToString.Trim, tablename, "sdate is same as  edate", "", Now(), "", village, round)
                hasError = True
            End If
        End If
      
        'perform startevent reference validations
        If Me.validateEndEvent(Residencyrecord, tablename, village, round) Then
            hasError = True
        End If

        Return hasError
    End Function

    Private Function isStartEvent(ByVal seveventype As String) As Boolean
        For Each startevent As String In startEvents
            If startevent.ToLower.Trim = seveventype.ToLower.Trim Then
                Return True
            End If
        Next
        Return False
    End Function
    Private Function isEndEvent(ByVal eeveventype As String) As Boolean
        For Each endevent As String In endEvents
            If endevent.ToLower.Trim = eeveventype.ToLower.Trim Then
                Return True
            End If
        Next
        Return False
    End Function
    Private Function validateStartEvent(ByVal Residencyrecord As DataRow, ByVal tablename As String, ByVal village As String, ByVal round As String) As Boolean
        Dim hasError As Boolean = False
        If isStartEvent(Residencyrecord("seventtype").ToString) Then
            Select Case Residencyrecord("seventtype").ToString.ToUpper.Trim
                'should be the first episode for individual
                Case "ENU", "BIR"

                Case "ENT"
                    'check if its a return migrant
                    If Me.hasEpisodes(Residencyrecord) Then
                        'last event for return migrants should be 'EXT'
                        
                        'if last event was an 'EXT' then
                        'check if there is another event that occured in the same date
                        If Me.eventDateExists(Residencyrecord) Then
                            Me.da.saveError(Residencyrecord("ResidencyID").ToString.Trim, tablename, "the event date already exists", "", Now(), "", village, round)
                            hasError = True
                        End If
                        'check if the individual had died
                        If Me.hadDied(Residencyrecord) Then
                            Me.da.saveError(Residencyrecord("ResidencyID").ToString.Trim, tablename, "Individual has a DTH event", "", Now(), "", village, round)
                            hasError = True
                        End If
                        'The ENT date value should not be equal to the subsequent EXT date value
                        If Me.endEventDateExists(Residencyrecord) Then
                            Me.da.saveError(Residencyrecord("ResidencyID").ToString.Trim, tablename, "the previous end event date is similar to current sdate ", "", Now(), "", village, round)
                            hasError = True
                        End If
                    End If
                    
                Case "TRI"
                    'check if its has episodes
                    If Me.hasEpisodes(Residencyrecord) Then
                        'last event for return migrants should be 'TRO'
                        If Not Me.TRIhasValideTRO(Residencyrecord) Then
                            Me.da.saveError(Residencyrecord("ResidencyID").ToString.Trim, tablename, "the last event for the TRI is not a TRO", "", Now(), "", village, round)
                            hasError = True
                        Else
                            'if last event was an 'EXT' then
                            'check if there is another event that occured in the same date
                            If Me.eventDateExists(Residencyrecord) Then
                                Me.da.saveError(Residencyrecord("ResidencyID").ToString.Trim, tablename, "the event date already exists", "", Now(), "", village, round)
                                hasError = True
                            End If
                            'check if the individual had died
                            If Me.hadDied(Residencyrecord) Then
                                Me.da.saveError(Residencyrecord("ResidencyID").ToString.Trim, tablename, "Individual has a DTH event", "", Now(), "", village, round)
                                hasError = True
                            End If
                            'individual should have only one open episode
                            'If Me.hasOpenEpisode(Residencyrecord) Then
                            '    Me.da.saveError(Residencyrecord("ResidencyID").ToString.Trim, tablename, "Individual has another open episode", "", Now(), "", village, round)
                            '    hasError = True
                            'End If
                        End If
                    Else
                        'if individual has no episodes
                        Me.da.saveError(Residencyrecord("ResidencyID").ToString.Trim, tablename, "Has no previous episodes", "", Now(), "", village, round)
                        hasError = True
                    End If
                Case Else
                    'Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "Unknown start event", "", Now(), "")
                    'hasError = True
            End Select

            Select Case Residencyrecord("seventtype").ToString.ToUpper.Trim
                Case "ENT", "BIR", "ENU"
                    If Me.da.GetMatchingStartEndEvent(Residencyrecord("individid").ToString, Residencyrecord("sdate").ToString(), Residencyrecord("seventtype").ToString, 1, 2) Then
                        Me.da.saveError(Residencyrecord("ResidencyID").ToString.Trim, tablename, "This Residency record is missing a matching Membership record for this start event", "", Now(), "", village, round)
                        hasError = True
                    End If
            End Select
        Else
            hasError = True
        End If
        Return hasError
    End Function
    Private Function validateEndEvent(ByVal Residencyrecord As DataRow, ByVal tablename As String, ByVal village As String, ByVal round As String) As Boolean
        Dim hasError As Boolean = False
        If isEndEvent(Residencyrecord("eeventtype").ToString) Then

            'check if individual has open episode at place of death
            'If Me.hasOpenEpisodeInLocation(Residencyrecord) Then
            'check if there is another event that occured in the same date
            If Me.eventDateExists(Residencyrecord) Then
                Me.da.saveError(Residencyrecord("ResidencyID").ToString.Trim, tablename, "the event date already exists", "", Now(), "", village, round)
                hasError = True
            End If
            ''check for four calender month rule
            If Not Me.meetsFourCalenderMonthsRuleUsingvisitation(Residencyrecord) Then
                Me.da.saveError(Residencyrecord("ResidencyID").ToString.Trim, tablename, "Individual has not met the 4 calendar Month", "", Now(), "", village, round)
                hasError = True
            End If
            'Else
            '    Me.da.saveError(Residencyrecord("ResidencyID").ToString.Trim, tablename, "Individual has no open episode in location", "", Now(), "", village, round)
            '    hasError = True
            '    Return hasError
            'End If

            ' perform event specific validations
            Select Case Residencyrecord("eeventtype").ToString.ToUpper.Trim
                Case "DTH"

                Case "TRO"

                Case "EXT"

                Case Else
                    'Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "Unknown end event", "", Now(), "")
                    'hasError = True
            End Select

            Select Case Residencyrecord("eeventtype").ToString.ToUpper.Trim
                Case "DTH", "EXT"
                    If Me.da.GetMatchingStartEndEvent(Residencyrecord("individid").ToString, Residencyrecord("edate").ToString, _
                                                Residencyrecord("eeventtype"), 2, 2) Then
                        Me.da.saveError(Residencyrecord("ResidencyID").ToString.Trim, tablename, "This Residency record is missing a matching Membership record for this end event", "", Now(), "", village, round)
                        hasError = True
                    End If
            End Select
        Else
            'Me.da.saveError(Residencyrecord("ResidencyID").ToString.Trim, tablename, "Invalid eeventtype ", "", Now(), "", village, round)
            hasError = True
        End If
        Return hasError
    End Function
#End Region
#Region "reference validations functions"
    Private Function eventDateExists(ByVal Residencyrecord As DataRow) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = ""
        Select Case (IsDBNull(Residencyrecord("eeventtype")) Or IsDBNull(Residencyrecord("edate")) Or IsDBNull(Residencyrecord("eobserveid")))
            Case True
                sql = "SELECT count(*) FROM [DSSHRS].[DSS].[residency] " _
                    & " where (sdate is not null) and (cast(floor(cast([sdate] as float)) as datetime) ='" + CDate(Residencyrecord("sdate")).ToString("dd-MMM-yyyy") + "')  " _
                    & "  and (individid='" + Residencyrecord("individid").ToString.Trim + "') and (ResidencyID<>'" + Residencyrecord("ResidencyID").ToString + "')"
                If Me.da.executeScalar_INMainDB(sql) > 0 Then
                    returnValue = True
                Else
                    returnValue = False
                End If
            Case False
                sql = "SELECT count(*)  FROM [DSSHRS].[DSS].[residency] " _
                    & " where (edate is not null) and (year([edate])=" + CDate(Residencyrecord("edate")).Year.ToString + ") and " _
                    & "(month(edate)=" + CDate(Residencyrecord("edate")).Month.ToString + ") " _
                    & " and (day(edate)=" + CDate(Residencyrecord("edate")).Day.ToString + ") and (individid='" + Residencyrecord("individid").ToString.Trim + "') and (ResidencyID<>'" + Residencyrecord("ResidencyID").ToString + "')"
                If Me.da.executeScalar_INMainDB(sql) > 0 Then
                    returnValue = True
                Else
                    returnValue = False
                End If
            Case Else
                returnValue = True
        End Select
        Return returnValue
    End Function

    Private Function endEventDateExists(ByVal Residencyrecord As DataRow) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = ""
        sql = "select count(*) as num  " _
                    & "  from  [DSSHRS].dss.residency " _
                    & " where (edate is not null) and CAST(floor(cast(edate as float)) as datetime)='" + CDate(Residencyrecord("sdate")).ToString("dd-MMM-yyyy") + "' " _
                    & " and (individid='" + Residencyrecord("individid").ToString.Trim + "')"
        If Me.da.executeScalar_INMainDB(sql) > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If
            
        Return returnValue
    End Function

    Private Function hadDied(ByVal Residencyrecord As DataRow) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT count(*)  FROM [DSSHRS].[DSS].[residency]" _
                            & "where (eeventtype='DTH') and (individid='" + Residencyrecord("individid").ToString + "') and " _
                            & "(DATEDIFF(day,[edate],getdate()))>=(DATEDIFF(day,@eventdate,getdate())"
        If Me.da.hadprevioslyDied(sql, Residencyrecord("sdate")) > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function
    Private Function hasEpisodes(ByVal Residencyrecord As DataRow) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT count(*)  FROM [DSSHRS].[DSS].[residency]" _
                            & "where  (individid='" + Residencyrecord("individid").ToString + "') and (ResidencyID<>'" + Residencyrecord("ResidencyID").ToString + "')"
        If Me.da.executeScalar_INMainDB(sql) > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function
    Private Function hasOpenEpisodeInLocation(ByVal Residencyrecord As DataRow) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT count(*) FROM [DSSHRS].[DSS].[residency]" _
                            & " where  (individid='" + Residencyrecord("individid").ToString + "') and (eeventtype is null or eeventtype='' )" _
                            & " and  (eobserveid is null) and (edate is null) and ([locationid]='" + Residencyrecord("locationid").ToString + "') and (ResidencyID<>'" + Residencyrecord("ResidencyID").ToString + "')"
        If Me.da.executeScalar_INMainDB(sql) > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function
   
    Private Function hasArecordedObservation(ByVal observationid As String) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT count(*) FROM [DSSHRS].[DSS].[observation]" _
                            & " where  (observeid='" + observationid + "') "
        If Me.da.executeScalar_INMainDB(sql) > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function
   
    Private Function getLastEvent(ByVal Residencyrecord As DataRow) As String
        Dim returnValue As String = Nothing
       
        returnValue = Me.da.getResidencyPreVEvent(Residencyrecord)

        Return returnValue
    End Function
    Private Function TRIhasValideTRO(ByVal Residencyrecord As DataRow) As Boolean
        Dim locationid As Object = Nothing
        Dim sdate As Date = Residencyrecord("sdate")
        Dim sql As String = "SELECT top 1 locationid  FROM [DSSHRS].[dss].[Residency]" _
                            & " where  (individid='" + Residencyrecord("individid").ToString + "') and (eeventtype='TRO')  " _
                            & " and (year([edate])='" + sdate.Year.ToString + "') and " _
                            & " (month([edate])='" + sdate.Month.ToString + "') " _
                            & " and (day([edate])='" + sdate.Day.ToString + "') and (ResidencyID<>'" + Residencyrecord("ResidencyID").ToString + "')"
        locationid = Me.da.getScalar_inMainDB(sql)
        If Not locationid Is Nothing Then
            If clsUserDefinedFunctions.getcompound_from_locationid(locationid.ToString.Trim).Trim = clsUserDefinedFunctions.getcompound_from_locationid(Residencyrecord("locationid").ToString.Trim).Trim Then
                Return False
            Else
                Return True
            End If
        Else
            Return False
        End If
    End Function
    Private Function meetsFourCalenderMonthsRule(ByVal Residencyrecord As DataRow) As Boolean
        Dim currentEventDate As Date
        Select Case (IsDBNull(Residencyrecord("eeventtype")) Or IsDBNull(Residencyrecord("edate")) Or IsDBNull(Residencyrecord("eobserveid")))
            Case True
                currentEventDate = CDate(Residencyrecord("sdate"))
            Case False
                currentEventDate = CDate(Residencyrecord("edate"))
        End Select
        Dim sql As String = "SELECT max([sdate])  FROM [DSSHRS].[DSS].[residency] " _
                           & "where  (individid='" + Residencyrecord("individid").ToString + "') and (ResidencyID<>'" + Residencyrecord("ResidencyID").ToString + "')"
        Dim lastdate As Date
        Try
            lastdate = CDate(Me.da.getScalar_inMainDB(sql))
        Catch ex As Exception

        End Try

        If clsUserDefinedFunctions.meetsFourMonthsRule(lastdate, currentEventDate) Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function meetsFourCalenderMonthsRuleUsingvisitation(ByVal Residencyrecord As DataRow) As Boolean
        Dim etype As Int16
        Select Case (IsDBNull(Residencyrecord("eeventtype")) Or IsDBNull(Residencyrecord("edate")) Or IsDBNull(Residencyrecord("eobserveid")))
            Case True
                etype = 2
            Case False
                etype = 1
        End Select

        Dim individid As String = Residencyrecord("individid")
        Dim guid As String = Residencyrecord("ResidencyID").ToString
        Dim locationid As String = Residencyrecord("locationid")
        Dim sevent As String = Residencyrecord("seventtype")
        Dim sdate As Date = Residencyrecord("sdate")
        Dim sobserve As String = Residencyrecord("sobserveid")
        Dim edate As Date = IIf(IsDBNull(Residencyrecord("edate")), Nothing, CDate(Residencyrecord("edate")))
        Dim eevent As String = IIf(IsDBNull(Residencyrecord("eeventtype")), String.Empty, Residencyrecord("eeventtype"))
        Dim eobserve As String = IIf(IsDBNull(Residencyrecord("eobserveid")), String.Empty, Residencyrecord("eobserveid"))

        Dim sql As String = "Dsshrs.[DSS].[Res_meetsfourCalendarMonth2]  '" + individid + "','" + guid + "','" + locationid + "','" + sevent + "','" + sdate + "','" + _
            sobserve + "','" + edate + "','" + eevent + "','" + eobserve + "'," + etype.ToString


        Dim lastdate As Int16
        Try
            lastdate = Me.da.getScalar_inMainDB(sql)
        Catch ex As Exception

        End Try


        If lastdate = 1 Then
            Return True
        Else
            Return False
        End If
       
    End Function

#End Region

End Class


