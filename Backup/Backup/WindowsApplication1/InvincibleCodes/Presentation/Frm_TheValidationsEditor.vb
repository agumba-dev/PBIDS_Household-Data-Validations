Imports Microsoft.SqlServer.Management.Smo
Imports System.Data.SqlClient
Public Class Frm_TheValidationsEditor

    Dim dtlSQLServers As DataTable
    Public objda As clsDataAccess = clsDataAccess.getObject
    Public objVal As clsvalidations = clsvalidations.getObject
    Public objRef As clsformrefrences = clsformrefrences.getObject
    Public objUtil As clsUtilities = clsUtilities.getObject

    ' Private bsource As New BindingSource()
    'Dim ds As New DataSet()
    'Dim da As SqlDataAdapter
    'Dim defaultvalue As String = ""
    Dim validationID As Integer = 0

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


    End Sub
    Private Sub loadTables()
        ' Dim databasename As String = databasesComboBox.Text

        Me.objda.mydatabase = Me.objda.myServer.Databases.Item(Me.objda.databaseName)
        tableNameListBox.Items.Clear()
        For Each tble As Table In Me.objda.mydatabase.Tables

            ' get only tables that are user defined
            If Not tble.IsSystemObject() Then tableNameListBox.Items.Add(tble)

        Next
        getfunctions()
        'setConnection()
    End Sub


    Private Sub getfunctions()
        cmb_valueFunctions.Items.Clear()
        cmb_valueFunctions.Items.Add("")
        cmb_skipFunctions.Items.Clear()
        cmb_skipFunctions.Items.Add("")
        For Each func As UserDefinedFunction In Me.objda.mydatabase.UserDefinedFunctions
            ' get only tables that are user defined
            If Not func.IsSystemObject() Then cmb_valueFunctions.Items.Add(func)
            If Not func.IsSystemObject() Then cmb_skipFunctions.Items.Add(func)
        Next

    End Sub
    Private Sub tableNameListBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tableNameListBox.SelectedIndexChanged

        Me.Cursor = Cursors.WaitCursor

        Me.objda.mytable = Me.objda.mydatabase.Tables.Item(CType(tableNameListBox.SelectedItem, Table).Name, CType(tableNameListBox.SelectedItem, Table).Schema)

        TableDetailsListView.Items.Clear()
        cmbColumnName.Items.Clear()
        Dim x As Integer
        For Each clmn As Column In Me.objda.mytable.Columns
            'TableDetailsListView.Items.Insert(0, clmn.Name)
            'cmbColumnName.Items.Add(clmn.Name)
            'TableDetailsListView.Items.Item(0).SubItems.Add(clmn.DataType.Name)
            'TableDetailsListView.Items.Item(0).SubItems.Add(clmn.DataType.MaximumLength)
            x = TableDetailsListView.Items.Count
            TableDetailsListView.Items.Insert(x, clmn.Name)
            cmbColumnName.Items.Add(clmn.Name)
            TableDetailsListView.Items.Item(x).SubItems.Add(clmn.DataType.Name)
            TableDetailsListView.Items.Item(x).SubItems.Add(clmn.DataType.MaximumLength)

        Next
        'For num As Integer = 0 To SplitContainer1.Panel2.Controls.Count - 1
        '    num += 1
        '    SplitContainer1.Panel2.Controls.Item(num).Enabled = False

        'Next
        Me.Cursor = Cursors.Default

    End Sub

    Private Sub btnconnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnconnect.Click
        'we want to now get the databases in the server selected
        Try
            Dim servername As String = serversComboBox.Text

            Me.objda.myServer = New Server(servername)
            servername = Me.objda.myServer.Name
            databasesComboBox.Items.Clear()
            For Each db As Database In Me.objda.myServer.Databases

                'gets only databasess that are user defined
                If Not db.IsSystemObject() Then databasesComboBox.Items.Add(db.Name)
            Next
        Catch ex As Exception
            Me.objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            MsgBox("connection Failed: " & ex.Message, MsgBoxStyle.Critical)

        End Try
    End Sub

    Private Sub TableDetailsListView_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TableDetailsListView.SelectedIndexChanged
        'cmbColumnName.Items.Remove(TableDetailsListView.FocusedItem.Text)

        rbtn_range.Checked = False
        rbtn_singleValue.Checked = False
        rbtn_anyValue.Checked = False
        ' loadColumnValidations(TableDetailsListView.FocusedItem.Text)
        loadGridColumnValidations(TableDetailsListView.FocusedItem.Text)
        If TableDetailsListView.SelectedItems.Count > 0 Then
            lblselectedColumn.Text = TableDetailsListView.FocusedItem.Text & vbNewLine & TableDetailsListView.FocusedItem.SubItems(1).Text
            grpBoxDataGrid.Text = "Column Validations (" & TableDetailsListView.FocusedItem.Text & ") "
            gpb_valueType.Enabled = True
            gpb_skipCriteria.Enabled = True
        Else
            lblselectedColumn.Text = ""
            grpBoxDataGrid.Text = "Column Validations"
            gpb_valueType.Enabled = False
            gpb_skipCriteria.Enabled = False
        End If
    End Sub

    Private Sub rbtn_singleValue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtn_singleValue.CheckedChanged
        If rbtn_singleValue.Checked Then
            'enable date or numbers depending on the datatype of the selected column
            pnl_singleValue.Enabled = True

            If TableDetailsListView.FocusedItem.SubItems(1).Text.ToUpper.Contains("DATE") Then
                ckb_insertDate.Checked = True
                dtp_singleValue.Enabled = True

                txt_singleValue.Enabled = False
                txt_singleValue.Text = ""
            Else
                ckb_insertDate.Checked = False
                dtp_singleValue.Enabled = False
                txt_singleValue.Enabled = True

            End If
        Else
            pnl_singleValue.Enabled = False
            txt_singleValue.Text = ""
            dtp_singleValue.Enabled = False
            dtp_singleValue.Value = Now.Date
        End If

    End Sub

    Private Sub rbtn_range_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtn_range.CheckedChanged
        If rbtn_range.Checked Then
            'enable date or numbers depending on the datatype of the selected column
            If TableDetailsListView.FocusedItem.SubItems(1).Text.ToUpper.Contains("DATE") Then
                pnl_daterange.Enabled = True
                pnl_range.Enabled = False
                num_rangeFrom.Value = 0
                num_rangeTo.Value = 0
            Else
                pnl_range.Enabled = True
                pnl_daterange.Enabled = False
                ckbCurrentDate.Checked = False
                dtp_rangeFrom.Value = Now.Date
                dtp_rangeTo.Value = Now.Date
            End If
        Else
            pnl_daterange.Enabled = False
            pnl_range.Enabled = False
            num_rangeFrom.Value = 0
            num_rangeTo.Value = 0
            ckbCurrentDate.Checked = False
            dtp_rangeFrom.Value = Now.Date
            dtp_rangeTo.Value = Now.Date
        End If

    End Sub
    Private Sub rbtn_anyValue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtn_anyValue.CheckedChanged
        'TODO enable any value to be input in the column...
        'If rbtn_anyValue.Checked AndAlso lstbox_ValuesAllowed.Items.Count > 0 Then
        'If MsgBox("This will clear the values already added continue?", MsgBoxStyle.Question _
        ' + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
        '    lstbox_ValuesAllowed.Items.Clear()
        '    defaultvalue = ""
        'Else
        '    rbtn_anyValue.Checked = False
        'End If

        If rbtn_anyValue.Checked Then
            lstbox_ValuesAllowed.Items.Clear()
            txt_defaultValue.Text = ""
            txt_defaultValue.Enabled = False
            lblDefaultvalue.Text = "Any Value allowed"
            btn_remove.Enabled = False
            btn_add.Enabled = False

            pnl_daterange.Enabled = False
            pnl_range.Enabled = False
            num_rangeFrom.Value = 0
            num_rangeTo.Value = 0
            ckbCurrentDate.Checked = False
            dtp_rangeFrom.Value = Now.Date
            dtp_rangeTo.Value = Now.Date

            pnl_singleValue.Enabled = False
            txt_singleValue.Text = ""
            dtp_singleValue.Enabled = False
            dtp_singleValue.Value = Now.Date
        Else
            btn_remove.Enabled = True
            btn_add.Enabled = True
            lblDefaultvalue.Text = "Default Value:"
            txt_defaultValue.Enabled = True
        End If

    End Sub

    Private Sub num_rangeFrom_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles num_rangeFrom.ValueChanged
        num_rangeTo.Minimum = num_rangeFrom.Value
    End Sub

    Private Sub dtp_rangeFrom_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_rangeFrom.ValueChanged
        dtp_rangeTo.MinDate = dtp_rangeFrom.Value
    End Sub

    Private Sub ckbCurrentDate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckbCurrentDate.CheckedChanged
        If ckbCurrentDate.Checked Then
            dtp_rangeTo.Enabled = False
        Else
            dtp_rangeTo.Enabled = True
        End If

    End Sub

    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtn_if.CheckedChanged
        If rbtn_if.Checked Then
            pnl_skipValues.Enabled = True
        Else
            pnl_skipValues.Enabled = False
        End If


    End Sub

    'Private Sub btn_addSkiplogic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_addSkiplogic.Click
    '    Dim valstr As String = ""
    '    'create the skip logic string
    '    'TODO the AND/OR closing bracket will look for its pair(opening bracket) from the last bracket written going upwards
    '    'in case maybe you want to add or remove an enclosed OR block or an enclosed AND block
    '    If rbtn_or.Checked AndAlso lstbox_SkipAdded.Items.Count > 0 Then valstr = "OR "
    '    valstr = valstr & cmbColumnName.Text.Trim & " = "
    '    valstr = valstr & "'" & txt_skipValue.Text.Trim & "'"
    '    lstbox_SkipAdded.Items.Add(valstr)
    'End Sub

    'Private Sub rbtn_required_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtn_required.CheckedChanged
    '    If rbtn_required.Checked AndAlso lstbox_SkipAdded.Items.Count > 0 Then
    '        If MsgBox("This will clear the skip logic already added continue?", MsgBoxStyle.Question _
    '         + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
    '            lstbox_SkipAdded.Items.Clear()
    '        Else
    '            rbtn_required.Checked = False
    '        End If


    '    End If
    'End Sub

    Private Sub btn_add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_add.Click
        'code for adding values accepted in a field
        Dim varstr As String = ""
        If rbtn_range.Checked Then 'add either date range or integer range
            'chek which range to add
            If pnl_daterange.Enabled Then ' it means its a date range inserted

                varstr = "$" & dtp_rangeFrom.Value.Date.ToString("yyyy/MM/dd") & "-"
                ' check if the range will be until a certain date or continuous 
                If ckbCurrentDate.Checked Then
                    varstr = varstr & "currentDate;"
                Else
                    varstr = varstr & dtp_rangeTo.Value.Date.ToString("yyyy/MM/dd") & ";"
                End If


                lstbox_ValuesAllowed.Items.Add(varstr)

            ElseIf pnl_range.Enabled Then

                varstr = "#" & num_rangeFrom.Value & "-"
                varstr = varstr & num_rangeTo.Value & ";"

                lstbox_ValuesAllowed.Items.Add(varstr)


            End If
        ElseIf rbtn_singleValue.Checked Then
            If txt_singleValue.Enabled Then
                lstbox_ValuesAllowed.Items.Add(txt_singleValue.Text.Trim & ";")
                txt_singleValue.Text = ""
            ElseIf dtp_singleValue.Enabled Then
                lstbox_ValuesAllowed.Items.Add("^" & dtp_singleValue.Value.Date.ToString("yyyy/MM/dd") & ";")
            End If


        End If
    End Sub


    Private Sub btn_remove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_remove.Click
        lstbox_ValuesAllowed.Items.Remove(lstbox_ValuesAllowed.SelectedItem)
    End Sub

    'Private Sub btn_RemoveSkipLogic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_RemoveSkipLogic.Click
    '    'if the removed item is at the top then remove the conjunction in the next line
    '    If lstbox_SkipAdded.SelectedIndex = 0 Then
    '        lstbox_SkipAdded.Items.Item(1) = lstbox_SkipAdded.Items.Item(1).ToString.TrimStart(" OR")
    '        lstbox_SkipAdded.Items.Item(1) = lstbox_SkipAdded.Items.Item(1).ToString.TrimStart(" AND")
    '    End If
    '    lstbox_SkipAdded.Items.Remove(lstbox_SkipAdded.SelectedItem)

    'End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        If Me.objUtil.isAuthorizedUser() Then

            If Me.objda.addValidation(TableDetailsListView.FocusedItem.Text, getAllowedValues(), getSkipLogic(), _
         txt_errorDesc.Text.Trim, txt_errordescSkipLogic.Text.Trim, txt_defaultValue.Text.Trim, _
         cmb_valueFunctions.Text, cmbValidationStatus.Text, validationID) Then
                MsgBox("Successfully added", MsgBoxStyle.Information)
                loadGridColumnValidations(TableDetailsListView.FocusedItem.Text)
            Else
                MsgBox("failed to add", MsgBoxStyle.Critical)
            End If
        Else
            MsgBox("You can't save validations! See system Admin.")
        End If



    End Sub

    Private Function getAllowedValues() As String
        Dim varstr As String = ""
        For Each kago As String In lstbox_ValuesAllowed.Items
            varstr = varstr & kago

        Next
        'remove the last ";" in the string
        varstr = varstr.TrimEnd(";"c)
        Return varstr
    End Function

    'Private Sub loadColumnValidations(ByVal columnName As String)
    '    Dim dt As DataTable = getColumnValidations(columnName, mytable.Schema & "." & mytable.Name)
    '    lstbox_ValuesAllowed.Items.Clear()
    '    lstbox_SkipAdded.Items.Clear()
    '    txt_errorDesc.Text = ""
    '    txt_errordescSkipLogic.Text = ""
    '    lblDefaultvalue.Text = "Default Value :"
    '    Dim strarr, strarrskipLogic As String()
    '    If dt.Rows.Count > 1 Then MsgBox("column " & mytable.Schema & "." & mytable.Name & " " & columnName _
    '    & " has more than i validation criteria defined for proper validation outcome, " & vbNewLine & "please ensure only 1 is defined ")
    '    For Each row As DataRow In dt.Rows
    '        strarr = row.Item("allowedValues").ToString.Split(";"c)
    '        strarrskipLogic = row.Item("skipLogic").ToString.Split(";"c)

    '        For Each str As String In strarr
    '            '  If String.IsNullOrEmpty(str) Then Continue For
    '            lstbox_ValuesAllowed.Items.Add(str & ";")

    '        Next
    '        For Each str As String In strarrskipLogic
    '            '  If String.IsNullOrEmpty(str) Then Continue For
    '            lstbox_SkipAdded.Items.Add(str)

    '        Next
    '        'load error description 

    '        txt_errorDesc.Text = row.Item("ErrorDescription").ToString
    '        txt_errordescSkipLogic.Text = row.Item("ErrorDescSkipLogic").ToString
    '        'load default values
    '        ' defaultvalue = row.Item("DefaultValue").ToString
    '        'lblDefaultvalue.Text = "Default Value: " & defaultvalue

    '        If strarr.Length = 1 AndAlso strarr(0).Trim = "" Then ' its an empty string denoting all values are valid
    '            lstbox_ValuesAllowed.Items.Clear()

    '            lblDefaultvalue.Text = "Any Value allowed"
    '        End If
    '    Next
    '    getDefaultValue(columnName, mytable.Schema & "." & mytable.Name)

    'End Sub
    'End Function

    Private Sub loadColumnValidations(ByVal row As DataRow)

        lstbox_ValuesAllowed.Items.Clear()
        lstbox_SkipAdded.Items.Clear()
        txt_errorDesc.Text = ""
        txt_errordescSkipLogic.Text = ""
        lblDefaultvalue.Text = "Default Value :"
        txt_defaultValue.Text = ""
        cmb_valueFunctions.SelectedIndex = -1
        cmbValidationStatus.SelectedIndex = -1

        Dim strarr, strarrskipLogic As String()

        validationID = row.Item("validationID")
        strarr = row.Item("allowedValues").ToString.Split(";"c)
        strarrskipLogic = row.Item("skipLogic").ToString.Split(";"c)

        For Each str As String In strarr
            '  If String.IsNullOrEmpty(str) Then Continue For
            lstbox_ValuesAllowed.Items.Add(str & ";")

        Next
        For Each str As String In strarrskipLogic
            '  If String.IsNullOrEmpty(str) Then Continue For
            lstbox_SkipAdded.Items.Add(str)

        Next
        'load error description 

        txt_errorDesc.Text = row.Item("ErrorDescription").ToString
        txt_errordescSkipLogic.Text = row.Item("ErrorDescSkipLogic").ToString
        'load Validation status
        cmbValidationStatus.Text = row.Item("validationStatus").ToString
        'load function used if any
        If row.Item("functionName").ToString.Trim <> "" Then
            ' cmb_functions.SelectedValue = row.Item("functionName").ToString
            'cmb_functions.SelectedItem = row.Item("functionName").ToString
            cmb_valueFunctions.Text = row.Item("functionName").ToString
        End If
        'load default values
        If IsDBNull(row.Item("DefaultValue")) Then
            txt_defaultValue.Text = ""
        Else
            Dim d As String = row.Item("DefaultValue")
            'check if the default value is an empty string if so the add it to the default value text box
            If d.Trim = "" Then d = "''"
            txt_defaultValue.Text = d
        End If
        lblDefaultvalue.Text = "Default Value: " & txt_defaultValue.Text

        If strarr.Length = 1 AndAlso strarr(0).Trim = "" Then ' its an empty string denoting all values are valid
            lstbox_ValuesAllowed.Items.Clear()

            lblDefaultvalue.Text = "Any Value allowed"
            txt_defaultValue.Enabled = False
            txt_defaultValue.Text = ""
        End If


    End Sub
    Private Sub loadGridColumnValidations(ByVal columnName As String)
        Dim dt As DataTable = Me.objda.getColumnValidationsDefinations(columnName, Me.objda.mytable.Schema & "." & Me.objda.mytable.Name)

        Dim bsource As New BindingSource()
        bsource.DataSource = dt

        dgvValidations.DataSource = bsource


    End Sub
    Private Sub loadColumnAllowedValues(ByVal columnName As String)
        Dim dt As DataTable = Me.objda.getColumnAllowedValues(columnName, Me.objda.mytable.Schema & "." & Me.objda.mytable.Name)
        Dim strarr As String()
        cmb_skipValue.Items.Clear()
        For Each row As DataRow In dt.Rows
            strarr = row.Item("allowedValues").ToString.Split(";"c)

            For Each str As String In strarr
                '  If String.IsNullOrEmpty(str) Then Continue For
                lstbox_ValuesAllowed.Items.Add(str)

            Next



        Next
    End Sub

    Private Sub ValidationsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ValidationsToolStripMenuItem.Click
        Dim frm_Vldtns As New frm_ToValidateForm
        frm_Vldtns.lstbox_tables.Items.AddRange(tableNameListBox.Items)
        ' frm_Vldtns.MdiParent = Me
        frm_Vldtns.ShowDialog()
    End Sub

    'Private Sub btn_insertopen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_insertopen.Click
    '    If lstbox_SkipAdded.SelectedItems.Count < 1 Then
    '        MsgBox("Select the line where the action is to be taken", MsgBoxStyle.Exclamation)
    '        Return
    '    End If
    '    Dim s As String = lstbox_SkipAdded.SelectedItem.ToString
    '    If s.StartsWith("AND ") Or s.StartsWith("OR ") Then
    '        MsgBox("Cannot add in selected line. please check logic", MsgBoxStyle.Exclamation)
    '        Return
    '    Else
    '        s = "(" & s
    '        lstbox_SkipAdded.Items.Insert(lstbox_SkipAdded.SelectedIndex, s)
    '        lstbox_SkipAdded.Items.RemoveAt(lstbox_SkipAdded.SelectedIndex)
    '    End If
    'End Sub

    'Private Sub btn_insertclose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_insertclose.Click
    '    If lstbox_SkipAdded.SelectedItems.Count < 1 Then
    '        MsgBox("Select the line where the action is to be taken", MsgBoxStyle.Exclamation)
    '        Return
    '    End If
    '    Dim s As String = lstbox_SkipAdded.SelectedItem.ToString
    '    If s.EndsWith("AND ") Or s.EndsWith("OR ") Then
    '        MsgBox("Cannot add in selected line. please check logic", MsgBoxStyle.Exclamation)
    '        Return
    '    Else
    '        s = s & ")"
    '        lstbox_SkipAdded.Items.Insert(lstbox_SkipAdded.SelectedIndex, s)
    '        lstbox_SkipAdded.Items.RemoveAt(lstbox_SkipAdded.SelectedIndex)
    '    End If
    'End Sub
    Private Sub btn_addSkiplogic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_addSkiplogic.Click
        Dim valstr As String = ""
        'create the skip logic string
        'TODO the AND/OR closing bracket will look for its pair(opening bracket) from the last bracket written going upwards
        'in case maybe you want to add or remove an enclosed OR block or an enclosed AND block

        'TODO add code for inserting a value a a certain position in the listview not necesarily @ the end
        If lstbox_SkipAdded.Items.Count > 0 Then
            If rbtn_or.Checked Then
                valstr = "OR "
            ElseIf rbtn_and.Checked Then
                valstr = "AND "
            End If
        End If


        valstr = valstr & cmbColumnName.Text.Trim & " " & cmb_booleanExp.SelectedItem.ToString & " "
        If rbtn_column.Checked Then
            valstr = valstr & " " & cmb_skipValue.Text.Trim & " "
        Else
            valstr = valstr & "'" & cmb_skipValue.Text.Trim & "'"
        End If

        lstbox_SkipAdded.Items.Add(valstr)
    End Sub

    Private Sub rbtn_required_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtn_required.CheckedChanged
        If rbtn_required.Checked AndAlso lstbox_SkipAdded.Items.Count > 0 Then
            If MsgBox("This will clear the skip logic already added continue?", MsgBoxStyle.Question _
             + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                lstbox_SkipAdded.Items.Clear()
            Else
                rbtn_required.Checked = False
            End If


        End If
    End Sub

    Private Sub btn_RemoveSkipLogic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_RemoveSkipLogic.Click
        'if the removed item is at the top then remove the conjunction in the next line
        If lstbox_SkipAdded.SelectedIndex = 0 AndAlso lstbox_SkipAdded.Items.Count > 1 Then
            Dim s As String = lstbox_SkipAdded.Items.Item(1).ToString
            If s.StartsWith("AND ") Then
                'add the thing '(' after the and
                s = s.Remove(0, 4)
            ElseIf s.StartsWith("OR ") Then
                'add the thing '(' after the or
                s = s.Remove(0, 3)
            End If

            lstbox_SkipAdded.Items.Item(1) = s

        End If
        lstbox_SkipAdded.Items.Remove(lstbox_SkipAdded.SelectedItem)

    End Sub

    Private Function getSkipLogic() As String
        Dim varstr As String = ""
        For Each kago As String In lstbox_SkipAdded.Items
            varstr = varstr & kago.Trim & ";"

        Next
        'remove the last ";" in the string
        varstr = varstr.TrimEnd(";"c)
        Return varstr
    End Function


    Private Sub btn_insertopen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_insertopen.Click
        If lstbox_SkipAdded.SelectedItems.Count < 1 Then
            MsgBox("Select the line where the action is to be taken", MsgBoxStyle.Exclamation)
            Return
        End If
        Dim s As String = lstbox_SkipAdded.SelectedItem.ToString
        If s.StartsWith("AND ") Then
            'add the thing '(' after the and
            s = s.Insert(4, "(")
        ElseIf s.StartsWith("OR ") Then
            'add the thing '(' after the or
            s = s.Insert(3, "(")
        Else
            s = "(" & s
        End If
        lstbox_SkipAdded.Items.Insert(lstbox_SkipAdded.SelectedIndex, s)
        lstbox_SkipAdded.Items.RemoveAt(lstbox_SkipAdded.SelectedIndex)

    End Sub

    Private Sub btn_insertclose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_insertclose.Click
        If lstbox_SkipAdded.SelectedItems.Count < 1 Then
            MsgBox("Select the line where the action is to be taken", MsgBoxStyle.Exclamation)
            Return
        End If
        Dim s As String = lstbox_SkipAdded.SelectedItem.ToString
        'If s.EndsWith("AND ") Or s.EndsWith("OR ") Then
        '    MsgBox("Cannot add in selected line. please check logic", MsgBoxStyle.Exclamation)
        '    Return
        'Else
        s = s & ")"
        lstbox_SkipAdded.Items.Insert(lstbox_SkipAdded.SelectedIndex, s)
        lstbox_SkipAdded.Items.RemoveAt(lstbox_SkipAdded.SelectedIndex)
        'End If
    End Sub

    Private Sub btn_removeopen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_removeopen.Click
        If lstbox_SkipAdded.SelectedItems.Count < 1 Then
            MsgBox("Select the line where the action is to be taken", MsgBoxStyle.Exclamation)
            Return
        End If
        Dim s As String = lstbox_SkipAdded.SelectedItem.ToString
        If s.StartsWith("AND (") Then
            'remove the thing '(' after the and
            s = s.Remove(4, 1)
        ElseIf s.StartsWith("OR (") Then
            'remove the thing '(' after the or
            s = s.Remove(3, 1)
        ElseIf s.StartsWith("(") Then
            s = s.Remove(0, 1)
        Else
            MsgBox("Value not present be removed in selected line", MsgBoxStyle.Exclamation)
            Return
        End If
        lstbox_SkipAdded.Items.Insert(lstbox_SkipAdded.SelectedIndex, s)
        lstbox_SkipAdded.Items.RemoveAt(lstbox_SkipAdded.SelectedIndex)
    End Sub
    Private Sub btn_removeClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_removeClose.Click

    End Sub

    Private Sub cmbColumnName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbColumnName.SelectedIndexChanged

    End Sub

    Private Sub lstbox_ValuesAllowed_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstbox_ValuesAllowed.SelectedIndexChanged
        If lstbox_ValuesAllowed.SelectedItems.Count = 1 Then
            lstbox_ValuesAllowed.ContextMenuStrip = ContextMenuDefaultvalue
        End If
    End Sub

    Private Sub SetAsDefaultValueToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetAsDefaultValueToolStripMenuItem.Click
        'defaultvalue = lstbox_ValuesAllowed.SelectedItem.ToString.TrimEnd(";"c)
        'defaultvalue = defaultvalue.Trim
        ''lstbox_ValuesAllowed.b()
        'lblDefaultvalue.Text = "Default value: " & defaultvalue
    End Sub


    Private Sub dgvValidations_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgvValidations.SelectionChanged
        'clear everything
        lstbox_ValuesAllowed.Items.Clear()
        lstbox_SkipAdded.Items.Clear()
        txt_errorDesc.Text = ""
        txt_errordescSkipLogic.Text = ""
        lblDefaultvalue.Text = "Default Value :"
        txt_defaultValue.Text = ""
        txt_defaultValue.Enabled = True
        validationID = 0
        GrpBoxSkiplogic.Enabled = False
        grpBoxValidValues.Enabled = False
        btn_save.Enabled = False
        cmb_valueFunctions.SelectedIndex = -1

        If Not dgvValidations.SelectedRows.Count = 0 Then
            GrpBoxSkiplogic.Enabled = True
            grpBoxValidValues.Enabled = True
            btn_save.Enabled = True

            If Not dgvValidations.SelectedRows.Item(0).IsNewRow Then


                Dim drv As DataRowView = dgvValidations.SelectedRows.Item(0).DataBoundItem
                Dim dr As DataRow = drv.Row
                loadColumnValidations(dr)

            End If

        End If

    End Sub

    Private Sub dgvValidations_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvValidations.CellContentClick

    End Sub

    Private Sub frmValidator_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cmb_booleanExp.SelectedIndex = 0
        Me.objda.initializeServerAndDB()
        loadTables()
    End Sub

    Private Sub rbtn_value_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtn_value.CheckedChanged
        If rbtn_value.Checked Then
            cmb_skipValue.Items.Clear()
            'TODO populate the cmb with all values allowed for that column?
        End If


    End Sub

    Private Sub rbtn_column_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtn_column.CheckedChanged
        If rbtn_column.Checked Then
            cmb_skipValue.Items.Clear()
            Dim val As New ArrayList()
            val.AddRange(cmbColumnName.Items)
            'val.CreateInstance(System.String, cmbColumnName.Items.Count)
            'val = cmbColumnName.Items.
            'cmbColumnName.Items.CopyTo(val., 0)
            cmb_skipValue.Items.AddRange(val.ToArray)
        End If


    End Sub

    Private Sub ckb_insertDate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ckb_insertDate.CheckedChanged
        If ckb_insertDate.Checked Then
            dtp_singleValue.Enabled = True
            txt_singleValue.Enabled = False
            txt_singleValue.Text = ""
        Else
            dtp_singleValue.Enabled = False
            txt_singleValue.Enabled = True
        End If
    End Sub
End Class
