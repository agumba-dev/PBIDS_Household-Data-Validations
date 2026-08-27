Imports System.Data
Imports System.Data.SqlClient
Public Class clsMembership

#Region "variables and constructor"
    Private globalvariables As clsGlobalVariables = clsGlobalVariables.getObject
    Private da As clsDataAccess = clsDataAccess.getObject
    Private startEvents As String() = {"BIR", "ENT", "ENU", "TRI", "TRX"}
    Private endEvents As String() = {"DTH", "TRO", "EXT", "TRX"}
    Private Sub New()

    End Sub
    Public Sub New(ByVal validationtype As mhrsSyncValidationTypes)
        da.validationtype = validationtype
    End Sub
#End Region
#Region "procedures"
    Friend Function validateMembership(ByVal Membershiprecords As DataTable, ByVal tablename As String, ByVal worker As System.ComponentModel.BackgroundWorker) As Boolean
        Dim isValidrecord As Boolean = True
        If Membershiprecords.Rows.Count > 0 Then
            'Dim j As Integer = 0
            'If Not worker Is Nothing Then worker.ReportProgress(Nothing, "validating " & Membershiprecords.Rows.Count & " records in " & tablename & " " & Now.ToString())
            For Each record As DataRow In Membershiprecords.Rows
                'j = j + 1
                'If Not worker Is Nothing Then worker.ReportProgress(1, "Validating record " & j & " of  " & Membershiprecords.Rows.Count & " in " & tablename)
              
                isValidrecord = Me.validaterec(record, tablename)
                
            Next
            'If Not worker Is Nothing Then worker.ReportProgress(Nothing, "finished validating " & Membershiprecords.Rows.Count & " records in " & tablename & " " & Now.ToString())
        End If
        Return isValidrecord
    End Function
    Friend Function validaterec(ByVal Membershiprecord As DataRow, ByVal tablename As String) As Boolean
        Dim Village As String = da.getrecordsCompound(tablename, Membershiprecord).Trim
        Dim round As String = da.getrecordsRound(tablename, Membershiprecord).Trim
        Dim isValidrecord As Boolean = True

        'ResidencyID	1	uniqueidentifier
        'individid	2	varchar
        If Not clsUserDefinedFunctions.isValidIndividid(Membershiprecord("individid").ToString) Then
            Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "invalid individid", "", Now(), "", Village, round)
            isValidrecord = False
        End If
        If Not (IsDBNull(Membershiprecord("edate")) Or IsDBNull(Membershiprecord("eeventtype")) Or IsDBNull(Membershiprecord("eobserveid"))) Then
            Select Case Membershiprecord("rec_status").ToString.ToLower.Trim
                Case "i", "di", "ti", "mi"
                    Membershiprecord("rec_status") = "u"
                Case Else
            End Select
        End If
        If (IsDBNull(Membershiprecord("edate")) And IsDBNull(Membershiprecord("eeventtype")) And IsDBNull(Membershiprecord("eobserveid"))) Then
            Select Case Membershiprecord("rec_status").ToString.ToLower.Trim
                Case "u", "du", "tu", "mu"
                    Membershiprecord("rec_status") = "i"
                Case Else
            End Select
        End If
        If da.TempDbEpisodeHasEventAfterthisInMainDB(Membershiprecord, tablename) Then
            Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "There is an event in Main DB equal or after this", "", Now(), "", Village, round)
            isValidrecord = False
        End If
        If isValidrecord Then
            Select Case Membershiprecord("rec_status").ToString.ToLower.Trim
                Case "i", "di", "ti", "mi"
                    If da.hasstartEpisodePrecedenceConflict(Membershiprecord, tablename) Then
                        Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "episode has precedence conflict", "", Now(), "", Village, round)
                        isValidrecord = False
                    Else
                        If Me.validateStartofEpisode(Membershiprecord, tablename, Village, round) Then
                            isValidrecord = False
                        End If
                    End If
                Case "u", "du", "tu", "mu"
                    If da.hasEndEpisodePrecedenceConflict(Membershiprecord, tablename) Then
                        Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "episode has precedence conflict", "", Now(), "", Village, round)
                        isValidrecord = False
                    Else
                        If Me.validateEndofEpisode(Membershiprecord, tablename, Village, round) Then
                            isValidrecord = False
                        End If
                    End If
                Case Else
                    Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "Unknown operation", "", Now(), "", Village, round)
                    isValidrecord = False
            End Select
        End If
        'rec_status	12	varchar
        If Not isValidrecord Then
            Me.da.exec_nonqueryInTEMPDB("UPDATE " + tablename + " SET [errflag] = 'true' , errdate=getdate() where transit_id=" + Membershiprecord("transit_id").ToString)
        End If
        Return isValidrecord
    End Function
