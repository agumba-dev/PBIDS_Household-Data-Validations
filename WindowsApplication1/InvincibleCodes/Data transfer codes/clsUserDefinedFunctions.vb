Imports System
Imports System.Data
Imports Microsoft.SqlServer.Server


Partial Public Class clsUserDefinedFunctions
#Region "ID validations"
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function isValidObservationid(ByVal observeid As String) As Boolean
        ' Add your code here
        Dim result As Boolean = False
        Dim parts() As String = observeid.Split("-")
        If (parts.Length = 4) And Not (observeid.StartsWith("-") Or observeid.EndsWith("-")) Then
            result = True
            If Not isValidRound(parts(3).ToString.Trim) Then
                result = False
            End If
            If Not isValidLocationid(getlocation_from_observationid(observeid)) Then
                result = False
            End If
        Else
            result = False
        End If
        Return result
    End Function
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function isValidIndividid(ByVal individid As String) As Boolean
        ' Add your code here
        Dim result As Boolean = False
        Dim parts() As String = individid.Split("-")
        If (parts.Length = 4) And Not (individid.StartsWith("-") Or individid.EndsWith("-")) Then
            result = True
            For Each part As String In parts
                If IsNumeric(part) Then
                    If CInt(part) < 1 Then
                        result = False
                    End If
                Else
                    result = False
                End If
            Next
        Else
            result = False
        End If
        Return result
    End Function
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function isValidcompoundid(ByVal compoundid As String) As Boolean
        ' Add your code here
        Dim result As Boolean = False
        Dim parts() As String = compoundid.Split("-"c)
        If (parts.Length = 2) And Not (compoundid.StartsWith("-") Or compoundid.EndsWith("-")) Then
            result = True
            For Each part As String In parts
                If IsNumeric(part) Then
                    If CInt(part) < 1 Then
                        result = False
                    End If
                Else
                    result = False
                End If
            Next
        Else
            result = False
        End If
        Return result
    End Function
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function isValidLocationid(ByVal locationid As String) As Boolean
        ' Add your code here
        Dim result As Boolean = False
        Dim parts() As String = locationid.Split("-")
        If (parts.Length = 3) And Not (locationid.StartsWith("-") Or locationid.EndsWith("-")) Then
            result = True
            For Each part As String In parts
                If IsNumeric(part) Then
                    If CInt(part) < 1 Then
                        result = False
                    End If
                Else
                    result = False
                End If
            Next
        Else
            result = False
        End If
        Return result
    End Function

    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function isValidsocialGroupid(ByVal socialgroupid As String) As Boolean
        ' Add your code here
        Dim result As Boolean = False
        Dim parts() As String = socialgroupid.Split("-")
        If (parts.Length = 3) And Not (socialgroupid.StartsWith("-") Or socialgroupid.EndsWith("-")) Then
            result = True
            For Each part As String In parts
                If IsNumeric(part) Then
                    If CInt(part) < 1 Then
                        result = False
                    End If
                Else
                    result = False
                End If
            Next
        Else
            result = False
        End If
        Return result
    End Function
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function isValidRound(ByVal round As String) As Boolean
        ' Add your code here
        Dim result As Boolean = False
        If round.Trim.Length = 5 Then
            If IsNumeric(round) Then
                result = True
            End If
        End If
        Return result
    End Function
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function isValidVillcode(ByVal villcode As String) As Boolean
        ' Add your code here
        Dim result As Boolean = False
        If villcode.Trim.Length = 3 Then
            If IsNumeric(villcode) Then
                result = True
            End If
        End If
        Return result
    End Function
