Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Data.Odbc
Imports System.Windows.Forms

Public Class clsdbAccess

#Region " variables and constructors"
    Private sqlServerConStr As String 'connection string to the server
    Private sqlConnection As New System.Data.SqlClient.SqlConnection 'for connection to the database
    Private sqlSelectCommand As New System.Data.SqlClient.SqlCommand 'for selection statements
    Private sqlInsertCommand As New System.Data.SqlClient.SqlCommand 'for insert statements
    Private sqlUpdateCommand As New System.Data.SqlClient.SqlCommand ' for update statements
    Public mySqlTransaction As SqlTransaction ' for transaction
    Private strServerName As String
    Private serverNameRetrived As Boolean = False
    Public objRef As clsformrefrences = clsformrefrences.getObject
    Public arrUpdateList As New Dictionary(Of Object, Object) ' for marking records for deletion
    Private Shared objSingle As clsdbAccess
    Private Shared blCreated As Boolean

    Public Shared Function getObject(ByVal strSchema As String) As clsdbAccess
        If blCreated = False Then
            objSingle = New clsdbAccess
            blCreated = True
            Try
                objSingle.sqlServerConStr = "Data Source= " & objSingle.getServerName.Trim & "; initial catalog=" & strSchema & "; User Id=sa; Password=desktophrs; Trusted_Connection=True;  MultipleActiveResultSets=True"
                objSingle.sqlConnection.ConnectionString = objSingle.sqlServerConStr
            Catch ex As Exception
                objSingle.objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

                'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

                MsgBox(ex.Message)
                ' 'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
            End Try
            Return objSingle
        Else
            Return objSingle
        End If
    End Function
    Public Shared Function getObject() As clsdbAccess
        If blCreated = False Then
            objSingle = New clsdbAccess
            blCreated = True
            objSingle.arrUpdateList.Add("rec_status", "X") 'deletion will append an 'X' to existing rec_status value
            Try

                objSingle.sqlServerConStr = "Data Source= " & objSingle.getServerName.Trim & "; initial catalog=TEMP_DSSHRS; User Id=desktophrs; Password=123desktop; Trusted_Connection=True; MultipleActiveResultSets=True"
                objSingle.sqlConnection.ConnectionString = objSingle.sqlServerConStr
            Catch ex As Exception
                objSingle.objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

                'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

                MsgBox(ex.Message)
                ' 'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
            End Try
            Return objSingle
        Else
            Return objSingle
        End If
    End Function
    Private Sub New()
        'arrUpdateList.Add("rec_status", "X") 'deletion will append an 'X' to existing rec_status value
        'Try

        '    sqlServerConStr = "Data Source= " & getServerName.Trim & "; initial catalog=TEMP_DSSHRS; User Id=desktophrs; Password=123desktop; Trusted_Connection=True; MultipleActiveResultSets=True"
        '    sqlConnection.ConnectionString = sqlServerConStr
        'Catch ex As Exception
        '    objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

        '    'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

        '    MsgBox(ex.Message)
        '    ' 'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        'End Try
    End Sub
    'Friend Sub New(ByVal strSchema As String)
    '    'arrUpdateList.Add("rec_status", "X + [rec_status]") 'deletion will append an 'X' to existing rec_status value
    '    Try
    '        sqlServerConStr = "Data Source= " & getServerName.Trim & "; initial catalog=" & strSchema & "; User Id=sa; Password=desktophrs; Trusted_Connection=True;  MultipleActiveResultSets=True"
    '        sqlConnection.ConnectionString = sqlServerConStr
    '    Catch ex As Exception
    '        objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

    '        'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

    '        MsgBox(ex.Message)
    '        ' 'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
    '    End Try
    'End Sub
    Public ReadOnly Property getConnection() As SqlConnection
        Get
            Return sqlConnection
        End Get
    End Property
#End Region

#Region "procedures"
    Public Function getServerName() As String

        If serverNameRetrived Then Return strServerName

        Try
            Dim freader As System.IO.StreamReader
            freader = System.IO.File.OpenText("serverpath")

            strServerName = freader.ReadLine()

            freader.Close()
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()
            Return ""
        End Try
        Return strServerName
    End Function



    Friend Sub openConnection()
        Try
            If sqlConnection.State = ConnectionState.Closed Then Me.sqlConnection.Open() 'open the database connection if closed
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()



            MsgBox("The Database is Not responding.")

            Exit Sub
        End Try
    End Sub
    Friend Sub closeConnection()
        Try
            If sqlConnection.State = ConnectionState.Open Then Me.sqlConnection.Close() 'Close the database if open
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            MsgBox("The Database is Not responding.")
            '' 'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
            Exit Sub
        End Try
        'sqlConnection.Close()
    End Sub




    'This function executes queries supplied by the client on the Database.
    'Nature of queries executed: DELETE, UPDATE
    'PRE CONDITION: strQry has been assigned, Connection String Created
    'POST CONDITION: Sql command has been executed, database connection closed

    Friend Function dbExecute(ByVal strQry As String)
        Dim rowsAffected As Integer
        sqlSelectCommand.Connection = sqlConnection 'make connection
        sqlSelectCommand.CommandText = strQry 'prepare the sql command
        sqlSelectCommand.Transaction = mySqlTransaction 'Assign the Transaction object to the Transaction property of the SqlCommand to be executed
        'sqlConnection.ConnectionString = sqlServerConStr
        Me.openConnection() 'check if database connection is closed and open if closed
        Try
            rowsAffected = sqlSelectCommand.ExecuteNonQuery 'execute query
            'sqlConnection.Close()
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            MsgBox("Failed Database Execution: " & ex.Message, MsgBoxStyle.Critical)
            '' 'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
            If sqlConnection.State = ConnectionState.Open Then
                'sqlConnection.Close()
            End If
            rowsAffected = 0
        End Try
        Return rowsAffected
    End Function
#End Region

End Class
