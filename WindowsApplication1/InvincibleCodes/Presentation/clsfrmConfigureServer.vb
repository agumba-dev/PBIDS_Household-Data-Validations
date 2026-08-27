Imports System.Data
Imports System.Windows.Forms
Imports Microsoft.SqlServer.Management.Smo
'Imports Microsoft.SqlServer.Management.Smo.Urn

Public Class clsfrmConfigureServer
    Public objRef As clsformrefrences = clsformrefrences.getObject
    Dim success As Boolean = False
    Dim myservername As String
    Dim dt As DataTable


    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        If success Then
            If (Me.txt_MainDB.Text.Trim <> "") Or (Me.txt_TempDB.Text.Trim <> "") Then
                Me.saveconfiguration()
            Else
                MsgBox("Enter Database names", MsgBoxStyle.Exclamation)
            End If

        Else
            MsgBox("connection is not available. Cannot save the configuration settings", MsgBoxStyle.Critical)
        End If
        'Me.Close()
        Me.Visible = False
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub findserversButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles findserversButton.Click
        findserversButton.Enabled = False
        BackgroundWorker1.RunWorkerAsync()
    End Sub
    Private Function allservers2() As DataTable
        Dim dtlSQLServers As DataTable
        ' Get list of all available servers.
        dtlSQLServers = SmoApplication.EnumAvailableSqlServers(False)
        Return dtlSQLServers
    End Function

    Private Sub configureServer_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        If Not System.IO.File.Exists("serverpath") Then
            frmDataTransfer.Close()
        End If
    End Sub


    Private Sub testConnectionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles testConnectionButton.Click
        success = False
        Try
            Dim servername As String = serversComboBox2.Text
            '  MsgBox(servername)
            Dim myServer As New Server(servername)
            If CheckBox1.Checked Then
                myServer.ConnectionContext.LoginSecure = False
                myServer.ConnectionContext.Password = passwordTextBox.Text
                myServer.ConnectionContext.Login = userNameTextBox.Text
            End If


            MsgBox("Connection Successful to " & myServer.Information.NetName, MsgBoxStyle.Information)
            myservername = myServer.Name

            success = True
            OK_Button.Enabled = success
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            MsgBox("connection Failed: " & ex.Message, MsgBoxStyle.Critical)
            'emailErrors("File Name: " & strObjFileName & vbCrLf & "Method Name: " & strObjFileName & vbCrLf & "Error Message: " & ex.Message)
        End Try
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        userNameTextBox.Enabled = CheckBox1.Checked
        passwordTextBox.Enabled = CheckBox1.Checked
    End Sub

    Sub saveconfiguration()
        'saving server name to a file where it can be accessed
        Try

            Dim fwriter As System.IO.StreamWriter
            fwriter = System.IO.File.CreateText("serverpath")
            System.IO.File.SetAttributes("serverpath", IO.FileAttributes.Hidden)
            fwriter.WriteLine(myservername + "," + Me.txt_MainDB.Text.Trim + "," + Me.txt_TempDB.Text.Trim)
            fwriter.Flush()
            fwriter = System.IO.File.CreateText("runSetup")
            System.IO.File.SetAttributes("runSetup", IO.FileAttributes.Hidden)
            fwriter.WriteLine("off")
            fwriter.Flush()
            fwriter.Close()
            If MsgBox("Settings saved successfully, " & vbNewLine & "Application will have to close and be reopened to effect changes, Continue? ", MsgBoxStyle.YesNo + MsgBoxStyle.Question) = MsgBoxResult.Yes Then
                Application.Restart()
            Else
                MsgBox("Changes have not been effected, manually close and reopen application for the changes to tale effect", MsgBoxStyle.Information)
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            MsgBox("Failed to save: " & ex.Message, MsgBoxStyle.Critical)
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        'gets the list of all available sql servers (needs sql server 2000 sp2 and above to work... i think!)
        dt = New DataTable
        dt = allservers2()
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Dim i As Integer = 0
        serversComboBox2.Items.Clear()
        serversComboBox2.Items.Add("(local)")
        For Each drServer As DataRow In dt.Rows
            If drServer("IsLocal") = True Then
                serversComboBox2.Items.Add(drServer("Name"))
                serversComboBox2.SelectedItem = drServer("Name")
            Else
                serversComboBox2.Items.Add(drServer("Name"))
            End If
        Next
        findserversButton.Enabled = True
    End Sub

    Private Sub configureServer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