#End Region
#Region "ID conversion"
    'villcode
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function get_villcode(ByVal oldvillcode As String) As String
        If oldvillcode Is Nothing Then
            Return "Q" 'Nothing
        End If
        ' Add your code here
        Dim result As Integer = 0
        If oldvillcode.Trim.Length = 3 Then
            If IsNumeric(oldvillcode) Then
                result = CInt(oldvillcode)
            End If
        End If
        Return result.ToString
    End Function
    'compoundid
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function get_compoundid(ByVal oldcompoundid As String) As String
        ' Add your code herer
        If oldcompoundid Is Nothing Then
            Return "Q" 'Nothing
        ElseIf (oldcompoundid.Trim = "") Then
            Return "Q"
        End If
        Dim result As String = oldcompoundid ' "0"
        If oldcompoundid.Trim.Length = 6 Then
            If IsNumeric(oldcompoundid) Then
                result = CInt(oldcompoundid.Substring(0, 3)).ToString + "-" + CInt(oldcompoundid.Substring(3, 3)).ToString
            End If
        End If
        Return result
    End Function
    'locationid 
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function get_locationid(ByVal oldlocationid As String) As String
        ' Add your code herer
        If (oldlocationid Is Nothing) Then
            Return "Q" 'Nothing
        ElseIf (oldlocationid.Trim = "") Then
            Return "Q"
        End If
        oldlocationid = removeSpaceInIDs(oldlocationid)
        Dim result As String = oldlocationid '"0"
        If oldlocationid.Trim.Length = 7 Then
            If (IsNumeric(oldlocationid.Substring(0, 6))) And (Char.IsLetter(GetChar(oldlocationid, 7))) Then
                result = CInt(oldlocationid.Substring(0, 3)).ToString + "-" + CInt(oldlocationid.Substring(3, 3)).ToString + "-" + (Asc(Char.ToUpper(GetChar(oldlocationid, 7))) - 64).ToString
            End If
        ElseIf oldlocationid.Trim.Length = 8 Then
            result = get_socialgroupid(oldlocationid)
        End If
        Return result

    End Function
    'socialgroupid
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function get_socialgroupid(ByVal oldsocialgroupid As String) As String

        If (oldsocialgroupid Is Nothing) Then
            Return "Q" 'Nothing
        ElseIf (oldsocialgroupid.Trim = "") Then
            Return "Q"
        End If
        oldsocialgroupid = removeSpaceInIDs(oldsocialgroupid)
        ' Add your code herer
        Dim result As String = oldsocialgroupid '"0"
        Try

            If oldsocialgroupid.Trim.Length = 8 Then
                If IsNumeric(oldsocialgroupid) Then
                    result = CInt(oldsocialgroupid.Substring(0, 3)).ToString + "-" + CInt(oldsocialgroupid.Substring(3, 3)).ToString + "-" + CInt(oldsocialgroupid.Substring(6, 2)).ToString
                End If
            ElseIf oldsocialgroupid.Trim.Length = 7 Then
                result = get_locationid(oldsocialgroupid)
            End If
        Catch ex As Exception
            result = oldsocialgroupid '"0"
        End Try
        Return result
    End Function
    'individualid
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function get_individualid(ByVal oldindividualid As String) As String
        If (oldindividualid Is Nothing) Then
            Return "Q" 'Nothing
        ElseIf (oldindividualid.Trim = "") Then
            Return "Q" 'Nothing
        End If
        ' Add your code herer
        Dim result As String = oldindividualid '"0"
        If oldindividualid.Trim.Length = 10 Then
            result = get_locationid(oldindividualid.Substring(0, 7))
            If (IsNumeric(oldindividualid.Substring(7, 3))) And (result <> "0") Then
                result = result + "-" + CInt(oldindividualid.Substring(7, 3)).ToString
            Else
                result = oldindividualid '"0"
            End If
        ElseIf oldindividualid.Trim.ToLower = "unk" Then
            result = "UNK"
        ElseIf oldindividualid.Trim = "" Then
            result = "Q" ' Nothing
        End If
        Return result
    End Function
    'observationid
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function get_observationid(ByVal oldobservationid As String) As String
        If (oldobservationid = Nothing) Then
            Return Nothing
        ElseIf (oldobservationid.Trim = "") Then
            Return Nothing
        End If
        ' Add your code herer
        Dim result As String = oldobservationid '"0"
        If oldobservationid Is Nothing Then
            result = Nothing
        Else
            If oldobservationid.Trim.Length = 12 Then
                result = get_locationid(oldobservationid.Substring(0, 7))
                If (IsNumeric(oldobservationid.Substring(7, 5))) And (result <> "0") Then
                    result = result + "-" + CInt(oldobservationid.Substring(7, 5)).ToString
                Else
                    result = oldobservationid '"0"
                End If
            ElseIf oldobservationid.Trim.Length = 13 Then
                result = get_socialgroupid(oldobservationid.Substring(0, 8))
                If (IsNumeric(oldobservationid.Substring(8, 5))) And (result <> "0") Then
                    result = result + "-" + CInt(oldobservationid.Substring(8, 5)).ToString
                Else
                    result = oldobservationid '"0"
                End If
            End If
        End If
        Return result
    End Function
    'getcompound from locationid ,getcompound_from_locationid
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function getcompound_from_locationid(ByVal locationid As String) As String
        If locationid Is Nothing Then
            Return Nothing
        End If
        ' Add your code here
        Dim result As String = Nothing
        Dim parts() As String = locationid.Split("-"c)
        If (parts.Length = 3) And Not (locationid.StartsWith("-") Or locationid.EndsWith("-")) Then
            result = parts(0).ToString + "-" + parts(1).ToString
        Else
            result = Nothing
        End If
        Return result
    End Function
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function getOldIndividid(ByVal individid As String) As String
        ' Add your code here
        If isValidIndividid(individid) Then
            Dim parts() As String = individid.Split("-"c)
            If (CInt(parts(0)) < 1000) And (CInt(parts(1)) < 1000) And (CInt(parts(2)) < 27) And (CInt(parts(3)) < 1000) Then
                Return parts(0).ToString.PadLeft(3, "0"c) + parts(1).ToString.PadLeft(3, "0"c) + ChrW(CInt(parts(2)) + 64).ToString + parts(3).ToString.PadLeft(3, "0"c)
            Else
                Return individid
            End If
        Else
            Return individid
        End If
    End Function
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function getOldLocationid(ByVal locationid As String) As String
        ' Add your code here
        If isValidLocationid(locationid) Then
            Dim parts() As String = locationid.Split("-"c)
            If (CInt(parts(0)) < 1000) And (CInt(parts(1)) < 1000) And (CInt(parts(2)) < 27) Then
                Return parts(0).ToString.PadLeft(3, "0"c) + parts(1).ToString.PadLeft(3, "0"c) + ChrW(CInt(parts(2)) + 64).ToString
            Else
                Return locationid
            End If
        Else
            Return locationid
        End If
    End Function
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function getOldSocialGroupid(ByVal socialgrpid As String) As String
        ' Add your code here
        If isValidsocialGroupid(socialgrpid) Then
            Dim parts() As String = socialgrpid.Split("-"c)
            If (CInt(parts(0)) < 1000) And (CInt(parts(1)) < 1000) And (CInt(parts(2)) < 100) Then
                Return parts(0).ToString.PadLeft(3, "0"c) + parts(1).ToString.PadLeft(3, "0"c) + parts(2).ToString.PadLeft(2, "0"c)
            Else
                Return socialgrpid
            End If
        Else
            Return socialgrpid
        End If
    End Function
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function getOldCompoundid(ByVal compoundid As String) As String
        ' Add your code here
        If isValidcompoundid(compoundid) Then
            Dim parts() As String = compoundid.Split("-"c)
            If (CInt(parts(0)) < 1000) And (CInt(parts(1)) < 1000) Then
                Return parts(0).ToString.PadLeft(3, "0"c) + parts(1).ToString.PadLeft(3, "0"c)
            Else
                Return compoundid
            End If
        Else
            Return compoundid
        End If
    End Function
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function convert242Observedid(ByVal observeid As String) As String
        If observeid Is Nothing Then
            Return Nothing
        End If
        ' Add your code here
        Dim result As String = Nothing
        Dim parts() As String = observeid.Split("-")
        If (parts.Length = 4) And Not (observeid.StartsWith("-") Or observeid.EndsWith("-")) Then
            If parts(0).ToString.ToLower = "242" Then
                result = "116-" + (999 + CInt(parts(1))).ToString + "-" + parts(2).ToString + "-" + parts(3).ToString
            Else
                result = observeid
            End If
        Else
            result = Nothing
        End If
        Return result
    End Function
    'getcompound from locationid ,getcompound_from_locationid
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function getcompoundNUM_from_compound(ByVal compoundid As String) As String
        If compoundid Is Nothing Then
            Return Nothing
        End If
        ' Add your code here
        Dim result As String = Nothing
        Dim parts() As String = compoundid.Split("-")
        If (parts.Length > 1) And Not (compoundid.StartsWith("-") Or compoundid.EndsWith("-")) Then
            result = parts(1).ToString
        Else
            result = Nothing
        End If
        Return result
    End Function
    'getcompound from locationid ,getcompound_from_locationid
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function getlocation_from_observationid(ByVal observationid As String) As String
        If observationid Is Nothing Then
            Return Nothing
        End If
        ' Add your code here
        Dim result As String = Nothing
        Dim parts() As String = observationid.Split("-")
        If (parts.Length = 4) And Not (observationid.StartsWith("-") Or observationid.EndsWith("-")) Then
            result = parts(0).ToString + "-" + parts(1).ToString + "-" + parts(2).ToString
        Else
            result = Nothing
        End If
        Return result
    End Function
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function getRound_from_observationid(ByVal observationid As String) As String
        If observationid Is Nothing Then
            Return Nothing
        End If
        ' Add your code here
        Dim result As String = Nothing
        Dim parts() As String = observationid.Split("-")
        If (parts.Length = 4) And Not (observationid.StartsWith("-") Or observationid.EndsWith("-")) Then
            result = parts(3).ToString
        Else
            result = Nothing
        End If
        Return result
    End Function
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function getLocationSeq_from_locationid(ByVal locationid As String) As String
        If locationid Is Nothing Then
            Return Nothing
        End If
        ' Add your code here
        Dim result As String = Nothing
        Dim parts() As String = locationid.Split("-")
        If (parts.Length > 2) And Not (locationid.StartsWith("-") Or locationid.EndsWith("-")) Then
            result = parts(2).ToString
        Else
            result = Nothing
        End If
        Return result
    End Function

    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function getcompound_from_observationid(ByVal observationid As String) As String
        If observationid Is Nothing Then
            Return Nothing
        End If
        ' Add your code here
        Dim result As String = Nothing
        Dim parts() As String = observationid.Split("-")
        If (parts.Length > 2) And Not (observationid.StartsWith("-") Or observationid.EndsWith("-")) Then
            result = parts(0).ToString + "-" + parts(1).ToString
        Else
            result = Nothing
        End If

        Return result
    End Function
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function getVillage_from_observationid(ByVal observationid As String) As String
        If observationid Is Nothing Then
            Return Nothing
        End If
        ' Add your code here
        Dim result As String = Nothing
        Dim parts() As String = observationid.Split("-")
        If (parts.Length > 1) And Not (observationid.StartsWith("-") Or observationid.EndsWith("-")) Then
            result = parts(0).ToString
        Else
            result = Nothing
        End If
        Return result
    End Function
    '<Microsoft.SqlServer.Server.SqlFunction()>
    Public Shared Function removeSpaceInIDs(ByVal id As String) As String
        ' Add your code here
        Dim result As String = Nothing
        If (id Is Nothing) Then
            Return "Q" 'nothing
        ElseIf (id.Trim = "") Then
            Return "Q"
        End If
        If id.Contains(" ") Then
            result = id.Replace(" ", "")
        Else
            result = id
        End If
        Return result
    End Function
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function toDouble(ByVal txtvalue As String) As String
        ' Add your code here
        Dim result As String = Nothing
        If txtvalue.Contains(" ") Then
            result = txtvalue.Replace(" ", "")
        Else
            result = txtvalue
        End If
        Return result
    End Function
