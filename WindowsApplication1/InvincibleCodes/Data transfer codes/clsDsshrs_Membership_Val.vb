
Imports System.Data
Imports System.Data.SqlClient
Public Class clsDsshrs_Membership_Val
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
    Friend Function validaterec(ByVal Membershiprecord As DataRow, ByVal tablename As String) As Boolean
        Dim Village As String = da.getrecordsCompound(tablename, Membershiprecord).Trim
        Dim round As String = da.getrecordsRound(tablename, Membershiprecord).Trim
        Dim isValidRecord As Boolean = True

        'memberShipID	1	uniqueidentifier
        'individid	2	varchar
        If Not clsUserDefinedFunctions.isValidIndividid(Membershiprecord("individid").ToString) Then
            Me.da.saveError(Membershiprecord("memberShipID").ToString.Trim, tablename, "invalid individid", "", Now(), "", Village, round)
            isValidRecord = False
        End If
        'If Not isValidRecord Then
        Select Case (IsDBNull(Membershiprecord("eeventtype")) Or IsDBNull(Membershiprecord("edate")) Or IsDBNull(Membershiprecord("eobserveid")))
            Case True
                If Me.validateStartofEpisode(Membershiprecord, tablename, Village, round) Then
                    isValidRecord = False
                End If
            Case False
                If Me.validateStartofEpisode(Membershiprecord, tablename, Village, round) Then
                    isValidRecord = False
                End If
                If Me.validateEndofEpisode(Membershiprecord, tablename, Village, round) Then
                    isValidRecord = False
                End If
        End Select
        'End If
        Return isValidRecord
    End Function
