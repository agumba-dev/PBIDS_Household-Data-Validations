
Imports System.Data.SqlClient
Imports System.Data
Imports Microsoft.SqlServer.Management.Smo
Public Class clsDataAccess
    'Do constructor declaration her
#Region "Constructors"
    Private Shared objSingle As clsDataAccess
    Private Shared blCreated As Boolean
    Public Shared Function getObject() As clsDataAccess
        If blCreated = False Then
            objSingle = New clsDataAccess
            blCreated = True
            Return objSingle
        Else
            Return objSingle
        End If
    End Function
    Private Sub New()
        'Override the default constructor
    End Sub

#End Region
    'Creates a singleton Object of this class.
    'Thus ensuring only one instance is created.
    'To obtain a reference to the only instance of the SqlCeStuff class, 
    'you don't use its constructor. Instead, you call its getObject() method
    'Do variable declaration here
#Region "Variable Declaration"
    Private clsGlobalVariable As clsGlobalVariables = clsGlobalVariables.getObject
    Public objRef As clsformrefrences = clsformrefrences.getObject
    Public objVal As clsvalidations = clsvalidations.getObject
    ' Private ServerConnection As New SqlConnection
    Private sqlSelectCommand As New SqlCommand
    'Friend sqlOleConnection As New System.Data.OleDb.OleDbConnection
    Friend validationtype As mhrsSyncValidationTypes
    Friend UserAppvalidationerrors As String


#End Region

#Region "Data access Generic functions "

    Public Function updateRecord(ByVal tab As DataTable, Optional ByVal wherepart As String = "") As Boolean
        '  Try
        Dim cmd As New SqlCommand
        cmd.Connection = Me.clsGlobalVariable.HRS_Temp_DBCon
        'set this command to be a member of a transaction
        Me.clsGlobalVariable.open_HRS_TEMP_DBCon()
        cmd.CommandText = generateUpdateSql(tab, wherepart)
        cmd.Parameters.Clear()
        For Each row As DataRow In tab.Rows
            cmd.Parameters.AddWithValue("@" + row("Column_name").ToString, row("value"))
        Next
        'cmd.Prepare()
        If cmd.ExecuteNonQuery() > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function submitNewRecord(ByVal tab As DataTable) As Boolean
        ' Try
        Dim cmd As New SqlCommand
        cmd.Connection = Me.clsGlobalVariable.HRS_Temp_DBCon
        'set this command to be a member of a transaction
        Me.clsGlobalVariable.open_HRS_TEMP_DBCon()  '= False Then Return False
        cmd.CommandText = generateInsertSql(tab)
        cmd.Parameters.Clear()
        For Each row As DataRow In tab.Rows
            cmd.Parameters.AddWithValue("@" + row("Column_name").ToString, row("value"))
        Next
        ' cmd.Prepare()
        If cmd.ExecuteNonQuery() > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function addColValue(ByVal table As DataTable, ByVal column_name As String, ByVal value As Object) As Boolean
        Dim newrow As DataRow = table.NewRow
        newrow("Column_name") = column_name
        newrow("value") = value
        table.Rows.Add(newrow)
    End Function

    Public Function newRecordValuesTable(ByVal tableName As String) As DataTable
        Dim myTable As New DataTable()
        myTable.CaseSensitive = False
        myTable.TableName = tableName
        Dim colName As DataColumn = New DataColumn("Column_name")
        colName.DataType = System.Type.GetType("System.String")
        myTable.Columns.Add(colName)
        Dim colValue As DataColumn = New DataColumn("value")
        colValue.DataType = System.Type.GetType("System.Object")
        myTable.Columns.Add(colValue)
        Return myTable
    End Function

    Public Function generateInsertSql(ByVal tab As DataTable) As String
        Dim sql As String = ""
        Dim colsection As String = ""
        Dim parameterSection As String = ""
        Dim rowCount As Integer = tab.Rows.Count
        If rowCount > 0 Then
            colsection = " (" + tab.Rows(0).Item("Column_name").ToString.Trim
            parameterSection = " (@" + tab.Rows(0).Item("Column_name").ToString.Trim
            If rowCount > 1 Then
                For i As Integer = 1 To rowCount - 1
                    colsection = colsection + "," + tab.Rows(i).Item("Column_name").ToString.Trim
                    parameterSection = parameterSection + ",@" + tab.Rows(i).Item("Column_name").ToString.Trim
                Next
            End If
            colsection = colsection + ")"
            parameterSection = parameterSection + ")"
            sql = "INSERT INTO " + tab.TableName + " " + colsection + " values " + parameterSection
        Else
            sql = ""
        End If
        Return sql
    End Function

    Public Function generateUpdateSql(ByVal tab As DataTable, ByVal wherePart As String) As String
        Dim sql As String = ""
        Dim colsection As String = ""
        Dim rowCount As Integer = tab.Rows.Count
        If rowCount > 0 Then
            colsection = " set " + tab.Rows(0).Item("Column_name").ToString.Trim + " =@" + tab.Rows(0).Item("Column_name").ToString.Trim
            If rowCount > 1 Then
                For i As Integer = 1 To rowCount - 1
                    colsection = colsection + "," + tab.Rows(i).Item("Column_name").ToString.Trim + " =@" + tab.Rows(i).Item("Column_name").ToString.Trim
                Next
            End If
            sql = "UPDATE " + tab.TableName + " " + colsection + "  " + wherePart.Trim
        Else
            sql = ""
        End If
        Return sql
    End Function
#End Region

#Region "Public Functions"

    Friend Function getTableData(ByVal query As String) As DataTable
        Dim table As New DataTable
        Dim readb As Data.SqlClient.SqlDataReader
        sqlSelectCommand.Connection = clsGlobalVariable.HRS_Temp_DBCon
        sqlSelectCommand.CommandText = query
        Try
            clsGlobalVariable.open_HRS_TEMP_DBCon()
            readb = sqlSelectCommand.ExecuteReader(CommandBehavior.CloseConnection)
            table.Load(readb)
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            MsgBox(ex.Message)
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        clsGlobalVariable.close_HRS_TEMP_DBCon()
        Return table
    End Function
    Friend Function getDatabaseTables() As DataTable
        Dim SchemaTable As New DataTable
        Dim readb As Data.SqlClient.SqlDataReader
        sqlSelectCommand.Connection = clsGlobalVariable.HRS_Main_DBCon
        sqlSelectCommand.CommandText = "select table_schema,table_name from information_schema.tables  " _
        & "  where table_type='BASE TABLE' and NOT(table_schema='dbo')"
        Try
            clsGlobalVariable.open_HRS_Main_DBCon()
            readb = sqlSelectCommand.ExecuteReader(CommandBehavior.CloseConnection)
            SchemaTable.Load(readb)
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            MsgBox(ex.Message)
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return SchemaTable
    End Function
    Friend Function getTablefromTempDb(ByVal sql As String) As DataTable
        Dim table As New DataTable
        Dim readb As Data.SqlClient.SqlDataReader
        sqlSelectCommand.Connection = clsGlobalVariable.HRS_Temp_DBCon
        sqlSelectCommand.CommandText = sql
        Try
            clsGlobalVariable.open_HRS_TEMP_DBCon()
            readb = sqlSelectCommand.ExecuteReader(CommandBehavior.CloseConnection)
            table.Load(readb)
        Catch ex As Exception
            clsGlobalVariable.close_HRS_TEMP_DBCon()
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            MsgBox(ex.Message)
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return table
    End Function
    Friend Function exec_nonqueryMain(ByVal sql As String) As Integer
        Dim selectcmd As New SqlCommand
        Dim i As Integer = 0
        Try
            selectcmd.CommandText = sql
            selectcmd.Connection = clsGlobalVariable.HRS_Main_DBCon
            clsGlobalVariable.open_HRS_Main_DBCon()
            i = selectcmd.ExecuteNonQuery
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            MsgBox(ex.Message)
            i = 0
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        clsGlobalVariable.close_HRS_Main_DBCon()
        Return i
    End Function
    Friend Function exec_nonqueryInTEMPDB(ByVal sql As String) As Integer
        Dim cmd As New SqlCommand
        Dim i As Integer = 0
        Try
            cmd.CommandText = sql
            cmd.Connection = clsGlobalVariable.HRS_Temp_DBCon
            clsGlobalVariable.open_HRS_TEMP_DBCon()
            i = cmd.ExecuteNonQuery
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            MsgBox(ex.Message)
            i = 0
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        clsGlobalVariable.close_HRS_TEMP_DBCon()
        Return i
    End Function

    'updated now
    Public Function saveError( _
                                     ByVal Recordid As String, _
                                     ByVal tablename As String, _
                                     ByVal Errortype As String, _
                                     ByVal Comments As String, _
                                     ByVal ErrorDate As Date, _
                                     ByVal QueryRefID As String, _
                                     ByVal ErrorCompound As String, _
                                      ByVal ErrorRound As String) As Boolean 'ByVal cmd As SqlCommand) As Boolean

        If validationtype = mhrsSyncValidationTypes.userpplication Then
            UserAppvalidationerrors = UserAppvalidationerrors & Errortype & vbNewLine
            Return True
        End If

        Dim success As Boolean = False
        Dim cmd As New SqlCommand
        Dim current_databaseName As String = ""
        'ByVal db As datalevel,
        Select Case Me.clsGlobalVariable.currectDBtoValidate
            Case datalevel.DSSHRS
                Recordid = Me.clsGlobalVariable.currectRecPrimarykeyValues
                current_databaseName = "dsshrs"
            Case datalevel.TEMP_DSSHRS
                current_databaseName = "temp_dsshrs"
        End Select
        ' MsgBox(Me.clsGlobalVariable.currectRecPrimarykeyValues)

        cmd.Connection = Me.clsGlobalVariable.HRS_Temp_DBCon
        If Not Me.clsGlobalVariable.HRS_Temp_DBCon.State = ConnectionState.Open Then Me.clsGlobalVariable.HRS_Temp_DBCon.Open()
        cmd.CommandText = "if not exists(select * from [MHRS_SYS].[Temp_Data_Errors] where recordid=@recordid and " _
                        & " errortype=@Errortype and tablename=@tablename and rec_status=@rec_status) " _
                        & "INSERT INTO [MHRS_SYS].[Temp_Data_Errors]" _
                        & "([Recordid],[tablename],[Errortype],[Comments],[ErrorDate],[QueryRefID],[rec_status],[compound],[village],[round],databaseName,rec_details)" _
                        & "VALUES(@Recordid,@tablename,@Errortype,@Comments,@ErrorDate,@QueryRefID,@rec_status,@compound,@village,@round,@databaseName,@rec_details)"
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@Recordid", Recordid)
        cmd.Parameters.AddWithValue("@tablename", tablename)
        cmd.Parameters.AddWithValue("@Errortype", Errortype)
        cmd.Parameters.AddWithValue("@Comments", Comments)
        cmd.Parameters.AddWithValue("@ErrorDate", ErrorDate)
        cmd.Parameters.AddWithValue("@QueryRefID", QueryRefID)
        cmd.Parameters.AddWithValue("@rec_status", "P")
        cmd.Parameters.AddWithValue("@village", Me.objVal.getIDSubstring(ErrorCompound, idTypes.VILLAGE))
        'cmd.Parameters.AddWithValue("@village", ErrorVillage)
        cmd.Parameters.AddWithValue("@round", ErrorRound)
        cmd.Parameters.AddWithValue("@compound", ErrorCompound)
        cmd.Parameters.AddWithValue("@databaseName", current_databaseName)
        cmd.Parameters.AddWithValue("@rec_details", Me.clsGlobalVariable.currectRecPrimarykeyValues)
        clsGlobalVariable.open_HRS_TEMP_DBCon()
        If cmd.ExecuteNonQuery > 0 Then
            success = True
        Else
            success = False
        End If
        Me.clsGlobalVariable.HRS_Temp_DBCon.Close()
        Return success
    End Function
    Friend Function getTableDataFromTempDB(ByVal sql As String) As DataTable
        Dim selectcmd As New SqlCommand
        Dim prints As New DataTable
        Try
            selectcmd.CommandText = sql
            selectcmd.Connection = Me.clsGlobalVariable.HRS_Temp_DBCon
            Me.clsGlobalVariable.open_HRS_TEMP_DBCon()
            Dim readDB As SqlDataReader = selectcmd.ExecuteReader(CommandBehavior.CloseConnection)
            prints.Load(readDB)
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            MsgBox(ex.Message)
            prints = Nothing
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Me.clsGlobalVariable.close_HRS_TEMP_DBCon()
        Return prints
    End Function
    Friend Function getTableDataFromMAINDB(ByVal sql As String) As DataTable
        Dim selectcmd As New SqlCommand
        Dim prints As New DataTable
        Try
            selectcmd.CommandText = sql
            selectcmd.Connection = clsGlobalVariable.HRS_Main_DBCon
            clsGlobalVariable.open_HRS_Main_DBCon()
            Dim readDB As SqlDataReader = selectcmd.ExecuteReader(CommandBehavior.CloseConnection)
            prints.Load(readDB)
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            MsgBox(ex.Message)
            prints = Nothing
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        clsGlobalVariable.close_HRS_Main_DBCon()
        Return prints
    End Function
    Friend Function exec_nonquery(ByVal sql As String) As Integer
        Dim selectcmd As New SqlCommand
        Dim i As Integer = 0
        Try
            selectcmd.CommandText = sql
            selectcmd.Connection = Me.clsGlobalVariable.HRS_Temp_DBCon
            Me.clsGlobalVariable.open_HRS_TEMP_DBCon()
            selectcmd.CommandTimeout = 0
            i = selectcmd.ExecuteNonQuery
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            MsgBox(ex.Message)
            i = 0
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Me.clsGlobalVariable.close_HRS_TEMP_DBCon()
        Return i
    End Function
    Private Function countTableRecords(ByVal sql As String) As Integer
        Dim selectcmd As New SqlCommand
        Dim recCount As Integer = 0
        Try
            selectcmd.CommandText = sql
            selectcmd.Connection = Me.clsGlobalVariable.HRS_Temp_DBCon
            Me.clsGlobalVariable.open_HRS_TEMP_DBCon()
            recCount = selectcmd.ExecuteScalar
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            MsgBox(ex.Message)
            recCount = 0
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Me.clsGlobalVariable.close_HRS_TEMP_DBCon()
        Return recCount
    End Function
    Friend Function checkifrecordexists_INTEMPDB(ByVal keycolname As String, ByVal ID As String, ByVal tablename As String) As Boolean
        'Dim table As String = "NonResidentDetails"
        Dim existsRecord As Boolean = False
        Dim commsql As New SqlCommand
        Dim individualsId As Integer = 0
        commsql.Connection = clsGlobalVariable.HRS_Temp_DBCon
        If (tablename.ToLower = "dss.round") Or (tablename.ToLower = "dss.villages") Then
            commsql.CommandText = "select count(*) from " + tablename + "  where (" + keycolname + " ='" + ID + "') "
        Else
            commsql.CommandText = "select count(*) from " + tablename + "  where (" + keycolname + " ='" + ID + "')  and (rec_status not like '%x%')"
        End If
        'commsql.CommandText = "select count(*) from " + tablename + "  where (" + keycolname + " ='" + ID + "') and (rec_status not like 'x%')"
        Try
            clsGlobalVariable.open_HRS_TEMP_DBCon()
            Try

                individualsId = commsql.ExecuteScalar()
                If individualsId > 0 Then
                    existsRecord = True
                Else
                    existsRecord = False
                End If
            Catch ex As Exception
                individualsId = -1
                existsRecord = False
                MsgBox("check " + tablename + " " + ex.Message)
                'emailErrors("check" + tablename + "" + ex.Message)
            End Try
        Catch ex As Exception
            individualsId = -1
            existsRecord = False
            MsgBox("check " + tablename + " " + ex.Message)
            'emailErrors("check " + tablename + " " + ex.Message)
        End Try
        clsGlobalVariable.close_HRS_TEMP_DBCon()
        Return existsRecord
    End Function
    Friend Function checkifMultiplerecordexists_INTEMPDB(ByVal keycolname As String, ByVal ID As String, ByVal tablename As String) As Boolean
        'Dim table As String = "NonResidentDetails"
        Dim existsRecord As Boolean = False
        Dim commsql As New SqlCommand
        Dim individualsId As Integer = 0
        commsql.Connection = clsGlobalVariable.HRS_Temp_DBCon
        If (tablename.ToLower = "dss.round") Or (tablename.ToLower = "dss.villages") Then
            commsql.CommandText = "select count(*) from " + tablename + "  where (" + keycolname + " ='" + ID + "') "
        Else
            commsql.CommandText = "select count(*) from " + tablename + "  where (" + keycolname + " ='" + ID + "')  and (rec_status not like '%x%')"
        End If


        Try
            clsGlobalVariable.open_HRS_TEMP_DBCon()
            Try

                individualsId = commsql.ExecuteScalar()
                If individualsId > 1 Then
                    existsRecord = True
                Else
                    existsRecord = False
                End If
            Catch ex As Exception
                individualsId = -1
                existsRecord = True
                MsgBox("check " + tablename + " " + ex.Message)
                'emailErrors("check " + tablename + " " + ex.Message)
            End Try
        Catch ex As Exception
            individualsId = -1
            existsRecord = True
            MsgBox("check " + tablename + " " + ex.Message)
            'emailErrors("check " + tablename + " " + ex.Message)
        End Try
        clsGlobalVariable.close_HRS_TEMP_DBCon()
        Return existsRecord
    End Function
    Private Function recordexist(ByVal query As String) As Boolean
        Dim i As Integer = 0
        Using cmd As New SqlCommand
            cmd.Connection = clsGlobalVariable.HRS_Main_DBCon
            cmd.CommandText = query
            Try
                clsGlobalVariable.open_HRS_Main_DBCon()
                i = cmd.ExecuteScalar
            Catch ex As Exception
                objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

                'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

                MsgBox(ex.Message)
                'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
            End Try
        End Using
        clsGlobalVariable.close_HRS_Main_DBCon()
        If i > 1 Then
            Return True
        Else
            Return False
        End If
    End Function
