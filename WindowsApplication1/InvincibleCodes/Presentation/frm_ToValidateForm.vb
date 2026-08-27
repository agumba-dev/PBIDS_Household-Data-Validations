Imports System.Data.SqlClient
Imports Microsoft.SqlServer.Management.Smo
Imports DgvFilterPopup
Imports System.Data

Public Class frm_ToValidateForm

#Region " constructor and Variables"
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
    Private errorlogstream As System.IO.StreamWriter
    Private autorun As Boolean = False
    Private autoclose As Boolean = True
    Public Shared idColumn As String
    Public combinederrorText As String
    Dim errorTypes As String
    Shared errorvillage, errorcompound As String
    Private dacc As clsDataAccess = clsDataAccess.getObject
    Shared errorRound As String
    Public objUtil As clsUtilities = clsUtilities.getObject
    Public da As clsDataAccess = clsDataAccess.getObject
    Private clsGlobalVariable As clsGlobalVariables = clsGlobalVariables.getObject
    Public objRef As clsformrefrences = clsformrefrences.getObject
    Friend ObjDbAccess As clsdbAccess = clsdbAccess.getObject
    Private Enum record_state
        valid = 2
        invalid = 1
        defaultstate = 0
    End Enum
#End Region

#Region "Procedures"
    Private Function filterCriteria() As String
        Select Case cmb_errorView.Text.ToUpper
            Case "ALL ERRORS"
                Return ""
            Case "PENDING"
                Return "rec_status like 'P' AND "
            Case "CLEANED ONLY"
                Return "rec_status='C' AND "
            Case "WITH COMMENTS"
                Return "rec_status='C' and Comments <>'' AND "
            Case Else
                Return ""
        End Select
    End Function





    Private Function validateColumnSkip(ByVal validations As DataRow, ByVal record As DataRow, ByVal clmn As DataColumn, ByVal tablename As String) As Integer
        'If clmn.ColumnName.Trim.ToLower = "cardavail" Then
        '    MsgBox("fdgdf")
        'End If
        '  Dim value As Object = record.Item(clmn)
        Dim skipLogicStr As String = ""
        'get the skip pattern
        Dim valid As Boolean = False
        Dim skipLogics As String() = validations.Item("skipLogic").ToString.Split(";"c)
        For Each str As String In skipLogics
            '  If String.IsNullOrEmpty(str) Then Continue For
            skipLogicStr = skipLogicStr + str

        Next

        'check if skip logic is available if not then just validate for column values
        If skipLogicStr.Trim = "" Then
            Return validateColumnValue(validations, record, clmn, tablename)
            'If validateColumnValue(validations, record, clmn, tablename, True) Then
            '    Return 2
            'Else
            '    Return 1
            'End If

        End If
        'createa a dataTable put the row in it and query the table to see whether the inserted row 
        ' meets the skip criteria
        Dim dt As New DataTable()
        dt = record.Table.Clone

        Dim r As DataRow = dt.NewRow()
        ' dt.Load(record)
        r.ItemArray = record.ItemArray
        dt.Rows.Add(r)

        ' dt.Rows.Add(record)
        Dim results As DataRow() = dt.Select(skipLogicStr)
        If results.Length = 0 Then ' row dosnt meet skip criteria hence  return value denoting this...if it is the last skip criteria then validate for default
            ' Return validateColumnValue(validations, record, clmn, tablename)
            Return 0
            'Dim value As Object = record.Item(clmn)
            'Dim defaultV As String = validations.Item("DefaultValue").ToString
            'If defaultV.Trim = "" Then ' column allows for nulls and empty string as default value
            '    If Not (value Is Nothing OrElse value.ToString.Trim = "") Then
            '        saveError(record.Item(idColumn).ToString, tablename, validations.Item("ErrorDescSkipLogic").ToString, "should be default value (null or empty)", Now(), "")
            '    End If
            'Else ' column has a default value so compare
            '    If Not (value.ToString.Trim.ToUpper = defaultV.ToUpper) Then
            '        saveError(record.Item(idColumn).ToString, tablename, validations.Item("ErrorDescSkipLogic").ToString, "should be default value", Now(), "")
            '    End If
            'End If


        Else
            If Not record.Item(clmn).Equals(DBNull.Value) Then
                ' row meets criteria so now just check for valid Value
                If validateColumnValue(validations, record, clmn, tablename) = 1 Then
                    '  saveError(record.Item(idColumn).ToString, tablename, validations.Item("ErrorDescSkipLogic").ToString, "", Now(), "", errorvillage)
                    Return 1
                End If
            End If
        End If
        'test the skip logic
        Return 2
    End Function
    Public Sub getTables()
        dacc.mydatabase = dacc.myServer.Databases.Item("TEMP_DSSHRS")
        lstbox_tables.Items.Clear()
        For Each tble As Table In dacc.mydatabase.Tables

            ' get only tables that are user defined
            If Not tble.IsSystemObject() Then lstbox_tables.Items.Add(tble)

        Next
    End Sub
    Private Sub getErrors(ByVal sqlquerry As String)
        Dim cmd As New SqlCommand(sqlquerry, Me.clsGlobalVariable.HRS_Temp_DBCon)
        Dim dt As New DataTable()
        Dim da As New SqlDataAdapter(cmd)
        dt.Clear()
        da.Fill(dt)
        Dim bs As New BindingSource(dt, "")
        dgr_results.DataSource = bs
        bndNvgerrors.BindingSource = bs
    End Sub
    Private Function generatekeyValues(ByVal rec As DataRow, ByVal tab As DataTable) As String
        ' Dim tab As DataTable = Me.da.getTableDataFromMAINDB("SELECT COLUMN_NAME FROM PrimaryKeyColumns where tableschema+'.'+TABLE_NAME='" + fulltablename + "'")
        Dim ret As String = " "
        For Each row As DataRow In tab.Rows
            ret = ret + "(" + row("COLUMN_NAME").ToString.Trim + "=" + rec(row("COLUMN_NAME").ToString.Trim).ToString.Trim + ") ,"
        Next
        ret = ret.TrimEnd(","c)
        Return ret.Replace(",", " and ")
    End Function
    Public Function validateMainDB(ByVal dbTable As Table) As Boolean
        Dim str As String = "Select * FROM " & dbTable.Schema & "." & dbTable.Name & " where rec_status not like '%X%'"
        Dim tab As DataTable
        tab = Me.da.getTableDataFromMAINDB("SELECT COLUMN_NAME FROM PrimaryKeyColumns where tableschema+'.'+TABLE_NAME='" + dbTable.Schema & "." & dbTable.Name + "'")
        Dim tablerecords As DataTable = Me.da.getTableDataFromMAINDB(str)
        Me.da.validationtype = mhrsSyncValidationTypes.batchprocessing
        Me.clsGlobalVariable.currectDBtoValidate = datalevel.DSSHRS
        frmDataTransfer.initialiseGlobalVariables()
        Dim data_transfer As clsDataTransfer = clsDataTransfer.getObject
        data_transfer.da.validationtype = mhrsSyncValidationTypes.batchprocessing
        data_transfer.da.UserAppvalidationerrors = ""
        Dim j As Integer = 0
        Dim count As Integer = tablerecords.Rows.Count
        For Each record As DataRow In tablerecords.Rows
            'dacc.initializeServerAndDB()
            Me.combinederrorText = ""
            Dim status As Boolean = True

            errorvillage = dacc.getrecordsCompound(dbTable.Schema & "." & dbTable.Name, record)
            errorRound = dacc.getrecordsRound(dbTable.Schema & "." & dbTable.Name, record)

            Me.clsGlobalVariable.currectRecPrimarykeyValues = Me.generatekeyValues(record, tab)
            j = j + 1
            LabelProgress.Text = "Validating record " & j & " of  " & count & " in " & dbTable.Schema & "." & dbTable.Name
            Try
                If Me.getRowValidations(dbTable.Schema, dbTable.Name, record, , False) = False Then
                    status = False
                End If
            Catch ex As Exception
                ' MsgBox(ex.Message)
            End Try

            If data_transfer.getRowValidations(datalevel.DSSHRS, dbTable.Schema.ToLower, dbTable.Name.ToLower, record, , False) = False Then
                status = False
            End If
        Next
    End Function
    Public Function validateTable(ByVal db As datalevel, ByVal dbTable As Table, ByVal data_transfer As clsDataTransfer, ByVal processType As ValidationType) As Boolean
        Dim str As String = "Select * FROM " & dbTable.Schema & "." & dbTable.Name & " where rec_status not like '%X%'"
        Dim cmd As New SqlCommand(str, Me.clsGlobalVariable.HRS_Temp_DBCon)
        Dim dt As New DataTable()
        Dim da As New SqlDataAdapter(cmd)
        dt.Clear()
        da.Fill(dt)
        'get tables primary key columns for errortable
        Dim tab As New DataTable
        Select Case db
            Case datalevel.DSSHRS
                tab = Me.da.getTableDataFromMAINDB("SELECT COLUMN_NAME FROM PrimaryKeyColumns where tableschema+'.'+TABLE_NAME='" + dbTable.Schema & "." & dbTable.Name + "'")
            Case datalevel.TEMP_DSSHRS
                tab = Me.da.getTableDataFromTempDB("SELECT COLUMN_NAME FROM PrimaryKeyColumns where tableschema+'.'+TABLE_NAME='" + dbTable.Schema & "." & dbTable.Name + "'")
            Case Else
        End Select

        idColumn = dacc.getIDColumn(dbTable.Name, Me.clsGlobalVariable.HRS_Temp_DBCon)
        Dim count As Integer = dt.Rows.Count
        Dim j As Integer = 0
        Dim ercount As Integer = 0
        For Each row As DataRow In dt.Rows
            'display recordidvalues
            Me.clsGlobalVariable.currectRecPrimarykeyValues = Me.generatekeyValues(row, tab)
            Me.clsGlobalVariable.currectDBtoValidate = db
            j = j + 1
            errorvillage = dacc.getrecordsCompound(dbTable.Schema & "." & dbTable.Name, row)
            'Dim errorlocDet As String() = getrecordsVillage(dbTable.Schema & "." & dbTable.Name, row)
            'errorvillage = errorlocDet(0)
            'errorcompound = errorlocDet(1)
            errorRound = dacc.getrecordsRound(dbTable.Schema & "." & dbTable.Name, row)
            LabelProgress.Text = "Validating record " & j & " of  " & count & " in " & dbTable.Schema & "." & dbTable.Name
            Dim isvalidRecord As Boolean = getRowValidations(dbTable.Schema, dbTable.Name, row)

            If Not isvalidRecord Then
                ercount = ercount + 1
                'add the error flag error
                dacc.updateErrorFlag(dbTable.Schema & "." & dbTable.Name, row.Item(idColumn), True)
            Else
                'remove the error flag error
                dacc.updateErrorFlag(dbTable.Schema & "." & dbTable.Name, row.Item(idColumn), False)
            End If
            If processType = ValidationType.Transactionprocessing Then
                data_transfer.getRowValidations(db, dbTable.Schema, dbTable.Name, row)
            End If
        Next
        BackgroundWorkerValidate.ReportProgress(Nothing, ercount & " of  " & count & " records in " & dbTable.Schema & "." & dbTable.Name & " have errors")
        Me.clsGlobalVariable.HRS_Temp_DBCon.Close()
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="schemaName"></param>
    ''' <param name="tablename"></param>
    ''' <param name="record"></param>
    ''' <param name="clmName"></param>
    ''' <param name="displayerrormessage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getRowValidations(ByVal schemaName As String, ByVal tablename As String, ByVal record As DataRow,
    Optional ByVal clmName As DataColumn = Nothing, Optional ByVal displayerrormessage As Boolean = False) As Boolean
        'for each column in the row get all the validations for it
        Dim validRecord As Boolean = True
        'Dim validationResult As Integer = 0
        'Dim valres As Integer = 0
        tablename = schemaName.Trim & "." & tablename.Trim
        'Dim rowcolumns As DataColumnCollection
        'If clmName Is Nothing Then
        '    rowcolumns = record.Table.Columns

        'Else

        '    Dim ddt As New DataTable
        '    rowcolumns = ddt.Columns
        '    '  rowcolumns.Clear

        '    rowcolumns.Add(record.Table.Columns(clmName.ColumnName))

        'End If
        For Each clmn As DataColumn In record.Table.Columns
            If Not clmName Is Nothing Then
                clmn = record.Table.Columns(clmName.ColumnName)
            End If
            Dim validationResult As Integer = 0
            Dim valres As Integer = 0
            'Dim str As String = "Select [allowedValues],[skipLogic],[ErrorDescription],[ErrorDescSkipLogic],[functionName] from  tableValidations where " _
            '        & "columnName= '" & clmn.ColumnName & "' AND tableName='" & tablename.Trim & "' AND validationStatus='ACTIVE'"
            'Dim cmd As New SqlCommand(str, conDataCheker)
            'Dim dt As New DataTable()
            'Dim da As New SqlDataAdapter(cmd)
            'dt.Clear()
            'da.Fill(dt)
            'conDataCheker.Close()
            Dim dt As DataTable = dacc.getColumnvalidations(clmn.ColumnName, tablename.Trim)
            'now validate against each validation check
            Dim errors As New List(Of String)

            For Each valrow As DataRow In dt.Rows
                Try
                    valres = validateColumnSkip(valrow, record, clmn, tablename)
                Catch ex As Exception
                    valres = 1
                    'MsgBox(ex.Message & " " & clmName.ColumnName)
                End Try

                If valres = 1 Then 'column has an error so
                    errors.Add(valrow.Item("ErrorDescription").ToString)
                    validationResult = valres
                ElseIf valres = 2 Then ' column is valid so no point of continuing validations
                    validationResult = valres
                    Exit For
                End If
                '
                If valres = 0 Then
                    validationResult = valres
                End If

            Next
            If validationResult = 1 Then
                validRecord = False
            End If

            'check if the row dnt meet any skippattern specified... if it did not then the column being validated
            ' should only have a default value
            If validationResult = 0 Then
                'get defaultvalue first
                'str = "Select [DefaultValue] from  columnDefaultValues where " _
                '                    & "columnName= '" & clmn.ColumnName & "' AND tableName='" & tablename.Trim & "'"
                'cmd.CommandText = str
                'If cmd.Connection.State <> ConnectionState.Open Then cmd.Connection.Open()
                'Dim defV As String = cmd.ExecuteScalar()

                'cmd.Connection.Close()
                'now validate on the default value

                If validateDefaultValue(record, clmn, tablename) = 1 Then
                    validationResult = 1
                    errors.Add("Invalid Default Value - " & clmn.ColumnName)
                    validRecord = False

                End If
            End If
            'record each erro that was found since no record returned true i.e. all possible combinations wa errors
            If validationResult = 1 Then
                Dim s As String = ""
                If Not displayerrormessage Then
                    For Each errorText As String In errors
                        Select Case Me.currentValidationlevel
                            Case datalevel.DSSHRS
                                dacc.saveError("", tablename, errorText, "", Now(), "", errorvillage, errorRound)
                            Case datalevel.TEMP_DSSHRS
                                dacc.saveError(record.Item(idColumn).ToString, tablename, errorText, "", Now(), "", errorvillage, errorRound)
                        End Select
                    Next
                Else
                    Dim allerrors As String = ""
                    For Each errorText As String In errors
                        allerrors = allerrors & errorText & vbNewLine
                    Next
                    combinederrorText = allerrors
                    ' MsgBox(allerrors, MsgBoxStyle.Critical)
                    Return validRecord
                End If


            End If
            If Not clmName Is Nothing Then Exit For
        Next

        Return validRecord

    End Function
    Private Function validateDefaultValue(ByVal record As DataRow, ByVal clmn As DataColumn, ByVal tablename As String) As Integer

        Dim str As String
        str = "Select [DefaultValue] from  columnDefaultValues where " _
                                                  & "columnName= '" & clmn.ColumnName & "' AND tableName='" & tablename.Trim & "'"
        Dim cmd As New SqlCommand(str, Me.dacc.conDataCheker)

        'cmd.CommandText = str
        If cmd.Connection.State <> ConnectionState.Open Then cmd.Connection.Open()
        Dim defV As String = cmd.ExecuteScalar()

        cmd.Connection.Close()
        If Not defV Is Nothing Then


            ' row dosnt meet skip criteria hence the column being validated should only be the default value or null
            'saveError(record.Item(idColumn).ToString, tablename, validations.Item("ErrorDescription").ToString, "", Now(), "")
            Dim value As Object = record.Item(clmn)

            If (defV.Trim = "") Then ' column allows for nulls and empty string as default value
                If Not (value Is Nothing OrElse value.ToString.Trim = "") Then
                    'saveError(record.Item(idColumn).ToString, tablename, "Invalid Default Value - " & clmn.ColumnName, "should be default value (null or empty)", Now(), "")
                    Return 1
                End If
            Else ' column has a default value so compare
                'If Not (value.ToString.Trim.ToUpper = defV.ToUpper) Then
                '    'saveError(record.Item(idColumn).ToString, tablename, "Invalid Default Value - " & clmn.ColumnName, "should be default value", Now(), "")
                '    Return 1
                'End If
                'check if the value is of date datatype
                If clmn.DataType.Name.Trim.ToLower Like "*date*" Then

                    If Not (value.Equals(DBNull.Value)) Then
                        'If value.Equals(DBNull.Value) Then
                        '    MsgBox("whahahahha")
                        'End If
                        If Not CType(value, Date).Date = CType(defV.Trim, Date) Then
                            'saveError(record.Item(idColumn).ToString, tablename, "Invalid Default Value - " & clmn.ColumnName, "should be default value", Now(), "")
                            Return 1
                        End If
                    End If
                Else
                    If Not (value.ToString.Trim.ToUpper = defV.ToUpper) Then
                        'saveError(record.Item(idColumn).ToString, tablename, "Invalid Default Value - " & clmn.ColumnName, "should be default value", Now(), "")
                        Return 1
                    End If
                End If
            End If
        Else
            Return 0
        End If
        Return 2
    End Function
    Private Function hasDefaultValue(ByVal clmn As DataColumn, ByVal tablename As String) As Boolean

        Dim str As String
        str = "Select count(*) from  columnDefaultValues where " _
                                                  & "columnName= '" & clmn.ColumnName & "' AND tableName='" & tablename.Trim & "'"
        Dim cmd As New SqlCommand(str, Me.dacc.conDataCheker)

        'cmd.CommandText = str
        If cmd.Connection.State <> ConnectionState.Open Then cmd.Connection.Open()
        Dim defV As Integer = cmd.ExecuteScalar()
        If defV > 0 Then Return True Else Return False
    End Function
    Private Shared Function splitRange(ByVal value As String) As String()
        'first remove the end ; if any
        value = value.TrimEnd(";"c)
        'next split the values with - as the delimiter
        Dim intarr As String() = value.Split("-"c)
        Return intarr
    End Function
    Private Function validateColumnValue(ByVal validations As DataRow, ByVal record As DataRow, ByVal clmn As DataColumn,
                                                ByVal tablename As String) As Integer

        Dim value As Object = record.Item(clmn)
        'TODO get whether this record has a function that should be run on it
        If validations.Item("functionName").ToString.Trim <> "" Then
            value = Me.dacc.executeFunction(validations.Item("functionName").ToString.Trim, value)
        End If

        'get the allowed values
        Dim valid As Boolean = False
        Dim validvalues As String() = validations.Item("allowedValues").ToString.Split(";"c)

        'test if column allows any/all values if so the just check for empty/null
        If validvalues.Length = 1 AndAlso validvalues(0).Trim = "" Then ' its an empty string denoting all none empty values are valid
            If Not (value Is Nothing OrElse value.ToString.Trim = "") Then
                Return 2
            Else
                'if it dosnt meet the above criteria soooooooooo it means the value isn't valid so we need to record it in our error log
                'saveError(record.Item(idColumn).ToString, tablename, validations.Item("ErrorDescription").ToString, "", Now(), "", errorvillage)
                Return 1
            End If

        End If
        For Each validvalue As String In validvalues
            ' If String.IsNullOrEmpty(validvalue) Then Continue For
            'check if the value is a range
            'ranges # number range; $ date range ; ^ date value

            If validvalue.StartsWith("^") Then 'its a date value
                If CType(value, Date).Date = CType(validvalue.TrimStart("^"c), Date).Date Then Return 2

            ElseIf validvalue.StartsWith("#") Then 'its a number range

                'test if value is a number
                If IsNumeric(value) Then
                    ''split the number range first
                    Dim intarr As String() = splitRange(validvalue.TrimStart("#"c))
                    'test if the value falls within the range
                    If CInt(value) >= CInt(intarr(0)) And CInt(value) <= CInt(intarr(1)) Then Return 2
                End If

            ElseIf validvalue.StartsWith("$") Then 'its a date range
                'test if its a valid date
                If IsDate(value) Then
                    ''split the date range first
                    Dim intarr As String() = splitRange(validvalue.TrimStart("$"c))
                    'check if second date is "currentdate" then act acordingly
                    If intarr(1).Trim.ToLower = "currentdate" Then
                        If CDate(value).Date >= CDate(intarr(0)).Date And CDate(value).Date <= Now.Date Then Return 2
                    Else
                        If CDate(value).Date >= CDate(intarr(0)).Date And CDate(value).Date <= CDate(intarr(1)).Date Then Return 2
                    End If

                End If
            Else ' it is a value that can be a string
                'If value.Equals(DBNull.Value) And ((validvalue.Trim.ToLower = "") Or (validvalue.Trim.ToLower = "null")) Then
                '    If clmn.ColumnName.Trim.ToLower = "vita" Then
                '        MsgBox("fdgdf")
                '    End If
                '    Return 2
                'End If
                If value.ToString.Trim.ToLower = validvalue.Trim.ToLower Then
                    Return 2
                End If

            End If

        Next
        ' If hasDefaultValue(clmn, tablename) Then
        'validateDefaultValue
        ' End If
        Dim i As Integer = validateDefaultValue(record, clmn, tablename)
        If i = 1 Then
            Return 1
        ElseIf i = 2 Then
            Return 2
        End If
        'End If

        If Not valid Then
            'if it dosnt meet the above criteria soooooooooo it means the value isn't valid so we need to record it in our error log
            ' saveError(record.Item(idColumn).ToString, tablename, validations.Item("ErrorDescription").ToString, "", Now(), "", errorvillage)
        End If
        Return 1
    End Function
    Public Function removeDuplicates(ByVal schemaName As String, ByVal tablename As String) As Integer


        Dim dupStr As String = "SELECT DISTINCT "
        Dim tblcolumns As String = ""
        Dim wherClause As String = ""
        Dim existswhereClause As String = ""
        Dim str As String = "select column_name from information_schema.columns where table_Schema='" & schemaName & "' and table_name='" & tablename _
        & "' AND column_name not in('rec_status', 'errflag', 'errdate', 'transit_id','download_date')"
        Dim cmd As New SqlCommand(str, Me.clsGlobalVariable.HRS_Temp_DBCon)
        Dim dt As New DataTable()
        Dim da As New SqlDataAdapter(cmd)
        dt.Clear()
        da.Fill(dt)

        'now validate against each validation check
        Dim ignoreColumns As New List(Of String)
        Dim s As String() = {"rec_status", "errflag", "errdate", "transit_id", "download_date"}

        ignoreColumns.AddRange(s)
        'ignore episodeid also for marriage
        If tablename.ToLower.Trim = "marriage" Then
            ignoreColumns.Add("episodeid")
        End If
        Dim col As String
        For Each tablecol As DataRow In dt.Rows
            col = tablecol.Item("column_name").ToString
            If ignoreColumns.Contains(col.Trim.ToLower) Then Continue For
            tblcolumns = tblcolumns & col & ","
            wherClause = wherClause & col & "= @" & col & " AND "
            existswhereClause = existswhereClause & "TEMP_DSSHRS." & schemaName.Trim & "." & tablename.Trim & "." & col & "=M." & col & " AND "
        Next
        'dupStr = dupStr & tblcolumns & " count(*) totalNumber FROM " & schemaName.Trim & "." & tablename.Trim & " where rec_status not like 'X%' GROUP BY " & tblcolumns.TrimEnd(","c) _
        '& " having count(*)>1"
        dupStr = dupStr & tblcolumns.TrimEnd(","c) & " FROM " & schemaName.Trim & "." & tablename.Trim & " where rec_status not like '%X%' GROUP BY " & tblcolumns.TrimEnd(","c) _
    & " having count(*)>1"

        'dupStr = dupStr & tblcolumns.TrimEnd(","c) & " FROM ( " _
        '& "SELECT DISTINCT " & tblcolumns & " rec_status FROM DSSHRS." & schemaName.Trim & "." & tablename.Trim _
        '& " UNION ALL SELECT DISTINCT " & tblcolumns & " rec_status FROM TEMP_DSSHRS." & schemaName.Trim & "." & tablename.Trim _
        '& ") as myTable where rec_status not like 'X%' GROUP BY " & tblcolumns.TrimEnd(","c) _
        '& " having count(*)>1"

        cmd.CommandText = dupStr

        da.SelectCommand = cmd
        dt.Clear()
        dt.Columns.Clear()
        da.Fill(dt)
        Dim top1 As String = ""

        Dim i As Integer
        ' mark all records that have duplicates in main as deleted in temp
        existswhereClause = existswhereClause.Remove(existswhereClause.Length - 4, 4)
        top1 = "UPDATE " & schemaName.Trim & "." & tablename.Trim & " SET rec_status='X'+ substring(rec_status,1,1) where rec_status not like '%X%' AND " _
                   & "EXISTS (SELECT * FROM DSSHRS." & schemaName.Trim & "." & tablename.Trim & " M where " & existswhereClause.Trim & ")"

        cmd.CommandText = top1
        If cmd.Connection.State <> ConnectionState.Open Then cmd.Connection.Open()
        i = i + cmd.ExecuteNonQuery()

        For Each rec As DataRow In dt.Rows
            'TODO this code works funny hahah from here check it out
            cmd.Parameters.Clear()
            For Each reccolumn As DataColumn In rec.Table.Columns
                cmd.Parameters.AddWithValue("@" & reccolumn.ColumnName, rec.Item(reccolumn.ColumnName))
                'wherClause = wherClause & reccolumn.ColumnName & "='" & rec.Item(reccolumn.ColumnName) & "' AND "
            Next

            'mark all records that are duplicates in temp as deleted
            top1 = "UPDATE " & schemaName.Trim & "." & tablename.Trim & " SET rec_status='X'+ substring(rec_status,1,1) where rec_status not like '%X%' AND " & wherClause & " transit_id not in " _
            & "(SELECT top 1 transit_id  FROM " & schemaName.Trim & "." & tablename.Trim & " where " & wherClause & " rec_status not like '%X%')"

            cmd.CommandText = top1
            If cmd.Connection.State <> ConnectionState.Open Then cmd.Connection.Open()
            i = i + cmd.ExecuteNonQuery()

            'UPDATE DSS.pregoutcome  SET rec_status='X'+ substring(rec_status,1,1) where rec_status 
            'NOT LIKE '%X%' AND EXISTS (SELECT * FROM DSSHRS.DSS.pregoutcome M WHERE M.EventID=EventID AND M.individid=individid ) 
        Next
        Return i
    End Function
    Public Function removeDuplicatesOrcasTEMP(ByVal schemaName As String, ByVal tablename As String) As Integer
        'Dim correctRecordsDuplicates As String = "SELECT max(transit_id) "
        'Dim tblcolumns As String = ""
        'Dim str As String = "select column_name from information_schema.columns where table_Schema='" & schemaName & "' and table_name='" & tablename _
        '& "' AND column_name not in('rec_status', 'errflag', 'errdate', 'transit_id','download_date')"
        Dim cmd As New SqlCommand("[MHRS_SYS].[removeDuplicatesOrcasTEMP]", Me.clsGlobalVariable.HRS_Temp_DBCon)
        cmd.CommandType = CommandType.StoredProcedure

        Dim outputIdParam As SqlParameter = New SqlParameter("@intoutput", SqlDbType.Int)
        outputIdParam.Direction = ParameterDirection.Output
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@schemaName", schemaName)
        cmd.Parameters.AddWithValue("@tablename", tablename)
        cmd.Parameters.Add(outputIdParam)


        'Dim dt As New DataTable()
        'Dim da As New SqlDataAdapter(cmd)
        'dt.Clear()
        'da.Fill(dt)

        ''now validate against each validation check
        'Dim ignoreColumns As New List(Of String)
        'Dim s As String() = {"rec_status", "errflag", "errdate", "transit_id", "download_date"}
        'ignoreColumns.AddRange(s)
        ''ignore episodeid also for marriage
        'If tablename.ToLower.Trim = "marriage" Then
        '    ignoreColumns.Add("episodeid")
        'End If
        'Dim col As String
        'For Each tablecol As DataRow In dt.Rows
        '    col = tablecol.Item("column_name").ToString
        '    If ignoreColumns.Contains(col.Trim.ToLower) Then Continue For
        '    tblcolumns = tblcolumns & "[" & col & "],"
        'Next
        'correctRecordsDuplicates = correctRecordsDuplicates & " FROM " & schemaName.Trim & "." & tablename.Trim & " where rec_status not like '%X%' GROUP BY " & tblcolumns.TrimEnd(","c)
        'Dim updatesql As String = ""
        'updatesql = "UPDATE " & schemaName.Trim & "." & tablename.Trim & " SET rec_status='X'+ substring(rec_status,1,1) where rec_status not like '%X%' AND " _
        '                  & "transit_id not in  (" & correctRecordsDuplicates.Trim & ")"

        'cmd.CommandText = updatesql
        cmd.CommandTimeout = 0
        If cmd.Connection.State <> ConnectionState.Open Then cmd.Connection.Open()
        cmd.ExecuteNonQuery()
        cmd.Connection.Close()
        Dim i As Integer = outputIdParam.Value
        Return i
    End Function

    Public Function removeDuplicatesOrcasBothinTEMPnMain(ByVal schemaName As String, ByVal tablename As String) As Integer
        'Dim duplicatesTransit_ids As String = "SELECT max(transit_id) "
        'Dim tblcolumns As String = ""
        'Dim unionsql As String = ""
        'Dim str As String = "select column_name from information_schema.columns where table_Schema='" & schemaName & "' and table_name='" & tablename _
        '& "' AND column_name not in('rec_status', 'errflag', 'errdate', 'transit_id','download_date')"
        'Dim cmd As New SqlCommand(str, Me.clsGlobalVariable.HRS_Temp_DBCon)

        Dim cmd As New SqlCommand("[MHRS_SYS].[removeDuplicatesOrcasBothinTEMPnMain]", Me.clsGlobalVariable.HRS_Temp_DBCon)
        cmd.CommandType = CommandType.StoredProcedure

        Dim outputIdParam As SqlParameter = New SqlParameter("@intoutput", SqlDbType.Int)
        outputIdParam.Direction = ParameterDirection.Output
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@schemaName", schemaName)
        cmd.Parameters.AddWithValue("@tablename", tablename)
        cmd.Parameters.Add(outputIdParam)

        'Dim dt As New DataTable()
        'Dim da As New SqlDataAdapter(cmd)
        'dt.Clear()
        'da.Fill(dt)

        ''now validate against each validation check
        'Dim ignoreColumns As New List(Of String)
        'Dim s As String() = {"rec_status", "errflag", "errdate", "transit_id", "download_date"}
        'ignoreColumns.AddRange(s)
        ''ignore episodeid also for marriage
        'If tablename.ToLower.Trim = "marriage" Then
        '    ignoreColumns.Add("episodeid")
        'End If
        'Dim col As String
        'For Each tablecol As DataRow In dt.Rows
        '    col = tablecol.Item("column_name").ToString
        '    If ignoreColumns.Contains(col.Trim.ToLower) Then Continue For
        '    tblcolumns = tblcolumns & "[" & col & "],"
        'Next
        'unionsql = "(select " & tblcolumns.TrimEnd(","c) & ",transit_id FROM TEMP_DSSHRS." & schemaName.Trim & "." & tablename.Trim & " where rec_status not like '%X%'" _
        '& " union select " & tblcolumns.TrimEnd(","c) & ",-1 as transit_id FROM DSSHRS." & schemaName.Trim & "." & tablename.Trim & ")"

        'duplicatesTransit_ids = "( " & duplicatesTransit_ids & " FROM " & unionsql & " as allrecords GROUP BY " & tblcolumns.TrimEnd(","c) _
        '& " having  max([transit_id])>0 and COUNT(*)>1 and MIN(transit_id)=-1 )"


        'Dim updatesql As String = ""
        'updatesql = "UPDATE TEMP_DSSHRS." & schemaName.Trim & "." & tablename.Trim & " SET rec_status='X'+ substring(rec_status,1,1) where rec_status not like '%X%' AND " _
        '                  & "transit_id in  (" & duplicatesTransit_ids.Trim & ")"

        'cmd.CommandText = updatesql
        cmd.CommandTimeout = 0
        If cmd.Connection.State <> ConnectionState.Open Then cmd.Connection.Open()
        'Dim i As Integer = i + cmd.ExecuteNonQuery()
        cmd.ExecuteNonQuery()
        cmd.Connection.Close()
        Dim i As Integer = outputIdParam.Value
        Return i
    End Function
    Public Function undoRecordsMarkdedFordeletion(ByVal schemaName As String, ByVal tablename As String) As Integer

        Dim cmd As New SqlCommand
        cmd.Connection = Me.clsGlobalVariable.HRS_Temp_DBCon
        Dim updatesql As String = ""
        updatesql = "UPDATE TEMP_DSSHRS." & schemaName.Trim & "." & tablename.Trim & "  set rec_status =Replace(rec_status,'x','') where rec_status  like '%x%'"
        cmd.CommandText = updatesql
        If cmd.Connection.State <> ConnectionState.Open Then cmd.Connection.Open()
        Dim i As Integer = i + cmd.ExecuteNonQuery()
        Return i
    End Function
    Private Function updateNullEpisodesQuery(ByVal episodetablename As String, ByVal episodeidName As String) As String
        Dim sql As String = "update [TEMP_DSSHRS].[DSS].[" & episodetablename & "] set rec_status='UX'" _
        & " where rec_status in ('u', 'du', 'tu', 'mu') and (edate is null  and eobserveid is null and eeventtype is null)" _
        & " and " + episodeidName + "  in (select a." + episodeidName + " from DSSHRS.DSS." & episodetablename & " as a)"
        Return sql
    End Function
    Private Function updateRectatusEpisodesInsertsQuery(ByVal episodetablename As String, ByVal episodeidName As String) As String
        Dim sql As String = "update [TEMP_DSSHRS].[DSS].[" & episodetablename & "] set rec_status='I'" _
        & " where rec_status in ('u', 'du', 'tu', 'mu') and (edate is null and eobserveid is null and eeventtype is null)" _
        & " and " + episodeidName + "  not in (select a." + episodeidName + " from DSSHRS.DSS." & episodetablename & " as a)"
        Return sql
    End Function
    Private Function updateRectatusEpisodesupdatesQuery(ByVal episodetablename As String, ByVal episodeidName As String) As String
        Dim sql As String = "update [TEMP_DSSHRS].[DSS].[" & episodetablename & "] set rec_status='U'" _
        & " where rec_status in ('i', 'di', 'ti', 'mi') and (edate is not null and eobserveid is not null and eeventtype is not null)"
        Return sql
    End Function
    Private Sub runMaintenanceScripts()
        'Episodes with wrong rec_status

        Me.da.exec_nonquery(Me.updateNullEpisodesQuery("residency", "ResidencyID"))
        Me.da.exec_nonquery(Me.updateNullEpisodesQuery("membership", "memberShipID"))
        Me.da.exec_nonquery(Me.updateNullEpisodesQuery("pregnancy", "PregnancyID"))

        Me.da.exec_nonquery(Me.updateRectatusEpisodesupdatesQuery("residency", "ResidencyID"))
        Me.da.exec_nonquery(Me.updateRectatusEpisodesupdatesQuery("membership", "memberShipID"))
        Me.da.exec_nonquery(Me.updateRectatusEpisodesupdatesQuery("pregnancy", "PregnancyID"))

        Me.da.exec_nonquery(Me.updateRectatusEpisodesInsertsQuery("residency", "ResidencyID"))
        Me.da.exec_nonquery(Me.updateRectatusEpisodesInsertsQuery("membership", "memberShipID"))
        Me.da.exec_nonquery(Me.updateRectatusEpisodesInsertsQuery("pregnancy", "PregnancyID"))


        Me.da.exec_nonquery("exec [TEMP_DSSHRS].[dbo].[generateMissingObservations] ")
        Me.da.exec_nonquery("exec [TEMP_DSSHRS].[dbo].[generateMissingObservationsMissinfObser] ")
        Me.da.exec_nonquery("exec [TEMP_DSSHRS].[dbo].[RemoveFakePrpgramGenObservations] ")
        Me.da.exec_nonquery("exec [TEMP_DSSHRS].[dbo].[generateMissingObservDemolish] ")
        Me.da.exec_nonquery("exec [TEMP_DSSHRS].[dbo].[deleteMovedAwayImmunize] ")

        Me.da.exec_nonquery("exec [TEMP_DSSHRS].[DSS].[Transfer_Updates_Membership_Inserts]")
        Me.da.exec_nonquery("exec [TEMP_DSSHRS].[DSS].[Transfer_Updates_Residency_Inserts]")
        Me.da.exec_nonquery("exec [TEMP_DSSHRS].[DSS].[Transfer_Updates_Pregnancy_Inserts]")

        Me.da.exec_nonquery("exec [TEMP_DSSHRS].[DSS].[Transfer_Updates_Membership]")
        Me.da.exec_nonquery("exec [TEMP_DSSHRS].[DSS].[Transfer_Updates_Residency]")
        Me.da.exec_nonquery("exec [TEMP_DSSHRS].[DSS].[Transfer_Updates_Pregnancy]")

        'parental survival
        Me.da.exec_nonquery("exec [TEMP_DSSHRS].[DSS].[Transfer_get_parantsurv_changes]")
        Me.da.exec_nonquery("exec [TEMP_DSSHRS].[DSS].[Transfer_Updates_parantsurv_psid]")

        'Education
        Me.da.exec_nonquery("exec [TEMP_DSSHRS].[DSS].[Transfer_Updates_EducationTempDB_xid]")


        Me.da.exec_nonquery("  update [TEMP_DSSHRS].[DSS].membership set eobserveid=null where  eobserveid ='' and rec_status in ('I', 'MI','TI')")

        Me.da.exec_nonquery("  update [TEMP_DSSHRS].[DSS].residency set eobserveid=null where  eobserveid ='' and rec_status in ('I', 'MI','TI')")

        Me.da.exec_nonquery("  update [TEMP_DSSHRS].[DSS].pregnancy set eobserveid=null where  eobserveid ='' and rec_status in ('I', 'MI','TI')")

        Me.da.exec_nonquery("truncate table temp_dsshrs.MHRS_SYS.Temp_Data_Errors")


        Me.da.exec_nonqueryInTEMPDB("UPDATE [MHRS_SYS].[Changes] SET [errflag] = 'true', errdate=getdate()")
    End Sub
    Private Sub refresherrortable()
        Me.da.exec_nonquery("exec [TEMP_DSSHRS].[dbo].[error_removenolongergenerated] ")
        Me.da.exec_nonquery("exec [TEMP_DSSHRS].[dbo].[error_RestoreGenuineErrors] ")
        Me.da.exec_nonquery("exec [TEMP_DSSHRS].[dbo].[error_InsertNewErrors]")
        Me.da.exec_nonquery("exec [TEMP_DSSHRS].[dbo].[error_removeduplicatederrors] ")

        Me.da.exec_nonquery("exec [TEMP_DSSHRS].[dbo].[error_updateCompound] ")
        Me.da.exec_nonquery("exec [TEMP_DSSHRS].[dbo].[error_updateround]")
        Me.da.exec_nonquery("exec [TEMP_DSSHRS].[dbo].[error_updateVillage] ")
    End Sub
    Private Sub doTransformData()
        Me.SendMail("KIBERA Validation program has started Running at " & Now().ToString & vbLf & vbLf & vbLf & " Courtesy of Validation Program" _
        , "***NOTICE KIBERA VALIDATION PROGRAM PROGRESS***", "cooduor@kemricdc.org;skiplangat@kemricdc.org;gmasyongo@kemricdc.org;aouma@kemricdc.org;pogada@kemricdc.org;nagumba@kemricdc.org;")
        ', "***NOTICE KIBERA VALIDATION PROGRAM PROGRESS***", "d_KisumuDSSProg@kemricdc.org;nairobi-gddd-data@kemricdc.org")
        Dim dt As DataTable
        Dim item As Table = Nothing
        Dim data_transfer As clsDataTransfer = clsDataTransfer.getObject
        Try
            Me.currentValidationlevel = datalevel.TEMP_DSSHRS
            Control.CheckForIllegalCrossThreadCalls = False
            frmDataTransfer.initialiseGlobalVariables()

            data_transfer.worker = BackgroundWorkerValidate
            'disable triggers
            data_transfer.configureTrigger(datalevel.DSSHRS, False)
            data_transfer.configureTrigger(datalevel.TEMP_DSSHRS, False)

            'scripts to update the database
            BackgroundWorkerValidate.ReportProgress(Nothing, "Updating Episodes with wrong rec_status " & " " & Now.ToString())
            Me.runMaintenanceScripts()
            BackgroundWorkerValidate.ReportProgress(Nothing, "Finished Episodes with wrong rec_status " & " " & Now.ToString())

            BackgroundWorkerValidate.ReportProgress(Nothing, "Process Started...validating selected tables " & Now.ToString())


            Dim c As Integer

            dt = dacc.getTablesToValidate()
            For Each obj As DataRow In dt.Rows
                item = New Table
                item.Schema = obj("TABLE_SCHEMA").ToString
                item.Name = obj("TABLE_NAME").ToString
                Try
                    BackgroundWorkerValidate.ReportProgress(Nothing, "Process Started...validating " & item.Schema & "." & item.Name & " " & Now.ToString())
                    'todo work on this code when you come back
                    BackgroundWorkerValidate.ReportProgress(Nothing, "Removing duplicates in  " & item.Schema & "." & item.Name & " " & Now.ToString())
                    ' c = removeDuplicates(item.Schema, item.Name)
                    c = removeDuplicatesOrcasTEMP(item.Schema, item.Name)
                    c = c + removeDuplicatesOrcasBothinTEMPnMain(item.Schema, item.Name)

                    BackgroundWorkerValidate.ReportProgress(Nothing, "Finished " & c & " duplicates removed in  " & item.Schema & "." & item.Name & " " & Now.ToString())
                    validateTable(Me.currentValidationlevel, item, data_transfer, ValidationType.BatchProcessing)
                    BackgroundWorkerValidate.ReportProgress(Nothing, "finished validating " & item.Schema & "." & item.Name & " " & Now.ToString())
                    'BackgroundWorkerValidate.ReportProgress(Nothing, "Updating DataErrors table " & item.Schema & "." & item.Name & " " & Now.ToString())
                    'updateDataErrors(item)
                    'BackgroundWorkerValidate.ReportProgress(Nothing, "Finished Updating DataErrors for table  " & item.Schema & "." & item.Name & " " & Now.ToString())
                Catch ex As Exception
                    BackgroundWorkerValidate.ReportProgress(Nothing, "Error on Table: " & ex.Message & " " & Now.ToString())
                    '   MsgBox(ex.StackTrace)
                End Try
            Next

            'upload data
            BackgroundWorkerValidate.ReportProgress(Nothing, "Process Started... validate and upload clean data" & " " & Now.ToString())

            Try

                data_transfer.uploadDatatoMainDb()
            Catch ex As Exception
                BackgroundWorkerValidate.ReportProgress(Nothing, "Error on Table: " & ex.Message & " " & Now.ToString())
            End Try
            '
            For Each obj As DataRow In dt.Rows
                Try
                    item = New Table
                    item.Schema = obj("TABLE_SCHEMA").ToString.Trim
                    item.Name = obj("TABLE_NAME").ToString.Trim
                    BackgroundWorkerValidate.ReportProgress(Nothing, "Updating DataErrors table " & item.Schema.Trim & "." & item.Name.Trim & " " & Now.ToString())
                    dacc.updateDataErrors(item)
                    dacc.updateDataErrors2(item)
                    BackgroundWorkerValidate.ReportProgress(Nothing, "Finished Updating DataErrors for table  " & item.Schema.Trim & "." & item.Name & " " & Now.ToString())
                Catch ex As Exception
                    BackgroundWorkerValidate.ReportProgress(Nothing, "Error on Table: " & ex.Message & " " & Now.ToString())
                End Try
            Next
        Catch ex As Exception
            'MsgBox(ex.Message, MsgBoxStyle.Critical)
            BackgroundWorkerValidate.ReportProgress(Nothing, "Error: " & ex.Message & " " & Now.ToString())
        End Try
        Try
            Me.refresherrortable()
            'disable triggers
            'data_transfer.configureTrigger(datalevel.DSSHRS, True)
            'data_transfer.configureTrigger(datalevel.TEMP_DSSHRS, True)
            For Each obj As DataRow In dt.Rows
                Try
                    item = New Table
                    item.Schema = obj("TABLE_SCHEMA").ToString
                    item.Name = obj("TABLE_NAME").ToString
                    BackgroundWorkerValidate.ReportProgress(Nothing, "Updating DataErrors table " & item.Schema & "." & item.Name & " " & Now.ToString())
                    dacc.refreshDataErrors(item)
                    BackgroundWorkerValidate.ReportProgress(Nothing, "Finished Updating DataErrors for table  " & item.Schema & "." & item.Name & " " & Now.ToString())
                Catch ex As Exception
                    BackgroundWorkerValidate.ReportProgress(Nothing, "Error on Table: " & ex.Message & " " & Now.ToString())
                End Try
            Next
            data_transfer.configureTrigger(datalevel.DSSHRS, True)
            data_transfer.configureTrigger(datalevel.TEMP_DSSHRS, True)
            BackgroundWorkerValidate.ReportProgress(Nothing, "program completed successfully")
            Me.SendMail("KIBERA Validation program completed successfully at " & Now().ToString & vbLf & vbLf & vbLf & " Courtesy of Validation Program" _
        , "***NOTICE KIBERA VALIDATION PROGRAM PROGRESS***", "d_KisumuDSSProg@kemrnickolicdc.org;nairobi-gddd-data@kemricdc.org")
            'MsgBox("program completed", MsgBoxStyle.Exclamation)
        Catch ex As Exception

        End Try
    End Sub
    Private Sub auturunProgram()

        If dacc.getrunSetup().Trim.ToLower = "on" Then
            autorun = True
            errorlogstream = System.IO.File.CreateText(Me.dacc.errorLogfilename)
            System.IO.File.SetAttributes(Me.dacc.errorLogfilename, IO.FileAttributes.Hidden)

            Me.LogValidationprogress("Start :" + Date.Now.ToString, Me.dacc.errorLogfilename)
            BackgroundWorkerValidate.RunWorkerAsync()
            PBar.Style = ProgressBarStyle.Marquee
            PBarlabel.Text = "Busy"
        Else

            'MsgBox("autorun failed")
        End If

    End Sub
    Private Sub LogValidationprogress(ByVal line As String, ByVal filename As String)
        Try
            'Dim fwriter As System.IO.StreamWriter
            'fwriter = System.IO.File.CreateText(filename)
            'System.IO.File.SetAttributes(filename, IO.FileAttributes.Hidden)
            'fwriter.Write(" " + vbCrLf + line + ":" + Date.Now.ToString)
            'fwriter.Flush()
            'fwriter.Close()
            errorlogstream.Write(" " + vbCrLf + line + ":" + Date.Now.ToString)
            errorlogstream.Flush()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
