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

' Compatibility for the legacy DgvFilterPopup component. The validator no longer
' requires the old external DLL; the remaining editor code only assigns a grid.
Namespace Global.DgvFilterPopup
    Friend Class DgvFilterManager
        Public Property DataGridView As System.Windows.Forms.DataGridView
    End Class
End Namespace

' SqlProcedureAttribute existed in the .NET Framework SQL-CLR surface but is not
' supplied by the modern .NET runtime used by this desktop/service application.
Namespace Global.Microsoft.SqlServer.Server
    <AttributeUsage(AttributeTargets.Method, AllowMultiple:=False, Inherited:=False)>
    Public NotInheritable Class SqlProcedureAttribute
        Inherits Attribute
    End Class
End Namespace

' This project intentionally keeps the Windows Service bootstrap in an existing
' source file so the application can run either interactively or under the
' Windows Service Control Manager without adding another project/file.
Friend Module Program
    Friend Const ServiceNameValue As String = "PBIDSHouseholdDataValidator"
    Friend Const ServiceRoot As String = "C:\services"
    Friend Const ValidationLogRoot As String = "D:\Logs\ValidationLogs"
    Friend Property IsServiceMode As Boolean = False
    Friend Property LastErrorMessage As String = ""

    Private handlingError As Boolean = False

    Friend Function GetRuntimeFolder() As String
        If Directory.Exists(ServiceRoot) Then
            Return ServiceRoot
        End If

        Return AppContext.BaseDirectory
    End Function

    Friend Function GetValidationLogFolder() As String
        Return ValidationLogRoot
    End Function

    <STAThread>
    Public Sub Main()
        Environment.CurrentDirectory = GetRuntimeFolder()
        IsServiceMode = Not Environment.UserInteractive
        ConfigureExceptionHandling()

        If Not IsServiceMode Then
            Dim startupError As String = ""
            If Not ValidateRuntimeConfiguration(startupError) Then
                LastErrorMessage = startupError
                MessageBox.Show(startupError,
                                "PBIDS Data Validator - Startup Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error)
                Return
            End If

            Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)
            Application.Run(New frm_ToValidateForm())
            ArchiveLatestValidationLog()
            Return
        End If

        ServiceBase.Run(New ServiceBase() {New PBIDSValidatorService()})
    End Sub

    Private Sub ConfigureExceptionHandling()
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException)
        AddHandler Application.ThreadException, AddressOf ApplicationThreadException
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf CurrentDomainUnhandledException
    End Sub

    Private Sub ApplicationThreadException(sender As Object, e As ThreadExceptionEventArgs)
        ReportApplicationError("An unexpected application error occurred.", e.Exception)

        If IsServiceMode Then
            Application.ExitThread()
        Else
            Application.Exit()
        End If
    End Sub

    Private Sub CurrentDomainUnhandledException(sender As Object, e As UnhandledExceptionEventArgs)
        Dim ex = TryCast(e.ExceptionObject, Exception)
        If ex Is Nothing Then
            ex = New Exception("An unknown unhandled error occurred.")
        End If

        ReportApplicationError("An unexpected background error occurred.", ex)

        If IsServiceMode Then
            Dim logFile = ArchiveLatestValidationLog()
            SendDatabaseMail(
                "PBIDS Household Data Validation - FAILED",
                LastErrorMessage,
                logFile)
        End If
    End Sub

    Friend Sub ReportApplicationError(context As String, ex As Exception)
        If handlingError Then Return
        handlingError = True

        Try
            Dim message As String = context

            If ex IsNot Nothing Then
                message &= Environment.NewLine & Environment.NewLine &
                           ex.GetType().Name & ": " &
                           If(String.IsNullOrWhiteSpace(ex.Message),
                              "No additional error details were provided.",
                              ex.Message)

                If Not String.IsNullOrWhiteSpace(ex.StackTrace) Then
                    message &= Environment.NewLine & Environment.NewLine & ex.StackTrace
                End If
            End If

            LastErrorMessage = message

            If Not IsServiceMode Then
                MessageBox.Show(message,
                                "PBIDS Data Validator Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error)
            End If
        Finally
            handlingError = False
        End Try
    End Sub

    Friend Function ValidateRuntimeConfiguration(ByRef errorMessage As String) As Boolean
        errorMessage = ""

        Dim serverFile = Path.Combine(GetRuntimeFolder(), "serverpath")
        If Not File.Exists(serverFile) Then
            errorMessage = "The SQL Server configuration file was not found." &
                           Environment.NewLine & Environment.NewLine &
                           "Expected file:" & Environment.NewLine & serverFile &
                           Environment.NewLine & Environment.NewLine &
                           "The validator expects the server configuration at C:\services\serverpath."
            Return False
        End If

        Dim serverName = ReadServerName()
        If String.IsNullOrWhiteSpace(serverName) Then
            errorMessage = "The serverpath file is empty." &
                           Environment.NewLine & Environment.NewLine &
                           "File:" & Environment.NewLine & serverFile &
                           Environment.NewLine & Environment.NewLine &
                           "Put the SQL Server name on the first line."
            Return False
        End If

        Try
            Directory.CreateDirectory(GetValidationLogFolder())
        Catch ex As Exception
            errorMessage = "Unable to access the validation log folder." &
                           Environment.NewLine & Environment.NewLine &
                           "Folder: " & GetValidationLogFolder() &
                           Environment.NewLine & Environment.NewLine &
                           ex.GetType().Name & ": " & ex.Message
            Return False
        End Try

        Try
            Dim connectionString = "Data Source=" & serverName &
                                   ";Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=15"

            Using con As New SqlConnection(connectionString)
                con.Open()

                Dim requiredDatabases As String() = {"TEMP_DSSHRS", "DSSHRS", "dataChecker"}
                Dim missing As New List(Of String)()

                For Each databaseName In requiredDatabases
                    Using cmd As New SqlCommand("SELECT DB_ID(@databaseName)", con)
                        cmd.Parameters.AddWithValue("@databaseName", databaseName)
                        Dim result = cmd.ExecuteScalar()

                        If result Is Nothing OrElse result Is DBNull.Value Then
                            missing.Add(databaseName)
                        End If
                    End Using
                Next

                If missing.Count > 0 Then
                    errorMessage = "SQL Server connection succeeded, but the following required database(s) were not found on server '" &
                                   serverName & "':" & Environment.NewLine & Environment.NewLine &
                                   String.Join(Environment.NewLine, missing)
                    Return False
                End If
            End Using
        Catch ex As Exception
            errorMessage = "Unable to initialize the PBIDS Data Validator against SQL Server '" & serverName & "'." &
                           Environment.NewLine & Environment.NewLine &
                           ex.GetType().Name & ": " & ex.Message
            Return False
        End Try

        Return True
    End Function

    Friend Function FindLatestValidationLog() As String
        Try
            Directory.CreateDirectory(GetValidationLogFolder())

            Dim files = Directory.GetFiles(GetValidationLogFolder(), "autorunLog_*")
            If files.Length > 0 Then
                Array.Sort(files, Function(a, b) File.GetLastWriteTimeUtc(b).CompareTo(File.GetLastWriteTimeUtc(a)))
                Return files(0)
            End If

            files = Directory.GetFiles(GetRuntimeFolder(), "autorunLog_*")
            If files.Length = 0 Then Return ""

            Array.Sort(files, Function(a, b) File.GetLastWriteTimeUtc(b).CompareTo(File.GetLastWriteTimeUtc(a)))
            Return files(0)
        Catch
            Return ""
        End Try
    End Function

    Friend Function ArchiveLatestValidationLog() As String
        Try
            Directory.CreateDirectory(GetValidationLogFolder())

            Dim sourceFiles = Directory.GetFiles(GetRuntimeFolder(), "autorunLog_*")
            If sourceFiles.Length = 0 Then
                Return FindLatestValidationLog()
            End If

            Array.Sort(sourceFiles, Function(a, b) File.GetLastWriteTimeUtc(b).CompareTo(File.GetLastWriteTimeUtc(a)))
            Dim sourceFile = sourceFiles(0)
            Dim fileName = Path.GetFileName(sourceFile)

            If String.IsNullOrWhiteSpace(Path.GetExtension(fileName)) Then
                fileName &= ".txt"
            End If

            Dim destinationFile = Path.Combine(GetValidationLogFolder(), fileName)

            If File.Exists(destinationFile) Then
                destinationFile = Path.Combine(
                    GetValidationLogFolder(),
                    Path.GetFileNameWithoutExtension(fileName) & "_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & Path.GetExtension(fileName))
            End If

            File.Move(sourceFile, destinationFile)
            Return destinationFile
        Catch
            Return FindLatestValidationLog()
        End Try
    End Function

    Friend Function ReadServerName() As String
        Try
            Dim serverFile = Path.Combine(GetRuntimeFolder(), "serverpath")
            If Not File.Exists(serverFile) Then Return ""

            Dim serverName = File.ReadAllText(serverFile)
            If serverName Is Nothing Then Return ""

            Return serverName.Trim()
        Catch
            Return ""
        End Try
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
        ServiceName = Program.ServiceNameValue
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
        Program.LastErrorMessage = ""

        Try
            Environment.CurrentDirectory = Program.GetRuntimeFolder()

            Dim startupError As String = ""
            If Not Program.ValidateRuntimeConfiguration(startupError) Then
                Program.LastErrorMessage = startupError
                Program.SendDatabaseMail(
                    "PBIDS Household Data Validation - FAILED",
                    startupError)
                Return
            End If

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
            Dim logFile = Program.ArchiveLatestValidationLog()

            If Not String.IsNullOrWhiteSpace(Program.LastErrorMessage) Then
                Program.SendDatabaseMail(
                    "PBIDS Household Data Validation - FAILED",
                    "The PBIDS household data validation service failed at " & finishedAt.ToString("yyyy-MM-dd HH:mm:ss") &
                    "." & Environment.NewLine & Environment.NewLine & Program.LastErrorMessage,
                    logFile)
                Return
            End If

            Program.SendDatabaseMail(
                "PBIDS Household Data Validation - COMPLETED",
                "The PBIDS household data validation service completed at " & finishedAt.ToString("yyyy-MM-dd HH:mm:ss") &
                "." & Environment.NewLine & "Started: " & startedAt.ToString("yyyy-MM-dd HH:mm:ss") &
                Environment.NewLine & "Duration: " & (finishedAt - startedAt).ToString(),
                logFile)
        Catch ex As Exception
            Program.ReportApplicationError("The PBIDS household validation service failed.", ex)

            Dim logFile = Program.ArchiveLatestValidationLog()
            Program.SendDatabaseMail(
                "PBIDS Household Data Validation - FAILED",
                Program.LastErrorMessage,
                logFile)
        Finally
            Try
                Me.Stop()
            Catch
            End Try
        End Try
    End Sub
End Class