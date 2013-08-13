
Imports Microsoft.SqlServer.Management.Smo
Imports System.Data.SqlClient
Imports DgvFilterPopup


Public Class frm_EditConfigurations
    'Public conStr As String

#Region "Declarations"
    'Dim con As New SqlConnection(sConnection)
    Dim DBname As Integer
    Dim table As Integer
    Dim column_name As String
    Dim chkrec As Integer
    Dim sql As String
#End Region
#Region "functions"


#End Region



    Public myServer As Server
    Public mytable As Table
    Friend mydatabase As Database
    Friend mycon As SqlConnection
    Dim dtlSQLServers As DataTable
    Dim enablerowEnter As Boolean


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

        Me.Cursor = Cursors.Default

        findServersButton.Enabled = True
    End Sub

    Private Sub databasesComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles databasesComboBox.SelectedIndexChanged
        'get now the table names of a database  selected
        enablerowEnter = False
        Dim databasename As String = databasesComboBox.Text

        mydatabase = myServer.Databases.Item(databasename)
        cmb_chooseTable.Items.Clear()
        dgv_tables.Rows.Clear()
        setConnection()
        Dim editable As Boolean
        For Each tble As Table In mydatabase.Tables


            ' get only tables that are user defined
            'If Not tble.IsSystemObject() Then tableNameListBox.Items.Add(tble)
            If Not tble.IsSystemObject() Then
                cmb_chooseTable.Items.Add(tble)
                editable = doesTableExist(databasesComboBox.Text, "[" & tble.Schema & "].[" & tble.Name & "]")
                Dim myrow As Object() = {tble, editable}
                dgv_tables.Rows.Add(myrow)
            End If


        Next
        enablerowEnter = True

    End Sub


    Private Sub cmb_chooseTable_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmb_chooseTable.SelectedIndexChanged
        mytable = mydatabase.Tables.Item(CType(cmb_chooseTable.SelectedItem, Table).Name, CType(cmb_chooseTable.SelectedItem, Table).Schema)
        getTableCLmnSettings()

    End Sub
    Private Sub getTableCLmnSettings()
        Me.Cursor = Cursors.WaitCursor




        'Dim x As Integer
        dgv_records.Rows.Clear()
        Dim editable As Boolean

        For Each clmn As Column In mytable.Columns
            '    editable = doescolumnExist("[" & mytable.Schema & "].[" & mytable.Name & "]", cmb_chooseTable.Text, clmn.Name)

            editable = doescolumnExist(databasesComboBox.Text, "[" & mytable.Schema & "].[" & mytable.Name & "]", clmn.Name)
            Dim myrow As Object() = {clmn.Name, editable}
            dgv_records.Rows.Add(myrow)

            For Each r As DataGridViewRow In dgv_records.Rows
                If r.Cells(1).Value = True Then



                    'check if already there if not then
                    'save that record
                Else
                    'delete
                End If
            Next

        Next

        Me.Cursor = Cursors.Default
    End Sub


    Private Sub btnconnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnconnect.Click
        'we want to now get the databases in the server selected
        Try
            Dim servername As String = serversComboBox.Text

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

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Call setConnection()
        
        'Dim con As New SqlConnection
        Dim cmd As New SqlCommand

        'Dim ID = Me.dgv_records.CurrentRow.Cells("& r.Cells(0).Value & ").Value
        If mycon.State <> ConnectionState.Open Then mycon.Open()
        cmd.Connection = mycon
        For Each r As DataGridViewRow In dgv_records.Rows
            If r.Cells(1).Value = True Then
                Try

                    cmd.CommandText = "If Not exists(SELECT column_name FROM [dataChecker].dbo.dbconfiguration WHERE column_name = '" & r.Cells(0).Value _
                     & "' AND table_name='[" & mytable.Schema & "].[" & mytable.Name & "]' AND DBname='" & databasesComboBox.Text & "' )" _
                                                    & "INSERT INTO [dataChecker].dbo.dbconfiguration (DBname,table_name,column_name)" _
                                                              & " VALUES(@DBname,@table_name,@column_name)"
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@DBname", databasesComboBox.Text)
                    cmd.Parameters.AddWithValue("@table_name", "[" & mytable.Schema & "].[" & mytable.Name & "]")
                    cmd.Parameters.AddWithValue("@column_name", r.Cells(0).Value)


                    cmd.ExecuteNonQuery()
                Catch ex As Exception
                    MsgBox(ex.Message)
                Finally
                    'Me.closeCon()
                End Try

                ' mycon.Close()
                'check if already there if not then
                'save that record
            Else
                cmd.CommandText = "DELETE FROM [dataChecker].[dbo].[dbconfiguration]WHERE column_name = '" & r.Cells(0).Value _
                     & "' AND table_name='[" & mytable.Schema & "].[" & mytable.Name & "]' AND DBname='" & databasesComboBox.Text & "'"

                cmd.ExecuteNonQuery()
                'delete
            End If
        Next

        For Each r As DataGridViewRow In dgv_tables.Rows
            If r.Cells(1).Value = True Then

                Try

                    cmd.CommandText = "If Not exists(SELECT table_name FROM [dataChecker].dbo.EditableTables WHERE table_name = '" & r.Cells(0).Value.ToString _
                     & "' AND DBname='" & databasesComboBox.Text & "' )" _
                                                    & "INSERT INTO [dataChecker].dbo.EditableTables (DBname,table_name)" _
                                                              & " VALUES(@DBname,@table_name)"
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@DBname", databasesComboBox.Text)
                    cmd.Parameters.AddWithValue("@table_name", r.Cells(0).Value.ToString)

                    cmd.ExecuteNonQuery()
                Catch ex As Exception
                    MsgBox(ex.Message)
                Finally
                    'Me.closeCon()
                End Try

                ' mycon.Close()
                'check if already there if not then
                'save that record
            Else
                cmd.CommandText = "DELETE FROM [dataChecker].[dbo].[EditableTables]WHERE table_name = '" & r.Cells(0).Value.ToString _
                     & "' AND DBname='" & databasesComboBox.Text & "'"

                cmd.ExecuteNonQuery()
                'delete
            End If
        Next
        MsgBox("New entry(s) made succesfully. ", MsgBoxStyle.Information, "Succesful Entry")


    End Sub

    Private Sub dgv_records_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_records.CellContentClick

    End Sub
 
    Private Sub frm_EditConfigurations_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
    Private Function doescolumnExist(ByVal DBname As String, ByVal table_name As String, ByVal column_name As String) As Boolean
        Dim mycommand As New SqlCommand
        If mycon.State <> ConnectionState.Open Then mycon.Open()
        Try
            mycommand.Connection = mycon

            mycommand.CommandText = "SELECT COUNT(*) FROM [dataChecker].[dbo].[dbconfiguration]  WHERE column_name = '" & column_name & "' and table_name='" & table_name & "' and dbname='" & DBname & "'"

            Dim I As Integer = mycommand.ExecuteScalar

            If I > 0 Then
                doescolumnExist = True

                'MsgBox("found column", MsgBoxStyle.Critical, "problog")
            Else
                doescolumnExist = False

            End If
        Catch ex As Exception
            MsgBox("Failed loading values" + ex.Message, MsgBoxStyle.Critical)
        End Try
        mycon.Close()
    End Function

    Private Function doesTableExist(ByVal DBname As String, ByVal table_name As String) As Boolean
        Dim mycommand As New SqlCommand
        If mycon.State <> ConnectionState.Open Then mycon.Open()
        Try
            mycommand.Connection = mycon

            mycommand.CommandText = "SELECT COUNT(*) FROM [dataChecker].[dbo].[EditableTables]  WHERE table_name='" & table_name & "' and DBname='" & DBname & "'"

            Dim I As Integer = mycommand.ExecuteScalar

            If I > 0 Then
                doesTableExist = True

                'MsgBox("found column", MsgBoxStyle.Critical, "problog")
            Else
                doesTableExist = False

            End If
        Catch ex As Exception
            MsgBox("Failed loading values" + ex.Message, MsgBoxStyle.Critical)
        End Try
        mycon.Close()
    End Function

    Private Sub serversComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles serversComboBox.SelectedIndexChanged

    End Sub

    Private Sub dgv_tables_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles dgv_tables.CellBeginEdit
     
    End Sub

    Private Sub dgv_tables_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_tables.CellContentClick
        If Not enablerowEnter Then Return
        If dgv_tables.Rows(e.RowIndex).Cells(1).EditedFormattedValue = True Then
            mytable = mydatabase.Tables.Item(CType(dgv_tables.Rows(e.RowIndex).Cells(0).Value, Table).Name, CType(dgv_tables.Rows(e.RowIndex).Cells(0).Value, Table).Schema)
            getTableCLmnSettings()
        Else
            dgv_records.Rows.Clear()
        End If
    End Sub

    Private Sub dgv_tables_CellValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) Handles dgv_tables.CellValidating

    End Sub

    Private Sub dgv_tables_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_tables.CellValueChanged

    End Sub


    Private Sub dgv_tables_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv_tables.RowEnter
        If Not enablerowEnter Then Return

        If dgv_tables.Rows(e.RowIndex).Cells(1).Value = True Then
            mytable = mydatabase.Tables.Item(CType(dgv_tables.Rows(e.RowIndex).Cells(0).Value, Table).Name, CType(dgv_tables.Rows(e.RowIndex).Cells(0).Value, Table).Schema)
            getTableCLmnSettings()
        Else
            dgv_records.Rows.Clear()
        End If
    End Sub
End Class