#End Region
#Region "Validation functions"
    Private Function validateStartofEpisode(ByVal Membershiprecord As DataRow, ByVal tablename As String, ByVal village As String, ByVal round As String) As Boolean
        Dim hasError As Boolean = False
        If hasDuplicatesINTemp(Membershiprecord) Then
            Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "Duplicate records in temp_DB", "", Now(), "", village, round)
            hasError = True
        Else
            'check if episode already exists
            If Me.Episodes_exists(Membershiprecord) Then
                Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "Episode already uploaded", "", Now(), "", village, round)
                hasError = True
            Else
                'socialgpid	3	varchar
                If Not clsUserDefinedFunctions.isValidLocationid(Membershiprecord("socialgpid").ToString) Then
                    Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "invalid socialgpid", "", Now(), "", village, round)
                    hasError = True
                End If
                'seventtype	4	char
                If Not isStartEvent(Membershiprecord("seventtype").ToString) Then
                    Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "invalid seventtype", "", Now(), "", village, round)
                    hasError = True
                End If
                'sdate	5	datetime
                If CDate(Membershiprecord("sdate")) > Now() Then
                    Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "invalid date sdate", "", Now(), "", village, round)
                    hasError = True
                End If
                'sobserveid	6	varchar
                If Not clsUserDefinedFunctions.isValidObservationid(Membershiprecord("sobserveid").ToString) Then
                    Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "invalid sobserveid", "", Now(), "", village, round)
                    hasError = True
                End If
                'sfieldworker	7	varchar

                'eeventtype	8	char
                If Not (IsDBNull(Membershiprecord("eeventtype")) Or IsDBNull(Membershiprecord("edate")) Or IsDBNull(Membershiprecord("eobserveid"))) Then
                    'Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "End episode should be null", "", Now(), "", village, round)
                    hasError = True
                End If
                'edate	9	datetime
                'If Not IsDBNull(Membershiprecord("edate")) Then
                '    Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "End episode should be null", "", Now(), "", village, round)
                '    hasError = True
                'End If

                ''eobserveid	10	varchar
                'If Not IsDBNull(Membershiprecord("eobserveid")) Then
                '    Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "End episode should be null", "", Now(), "", village, round)
                '    hasError = True
                'End If
                ''efieldworker	11	varchar

                'perform startevent reference validations
                If Me.validateStartEvent(Membershiprecord, tablename, village, round) Then
                    hasError = True
                End If
            End If
        End If

        Return hasError
    End Function
    Private Function validateEndofEpisode(ByVal Membershiprecord As DataRow, ByVal tablename As String, ByVal village As String, ByVal round As String) As Boolean

        Dim hasError As Boolean = False
        'Check if the record exists in MainDB
        'membershipID	1	uniqueidentifier
        If IsDBNull(Membershiprecord("edate")) Or IsDBNull(Membershiprecord("eeventtype")) Or IsDBNull(Membershiprecord("eobserveid")) Then
            If Me.da.validationtype = mhrsSyncValidationTypes.userpplication Then
                Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "the end episode has null attributes", "", Now(), "", village, round)
            End If
            hasError = True
        Else
            'eeventtype	8	char
            If Not isEndEvent(Membershiprecord("eeventtype").ToString) Then
                Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "invalid eeventtype", "", Now(), "", village, round)
                hasError = True
            End If
            'edate	9	datetime

            If CDate(Membershiprecord("edate")) > Now() Then
                Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "invalid date edate", "", Now(), "", village, round)
                hasError = True
            End If
            If CDate(Membershiprecord("sdate")) > CDate(Membershiprecord("edate")) Then
                Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "sdate is greater than edate", "", Now(), "", village, round)
                hasError = True
            End If
            'eobserveid	10	varchar
            If Not clsUserDefinedFunctions.isValidObservationid(Membershiprecord("eobserveid").ToString) Then
                Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "invalid eobserveid", "", Now(), "", village, round)
                hasError = True
            End If
            'efieldworker	11	varchar

            'Other
            If Not ((Membershiprecord("seventtype").ToString.ToUpper.Trim = "BIR")) Then
                If CDate(Membershiprecord("sdate")) = CDate(Membershiprecord("edate")) Then
                    Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "sdate is same as  edate", "", Now(), "", village, round)
                    hasError = True
                End If
            End If
            'If Membershiprecord("sobserveid").ToString.Trim = Membershiprecord("eobserveid").ToString.Trim Then
            '    Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "sobserveid is same as  eobserveid", "", Now(), "", village)
            '    hasError = True
            'End If

            'perform startevent reference validations
            If Me.validateEndEvent(Membershiprecord, tablename, village, round) Then
                hasError = True
            End If
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
    Private Function validateStartEvent(ByVal Membershiprecord As DataRow, ByVal tablename As String, ByVal village As String, ByVal round As String) As Boolean
        Dim hasError As Boolean = False
        If isStartEvent(Membershiprecord("seventtype").ToString) Then
            Select Case Membershiprecord("seventtype").ToString.ToUpper.Trim
                'should be the first episode for individual
                Case "ENU", "BIR"
                    'check if individual has another episode
                    If Me.hasEpisodes(Membershiprecord("individid").ToString) Then
                        Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "Individual has another episode", "", Now(), "", village, round)
                        hasError = True
                    End If
                Case "ENT"
                    'check if its a return migrant
                    If Me.hasEpisodes(Membershiprecord("individid").ToString) Then
                        'last event for return migrants should be 'EXT'
                        If Me.getLastEvent(Membershiprecord("individid").ToString) <> "EXT" Then
                            Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "the last event of return migrant is not EXT", "", Now(), "", village, round)
                            hasError = True
                        Else
                            'if last event was an 'EXT' then
                            'check if there is another event that occured in the same date
                            If Me.eventDateExists(Membershiprecord) Then
                                Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "the event date already exists", "", Now(), "", village, round)
                                hasError = True
                            End If
                            'check if the individual had died
                            If Me.hadDied(Membershiprecord("individid").ToString) Then
                                Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "Individual has a DTH event", "", Now(), "", village, round)
                                hasError = True
                            End If
                            'individual should have only one open episode
                            If Me.hasOpenEpisode(Membershiprecord) Then
                                Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "Individual has another open episode", "", Now(), "", village, round)
                                hasError = True
                            End If
                        End If
                    Else

                    End If
                Case "TRI"
                    'check if its has episodes
                    If Me.hasEpisodes(Membershiprecord("individid").ToString) Then
                        'last event for return migrants should be 'TRO'
                        If Not Me.TRIhasValideTRO(Membershiprecord) Then
                            Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "the last event for the TRI is not a TRO", "", Now(), "", village, round)
                            hasError = True
                        Else
                            'if last event was an 'EXT' then
                            'check if there is another event that occured in the same date
                            If Me.eventDateExists(Membershiprecord) Then
                                Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "the event date already exists", "", Now(), "", village, round)
                                hasError = True
                            End If
                            'check if the individual had died
                            If Me.hadDied(Membershiprecord("individid").ToString) Then
                                Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "Individual has a DTH event", "", Now(), "", village, round)
                                hasError = True
                            End If
                            'individual should have only one open episode
                            If Me.hasOpenEpisode(Membershiprecord) Then
                                Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "Individual has another open episode", "", Now(), "", village, round)
                                hasError = True
                            End If
                        End If
                    Else
                        'if individual has no episodes
                        Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "Has no previous episodes", "", Now(), "", village, round)
                        hasError = True
                    End If

                Case "TRX"
                    'check if its has episodes
                    If Me.hasEpisodes(Membershiprecord("individid").ToString) Then
                        'last event for return migrants should be 'TRO'
                        If Not Me.TRXhasValideTRX(Membershiprecord) Then
                            Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "the last event for the TRX is not a TRX", "", Now(), "", village, round)
                            hasError = True
                        Else
                            'if last event was an 'EXT' then
                            'check if there is another event that occured in the same date
                            If Me.eventDateExists(Membershiprecord) Then
                                Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "the event date already exists", "", Now(), "", village, round)
                                hasError = True
                            End If
                            'check if the individual had died
                            If Me.hadDied(Membershiprecord("individid").ToString) Then
                                Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "Individual has a DTH event", "", Now(), "", village, round)
                                hasError = True
                            End If
                            'individual should have only one open episode
                            If Me.hasOpenEpisode(Membershiprecord) Then
                                Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "Individual has another open episode", "", Now(), "", village, round)
                                hasError = True
                            End If
                        End If
                    Else
                        'if individual has no episodes
                        Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "Has no previous episodes", "", Now(), "", village, round)
                        hasError = True
                    End If

                Case Else
                    'Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "Unknown start event", "", Now(), "")
                    'hasError = True
            End Select
        Else
            hasError = True
        End If
        Return hasError
    End Function
    Private Function validateEndEvent(ByVal Membershiprecord As DataRow, ByVal tablename As String, ByVal village As String, ByVal round As String) As Boolean
        Dim hasError As Boolean = False
        If isEndEvent(Membershiprecord("eeventtype").ToString) Then
            If Me.episodeIsINMembership(Membershiprecord) Then
                'check if individual has open episode at place of death
                If Me.hasOpenEpisodeInSocialGroup(Membershiprecord) Then
                    'check if there is another event that occured in the same date
                    If Me.eventDateExists(Membershiprecord) Then
                        Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "the event date already exists", "", Now(), "", village, round)
                        hasError = True
                    End If
                    'check for four calender month rule
                    If Not Me.meetsFourCalenderMonthsRule(Membershiprecord) Then
                        '' Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "Individual has no open episode in location", "", Now(), "")
                        '' hasError = True
                    End If
                Else
                    Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "Individual has no open episode in socialgroup", "", Now(), "", village, round)
                    hasError = True
                    Return hasError
                End If

                ' perform event specific validations
                Select Case Membershiprecord("eeventtype").ToString.ToUpper.Trim
                    Case "DTH"

                    Case "TRO"

                    Case "EXT"

                    Case "TRX"

                    Case Else
                        'Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "Unknown end event", "", Now(), "")
                        'hasError = True
                End Select
            Else
                Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "episode not in membership", "", Now(), "", village, round)
                hasError = True
            End If
        Else
            hasError = True
        End If
        Return hasError
    End Function
