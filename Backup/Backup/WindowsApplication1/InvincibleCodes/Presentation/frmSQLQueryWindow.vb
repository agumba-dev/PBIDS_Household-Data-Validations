Imports Microsoft.SqlServer.Management.Smo
Imports System.Data.SqlClient
Imports DgvFilterPopup
Public Class frmSQLQueryWindow
    Public myServer As Server
    Public mytable As Table
    Private currentTable As Table
    Friend mydatabase As Database
    Friend mycon As SqlConnection
    Dim dtlSQLServers As DataTable
    Dim ds As New DataSet()
    Dim da As SqlDataAdapter
    Friend bsource As New BindingSource()
    Dim filt As New DgvFilterManager()
    Public dacc As clsDataAccess = clsDataAccess.getObject
    Dim data_transfer As clsDataTransfer = clsDataTransfer.getObject

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
    Friend Sub setConnection()
        Dim sqlServerConStr As String = "Data Source= " & myServer.Name & "; initial catalog= " & mydatabase.Name & "; integrated security=true"
        mycon = New SqlConnection(sqlServerConStr)

    End Sub

    Private Sub getData(ByVal sql As String)
        Try
            mycon.Open()
            da = New SqlDataAdapter(sql, mycon)
            mycon.Close()
            ds.Tables.Clear()
            da.Fill(ds, "Table")
            bsource.DataSource = Nothing
            bsource.DataSource = ds
            bsource.DataMember = "Table"
            dgv_records.DataSource = bsource
            currentTable = mytable
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub cmb_chooseTable_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmb_chooseTable.SelectedIndexChanged
        Me.Cursor = Cursors.WaitCursor
        mytable = mydatabase.Tables.Item(CType(cmb_chooseTable.SelectedItem, Table).Name, CType(cmb_chooseTable.SelectedItem, Table).Schema)
        CBFiledDesc1.Items.Clear()
        For Each clmn As Column In mytable.Columns
            CBFiledDesc1.Items.Add(clmn.Name)
        Next
      
        Me.Cursor = Cursors.Default
    End Sub

   

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Control.CheckForIllegalCrossThreadCalls = False
        Dim sql As String
        If Me.txt_SQLQUERY.SelectedText.Trim = "" Then
            sql = Me.txt_SQLQUERY.Text.Trim.ToLower
        Else
            sql = Me.txt_SQLQUERY.SelectedText.Trim.ToLower
        End If
        If sql.Contains(" delete ") Or sql.Contains(" update ") _
            Or sql.StartsWith("delete") Or sql.StartsWith("update") _
             Or sql.EndsWith(" delete") Or sql.EndsWith(" update") _
            Then
            MsgBox("delete or update sql statements not allowed in this window", MsgBoxStyle.Exclamation)
        Else
            Me.getData(sql)
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        dgv_records.DataSource = Nothing
        If Not Me.BackgroundWorker1.IsBusy Then
            Me.BackgroundWorker1.RunWorkerAsync()
        Else
            MsgBox("Still processing another query")
        End If
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Me.TabControl1.SelectedTab = Me.TabPage2
    End Sub
End Class