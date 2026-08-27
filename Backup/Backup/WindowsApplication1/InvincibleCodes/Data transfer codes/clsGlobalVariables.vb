
Imports System.Data.SqlClient
Imports System.Data
Public Enum mhrsSyncDatabaseTypes
    sqlserver = 1
    sqlserverce = 2
    Access = 3
    Oracle = 4
End Enum
Public Enum ErrorConversion
    ExceptionMsg = 1
    StackTrace = 2
    ConnectionString = 3
    SQLCommand = 4
    OtherMessage = 5
End Enum
Public Enum mhrsSyncValidationTypes
    batchprocessing = 1
    triggers = 2
    userpplication = 3
End Enum
Public Class clsGlobalVariables
    'Do variable declaration here
#Region "Variable Declaration"
    Friend currentRound As String
    ' Friend sqlclientString As String
    Friend CurrentProgramUser As String
    Friend tempHRBfullDir As String
    Friend HRS_Temp_DBname As String
    Friend HRS_Main_DBname As String
    ' Friend Val_Current_Connection As New SqlConnection
    Friend HRS_Temp_DBCon As New SqlConnection
    Friend HRS_Main_DBCon As New SqlConnection
    Friend validationtype As mhrsSyncValidationTypes
    Private Shared objSingle As clsGlobalVariables
    Private Shared blCreated As Boolean
    Public objRef As clsformrefrences = clsformrefrences.getObject
    Public currectRecPrimarykeyValues As String = ""
    Public currectDBtoValidate As datalevel = datalevel.TEMP_DSSHRS
#End Region
#Region "Singleton function"
    Private Sub New()
        'Override the default constructor
    End Sub
    Public Shared Function getObject() As clsGlobalVariables
        If blCreated = False Then
            objSingle = New clsGlobalVariables
            blCreated = True

            Return objSingle
        Else
            Return objSingle
        End If
    End Function
#End Region
    'Do variable declaration here
#Region "Procedures and functions"

    'Friend Function open_Val_Current_Connection() As Boolean
    '    If validationtype <> mhrsSyncValidationTypes.triggers Then
    '        ' Me.close_Val_Current_Connection()
    '        If Me.Val_Current_Connection.State <> ConnectionState.Open Then
    '            Me.Val_Current_Connection.Open()
    '        End If
    '    End If
    'End Function
    'Friend Function close_Val_Current_Connection() As Boolean
    '    If validationtype <> mhrsSyncValidationTypes.triggers Then
    '        If Me.Val_Current_Connection.State <> ConnectionState.Closed Then
    '            Me.Val_Current_Connection.Close()
    '        End If
    '    End If

    'End Function
    Friend Function open_HRS_Main_DBCon() As Boolean
        If validationtype <> mhrsSyncValidationTypes.triggers Then
            'Me.close_HRS_Main_DBCon()
            If Me.HRS_Main_DBCon.State <> ConnectionState.Open Then
                Me.HRS_Main_DBCon.Open()
            End If
        End If

    End Function
    Friend Function close_HRS_Main_DBCon() As Boolean
        If validationtype <> mhrsSyncValidationTypes.triggers Then
            If Me.HRS_Main_DBCon.State <> ConnectionState.Closed Then
                Me.HRS_Main_DBCon.Close()
            End If
        End If
    End Function
    Friend Function open_HRS_TEMP_DBCon() As Boolean
        If validationtype <> mhrsSyncValidationTypes.triggers Then
            'Me.close_HRS_Main_DBCon()
            If Me.HRS_Temp_DBCon.State <> ConnectionState.Open Then
                Me.HRS_Temp_DBCon.Open()
            End If
        End If

    End Function
    Friend Function close_HRS_TEMP_DBCon() As Boolean
        If validationtype <> mhrsSyncValidationTypes.triggers Then
            If Me.HRS_Temp_DBCon.State <> ConnectionState.Closed Then
                Me.HRS_Temp_DBCon.Close()
            End If
        End If
    End Function
    Friend Sub initialiseGlobalVariables()
        Dim DBcof As New clsfrmConfigureServer
        If Not System.IO.File.Exists("serverpath") Then
            DBcof.ShowDialog()
        End If
        Try
            Dim servername As String = readServerName()
            Me.HRS_Main_DBname = "DSSHRS"
            Me.HRS_Temp_DBname = "TEMP_DSSHRS"
            Me.HRS_Main_DBCon.ConnectionString = "Data Source= " & servername & "; initial catalog=" + Me.HRS_Main_DBname + "; integrated security=true; Connect Timeout=0"
            Me.HRS_Temp_DBCon.ConnectionString = "Data Source= " & servername & "; initial catalog=" + Me.HRS_Temp_DBname + "; integrated security=true; Connect Timeout=0"
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
            'objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return server
    End Function
#End Region

End Class
