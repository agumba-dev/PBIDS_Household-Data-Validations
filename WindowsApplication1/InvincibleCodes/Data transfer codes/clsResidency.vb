Imports System.Data
Imports System.Data.SqlClient
Public Class clsResidency

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
    Friend Function validateResidency(ByVal Residencyrecords As DataTable, ByVal tablename As String, ByVal worker As System.ComponentModel.BackgroundWorker) As Boolean
        Dim isValidrecord As Boolean = True
        If Residencyrecords.Rows.Count > 0 Then
            'Dim j As Integer = 0
            'worker.ReportProgress(Nothing, "validating " & Residencyrecords.Rows.Count & " records in " & tablename & " " & Now.ToString())
            For Each record As DataRow In Residencyrecords.Rows
                'j = j + 1
                'worker.ReportProgress(1, "Validating record " & j & " of  " & Residencyrecords.Rows.Count & " in " & tablename)
                isValidrecord = Me.validaterec(record, tablename)
            Next
            'worker.ReportProgress(Nothing, "finished validating " & Residencyrecords.Rows.Count & " records in " & tablename & " " & Now.ToString())
        End If
        Return isValidrecord
    End Function
    Friend Function validaterec(ByVal Residencyrecord As DataRow, ByVal tablename As String) As Boolean
        'reset errror column

        Dim Village As String = da.getrecordsCompound(tablename, Residencyrecord).Trim
        Dim isValidrecord As Boolean = True
        Dim round As String = da.getrecordsRound(tablename, Residencyrecord).Trim
        'ResidencyID	1	uniqueidentifier
        'individid	2	varchar
        If Not clsUserDefinedFunctions.isValidIndividid(Residencyrecord("individid").ToString) Then
            Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "invalid individid", "", Now(), "", Village, round)
            isValidrecord = False
        End If

        If Not (IsDBNull(Residencyrecord("edate")) Or IsDBNull(Residencyrecord("eeventtype")) Or IsDBNull(Residencyrecord("eobserveid"))) Then
            Select Case Residencyrecord("rec_status").ToString.ToLower.Trim
                Case "i", "di", "ti", "mi"
                    Residencyrecord("rec_status") = "u"
                Case Else
            End Select
        End If
        If (IsDBNull(Residencyrecord("edate")) And IsDBNull(Residencyrecord("eeventtype")) And IsDBNull(Residencyrecord("eobserveid"))) Then
            Select Case Residencyrecord("rec_status").ToString.ToLower.Trim
                Case "u", "du", "tu", "mu"
                    Residencyrecord("rec_status") = "i"
                Case Else
            End Select
        End If
        'validate the event date should not be prior to the individuals dob.

        




        If da.TempDbEpisodeHasEventAfterthisInMainDB(Residencyrecord, tablename) Then
            Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "There is an event in Main DB equal or after this", "", Now(), "", Village, round)
            isValidrecord = False
        End If

        If isValidrecord Then
            Select Case Residencyrecord("rec_status").ToString.ToLower.Trim
                Case "i", "di", "ti", "mi"
                    If da.hasstartEpisodePrecedenceConflict(Residencyrecord, tablename) Then
                        Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "episode has precedence conflict", "", Now(), "", Village, round)
                        isValidrecord = False
                    Else
                        If Me.validateStartofEpisode(Residencyrecord, tablename, Village, round) Then
                            isValidrecord = False
                        End If
                    End If

                Case "u", "du", "tu", "mu"
                    If da.hasEndEpisodePrecedenceConflict(Residencyrecord, tablename) Then
                        Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "episode has precedence conflict", "", Now(), "", Village, round)
                        isValidrecord = False
                    Else
                        If Me.validateEndofEpisode(Residencyrecord, tablename, Village, round) Then
                            isValidrecord = False
                        End If
                    End If

                Case Else
                    Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "Unknown operation", "", Now(), "", Village, round)
                    isValidrecord = False
            End Select
        End If
        'rec_status	12	varchar
        If Not isValidrecord Then
            Me.da.exec_nonqueryInTEMPDB("UPDATE " + tablename + " SET [errflag] = 'true', errdate=getdate() where transit_id=" + Residencyrecord("transit_id").ToString)
        End If
        Return isValidrecord
    End Function
