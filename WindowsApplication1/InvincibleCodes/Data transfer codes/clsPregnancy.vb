Imports System.Data
Imports System.Data.SqlClient
Public Class clsPregnancy
#Region "variables and constructor"
    Private globalvariables As clsGlobalVariables = clsGlobalVariables.getObject
    Private da As clsDataAccess = clsDataAccess.getObject
    Private startEvents As String() = {"PRX", "ENT", "ENU", "PRN"}
    'Private endEvents As String() = {"EXT", "TRO", "DTH", "MULLBR", "LBRSTB", "SINLBR", "MULSTB", "SINSTB", "MISCAR"}
    Private endEvents As String() = {"EXT", "TRO", "DTH", "MULLBR", "LBRSTB", "SINLBR", "MULSTB", "SINSTB", "MISCAR", "BIR" _
     , "BIR", "CEN", "EXT", "PRO", "DTH", "NOTAPP", "NAP", "DTH/EX", "SINSTB", "SINLBR", "LBR", "MISCAR", "TRO", "MULSTB", "MULLBR", "SABORT", "IABORT"}

    Private Sub New()

    End Sub
    Public Sub New(ByVal validationtype As mhrsSyncValidationTypes)
        da.validationtype = validationtype
    End Sub
#End Region
#Region "procedures"
    Friend Function validatePregnancy(ByVal Pregnancyrecords As DataTable, ByVal tablename As String, ByVal worker As System.ComponentModel.BackgroundWorker) As Boolean
        Dim isValidrecord As Boolean = True
        If Pregnancyrecords.Rows.Count > 0 Then
            'Dim j As Integer = 0
            'worker.ReportProgress(Nothing, "validating " & Pregnancyrecords.Rows.Count & " records in " & tablename & " " & Now.ToString())

            For Each record As DataRow In Pregnancyrecords.Rows
                ' j = j + 1
                'worker.ReportProgress(1, "Validating record " & j & " of  " & Pregnancyrecords.Rows.Count & " in " & tablename)
                isValidrecord = Me.validaterec(record, tablename)
            Next
            'worker.ReportProgress(Nothing, "finished validating " & Pregnancyrecords.Rows.Count & " records in " & tablename & " " & Now.ToString())

        End If
        Return isValidrecord
    End Function
    Friend Function validaterec(ByVal Pregnancyrecord As DataRow, ByVal tablename As String) As Boolean
        Dim Village As String = da.getrecordsCompound(tablename, Pregnancyrecord).Trim
        Dim round As String = da.getrecordsRound(tablename, Pregnancyrecord).Trim
        Dim isValidrecord As Boolean = True

        'ResidencyID	1	uniqueidentifier
        'individid	2	varchar
        If Not clsUserDefinedFunctions.isValidIndividid(Pregnancyrecord("individid").ToString) Then
            Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "invalid individid", "", Now(), "", Village, round)
            isValidrecord = False
        End If
        If Not (IsDBNull(Pregnancyrecord("edate")) Or IsDBNull(Pregnancyrecord("eeventtype")) Or IsDBNull(Pregnancyrecord("eobserveid"))) Then
            Select Case Pregnancyrecord("rec_status").ToString.ToLower.Trim
                Case "i", "di", "ti", "mi"
                    Pregnancyrecord("rec_status") = "u"
                Case Else
            End Select
        End If
        If (IsDBNull(Pregnancyrecord("edate")) And IsDBNull(Pregnancyrecord("eeventtype")) And IsDBNull(Pregnancyrecord("eobserveid"))) Then
            Select Case Pregnancyrecord("rec_status").ToString.ToLower.Trim
                Case "u", "du", "tu", "mu"
                    Pregnancyrecord("rec_status") = "i"
                Case Else
            End Select
        End If

        If da.TempDbEpisodeHasEventAfterthisInMainDB(Pregnancyrecord, tablename) Then
            Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "There is an event in Main DB equal or after this", "", Now(), "", Village, round)
            isValidrecord = False
        End If
        If isValidrecord Then
            Select Case Pregnancyrecord("rec_status").ToString.ToLower.Trim
                Case "i", "di", "ti", "mi"
                    If da.hasstartEpisodePrecedenceConflict(Pregnancyrecord, tablename) Then
                        Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "episode has precedence conflict", "", Now(), "", Village, round)
                        isValidrecord = False
                    Else
                        If Me.validateStartofEpisode(Pregnancyrecord, tablename, Village, round) Then
                            isValidrecord = False
                        End If
                    End If
                Case "u", "du", "tu", "mu"
                    If da.hasEndEpisodePrecedenceConflict(Pregnancyrecord, tablename) Then
                        Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "episode has precedence conflict", "", Now(), "", Village, round)
                        isValidrecord = False
                    Else
                        If Me.validateEndofEpisode(Pregnancyrecord, tablename, Village, round) Then
                            isValidrecord = False
                        End If
                    End If
                Case Else
                    Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "Unknown operation", "", Now(), "", Village, round)
                    isValidrecord = False
            End Select
        End If
        'rec_status	12	varchar
        If Not isValidrecord Then
            Me.da.exec_nonqueryInTEMPDB("UPDATE " + tablename + " SET [errflag] = 'true', errdate=getdate() where transit_id=" + Pregnancyrecord("transit_id").ToString)
        End If
        Return isValidrecord
    End Function
