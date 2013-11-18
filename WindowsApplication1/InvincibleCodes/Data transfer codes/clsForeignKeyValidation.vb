Imports System.Data
Imports System.Data.SqlClient
Public Class clsForeignKeyValidation

    'constructor and variables
#Region "Constructors and variables"
    Private Shared objSingle As clsForeignKeyValidation
    Private Shared blCreated As Boolean
    Public worker As System.ComponentModel.BackgroundWorker
    Private da As clsDataAccess = clsDataAccess.getObject
    Private Sub New()
    End Sub
    Public Shared Function getObject() As clsForeignKeyValidation
        If blCreated = False Then
            objSingle = New clsForeignKeyValidation
            blCreated = True
            Return objSingle
        Else
            Return objSingle
        End If
    End Function

#End Region
    'laptop to main database.
#Region "Generic validation codes"

    Private Sub errorTables()
        Dim tabvaliditems As DataTable = Nothing
        For Each validationtables As DataRow In Me.da.getTableDataFromTempDB("select distinct table_name from validationitems").Rows
            MsgBox(validationtables("table_name").ToString)
            Me.da.exec_nonqueryInTEMPDB("UPDATE " + validationtables("table_name").ToString + " SET [errflag] = 'true', errdate=getdate()")
        Next
    End Sub
    
    Public Function ValidateforeignKey_Table_inTEMP_DSSHRS(ByVal tablename As String, ByVal wherepart As String) As Boolean
        '  RemoveDuplicateRecords(tablename)
        Dim returnValue As Boolean = True
        Dim tabvaliditems As DataTable = Me.da.getTableDataFromTempDB("select [table_name],[table_col],[ref_table],[ref_col],[coltype] " _
                        & " from validationitems where " _
                        & " (table_name='" + tablename.ToLower + "') and ([validtype]='s') and (isenabled=1)")
        'Dim j As Integer = 0
        Dim dt As DataTable = Me.da.getTableDataFromTempDB("select * from " + tablename + " where  " + wherepart)

        'If Not worker Is Nothing Then worker.ReportProgress(Nothing, "validating foreign key refrences for " & dt.Rows.Count & " records in " & tablename & " " & Now.ToString())
        For Each tempDataRec As DataRow In dt.Rows
            ' j = j + 1
            'If Not worker Is Nothing Then worker.ReportProgress(1, "Validating record " & j & " of  " & dt.Rows.Count & " in " & tablename)
            If tablename.Trim.ToLower.Trim = "dss.socialgroupadmin".Trim.ToLower Then
                returnValue = Me.validatesocialGroupadmin_TEMP_DSSHRS(tempDataRec, da.getrecordsCompound(tablename, tempDataRec).Trim, da.getrecordsRound(tablename, tempDataRec).Trim)
            Else
                returnValue = Me.validateRec_TEMP_DSSHRS(tempDataRec, tabvaliditems, da.getrecordsCompound(tablename, tempDataRec).Trim, da.getrecordsRound(tablename, tempDataRec).Trim)
            End If

            If tablename.Trim.ToLower = "dss.individual".Trim.ToLower Then
                If Me.da.hasSmallAgediffwithfather(tempDataRec, 13) Then
                    Me.da.saveError(tempDataRec("transit_id").ToString.Trim, "DSS.individual", "The father is too young", "", Now(), "", da.getrecordsCompound("dss.individual", tempDataRec).Trim, da.getrecordsRound("dss.individual", tempDataRec).Trim())
                    Me.da.exec_nonqueryInTEMPDB("UPDATE [dss].[individual] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + tempDataRec("transit_id").ToString.Trim)
                    returnValue = False
                End If
                If Me.da.hasSmallAgediffwithMother(tempDataRec, 13) Then
                    Me.da.saveError(tempDataRec("transit_id").ToString.Trim, "DSS.individual", "The mother is too young", "", Now(), "", da.getrecordsCompound("dss.individual", tempDataRec).Trim, da.getrecordsRound("dss.individual", tempDataRec).Trim())
                    Me.da.exec_nonqueryInTEMPDB("UPDATE [dss].[individual] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + tempDataRec("transit_id").ToString.Trim)
                    returnValue = False
                End If
            End If
            If tablename.Trim.ToLower = "dss.marriage" Then
                If Me.da.individualtooYounfForMarriage(tempDataRec("individid").ToString, 13) Then
                    Me.da.saveError(tempDataRec("transit_id").ToString.Trim, "dss.marriage", " individual too young for marriage ", "", Now(), "", da.getrecordsCompound("dss.marriage", tempDataRec).Trim, da.getrecordsRound("dss.marriage", tempDataRec).Trim())
                    Me.da.exec_nonqueryInTEMPDB("UPDATE [dss].[marriage] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + tempDataRec("transit_id").ToString.Trim)
                    returnValue = False
                End If
                If (Not tempDataRec("spouseid").ToString.Trim.Equals("")) AndAlso Me.da.spousetooYounfForMarriage(tempDataRec("spouseid").ToString, 13) Then
                    Me.da.saveError(tempDataRec("transit_id").ToString.Trim, "dss.marriage", "Spouse too young for marriage", "", Now(), "", da.getrecordsCompound("dss.marriage", tempDataRec).Trim, da.getrecordsRound("dss.marriage", tempDataRec).Trim())
                    Me.da.exec_nonqueryInTEMPDB("UPDATE [dss].[marriage] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + tempDataRec("transit_id").ToString.Trim)
                    returnValue = False
                End If
            End If

            If tablename.Trim.ToLower = "dss.pregoutcome" Then
                If Me.da.has_closePregnacy_Outcome(tempDataRec("individid").ToString, tempDataRec("date")) Then
                    Me.da.saveError(tempDataRec("transit_id").ToString.Trim, "dss.pregoutcome", " individual had another preg recently ", "", Now(), "", da.getrecordsCompound("dss.pregoutcome", tempDataRec).Trim, da.getrecordsRound("dss.pregoutcome", tempDataRec).Trim())
                    Me.da.exec_nonqueryInTEMPDB("UPDATE [dss].[pregoutcome] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + tempDataRec("transit_id").ToString.Trim)
                    returnValue = False
                End If
            End If
            If tablename.Trim.ToLower = "dss.visitation" Then
                If Me.da.has_Completed_Visitation(tempDataRec("locationid").ToString.Trim, tempDataRec("round").ToString.Trim) Then
                    Dim sql As String = "SELECT [TEMP_DSSHRS].[dbo].[getRevisitsWithCompStatus] ('" & tempDataRec("round").ToString.Trim & "','" & tempDataRec("locationid").ToString.Trim & "')"
                    If tempDataRec("Outcome").ToString.ToUpper.Trim.Equals("REVISIT") AndAlso Me.da.executeScalar_INMainDB(sql) > 0 Then

                    Else
                        Me.da.saveError(tempDataRec("transit_id").ToString.Trim, "dss.visitation", " Location has a completed visitation In MainDB ", "", Now(), "", da.getrecordsCompound("dss.visitation", tempDataRec).Trim, da.getrecordsRound("dss.visitation", tempDataRec).Trim())
                        Me.da.exec_nonqueryInTEMPDB("UPDATE [dss].[visitation] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + tempDataRec("transit_id").ToString.Trim)
                        returnValue = False
                    End If
                End If
            End If

        Next
        'If Not worker Is Nothing Then worker.ReportProgress(Nothing, " finished validating foreign key refrences " & tablename & " " & Now.ToString())
        Return returnValue
    End Function
    Public Function ValidateforeignKey_Row_INTEMP_DSSHRS(ByVal tablename As String, ByVal tempDataRec As DataRow) As Boolean
        '  RemoveDuplicateRecords(tablename)
        Dim returnValue As Boolean = True
        Dim tabvaliditems As DataTable = Me.da.getTableDataFromTempDB("select [table_name],[table_col],[ref_table],[ref_col],[coltype] " _
                        & " from validationitems where " _
                        & " (table_name='" + tablename.ToLower + "') and ([validtype]='s') and (isenabled=1)")
        'Dim j As Integer = 0
        'Dim dt As DataTable = Me.da.getTableDataFromTempDB("select * from " + tablename + " where  " + wherepart)

        'If Not worker Is Nothing Then worker.ReportProgress(Nothing, "validating foreign key refrences for " & dt.Rows.Count & " records in " & tablename & " " & Now.ToString())
        'For Each tempDataRec As DataRow In dt.Rows
        ' j = j + 1
        'If Not worker Is Nothing Then worker.ReportProgress(1, "Validating record " & j & " of  " & dt.Rows.Count & " in " & tablename)
        If tablename.Trim.ToLower.Trim = "dss.socialgroupadmin".Trim.ToLower Then
            returnValue = Me.validatesocialGroupadmin_TEMP_DSSHRS(tempDataRec, da.getrecordsCompound(tablename, tempDataRec).Trim, da.getrecordsRound(tablename, tempDataRec).Trim)
        Else
            returnValue = Me.validateRec_TEMP_DSSHRS(tempDataRec, tabvaliditems, da.getrecordsCompound(tablename, tempDataRec).Trim, da.getrecordsRound(tablename, tempDataRec).Trim)
        End If
        'Next
        'If Not worker Is Nothing Then worker.ReportProgress(Nothing, " finished validating foreign key refrences " & tablename & " " & Now.ToString())
        Return returnValue
    End Function
    Public Sub RemoveDuplicateRecords(ByVal tablename As String)


        '  worker.ReportProgress(Nothing, " Starting removing duplicate records from " & tablename & " " & Now.ToString())

    End Sub

    Private Function validateRec_TEMP_DSSHRS(ByVal currentRecord As DataRow, ByVal validtable As DataTable, ByVal village As String, ByVal round As String) As Boolean
        Dim tablename As String = ""
        Dim colname As String = ""
        Dim reftablename As String = ""
        Dim refcolname As String = ""
        Dim id As String = ""
        Dim returnValue As Boolean = True
        For Each validitems As DataRow In validtable.Rows
            tablename = validitems("table_name").ToString.ToLower.Trim
            colname = validitems("table_col").ToString.Trim
            reftablename = validitems("ref_table").ToString.ToLower.Trim
            refcolname = validitems("ref_col").ToString.Trim
            id = currentRecord(colname).ToString
            If Not IsDBNull(currentRecord(colname)) Then
                'If (currentRecord(colname).ToString.Trim = "") Then
                '    MsgBox(tablename & " col " & colname)
                '    'Me.da.exec_nonqueryInTEMPDB("UPDATE " + tablename + " SET [errflag] = 'true ' , errdate=getdate() where " + colname + "='" + id + "'")
                'End If
                If Me.da.checkifrecordexists_INMainDB(refcolname, id, reftablename) Then
                    'check for duplicates
                    If tablename.Trim.ToLower = reftablename.Trim.ToLower Then
                        Me.da.saveError(currentRecord("transit_id").ToString, tablename, colname + " Duplicate record", "", Now(), "", village, round)
                        Me.da.exec_nonqueryInTEMPDB("UPDATE " + tablename + " SET [errflag] = 'true' , errdate=getdate() where " + colname + "='" + id + "'")
                        returnValue = False
                    Else
                        ' reference table has duplicates
                        If Me.da.checkifrecordexists_INTEMPDB(refcolname, id, reftablename) Then
                            Me.da.saveError(currentRecord("transit_id").ToString, tablename, colname + ": " & refcolname & " in " & reftablename & " table is Duplicate record", "", Now(), "", village, round)
                            Me.da.exec_nonqueryInTEMPDB("UPDATE " + tablename + " SET [errflag] = 'true' , errdate=getdate() where " + colname + "='" + id + "'")
                            returnValue = False
                        End If

                    End If
                Else
                    'record not in reference table
                    If tablename <> reftablename Then
                        If ((colname.ToLower = "chheadid") Or (colname.ToLower = "motherid") Or (colname.ToLower = "fatherid")) And (id.Trim.ToLower = "unk") Then

                        Else
                            Me.da.saveError(currentRecord("transit_id").ToString, tablename, colname + " not in " + reftablename, "", Now(), "", village, round)
                            Me.da.exec_nonqueryInTEMPDB("UPDATE " + tablename + " SET [errflag] = 'true', errdate=getdate() where " + colname + "='" + id + "'")
                            returnValue = False
                        End If
                    End If
                    'check if there are multiple records in temporatory database
                    If tablename.Trim.ToLower = reftablename.Trim.ToLower Then
                        If Me.da.checkifMultiplerecordexists_INTEMPDB(refcolname, id, reftablename) Then
                            Me.da.saveError(currentRecord("transit_id").ToString, tablename, colname + " Duplicate record in temp", "", Now(), "", village, round)
                            Me.da.exec_nonqueryInTEMPDB("UPDATE " + tablename + " SET [errflag] = 'true' , errdate=getdate() where " + colname + "='" + id + "'")
                            returnValue = False
                        End If
                    End If
                End If

            End If
        Next
        Return returnValue
    End Function
    Private Function validatesocialGroupadmin_TEMP_DSSHRS(ByVal socialgroupadminrec As DataRow, ByVal village As String, ByVal round As String) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT count(*)  FROM [DSSHRS].[DSS].[SocialGroupadmin] " _
                            & " where   ([socialgpid]='" + socialgroupadminrec("socialgpid").ToString + "') and ([round]='" + socialgroupadminrec("round").ToString + "') "

        Dim i As Integer = Me.da.executeScalar_INMainDB(sql)
        If Not (i = 0) Then
            Me.da.saveError(socialgroupadminrec("transit_id").ToString, "DSS.SocialGroupadmin", "Duplicate record", "", Now(), "", village, round)
            Me.da.exec_nonqueryInTEMPDB("UPDATE DSS.SocialGroupadmin SET [errflag] = 'true' , errdate=getdate() where transit_id=" + socialgroupadminrec("transit_id").ToString + "")
            returnValue = False
        Else
            'socialgpid
            If Not Me.da.checkifrecordexists_INMainDB("socialgpid", socialgroupadminrec("socialgpid").ToString.Trim, "[DSSHRS].[DSS].[socialgroup]") Then
                Me.da.saveError(socialgroupadminrec("transit_id").ToString, "DSS.SocialGroupadmin", "socialgpid not in DSS.socialgroup", "", Now(), "", village, round)
                Me.da.exec_nonqueryInTEMPDB("UPDATE DSS.SocialGroupadmin SET [errflag] = 'true', errdate=getdate() where transit_id=" + socialgroupadminrec("transit_id").ToString + "")
                returnValue = False
            End If
            'round
            If Not Me.da.checkifrecordexists_INMainDB("round_num", socialgroupadminrec("round").ToString, "[DSSHRS].[DSS].[round]") Then
                Me.da.saveError(socialgroupadminrec("transit_id").ToString, "DSS.SocialGroupadmin", "round not in DSS.round", "", Now(), "", village, round)
                Me.da.exec_nonqueryInTEMPDB("UPDATE DSS.SocialGroupadmin SET [errflag] = 'true', errdate=getdate() where transit_id=" + socialgroupadminrec("transit_id").ToString + "")
                returnValue = False
            End If
            'adminid
            If Not Me.da.checkifrecordexists_INMainDB("individid", socialgroupadminrec("adminid").ToString, "[DSSHRS].[DSS].[individual]") Then
                Me.da.saveError(socialgroupadminrec("transit_id").ToString, "DSS.SocialGroupadmin", "adminid not in DSS.individual", "", Now(), "", village, round)
                Me.da.exec_nonqueryInTEMPDB("UPDATE DSS.SocialGroupadmin SET [errflag] = 'true', errdate=getdate() where transit_id=" + socialgroupadminrec("transit_id").ToString + "")
                returnValue = False
            End If
        End If
        Return returnValue
    End Function
    Public Function validateIndividualChanges_Table_InTEMP_DSSHRS(ByVal query As String) As Boolean
        Dim returnValue As Boolean = True
        Dim changes As DataTable = Me.da.getTableDataFromTempDB(query)
        'Dim changes As DataTable = Me.da.getTableDataFromTempDB("select * from [MHRS_SYS].[Changes] where tablename='dss.individual'")
        Dim individualrecords As DataTable = Nothing
        'If Me.da.exec_nonqueryInTEMPDB("UPDATE [MHRS_SYS].[Changes] SET [errflag] = 'false ', errdate=getdate()") < 1 Then
        '    'MsgBox("There seems to be no records in changes")
        'End If
        Dim village As String = ""
        Dim round As String = ""
        Dim olddobchanges As New Date()
        Dim olddobIndi As New Date()

        For Each change As DataRow In changes.Rows
            village = da.getrecordsCompound("MHRS_SYS.Changes", change).Trim
            individualrecords = Nothing
            round = da.getrecordsRound("mhrs_sys.Changes", change).Trim
            Try

                If change("oldValue").ToString.Trim = change("NewValue").ToString.Trim Then
                    Me.da.saveError(change("transit_id").ToString.Trim, "MHRS_SYS.Changes", "Old value same as new value", "", Now(), "", village, round)
                    Me.da.exec_nonqueryInTEMPDB("UPDATE [MHRS_SYS].[Changes] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + change("transit_id").ToString.Trim)
                    returnValue = False
                Else
                    individualrecords = Me.da.getTableDataFromMAINDB("select * from [dss].[individual] where individid='" + change("recordid").ToString.Trim + "'")
                    If individualrecords.Rows.Count < 1 Then
                        Me.da.saveError(change("transit_id").ToString.Trim, "MHRS_SYS.Changes", "Individual not in the database", "", Now(), "", village, round)
                        Me.da.exec_nonqueryInTEMPDB("UPDATE [MHRS_SYS].[Changes] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + change("transit_id").ToString.Trim)
                        returnValue = False
                    Else
                        For Each individual As DataRow In individualrecords.Rows
                            If change("tablename").ToString.Trim.ToLower = "dss.individual" Then
                                If change("colname").ToString.Trim.ToLower.Trim = "dob" Then
                                    olddobchanges = New Date(CInt(change("oldValue").ToString.Trim.ToLower.Substring(6, 4)), _
                                     CInt(change("oldValue").ToString.Trim.ToLower.Substring(3, 2)), _
                                    CInt(change("oldValue").ToString.Trim.ToLower.Substring(0, 2))) 'DateTime.Parse(change("oldValue").ToString) '
                                    olddobIndi = individual(change("colname").ToString.Trim)
                                    If (olddobIndi.Day <> olddobchanges.Day) Or (olddobIndi.Year <> olddobchanges.Year) Or (olddobIndi.Month <> olddobchanges.Month) Then
                                        Me.da.saveError(change("transit_id").ToString.Trim, "MHRS_SYS.Changes", "old value in database not same as old value in changes", "", Now(), "", village, round)
                                        Me.da.exec_nonqueryInTEMPDB("UPDATE [MHRS_SYS].[Changes] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + change("transit_id").ToString.Trim)
                                        returnValue = False
                                    End If
                                Else
                                    If individual(change("colname").ToString.Trim).ToString.ToLower.Trim <> change("oldValue").ToString.Trim.ToLower Then
                                        Me.da.saveError(change("transit_id").ToString.Trim, "MHRS_SYS.Changes", "old value in database not same as old value in changes", "", Now(), "", village, round)
                                        Me.da.exec_nonqueryInTEMPDB("UPDATE [MHRS_SYS].[Changes] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + change("transit_id").ToString.Trim)
                                        returnValue = False
                                    Else
                                        ''update
                                    End If
                                End If

                            End If
                        Next
                    End If
                End If

            Catch ex As Exception
                returnValue = False
            End Try
        Next
        Return returnValue
    End Function
    Public Function validateIndividualChanges_Row_inTEMP_DSSHRS(ByVal change As DataRow) As Boolean
        Dim returnValue As Boolean = True
        'Dim changes As DataTable = Me.da.getTableDataFromTempDB("select * from [MHRS_SYS].[Changes] where tablename='dss.individual'")
        Dim individualrecords As DataTable = Nothing
        'If Me.da.exec_nonqueryInTEMPDB("UPDATE [MHRS_SYS].[Changes] SET [errflag] = 'false ', errdate=getdate()") < 1 Then
        '    'MsgBox("There seems to be no records in changes")
        'End If
        Dim village As String = ""
        Dim round As String = ""
        Dim olddobchanges As New Date()
        Dim olddobIndi As New Date()
        village = da.getrecordsCompound("MHRS_SYS.Changes", change).Trim
        individualrecords = Nothing
        round = da.getrecordsRound("mhrs_sys.Changes", change).Trim
        Try

       
            If change("oldValue").ToString.Trim = change("NewValue").ToString.Trim Then
                Me.da.saveError(change("transit_id").ToString.Trim, "MHRS_SYS.Changes", "Old value same as new value", "", Now(), "", village, round)
                Me.da.exec_nonqueryInTEMPDB("UPDATE [MHRS_SYS].[Changes] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + change("transit_id").ToString.Trim)
                returnValue = False
            Else
                individualrecords = Me.da.getTableDataFromMAINDB("select * from [dss].[individual] where individid='" + change("recordid").ToString.Trim + "'")
                If individualrecords.Rows.Count < 1 Then
                    Me.da.saveError(change("transit_id").ToString.Trim, "MHRS_SYS.Changes", "Individual not in the database", "", Now(), "", village, round)
                    Me.da.exec_nonqueryInTEMPDB("UPDATE [MHRS_SYS].[Changes] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + change("transit_id").ToString.Trim)
                    returnValue = False
                Else
                    For Each individual As DataRow In individualrecords.Rows
                        If change("colname").ToString.Trim.ToLower.Trim = "dob" Then
                            olddobchanges = New Date(CInt(change("oldValue").ToString.Trim.ToLower.Substring(6, 4)), _
                            CInt(change("oldValue").ToString.Trim.ToLower.Substring(3, 2)), _
                            CInt(change("oldValue").ToString.Trim.ToLower.Substring(0, 2)))
                            olddobIndi = individual(change("colname").ToString.Trim)
                            If (olddobIndi.Day <> olddobchanges.Day) Or (olddobIndi.Year <> olddobchanges.Year) Or (olddobIndi.Month <> olddobchanges.Month) Then
                                Me.da.saveError(change("transit_id").ToString.Trim, "MHRS_SYS.Changes", "old value in database not same as old value in changes", "", Now(), "", village, round)
                                Me.da.exec_nonqueryInTEMPDB("UPDATE [MHRS_SYS].[Changes] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + change("transit_id").ToString.Trim)
                                returnValue = False
                            End If
                        Else
                            If change("tablename").ToString.Trim.ToLower = "dss.individual" Then
                                If individual(change("colname").ToString.Trim).ToString.ToLower.Trim <> change("oldValue").ToString.Trim.ToLower Then
                                    Me.da.saveError(change("transit_id").ToString.Trim, "MHRS_SYS.Changes", "old value in database not same as old value in changes", "", Now(), "", village, round)
                                    Me.da.exec_nonqueryInTEMPDB("UPDATE [MHRS_SYS].[Changes] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + change("transit_id").ToString.Trim)
                                    returnValue = False
                                Else
                                    ''update
                                End If
                            End If
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            Return False
        End Try
        Return returnValue
    End Function
    Public Function validateResidencyChanges_table_InTEMP_DSSHRS(ByVal query As String) As Boolean
        Dim returnValue As Boolean = True
        Dim changes As DataTable = Me.da.getTableDataFromTempDB(query)
        'Dim changes As DataTable = Me.da.getTableDataFromTempDB("select * from [MHRS_SYS].[Changes] where tablename='dss.residency'")
        Dim Residencyrecords As DataTable = Nothing
        'If Me.da.exec_nonqueryInTEMPDB("UPDATE [MHRS_SYS].[Changes] SET [errflag] = 'false ', errdate=getdate()") < 1 Then
        '    'MsgBox("There seems to be no records in changes")
        'End If
        Dim village As String = ""
        Dim round As String = ""
        For Each change As DataRow In changes.Rows
            village = da.getrecordsCompound("MHRS_SYS.Changes", change).Trim
            Residencyrecords = Nothing
            round = da.getrecordsRound("mhrs_sys.Changes", change).Trim

            'Make sure that changes to loationis are done on the same compound
            Dim objVal As clsvalidations = clsvalidations.getObject


            If change("colname").ToString.ToLower.Trim.Equals("locationid") And objVal.getIDSubstring(change("oldValue").ToString.Trim, idTypes.COMPOUND) <> objVal.getIDSubstring(change("NewValue").ToString.Trim, idTypes.COMPOUND) Then
                Me.da.saveError(change("transit_id").ToString.Trim, "MHRS_SYS.Changes", "Residency locationid Changes can only be done within the same compound", "", Now(), "", village, round)
                Me.da.exec_nonqueryInTEMPDB("UPDATE [MHRS_SYS].[Changes] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + change("transit_id").ToString.Trim)
                returnValue = False
            End If


            If change("oldValue").ToString.Trim = change("NewValue").ToString.Trim Then
                Me.da.saveError(change("transit_id").ToString.Trim, "MHRS_SYS.Changes", "Old value same as new value", "", Now(), "", village, round)
                Me.da.exec_nonqueryInTEMPDB("UPDATE [MHRS_SYS].[Changes] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + change("transit_id").ToString.Trim)
                returnValue = False
            Else
                Residencyrecords = Me.da.getTableDataFromMAINDB("select * from [dss].[residency] where residencyid='" + change("recordid").ToString.Trim + "'")
                If Residencyrecords.Rows.Count < 1 Then
                    Me.da.saveError(change("transit_id").ToString.Trim, "MHRS_SYS.Changes", "Residency record not in the database", "", Now(), "", village, round)
                    Me.da.exec_nonqueryInTEMPDB("UPDATE [MHRS_SYS].[Changes] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + change("transit_id").ToString.Trim)
                    returnValue = False
                Else
                    For Each residency As DataRow In Residencyrecords.Rows
                        If change("tablename").ToString.Trim.ToLower = "dss.residency" Then
                            If residency(change("colname").ToString.Trim).ToString.ToLower.Trim <> change("oldValue").ToString.ToLower.Trim Then
                                Me.da.saveError(change("transit_id").ToString.Trim, "MHRS_SYS.Changes", "old value in database not same as old value in changes", "", Now(), "", village, round)
                                Me.da.exec_nonqueryInTEMPDB("UPDATE [MHRS_SYS].[Changes] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + change("transit_id").ToString.Trim)
                                returnValue = False
                            Else
                                ''update
                            End If
                        End If
                    Next
                End If
            End If
        Next
        Return returnValue
    End Function
    Public Function validateResidencyChanges_row_InTEMP_DSSHRS(ByVal change As DataRow) As Boolean
        Dim returnValue As Boolean = True

        'Dim changes As DataTable = Me.da.getTableDataFromTempDB("select * from [MHRS_SYS].[Changes] where tablename='dss.residency'")
        Dim Residencyrecords As DataTable = Nothing
        'If Me.da.exec_nonqueryInTEMPDB("UPDATE [MHRS_SYS].[Changes] SET [errflag] = 'false ', errdate=getdate()") < 1 Then
        '    'MsgBox("There seems to be no records in changes")
        'End If
        Dim village As String = ""
        Dim round As String = ""

        village = da.getrecordsCompound("MHRS_SYS.Changes", change).Trim
        Residencyrecords = Nothing
        round = da.getrecordsRound("mhrs_sys.Changes", change).Trim

        'Make sure that changes to loationis are done on the same compound
        Dim objVal As clsvalidations = clsvalidations.getObject


        If change("colname").ToString.ToLower.Trim.Equals("locationid") And objVal.getIDSubstring(change("oldValue").ToString.Trim, idTypes.COMPOUND) <> objVal.getIDSubstring(change("NewValue").ToString.Trim, idTypes.COMPOUND) Then
            Me.da.saveError(change("transit_id").ToString.Trim, "MHRS_SYS.Changes", "Residency locationid Changes can only be done within the same compound", "", Now(), "", village, round)
            Me.da.exec_nonqueryInTEMPDB("UPDATE [MHRS_SYS].[Changes] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + change("transit_id").ToString.Trim)
            returnValue = False
        End If
        If change("oldValue").ToString.Trim = change("NewValue").ToString.Trim Then
            Me.da.saveError(change("transit_id").ToString.Trim, "MHRS_SYS.Changes", "Old value same as new value", "", Now(), "", village, round)
            Me.da.exec_nonqueryInTEMPDB("UPDATE [MHRS_SYS].[Changes] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + change("transit_id").ToString.Trim)
            returnValue = False
        Else
            Residencyrecords = Me.da.getTableDataFromMAINDB("select * from [dss].[residency] where residencyid='" + change("recordid").ToString.Trim + "'")
            If Residencyrecords.Rows.Count < 1 Then
                Me.da.saveError(change("transit_id").ToString.Trim, "MHRS_SYS.Changes", "Residency record not in the database", "", Now(), "", village, round)
                Me.da.exec_nonqueryInTEMPDB("UPDATE [MHRS_SYS].[Changes] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + change("transit_id").ToString.Trim)
                returnValue = False
            Else
                For Each residency As DataRow In Residencyrecords.Rows
                    If change("tablename").ToString.Trim.ToLower = "dss.residency" Then
                        If residency(change("colname").ToString.Trim).ToString.ToLower.Trim <> change("oldValue").ToString.ToLower.Trim Then
                            Me.da.saveError(change("transit_id").ToString.Trim, "MHRS_SYS.Changes", "old value in database not same as old value in changes", "", Now(), "", village, round)
                            Me.da.exec_nonqueryInTEMPDB("UPDATE [MHRS_SYS].[Changes] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + change("transit_id").ToString.Trim)
                            returnValue = False
                        Else
                            ''update
                        End If
                    End If
                Next
            End If
        End If

        Return returnValue
    End Function
#End Region
    'validation codes
#Region "other validation codes"
    'Private recexists As Boolean = False
    'Private Function istempid(ByVal id As String) As Boolean
    '    If id.Trim.Substring(0, 1).ToLower = "t" Then
    '        Return True
    '    Else
    '        Return False
    '    End If
    'End Function
#End Region

#Region "Generic validation part two"

   
    Public Function ValidateforeignKey_DSSHRS(ByVal tablename As String, ByVal tempDataRec As DataRow) As Boolean
        Dim returnVAl As Boolean = True
        Dim tabvaliditems As DataTable = Me.da.getTableDataFromTempDB("select [table_name],[table_col],[ref_table],[ref_col],[coltype] " _
                        & " from validationitems where " _
                        & " (table_name='" + tablename.ToLower + "') and ([validtype]='s') and (isenabled=1)")
        With tempDataRec
            If tablename.Trim.ToLower.Trim = "dss.socialgroupadmin".Trim.ToLower Then
                returnVAl = Me.validatesocialGroupadmin_DSSHRS(tempDataRec, da.getrecordsCompound(tablename, tempDataRec).Trim, da.getrecordsRound(tablename, tempDataRec).Trim)
            Else
                returnVAl = Me.validateRec_DSSHRS(tempDataRec, tabvaliditems, da.getrecordsCompound(tablename, tempDataRec).Trim, da.getrecordsRound(tablename, tempDataRec).Trim)
            End If
        End With
        Return returnVAl
    End Function
    Private Function validateRec_DSSHRS(ByVal currentRecord As DataRow, ByVal validtable As DataTable, ByVal village As String, ByVal round As String) As Boolean
        Dim tablename As String = ""
        Dim colname As String = ""
        Dim reftablename As String = ""
        Dim refcolname As String = ""
        Dim id As String = ""

        Dim returnVAl As Boolean = True
        For Each validitems As DataRow In validtable.Rows
            tablename = validitems("table_name").ToString.ToLower.Trim
            colname = validitems("table_col").ToString.Trim
            reftablename = validitems("ref_table").ToString.ToLower.Trim
            refcolname = validitems("ref_col").ToString.Trim
            id = currentRecord(colname).ToString
            If Not IsDBNull(currentRecord(colname)) Then
                If Not Me.da.checkifrecordexists_INMainDB(refcolname, id, reftablename) Then
                    'record not in reference table
                    If tablename <> reftablename Then
                        If ((colname.ToLower = "chheadid") Or (colname.ToLower = "motherid") Or (colname.ToLower = "fatherid")) And (id.Trim.ToLower = "unk") Then

                        Else
                            Me.da.saveError(currentRecord(colname).ToString, tablename, colname + " not in " + reftablename, "", Now(), "", village, round)
                            returnVAl = False
                        End If
                    End If
                End If
                
            End If
        Next
        Return returnVAl
    End Function
    Private Function validatesocialGroupadmin_DSSHRS(ByVal socialgroupadminrec As DataRow, ByVal village As String, ByVal round As String) As Boolean
        Dim returnValue As Boolean = True

        'socialgpid
        If Not Me.da.checkifrecordexists_INMainDB("socialgpid", socialgroupadminrec("socialgpid").ToString.Trim, "[DSSHRS].[DSS].[socialgroup]") Then
            Me.da.saveError(socialgroupadminrec("socialgpid").ToString, "DSS.SocialGroupadmin", "socialgpid not in DSS.socialgroup", "", Now(), "", village, round)
            returnValue = False
        End If
        'round
        If Not Me.da.checkifrecordexists_INMainDB("round_num", socialgroupadminrec("round").ToString, "[DSSHRS].[DSS].[round]") Then
            Me.da.saveError(socialgroupadminrec("socialgpid").ToString, "DSS.SocialGroupadmin", "round not in DSS.round", "", Now(), "", village, round)
            returnValue = False
        End If
        'adminid
        If Not Me.da.checkifrecordexists_INMainDB("individid", socialgroupadminrec("adminid").ToString, "[DSSHRS].[DSS].[individual]") Then
            Me.da.saveError(socialgroupadminrec("socialgpid").ToString, "DSS.SocialGroupadmin", "adminid not in DSS.individual", "", Now(), "", village, round)
            returnValue = False
        End If
        Return returnValue
    End Function
#End Region
End Class