#End Region
#Region "Validation functions"
    Private Function validateStartofEpisode(ByVal Residencyrecord As DataRow, ByVal tablename As String, ByVal village As String, ByVal round As String) As Boolean
        Dim hasError As Boolean = False

        'check for duplicates
        If hasDuplicatesINTemp(Residencyrecord) Then
            Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "It is a duplicate record in temp_dsshrs", "", Now(), "", village, round)
            hasError = True
        Else
            'check if episode already exists
            If Me.Episodes_exists(Residencyrecord) Then
                Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "Episode already uploaded", "", Now(), "", village, round)
                hasError = True
            Else
                'locationid	3	varchar
                If Not clsUserDefinedFunctions.isValidLocationid(Residencyrecord("locationid").ToString) Then
                    Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "invalid locationid", "", Now(), "", village, round)
                    hasError = True
                End If
                'seventtype	4	char
                If Not isStartEvent(Residencyrecord("seventtype").ToString) Then
                    Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "invalid seventtype", "", Now(), "", village, round)
                    hasError = True
                End If
                'sdate	5	datetime
                If CDate(Residencyrecord("sdate")) > Now() Then
                    Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "invalid date sdate", "", Now(), "", village, round)
                    hasError = True
                End If
                'sobserveid	6	varchar
                If Not clsUserDefinedFunctions.isValidObservationid(Residencyrecord("sobserveid").ToString) Then
                    Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "invalid sobserveid", "", Now(), "", village, round)
                    hasError = True
                End If
                'sfieldworker	7	varchar

                'eeventtype	8	char /'edate	9	datetime/ 'eobserveid	10	varchar
                If Not (IsDBNull(Residencyrecord("eeventtype")) Or IsDBNull(Residencyrecord("edate")) Or IsDBNull(Residencyrecord("eobserveid"))) Then
                    'Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "End episode should be null", "", Now(), "", village, round)
                    hasError = True
                End If
                ''edate	9	datetime
                'If Not IsDBNull(Residencyrecord("edate")) Then
                '    Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "End episode should be null", "", Now(), "", village, round)
                '    hasError = True
                'End If

                ''eobserveid	10	varchar
                'If Not IsDBNull(Residencyrecord("eobserveid")) Then
                '    Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "End episode should be null", "", Now(), "", village, round)
                '    hasError = True
                'End If
                'efieldworker	11	varchar

                'perform startevent reference validations
                If Me.validateStartEvent(Residencyrecord, tablename, village, round) Then
                    hasError = True

                End If
            End If
        End If
        Return hasError
    End Function
    Private Function validateEndofEpisode(ByVal Residencyrecord As DataRow, ByVal tablename As String, ByVal village As String, ByVal round As String) As Boolean

        Dim hasError As Boolean = False
        'Check if the record exists in MainDB
        'ResidencyID	1	uniqueidentifier
        If IsDBNull(Residencyrecord("edate")) Or IsDBNull(Residencyrecord("eeventtype")) Or IsDBNull(Residencyrecord("eobserveid")) Then
            If Me.da.validationtype = mhrsSyncValidationTypes.userpplication Then
                Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "the end episode has null attributes", "", Now(), "", village, round)
            End If
            hasError = True
        Else
            'eeventtype	8	char
            If Not isEndEvent(Residencyrecord("eeventtype").ToString) Then
                Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "invalid eeventtype", "", Now(), "", village, round)
                hasError = True
            End If
            'edate	9	datetime

            If CDate(Residencyrecord("edate")) > Now() Then
                Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "invalid date edate", "", Now(), "", village, round)
                hasError = True
            End If
            If CDate(Residencyrecord("sdate")) > CDate(Residencyrecord("edate")) Then
                Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "sdate is greater than edate", "", Now(), "", village, round)
                hasError = True
            End If
            'eobserveid	10	varchar
            If Not clsUserDefinedFunctions.isValidObservationid(Residencyrecord("eobserveid").ToString) Then
                Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "invalid eobserveid", "", Now(), "", village, round)
                hasError = True
            End If
            'efieldworker	11	varchar
            'Other
            If Not ((Residencyrecord("seventtype").ToString.ToUpper.Trim = "BIR")) Then
                If CDate(Residencyrecord("sdate")) = CDate(Residencyrecord("edate")) Then
                    Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "sdate is same as  edate", "", Now(), "", village, round)
                    hasError = True
                End If
            End If
            'If Residencyrecord("sobserveid").ToString.Trim = Residencyrecord("eobserveid").ToString.Trim Then
            '    Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "sobserveid is same as  eobserveid", "", Now(), "", village)
            '    hasError = True
            'End If

            'perform startevent reference validations
            If Me.validateEndEvent(Residencyrecord, tablename, village, round) Then
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
    Private Function validateStartEvent(ByVal Residencyrecord As DataRow, ByVal tablename As String, ByVal village As String, ByVal round As String) As Boolean
        Dim hasError As Boolean = False
        If isStartEvent(Residencyrecord("seventtype").ToString) Then
            Select Case Residencyrecord("seventtype").ToString.ToUpper.Trim
                'should be the first episode for individual
                Case "ENU", "BIR"
                    'check if individual has another episode
                    If Me.hasEpisodes(Residencyrecord("individid").ToString) Then
                        Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "Individual has another episode", "", Now(), "", village, round)
                        hasError = True
                    End If
                Case "ENT"
                    'check if its a return migrant
                    If Me.hasEpisodes(Residencyrecord("individid").ToString) Then
                        'last event for return migrants should be 'EXT'
                        If Me.getLastEvent(Residencyrecord("individid").ToString) <> "EXT" Then
                            Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "the last event of return migrant is not EXT", "", Now(), "", village, round)
                            hasError = True
                        Else
                            'if last event was an 'EXT' then
                            'check if there is another event that occured in the same date
                            If Me.eventDateExists(Residencyrecord) Then
                                Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "the event date already exists", "", Now(), "", village, round)
                                hasError = True
                            End If
                            'check if the individual had died
                            If Me.hadDied(Residencyrecord("individid").ToString) Then
                                Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "Individual has a DTH event", "", Now(), "", village, round)
                                hasError = True
                            End If
                            'individual should have only one open episode
                            If Me.hasOpenEpisode(Residencyrecord) Then
                                Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "Individual has another open episode", "", Now(), "", village, round)
                                hasError = True
                            End If
                        End If
                    Else

                    End If
                Case "TRI"
                    'check if its has episodes
                    If Me.hasEpisodes(Residencyrecord("individid").ToString) Then
                        'last event for return migrants should be 'TRO'
                        If Not Me.TRIhasValideTRO(Residencyrecord) Then
                            Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "the last event for the TRI is not a TRO", "", Now(), "", village, round)
                            hasError = True
                        Else
                            'if last event was an 'EXT' then
                            'check if there is another event that occured in the same date
                            If Me.eventDateExists(Residencyrecord) Then
                                Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "the event date already exists", "", Now(), "", village, round)
                                hasError = True
                            End If
                            'check if the individual had died
                            If Me.hadDied(Residencyrecord("individid").ToString) Then
                                Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "Individual has a DTH event", "", Now(), "", village, round)
                                hasError = True
                            End If
                            'individual should have only one open episode
                            If Me.hasOpenEpisode(Residencyrecord) Then
                                Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "Individual has another open episode", "", Now(), "", village, round)
                                hasError = True
                            End If
                        End If
                    Else
                        'if individual has no episodes
                        Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "Has no previous episodes", "", Now(), "", village, round)
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
    Private Function validateEndEvent(ByVal Residencyrecord As DataRow, ByVal tablename As String, ByVal village As String, ByVal round As String) As Boolean
        Dim hasError As Boolean = False
        If isEndEvent(Residencyrecord("eeventtype").ToString) Then
            If Me.episodeIsINResidency(Residencyrecord) Then
                'check if individual has open episode at place of death
                If Me.hasOpenEpisodeInLocation(Residencyrecord) Then
                    'check if there is another event that occured in the same date
                    If Me.eventDateExists(Residencyrecord) Then
                        Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "the event date already exists", "", Now(), "", village, round)
                        hasError = True
                    End If
                    'check for four calender month rule
                    If Not Me.meetsFourCalenderMonthsRuleUsingvisitation(Residencyrecord) Then

                        ' If Not ((Residencyrecord("seventtype").ToString.ToUpper.Trim = "BIR") And (Residencyrecord("eeventtype").ToString.ToUpper.Trim = "DTH")) Then
                        Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "Individual has not met the 4 calendar Month", "", Now(), "", village, round)
                        hasError = True
                        'End If

                    End If
                Else
                    Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "Individual has no open episode in location", "", Now(), "", village, round)
                    hasError = True
                    Return hasError
                End If

                ' perform event specific validations
                Select Case Residencyrecord("eeventtype").ToString.ToUpper.Trim
                    Case "DTH"

                    Case "TRO"

                    Case "EXT"

                    Case Else
                        'Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "Unknown end event", "", Now(), "")
                        'hasError = True
                End Select
            Else
                Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "episode not in residency", "", Now(), "", village, round)
                hasError = True
            End If
        Else
            hasError = True
        End If
        Return hasError
    End Function