#End Region

#Region "Episode functions"
    Public Function executeScalar_INMainDB(ByVal query As String) As Integer
        Dim commsql As New SqlCommand
        Dim recCount As Integer = 0
        commsql.Connection = clsGlobalVariable.HRS_Main_DBCon
        commsql.CommandText = query
        Try
            clsGlobalVariable.open_HRS_Main_DBCon()
            Try
                recCount = CInt(commsql.ExecuteScalar())
            Catch ex As Exception
                objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

                'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

                recCount = -1
                'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
            End Try
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            recCount = -1
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        clsGlobalVariable.close_HRS_Main_DBCon()
        Return recCount
    End Function
    Public Function getScalar_inMainDB(ByVal query As String) As Object
        Dim commsql As New SqlCommand
        Dim recCount As Object = Nothing
        commsql.Connection = clsGlobalVariable.HRS_Main_DBCon
        commsql.CommandText = query
        Try
            clsGlobalVariable.open_HRS_Main_DBCon()
            Try
                recCount = commsql.ExecuteScalar()
            Catch ex As Exception
                objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

                'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

                recCount = Nothing
                'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
            End Try
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            recCount = Nothing
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        clsGlobalVariable.close_HRS_Main_DBCon()
        Return recCount
    End Function
    'fdsgdfhgf edit here
    Friend Function checkifrecordexists_INMainDB(ByVal keycolname As String, ByVal ID As String, ByVal tablename As String) As Boolean
        Dim commsql As New SqlCommand
        Dim recCount As Integer = 0
        commsql.Connection = clsGlobalVariable.HRS_Main_DBCon
        commsql.CommandText = "select count(*) from " + tablename + "  where (" + keycolname + " ='" + ID + "')"
        Try
            clsGlobalVariable.open_HRS_Main_DBCon()
            Try
                recCount = CInt(commsql.ExecuteScalar())
            Catch ex As Exception
                objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

                'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

                recCount = -1
                'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
            End Try
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            recCount = -1
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        clsGlobalVariable.close_HRS_Main_DBCon()
        If recCount > 0 Then
            Return True
        Else
            Return False
        End If

    End Function
    Public Function checkifEpisodeexists_INMainDB(ByVal individid As String, ByVal episode_tablename As String) As Integer
        Dim commsql As New SqlCommand
        Dim recCount As Integer = 0
        commsql.Connection = clsGlobalVariable.HRS_Main_DBCon
        commsql.CommandText = "select count(*) from " + episode_tablename + "  where individid ='" + individid + "'"
        Try
            clsGlobalVariable.open_HRS_Main_DBCon()
            Try
                recCount = CInt(commsql.ExecuteScalar())
            Catch ex As Exception

                objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

                'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

                recCount = -1
                'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
            End Try
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            recCount = -1
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        clsGlobalVariable.close_HRS_Main_DBCon()
        Return recCount
    End Function

#End Region

#Region " Validation rules saving"

    Friend Function AddValidationRule( _
                                         ByVal table_name As String, _
                                         ByVal table_col As String, _
                                         ByVal ref_table As String, _
                                         ByVal ref_col As String) As Boolean
        Dim newRecord As DataTable = Me.newRecordValuesTable("[dbo].[validationitems]")
        Dim coltype As String = ""
        If (table_col.ToLower.Trim = ref_col.ToLower.Trim) And (table_name.ToLower.Trim = ref_table.ToLower.Trim) Then
            coltype = "PRIMARY KEY"
        Else
            coltype = "FOREIGN KEY"
        End If
        Me.addColValue(newRecord, "table_name", table_name)
        Me.addColValue(newRecord, "table_col", table_col)
        Me.addColValue(newRecord, "ref_table", ref_table)
        Me.addColValue(newRecord, "ref_col", ref_col)
        Me.addColValue(newRecord, "coltype", coltype)
        Me.addColValue(newRecord, "constraint_name", "na")
        Me.addColValue(newRecord, "isComposite", False)
        Me.addColValue(newRecord, "validtype", "S")
        Me.addColValue(newRecord, "isenabled", True)

        Dim success As Boolean = False
        Try
            If Me.submitNewRecord(newRecord) Then
                success = True
            Else
                success = False
            End If

        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            success = False
            MsgBox(ex.Message)
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try

        Me.clsGlobalVariable.close_HRS_TEMP_DBCon()
        Return success
    End Function
#End Region

#Region "other codes"
    Public Function saveProblem( _
                                     ByVal Problem_level As String, _
                                     ByVal Problem_Item As String, _
                                     ByVal Problem_description As String, _
                                     ByVal Raised_By As String, _
                                     ByVal data_error_ID As String) As Boolean

        Dim success As Boolean = False
        Dim cmd As New SqlCommand
        cmd = New SqlCommand
        cmd.Connection = clsGlobalVariable.HRS_Temp_DBCon
        cmd.CommandText = "INSERT INTO [TEMP_DSSHRS].[MHRS_SYS].[Problems] " _
                            & " ([problem_ID],[Problem_level],[Problem_Item],[Problem_description],[DateRaised] " _
                            & " ,[Raised_By],[data_error_ID])  " _
                            & " VALUES  " _
                            & " (@problem_ID, @Problem_level,@Problem_Item,@Problem_description,@DateRaised  " _
                            & " ,@Raised_By,@data_error_ID)"
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@problem_ID", Guid.NewGuid)
        cmd.Parameters.AddWithValue("@Problem_level", Problem_level)
        cmd.Parameters.AddWithValue("@Problem_Item", Problem_Item)
        cmd.Parameters.AddWithValue("@Problem_description", Problem_description)
        cmd.Parameters.AddWithValue("@DateRaised", Now())
        cmd.Parameters.AddWithValue("@Raised_By", Raised_By)
        cmd.Parameters.AddWithValue("@data_error_ID", data_error_ID)
        Me.clsGlobalVariable.open_HRS_TEMP_DBCon()
        If cmd.ExecuteNonQuery > 0 Then
            success = True
        Else
            success = False
        End If
        Me.clsGlobalVariable.close_HRS_TEMP_DBCon()
        Return success
    End Function
    Public Function saveSolution( _
                                   ByVal Problem_ID As String, _
                                   ByVal Solution_description As String, _
                                   ByVal problem_fixed As Boolean, _
                                   ByVal SolvedBy As String) As Boolean
        Dim success As Boolean = False
        Dim cmd As New SqlCommand
        cmd = New SqlCommand
        cmd.Connection = clsGlobalVariable.HRS_Temp_DBCon
        cmd.CommandText = "INSERT INTO [TEMP_DSSHRS].[MHRS_SYS].[problem_solutions] " _
                             & " ([Solution_ID],[Problem_ID],[Solution_description],[problem_fixed] " _
                             & " ,[SolvedBy],[Solution_date]) " _
                             & " VALUES " _
                             & " (@Solution_ID,@Problem_ID,@Solution_description,@problem_fixed " _
                             & " ,@SolvedBy,@Solution_date)"
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@Solution_ID", Guid.NewGuid)
        cmd.Parameters.AddWithValue("@Problem_ID", Problem_ID)
        cmd.Parameters.AddWithValue("@Solution_description", Solution_description)
        cmd.Parameters.AddWithValue("@problem_fixed", problem_fixed)
        cmd.Parameters.AddWithValue("@SolvedBy", SolvedBy)
        cmd.Parameters.AddWithValue("@Solution_date", Now)
        Me.clsGlobalVariable.open_HRS_TEMP_DBCon()
        If cmd.ExecuteNonQuery > 0 Then
            success = True
        Else
            success = False
        End If
        Me.clsGlobalVariable.close_HRS_TEMP_DBCon()
        Return success
    End Function