#End Region
#Region "OTHER"
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function getlatestResidency(ByVal sqlquery As String) As Boolean
        ' Add your code here

        Return True
    End Function
    <Microsoft.SqlServer.Server.SqlProcedure()>
    Public Shared Sub tableRecordCount(ByVal tabl As String)
        ' Add your code here
        Using connection As New SqlConnection("context connection=true")
            'SqlContext.Pipe.Send("Hello world! It's now " & System.DateTime.Now.ToString() & "\n")
            connection.Open()
            Dim com As New SqlCommand("select count(*) from " + tabl, connection)
            Dim reader As SqlDataReader
            If connection.State <> ConnectionState.Open Then
                connection.Open()
            End If
            reader = com.ExecuteReader()
            SqlContext.Pipe.Send(reader)

            'SqlContext.Pipe.ExecuteAndSend(com)
        End Using
    End Sub
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function meetsFourMonthsRule(ByVal lastSdate As Date, ByVal currentEventdate As Date) As Boolean
        If lastSdate = Nothing Or currentEventdate = Nothing Then
            Return True
        Else
            If currentEventdate < lastSdate Then
                Return True
            Else
                If currentEventdate.AddMonths(-3) < lastSdate Then
                    Return False
                Else
                    Return True
                End If
            End If

        End If
    End Function
    Private Function ageDiff(ByVal indrec As DataRow, ByVal parDOB As Date) As Integer

        Return DateDiff(DateInterval.Year, parDOB, indrec("dob"))
    End Function
