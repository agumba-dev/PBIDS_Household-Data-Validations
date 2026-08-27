Imports Microsoft.SqlServer.Management.Smo
Imports System.Data.SqlClient
Imports DgvFilterPopup

Public Class frmDBEditor
    Public myServer As Server
    Public mytable As Table
    Private currentTable As Table
    Friend mydatabase As Database
    Friend mycon As SqlConnection
    Dim dtlSQLServers As DataTable
    Dim ds As New DataSet()
    Dim da As SqlDataAdapter
    Friend bsource As New BindingSource()
    Dim preventLeaveRow As Boolean = False
    ' Private bsource As New BindingSource()
    'Dim ds As New DataSet()
    'Dim da As SqlDataAdapter
    'Dim defaultvalue As String = ""
    Dim validationID As Integer = 0
    Dim filt As New DgvFilterManager()
    Dim enableValidation As Boolean
    Dim validationLevel As datalevel
    Public dacc As clsDataAccess = clsDataAccess.getObject
    Dim data_transfer As clsDataTransfer = clsDataTransfer.getObject
    Friend Sub setConnection()
        Dim sqlServerConStr As String = "Data Source= " & myServer.Name & "; initial catalog= " & mydatabase.Name & "; integrated security=true"
        mycon = New SqlConnection(sqlServerConStr)

    End Sub

    Private Sub findServersButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles findServersButton.Click
        Me.Cursor = Cursors.WaitCursor
        Dim i As Integer = 0

        ' Get list of all available servers.
        dtlSQLServers = SmoApplication.EnumAvailableSqlServers(False)
        serversComboBox.Items.Clear()
        serversComboBox.Items.Add("(local)")

        For Each drServer As DataRow In dtlSQLServers.Rows

            If drServer("IsLocal").Equals(True) Then
                serversComboBox.Items.Add(drServer("Name"))

                serversComboBox.SelectedItem = drServer("Name")
            Else
                serversComboBox.Items.Add(drServer("Name"))

            End If

        Next

        ' some code to show the column names in a specified datatable
        'For Each column As DataColumn In dtlSQLServers.Columns
        '    tableNameListBox.Items.Add(column.ColumnName)

        'Next
        Me.Cursor = Cursors.Default

        findServersButton.Enabled = True
    End Sub

    Private Sub databasesComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles databasesComboBox.SelectedIndexChanged
        'get now the table names of a database  selected
        Dim databasename As String = databasesComboBox.Text

        mydatabase = myServer.Databases.Item(databasename)
        cmb_chooseTable.Items.Clear()
        For Each tble As Table In mydatabase.Tables

            ' get only tables that are user defined
            'If Not tble.IsSystemObject() Then tableNameListBox.Items.Add(tble)
            If Not tble.IsSystemObject() Then cmb_chooseTable.Items.Add(tble)

        Next
        setConnection()

    End Sub


    Private Sub cmb_chooseTable_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmb_chooseTable.SelectedIndexChanged

        Me.Cursor = Cursors.WaitCursor

        mytable = mydatabase.Tables.Item(CType(cmb_chooseTable.SelectedItem, Table).Name, CType(cmb_chooseTable.SelectedItem, Table).Schema)

        CBFiledDesc1.Items.Clear()
        CBFiledDesc2.Items.Clear()
        'TODO add code that determines whether table is editable or not
        'Dim x As Integer
        For Each clmn As Column In mytable.Columns
            'TableDetailsListView.Items.Insert(0, clmn.Name)
            'cmbColumnName.Items.Add(clmn.Name)
            'TableDetailsListView.Items.Item(0).SubItems.Add(clmn.DataType.Name)
            'TableDetailsListView.Items.Item(0).SubItems.Add(clmn.DataType.MaximumLength)
            'x = TableDetailsListView.Items.Count
            'TableDetailsListView.Items.Insert(x, clmn.Name)
            'TableDetailsListView.Items.Item(x).SubItems.Add(clmn.DataType.Name)
            'TableDetailsListView.Items.Item(x).SubItems.Add(clmn.DataType.MaximumLength)

            CBFiledDesc1.Items.Add(clmn.Name)
            CBFiledDesc2.Items.Add(clmn.Name)
        Next
        'For num As Integer = 0 To SplitContainer1.Panel2.Controls.Count - 1
        '    num += 1
        '    SplitContainer1.Panel2.Controls.Item(num).Enabled = False

        'Next
        Me.Cursor = Cursors.Default

    End Sub

    Private Sub btnconnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnconnect.Click
        connectToServer(serversComboBox.Text)
    End Sub

    Public Sub connectToServer(ByVal servername As String)
        'we want to now get the databases in the server selected
        Try
            'Dim servername As String =

            myServer = New Server(servername)
            servername = myServer.Name
            databasesComboBox.Items.Clear()
            For Each db As Database In myServer.Databases
                'gets only databasess that are user defined
                If Not db.IsSystemObject() Then databasesComboBox.Items.Add(db.Name)
            Next
        Catch ex As Exception
            MsgBox("connection Failed: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnQuerry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuerry.Click
        Try
            'check if there are any uncomited changes in the current data shown
            If ds.HasChanges Then
                If Not MsgBox("There are uncommited changes to the database, continuing will cancel this changes and changes will be lost. continue?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) = MsgBoxResult.Yes Then
                    Return
                End If
            End If
            Me.Cursor = Cursors.WaitCursor

            getData()
            ckb_editmode.Checked = False
            Me.cbx_upload.Checked = False
            If Not (databasesComboBox.Text = "TEMP_DSSHRS" OrElse databasesComboBox.Text = "DSSHRS" OrElse databasesComboBox.Text.ToUpper.Trim = "KIMSAS" OrElse databasesComboBox.Text.ToUpper.Trim = "DSSIPDOPD") Then
                Me.cbx_upload.Enabled = False
                ckb_editmode.Enabled = False
            Else
                ckb_editmode.Enabled = True
                Me.cbx_upload.Enabled = (databasesComboBox.Text = "TEMP_DSSHRS") And (ckb_editmode.Checked)
            End If
            setEditState()
            'disablereadOnlyRows()
            '  ctrans.retriveInformation() '= New ClassTransform
            Me.Cursor = Cursors.Default
            ' PBarlabel.Text = "Ready"
            ' PBar.Style = ProgressBarStyle.Blocks
            Me.dgv_records.AllowUserToDeleteRows = (databasesComboBox.Text = "DSSHRS")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub getData()
        Try
            mycon.Open()

            Dim conjunction As String = "AND"
            If rbAnd.Checked Then
                conjunction = "AND"
            ElseIf rbOR.Checked Then
                conjunction = "OR"
            End If
            Dim sql As String = getSQL(mytable.Schema & "." & mytable.Name, CBFiledDesc1.Text.Trim, CBFiledDesc2.Text.Trim, _
            TbFieldDesc1.Text.Trim, TbFieldDesc2.Text.Trim, conjunction)
            '   MsgBox(sql)
            ' Return
            '   Dim sql As String = "SELECT * FROM " & (mytable.Name) & " WHERE fldwrk='" & INDIVIDUALS.cbCICode.Text.Trim & "'"

            da = New SqlDataAdapter(sql, mycon)

            mycon.Close()
            'da.AcceptChangesDuringUpdate=True

            Dim rowcmdbuild As SqlCommandBuilder = New SqlCommandBuilder(da)

            ' Dim dt As New DataTable()
            ds.Tables.Clear()
            'ds.Clear()
            'dont get the data just get the resultset structure
            'da.FillSchema(ds, SchemaType.Source)
            da.MissingSchemaAction = MissingSchemaAction.AddWithKey

            da.Fill(ds, "Table")
            bsource.DataSource = Nothing
            bsource.DataSource = ds
            bsource.DataMember = "Table" 'mytable.Schema & "." & mytable.Name
            bn_mainDB.BindingSource = bsource
            dgv_records.DataSource = bsource
            currentTable = mytable
          
            'disable add new item and delete until triggers in the db are in place esp. for deletion
            BindingNavigatorDeleteItem.Enabled = False
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Public Function getSQL(ByVal table As String, ByVal criteria1 As String, ByVal criteria2 As String, _
    ByVal value1 As String, ByVal value2 As String, ByVal Conjunction As String) As String
        Dim comparetype1 As String = "="
        Dim comparetype2 As String = "="
        Dim endTag1 As String = ""
        Dim endTag2 As String = ""
        Dim generatedsql As String = ""

        'check if its a location
        'If isLocationField(table, criteria1) Then
        '    comparetype1 = "LIKE"
        '    endTag1 = "%"
        'End If
        'If isLocationField(table, criteria2) Then
        '    comparetype2 = "LIKE"
        '    endTag2 = "%"
        'End If

        Dim defaultSQL As String = "SELECT " + table + ".*  FROM  " + table

        If criteria1.Equals("") Or criteria2.Equals("") Then 'if one is empty
            If Not criteria1.Equals("") Then
                generatedsql = " WHERE (" + criteria1 + " " & comparetype1 & " '" + value1 + endTag1 + "')"
            ElseIf Not criteria2.Equals("") Then
                generatedsql = " WHERE (" + criteria2 + " " & comparetype2 & " '" + value2 + endTag2 + "')"
            End If
        ElseIf Not criteria1.Equals("") And Not criteria2.Equals("") Then 'none is empty

            generatedsql = " WHERE (" + criteria1 + " " & comparetype1 & " '" + value1 + endTag1 + "'" & Conjunction & " " + _
            criteria2 + " " & comparetype2 & " '" + value2 + endTag2 + "' )"
        End If
        Return defaultSQL + generatedsql
    End Function
    Public Sub disablereadOnlyRows()
        Dim mycommand As New SqlCommand
        If mycon.State <> ConnectionState.Open Then mycon.Open()

        mycommand.Connection = mycon

        mycommand.CommandText = "SELECT column_name FROM [dataChecker].[dbo].[dbconfiguration]  WHERE table_name='[" & mytable.Schema & "].[" & mytable.Name & "]' and dbname='" & mydatabase.Name & "'"
        Dim myadpt As New SqlDataAdapter(mycommand)
        Dim dt As New DataTable
        dt.Clear()
        myadpt.Fill(dt)
        For Each clm As DataGridViewColumn In dgv_records.Columns
            Dim results As DataRow() = dt.Select("column_name='" & clm.Name & "'")

            If results.Length > 0 Then 'TODO querry the data table here

                clm.ReadOnly = True

            End If

        Next
        mycon.Close()
    End Sub
    Public Sub setEditState()

        Dim mycommand As New SqlCommand
        If mycon.State <> ConnectionState.Open Then mycon.Open()

        mycommand.Connection = mycon

        mycommand.CommandText = "SELECT count(*) FROM [dataChecker].[dbo].[EditableTables]  WHERE table_name='[" & mytable.Schema & "].[" & mytable.Name & "]' and dbname='" & mydatabase.Name & "'"
        Dim i As Integer = mycommand.ExecuteScalar()

        If i > 0 Then
            disablereadOnlyRows()
            lbl_datasetState.Text = "Editable"
            lbl_datasetState.ForeColor = Color.Green

            If mydatabase.Name.ToUpper = "TEMP_DSSHRS" Then

                validationLevel = datalevel.TEMP_DSSHRS
            Else
                validationLevel = datalevel.DSSHRS
            End If
        Else
            For Each clm As DataGridViewColumn In dgv_records.Columns
                clm.ReadOnly = True
            Next
            ckb_editmode.Enabled = False
            lbl_datasetState.Text = "Read Only"
            lbl_datasetState.ForeColor = Color.Red
        End If
        mycon.Close()


    End Sub
    Private Sub frmDBEditor_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim filt As New DgvFilterManager(dgv_records)
        filt.DataGridView = dgv_records
        enableValidation = ckb_editmode.Checked
    End Sub

    Private Sub SaveToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripButton.Click
        'dgv_records.CurrentRow.Selected = False
        If dgv_records.IsCurrentRowDirty Then

            Dim drv As DataRowView = dgv_records.CurrentRow.DataBoundItem
            Dim dr As DataRow = drv.Row
            If Not dgv_records.CurrentCell.EditedFormattedValue.Equals(dgv_records.CurrentCell.Value) Then
                dgv_records.CurrentCell.Style.BackColor = Color.Cyan
            End If
            dr.Item(dgv_records.CurrentCell.OwningColumn.Name) = dgv_records.CurrentCell.EditedFormattedValue
            dacc.initializeServerAndDB()
            '   If Not frm_Validations.getRowValidations(currentTable.Schema, currentTable.Name, dr, , True) Then
            If Not validaterow(dr, False) Then

                dgv_records.CurrentRow.DefaultCellStyle.BackColor = Color.White

                dr.RejectChanges()
                dr.CancelEdit()
                dgv_records.CancelEdit()
                preventLeaveRow = True
                Return
            Else

                preventLeaveRow = False
            End If

        End If
        saveChanges()
    End Sub
    Public Sub saveChanges()
        'bsource.
        'bsource.EndEdit()
        If ds.HasChanges Then
            Try
                MsgBox(da.Update(ds.Tables(0)) & " row(s) Changed")
                ds.AcceptChanges()
            Catch ex As Exception
                MsgBox(ex.Message)
                ds.RejectChanges()
            End Try

        Else
            MsgBox("records can't be updated")
        End If

    End Sub
    
    Private Sub dgv_records_CellValueChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_records.CellValueChanged
        Try
            If Not enableValidation Then Return
            'If Not MsgBox("Save Changes?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            '    dgv_records.CancelEdit()
            'End If
            '  If dgv_records.IsCurrentCellDirty Then
            Dim drv As DataRowView = dgv_records.CurrentRow.DataBoundItem
            Dim dr As DataRow = drv.Row

            '   dr.Item(dgv_records.CurrentCell.OwningColumn.Name) = dgv_records.CurrentCell.EditedFormattedValue
            ' If Not MsgBox("Keep Changes?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

            ' dr.RejectChanges()
            'dgv_records.CancelEdit()
            ' Else
            'get the row and validate the changed field using the validator

            'Dim drv As DataRowView = dgv_records.CurrentRow.DataBoundItem
            'Dim dr As DataRow = drv.Row
            'dr.Item(dgv_records.CurrentCell.OwningColumn.Name) = dgv_records.CurrentCell.EditedFormattedValue
            '    dr.Item(dgv_records.CurrentCell.OwningColumn.Name) = dgv_records.CurrentCell.EditedFormattedValue
            If Not validaterowColumn(New DataColumn(dgv_records.CurrentCell.OwningColumn.Name, dgv_records.CurrentCell.OwningColumn.ValueType), dr) Then

                dr.RejectChanges()
                dgv_records.CancelEdit()
                dgv_records.CurrentCell.Style.BackColor = Color.White

                '   MsgBox("invalid value specified, cannot post changes")
            Else
                '   dr.AcceptChanges()

                dgv_records.CurrentCell.Style.BackColor = Color.Cyan
            End If

            ' End If
            ' End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        
    End Sub

    Private Sub dgv_records_CellValidating(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) Handles dgv_records.CellValidating

    End Sub


    Private Sub dgv_records_CellLeave(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_records.CellLeave
        'If dgv_records.IsCurrentCellDirty Then
        '    Dim drv As DataRowView = dgv_records.CurrentRow.DataBoundItem
        '    Dim dr As DataRow = drv.Row

        '    dr.Item(dgv_records.CurrentCell.OwningColumn.Name) = dgv_records.CurrentCell.EditedFormattedValue
        '    If Not MsgBox("Keep Changes?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

        '        dr.RejectChanges()
        '        dgv_records.CancelEdit()
        '    Else
        '        'get the row and validate the changed field using the validator

        '        'Dim drv As DataRowView = dgv_records.CurrentRow.DataBoundItem
        '        'Dim dr As DataRow = drv.Row
        '        'dr.Item(dgv_records.CurrentCell.OwningColumn.Name) = dgv_records.CurrentCell.EditedFormattedValue
        '        dr.Item(dgv_records.CurrentCell.OwningColumn.Name) = dgv_records.CurrentCell.EditedFormattedValue
        '        If Not validaterow(New DataColumn(dgv_records.CurrentCell.OwningColumn.Name, dgv_records.CurrentCell.OwningColumn.ValueType), dr) Then
        '            dr.RejectChanges()
        '            dgv_records.CancelEdit()
        '            '   MsgBox("invalid value specified, cannot post changes")
        '        Else
        '            dr.AcceptChanges()
        '            dgv_records.CurrentCell.Style.BackColor = Color.Cyan
        '        End If

        '    End If
        'End If
    End Sub

    Private Sub dgv_records_RowLeave(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_records.RowLeave
        Try
            If dgv_records.IsCurrentRowDirty Then
                'xxxxx
                Dim drv As DataRowView = dgv_records.CurrentRow.DataBoundItem
                Dim dr As DataRow = drv.Row
                If Not dgv_records.CurrentCell.EditedFormattedValue.Equals(dgv_records.CurrentCell.Value) Then
                    dgv_records.CurrentCell.Style.BackColor = Color.Cyan
                End If
                dr.Item(dgv_records.CurrentCell.OwningColumn.Name) = dgv_records.CurrentCell.EditedFormattedValue
                dacc.initializeServerAndDB()
                '   If Not frm_Validations.getRowValidations(currentTable.Schema, currentTable.Name, dr, , True) Then
                If Not validaterow(dr, False) Then

                    dgv_records.CurrentRow.DefaultCellStyle.BackColor = Color.White

                    dr.RejectChanges()
                    dr.CancelEdit()
                    dgv_records.CancelEdit()

                    Dim i As Integer
                    For i = 0 To dgv_records.Rows(e.RowIndex).Cells.Count - 1
                        dgv_records(i, e.RowIndex).Style _
                            .BackColor = Color.Empty
                    Next i
                    preventLeaveRow = True
                Else

                    preventLeaveRow = False

                End If

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        
    End Sub

    Private Sub dgv_records_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_records.RowEnter
        '  If Not dgv_records.IsCurrentRowDirty Then

        '   dgvr = dgv_records.CurrentRow
        ' End If
        Me.dgv_records.AllowUserToDeleteRows = (databasesComboBox.Text = "DSSHRS")
    End Sub
    Public Function validaterow(ByVal record As DataRow, ByVal cantransferData As Boolean) As Boolean
        If Not enableValidation Then Return False
        dacc.initializeServerAndDB()
        frm_ToValidateForm.combinederrorText = ""
        Dim status As Boolean = True

        If frm_ToValidateForm.getRowValidations(currentTable.Schema, currentTable.Name, record, , True) = False Then
            status = False
        End If
        If (validationLevel = datalevel.TEMP_DSSHRS) Then
            'And (Not Me.dgv_records.IsCurrentRowDirty) And cantransferData Then
            If status = False Then
                dacc.updateErrorFlag(currentTable.Schema & "." & currentTable.Name, record.Item("transit_id"), True)
            Else
                If (Not Me.dgv_records.IsCurrentRowDirty) And cantransferData Then
                    dacc.updateErrorFlag(currentTable.Schema & "." & currentTable.Name, record.Item("transit_id"), False)
                End If
            End If
        End If
        frmDataTransfer.initialiseGlobalVariables()
        Dim data_transfer As clsDataTransfer = clsDataTransfer.getObject
        data_transfer.da.validationtype = mhrsSyncValidationTypes.userpplication
        data_transfer.da.UserAppvalidationerrors = ""
        'commented out since it aint working well
        If data_transfer.getRowValidations(validationLevel, currentTable.Schema.ToLower, currentTable.Name.ToLower, record, , True) = False Then
            status = False
        End If


        If status = False AndAlso frm_ToValidateForm.combinederrorText.Trim & data_transfer.da.UserAppvalidationerrors.Trim <> "" Then
            MsgBox(frm_ToValidateForm.combinederrorText & data_transfer.da.UserAppvalidationerrors)
        Else

            'force status  to be true since no error erros messages
            ' done to cater for temp_dsshrs validation functions that dont return true or false

            If (validationLevel = datalevel.TEMP_DSSHRS) And cantransferData And (Me.cbx_upload.Checked) And (status = True) Then
                If (Me.dgv_records.IsCurrentRowDirty) Then
                    MsgBox(" The record cannot be uploaded because it has uncommitted changes ", MsgBoxStyle.Critical)
                Else
                    If record("rec_status").ToString.ToLower.Trim().Contains("x") Then
                        MsgBox("record was marked for deletion", MsgBoxStyle.Critical)
                        Return False
                    End If
                    If MsgBox("Are you sure you want to upload the record now?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

                        If data_transfer.transferTEMPRowValidations(currentTable.Schema.ToLower, currentTable.Name.ToLower, record) Then
                            MsgBox(" record has been uploaded to main database")
                        Else
                            MsgBox(" record has not been uploaded to main database", MsgBoxStyle.Exclamation)
                        End If
                    End If
                End If
            End If
            status = True

        End If


        'data_transfer.worker = BackgroundWorkerValidate

        data_transfer.da.validationtype = 0
        Return status
    End Function
    Public Function validaterowColumn(ByVal clmn As DataColumn, ByVal record As DataRow) As Boolean
        If Not enableValidation Then Return False

        Me.dacc.initializeServerAndDB()
        frm_ToValidateForm.combinederrorText = ""
        Dim status As Boolean = True

        If frm_ToValidateForm.getRowValidations(currentTable.Schema, currentTable.Name, record, clmn, True) = False Then status = False

        'frmDataTransfer.initialiseGlobalVariables()
        'Dim data_transfer As New clsDataTransfer()
        'data_transfer.da.validationtype = mhrsSyncValidationTypes.userpplication
        'data_transfer.da.UserAppvalidationerrors = ""
        ' If data_transfer.getRowValidations(currentTable.Schema, currentTable.Name, record, clmn) = False Then status = False

        If status = False Then
            '   MsgBox(frm_Validations.combinederrorText & data_transfer.da.UserAppvalidationerrors)
            MsgBox(frm_ToValidateForm.combinederrorText) '& data_transfer.da.UserAppvalidationerrors)
        End If


        'data_transfer.worker = BackgroundWorkerValidate
        ' data_transfer.da.validationtype = 0
        '    data_transfer(validationtype)
        Return status
    End Function

    Private Sub dgv_records_CellEndEdit(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_records.CellEndEdit

    End Sub


    Private Sub dgv_records_RowValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles dgv_records.RowValidating
     
        If preventLeaveRow Then
            e.Cancel = True
            dgv_records.Rows(e.RowIndex).Selected = True
            preventLeaveRow = False
        Else
            e.Cancel = False
        End If

    End Sub

    Private Sub dgv_records_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_records.CellContentClick

    End Sub

    Private Sub ckb_editmode_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckb_editmode.CheckedChanged
        If ckb_editmode.Checked Then

            SaveToolStripButton.Enabled = True
            'BindingNavigatorDeleteItem.Enabled = True
            Me.BindingNavigatorDeleteItem.Enabled = (databasesComboBox.Text = "DSSHRS")
            For Each dgvCol As DataGridViewColumn In dgv_records.Columns

                dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable

            Next
            enableValidation = True
            Me.cbx_upload.Enabled = (databasesComboBox.Text = "TEMP_DSSHRS")
        Else
            enableValidation = False

            ' filt.DataGridView = dgv_records
            SaveToolStripButton.Enabled = False
            BindingNavigatorDeleteItem.Enabled = False
            For Each dgvCol As DataGridViewColumn In dgv_records.Columns

                dgvCol.SortMode = DataGridViewColumnSortMode.Automatic

            Next
            enableValidation = False
            Me.cbx_upload.Checked = False
            Me.cbx_upload.Enabled = False
        End If
    End Sub
    Private Sub BindingNavigatorDeleteItem_EnabledChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BindingNavigatorDeleteItem.EnabledChanged
        If Not ckb_editmode.Checked Then
            BindingNavigatorDeleteItem.Enabled = False
        End If
        If Not (databasesComboBox.Text = "DSSHRS") Then
            BindingNavigatorDeleteItem.Enabled = False
        End If
    End Sub
    Private Sub BindingNavigatorDeleteItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BindingNavigatorDeleteItem.Click
        'MsgBox("nkt!")
    End Sub

    Private Sub dgv_records_UserDeletingRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs) Handles dgv_records.UserDeletingRow
        If Not (databasesComboBox.Text = "DSSHRS") Then
            MsgBox("You are not allowed to delete records for these db")
        Else
            MsgBox("nkt!")
        End If

    End Sub

    Private Sub tsp_validate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsp_validate.Click
        Try
            'Dim drv As DataRowView = dgv_records.CurrentRow.DataBoundItem
            'Dim dr As DataRow = drv.Row
            Me.validateSelectedRow()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub validateSelectedRow()
        
        If Me.dgv_records.IsCurrentRowDirty Then
            MsgBox("The record has un committed changes ", MsgBoxStyle.Critical)
            Exit Sub
        End If

        dacc.initializeServerAndDB()

        If (validationLevel = datalevel.TEMP_DSSHRS) Then
            dgv_records.CurrentRow.DefaultCellStyle.BackColor = Color.White
            For Each drv1 As DataGridViewRow In Me.dgv_records.Rows
                Dim recdrv As DataRowView = drv1.DataBoundItem
                Dim rec As DataRow = recdrv.Row
                rec.RejectChanges()
                rec.CancelEdit()
                For Each cell As DataGridViewCell In drv1.Cells
                    cell.Style.BackColor = Color.Empty
                Next
            Next
            Me.dgv_records.CancelEdit()
        End If
        Dim drv As DataRowView = dgv_records.CurrentRow.DataBoundItem
        Dim dr As DataRow = drv.Row
        '' validate and transfer record
        If Not validaterow(dr, True) Then
            'dgv_records.CurrentRow.DefaultCellStyle.BackColor = Color.White
            'dr.RejectChanges()
            'dr.CancelEdit()
            'dgv_records.CancelEdit()
        Else
            MsgBox("record has no error", MsgBoxStyle.Information)
        End If
    End Sub
    Private Sub tsp_Cancelchanges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsp_Cancelchanges.Click
        Try
            dgv_records.CurrentRow.DefaultCellStyle.BackColor = Color.White
            For Each drv1 As DataGridViewRow In Me.dgv_records.Rows
                Dim recdrv As DataRowView = drv1.DataBoundItem
                Dim rec As DataRow = recdrv.Row
                rec.RejectChanges()
                rec.CancelEdit()
                For Each cell As DataGridViewCell In drv1.Cells
                    cell.Style.BackColor = Color.Empty
                Next
            Next
            Me.dgv_records.CancelEdit()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub tsmn_ValidateRecord_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmn_ValidateRecord.Click
        Me.tsp_validate.PerformClick()
    End Sub

    Private Sub tsmn_CancelChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmn_CancelChange.Click
        Me.tsp_Cancelchanges.PerformClick()
    End Sub

    Private Sub tsp_RetrieveRecords_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsp_RetrieveRecords.Click
        Me.btnQuerry.PerformClick()
    End Sub

    Private Sub CutToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CutToolStripButton.Click

    End Sub

    Private Sub serversComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles serversComboBox.SelectedIndexChanged

    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub MenuStrip1_ItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles MenuStrip1.ItemClicked

    End Sub
End Class