#End Region
#Region "reference validations functions"
    Private Function eventDateExists(ByVal Residencyrecord As DataRow) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = ""
        Select Case Residencyrecord("rec_status").ToString.ToLower.Trim
            Case "i", "di", "ti"
                sql = "SELECT count(*) FROM [DSSHRS].[DSS].[residency] " _
                    & " where (sdate is not null) and (cast(floor(cast(sdate as float)) as datetime)='" + CDate(Residencyrecord("sdate")).ToString("dd-MMM-yyyy") + "') and (individid='" + Residencyrecord("individid").ToString.Trim + "') and seventtype <>'bir'"
                If Me.da.executeScalar_INMainDB(sql) > 0 Then
                    returnValue = True
                Else
                    returnValue = False
                End If
            Case "u", "du", "tu"
                sql = "SELECT count(*)  FROM [DSSHRS].[DSS].[residency] " _
                    & " where (edate is not null) and (cast(floor(cast(sdate as float)) as datetime)='" + CDate(Residencyrecord("sdate")).ToString("dd-MMM-yyyy") + "') and (individid='" + Residencyrecord("individid").ToString.Trim + "')  and seventtype <>'bir'"
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
        Dim sql As String = "SELECT count(*)  FROM [DSSHRS].[DSS].[residency]" _
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
        Dim sql As String = "SELECT count(*)  FROM [DSSHRS].[DSS].[residency]" _
                            & "where  (individid='" + individid + "')"
        If Me.da.executeScalar_INMainDB(sql) > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function
    Private Function Episodes_exists(ByVal Residencyrecord As DataRow) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT count(*)  FROM [DSSHRS].[DSS].[residency]" _
                            & "where  (ResidencyID='" + Residencyrecord("ResidencyID").ToString + "')"
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
                            & " and  (eobserveid is null) and (edate is null) and ([locationid]='" + Residencyrecord("locationid").ToString + "')"
        If Me.da.executeScalar_INMainDB(sql) > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function
    Private Function hasOpenEpisode(ByVal Residencyrecord As DataRow) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT count(*) FROM [DSSHRS].[DSS].[residency]" _
                            & " where  (individid='" + Residencyrecord("individid").ToString + "') and (eeventtype is null or eeventtype='' )" _
                            & " and  (eobserveid is null) and (edate is null) "
        If Me.da.executeScalar_INMainDB(sql) > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function
    Private Function episodeIsINResidency(ByVal Residencyrecord As DataRow) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT count(*) FROM [DSSHRS].[DSS].[residency]" _
                            & " where  (ResidencyID='" + Residencyrecord("ResidencyID").ToString + "') "
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
        Dim sql As String = "SELECT [lastevent]  FROM [DSSHRS].[dbo].[lastResidency]" _
                            & "where  (individid='" + individid + "')"
        returnValue = Me.da.getScalar_inMainDB(sql).ToString.Trim
        Return returnValue
    End Function
    Private Function TRIhasValideTRO(ByVal Residencyrecord As DataRow) As Boolean
        Dim locationid As Object = Nothing
        Dim sdate As Date = Residencyrecord("sdate")
        Dim sql As String = "SELECT locationid  FROM [DSSHRS].[dbo].[lastResidency]" _
                            & " where  (individid='" + Residencyrecord("individid").ToString + "') and (lastevent='TRO')  " _
                            & " and (year([date])='" + sdate.Year.ToString + "') and " _
                            & " (month([date])='" + sdate.Month.ToString + "') " _
                            & " and (day([date])='" + sdate.Day.ToString + "')"
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
        Select Case Residencyrecord("rec_status").ToString.ToLower.Trim
            Case "i", "di", "ti"
                currentEventDate = CDate(Residencyrecord("sdate"))
            Case "u", "du", "tu"
                currentEventDate = CDate(Residencyrecord("edate"))
        End Select
        Dim sql As String = "SELECT max([sdate])  FROM [DSSHRS].[DSS].[residency] " _
                           & "where  (individid='" + Residencyrecord("individid").ToString + "')"
        Dim lastdate As Date = CDate(Me.da.getScalar_inMainDB(sql))
        If clsUserDefinedFunctions.meetsFourMonthsRule(lastdate, currentEventDate) Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Checks the four calendar month rule using the dates available
    ''' </summary>
    ''' <param name="Residencyrecord"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
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

        Dim sql As String = "dsshrs.[DSS].[Res_meetsfourCalendarMonth2]  '" + individid + "','" + guid + "','" + locationid + "','" + sevent + "','" + sdate + "','" + _
            sobserve + "','" + edate + "','" + eevent + "','" + eobserve + "'," + etype.ToString


        'Dim sql As String = "DSSHRS.[DSS].[Res_meetsfourCalendarMonth]  '" + Residencyrecord("individid").ToString + "' ,'" + Residencyrecord("ResidencyID").ToString + "'," + etype.ToString
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
    Private Function hasDuplicatesINTemp(ByVal Residencyrecord As DataRow) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT count(*)  FROM [TEMP_DSSHRS].[DSS].[residency]" _
                            & "where  (rec_status in('DI','I', 'TI') )and (ResidencyID='" + Residencyrecord("ResidencyID").ToString + "')"
        If Me.da.executeScalar_INMainDB(sql) > 1 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function
    
#End Region

End Class