#End Region
#Region "reference validations functions"
    Private Function eventDateExists(ByVal Membershiprecord As DataRow) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = ""
        Select Case Membershiprecord("rec_status").ToString.ToLower.Trim
            Case "i", "di", "ti"
                sql = "SELECT count(*) FROM [DSSHRS].[DSS].[membership] " _
                    & " where (sdate is not null) and cast(floor(cast([sate] as float)) as  datetime)= '" + CDate(Membershiprecord("sdate")).ToString + "' " _
                    & "  and (individid='" + Membershiprecord("individid").ToString.Trim + "')"
                If Me.da.executeScalar_INMainDB(sql) > 0 Then
                    returnValue = True
                Else
                    returnValue = False
                End If
            Case "u", "du", "tu"
                sql = "SELECT count(*)  FROM [DSSHRS].[DSS].[membership] " _
                    & " where (edate is not null)  and cast(floor(cast([edate] as float)) as  datetime)= '" + CDate(Membershiprecord("edate")).ToString + "' " _
                    & " and (individid='" + Membershiprecord("individid").ToString.Trim + "')"
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
    Private Function hadDied(ByVal individid As String) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT count(*)  FROM [DSSHRS].[DSS].[membership]" _
                            & "where (eeventtype='DTH') and (individid='" + individid + "')"
        If Me.da.executeScalar_INMainDB(sql) > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function
    Private Function hasEpisodes(ByVal individid As String) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT count(*)  FROM [DSSHRS].[DSS].[membership]" _
                            & "where  (individid='" + individid + "')"
        If Me.da.executeScalar_INMainDB(sql) > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function
    Private Function Episodes_exists(ByVal membershiprecord As DataRow) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT count(*)  FROM [DSSHRS].[DSS].[membership]" _
                            & "where  (memberShipID='" + membershiprecord("memberShipID").ToString + "')"
        If Me.da.executeScalar_INMainDB(sql) > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function
    Private Function hasOpenEpisodeInSocialGroup(ByVal Membershiprecord As DataRow) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT count(*) FROM [DSSHRS].[DSS].[membership]" _
                            & " where  (individid='" + Membershiprecord("individid").ToString + "') and (eeventtype is null or eeventtype='' )" _
                            & " and  (eobserveid is null) and (edate is null) and ([socialgpid]='" + Membershiprecord("socialgpid").ToString + "')"
        If Me.da.executeScalar_INMainDB(sql) > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function
    Private Function hasOpenEpisode(ByVal Membershiprecord As DataRow) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT count(*) FROM [DSSHRS].[DSS].[membership]" _
                            & " where  (individid='" + Membershiprecord("individid").ToString + "') and (eeventtype is null or eeventtype='' )" _
                            & " and  (eobserveid is null) and (edate is null) "
        If Me.da.executeScalar_INMainDB(sql) > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function
    Private Function episodeIsINMembership(ByVal Membershiprecord As DataRow) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT count(*) FROM [DSSHRS].[DSS].[membership]" _
                            & " where  (membershipID='" + Membershiprecord("membershipid").ToString + "') "
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
    Private Function Isregistered(ByVal individid As String) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT count(*)  FROM [DSSHRS].[DSS].[individual]" _
                            & "where  (individid='" + individid + "')"
        If Me.da.executeScalar_INMainDB(sql) > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function
    Private Function getLastEvent(ByVal individid As String) As String
        Dim returnValue As String = Nothing
        Dim sql As String = "SELECT distinct [lastevent]  FROM [DSSHRS].[dbo].[lastmemberships]" _
                            & "where  (individid='" + individid + "') "
        returnValue = Me.da.getScalar_inMainDB(sql).ToString.Trim
        Return returnValue
    End Function
    Private Function TRIhasValideTRO(ByVal Membershiprecord As DataRow) As Boolean
        Dim socialgroupid As Object = Nothing
        Dim sql As String = "SELECT socialgpid  FROM [DSSHRS].[dbo].[lastmemberships]" _
                            & " where  (individid='" + Membershiprecord("individid").ToString + "') and (lastevent ='TRO') " _
                            & " and cast(floor(cast([date] as float)) as  datetime)= '" + CDate(Membershiprecord("sdate")).ToString + "'"
        socialgroupid = Me.da.getScalar_inMainDB(sql)
        If Not socialgroupid Is Nothing Then
            If socialgroupid.ToString.Trim = Membershiprecord("socialgpid").ToString.Trim Then
                Return False
            Else
                Return True
            End If
        Else
            Return False
        End If
    End Function
    Private Function TRXhasValideTRX(ByVal Membershiprecord As DataRow) As Boolean
        Dim socialgroupid As Integer = 0
        Dim sql As String = "SELECT count(*)  FROM [DSSHRS].[dbo].[lastmemberships]" _
                            & " where  (individid='" + Membershiprecord("individid").ToString + "') and (lastevent ='TRX') " _
                            & " and cast(floor(cast([date] as float)) as  datetime)= '" + CDate(Membershiprecord("sdate")).ToString + "' " _
                            & "  and (ltrim(socialgpid)<>'" + Membershiprecord("socialgpid").ToString.Trim + "') "
        socialgroupid = Me.da.getScalar_inMainDB(sql)
        If socialgroupid > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Function meetsFourCalenderMonthsRule(ByVal Membershiprecord As DataRow) As Boolean
        Dim currentEventDate As Date
        Select Case Membershiprecord("rec_status").ToString.ToLower.Trim
            Case "i", "di", "ti"
                currentEventDate = CDate(Membershiprecord("sdate"))
            Case "u", "du", "tu"
                currentEventDate = CDate(Membershiprecord("edate"))
        End Select
        Dim sql As String = "SELECT max([sdate])  FROM [DSSHRS].[DSS].[membership] " _
                           & "where  (individid='" + Membershiprecord("individid").ToString + "')"
        Dim lastdate As Date = CDate(Me.da.getScalar_inMainDB(sql))
        If clsUserDefinedFunctions.meetsFourMonthsRule(lastdate, currentEventDate) Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Function hasDuplicatesINTemp(ByVal memberShiprecord As DataRow) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT count(*)  FROM [TEMP_DSSHRS].[DSS].[membership]" _
                            & "where  (rec_status in('DI','I','TI') )and (memberShipID='" + memberShiprecord("memberShipID").ToString + "') "
        If Me.da.executeScalar_INMainDB(sql) > 1 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function
#End Region

End Class