#End Region

#Region " validation codes"
    Friend Function getResidencyPreVEvent(ByVal Residencyrecord As DataRow) As String
        Dim returnValue As String = Nothing
        Dim edate As Date = Residencyrecord("edate")
        Dim sql As String = "SELECT [eeventtype]  FROM [DSSHRS].[dss].[Residency]" _
                            & "where  (individid='" + Residencyrecord("individid").ToString + "') and (ResidencyID<>'" + Residencyrecord("ResidencyID").ToString + "')  and " _
                            & " where (DATEDIFF(day,[edate],getdate()))>=(DATEDIFF(day,@eventdate,getdate()))"
        Using cmd As New SqlCommand
            cmd.CommandText = sql
            cmd.Connection = clsGlobalVariable.HRS_Main_DBCon
            cmd.Parameters.AddWithValue("@eventdate", edate)
            Try
                clsGlobalVariable.open_HRS_Main_DBCon()
                Try
                    returnValue = cmd.ExecuteScalar().ToString
                Catch ex As Exception
                    returnValue = ""
                End Try
            Catch ex As Exception
                returnValue = ""
            End Try
            clsGlobalVariable.close_HRS_Main_DBCon()
            Return returnValue = ""
        End Using
        Return returnValue
    End Function
    Friend Function getmembershipPreVEvent(ByVal Membershiprecord As DataRow) As String
        Dim returnValue As String = Nothing
        Dim edate As Date = Membershiprecord("edate")
        Dim sql As String = "SELECT [eeventtype]  FROM [DSSHRS].[dss].[Membership]" _
                            & "where  (individid='" + Membershiprecord("individid").ToString + "') and (MembershipID<>'" + Membershiprecord("MembershipID").ToString + "')  and " _
                            & " where (DATEDIFF(day,[edate],getdate()))>=(DATEDIFF(day,@eventdate,getdate()))"
        Using cmd As New SqlCommand
            cmd.CommandText = sql
            cmd.Connection = clsGlobalVariable.HRS_Main_DBCon
            cmd.Parameters.AddWithValue("@eventdate", edate)
            Try
                clsGlobalVariable.open_HRS_Main_DBCon()
                Try
                    returnValue = cmd.ExecuteScalar().ToString
                Catch ex As Exception
                    returnValue = ""
                End Try
            Catch ex As Exception
                returnValue = ""
            End Try
            clsGlobalVariable.close_HRS_Main_DBCon()
            Return returnValue = ""
        End Using
        Return returnValue
    End Function
    Public Function hadprevioslyDied(ByVal query As String, ByVal edate As Date) As Integer
        Dim returnValue As String = Nothing
        Dim commsql As New SqlCommand
        Dim recCount As Integer = 0
        Using cmd As New SqlCommand
            cmd.CommandText = query
            cmd.Connection = clsGlobalVariable.HRS_Main_DBCon
            cmd.Parameters.AddWithValue("@eventdate", edate)
            Try
                clsGlobalVariable.open_HRS_Main_DBCon()
                Try
                    returnValue = CInt(cmd.ExecuteScalar())
                Catch ex As Exception
                    returnValue = -1
                End Try
            Catch ex As Exception
                returnValue = -1
            End Try
            clsGlobalVariable.close_HRS_Main_DBCon()
        End Using
        Return returnValue
    End Function


#End Region