#End Region
#Region "Validation functions"
    Private Function validateStartofEpisode(ByVal Pregnancyrecord As DataRow, ByVal tablename As String, ByVal village As String, ByVal round As String) As Boolean
        Dim hasError As Boolean = False
        If hasDuplicatesINTemp(Pregnancyrecord) Then
            Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "Duplicate record in temp_DB", "", Now(), "", village, round)
            hasError = True
        Else
            'check if episode already exists
            If Me.Episodes_exists(Pregnancyrecord) Then
                Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "Episode already uploaded", "", Now(), "", village, round)
                hasError = True
            Else
                'locationid	3	varchar
                If Not clsUserDefinedFunctions.isValidLocationid(Pregnancyrecord("locationid").ToString) Then
                    Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "invalid locationid", "", Now(), "", village, round)
                    hasError = True
                End If
                'seventtype	4	char
                If Not isStartEvent(Pregnancyrecord("seventype").ToString) Then
                    Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "invalid seventype", "", Now(), "", village, round)
                    hasError = True
                End If
                'sdate	5	datetime
                If CDate(Pregnancyrecord("sdate")) > Now() Then
                    Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "invalid date sdate", "", Now(), "", village, round)
                    hasError = True
                End If
                'sobserveid	6	varchar
                If Not clsUserDefinedFunctions.isValidObservationid(Pregnancyrecord("sobserveid").ToString) Then
                    Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "invalid sobserveid", "", Now(), "", village, round)
                    hasError = True
                End If
                'sfieldworker	7	varchar

                'eeventtype	8	char
                If Not IsDBNull(Pregnancyrecord("eeventtype")) Then
                    Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "End episode should be null", "", Now(), "", village, round)
                    hasError = True
                End If
                'edate	9	datetime
                If Not IsDBNull(Pregnancyrecord("edate")) Then
                    Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "End episode should be null", "", Now(), "", village, round)
                    hasError = True
                End If

                'eobserveid	10	varchar
                If Not IsDBNull(Pregnancyrecord("eobserveid")) Then
                    Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "End episode should be null", "", Now(), "", village, round)
                    hasError = True
                End If
                'efieldworker	11	varchar

                'perform startevent reference validations
                If Me.validateStartEvent(Pregnancyrecord, tablename, village, round) Then
                    hasError = True
                End If
            End If
        End If
        Return hasError
    End Function
    Private Function validateEndofEpisode(ByVal Pregnancyrecord As DataRow, ByVal tablename As String, ByVal village As String, ByVal round As String) As Boolean

        Dim hasError As Boolean = False
        'Check if the record exists in MainDB
        'PregnancyID	1	uniqueidentifier
        If IsDBNull(Pregnancyrecord("edate")) Or IsDBNull(Pregnancyrecord("eeventtype")) Or IsDBNull(Pregnancyrecord("eobserveid")) Then
            If Me.da.validationtype = mhrsSyncValidationTypes.userpplication Then
                Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "the end episode has null attributes", "", Now(), "", village, round)
            End If
            hasError = True
        Else
            'eeventtype	8	char
            If Not isEndEvent(Pregnancyrecord("eeventtype").ToString) Then
                Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "invalid eeventtype", "", Now(), "", village, round)
                hasError = True
            End If
            'edate	9	datetime

            If CDate(Pregnancyrecord("edate")) > Now() Then
                Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "invalid date edate", "", Now(), "", village, round)
                hasError = True
            End If
            If CDate(Pregnancyrecord("sdate")) > CDate(Pregnancyrecord("edate")) Then
                Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "sdate is greater than edate", "", Now(), "", village, round)
                hasError = True
            End If
            'eobserveid	10	varchar
            If Not clsUserDefinedFunctions.isValidObservationid(Pregnancyrecord("eobserveid").ToString) Then
                Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "invalid eobserveid", "", Now(), "", village, round)
                hasError = True
            End If
            'efieldworker	11	varchar

            'Other
            'If CDate(Pregnancyrecord("sdate")) = CDate(Pregnancyrecord("edate")) Then
            '    Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "sdate is same as  edate", "", Now(), "", village, round)
            '    hasError = True
            'End If

            'If Pregnancyrecord("sobserveid").ToString.Trim = Pregnancyrecord("eobserveid").ToString.Trim Then
            '    Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "sobserveid is same as  eobserveid", "", Now(), "", village)
            '    hasError = True
            'End If

            'perform startevent reference validations
            If Me.validateEndEvent(Pregnancyrecord, tablename, village, round) Then
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
    Private Function validateStartEvent(ByVal Pregnancyrecord As DataRow, ByVal tablename As String, ByVal village As String, ByVal round As String) As Boolean
        Dim hasError As Boolean = False  '{"PRX", "ENT", "ENU", "PRN"}
        If isStartEvent(Pregnancyrecord("seventype").ToString) Then
            Select Case Pregnancyrecord("seventype").ToString.ToUpper.Trim
                'should be the first episode for individual
                Case "ENU"
                    'check if individual has another episode
                    If Me.hasEpisodes(Pregnancyrecord("individid").ToString) Then
                        Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "Individual has another episode", "", Now(), "", village, round)
                        hasError = True
                    End If
                Case "ENT"
                    'check if its a return migrant
                    If Me.hasEpisodes(Pregnancyrecord("individid").ToString) Then
                        'last event for return migrants should be 'EXT'
                        If Me.getLastEvent(Pregnancyrecord("individid").ToString) <> "EXT" Then
                            Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "the last event of return migrant is not EXT", "", Now(), "", village, round)
                            hasError = True
                        Else
                            'if last event was an 'EXT' then
                            'check if there is another event that occured in the same date
                            If Me.eventDateExists(Pregnancyrecord) Then
                                Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "the event date already exists", "", Now(), "", village, round)
                                hasError = True
                            End If
                            'check if the individual had died
                            If Me.hadDied(Pregnancyrecord("individid").ToString) Then
                                Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "Individual has a DTH event", "", Now(), "", village, round)
                                hasError = True
                            End If
                            'individual should have only one open episode
                            If Me.hasOpenEpisode(Pregnancyrecord) Then
                                Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "Individual has another open episode", "", Now(), "", village, round)
                                hasError = True
                            End If
                        End If
                    Else

                    End If
                Case "PRN"
                    'check if its has episodes
                    If Me.hasEpisodes(Pregnancyrecord("individid").ToString) Then
                        'last event for return migrants should be 'TRO'
                        'If Not Me.TRIhasValideTRO(Pregnancyrecord) Then
                        '    Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "the last event for the TRI is not a TRO", "", Now(), "", village, round)
                        '    hasError = True
                        'Else
                        'if last event was an 'EXT' then
                        'check if there is another event that occured in the same date
                        If Me.eventDateExists(Pregnancyrecord) Then
                            Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "the event date already exists", "", Now(), "", village, round)
                            hasError = True
                        End If
                        'check if the individual had died
                        If Me.hadDied(Pregnancyrecord("individid").ToString) Then
                            Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "Individual has a DTH event", "", Now(), "", village, round)
                            hasError = True
                        End If
                        'individual should have only one open episode
                        If Me.hasOpenEpisode(Pregnancyrecord) Then
                            Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "Individual has another open episode", "", Now(), "", village, round)
                            hasError = True
                        End If
                    End If
                   
                Case "PRX"

                Case Else
                    'Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "Unknown start event", "", Now(), "")
                    'hasError = True
            End Select
        Else
            hasError = True
        End If
        Return hasError
    End Function
    Private Function validateEndEvent(ByVal Pregnancyrecord As DataRow, ByVal tablename As String, ByVal village As String, ByVal round As String) As Boolean
        Dim hasError As Boolean = False
        If isEndEvent(Pregnancyrecord("eeventtype").ToString) Then
            If Me.episodeIsINPregnancy(Pregnancyrecord) Then
                'check if individual has open episode at place of death
                If Me.hasOpenEpisodeInLocation(Pregnancyrecord) Then
                    'check if there is another event that occured in the same date
                    If Me.eventDateExists(Pregnancyrecord) Then
                        Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "the event date already exists", "", Now(), "", village, round)
                        hasError = True
                    End If
                    'check for four calender month rule
                    If Not Me.meetsFourCalenderMonthsRule(Pregnancyrecord) Then
                        '' Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "Individual has no open episode in location", "", Now(), "")
                        '' hasError = True
                    End If
                Else
                    Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "Individual has no open episode in location", "", Now(), "", village, round)
                    hasError = True
                    Return hasError
                End If

                ' perform event specific validations
                Select Case Pregnancyrecord("eeventtype").ToString.ToUpper.Trim
                    '{"EXT", "TRO", "DTH", "", "", "", "", "", ""}
                    Case "DTH"
                    Case "TRO"
                    Case "EXT"
                    Case "MULLBR"
                    Case "LBRSTB"
                    Case "SINLBR"
                    Case "MULSTB"
                    Case "SINSTB"
                    Case "MISCAR"
                    Case Else
                        'Me.da.saveError(Residencyrecord("transit_id").ToString.Trim, tablename, "Unknown end event", "", Now(), "")
                        'hasError = True
                End Select
            Else
                Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "episode not in Pregnancy", "", Now(), "", village, round)
                hasError = True
            End If
        Else
            hasError = True
        End If
        Return hasError
    End Function
