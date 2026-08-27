
Imports DgvFilterPopup
Imports Microsoft.EntityFrameworkCore.Metadata.Internal
Imports Microsoft.SqlServer
Imports Microsoft.SqlServer.Management.Smo
Imports System.Data
Imports System.Data.SqlClient
Imports Smo = Microsoft.SqlServer.Management.Smo

Public Class frmSQLQueryWindow
    Public myServer As Server
    Public mytable As Smo.Table
    Private currentTable As Smo.Table
    Friend mydatabase As Database
    Friend mycon As SqlConnection
    Dim dtlSQLServers As DataTable
    Dim ds As New DataSet()
    Dim da As SqlDataAdapter
    Friend bsource As New BindingSource()
    Dim filt As New DgvFilterManager()
    Public dacc As clsDataAccess = clsDataAccess.getObject
    Dim data_transfer As clsDataTransfer = clsDataTransfer.getObject

    Private Sub btnconnect_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnconnect.Click
        ConnectToServer(serversComboBox.Text)
    End Sub

    Public Sub ConnectToServer(ByVal servername As String)
        Try
            myServer = New Server(servername)
            servername = myServer.Name
            databasesComboBox.Items.Clear()

            For Each db As Database In myServer.Databases
                If Not db.IsSystemObject Then databasesComboBox.Items.Add(db.Name)
            Next
        Catch ex As Exception
            MsgBox($"Connection Failed: {ex.Message}", MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub findServersButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles findServersButton.Click
        Me.Cursor = Cursors.WaitCursor

        Try
            dtlSQLServers = SmoApplication.EnumAvailableSqlServers(False)
            serversComboBox.Items.Clear()
            serversComboBox.Items.Add("(local)")

            For Each drServer As DataRow In dtlSQLServers.Rows
                serversComboBox.Items.Add(drServer("Name"))
                If drServer("IsLocal").Equals(True) Then
                    serversComboBox.SelectedItem = drServer("Name")
                End If
            Next
        Catch ex As Exception
            MsgBox($"Error finding servers: {ex.Message}", MsgBoxStyle.Critical)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub databasesComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles databasesComboBox.SelectedIndexChanged
        Dim databasename As String = databasesComboBox.Text
        mydatabase = myServer.Databases.Item(databasename)
        cmb_chooseTable.Items.Clear()

        For Each tble As Table In mydatabase.Tables
            If Not tble.IsSystemObject Then cmb_chooseTable.Items.Add(tble)
        Next

        SetConnection()
    End Sub

    Friend Sub SetConnection()
        Dim sqlServerConStr As String = $"Data Source={myServer.Name}; Initial Catalog={mydatabase.Name}; Integrated Security=True"
        mycon = New SqlConnection(sqlServerConStr)
    End Sub

    Private Sub GetData(ByVal sql As String)
        Try
            mycon.Open()
            da = New SqlDataAdapter(sql, mycon)
            ds.Tables.Clear()
            da.Fill(ds, "Table")
            bsource.DataSource = ds
            bsource.DataMember = "Table"
            dgv_records.DataSource = bsource
            currentTable = mytable
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            If mycon.State = ConnectionState.Open Then
                mycon.Close()
            End If
        End Try
    End Sub

    Private Sub cmb_chooseTable_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmb_chooseTable.SelectedIndexChanged
        Me.Cursor = Cursors.WaitCursor
        Try
            mytable = mydatabase.Tables.Item(CType(cmb_chooseTable.SelectedItem, Table).Name, CType(cmb_chooseTable.SelectedItem, Table).Schema)
            CBFiledDesc1.Items.Clear()

            For Each clmn As Column In mytable.Columns
                CBFiledDesc1.Items.Add(clmn.Name)
            Next
        Catch ex As Exception
            MsgBox($"Error loading columns: {ex.Message}", MsgBoxStyle.Critical)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Control.CheckForIllegalCrossThreadCalls = False
        Dim sql As String = If(String.IsNullOrWhiteSpace(Me.txt_SQLQUERY.SelectedText.Trim), Me.txt_SQLQUERY.Text.Trim.ToLower, Me.txt_SQLQUERY.SelectedText.Trim.ToLower)

        If sql.Contains(" delete ") OrElse sql.Contains(" update ") OrElse sql.StartsWith("delete") OrElse sql.StartsWith("update") OrElse sql.EndsWith(" delete") OrElse sql.EndsWith(" update") Then
            MsgBox("DELETE or UPDATE SQL statements are not allowed in this window", MsgBoxStyle.Exclamation)
        Else
            GetData(sql)
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        dgv_records.DataSource = Nothing
        If Not BackgroundWorker1.IsBusy Then
            BackgroundWorker1.RunWorkerAsync()
        Else
            MsgBox("Still processing another query")
        End If
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Me.TabControl1.SelectedTab = Me.TabPage2
    End Sub

End Class