#End Region
#Region "changes"
    '  '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function get_IndivChanges(ByVal indichangeCode As String) As String

        Dim result As String = Nothing
        If (indichangeCode Is Nothing) Then
            Return "Q" 'nothing
        ElseIf (indichangeCode.Trim = "") Then
            Return "Q"
        End If

        Select Case indichangeCode
            Case "01" '"Movement within Compound", "LocationID Change"
                result = "locationid"
            Case "02" ' "Sex"
                result = "gender"
            Case "03" '"Date of Birth"
                result = "dob"

            Case "04" '"First Name"
                result = "fname"

            Case "05" '"Juok Name"
                result = "jname"

            Case "06" '"Last Name"
                result = "lname"

            Case "07" '"Clan Name"
                result = "famcla"

            Case "08" '"Mother's First Name"
                result = "mfname"

            Case "09" '"Mother's Juok Name"
                result = "mjname"

            Case "10" '"Mother's ID"
                result = "motherid"

                'TODO find table to update when this values change i.e. 35
                'Case "35" '"Mother Alive"
                '    Return updateAnIndiDetail("moalive", indid, frmindch.txtIniChNewV.Text.ToUpper)

            Case "11" '"Father's First Name"
                result = "ffname"

            Case "12" '"Father's Juok Name"
                result = "fjname"

            Case "13" '"Father's ID"
                result = "fatherid"
                'TODO find table to update when this values change i.e. 36
                'Case "36" '"Father Alive"
                '    Return updateAnIndiDetail("faalive", indid, frmindch.txtIniChNewV.Text.ToUpper)

            Case "21" '"Sleeping Place Correction"
                result = "locationid"
                'get the guid of the latest residency to change its location
            Case "22" '"Eating Place Correction", "HouseholdID correction"
                result = "socialgpid"
                'get the guid of the latest membership to change its hhld

            Case "33" '"EatingPlace Change", "Household Change"
                result = "socialgpid"
            Case Else
                result = "Q"
        End Select

        Return result
    End Function


    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function get_compoundChange(ByVal compHHchangeCode As String) As String

        If compHHchangeCode = "42" Then 'signifies social group administrator change
            'check if the admin is amember of that social group

        ElseIf compHHchangeCode = "41" Then ' signifies compound administrator change
            'check if admin is a member of the compound

        ElseIf compHHchangeCode = "41" Then 'signifies compound head correction


        End If
        Return ""
    End Function

    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function get_Round(ByVal entrydate As Date) As String
        Select Case entrydate.Month
            Case 1, 2, 3, 4
                Return entrydate.Year.ToString + "1"
            Case 5, 6, 7, 8
                Return entrydate.Year.ToString + "2"
            Case 9, 10, 11, 12
                Return entrydate.Year.ToString + "3"
        End Select
        Return entrydate.Year.ToString
    End Function
#End Region
#Region "QC"
    'getcompound from locationid ,getcompound_from_locationid
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    Public Shared Function get_nextidafternext(ByVal individid As String) As String
        If individid Is Nothing Then
            Return Nothing
        End If
        ' Add your code here
        Dim result As String = Nothing
        Dim parts() As String = individid.Split("-")
        If (parts.Length = 4) And Not (individid.StartsWith("-") Or individid.EndsWith("-")) Then
            result = parts(0).ToString + "-" + parts(1).ToString + "-" + parts(2).ToString + "-" + (CInt(parts(3)) + 1).ToString
        Else
            result = Nothing
        End If
        Return result
    End Function
#End Region
End Class
