Public Class frmDataTransfer
    Private globalvariables As clsGlobalVariables = clsGlobalVariables.getObject
    Private data_transfer As clsDataTransfer = clsDataTransfer.getObject
    Public objRef As clsformrefrences = clsformrefrences.getObject
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        Me.initialiseGlobalVariables()
    End Sub
    Friend Sub initialiseGlobalVariables()
        Dim DBcof As New clsfrmConfigureServer
        If Not System.IO.File.Exists("serverpath") Then
            DBcof.ShowDialog()
        End If
        Try
            Dim servername As String = readServerName()
            globalvariables.HRS_Main_DBname = "DSSHRS"
            globalvariables.HRS_Temp_DBname = "TEMP_DSSHRS"
            globalvariables.HRS_Main_DBCon.ConnectionString = "Data Source= " & servername & "; initial catalog=" + globalvariables.HRS_Main_DBname + "; integrated security=true"
            globalvariables.HRS_Temp_DBCon.ConnectionString = "Data Source= " & servername & "; initial catalog=" + globalvariables.HRS_Temp_DBname + "; integrated security=true"
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            MsgBox(ex.Message)
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
    End Sub
    Private Function readServerName() As String
        Dim server As String = ""
        Dim fileContents As String
        Try
            Dim freader As System.IO.StreamReader
            freader = System.IO.File.OpenText("serverpath")
            fileContents = freader.ReadLine()
            If fileContents.Trim.Length < 1 Then
                server = ""
            Else
                server = fileContents
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return server
    End Function
    Private Sub btn_Validate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Validate.Click
        Me.ProgressBar1.Style = ProgressBarStyle.Marquee
        Me.BackgroundWorker1.RunWorkerAsync()
    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        'Try
        'Me.data_validator.do_ALL_dataValidations()
        'Me.res.validateResidency(Me.db.getTablefromTempDb("select * from dss.Residency where rec_status in ('i','u')"), "dss.Residency")
        ' Me.mem.validateMembership(Me.db.getTablefromTempDb("select * from dss.membership where rec_status in ('i','u')"), "dss.membership")
        'Me.preg.validatePregnancy(Me.db.getTablefromTempDb("select * from dss.Pregnancy where rec_status in ('i','u')"), "dss.Pregnancy")

        Me.data_transfer.uploadDatatoMainDb()
        'Catch ex As Exception
        '    MsgBox(ex.Message)
        'End Try
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Me.ProgressBar1.Style = ProgressBarStyle.Blocks
        MsgBox(" work completed", MsgBoxStyle.Information)
    End Sub
End Class