#End Region
#Region "Validation functions"
    Private Function validateStartofEpisode(ByVal Membershiprecord As DataRow, ByVal tablename As String, ByVal village As String, ByVal round As String) As Boolean
        Dim hasError As Boolean = False
       
        'check if episode already exists
        
        'socialgpid	3	varchar
        If Not clsUserDefinedFunctions.isValidLocationid(Membershiprecord("socialgpid").ToString) Then
            Me.da.saveError(Membershiprecord("memberShipID").ToString.Trim, tablename, "invalid socialgpid", "", Now(), "", village, round)
            hasError = True
        End If
        'seventtype	4	char
        If Not isStartEvent(Membershiprecord("seventtype").ToString) Then
            Me.da.saveError(Membershiprecord("memberShipID").ToString.Trim, tablename, "invalid seventtype", "", Now(), "", village, round)
            hasError = True
        End If
        'sdate	5	datetime
        If CDate(Membershiprecord("sdate")) > Now() Then
            Me.da.saveError(Membershiprecord("memberShipID").ToString.Trim, tablename, "invalid date sdate", "", Now(), "", village, round)
            hasError = True
        End If
        'sobserveid	6	varchar
        If Not clsUserDefinedFunctions.isValidObservationid(Membershiprecord("sobserveid").ToString) Then
            Me.da.saveError(Membershiprecord("memberShipID").ToString.Trim, tablename, "invalid sobserveid", "", Now(), "", village, round)
            hasError = True
        End If
        'sfieldworker	7	varchar
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
      

        Return hasError
    End Function
    Private Function validateEndofEpisode(ByVal Membershiprecord As DataRow, ByVal tablename As String, ByVal village As String, ByVal round As String) As Boolean

        Dim hasError As Boolean = False
        'Check if the record exists in MainDB
        'membershipID	1	uniqueidentifier
        'eeventtype	8	char
        If Not isEndEvent(Membershiprecord("eeventtype").ToString) Then
            Me.da.saveError(Membershiprecord("memberShipID").ToString.Trim, tablename, "invalid eeventtype", "", Now(), "", village, round)
            hasError = True
        End If
        'edate	9	datetime

        If CDate(Membershiprecord("edate")) > Now() Then
            Me.da.saveError(Membershiprecord("memberShipID").ToString.Trim, tablename, "invalid date edate", "", Now(), "", village, round)
            hasError = True
        End If
        If CDate(Membershiprecord("sdate")) > CDate(Membershiprecord("edate")) Then
            Me.da.saveError(Membershiprecord("memberShipID").ToString.Trim, tablename, "sdate is greater than edate", "", Now(), "", village, round)
            hasError = True
        End If
        'eobserveid	10	varchar
        If Not clsUserDefinedFunctions.isValidObservationid(Membershiprecord("eobserveid").ToString) Then
            Me.da.saveError(Membershiprecord("memberShipID").ToString.Trim, tablename, "invalid eobserveid", "", Now(), "", village, round)
            hasError = True
        End If
        'efieldworker	11	varchar
        'Other
        If Not ((Membershiprecord("seventtype").ToString.ToUpper.Trim = "BIR")) Then
            If CDate(Membershiprecord("sdate")) = CDate(Membershiprecord("edate")) Then
                If Membershiprecord("efieldworker").ToString.ToUpper.Trim = "PROG" Then
                Else
                    Me.da.saveError(Membershiprecord("memberShipID").ToString.Trim, tablename, "sdate is same as  edate", "", Now(), "", village, round)
                    hasError = True
                End If
            End If
        End If

        'perform startevent reference validations
        If Me.validateEndEvent(Membershiprecord, tablename, village, round) Then
            hasError = True
        End If
        If Me.validateStartEvent(Membershiprecord, tablename, village, round) Then
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
    Private Function validateStartEvent(ByVal Membershiprecord As DataRow, ByVal tablename As String, ByVal village As String, ByVal round As String) As Boolean
        Dim hasError As Boolean = False
        If isStartEvent(Membershiprecord("seventtype").ToString) Then
            Select Case Membershiprecord("seventtype").ToString.ToUpper.Trim
                'should be the first episode for individual
                Case "ENU", "BIR"
                    'check if individual has another episode
                    'If Me.hasEpisodes(Membershiprecord("individid").ToString) Then
                    '    Me.da.saveError(Membershiprecord("memberShipID").ToString.Trim, tablename, "Individual has another episode", "", Now(), "", village, round)
                    '    hasError = True
                    'End If
                Case "ENT"
                    'check if its a return migrant
                    'If Me.hasEpisodes(Membershiprecord("individid").ToString) Then
                    'last event for return migrants should be 'EXT'
                    'If Me.getLastEvent(Membershiprecord) <> "EXT" Then
                    '    Me.da.saveError(Membershiprecord("memberShipID").ToString.Trim, tablename, "the last event of return migrant is not EXT", "", Now(), "", village, round)
                    '    hasError = True
                    'Else
                    'if last event was an 'EXT' then
                    'check if there is another event that occured in the same date
                    If Me.eventDateExists(Membershiprecord) Then
                        Me.da.saveError(Membershiprecord("memberShipID").ToString.Trim, tablename, "the event date already exists", "", Now(), "", village, round)
                        hasError = True
                    End If
                    'check if the individual had died
                    If Me.hadDied(Membershiprecord) Then
                        Me.da.saveError(Membershiprecord("memberShipID").ToString.Trim, tablename, "Individual has a DTH event", "", Now(), "", village, round)
                        hasError = True
                    End If

                    If Me.endEventDateExists(Membershiprecord) Then
                        Me.da.saveError(Membershiprecord("memberShipID").ToString.Trim, tablename, "the previous end event date is similar to current sdate ", "", Now(), "", village, round)
                        hasError = True
                    End If

                    ' End If
                    'Else

                    'End If
                Case "TRI"
                    'check if its has episodes
                    'If Me.hasEpisodes(Membershiprecord("individid").ToString) Then
                    'last event for return migrants should be 'TRO'
                    If Not Me.TRIhasValideTRO(Membershiprecord) Then
                        Me.da.saveError(Membershiprecord("memberShipID").ToString.Trim, tablename, "the last event for the TRI is not a TRO", "", Now(), "", village, round)
                        hasError = True
                    Else
                        'if last event was an 'EXT' then
                        'check if there is another event that occured in the same date
                        If Me.eventDateExists(Membershiprecord) Then
                            Me.da.saveError(Membershiprecord("memberShipID").ToString.Trim, tablename, "the event date already exists", "", Now(), "", village, round)
                            hasError = True
                        End If
                        'check if the individual had died
                        If Me.hadDied(Membershiprecord) Then
                            Me.da.saveError(Membershiprecord("memberShipID").ToString.Trim, tablename, "Individual has a DTH event", "", Now(), "", village, round)
                            hasError = True
                        End If

                    End If


                Case "TRX"
                    'check if its has episodes
                    If Me.hasEpisodes(Membershiprecord) Then
                        'last event for return migrants should be 'TRO'
                        If Not Me.TRXhasValideTRX(Membershiprecord) Then
                            Me.da.saveError(Membershiprecord("memberShipID").ToString.Trim, tablename, "the last event for the TRX is not a TRX", "", Now(), "", village, round)
                            hasError = True
                        Else
                            'if last event was an 'EXT' then
                            'check if there is another event that occured in the same date
                            If Me.eventDateExists(Membershiprecord) Then
                                Me.da.saveError(Membershiprecord("memberShipID").ToString.Trim, tablename, "the event date already exists", "", Now(), "", village, round)
                                hasError = True
                            End If
                            'check if the individual had died
                            If Me.hadDied(Membershiprecord) Then
                                Me.da.saveError(Membershiprecord("memberShipID").ToString.Trim, tablename, "Individual has a DTH event", "", Now(), "", village, round)
                                hasError = True
                            End If
                            ''individual should have only one open episode
                            'If Me.hasOpenEpisode(Membershiprecord) Then
                            '    Me.da.saveError(Membershiprecord("memberShipID").ToString.Trim, tablename, "Individual has another open episode", "", Now(), "", village, round)
                            '    hasError = True
                            'End If
                        End If
                    Else
                        'if individual has no episodes
                        Me.da.saveError(Membershiprecord("memberShipID").ToString.Trim, tablename, "Has no previous episodes", "", Now(), "", village, round)
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

            'check if individual has open episode at place of death

            'check if there is another event that occured in the same date
            If Me.eventDateExists(Membershiprecord) Then
                Me.da.saveError(Membershiprecord("memberShipID").ToString.Trim, tablename, "the event date already exists", "", Now(), "", village, round)
                hasError = True
            End If
            'check for four calender month rule
            If Not Me.meetsFourCalenderMonthsRule(Membershiprecord) Then
                'Me.da.saveError(Membershiprecord("transit_id").ToString.Trim, tablename, "Individual has no open episode in location", "", Now(), "", village, round)
                'hasError = True
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
            hasError = True
        End If
        Return hasError
    End Function
