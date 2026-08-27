Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.ServiceProcess
Imports System.Threading
Imports System.Windows.Forms

<Assembly: AssemblyTrademark("")>
<Assembly: ComVisible(False)>
<Assembly: Guid("59c15a68-f4dd-4c75-bdbf-04d0c4fb747c")>

' This project intentionally keeps the Windows Service bootstrap in an existing
' source file so the application can run either interactively or under the
' Windows Service Control Manager without adding another project/file.
Friend Module Program
    Private Const ServiceNameValue As String = "PBIDSHouseholdDataValidator"
    Friend Property IsServiceMode As Boolean = False

    <STAThread>
    Public Sub Main()
        Environment.CurrentDirectory = AppContext.BaseDirectory
        IsServiceMode = Not Environment.UserInteractive

        If Not IsServiceMode Then
            Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)
            Application.Run(New frm_ToValidateForm())
            Return
        End If

        ServiceBase.Run(New ServiceBase() {New PBIDSValidatorService()})
    End Sub

    Friend Function FindLatestValidationLog() As String
        Try
            Dim files = Directory.GetFiles(AppContext.BaseDirectory, "autorunLog_*")
            If files.Length = 0 Then Return ""

            Array.Sort(files, Function(a, b) File.GetLastWriteTimeUtc(b).CompareTo(File.GetLastWriteTimeUtc(a)))
            Return files(0)
        Catch
            Return ""
        End Try
    End Function

    Friend Function ReadServerName() As String
        Dim serverFile = Path.Combine(AppContext.BaseDirectory, "serverpath")
        If Not File.Exists(serverFile) Then Return ""
        Return File.ReadAllText(serverFile).Trim()
    End Function

    Friend Function GetNotificationRecipients() As String
        Dim serverName = ReadServerName()
        If String.IsNullOrWhiteSpace(serverName) Then Return ""

        Dim recipients As New List(Of String)()
        Dim seen As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        Dim connectionString = "Data Source=" & serverName & ";Initial Catalog=dataChecker;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=30"

        Using con As New SqlConnection(connectionString)
            con.Open()
            Using cmd As New SqlCommand("SELECT emails FROM dbo.contact_notifications WHERE NULLIF(LTRIM(RTRIM(emails)), '') IS NOT NULL ORDER BY id", con)
                Using rdr = cmd.ExecuteReader()
                    While rdr.Read()
                        Dim raw = rdr.GetValue(0).ToString()
                        For Each address In raw.Split(New Char() {";"c, ","c}, StringSplitOptions.RemoveEmptyEntries)
                            Dim email = address.Trim()
                            If email.Length > 0 AndAlso seen.Add(email) Then recipients.Add(email)
                        Next
                    End While
                End Using
            End Using
        End Using

        Return String.Join(";", recipients)
    End Function

    Friend Sub SendDatabaseMail(subject As String, body As String, Optional attachmentPath As String = "")
        Try
            Dim serverName = ReadServerName()
            Dim recipients = GetNotificationRecipients()
            If String.IsNullOrWhiteSpace(serverName) OrElse String.IsNullOrWhiteSpace(recipients) Then Return

            Dim connectionString = "Data Source=" & serverName & ";Initial Catalog=msdb;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=30"
            Using con As New SqlConnection(connectionString)
                con.Open()
                Using cmd As New SqlCommand("msdb.dbo.sp_send_dbmail", con)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.CommandTimeout = 60
                    cmd.Parameters.AddWithValue("@profile_name", "dsssrv")
                    cmd.Parameters.AddWithValue("@recipients", recipients)
                    cmd.Parameters.AddWithValue("@subject", subject)
                    cmd.Parameters.AddWithValue("@body", body)
                    cmd.Parameters.AddWithValue("@body_format", "TEXT")

                    If Not String.IsNullOrWhiteSpace(attachmentPath) AndAlso File.Exists(attachmentPath) Then
                        cmd.Parameters.AddWithValue("@file_attachments", attachmentPath)
                    End If

                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch
            ' Mail failure must never terminate validation processing.
        End Try
    End Sub
End Module

Friend NotInheritable Class PBIDSValidatorService
    Inherits ServiceBase

    Private serviceThread As Thread
    Private stopRequested As Boolean

    Public Sub New()
        ServiceName = ServiceNameValue
        CanStop = True
        CanPauseAndContinue = False
        AutoLog = True
    End Sub

    Protected Overrides Sub OnStart(args() As String)
        stopRequested = False
        serviceThread = New Thread(AddressOf RunValidation)
        serviceThread.IsBackground = True
        serviceThread.Name = "PBIDS Household Validator"
        serviceThread.SetApartmentState(ApartmentState.STA)
        serviceThread.Start()
    End Sub

    Protected Overrides Sub OnStop()
        stopRequested = True
    End Sub

    Private Sub RunValidation()
        Dim startedAt = DateTime.Now
        Try
            Environment.CurrentDirectory = AppContext.BaseDirectory
            Program.SendDatabaseMail(
                "PBIDS Household Data Validation - STARTED",
                "The PBIDS household data validation service started at " & startedAt.ToString("yyyy-MM-dd HH:mm:ss") & ".")

            If stopRequested Then Return

            Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)

            ' The existing form remains the validation engine. In unattended mode
            ' runSetup=on starts the BackgroundWorker automatically and the existing
            ' completion handler closes the application when the batch finishes.
            Using validator As New frm_ToValidateForm()
                validator.ShowInTaskbar = False
                validator.WindowState = FormWindowState.Minimized
                Application.Run(validator)
            End Using

            Dim finishedAt = DateTime.Now
            Dim logFile = Program.FindLatestValidationLog()
            Program.SendDatabaseMail(
                "PBIDS Household Data Validation - COMPLETED",
                "The PBIDS household data validation service completed at " & finishedAt.ToString("yyyy-MM-dd HH:mm:ss") &
                "." & Environment.NewLine & "Started: " & startedAt.ToString("yyyy-MM-dd HH:mm:ss") &
                Environment.NewLine & "Duration: " & (finishedAt - startedAt).ToString(),
                logFile)
        Catch ex As Exception
            Dim logFile = Program.FindLatestValidationLog()
            Program.SendDatabaseMail(
                "PBIDS Household Data Validation - FAILED",
                "The PBIDS household data validation service failed at " & DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") &
                "." & Environment.NewLine & Environment.NewLine & ex.ToString(),
                logFile)
        Finally
            Try
                Me.Stop()
            Catch
            End Try
        End Try
    End Sub
End Class