#End Region

#Region "events"
    Private currentValidationlevel As datalevel = datalevel.TEMP_DSSHRS
    Private Sub btn_Go_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Go.Click
        'If lstbox_tables.SelectedItems.Count = 0 And Not ckb_upload.Checked Then
        If lstbox_tables.SelectedItems.Count = 0 And Not ckb_upload.Checked Then

            MsgBox("You need to select at least 1 table", MsgBoxStyle.Information)
            Return
        End If
        autorun = False 'ckb_upload.Checked
        autoclose = False
        errorTypes = ""
        dgr_results.DataSource = Nothing
        bndNvgerrors.BindingSource = Nothing
        If cmb_selectOption.Text = "Validate tempdb" Then
            Me.currentValidationlevel = datalevel.TEMP_DSSHRS
            If Me.objUtil.isAuthorizedUser() Then
                If MsgBox("Are you sure you want to validate the selected forms?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    BackgroundWorkerValidate.RunWorkerAsync()
                    PBar.Style = ProgressBarStyle.Marquee
                    PBarlabel.Text = "Busy"
                End If
            Else
                MsgBox("You can't run validations! See system Admin.")
            End If
        ElseIf cmb_selectOption.Text = "Validate maindb" Then
            Me.currentValidationlevel = datalevel.DSSHRS
            If Me.objUtil.isAuthorizedUser() Then
                If MsgBox("Are you sure you want to validate the selected forms?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    BackgroundWorkerValidate.RunWorkerAsync()
                    PBar.Style = ProgressBarStyle.Marquee
                    PBarlabel.Text = "Busy"
                End If
            Else
                MsgBox("You can't run validations! See system Admin.")
            End If
        ElseIf cmb_selectOption.Text = "View Errors" Then
            Dim s As String = ""
            Dim selectedTables As String = ""

            For Each item As Table In lstbox_tables.SelectedItems
                selectedTables = selectedTables & "'" & item.Schema & "." & item.Name & "',"
                If item.Schema.ToUpper = "MHRS_SYS" And item.Name.ToUpper = "CHANGES" Then
                    selectedTables = selectedTables & "'[" & item.Schema & "].[" & item.Name & "]',"
                End If
            Next
            selectedTables = selectedTables.TrimEnd(","c)

            If rbtn_allDetails.Checked Then
                s = "SELECT * FROM mhrs_sys.data_errors where " & filterCriteria() & " tablename in(" & selectedTables & ")"
                errorTypes = "ALLDETAILS"
            ElseIf rbtn_errorCounterrtype.Checked Then
                s = "SELECT tablename,Errortype,count(*) [error count] FROM mhrs_sys.data_errors where " & filterCriteria() & " tablename in(" & selectedTables & ")" _
                             & "   group by tablename,Errortype"
                errorTypes = "ERRORTYPE"
            ElseIf rbtn_errorCountperRecord.Checked Then
                s = "SELECT tablename,Recordid,count(*) [error count] FROM mhrs_sys.data_errors where " & filterCriteria() & " tablename in(" & selectedTables & ")" _
                            & "  group by tablename,Recordid"
                errorTypes = "ALLDETAILS"
            ElseIf rbtn_errorCountPerTble.Checked Then
                s = "SELECT tablename, count(*) [error count] FROM mhrs_sys.data_errors where " & filterCriteria() & " tablename in (" & selectedTables & ")" _
                               & "  group by tablename"
                errorTypes = "TABLEERRORS"

            ElseIf rbtn_perVillage.Checked Then
                s = "SELECT village, count(*) [error count] FROM mhrs_sys.data_errors where " & filterCriteria() & " tablename in (" & selectedTables & ")" _
                               & "  group by village"
                errorTypes = "VILLAGEERRORS"
            ElseIf rbtn_pervillperTable.Checked Then
                s = "SELECT village, tablename, count(*) [error count] FROM mhrs_sys.data_errors where " & filterCriteria() & " tablename in (" & selectedTables & ")" _
                               & " group by village,tablename"
                errorTypes = "TABLEVILLAGEERRORS"

            ElseIf rbtn_perCompound.Checked Then
                s = "SELECT compound, count(*) [error count] FROM mhrs_sys.data_errors where " & filterCriteria() & " tablename in (" & selectedTables & ")" _
                               & "  group by compound"
                errorTypes = "COMPOUNDERRORS"

            End If


            getErrors(s)

        ElseIf cmb_selectOption.Text = "SyncErrorTab" Then

            '        Dim n As Integer = 0
            '        For Each item As Table In lstbox_tables.SelectedItems

            '            Dim s As String = "update  MHRS_SYS.Data_Errors set rec_status='C', [dataClerk]='Prg' " _
            '& " where rec_status<>'C'and recordid in(select recordid from " & item.Schema & "." & item.Name _
            '& " t join MHRS_SYS.Data_Errors on  transit_id=Recordid " _
            '& " where tablename =" & "'" & item.Schema & "." & item.Name & "' and  (errflag=0 or t.rec_status like '%X%')) "
            '            n = n + ObjDbAccess.dbExecute(s)

            '            s = "update  MHRS_SYS.Data_Errors set rec_status='C', [dataClerk]='Prg'  " _
            '            & " where rec_status<>'C'and recordid in(select recordid from   MHRS_SYS.Data_Errors where tablename =" & "'" & item.Schema & "." & item.Name & "' and not exists (" _
            '           & " select * from " & item.Schema & "." & item.Name & " where transit_id=recordid))"
            '            n = n + ObjDbAccess.dbExecute(s)

            '        Next
            '        MsgBox(n & " rows marked as cleaned ")

        ElseIf cmb_selectOption.Text = "UpdateCompoundField" Then
            If Me.objUtil.isAuthorizedUser() Then
                Dim s As String
                '  Dim n As Integer = 0

                s = "SELECT recordid,tablename FROM mhrs_sys.data_errors  where compound='' or compound is null group by tablename,recordid"
                Dim cmd As New SqlCommand(s, Me.clsGlobalVariable.HRS_Temp_DBCon)
                Dim dt As New DataTable()
                Dim da As New SqlDataAdapter(cmd)
                dt.Clear()
                da.Fill(dt)

                For Each dr As DataRow In dt.Rows

                    s = "update  MHRS_SYS.Data_Errors set compound='" & dacc.getDataErrorsrecordsCompound(dr.Item("tablename").ToString, dr.Item("recordid").ToString) & "'  " _
                    & " where recordid ='" & dr.Item("recordid").ToString & "' and tablename='" & dr.Item("tablename").ToString & "'"
                    ObjDbAccess.dbExecute(s)
                Next
                MsgBox(" finished ")
            Else
                MsgBox("You can't UpdateCompoundField! See system Admin.")
            End If


        ElseIf cmb_selectOption.Text = "remove duplicates" Then
            If Me.objUtil.isAuthorizedUser() Then
                For Each item As Table In lstbox_tables.SelectedItems
                    'MsgBox(removeDuplicates(item.Schema, item.Name) & " duplicates removed in table " & item.Schema & "." & item.Name)
                    MsgBox(removeDuplicatesOrcasTEMP(item.Schema, item.Name) & " duplicates removed in table " & item.Schema & "." & item.Name)
                    MsgBox(removeDuplicatesOrcasBothinTEMPnMain(item.Schema, item.Name) & " main and temp duplicates removed in table " & item.Schema & "." & item.Name)
                Next
            Else
                MsgBox("You can't remove duplicates! See system Admin.")
            End If
        ElseIf cmb_selectOption.Text = "undo all records marked for deletion" Then
            If Me.objUtil.isAuthorizedUser() Then
                For Each item As Table In lstbox_tables.SelectedItems
                    MsgBox(Me.undoRecordsMarkdedFordeletion(item.Schema, item.Name) & " records restored in table " & item.Schema & "." & item.Name)
                Next
            Else
                MsgBox("You can't undo deletions See system Admin.")
            End If
        End If



    End Sub



    Private Sub BackgroundWorkerValidate_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorkerValidate.DoWork
        If autorun Then
            doTransformData()
        Else
            Try
                errorlogstream = System.IO.File.CreateText(Me.dacc.errorLogfilename)
                System.IO.File.SetAttributes(Me.dacc.errorLogfilename, IO.FileAttributes.Hidden)

                Control.CheckForIllegalCrossThreadCalls = False
                grpbox_tables.Enabled = False
                tb_log.Text = ""
                ' Cursor.Current = Cursors.WaitCursor
                BackgroundWorkerValidate.ReportProgress(Nothing, "Process Started...validating selected tables " & Now.ToString())
                frmDataTransfer.initialiseGlobalVariables()
                Dim data_transfer As clsDataTransfer = clsDataTransfer.getObject
                data_transfer.worker = BackgroundWorkerValidate

                'disable triggers
                ' data_transfer.configureTrigger(datalevel.DSSHRS, False)
                ' data_transfer.configureTrigger(datalevel.TEMP_DSSHRS, False)
                Dim c As Integer
                For Each item As Table In lstbox_tables.SelectedItems

                    Try
                        BackgroundWorkerValidate.ReportProgress(Nothing, "Process Started...validating " & item.Schema & "." & item.Name & " " & Now.ToString())
                        Select Case Me.currentValidationlevel
                            Case datalevel.DSSHRS
                                Me.validateMainDB(item)
                            Case datalevel.TEMP_DSSHRS
                                'todo work on this code when you come back
                                BackgroundWorkerValidate.ReportProgress(Nothing, "Removing duplicates in  " & item.Schema & "." & item.Name & " " & Now.ToString())
                                c = removeDuplicatesOrcasTEMP(item.Schema, item.Name)
                                c = c + removeDuplicatesOrcasBothinTEMPnMain(item.Schema, item.Name)
                                Me.validateTable(Me.currentValidationlevel, item, data_transfer, ValidationType.Transactionprocessing)
                                BackgroundWorkerValidate.ReportProgress(Nothing, "Finished " & c & " duplicates removed in  " & item.Schema & "." & item.Name & " " & Now.ToString())
                        End Select
                        BackgroundWorkerValidate.ReportProgress(Nothing, "finished validating " & item.Schema & "." & item.Name & " " & Now.ToString())
                    Catch ex As Exception
                        BackgroundWorkerValidate.ReportProgress(Nothing, "Error on Table: " & ex.Message & " " & Now.ToString())
                    End Try

                Next
                If Me.currentValidationlevel = datalevel.TEMP_DSSHRS Then
                    If ckb_upload.Checked Then

                    Else
                        data_transfer.configureTrigger(datalevel.DSSHRS, True)
                        data_transfer.configureTrigger(datalevel.TEMP_DSSHRS, True)
                    End If


                    For Each item As Table In lstbox_tables.SelectedItems
                        BackgroundWorkerValidate.ReportProgress(Nothing, "Updating DataErrors table " & item.Schema & "." & item.Name & " " & Now.ToString())
                        dacc.updateDataErrors(item)
                        dacc.updateDataErrors2(item)
                        BackgroundWorkerValidate.ReportProgress(Nothing, "Finished Updating DataErrors for table  " & item.Schema & "." & item.Name & " " & Now.ToString())
                    Next
                End If

            Catch ex As Exception
                'MsgBox(ex.Message, MsgBoxStyle.Critical)
                BackgroundWorkerValidate.ReportProgress(Nothing, "Error: " & ex.Message & " " & Now.ToString())
            End Try
        End If

    End Sub

    Private Sub BackgroundWorkerValidate_ProgressChanged(ByVal sender As System.Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorkerValidate.ProgressChanged
        LabelProgress.Text = e.UserState
        If e.ProgressPercentage <> 1 Then
            tb_log.Text = tb_log.Text & e.UserState & vbCrLf
            Me.LogValidationprogress(e.UserState, Me.dacc.errorLogfilename)
            tb_log.ScrollToCaret()
        End If
    End Sub

    Private Sub BackgroundWorkerValidate_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorkerValidate.RunWorkerCompleted
        PBar.Style = ProgressBarStyle.Blocks
        PBarlabel.Text = "Ready"
        LabelProgress.Text = ""
        grpbox_tables.Enabled = True
        'Cursor.Current = Cursors.Default
        '  MsgBox("completed")
        'If autoclose = True Then
        '    Application.Exit()
        'End If
        If autorun = True Then
            Me.LogValidationprogress("End :" + Date.Now.ToString, Me.dacc.errorLogfilename)
            errorlogstream.Close()
            Application.Exit()
        End If
    End Sub





    Private Sub frm_Validations_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.mailpass = Me.getPass
        dacc.initializeServerAndDB()
        getTables()
        'Dim fm As New DgvFilterManager

        'fm.DataGridView = dgr_results

        cmb_errorView.Text = "PENDING"
        Me.dacc.errorLogfilename = "autorunLog_" + Guid.NewGuid.ToString
        Me.auturunProgram()
    End Sub
    Private Sub dgr_results_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgr_results.CellContentClick

    End Sub

    Private Sub dgr_results_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgr_results.SelectionChanged
        If dgr_results.SelectedRows.Count <> 0 Then
            dgr_results.ContextMenuStrip = cms_ShowRecord
        Else
            dgr_results.ContextMenuStrip = Nothing
        End If
    End Sub

    Private Sub ShowRecordsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowRecordsToolStripMenuItem.Click
        Dim drv As DataRowView = dgr_results.SelectedRows.Item(0).DataBoundItem
        Dim dr As DataRow = drv.Row

        If Not dr.Table.Columns.Contains("tablename") Then Return
        Dim tablename As String = dr.Item("tablename").ToString

        Dim errortype As String = ""

        Dim sqlquerry As String = ""
        Dim village As String = ""

        Select Case errorTypes
            Case "ALLDETAILS"
                Dim recordid As String = dr.Item("Recordid").ToString
                sqlquerry = "Select * from " & tablename & "  where transit_id =" & recordid & " and rec_status not like '%X%'"

            Case "ERRORTYPE"
                errortype = dr.Item("Errortype").ToString
                sqlquerry = " Select * from " & tablename & " tb  where  rec_status not like '%X%' and exists (Select * from [MHRS_SYS].[Data_Errors] er " _
                & " where Errortype='" & errortype & "' AND er.recordid=tb.transit_id AND tablename='" & tablename & "' )"
            Case "TABLEERRORS"
                sqlquerry = "Select * from " & tablename & " tb where  rec_status not like '%X%' and exists(Select * from [MHRS_SYS].[Data_Errors] er " _
                & " where er.recordid=tb.transit_id AND tablename='" & tablename & "')"

            Case "TABLEVILLAGEERRORS"
                village = dr.Item("village").ToString
                sqlquerry = "Select * from " & tablename & " tb where rec_status not like '%X%' and  exists(Select * from [MHRS_SYS].[Data_Errors] er " _
                & " where er.recordid=tb.transit_id AND village='" & village & "')"


            Case Else

        End Select
        Dim cmd As New SqlCommand(sqlquerry, Me.clsGlobalVariable.HRS_Temp_DBCon)
        Dim dt As New DataTable()
        Dim da As New SqlDataAdapter(cmd)
        dt.Clear()
        da.Fill(dt)
        Dim bs As New BindingSource(dt, "")
        If village <> "" Then village = "Village " & village
        If errortype <> "" Then errortype = "ErrorType " & errortype
        objUtil.setDataView(bs, tablename, village & " " & errortype, True)
        'HRS_Desktop.pnl_Docking.Controls.Add(frm_errorRecord)
        '  HRS_Desktop.pnl_Docking.Height = HRS_Desktop.pnl_Docking.MaximumSize.Height
        '  frm_errorRecord.Show()
        '  frm_errorRecord.WindowState = FormWindowState.Maximized
    End Sub



    Private Sub CleanedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CleanedToolStripMenuItem.Click

        Dim drv As DataRowView = dgr_results.SelectedRows.Item(0).DataBoundItem
        Dim dr As DataRow = drv.Row
        If dr.Table.Columns.Contains("ErrorID") Then
            Dim errorid As String = dr.Item("ErrorID").ToString
            Dim sqlquerry As String = "UPDATE [MHRS_SYS].[Data_Errors]  SET [rec_status] = 'C',dataClerk='" & objRef.ObjSingleton.userName & "'" _
                                       & " WHERE ErrorID =" & errorid
            Dim cmd As New SqlCommand(sqlquerry, Me.clsGlobalVariable.HRS_Temp_DBCon)
            If Me.clsGlobalVariable.HRS_Temp_DBCon.State <> ConnectionState.Open Then Me.clsGlobalVariable.HRS_Temp_DBCon.Open()
            Dim i As Integer = cmd.ExecuteNonQuery()
            'dgr_results.SelectedRows.Item(0).

            MsgBox("Marked as cleaned, reload data to view changes", MsgBoxStyle.Information)
        Else
            MsgBox("Could not update selected row, Ensure you select show all details in error options", MsgBoxStyle.Exclamation)

        End If
    End Sub

    Private Sub btn_selectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_selectAll.Click
        For num As Integer = 0 To lstbox_tables.Items.Count - 1
            lstbox_tables.SetSelected(num, True)
        Next
    End Sub

    Private Sub btn_deSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_deSelectAll.Click
        For num As Integer = 0 To lstbox_tables.Items.Count - 1
            lstbox_tables.SetSelected(num, False)
        Next
    End Sub



    Private Sub PendingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PendingToolStripMenuItem.Click
        Dim drv As DataRowView = dgr_results.SelectedRows.Item(0).DataBoundItem
        Dim dr As DataRow = drv.Row
        If dr.Table.Columns.Contains("ErrorID") Then
            Dim errorid As String = dr.Item("ErrorID").ToString
            Dim sqlquerry As String = "UPDATE [MHRS_SYS].[Data_Errors]  SET [rec_status] = 'P',dataClerk='" & objRef.ObjSingleton.userName & "'" _
                                       & " WHERE ErrorID =" & errorid
            Dim cmd As New SqlCommand(sqlquerry, Me.clsGlobalVariable.HRS_Temp_DBCon)
            If Me.clsGlobalVariable.HRS_Temp_DBCon.State <> ConnectionState.Open Then Me.clsGlobalVariable.HRS_Temp_DBCon.Open()
            Dim i As Integer = cmd.ExecuteNonQuery()
            'dgr_results.SelectedRows.Item(0).

            MsgBox("Marked as Pending, reload data to view changes", MsgBoxStyle.Information)
        Else
            MsgBox("Could not update selected row, Ensure you select show all details in error options", MsgBoxStyle.Exclamation)

        End If
    End Sub

    Private Sub ProgramFalseAlarmToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProgramFalseAlarmToolStripMenuItem.Click
        Dim drv As DataRowView = dgr_results.SelectedRows.Item(0).DataBoundItem
        Dim dr As DataRow = drv.Row
        If dr.Table.Columns.Contains("ErrorID") Then
            Dim errorid As String = dr.Item("ErrorID").ToString
            Dim sqlquerry As String = "UPDATE [MHRS_SYS].[Data_Errors]  SET [rec_status] = 'FA',dataClerk='" & objRef.ObjSingleton.userName & "'" _
                                       & " WHERE ErrorID =" & errorid
            Dim cmd As New SqlCommand(sqlquerry, Me.clsGlobalVariable.HRS_Temp_DBCon)
            If Me.clsGlobalVariable.HRS_Temp_DBCon.State <> ConnectionState.Open Then Me.clsGlobalVariable.HRS_Temp_DBCon.Open()
            Dim i As Integer = cmd.ExecuteNonQuery()
            'dgr_results.SelectedRows.Item(0).

            MsgBox("Marked as Programming/False Alarm, reload data to view changes", MsgBoxStyle.Information)
        Else
            MsgBox("Could not update selected row, Ensure you select show all details in error options", MsgBoxStyle.Exclamation)

        End If
    End Sub

    Private Sub CommentToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CommentToolStripMenuItem.Click
        Dim s As String = InputBox("Please write a brief comment about the error record selected ")
        If Not s.Trim = "" Then
            Dim drv As DataRowView = dgr_results.SelectedRows.Item(0).DataBoundItem
            Dim dr As DataRow = drv.Row
            If dr.Table.Columns.Contains("ErrorID") Then
                Dim errorid As String = dr.Item("ErrorID").ToString
                Dim sqlquerry As String = "UPDATE [MHRS_SYS].[Data_Errors]  SET [Comments] = '" & s & "' WHERE ErrorID =" & errorid
                Dim cmd As New SqlCommand(sqlquerry, Me.clsGlobalVariable.HRS_Temp_DBCon)
                If Me.clsGlobalVariable.HRS_Temp_DBCon.State <> ConnectionState.Open Then Me.clsGlobalVariable.HRS_Temp_DBCon.Open()
                Dim i As Integer = cmd.ExecuteNonQuery()
                'dgr_results.SelectedRows.Item(0).

                MsgBox("Comment posted, reload data to view changes", MsgBoxStyle.Information)
            Else
                MsgBox("Could not update selected row, Ensure you select show all details in error options", MsgBoxStyle.Exclamation)

            End If
        End If
    End Sub



    Private Sub InsertQuerryRefToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InsertQuerryRefToolStripMenuItem.Click
        Dim s As String = InputBox("Please Input the querryref ID ")
        If Not s.Trim = "" Then
            Dim drv As DataRowView = dgr_results.SelectedRows.Item(0).DataBoundItem
            Dim dr As DataRow = drv.Row
            If dr.Table.Columns.Contains("ErrorID") Then
                Dim errorid As String = dr.Item("ErrorID").ToString
                Dim sqlquerry As String = "UPDATE [MHRS_SYS].[Data_Errors]  SET [QueryRefID] = '" & s & "' WHERE ErrorID =" & errorid
                Dim cmd As New SqlCommand(sqlquerry, Me.clsGlobalVariable.HRS_Temp_DBCon)
                If Me.clsGlobalVariable.HRS_Temp_DBCon.State <> ConnectionState.Open Then Me.clsGlobalVariable.HRS_Temp_DBCon.Open()
                Dim i As Integer = cmd.ExecuteNonQuery()
                'dgr_results.SelectedRows.Item(0).

                MsgBox("QueryRefID posted, reload data to view changes", MsgBoxStyle.Information)
            Else
                MsgBox("Could not update selected row, Ensure you select show all details in error options", MsgBoxStyle.Exclamation)

            End If
        End If
    End Sub
#End Region

#Region "email system"
    Private mailpass As String = ""
    Public Function getPass() As String
        Dim pass As String = ""

        Try
            If System.IO.File.Exists(Application.StartupPath & "\pass") Then
                Dim freader As System.IO.StreamReader
                freader = System.IO.File.OpenText(Application.StartupPath & "\pass")
                pass = freader.ReadLine()
                freader.Close()
            Else
                Return ""
            End If
        Catch ex As Exception
            Return ""
        End Try
        Return pass
    End Function
    Public Sub SendMail(ByVal strbody As String, ByVal strSubject As String, ByVal strRecipient As String)

        Dim str As String
        str = "dbo.sendMail @mail_subject,@mail_bodystr ,@mail_recipient"
        Dim cmd As New SqlCommand(str, Me.clsGlobalVariable.HRS_Temp_DBCon)
        cmd.Parameters.AddWithValue("@mail_subject", strSubject)
        cmd.Parameters.AddWithValue("@mail_bodystr", strbody)
        cmd.Parameters.AddWithValue("@mail_recipient", strRecipient)
        cmd.CommandText = str
        If cmd.Connection.State <> ConnectionState.Open Then cmd.Connection.Open()
        Dim defV As Integer = cmd.ExecuteScalar()

        If cmd.Connection.State = ConnectionState.Open Then cmd.Connection.Close()
    End Sub
#End Region




End Class