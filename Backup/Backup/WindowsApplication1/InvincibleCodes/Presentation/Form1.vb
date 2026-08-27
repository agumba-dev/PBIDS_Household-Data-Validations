Public Class Form1
    Private globalvariables As clsGlobalVariables = clsGlobalVariables.getObject
    Public objRef As clsformrefrences = clsformrefrences.getObject

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Try
            globalvariables.initialiseGlobalVariables()
            Me.autorunTest()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub TheInvincibleToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TheInvincibleToolStripMenuItem.Click
        Dim frm As New frm_ToValidateForm
        frm.ShowDialog()
    End Sub
    Private Sub autorunTest()
        If getrunSetup().Trim.ToLower = "on" Then
            Dim frmVal As New frm_ToValidateForm
            frmVal.ShowDialog()
        End If
    End Sub
    Public Function getrunSetup() As String
        Dim strServerName As String = ""

        Try
            Dim freader As System.IO.StreamReader
            freader = System.IO.File.OpenText("runSetup")
            strServerName = freader.ReadLine()
            freader.Close()
        Catch ex As Exception
            Return ""
        End Try
        Return strServerName
    End Function

    Private Sub EditValidationsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditValidationsToolStripMenuItem.Click
        Dim frm As New Frm_TheValidationsEditor
        frm.ShowDialog()
    End Sub

    Private Sub DataEditorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataEditorToolStripMenuItem.Click
        Dim frm As New frmDBEditor
        frm.ShowDialog()
    End Sub

    Private Sub EditConfigToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditConfigToolStripMenuItem.Click
        Dim frm As New frm_EditConfigurations
        frm.ShowDialog()
    End Sub

    Private Sub RefValidationsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefValidationsToolStripMenuItem.Click
        Dim frm As New frmValidationMgmt
        frm.ShowDialog()
    End Sub

    Private Sub SqlWindowToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SqlWindowToolStripMenuItem.Click
        Dim frm As New frmSQLQueryWindow
        frm.ShowDialog()
    End Sub

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
