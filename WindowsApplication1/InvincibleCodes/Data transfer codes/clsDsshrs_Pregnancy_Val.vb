Imports System.Data
Imports System.Data.SqlClient
Public Class clsDsshrs_Pregnancy_Val
#Region "variables and constructor"
    Private globalvariables As clsGlobalVariables = clsGlobalVariables.getObject
    Private da As clsDataAccess = clsDataAccess.getObject
    Private startEvents As String() = {"PRX", "ENT", "ENU", "PRN"}
    Private endEvents As String() = {"EXT", "TRO", "DTH", "MULLBR", "LBRSTB", "SINLBR", "MULSTB", "SINSTB", "MISCAR", "BIR" _
     , "BIR", "CEN", "EXT", "PRO", "DTH", "NOTAPP", "NAP", "DTH/EX", "SINSTB", "SINLBR", "LBR", "MISCAR", "TRO", "MULSTB", "MULLBR", "SABORT"}

    Private Sub New()

    End Sub
    Public Sub New(ByVal validationtype As mhrsSyncValidationTypes)
        da.validationtype = validationtype
    End Sub
#End Region
#Region "procedures"

    Friend Function validaterec(ByVal Pregnancyrecord As DataRow, ByVal tablename As String) As Boolean
        Dim Village As String = da.getrecordsCompound(tablename, Pregnancyrecord).Trim
        Dim round As String = da.getrecordsRound(tablename, Pregnancyrecord).Trim
        Dim isValidRecord As Boolean = True


        'individid	2	varchar
        If Not clsUserDefinedFunctions.isValidIndividid(Pregnancyrecord("individid").ToString) Then
            Me.da.saveError(Pregnancyrecord("PregnancyID").ToString.Trim, tablename, "invalid individid", "", Now(), "", Village, round)
            isValidRecord = False
        End If
        'If Not isValidRecord Then
        Select Case IsDBNull(Pregnancyrecord("edate")) Or IsDBNull(Pregnancyrecord("eeventtype")) Or IsDBNull(Pregnancyrecord("eobserveid"))
            Case True
                If Me.validateStartofEpisode(Pregnancyrecord, tablename, Village, round) Then
                    isValidRecord = False
                End If
            Case False
                If Me.validateEndofEpisode(Pregnancyrecord, tablename, Village, round) Then
                    isValidRecord = False
                End If
        End Select
        'End If
        Return isValidRecord
    End Function
#End Region
#Region "Validation functions"
    Private Function validateStartofEpisode(ByVal Pregnancyrecord As DataRow, ByVal tablename As String, ByVal village As String, ByVal round As String) As Boolean
        Dim hasError As Boolean = False
        
        'check if episode already exists
       
        'locationid	3	varchar
        If Not clsUserDefinedFunctions.isValidLocationid(Pregnancyrecord("locationid").ToString) Then
            Me.da.saveError(Pregnancyrecord("PregnancyID").ToString.Trim, tablename, "invalid locationid", "", Now(), "", village, round)
            hasError = True
        End If
        'seventtype	4	char
        If Not isStartEvent(Pregnancyrecord("seventype").ToString) Then
            Me.da.saveError(Pregnancyrecord("PregnancyID").ToString.Trim, tablename, "invalid seventype", "", Now(), "", village, round)
            hasError = True
        End If
        'sdate	5	datetime
        If CDate(Pregnancyrecord("sdate")) > Now() Then
            Me.da.saveError(Pregnancyrecord("PregnancyID").ToString.Trim, tablename, "invalid date sdate", "", Now(), "", village, round)
            hasError = True
        End If
        'sobserveid	6	varchar
        If Not clsUserDefinedFunctions.isValidObservationid(Pregnancyrecord("sobserveid").ToString) Then
            Me.da.saveError(Pregnancyrecord("PregnancyID").ToString.Trim, tablename, "invalid sobserveid", "", Now(), "", village, round)
            hasError = True
        End If
        'sfieldworker	7	varchar

        'eeventtype	8	char
        'If Not IsDBNull(Pregnancyrecord("eeventtype")) Then
        '    Me.da.saveError(Pregnancyrecord("PregnancyID").ToString.Trim, tablename, "End episode should be null", "", Now(), "", village, round)
        '    hasError = True
        'End If
        ''edate	9	datetime
        'If Not IsDBNull(Pregnancyrecord("edate")) Then
        '    Me.da.saveError(Pregnancyrecord("PregnancyID").ToString.Trim, tablename, "End episode should be null", "", Now(), "", village, round)
        '    hasError = True
        'End If

        ''eobserveid	10	varchar
        'If Not IsDBNull(Pregnancyrecord("eobserveid")) Then
        '    Me.da.saveError(Pregnancyrecord("PregnancyID").ToString.Trim, tablename, "End episode should be null", "", Now(), "", village, round)
        '    hasError = True
        'End If
        'efieldworker	11	varchar

        'perform startevent reference validations
        'If Me.validateStartEvent(Pregnancyrecord, tablename, village, round) Then
        '    hasError = True
        'End If

        Return hasError
    End Function
    Private Function validateEndofEpisode(ByVal Pregnancyrecord As DataRow, ByVal tablename As String, ByVal village As String, ByVal round As String) As Boolean

        Dim hasError As Boolean = False
        'Check if the record exists in MainDB
        'PregnancyID	1	uniqueidentifier
       
        'eeventtype	8	char
        If Not isEndEvent(Pregnancyrecord("eeventtype").ToString) Then
            Me.da.saveError(Pregnancyrecord("PregnancyID").ToString.Trim, tablename, "invalid eeventtype", "", Now(), "", village, round)
            hasError = True
        End If
        'edate	9	datetime

        If CDate(Pregnancyrecord("edate")) > Now() Then
            Me.da.saveError(Pregnancyrecord("PregnancyID").ToString.Trim, tablename, "invalid date edate", "", Now(), "", village, round)
            hasError = True
        End If
        If CDate(Pregnancyrecord("sdate")) > CDate(Pregnancyrecord("edate")) Then
            Me.da.saveError(Pregnancyrecord("PregnancyID").ToString.Trim, tablename, "sdate is greater than edate", "", Now(), "", village, round)
            hasError = True
        End If
        'eobserveid	10	varchar
        If Not clsUserDefinedFunctions.isValidObservationid(Pregnancyrecord("eobserveid").ToString) Then
            Me.da.saveError(Pregnancyrecord("PregnancyID").ToString.Trim, tablename, "invalid eobserveid", "", Now(), "", village, round)
            hasError = True
        End If
        'efieldworker	11	varchar

        'Other
        'If CDate(Pregnancyrecord("sdate")) = CDate(Pregnancyrecord("edate")) Then
        '    Me.da.saveError(Pregnancyrecord("PregnancyID").ToString.Trim, tablename, "sdate is same as  edate", "", Now(), "", village, round)
        '    hasError = True
        'End If

        'If Pregnancyrecord("sobserveid").ToString.Trim = Pregnancyrecord("eobserveid").ToString.Trim Then
        '    Me.da.saveError(Pregnancyrecord("transit_id").ToString.Trim, tablename, "sobserveid is same as  eobserveid", "", Now(), "", village)
        '    hasError = True
        'End If

        'perform startevent reference validations
        'If Me.validateEndEvent(Pregnancyrecord, tablename, village, round) Then
        '    hasError = True
        'End If

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
    
#End Region
#Region "reference validations functions"
    
   
#End Region
End Class