#Region "other codes"
    ''' <summary>
    ''' Checks if the end episode in temp has the right precedence
    ''' </summary>
    ''' <param name="episodeRecord"></param>
    ''' <param name="tablename"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function hasEndEpisodePrecedenceConflict(ByVal episodeRecord As DataRow, ByVal tablename As String) As Boolean
        Dim returnValue As Boolean = True
        Dim cmd As New SqlCommand
        Dim recount As Integer = 0
        Dim sql As String = "select COUNT(*) from " + tablename + " where rec_status in('U','DU','TU','MU')" _
                            & " and (individid=@individid)and (edate <@edate) " _
                            & " and (errflag=1) and (transit_id<>@transit_id)"
        cmd.Parameters.Clear()
        cmd.CommandText = sql
        cmd.Parameters.AddWithValue("@individid", episodeRecord("individid"))
        cmd.Parameters.AddWithValue("@edate", episodeRecord("edate"))
        cmd.Parameters.AddWithValue("@transit_id", episodeRecord("transit_id"))
        cmd.Connection = clsGlobalVariable.HRS_Temp_DBCon
        clsGlobalVariable.open_HRS_TEMP_DBCon()
        Try
            recount = cmd.ExecuteScalar()
        Catch ex As Exception
            Return True
        End Try

        clsGlobalVariable.close_HRS_TEMP_DBCon()
        If recount > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function
    ''' <summary>
    ''' Checks if the end episode in temp has the right precedence
    ''' </summary>
    ''' <param name="episodeRecord"></param>
    ''' <param name="tablename"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function hasstartEpisodePrecedenceConflict(ByVal episodeRecord As DataRow, ByVal tablename As String) As Boolean
        Dim returnValue As Boolean = True
        Dim cmd As New SqlCommand
        Dim recount As Integer = 0
        Dim sql As String = "select COUNT(*) from " + tablename + " where rec_status in('DI','I','TI','MI')" _
                            & " and (individid=@individid)and (sdate <@sdate) " _
                            & " and (errflag=1) and (transit_id<>@transit_id) "
        cmd.Parameters.Clear()
        cmd.CommandText = sql
        cmd.Parameters.AddWithValue("@individid", episodeRecord("individid"))
        cmd.Parameters.AddWithValue("@sdate", episodeRecord("sdate"))
        cmd.Parameters.AddWithValue("@transit_id", episodeRecord("transit_id"))
        cmd.Connection = clsGlobalVariable.HRS_Temp_DBCon
        clsGlobalVariable.open_HRS_TEMP_DBCon()
        Try
            recount = cmd.ExecuteScalar()
        Catch ex As Exception
            Return True
        End Try
        clsGlobalVariable.close_HRS_TEMP_DBCon()
        If recount > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function

    ''' <summary>
    ''' Checks if the end episode in Main DB has the right precedence
    ''' </summary>
    ''' <param name="episodeRecord"></param>
    ''' <param name="tablename"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TempDbEpisodeHasEventAfterthisInMainDB(ByVal episodeRecord As DataRow, ByVal tablename As String) As Boolean
        Dim returnValue As Boolean = True
        Dim cmd As New SqlCommand
        Dim recount As Integer = 0
        Dim colname As String = ""
        Select Case tablename.ToLower
            Case "dss.residency"
                colname = "residencyid"
            Case "dss.membership"
                colname = "membershipid"
            Case "dss.pregnancy"
                colname = "PregnancyID"
        End Select
        Dim edatesql As String = " select count(*) from " _
                            & " (select a.sdate as pdate from " + tablename + " as a where (a.individid=@individid) " _
                            & " and a." + colname + "<> '" + episodeRecord(colname).ToString + "'" _
                            & " union " _
                            & " select b.edate as pdate from " + tablename + " as b where (b.individid=@individid)) as c " _
                            & " where (c.pdate >@edate) "
        Dim sdatesql As String = " select count(*) from " _
                           & " (select a.sdate as pdate from " + tablename + " as a where (a.individid=@individid) " _
                           & " union " _
                           & " select b.edate as pdate from " + tablename + " as b where (b.individid=@individid)) as c " _
                           & " where (c.pdate >@sdate) "

        cmd.Parameters.Clear()
        If IsDBNull(episodeRecord("edate")) Then
            cmd.CommandText = sdatesql
            cmd.Parameters.AddWithValue("@individid", episodeRecord("individid"))
            cmd.Parameters.AddWithValue("@sdate", episodeRecord("sdate"))
        Else
            cmd.CommandText = edatesql
            cmd.Parameters.AddWithValue("@individid", episodeRecord("individid"))
            cmd.Parameters.AddWithValue("@edate", episodeRecord("edate"))
        End If
        cmd.Connection = clsGlobalVariable.HRS_Main_DBCon
        clsGlobalVariable.open_HRS_Main_DBCon()
        Try
            recount = cmd.ExecuteScalar()
        Catch ex As Exception
            Return True
        End Try

        clsGlobalVariable.close_HRS_Main_DBCon()
        If recount > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function
   
#End Region

#Region "vincents codes"
    Public myServer As Server
    Public mytable As Table
    Friend mydatabase As Database
    Friend conDataCheker As SqlConnection
    Public databaseName As String = "TEMP_DSSHRS"
    Friend ObjDbAccess As clsdbAccess = clsdbAccess.getObject
    Friend mycon As SqlConnection
    Public Sub initializeServerAndDB()
        myServer = New Server(ObjDbAccess.getServerName.Trim)
        mydatabase = myServer.Databases.Item(databaseName)
        setConnection()
        'servername = myServer.Name
    End Sub
    Friend Sub setConnection()
        Dim sqlServerConStr As String = "Data Source= " & myServer.Name & "; initial catalog= " & mydatabase.Name & "; integrated security=true"
        mycon = New SqlConnection(sqlServerConStr)
        Dim conString As String = "Data Source= " & myServer.Name & "; initial catalog= dataChecker; integrated security=true"
        conDataCheker = New SqlConnection(conString)
    End Sub

#Region "Procedures"
    Public errorLogfilename As String = ""
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
    Public Function getTablesToValidate() As DataTable
        Dim str As String = "SELECT * FROM [dataChecker].[dbo].[TablesTovalidate]"
        Dim cmd As New SqlCommand(str, conDataCheker)
        If Not conDataCheker.State = ConnectionState.Open Then conDataCheker.Open()
        Dim dt As New DataTable()
        Dim da As New SqlDataAdapter(cmd)
        dt.Clear()
        da.Fill(dt)
        conDataCheker.Close()
        Return dt
    End Function
    Public Function addValidation(ByVal columnname As String, ByVal allowedValues As String, ByVal skiplogic As String, _
     ByVal errorDescription As String, ByVal ErrorDescSkipLogic As String, ByVal DefaultValue As String, _
     ByVal functionName As String, ByVal validationStatus As String, Optional ByVal validationID As Integer = Nothing) As Boolean

        Dim cmd As New SqlCommand()
        cmd.Connection = conDataCheker
        'check if validatio exists. if irt exist the update the existing one
        'for each column their can only be 1 record
        If Not conDataCheker.State = ConnectionState.Open Then conDataCheker.Open()
        Dim i As Integer
        If Not validationID = 0 Then


            cmd.CommandText = "UPDATE [dataChecker].[dbo].[TableValidations] " _
                    & "SET [allowedValues] = @allowedValues ,[skipLogic] =@skipLogic,[ErrorDescription] =@ErrorDescription" _
                    & ",[ErrorDescSkipLogic] = @ErrorDescSkipLogic,[functionName] = @functionName,[validationStatus] = @validationStatus  WHERE [validationID]=" & validationID
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@allowedValues", allowedValues)
            cmd.Parameters.AddWithValue("@skipLogic", skiplogic)
            cmd.Parameters.AddWithValue("@ErrorDescription", errorDescription)
            cmd.Parameters.AddWithValue("@ErrorDescSkipLogic", ErrorDescSkipLogic)
            cmd.Parameters.AddWithValue("@functionName", functionName)
            cmd.Parameters.AddWithValue("@validationStatus", validationStatus)

            i = cmd.ExecuteNonQuery()
        Else
            cmd.CommandText = "INSERT INTO [dataChecker].[dbo].[TableValidations]([tableName]" _
               & ",[columnName],[allowedValues],[skipLogic],[ErrorDescription],[ErrorDescSkipLogic],[functionName],[validationStatus])" _
               & " VALUES(@tableName,@columnName,@allowedValues,@skipLogic,@ErrorDescription,@ErrorDescSkipLogic,@functionName, @validationStatus)"
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@tableName", mytable.Schema & "." & mytable.Name)
            cmd.Parameters.AddWithValue("@columnName", columnname)
            cmd.Parameters.AddWithValue("@allowedValues", allowedValues)
            cmd.Parameters.AddWithValue("@skipLogic", skiplogic)
            cmd.Parameters.AddWithValue("@ErrorDescription", errorDescription)
            cmd.Parameters.AddWithValue("@ErrorDescSkipLogic", ErrorDescSkipLogic)
            cmd.Parameters.AddWithValue("@functionName", functionName)
            cmd.Parameters.AddWithValue("@validationStatus", validationStatus)

            'cmd.Prepare()
            i = cmd.ExecuteNonQuery()
        End If
        ' now update default value if any
        If DefaultValue.Trim <> "" Then
            If DefaultValue = "''" Then DefaultValue = ""
            cmd.CommandText = "UPDATE [dataChecker].[dbo].[columnDefaultValues] " _
                           & "SET [DefaultValue] = @DefaultValue  WHERE [columnName]='" & columnname & "'and [tableName]='" & mytable.Schema & "." & mytable.Name & "' "
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@DefaultValue", DefaultValue)

            If cmd.ExecuteNonQuery() > 0 Then
                conDataCheker.Close()
                Return True
            End If
            cmd.CommandText = "INSERT INTO [dataChecker].[dbo].[columnDefaultValues]([tableName]" _
               & ",[columnName],[DefaultValue])" _
               & " VALUES(@tableName,@columnName,@DefaultValue)"
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@tableName", mytable.Schema & "." & mytable.Name)
            cmd.Parameters.AddWithValue("@columnName", columnname)
            cmd.Parameters.AddWithValue("@DefaultValue", DefaultValue)

            'cmd.Prepare()
            i = cmd.ExecuteNonQuery()
        End If
        conDataCheker.Close()
        If i > 0 Then Return True

    End Function
    Public Function getColumnValidationsDefinations(ByVal columnname As String, ByVal tablename As String) As DataTable
        Dim cmd As New SqlCommand
        cmd.Connection = conDataCheker
        cmd.CommandText = "SELECT val.*,def.defaultValue FROM [dataChecker].[dbo].[TableValidations] val " _
                    & " left join columnDefaultValues def on def.columnName=val.columnName and def.tableName=val.tableName " _
                    & " where val.columnName='" & columnname & "' AND val.tablename='" & tablename & "'"
        If Not conDataCheker.State = ConnectionState.Open Then conDataCheker.Open()
        Dim dt As New DataTable()
        Dim da As New SqlDataAdapter(cmd)
        dt.Clear()
        da.Fill(dt)
        conDataCheker.Close()
        Return dt

    End Function
    Public Function getColumnvalidations(ByVal columnname As String, ByVal tablename As String) As DataTable
        Dim str As String = "Select [allowedValues],[skipLogic],[ErrorDescription],[ErrorDescSkipLogic],[functionName] from  tableValidations where " _
                & "columnName= '" & columnname & "' AND tableName='" & tablename & "' AND validationStatus='ACTIVE'"
        Dim cmd As New SqlCommand(str, conDataCheker)
        If Not conDataCheker.State = ConnectionState.Open Then conDataCheker.Open()
        Dim dt As New DataTable()
        Dim da As New SqlDataAdapter(cmd)
        dt.Clear()
        da.Fill(dt)
        conDataCheker.Close()
        Return dt
    End Function
    Public Function getDefaultValue(ByVal columnname As String, ByVal tablename As String) As String
        Dim cmd As New SqlCommand
        cmd.Connection = conDataCheker
        cmd.CommandText = "Select [DefaultValue] from  columnDefaultValues where " _
                                    & "columnName= '" & columnname & "' AND tableName='" & tablename & "'"
        If Not conDataCheker.State = ConnectionState.Open Then conDataCheker.Open()

        Dim defV As String = cmd.ExecuteScalar()
        cmd.Connection.Close()
        Return defV

    End Function
    Public Function executeFunction(ByVal functionName As String, ByVal value As Object) As Object
        Dim cmd As New SqlCommand
        cmd.Connection = Me.clsGlobalVariable.HRS_Temp_DBCon
        cmd.CommandText = "select " & functionName & "('" & value.ToString & "')"
        If Not Me.clsGlobalVariable.HRS_Temp_DBCon.State = ConnectionState.Open Then Me.clsGlobalVariable.HRS_Temp_DBCon.Open()

        Dim defV As Object = cmd.ExecuteScalar()
        cmd.Connection.Close()
        Return defV

    End Function
    Public Function getColumnAllowedValues(ByVal columnname As String, ByVal tablename As String) As DataTable
        Dim cmd As New SqlCommand
        cmd.Connection = conDataCheker
        cmd.CommandText = "SELECT allowedValues FROM [dataChecker].[dbo].[TableValidations] where columnName='" & columnname & "'" _
                          & " AND tablename='" & tablename & "'"
        If Not conDataCheker.State = ConnectionState.Open Then conDataCheker.Open()
        Dim dt As New DataTable()
        Dim da As New SqlDataAdapter(cmd)
        dt.Clear()
        da.Fill(dt)
        conDataCheker.Close()
        Return dt

    End Function
    Public Function getIDColumn(ByVal tablename As String, ByVal con As SqlConnection) As String
        Dim str As String = "select column_name from information_schema.KEY_COLUMN_USAGE where table_Name='" & tablename & "'"

        Dim cmd As New SqlCommand(str, con)
        If Not con.State = ConnectionState.Open Then con.Open()
        Return cmd.ExecuteScalar().ToString
    End Function

    '' updated now
    'Public Function saveError( _
    '                                 ByVal Recordid As String, _
    '                                 ByVal tablename As String, _
    '                                 ByVal Errortype As String, _
    '                                 ByVal Comments As String, _
    '                                 ByVal ErrorDate As Date, _
    '                                 ByVal QueryRefID As String, _
    '                                 ByVal ErrorCompound As String, _
    '                                  ByVal ErrorRound As String) As Boolean 'ByVal cmd As SqlCommand) As Boolean
    '    Dim success As Boolean = False
    '    Dim cmd As New SqlCommand


    '    cmd.Connection = Me.clsGlobalVariable.HRS_Temp_DBCon
    '    If Not Me.clsGlobalVariable.HRS_Temp_DBCon.State = ConnectionState.Open Then Me.clsGlobalVariable.HRS_Temp_DBCon.Open()
    '    cmd.CommandText = "if not exists(select * from [MHRS_SYS].[Temp_Data_Errors] where recordid=@recordid and " _
    '                    & " errortype=@Errortype and tablename=@tablename and rec_status=@rec_status) " _
    '                    & "INSERT INTO [MHRS_SYS].[Temp_Data_Errors]" _
    '                    & "([Recordid],[tablename],[Errortype],[Comments],[ErrorDate],[QueryRefID],[rec_status],[compound],[village],[round])" _
    '                    & "VALUES(@Recordid,@tablename,@Errortype,@Comments,@ErrorDate,@QueryRefID,@rec_status,@compound,@village,@round)"
    '    cmd.Parameters.Clear()
    '    cmd.Parameters.AddWithValue("@Recordid", Recordid)
    '    cmd.Parameters.AddWithValue("@tablename", tablename)
    '    cmd.Parameters.AddWithValue("@Errortype", Errortype)
    '    cmd.Parameters.AddWithValue("@Comments", Comments)
    '    cmd.Parameters.AddWithValue("@ErrorDate", ErrorDate)
    '    cmd.Parameters.AddWithValue("@QueryRefID", QueryRefID)
    '    cmd.Parameters.AddWithValue("@rec_status", "P")
    '    cmd.Parameters.AddWithValue("@village", getIDSubstring(ErrorCompound, idTypes.VILLAGE))
    '    'cmd.Parameters.AddWithValue("@village", ErrorVillage)
    '    cmd.Parameters.AddWithValue("@round", ErrorRound)
    '    cmd.Parameters.AddWithValue("@compound", ErrorCompound)
    '    'clsGlobalVariable.open_HRS_Temp_DBCon()
    '    If cmd.ExecuteNonQuery > 0 Then
    '        success = True
    '    Else
    '        success = False
    '    End If
    '    Me.clsGlobalVariable.HRS_Temp_DBCon.Close()
    '    Return success
    'End Function

    Public Function updateErrorFlag(ByVal tablename As String, ByVal recordid As Integer, ByVal errStatus As Boolean) As Boolean
        Dim cmd As New SqlCommand
        cmd.Connection = Me.clsGlobalVariable.HRS_Temp_DBCon
        If Not Me.clsGlobalVariable.HRS_Temp_DBCon.State = ConnectionState.Open Then Me.clsGlobalVariable.HRS_Temp_DBCon.Open()

        cmd.CommandText = "UPDATE " & tablename & " SET [errflag] = @errflag  WHERE [transit_id]=" & recordid
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@errflag", errStatus)


        If cmd.ExecuteNonQuery() > 0 Then
            Me.clsGlobalVariable.HRS_Temp_DBCon.Close()
            Return True
        End If
    End Function

    Public Function deleterecord(ByVal tablename As String, ByVal recordid As Integer) As Boolean
        Dim cmd As New SqlCommand
        cmd.Connection = Me.clsGlobalVariable.HRS_Temp_DBCon
        If Not Me.clsGlobalVariable.HRS_Temp_DBCon.State = ConnectionState.Open Then Me.clsGlobalVariable.HRS_Temp_DBCon.Open()

        cmd.CommandText = "UPDATE " & tablename & " SET [rec_status] = rec_status + 'X'  WHERE [transit_id]=" & recordid
        cmd.Parameters.Clear()


        If cmd.ExecuteNonQuery() > 0 Then
            Me.clsGlobalVariable.HRS_Temp_DBCon.Close()
            Return True
        End If
    End Function

    Public Function updateDataErrors(ByVal item As Table) As Boolean

        Dim cmd As New SqlCommand
        cmd.Connection = Me.clsGlobalVariable.HRS_Temp_DBCon
        If Not Me.clsGlobalVariable.HRS_Temp_DBCon.State = ConnectionState.Open Then Me.clsGlobalVariable.HRS_Temp_DBCon.Open()

        cmd.CommandText = "update  MHRS_SYS.Temp_Data_Errors set rec_status='C'" _
        & " where recordid in(select ch.transit_id from " & item.Schema & "." & item.Name _
        & " as ch join MHRS_SYS.Data_Errors as dt on  ch.transit_id=dt.Recordid  " _
        & " where dt.tablename ='" & item.Schema & "." & item.Name & "' and ch.errflag=0 and not(ch.rec_status like '%x%')) "
        cmd.ExecuteNonQuery()
        Me.clsGlobalVariable.HRS_Temp_DBCon.Close()
        Return True

    End Function
    Public Function updateDataErrors2(ByVal item As Table) As Boolean

        Dim cmd As New SqlCommand
        cmd.Connection = Me.clsGlobalVariable.HRS_Temp_DBCon
        If Not Me.clsGlobalVariable.HRS_Temp_DBCon.State = ConnectionState.Open Then Me.clsGlobalVariable.HRS_Temp_DBCon.Open()

        cmd.CommandText = "update  MHRS_SYS.Temp_Data_Errors set rec_status='C'" _
        & " where (tablename='" & item.Schema & "." & item.Name & "')" _
        & " and recordid not in (select ch.transit_id from " & item.Schema & "." & item.Name & " as ch where not(ch.rec_status like '%x%'))"

        cmd.ExecuteNonQuery()
        Me.clsGlobalVariable.HRS_Temp_DBCon.Close()
        Return True

    End Function
    Public Function refreshDataErrors(ByVal item As Table) As Boolean

        Dim cmd As New SqlCommand
        cmd.Connection = Me.clsGlobalVariable.HRS_Temp_DBCon
        If Not Me.clsGlobalVariable.HRS_Temp_DBCon.State = ConnectionState.Open Then Me.clsGlobalVariable.HRS_Temp_DBCon.Open()

        cmd.CommandText = "update  MHRS_SYS.Data_Errors set rec_status='C'" _
        & " where (tablename='" & item.Schema & "." & item.Name & "')" _
        & " and recordid not in (select ch.transit_id from " & item.Schema & "." & item.Name & " as ch where not(ch.rec_status like '%x%'))"
        cmd.ExecuteNonQuery()
        Me.clsGlobalVariable.HRS_Temp_DBCon.Close()
        Return True

    End Function
    Public Function getrecordsCompound(ByVal tableName As String, ByVal row As DataRow) As String
        If Me.clsGlobalVariable.HRS_Temp_DBCon Is Nothing Then
            initializeServerAndDB()
        End If
        Dim cmd As New SqlCommand("", Me.clsGlobalVariable.HRS_Temp_DBCon)
        Dim village As String = ""
        Dim compound As String = ""
        If cmd.Connection.State <> ConnectionState.Open Then cmd.Connection.Open()
        Try
            Select Case tableName.ToUpper
                Case "DSS.MIGRATIONS", "DSS.BIRTH"
                    cmd.CommandText = "select res.locationid as village " _
                    & " FROM  dsshrs.dbo.[vCombinedResidency] res " _
                    & " left join(select eventid,episodeid from dsshrs.dbo.vCombinedEvents_Episodes " _
                    & "  where [EpisodeType]='RES') evep on  evep.episodeid=res.residencyid " _
                    & " where  evep.eventid='" & row("eventid").ToString & "'"

                    If Not IsDBNull(row("observationid")) Then
                        village = row("observationid").ToString
                    Else
                        village = cmd.ExecuteScalar().ToString
                    End If

                    If Not (village Is Nothing Or village.Trim = "") Then
                        compound = Me.objVal.getIDSubstring(village, idTypes.COMPOUND)
                        village = Me.objVal.getIDSubstring(village, idTypes.VILLAGE)
                    Else
                        Try
                            compound = row("individid").ToString.Replace("T", "").Trim
                            compound = compound
                            compound = Me.objVal.getIDSubstring(compound, idTypes.COMPOUND)
                        Catch ex As Exception

                        End Try
                    End If


                Case "DSS.COMPADMIN", "DSS.COMPOUNDS", "SPECIALSTUDIES.HUAS_LITE", "SPECIALSTUDIES.GPSDATA"
                    village = Me.objVal.getIDSubstring(row("compoundid").ToString.Trim, idTypes.VILLAGE)
                    compound = row("compoundid").ToString.Trim

                Case "DSS.VILLAGES"
                    village = row("villcode").ToString.Trim
                    compound = row("villcode").ToString.Trim


                Case "DSS.INDIVIDUAL"
                    cmd.CommandText = "select locationid from dbo.lastResidency where individid='" & row("individid").ToString & "'"
                    If Not IsDBNull(row("observationid")) Then
                        village = row("observationid").ToString
                    Else
                        village = cmd.ExecuteScalar().ToString
                    End If
                    ' village = cmd.ExecuteScalar().ToString
                    If Not village Is Nothing Then
                        compound = Me.objVal.getIDSubstring(village, idTypes.COMPOUND)
                        village = Me.objVal.getIDSubstring(village, idTypes.VILLAGE)
                    Else
                        Try
                            village = row("individid").ToString.Replace("T", "").Trim
                            compound = village
                            compound = Me.objVal.getIDSubstring(village, idTypes.COMPOUND)
                        Catch ex As Exception

                        End Try
                    End If

                Case "DSS.INDVSTATUS", "DSS.MARRIAGE"
                    village = Me.objVal.getIDSubstring(row("observeid").ToString.Trim, idTypes.VILLAGE)
                    compound = Me.objVal.getIDSubstring(row("observeid").ToString.Trim, idTypes.COMPOUND)

                Case "DSS.LOCATION", "DSS.PREGNANCY", "DSS.OBSERVATION", "DSS.RESIDENCY", "DSS.VISITATION", "SPECIALSTUDIES.HSEDETAILS" _
                         , "SPECIALSTUDIES.IMMUNIZE", "SPECIALSTUDIES.ITN", "SPECIALSTUDIES.MORBIDITY", "SPECIALSTUDIES.RELIGION", "MHRS_SYS.CHANGES", "[MHRS_SYS].[CHANGES]", _
                                        "ghi.Child_health".ToUpper, _
                    "ghi.family_planning".ToUpper, _
                    "ghi.ghi_itn".ToUpper, _
                    "ghi.House_sanitation".ToUpper, _
                    "ghi.pregnancy_and_Birth".ToUpper, _
                    "ghi.Relationships".ToUpper, _
                    "ghi.vct_hiv".ToUpper, _
                    "PBR.ANC", "PBR.BIRTH_DELIVERY", _
                    "bh.BirthHistory".ToUpper

                    village = Me.objVal.getIDSubstring(row("locationid").ToString.Trim, idTypes.VILLAGE)
                    compound = Me.objVal.getIDSubstring(row("locationid").ToString.Trim, idTypes.COMPOUND)


                Case "ghi.Toilet".ToUpper, "ghi.Treat_water".ToUpper

                    cmd.CommandText = "SELECT max([locationid])FROM [DSSHRS].ghi.allHouse_sanitation where  id='" & row("id").ToString & "' "
                    village = cmd.ExecuteScalar().ToString
                    If Not (village Is Nothing OrElse IsDBNull(village) OrElse village = "") Then
                        compound = Me.objVal.getIDSubstring(village, idTypes.COMPOUND)
                        village = Me.objVal.getIDSubstring(village, idTypes.VILLAGE)
                    End If
                Case "ghi.fp_current_use".ToUpper
                    cmd.CommandText = "SELECT max([locationid])FROM [DSSHRS].ghi.allfamily_planning where  id='" & row("id").ToString & "' "
                    village = cmd.ExecuteScalar().ToString
                    If Not (village Is Nothing OrElse IsDBNull(village) OrElse village = "") Then
                        compound = Me.objVal.getIDSubstring(village, idTypes.COMPOUND)
                        village = Me.objVal.getIDSubstring(village, idTypes.VILLAGE)
                    End If

                Case "ghi.afterdelivery_service".ToUpper, "ghi.anc_place".ToUpper, "ghi.baby_drink".ToUpper
                    cmd.CommandText = "SELECT max([locationid])FROM [DSSHRS].ghi.allpregnancy_and_Birth where  id='" & row("id").ToString & "' "
                    village = cmd.ExecuteScalar().ToString
                    If Not (village Is Nothing OrElse IsDBNull(village) OrElse village = "") Then
                        compound = Me.objVal.getIDSubstring(village, idTypes.COMPOUND)
                        village = Me.objVal.getIDSubstring(village, idTypes.VILLAGE)
                    End If

                Case "pbr.anc_place".ToUpper, "pbr.afterdelivery_service".ToUpper, "bh.Children".ToUpper
                    Dim sqlstr As String
                    If tableName.ToLower.Equals("pbr.anc_place") Then
                        sqlstr = "SELECT max([locationid])FROM [DSSHRS].[PBR].[all_ANC] where  id='" & row("id").ToString & "' "
                    ElseIf tableName.ToLower.Equals("pbr.afterdelivery_service") Then
                        sqlstr = "SELECT max([locationid])FROM [DSSHRS].[PBR].[all_BirthDelivery] where  id='" & row("id").ToString & "' "
                    ElseIf tableName.ToLower.Equals("bh.children") Then
                        sqlstr = "SELECT max([locationid])FROM [DSSHRS].[BH].[all_BirthHistory] where  id='" & row("id").ToString & "' "
                    Else
                        Exit Select
                    End If

                    cmd.CommandText = sqlstr
                    village = cmd.ExecuteScalar().ToString
                    If Not (village Is Nothing OrElse IsDBNull(village) OrElse village = "") Then
                        compound = Me.objVal.getIDSubstring(village, idTypes.COMPOUND)
                        village = Me.objVal.getIDSubstring(village, idTypes.VILLAGE)
                    End If
                Case "ghi.fever_other".ToUpper, "ghi.Diarrhea_treat".ToUpper, "ghi.Fever_drug".ToUpper, "ghi.Fever_treatment".ToUpper

                    cmd.CommandText = "SELECT max([locationid])FROM [DSSHRS].ghi.allChild_health where  id='" & row("id").ToString & "' "
                    village = cmd.ExecuteScalar().ToString
                    If Not (village Is Nothing OrElse IsDBNull(village) OrElse village = "") Then
                        compound = Me.objVal.getIDSubstring(village, idTypes.COMPOUND)
                        village = Me.objVal.getIDSubstring(village, idTypes.VILLAGE)
                    End If

                Case "SpecialStudies.bednet".ToUpper.Trim, "SpecialStudies.bednet_individual_netUse".ToUpper.Trim, "SpecialStudies.bednet_netinfo".ToUpper.Trim
                    cmd.CommandText = "SELECT max([locationid])FROM [DSSHRS].[SpecialStudies].[allBednet] where  id='" & row("id").ToString & "' "
                    village = cmd.ExecuteScalar().ToString
                    If Not (village Is Nothing OrElse IsDBNull(village) OrElse village = "") Then
                        compound = Me.objVal.getIDSubstring(village, idTypes.COMPOUND)
                        village = Me.objVal.getIDSubstring(village, idTypes.VILLAGE)
                    End If

                Case "specialStudies.HHD_Waterusage".ToUpper.Trim, "specialStudies.FetchWater_Member".ToUpper.Trim _
                         , "specialStudies.CleanWater_Methods".ToUpper.Trim, "specialStudies.WaterAccess_Activities".ToUpper.Trim
                    cmd.CommandText = "SELECT max([locationid])FROM [DSSHRS].[SpecialStudies].[AllWaterAccess] where  wateraccessID='" & row("wateraccessID").ToString & "' "
                    village = cmd.ExecuteScalar().ToString
                    If Not (village Is Nothing OrElse IsDBNull(village) OrElse village = "") Then
                        compound = Me.objVal.getIDSubstring(village, idTypes.COMPOUND)
                        village = Me.objVal.getIDSubstring(village, idTypes.VILLAGE)
                    End If
                Case "DSS.MEMBERSHIP"

                    If IsDBNull(row("eobserveid")) Then
                        village = Me.objVal.getIDSubstring(row("sobserveid").ToString.Trim, idTypes.VILLAGE)
                        compound = Me.objVal.getIDSubstring(row("sobserveid").ToString.Trim, idTypes.COMPOUND)
                    Else
                        village = Me.objVal.getIDSubstring(row("eobserveid").ToString.Trim, idTypes.VILLAGE)
                        compound = Me.objVal.getIDSubstring(row("eobserveid").ToString.Trim, idTypes.COMPOUND)
                    End If

                Case "DSS.SOCIALGROUP", "DSS.SOCIALGROUPADMIN"


                    village = Me.objVal.getIDSubstring(row("socialgpid").ToString.Trim, idTypes.VILLAGE)
                    compound = Me.objVal.getIDSubstring(row("socialgpid").ToString.Trim, idTypes.COMPOUND)


                Case "DSS.PREGOUTCOME"
                    cmd.CommandText = "select res.locationid as village " _
                                  & " FROM  dss.PREGNANCY res " _
                                  & " left join(select eventid,episodeid from dsshrs.dbo.vCombinedEvents_Episodes " _
                                  & "  where [EpisodeType]='PRG') evep on  evep.episodeid=res.PregnancyID " _
                                  & " where  evep.eventid='" & row("eventid").ToString & "'"
                    If Not IsDBNull(row("observationid")) Then
                        village = row("observationid").ToString
                    Else
                        village = cmd.ExecuteScalar().ToString
                    End If
                    'village = cmd.ExecuteScalar()
                    If Not (village Is Nothing OrElse IsDBNull(village) OrElse village = "") Then
                        compound = Me.objVal.getIDSubstring(village, idTypes.COMPOUND)
                        village = Me.objVal.getIDSubstring(village, idTypes.VILLAGE)
                    Else
                        Try
                            village = row("individid").ToString.Replace("T", "")
                            compound = village
                            compound = Me.objVal.getIDSubstring(village, idTypes.COMPOUND)
                        Catch ex As Exception

                        End Try
                    End If
                Case "SPECIALSTUDIES.EDUCATION"
                    village = row("vill").ToString.Trim
                    compound = Me.objVal.getIDSubstring(row("observeid").ToString.Trim, idTypes.COMPOUND)
                    If (compound Is Nothing OrElse IsDBNull(compound) OrElse compound = "") Then
                        Try
                            village = row("individid").ToString.Replace("T", "").Trim
                            compound = village
                            compound = Me.objVal.getIDSubstring(village, idTypes.COMPOUND)
                        Catch ex As Exception

                        End Try
                    End If
                Case "SPECIALSTUDIES.PARENTSURV"
                    village = Me.objVal.getIDSubstring(row("hhid").ToString.Trim, idTypes.VILLAGE)
                    compound = Me.objVal.getIDSubstring(row("hhid").ToString.Trim, idTypes.COMPOUND)

                Case "SPECIALSTUDIES.SES"
                    village = Me.objVal.getIDSubstring(row("househid").ToString.Trim, idTypes.VILLAGE)
                    compound = Me.objVal.getIDSubstring(row("househid").ToString.Trim, idTypes.COMPOUND)


                Case "SpecialStudies.MobilePhoneUsage".ToUpper
                    Try
                        village = row("individid").ToString.Replace("T", "")
                        compound = village
                        compound = Me.objVal.getIDSubstring(village, idTypes.COMPOUND)
                    Catch ex As Exception

                    End Try
                Case "specialstudies.circumcision".ToUpper
                    If IsDBNull(row("observationid")) Then
                        village = row("Individid").ToString.Replace("T", "")
                        compound = village
                        compound = Me.objVal.getIDSubstring(village, idTypes.COMPOUND)
                    Else
                        village = row("observationid").ToString
                        compound = village
                        compound = Me.objVal.getIDSubstring(village, idTypes.COMPOUND)

                    End If
                Case "specialstudies.druguse".ToUpper
                    If IsDBNull(row("locationid")) Then
                        village = row("individid").ToString.Replace("T", "")
                        compound = village
                        compound = Me.objVal.getIDSubstring(village, idTypes.COMPOUND)
                    Else
                        village = row("locationid").ToString
                        compound = village
                        compound = Me.objVal.getIDSubstring(village, idTypes.COMPOUND)

                    End If
                Case "specialstudies.EVP_Anthropometric".ToUpper, "specialstudies.EVP".ToUpper, "specialstudies.EVPinterviewOutcome".ToUpper


                    Try
                        village = row("WAS_ANT_HHID").ToString.Replace("T", "")
                        compound = village
                        compound = Me.objVal.getIDSubstring(village, idTypes.COMPOUND)
                    Catch ex As Exception

                    End Try
                Case "Radio.prevention_practice".ToUpper
                    Try
                        row("locationid").ToString()
                        village = Me.objVal.getIDSubstring(row("locationid").ToString, idTypes.VILLAGE)
                        compound = Me.objVal.getIDSubstring(row("locationid").ToString, idTypes.COMPOUND)
                    Catch ex As Exception

                    End Try
                Case "Ms.washschool".ToUpper, "MS.WASHContainers".ToUpper, "MS.GIRLS_BEHAVIOUR", "MS.WASHHOME", "MS.NURSE"

                    village = "W"
                    compound = "W"
                    'Case ""
                    '    Try


                    '    Dim strParentTbl, strPK_Key As String
                    '    Select Case tableName.ToLower.Trim
                    '        Case "specialStudies.bednet_individual_netUse".ToLower.Trim, "specialStudies.bednet_netinfo".ToLower.Trim
                    '            strParentTbl = "specialstudies.bednet"
                    '            strPK_Key = "id"
                    '        Case "specialStudies.HHD_Waterusage".ToLower.Trim, "specialStudies.FetchWater_Member".ToLower.Trim _
                    '           , "specialStudies.CleanWater_Methods".ToLower.Trim, "specialStudies.WaterAccess_Activities".ToLower.Trim
                    '            strParentTbl = "specialstudies.WaterAccess"
                    '            strPK_Key = "wateraccessID"
                    '        Case "specialStudies.LiveStock".ToLower.Trim, "specialStudies.Cropgrown".ToLower.Trim
                    '            strParentTbl = "specialstudies.Crop_Live_production"
                    '            strPK_Key = "Crop_Live_ProductionID"
                    '        Case "MS.WASHLatrine".ToLower.Trim, "MS.WASHContainers".ToLower.Trim
                    '            strParentTbl = "MS.WASHSCHOOL"
                    '            strPK_Key = "wschid"
                    '    End Select

                    '    'check to ensure that the  parent record have all gone to the main databse
                    '    If tableName.ToLower.Trim.Equals("specialStudies.bednet_individual_netUse".ToLower.Trim) Or _
                    '        tableName.ToLower.Trim.Equals("specialStudies.bednet_netinfo".ToLower.Trim) Or _
                    '        tableName.ToLower.Trim.Equals("SpecialStudies.WaterAccess_Activities".ToLower.Trim) Or _
                    '        tableName.ToLower.Trim.Equals("specialStudies.HHD_Waterusage".ToLower.Trim) Or _
                    '        tableName.ToLower.Trim.Equals("specialStudies.FetchWater_Member".ToLower.Trim) Or _
                    '        tableName.ToLower.Trim.Equals("specialStudies.CleanWater_Methods".ToLower.Trim) Or _
                    '        tableName.ToLower.Trim.Equals("MS.WASHLatrine".ToLower.Trim) Or _
                    '        tableName.ToLower.Trim.Equals("MS.WASHContainers".ToLower.Trim) Then


                    '        'check to see if the recorded has a parent 
                    '        Dim cmd As New SqlCommand
                    '        'cmd.CommandType = CommandType.StoredProcedure
                    '        cmd.Connection = Me.clsGlobalVariable.HRS_Temp_DBCon
                    '        If Not Me.clsGlobalVariable.HRS_Temp_DBCon.State = ConnectionState.Open Then Me.clsGlobalVariable.HRS_Temp_DBCon.Open()
                    '        cmd.CommandText = "[DSSHRS].[dbo].[getParentRecords]  @tblParent,@tblChild,@tblTransit_id,@strPrimaryKey"
                    '        cmd.Parameters.Clear()
                    '        cmd.Parameters.AddWithValue("@tblParent", strParentTbl)
                    '        cmd.Parameters.AddWithValue("@tblChild", tableName.ToLower)
                    '        cmd.Parameters.AddWithValue("@tblTransit_id", row("transit_id"))
                    '        cmd.Parameters.AddWithValue("strPrimaryKey", strPK_Key)

                    '        Me.clsGlobalVariable.open_HRS_TEMP_DBCon()

                    '        Dim newValue As Integer
                    '        newValue = cmd.ExecuteScalar()

                    '        If newValue = 0 Then
                    '            Return False
                    '        End If
                    '        Me.clsGlobalVariable.HRS_Temp_DBCon.Close()
                    '    End If
                    'Catch ex As Exception
                    '    objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

                    'End Try

                Case "DSS.CONSENTS"
                Case "DSS.EVENTS_EPISODES"
                Case "DSS.REGIONS"
                Case "DSS.RELATIONSHIP"
                Case "DSS.ROUND"
                Case "SPECIALSTUDIES.HEALTH"

            End Select
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            village = ""
            compound = ""
            ' 'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        ' If village Is Nothing Or IsDBNull(village) Then village = "" : compound = ""
        If compound <> "W" Then
            If compound Is Nothing OrElse IsDBNull(compound) Or Not (clsUserDefinedFunctions.isValidcompoundid(compound)) Then
                compound = "Q"
            End If
        End If
        cmd.Connection.Close()
        'Dim s As String() = {village.ToString, compound}
        'Return s
        Return compound
    End Function

    Public Function getDataErrorsrecordsCompound(ByVal tableName As String, ByVal recordid As String) As String
        If Me.clsGlobalVariable.HRS_Temp_DBCon Is Nothing Then
            initializeServerAndDB()
        End If
        Dim cmd As New SqlCommand("", Me.clsGlobalVariable.HRS_Temp_DBCon)
        Dim village As String = ""
        Dim compound As String = ""
        If cmd.Connection.State <> ConnectionState.Open Then cmd.Connection.Open()

        Try
            Select Case tableName.ToUpper
                Case "DSS.MIGRATIONS", "DSS.BIRTH"

                    cmd.CommandText = "SELECT eventid from " & tableName & " where transit_id ='" & recordid.Trim & "'"
                    Dim eventid As String = cmd.ExecuteScalar().ToString

                    cmd.CommandText = "select res.locationid as village " _
                    & " FROM  dsshrs.dbo.[vCombinedResidency] res " _
                    & " left join(select eventid,episodeid from dsshrs.dbo.vCombinedEvents_Episodes " _
                    & "  where [EpisodeType]='RES') evep on  evep.episodeid=res.residencyid " _
                    & " where  evep.eventid='" & eventid & "'"

                    compound = cmd.ExecuteScalar().ToString.Replace("T", "").Trim
                    If Not (compound Is Nothing Or compound.Trim = "") Then
                        '   village = getIDSubstring(village, idTypes.VILLAGE)
                        compound = Me.objVal.getIDSubstring(compound.Replace("T", "").Trim, idTypes.COMPOUND)
                    Else
                        Try
                            cmd.CommandText = "SELECT individid from " & tableName & " where transit_id ='" & recordid.Trim & "'"
                            Dim indi As String = cmd.ExecuteScalar().ToString
                            compound = Me.objVal.getIDSubstring(indi.Replace("T", "").Trim, idTypes.COMPOUND)
                        Catch ex As Exception

                        End Try
                    End If

                Case "DSS.COMPADMIN", "DSS.COMPOUNDS", "SPECIALSTUDIES.HUAS_LITE", "SPECIALSTUDIES.GPSDATA"

                    cmd.CommandText = "SELECT compoundid from " & tableName & " where transit_id ='" & recordid.Trim & "'"
                    compound = cmd.ExecuteScalar().ToString


                Case "DSS.VILLAGES"
                    cmd.CommandText = "SELECT villcode from " & tableName & " where transit_id ='" & recordid.Trim & "'"
                    compound = cmd.ExecuteScalar().ToString




                Case "DSS.INDIVIDUAL"

                    cmd.CommandText = "SELECT individid from " & tableName & " where transit_id ='" & recordid.Trim & "'"
                    Dim indi As String = cmd.ExecuteScalar().ToString

                    cmd.CommandText = "select locationid from dbo.lastResidency where individid='" & indi & "'"

                    village = cmd.ExecuteScalar().ToString
                    If Not (village Is Nothing Or village.Trim = "") Then
                        '  village = getIDSubstring(village, idTypes.VILLAGE)
                        compound = Me.objVal.getIDSubstring(village.Replace("T", "").Trim, idTypes.COMPOUND)
                    Else
                        Try
                            compound = Me.objVal.getIDSubstring(indi.Replace("T", "").Trim, idTypes.COMPOUND)
                        Catch ex As Exception

                        End Try

                    End If


                Case "DSS.INDVSTATUS", "DSS.MARRIAGE"

                    cmd.CommandText = "SELECT observeid from " & tableName & " where transit_id ='" & recordid.Trim & "'"
                    Dim indi As String = cmd.ExecuteScalar().ToString

                    'village = getIDSubstring(row("observeid").ToString.Trim, idTypes.VILLAGE)
                    compound = Me.objVal.getIDSubstring(indi, idTypes.COMPOUND)

                Case "DSS.LOCATION", "DSS.PREGNANCY", "DSS.OBSERVATION", "DSS.RESIDENCY", "DSS.VISITATION", "SPECIALSTUDIES.HSEDETAILS" _
                         , "SPECIALSTUDIES.IMMUNIZE", "SPECIALSTUDIES.ITN", "SPECIALSTUDIES.MORBIDITY", "SPECIALSTUDIES.RELIGION", "MHRS_SYS.CHANGES", "[MHRS_SYS].[CHANGES]", _
                                             "ghi.Child_health".ToUpper, _
                    "ghi.family_planning".ToUpper, _
                    "ghi.ghi_itn".ToUpper, _
                    "ghi.House_sanitation".ToUpper, _
                    "ghi.pregnancy_and_Birth".ToUpper, _
                    "ghi.Relationships".ToUpper, _
                    "ghi.vct_hiv".ToUpper


                    cmd.CommandText = "SELECT locationid from " & tableName & " where transit_id ='" & recordid.Trim & "'"
                    Dim indi As String = cmd.ExecuteScalar().ToString

                    'village = getIDSubstring(row("observeid").ToString.Trim, idTypes.VILLAGE)
                    compound = Me.objVal.getIDSubstring(indi, idTypes.COMPOUND)




                Case "ghi.Toilet".ToUpper, "ghi.Treat_water".ToUpper

                    cmd.CommandText = "SELECT id from " & tableName & " where transit_id ='" & recordid.Trim & "'"
                    Dim id As String = cmd.ExecuteScalar().ToString.Trim

                    cmd.CommandText = "SELECT max([locationid])FROM [DSSHRS].ghi.allHouse_sanitation where  id='" & id & "' "
                    village = cmd.ExecuteScalar().ToString
                    If Not (village Is Nothing OrElse IsDBNull(village) OrElse village = "") Then
                        compound = Me.objVal.getIDSubstring(village, idTypes.COMPOUND)
                        village = Me.objVal.getIDSubstring(village, idTypes.VILLAGE)
                    End If
                Case "ghi.fp_current_use".ToUpper
                    cmd.CommandText = "SELECT id from " & tableName & " where transit_id ='" & recordid.Trim & "'"
                    Dim id As String = cmd.ExecuteScalar().ToString.Trim

                    cmd.CommandText = "SELECT max([locationid])FROM [DSSHRS].ghi.allfamily_planning where  id='" & id & "' "
                    village = cmd.ExecuteScalar().ToString
                    If Not (village Is Nothing OrElse IsDBNull(village) OrElse village = "") Then
                        compound = Me.objVal.getIDSubstring(village, idTypes.COMPOUND)
                        village = Me.objVal.getIDSubstring(village, idTypes.VILLAGE)
                    End If

                Case "ghi.afterdelivery_service".ToUpper, "ghi.anc_place".ToUpper, "ghi.baby_drink".ToUpper
                    cmd.CommandText = "SELECT id from " & tableName & " where transit_id ='" & recordid.Trim & "'"
                    Dim id As String = cmd.ExecuteScalar().ToString.Trim

                    cmd.CommandText = "SELECT max([locationid])FROM [DSSHRS].ghi.allpregnancy_and_Birth where  id='" & id & "' "
                    village = cmd.ExecuteScalar().ToString
                    If Not (village Is Nothing OrElse IsDBNull(village) OrElse village = "") Then
                        compound = Me.objVal.getIDSubstring(village, idTypes.COMPOUND)
                        village = Me.objVal.getIDSubstring(village, idTypes.VILLAGE)
                    End If
                Case "ghi.fever_other".ToUpper, "ghi.Diarrhea_treat".ToUpper, "ghi.Fever_drug".ToUpper, "ghi.Fever_treatment".ToUpper

                    cmd.CommandText = "SELECT id from " & tableName & " where transit_id ='" & recordid.Trim & "'"
                    Dim id As String = cmd.ExecuteScalar().ToString.Trim

                    cmd.CommandText = "SELECT max([locationid])FROM [DSSHRS].ghi.allChild_health where  id='" & id & "' "
                    village = cmd.ExecuteScalar().ToString
                    If Not (village Is Nothing OrElse IsDBNull(village) OrElse village = "") Then
                        compound = Me.objVal.getIDSubstring(village, idTypes.COMPOUND)
                        village = Me.objVal.getIDSubstring(village, idTypes.VILLAGE)
                    End If


                Case "DSS.MEMBERSHIP", "DSS.SOCIALGROUP", "DSS.SOCIALGROUPADMIN"

                    cmd.CommandText = "SELECT socialgpid from " & tableName & " where transit_id ='" & recordid.Trim & "'"
                    Dim indi As String = cmd.ExecuteScalar().ToString

                    'village = getIDSubstring(row("observeid").ToString.Trim, idTypes.VILLAGE)
                    compound = Me.objVal.getIDSubstring(indi, idTypes.COMPOUND)
                Case "DSS.PREGOUTCOME"

                    cmd.CommandText = "SELECT eventid from " & tableName & " where transit_id ='" & recordid.Trim & "'"
                    Dim indi As String = cmd.ExecuteScalar().ToString

                    'village = getIDSubstring(row("observeid").ToString.Trim, idTypes.VILLAGE)
                    '  compound = getIDSubstring(indi, idTypes.COMPOUND)


                    cmd.CommandText = "select res.locationid as village " _
                                  & " FROM  dsshrs.dbo.vCombinedPregnancy  res " _
                                  & " left join(select eventid,episodeid from dsshrs.dbo.vCombinedEvents_Episodes " _
                                  & "  where [EpisodeType]='PRG') evep on  evep.episodeid=res.PregnancyID " _
                                  & " where  evep.eventid='" & indi & "'"

                    village = cmd.ExecuteScalar()
                    If Not (village Is Nothing OrElse IsDBNull(village) OrElse village = "") Then
                        '   village = getIDSubstring(village, idTypes.VILLAGE)
                        compound = Me.objVal.getIDSubstring(village, idTypes.COMPOUND)
                    Else
                        Try
                            cmd.CommandText = "SELECT individid from " & tableName & " where transit_id ='" & recordid.Trim & "'"
                            indi = cmd.ExecuteScalar().ToString
                            compound = Me.objVal.getIDSubstring(indi.Replace("T", ""), idTypes.COMPOUND)
                        Catch ex As Exception

                        End Try
                    End If

                Case "SPECIALSTUDIES.EDUCATION"

                    cmd.CommandText = "SELECT observeid from " & tableName & " where transit_id ='" & recordid.Trim & "'"
                    Dim indi As String = cmd.ExecuteScalar().ToString

                    'village = row("vill").ToString.Trim

                    If Not (indi Is Nothing Or indi.Trim = "") Then
                        compound = Me.objVal.getIDSubstring(indi.Replace("T", "").Trim, idTypes.COMPOUND)
                    Else
                        Try
                            cmd.CommandText = "SELECT individid from " & tableName & " where transit_id ='" & recordid.Trim & "'"
                            indi = cmd.ExecuteScalar().ToString
                            compound = Me.objVal.getIDSubstring(indi.Replace("T", "").Trim, idTypes.COMPOUND)
                        Catch ex As Exception
                        End Try
                    End If
                Case "SPECIALSTUDIES.PARENTSURV"

                    cmd.CommandText = "SELECT hhid from " & tableName & " where transit_id ='" & recordid.Trim & "'"
                    Dim indi As String = cmd.ExecuteScalar().ToString

                    'village = row("vill").ToString.Trim
                    compound = Me.objVal.getIDSubstring(indi, idTypes.COMPOUND)

                Case "SPECIALSTUDIES.SES"

                    cmd.CommandText = "SELECT househid from " & tableName & " where transit_id ='" & recordid.Trim & "'"
                    Dim indi As String = cmd.ExecuteScalar().ToString

                    'village = row("vill").ToString.Trim
                    compound = Me.objVal.getIDSubstring(indi, idTypes.COMPOUND)

                Case "SpecialStudies.MobilePhoneUsage".ToUpper
                    Try
                        cmd.CommandText = "SELECT individid from " & tableName & " where transit_id ='" & recordid.Trim & "'"
                        Dim indi As String = cmd.ExecuteScalar().ToString
                        village = indi.Replace("T", "")
                        compound = village
                        compound = Me.objVal.getIDSubstring(village, idTypes.COMPOUND)
                    Catch ex As Exception

                    End Try
                Case "specialstudies.druguse".ToUpper
                    cmd.CommandText = "SELECT locationid from " & tableName & " where transit_id ='" & recordid.Trim & "'"
                    Dim indi As String = cmd.ExecuteScalar().ToString
                    If indi Is Nothing Then
                        cmd.CommandText = "SELECT Individid from " & tableName & " where transit_id ='" & recordid.Trim & "'"
                        indi = cmd.ExecuteScalar().ToString
                        compound = Me.objVal.getIDSubstring(indi, idTypes.COMPOUND)
                    Else
                        compound = Me.objVal.getIDSubstring(indi, idTypes.COMPOUND)
                    End If
                Case "specialstudies.circumcision".ToUpper
                    cmd.CommandText = "SELECT observationid from " & tableName & " where transit_id ='" & recordid.Trim & "'"
                    Dim indi As String = cmd.ExecuteScalar().ToString
                    If indi Is Nothing Then
                        cmd.CommandText = "SELECT Individid from " & tableName & " where transit_id ='" & recordid.Trim & "'"
                        indi = cmd.ExecuteScalar().ToString
                        compound = Me.objVal.getIDSubstring(indi, idTypes.COMPOUND)
                    Else
                        compound = Me.objVal.getIDSubstring(indi, idTypes.COMPOUND)
                    End If

                Case "DSS.CONSENTS"
                Case "DSS.EVENTS_EPISODES"
                Case "DSS.REGIONS"
                Case "DSS.RELATIONSHIP"
                Case "DSS.ROUND"
                Case "SPECIALSTUDIES.HEALTH"

            End Select
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            village = ""
            compound = ""
            ' 'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        'If village Is Nothing Or IsDBNull(village) Then village = "" : compound = ""
        If compound Is Nothing Or IsDBNull(compound) Or Not (clsUserDefinedFunctions.isValidcompoundid(compound)) Then
            compound = "Q"
        End If
        cmd.Connection.Close()
        'Dim s As String() = {village.ToString, compound}
        'Return s
        Return compound
    End Function
    Public Function getrecordsRound(ByVal tableName As String, ByVal row As DataRow) As String
        If Me.clsGlobalVariable.HRS_Temp_DBCon Is Nothing Then
            initializeServerAndDB()
        End If
        Dim cmd As New SqlCommand("", Me.clsGlobalVariable.HRS_Temp_DBCon)
        cmd.Parameters.Clear()
        Dim round As String = "", i = ""
        If cmd.Connection.State <> ConnectionState.Open Then cmd.Connection.Open()
        Try
            Select Case tableName.ToLower.Trim
                Case "dss.migrations", "dss.birth"
                    cmd.CommandText = "select  " _
                    & "case when ((res.[eobserveid] is null) or (rtrim(res.[eobserveid])='')) then right(rtrim(res.[sobserveid]),5)  " _
                    & "     else right(rtrim(res.[eobserveid]),5)  " _
                    & "end as round  " _
                    & " FROM  dsshrs.dbo.[vCombinedResidency] res " _
                    & " left join(select eventid,episodeid from dsshrs.dbo.vCombinedEvents_Episodes " _
                    & "  where [EpisodeType]='RES') evep on  evep.episodeid=res.residencyid " _
                    & " where  evep.eventid='" & row("eventid").ToString & "'"
                    If Not IsDBNull(row("observationid")) Then
                        i = Right(row("observationid").ToString.Trim, 5)
                    Else
                        i = cmd.ExecuteScalar()
                    End If
                    i = cmd.ExecuteScalar()
                    If i Is Nothing Then
                        round = ""
                    Else
                        round = i
                    End If
                Case "dss.pregoutcome".ToLower.Trim
                    cmd.CommandText = "select  " _
                                  & "case when ((res.[eobserveid] is null) or (rtrim(res.[eobserveid])='')) then right(rtrim(res.[sobserveid]),5)  " _
                                  & "     else right(rtrim(res.[eobserveid]),5)  " _
                                  & "end as round  " _
                                  & " FROM  dsshrs.dbo.vCombinedPregnancy res " _
                                  & " left join(select eventid,episodeid from dsshrs.dbo.vCombinedEvents_Episodes " _
                                  & "  where [EpisodeType]='PRG') evep on  evep.episodeid=res.PregnancyID " _
                                  & " where  evep.eventid='" & row("eventid").ToString & "'"


                    If Not IsDBNull(row("observationid")) Then
                        round = Right(row("observationid").ToString.Trim, 5)
                    Else
                        round = cmd.ExecuteScalar()
                    End If
                Case "specialstudies.ses", "specialstudies.religion", "specialstudies.parentsurv", "specialstudies.morbidity" _
                  , "specialstudies.itn", "specialstudies.immunize", "specialstudies.huas_lite", "specialstudies.hsedetails" _
                 , "specialstudies.health", "specialstudies.gpsdata" _
                  , "specialstudies.druguse", "specialstudies.dmicampaign", "specialstudies.contraception" _
                 , "dss.compadmin", "dss.observation", "dss.socialgroupadmin", "dss.visitation", _
                    "PBR.ANC".ToLower.Trim, "PBR.BIRTH_DELIVERY".ToLower.Trim
                    round = row("round").ToString.Trim


                Case "dss.compounds", "dss.location", "dss.socialgroup"

                    cmd.CommandText = "SELECT max([round_num]) as round FROM [DSSHRS].[DSS].[round]WHERE @Entry_date BETWEEN [start_date] AND [end_date]"
                    cmd.Parameters.AddWithValue("@Entry_date", row("Entry_date"))
                    i = cmd.ExecuteScalar()
                    If i Is Nothing Then
                        round = ""
                    Else
                        round = i
                    End If



                Case "ghi.Child_health".ToLower.Trim, _
                    "ghi.family_planning".ToLower.Trim, _
                    "ghi.ghi_itn".ToLower.Trim, _
                    "ghi.House_sanitation".ToLower.Trim, _
                    "ghi.pregnancy_and_Birth".ToLower.Trim, _
                    "ghi.Relationships".ToLower.Trim, _
                    "ghi.vct_hiv".ToLower.Trim, _
                    "Specialstudies.waterAccess".ToLower, _
                    "bh.BirthHistory".ToLower.Trim, "bh.Children".ToLower.Trim

                    round = row("round").ToString

                Case "ghi.Toilet".ToLower.Trim, "ghi.Treat_water".ToLower.Trim

                    cmd.CommandText = "SELECT max([round])FROM [DSSHRS].ghi.allHouse_sanitation where  id='" & row("id").ToString & "' "
                    i = cmd.ExecuteScalar().ToString
                    If i Is Nothing Then
                        round = ""
                    Else
                        round = i
                    End If
                Case "ghi.fp_current_use".ToLower.Trim
                    cmd.CommandText = "SELECT max([round])FROM [DSSHRS].ghi.allfamily_planning where  id='" & row("id").ToString & "' "
                    i = cmd.ExecuteScalar().ToString
                    If i Is Nothing Then
                        round = ""
                    Else
                        round = i
                    End If

                Case "ghi.afterdelivery_service".ToLower.Trim, "ghi.anc_place".ToLower.Trim, "ghi.baby_drink".ToLower.Trim
                    cmd.CommandText = "SELECT max([round])FROM [DSSHRS].ghi.allpregnancy_and_Birth where  id='" & row("id").ToString & "' "
                    i = cmd.ExecuteScalar().ToString
                    If i Is Nothing Then
                        round = ""
                    Else
                        round = i
                    End If
                Case "pbr.anc_place".ToLower, "pbr.afterdelivery_service".ToLower
                    Dim sqlstr As String
                    If tableName.ToLower.Equals("pbr.anc_place") Then
                        sqlstr = "SELECT max([round])FROM [DSSHRS].[PBR].[all_ANC] where  id='" & row("id").ToString & "' "
                    ElseIf tableName.ToLower.Equals("pbr.afterdelivery_service") Then
                        sqlstr = "SELECT max([locationid])FROM [DSSHRS].[PBR].[all_BirthDelivery] where  id='" & row("id").ToString & "' "
                    Else
                        Exit Select
                    End If
                    cmd.CommandText = sqlstr
                    i = cmd.ExecuteScalar().ToString
                    If i Is Nothing Then
                        round = ""
                    Else
                        round = i
                    End If
                Case "ghi.fever_other".ToLower.Trim, "ghi.Diarrhea_treat".ToLower.Trim, "ghi.Fever_drug".ToLower.Trim, "ghi.Fever_treatment".ToLower.Trim

                    cmd.CommandText = "SELECT max([round])FROM [DSSHRS].ghi.allChild_health where  id='" & row("id").ToString & "' "
                    i = cmd.ExecuteScalar().ToString
                    If i Is Nothing Then
                        round = ""
                    Else
                        round = i
                    End If
                Case "SpecialStudies.bednet".ToLower.Trim, "SpecialStudies.bednet_individual_netUse".ToLower.Trim, "SpecialStudies.bednet_netinfo".ToLower.Trim
                    cmd.CommandText = "SELECT max([round])FROM [DSSHRS].[SpecialStudies].[allBednet] where  id='" & row("id").ToString & "' "
                    i = cmd.ExecuteScalar().ToString
                    If i Is Nothing Then
                        round = ""
                    Else
                        round = i
                    End If

                Case "specialStudies.HHD_Waterusage".ToLower.Trim, "specialStudies.FetchWater_Member".ToLower.Trim _
                         , "specialStudies.CleanWater_Methods".ToLower.Trim, "specialStudies.WaterAccess_Activities".ToLower.Trim
                    cmd.CommandText = "SELECT max([round])FROM [DSSHRS].[SpecialStudies].[allWaterAccess] where  wateraccessID='" & row("wateraccessID").ToString & "' "
                    i = cmd.ExecuteScalar().ToString
                    If i Is Nothing Then
                        round = ""
                    Else
                        round = i
                    End If
                Case "mhrs_sys.changes"

                    cmd.CommandText = "SELECT max([round_num]) as round FROM [DSSHRS].[DSS].[round]WHERE @visitdate BETWEEN [start_date] AND [end_date]"
                    cmd.Parameters.AddWithValue("@visitdate", row("visitdate"))
                    i = cmd.ExecuteScalar()
                    If i Is Nothing Then
                        round = ""
                    Else
                        round = i
                    End If
                Case "dss.pregnancy", "dss.residency", "dss.membership"
                    If IsDBNull(row("eobserveid")) Then
                        round = Right(row("sobserveid").ToString.Trim, 5)
                    Else
                        round = Right(row("eobserveid").ToString.Trim, 5)
                    End If

                Case "dss.indvstatus", "dss.marriage", "dss.indvstatus", "specialstudies.education"
                    round = Right(row("observeid").ToString.Trim, 5)
                Case "dss.individual", "dss.nationalid"
                    cmd.CommandText = "SELECT  min(right(rtrim([sobserveid]),5)) as round FROM [DSSHRS].[dbo].[vCombinedResidency] " _
                                        & "where [individid]=@individid group by [individid]"
                    cmd.Parameters.AddWithValue("@individid", row("individid").ToString)
                    'round = cmd.ExecuteScalar().ToString
                    'i = cmd.ExecuteScalar()
                    'If i Is Nothing Then
                    '    round = ""
                    'Else
                    '    round = i
                    'End If

                    If Not IsDBNull(row("observationid")) Then
                        round = Right(row("observationid").ToString.Trim, 5)
                    Else
                        round = cmd.ExecuteScalar().ToString
                    End If
                Case "specialstudies.circumcision".ToLower
                    If IsDBNull(row("observationid")) Then
                        cmd.CommandText = "SELECT  min(right(rtrim([sobserveid]),5)) as round FROM [DSSHRS].[dbo].[vCombinedResidency] " _
                                        & "where [individid]=@individid group by [individid]"
                        cmd.Parameters.AddWithValue("@individid", row("Individid").ToString)
                        round = cmd.ExecuteScalar().ToString
                        i = cmd.ExecuteScalar()
                        If i Is Nothing Then
                            round = ""
                        Else
                            round = i
                        End If
                    Else
                        round = Right(row("observationid").ToString.Trim, 5)
                    End If
                Case "DSS.CONSENTS"
                Case "DSS.EVENTS_EPISODES"
                Case "DSS.REGIONS"
                Case "DSS.RELATIONSHIP"
                Case "DSS.ROUND"
                Case "SpecialStudies.MobilePhoneUsage".ToLower
                    Try
                        cmd.CommandText = "SELECT DSSHRS.dbo.get_Round(Entry_date) from " & tableName & " where transit_id ='" & row("transit_id").ToString & "'"
                        i = cmd.ExecuteScalar().ToString
                        If i Is Nothing Then
                            round = ""
                        Else
                            round = i
                        End If
                    Catch ex As Exception

                    End Try

                Case "specialstudies.EVP_Anthropometric".ToLower, "specialstudies.EVP".ToLower, "specialstudies.EVPinterviewOutcome".ToLower


                    Try
                        round = row("round").ToString
                    Catch ex As Exception

                    End Try
                Case "Ms.washschool".ToLower, "MS.WASHContainers".ToLower, "MS.GIRLS_BEHAVIOUR".ToLower, "MS.WASHHOME".ToLower, "MS.NURSE".ToLower
                    Try
                        round = "W" 'row("round_num").ToString
                    Catch ex As Exception

                    End Try
                Case "Radio.prevention_practice".ToLower
                    round = "20123"


            End Select
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            round = ""
            ' 'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        If round.Trim <> "W" Then
            If round Is Nothing OrElse IsDBNull(round) OrElse Not (clsUserDefinedFunctions.isValidRound(round)) Then round = "Q"
            cmd.Connection.Close()
        End If
        Return round.ToString
    End Function
    'Public Function executeFunction(ByVal functionName As String, ByVal value As Object) As Object
    '    Dim cmd As New SqlCommand
    '    cmd.Connection = Me.clsGlobalVariable.HRS_Temp_DBCon
    '    cmd.CommandText = "select " & functionName & "('" & value.ToString & "')"
    '    If Not Me.clsGlobalVariable.HRS_Temp_DBCon.State = ConnectionState.Open Then Me.clsGlobalVariable.HRS_Temp_DBCon.Open()

    '    Dim defV As Object = cmd.ExecuteScalar()
    '    cmd.Connection.Close()
    '    Return defV

    'End Function
#End Region

#End Region

#Region "other validations"
    Public Function Individualtable(ByVal Record As DataRow, ByVal tablename As String) As Boolean
        If Me.hasSmallAgediffwithfather(Record, 13) Then

            Me.saveError(Record("transit_id").ToString.Trim, tablename, "The father is too young", "", Now(), "", "Q", "Q")
            Me.exec_nonqueryInTEMPDB("UPDATE [dss].[individual] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + Record("transit_id").ToString.Trim)
        End If
        If Me.hasSmallAgediffwithMother(Record, 13) Then
            Me.saveError(Record("transit_id").ToString.Trim, tablename, "The mother is too young", "", Now(), "", "Q", "Q")
            Me.exec_nonqueryInTEMPDB("UPDATE [dss].[individual] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + Record("transit_id").ToString.Trim)
        End If
    End Function
    Public Function pregoutcometable(ByVal Record As DataRow, ByVal tablename As String) As Boolean

    End Function
    Public Function hasSmallAgediffwithfather(ByVal indrec As DataRow, ByVal agediff As Integer) As Boolean
        Dim returnValue As Boolean = True
        If indrec("fatherid").ToString.Trim.Equals("UNK") Then
            returnValue = False
            GoTo ExitPoint
        End If
        Dim sql As String = "SELECT COUNT(*) FROM [DSSHRS].[dbo].[getIndividual_Fatheragediff2] (@minageDiffFilter,@newchilddob,@newfatherid) " _
                            & " where individid =@individid"

        Dim cmd As New SqlCommand
        cmd.Connection = clsGlobalVariable.HRS_Main_DBCon
        cmd.CommandText = sql
        cmd.Parameters.AddWithValue("@minageDiffFilter", agediff)
        cmd.Parameters.AddWithValue("@newchilddob", indrec("dob"))
        cmd.Parameters.AddWithValue("@newfatherid", indrec("fatherid"))
        cmd.Parameters.AddWithValue("@individid", indrec("individid"))
        Try
            clsGlobalVariable.open_HRS_Main_DBCon()
            Try
                If CInt(cmd.ExecuteScalar()) > 0 Then
                    returnValue = True
                Else
                    returnValue = False
                End If
            Catch ex As Exception
                returnValue = True
            End Try
        Catch ex As Exception
            returnValue = True
        End Try
        clsGlobalVariable.close_HRS_Main_DBCon()
ExitPoint:
        Return returnValue
    End Function


    Public Function getIndivdualsAge(ByVal _startDate As Date, ByVal _endDate As Date, ByVal _PID As String) As Double
        Dim returnValue As Double = 0
        Dim sql As String = "SELECT [TEMP_DSSHRS].[DSS].[GetAge] (@startdate,@enddate ,@individid)"

        Dim cmd As New SqlCommand
        cmd.Connection = clsGlobalVariable.HRS_Main_DBCon
        cmd.CommandText = sql
        cmd.Parameters.AddWithValue("@startdate", IIf(_startDate = "12:00:00 AM", DBNull.Value, _startDate))
        cmd.Parameters.AddWithValue("@enddate", _endDate)
        cmd.Parameters.AddWithValue("@individid", _PID)
        Try
            clsGlobalVariable.open_HRS_Main_DBCon()
            returnValue = cmd.ExecuteScalar()
        Catch ex As Exception
            returnValue = 0
        Finally
            clsGlobalVariable.close_HRS_Main_DBCon()
        End Try
        Return returnValue
    End Function


    Public Function hasSmallAgediffwithMother(ByVal indrec As DataRow, ByVal agediff As Integer) As Boolean
        Dim returnValue As Boolean = True
        If indrec("motherid").ToString.Trim.Equals("UNK") Then
            returnValue = False
            GoTo ExitPoint
        End If
        Dim sql As String = "SELECT COUNT(*) FROM [DSSHRS].[dbo].[getIndividual_motheragediff2] (@minageDiffFilter,@newchilddob,@newmotherid) " _
                            & " where individid =@individid"

        Dim cmd As New SqlCommand
        cmd.Connection = clsGlobalVariable.HRS_Main_DBCon
        cmd.CommandText = sql
        cmd.Parameters.AddWithValue("@minageDiffFilter", agediff)
        cmd.Parameters.AddWithValue("@newchilddob", indrec("dob"))
        cmd.Parameters.AddWithValue("@newmotherid", indrec("motherid"))
        cmd.Parameters.AddWithValue("@individid", indrec("individid"))
        Try
            clsGlobalVariable.open_HRS_Main_DBCon()
            Try
                If CInt(cmd.ExecuteScalar()) > 0 Then
                    returnValue = True
                Else
                    returnValue = False
                End If
            Catch ex As Exception
                returnValue = True
            End Try
        Catch ex As Exception
            returnValue = True
        End Try
        clsGlobalVariable.close_HRS_Main_DBCon()
ExitPoint:
        Return returnValue
    End Function
    'Public Function hasSmallAgediffwithMother(ByVal individid As String, ByVal agediff As Integer) As Boolean
    '    Dim returnValue As Boolean = True
    '    Dim sql As String = "SELECT COUNT(*) FROM [DSSHRS].[dbo].[getIndividual_motheragediff] (" + agediff.ToString + ") " _
    '                        & " where individid ='" + individid + "' "
    '    If Me.executeScalar_INMainDB(sql) > 0 Then
    '        returnValue = True
    '    Else
    '        returnValue = False
    '    End If
    '    Return returnValue
    'End Function
    Public Function individualtooYounfForMarriage(ByVal individid As String, ByVal agediff As Integer) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT COUNT(*) FROM [TEMP_DSSHRS].[dbo].[marriedpersonsages] ()  " _
                            & " where individid ='" + individid.Trim + "' and ( indi_visitationage<" + agediff.ToString + ") "
        If Me.executeScalar_INMainDB(sql) > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function
    Public Function spousetooYounfForMarriage(ByVal individid As String, ByVal agediff As Integer) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT COUNT(*) FROM [TEMP_DSSHRS].[dbo].[marriedpersonsages] ()  " _
                            & " where spouseid ='" + individid.Trim + "' and ( spouse_visitationage<" + agediff.ToString + ") "
        If Me.executeScalar_INMainDB(sql) > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function
    Public Function has_closePregnacy_Outcome(ByVal individid As String, ByVal outcomedate As Date) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT COUNT(*) FROM [DSSHRS].[DSS].[pregoutcome]  " _
                            & " where (date is not null) and individid ='" + individid.Trim + "' and (datediff(month,date,'" + outcomedate.Year.ToString + outcomedate.Month.ToString.PadLeft(2, "0") + outcomedate.Day.ToString.PadLeft(2, "0") + "')<9)"
        If Me.executeScalar_INMainDB(sql) > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function
    Public Function has_Completed_Visitation(ByVal locationid As String, ByVal round As String) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT COUNT(*) FROM [DSSHRS].[DSS].[visitation]  " _
                            & " where (outcome='Completed') and (locationid ='" + locationid.Trim + "') and (round='" + round.Trim + "')"
        If Me.executeScalar_INMainDB(sql) > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function
#End Region

End Class