#End Region
#Region "reference validations functions"
    Private Function eventDateExists(ByVal Pregnancyrecord As DataRow) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = ""
        Select Case Pregnancyrecord("rec_status").ToString.ToLower.Trim
            Case "i", "di", "ti"
                sql = "SELECT count(*) FROM [DSSHRS].[DSS].[Pregnancy] " _
                    & " where (sdate is not null) and (year([sdate])=" + CDate(Pregnancyrecord("sdate")).Year.ToString + ") and (month(sdate)=" + CDate(Pregnancyrecord("sdate")).Month.ToString + ") " _
                    & " and (day(sdate)=" + CDate(Pregnancyrecord("sdate")).Day.ToString + ") and (individid='" + Pregnancyrecord("individid").ToString.Trim + "')"
                If Me.da.executeScalar_INMainDB(sql) > 0 Then
                    returnValue = True
                Else
                    returnValue = False
                End If
            Case "u", "du", "tu"
                sql = "SELECT count(*)  FROM [DSSHRS].[DSS].[Pregnancy] " _
                    & " where (edate is not null) and (year([edate])=" + CDate(Pregnancyrecord("edate")).Year.ToString + ") and " _
                    & "(month(edate)=" + CDate(Pregnancyrecord("edate")).Month.ToString + ") " _
                    & " and (day(edate)=" + CDate(Pregnancyrecord("edate")).Day.ToString + ") and (individid='" + Pregnancyrecord("individid").ToString.Trim + "')"
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
        Dim sql As String = "SELECT count(*)  FROM [DSSHRS].[DSS].[Pregnancy]" _
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
        Dim sql As String = "SELECT count(*)  FROM [DSSHRS].[DSS].[Pregnancy]" _
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
        Dim sql As String = "SELECT count(*)  FROM [DSSHRS].[DSS].[Pregnancy]" _
                            & "where  (PregnancyID='" + membershiprecord("PregnancyID").ToString + "')"
        If Me.da.executeScalar_INMainDB(sql) > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function
    Private Function hasOpenEpisodeInLocation(ByVal Pregnancyrecord As DataRow) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT count(*) FROM [DSSHRS].[DSS].[Pregnancy]" _
                            & " where  (individid='" + Pregnancyrecord("individid").ToString + "') and (eeventtype is null or eeventtype='' )" _
                            & " and  (eobserveid is null) and (edate is null) and ([locationid]='" + Pregnancyrecord("locationid").ToString + "')"
        If Me.da.executeScalar_INMainDB(sql) > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function
    Private Function hasOpenEpisode(ByVal Pregnancyrecord As DataRow) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT count(*) FROM [DSSHRS].[DSS].[Pregnancy]" _
                            & " where  (individid='" + Pregnancyrecord("individid").ToString + "') and (eeventtype is null or eeventtype='' )" _
                            & " and  (eobserveid is null) and (edate is null) "
        If Me.da.executeScalar_INMainDB(sql) > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function
    Private Function episodeIsINPregnancy(ByVal Pregnancyrecord As DataRow) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT count(*) FROM [DSSHRS].[DSS].[Pregnancy]" _
                            & " where  (PregnancyID='" + Pregnancyrecord("PregnancyID").ToString + "') "
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
        Dim sql As String = "SELECT [lastevent]  FROM [DSSHRS].[dbo].[lastPregnancy]" _
                            & "where  (individid='" + individid + "')"
        returnValue = Me.da.getScalar_inMainDB(sql).ToString.Trim
        Return returnValue
    End Function
    'Private Function TRIhasValideTRO(ByVal Pregnancyrecord As DataRow) As Boolean
    '    Dim locationid As Object = Nothing
    '    Dim sql As String = "SELECT locationid  FROM [DSSHRS].[dbo].[lastPregnancy]" _
    '                        & " where  (individid='" + Pregnancyrecord("individid").ToString + "') and (lastevent='TRO') and " _
    '                        & " and (year([date])=" + CDate(Pregnancyrecord("sdate")).Year.ToString + ") and " _
    '                        & " (month([date])=" + CDate(Pregnancyrecord("sdate")).Month.ToString + ") " _
    '                        & " and (day([date])=" + CDate(Pregnancyrecord("sdate")).Day.ToString + ")"
    '    locationid = Me.da.getScalar_inMainDB(sql)
    '    If Not locationid Is Nothing Then
    '        If clsUserDefinedFunctions.getcompound_from_locationid(locationid.ToString.Trim).Trim = clsUserDefinedFunctions.getcompound_from_locationid(Pregnancyrecord("locationid").ToString.Trim).Trim Then
    '            Return False
    '        Else
    '            Return True
    '        End If
    '    Else
    '        Return False
    '    End If
    'End Function
    Private Function meetsFourCalenderMonthsRule(ByVal Pregnancyrecord As DataRow) As Boolean
        Dim currentEventDate As Date
        Select Case Pregnancyrecord("rec_status").ToString.ToLower.Trim
            Case "i", "di", "ti"
                currentEventDate = CDate(Pregnancyrecord("sdate"))
            Case "u", "du", "tu"
                currentEventDate = CDate(Pregnancyrecord("edate"))
        End Select
        Dim sql As String = "SELECT max([sdate])  FROM [DSSHRS].[DSS].[Pregnancy] " _
                           & " where  (individid='" + Pregnancyrecord("individid").ToString + "')"
        Dim lastdate As Date = CDate(Me.da.getScalar_inMainDB(sql))
        If clsUserDefinedFunctions.meetsFourMonthsRule(lastdate, currentEventDate) Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Function hasDuplicatesINTemp(ByVal Pregnancyrecord As DataRow) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT count(*)  FROM [TEMP_DSSHRS].[DSS].[Pregnancy]" _
                            & " where  (rec_status in('DI','I','TI') )and (PregnancyID='" + Pregnancyrecord("PregnancyID").ToString + "')"
        If Me.da.executeScalar_INMainDB(sql) > 1 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function
#End Region
End Class