#End Region
#Region "reference validations functions"
    Private Function eventDateExists(ByVal Membershiprecord As DataRow) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = ""
        Select Case (IsDBNull(Membershiprecord("eeventtype")) Or IsDBNull(Membershiprecord("edate")) Or IsDBNull(Membershiprecord("eobserveid")))
            Case True
                'sql = "SELECT count(*) FROM [DSSHRS].[DSS].[membership] " _
                '    & " where (sdate is not null) and (year([sdate])=" + CDate(Membershiprecord("sdate")).Year.ToString + ") and (month(sdate)=" + CDate(Membershiprecord("sdate")).Month.ToString + ") " _
                '    & " and (day(sdate)=" + CDate(Membershiprecord("sdate")).Day.ToString + ") and (individid='" + Membershiprecord("individid").ToString.Trim + "') and (memberShipID<>'" + Membershiprecord("memberShipID").ToString + "')"
                'Emmanuel Added this change
                'Due to the socialgroup enumaration, the end event with efieldworker as PROG have the same sdate this flags an error
                'I added a check to ignore all records whose end event is not equal to PROG
                sql = "SELECT     COUNT(individid) AS Expr1 " _
                    & "FROM         DSS.membership " _
                    & "WHERE     (sdate IS NOT NULL) AND cast(floor(cast(sdate as float)) as datetime)='" + CDate(Membershiprecord("sdate")).ToString("dd-MMM-yyyy") + "' " _
                    & "AND (individid = '" + Membershiprecord("individid").ToString.Trim + "') AND  " _
                    & "(memberShipID <> '" + Membershiprecord("memberShipID").ToString + "')  AND sfieldworker <> 'PROG' and  efieldworker <> 'PROG' "
                sql = "SELECT * FROM [DSSHRS].[DSS].[getMembershipRecordWithoutProg] (  '" & _
                "" + Membershiprecord("memberShipID").ToString + "'" & _
                "  ,'" + Membershiprecord("individid").ToString.Trim + "'  " & _
                "  ,'" + CDate(Membershiprecord("sdate")).ToString("dd-MMM-yyyy") + "'  " & _
                "  ,1)  "


                If Me.da.executeScalar_INMainDB(sql) > 0 Then
                    returnValue = True
                Else
                    returnValue = False
                End If
            Case False
                'sql = "SELECT count(*)  FROM [DSSHRS].[DSS].[membership] " _
                '& " where (edate is not null) and (year([edate])=" + CDate(Membershiprecord("edate")).Year.ToString + ") and " _
                '& "(month(edate)=" + CDate(Membershiprecord("edate")).Month.ToString + ") " _
                '& " and (day(edate)=" + CDate(Membershiprecord("edate")).Day.ToString + ") and (individid='" + Membershiprecord("individid").ToString.Trim + "') and (memberShipID<>'" + Membershiprecord("memberShipID").ToString + "')"
                sql = "SELECT     COUNT(*) AS Expr1 " _
                    & "FROM         DSS.membership " _
                    & "WHERE     (edate IS NOT NULL) AND cast(floor(cast(edate as float)) as datetime)='" + CDate(Membershiprecord("sdate")).ToString("dd-MMM-yyyy") + "' " _
                    & "AND (individid = '" + Membershiprecord("individid").ToString.Trim + "') " _
                    & "AND           (memberShipID <> '" + Membershiprecord("memberShipID").ToString + "')  AND sfieldworker <> 'PROG' and  efieldworker <> 'PROG' "
                sql = "SELECT * FROM [DSSHRS].[DSS].[getMembershipRecordWithoutProg] (  " & _
               "'" + Membershiprecord("memberShipID").ToString + "'" & _
               "  ,'" + Membershiprecord("individid").ToString.Trim + "'  " & _
               "  ,'" + CDate(Membershiprecord("edate")).ToString("dd-MMM-yyyy") + "'  " & _
               "  ,2)  "

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


    Private Function endEventDateExists(ByVal Membershiprecord As DataRow) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = ""
        

        'Emmanuel Added this change
        'Due to the socialgroup enumaration, the end event with efieldworker as PROG have the same sdate this flags an error
        'I added a check to ignore all records whose end event is not equal to PROG
        sql = "SELECT count(*) FROM [DSSHRS].[DSS].[getMembershipEndRecordWithoutProg] (  '" & _
        "" + Membershiprecord("memberShipID").ToString + "'" & _
        "  ,'" + Membershiprecord("individid").ToString.Trim + "'  " & _
        "  ,'" + CDate(Membershiprecord("sdate")).ToString("dd-MMM-yyyy") + "'  " & _
        "  ,1)  "

        If Me.da.executeScalar_INMainDB(sql) > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If

        Return returnValue
    End Function


    Private Function hadDied(ByVal Membershiprecord As DataRow) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT count(*)  FROM [DSSHRS].[DSS].[membership]" _
                            & "where (eeventtype='DTH') and (individid='" + Membershiprecord("individid").ToString + "')" _
                            & " AND (DATEDIFF(day,[edate],getdate())>=DATEDIFF(day,@eventdate,getdate()))"
        If Me.da.hadprevioslyDied(sql, Membershiprecord("sdate")) > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function
    Private Function hasEpisodes(ByVal Membershiprecord As DataRow) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT count(*)  FROM [DSSHRS].[DSS].[membership]" _
                            & "where  (individid='" + Membershiprecord("individid").ToString + "' ) and (memberShipID<>'" + Membershiprecord("memberShipID").ToString + "')"
        If Me.da.executeScalar_INMainDB(sql) >= 1 Then
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
   
    Private Function getLastEvent(ByVal Membershiprecord As DataRow) As String
        Dim returnValue As String = Nothing
        returnValue = Me.da.getmembershipPreVEvent(Membershiprecord)
        Return returnValue
    End Function
    Private Function TRIhasValideTRO(ByVal Membershiprecord As DataRow) As Boolean
        Dim socialgroupid As Object = Nothing
        Dim sql As String = "SELECT socialgpid  FROM [DSSHRS].[dss].[membership]" _
                            & " where  (individid='" + Membershiprecord("individid").ToString + "') and (eeventtype ='TRO') " _
                            & " and cast(floor(cast(edate as float)) as datetime)='" + CDate(Membershiprecord("sdate")).ToString("dd-MMM-yyyy") + "' " _
                            & " and (membershipId<>'" + Membershiprecord("membershipid").ToString + "')"
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
        Dim sql As String = "SELECT count(*)  FROM [DSSHRS].[dss].[membership]" _
                            & " where  (individid='" + Membershiprecord("individid").ToString + "') and (eeventtype ='TRX') and " _
                            & "  (year([edate])=" + CDate(Membershiprecord("sdate")).Year.ToString + ") and " _
                            & " (month([edate])=" + CDate(Membershiprecord("sdate")).Month.ToString + ") " _
                            & " and (day([edate])=" + CDate(Membershiprecord("sdate")).Day.ToString + ") " _
                            & "  and (ltrim(socialgpid)<>'" + Membershiprecord("socialgpid").ToString.Trim + "') and (membershipID<>'" + Membershiprecord("membershipid").ToString + "') "
        socialgroupid = Me.da.getScalar_inMainDB(sql)
        If socialgroupid > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Function meetsFourCalenderMonthsRule(ByVal Membershiprecord As DataRow) As Boolean
        Dim currentEventDate As Date
        Select Case (IsDBNull(Membershiprecord("eeventtype")) Or IsDBNull(Membershiprecord("edate")) Or IsDBNull(Membershiprecord("eobserveid")))
            Case True
                currentEventDate = CDate(Membershiprecord("sdate"))
            Case False
                currentEventDate = CDate(Membershiprecord("edate"))
        End Select
        Dim sql As String = "SELECT max([sdate])  FROM [DSSHRS].[DSS].[membership] " _
                           & "where  (individid='" + Membershiprecord("individid").ToString + "') and (membershipI<>'" + Membershiprecord("membershipid").ToString + "')"
        Dim lastdate As Date = CDate(Me.da.getScalar_inMainDB(sql))
        If clsUserDefinedFunctions.meetsFourMonthsRule(lastdate, currentEventDate) Then
            Return True
        Else
            Return False
        End If
    End Function
    
#End Region
End Class
