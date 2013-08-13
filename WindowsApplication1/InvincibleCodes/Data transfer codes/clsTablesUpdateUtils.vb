Imports System.Data
Imports System.Data.SqlClient
Public Class clsTablesUpdateUtils
    Public currentTransaction As SqlTransaction
    Private clsGlobalVariable As clsGlobalVariables = clsGlobalVariables.getObject
    Public objRef As clsformrefrences = clsformrefrences.getObject

#Region "Data access functions "
    Public Function updateRecord(ByVal tab As DataTable, Optional ByVal wherepart As String = "", Optional ByRef trans As SqlTransaction = Nothing) As Boolean
        '  Try

        Dim cmd As New SqlCommand
        cmd.Connection = clsGlobalVariable.HRS_Main_DBCon
        'set this command to be a member of a transaction
        If Not trans Is Nothing Then cmd.Transaction = trans

        clsGlobalVariable.open_HRS_Main_DBCon()  '= False Then Return False
        cmd.CommandText = generateUpdateSql(tab, wherepart)
        cmd.Parameters.Clear()
        For Each row As DataRow In tab.Rows
            cmd.Parameters.AddWithValue("@" & row("Column_name").ToString, row("value"))
        Next
        ' cmd.Prepare()
        If cmd.ExecuteNonQuery() > 0 Then

            Return True
        Else
            Return False
        End If
        'Catch ex As Exception
        '    MsgBox(ex.Message)
        '    Return False
        '    throw ex
        'End Try
    End Function
    Public Function submitNewRecord(ByVal tab As DataTable, Optional ByRef trans As SqlTransaction = Nothing) As Boolean
        ' Try
        Dim cmd As New SqlCommand
        cmd.Connection = clsGlobalVariable.HRS_Main_DBCon
        'set this command to be a member of a transaction
        If Not trans Is Nothing Then cmd.Transaction = trans

        clsGlobalVariable.open_HRS_Main_DBCon() '= False Then Return False
        cmd.CommandText = generateInsertSql(tab)
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
        'Catch ex As Exception
        '    MsgBox(ex.Message)
        '    Return False
        'End Try
    End Function
    Public Function generic_delete(ByVal sourceRow As DataRow, ByVal SourceTable As DataTable, _
                        ByVal fullTableName As String, _
                        Optional ByRef trans As SqlTransaction = Nothing) As Boolean
        Dim col_name As String = ""
        Dim record_Todelete As DataTable = Me.newRecordValuesTable(fullTableName) 'Me.newRecordValuesTable("[DSS].[individual]")
        For Each col As DataColumn In SourceTable.Columns
            col_name = col.ColumnName.Trim
            Me.addColValue(record_Todelete, col_name, sourceRow(col_name))
        Next
        If Me.DeleteRecord(record_Todelete, currentTransaction) Then
            Return True
        Else
            Return False
        End If
    End Function
    Friend Function DeleteSpecialStudRecord(ByVal transitid As Integer, ByVal tablename As String, Optional ByRef trans As SqlTransaction = Nothing) As Boolean
        ' Try
        Dim cmd As New SqlCommand
        cmd.Connection = clsGlobalVariable.HRS_Temp_DBCon
        'set this command to be a member of a transaction
        'If Not trans Is Nothing Then cmd.Transaction = trans

        clsGlobalVariable.open_HRS_Temp_DBCon() '= False Then Return False
        cmd.CommandText = "delete from " + tablename + " where transit_id=@transit_id"
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@transit_id", transitid)

        'cmd.Prepare()
        If cmd.ExecuteNonQuery() > 0 Then
            Return True
        Else
            Return False
        End If
        'Catch ex As Exception
        '    MsgBox(ex.Message)
        '    Return False
        'End Try
    End Function
    
    Private Function DeleteRecord(ByVal tab As DataTable, Optional ByRef trans As SqlTransaction = Nothing) As Boolean
        ' Try
        Dim cmd As New SqlCommand
        cmd.Connection = clsGlobalVariable.HRS_Temp_DBCon
        'set this command to be a member of a transaction
        'If Not trans Is Nothing Then cmd.Transaction = trans

        clsGlobalVariable.open_HRS_Temp_DBCon() '= False Then Return False
        cmd.CommandText = Me.generateDeleteSql(tab)
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
        'Catch ex As Exception
        '    MsgBox(ex.Message)
        '    Return False
        'End Try
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
            sql = "UPDATE " + tab.TableName + "  " + colsection + "  " + wherePart.Trim
        Else
            sql = ""
        End If
        Return sql
    End Function
    Public Function generateDeleteSql(ByVal tab As DataTable) As String
        Dim sql As String = ""
        Dim wherePart As String = ""
        Dim rowCount As Integer = tab.Rows.Count
        If rowCount > 0 Then
            wherePart = " where " + tab.Rows(0).Item("Column_name").ToString.Trim + " =@" + tab.Rows(0).Item("Column_name").ToString.Trim
            If rowCount > 1 Then
                For i As Integer = 1 To rowCount - 1
                    wherePart = wherePart + " AND " + tab.Rows(i).Item("Column_name").ToString.Trim + " =@" + tab.Rows(i).Item("Column_name").ToString.Trim
                Next
            End If
            sql = "delete from  " + tab.TableName + "  " + wherePart.Trim
        Else
            sql = ""
        End If
        Return sql
    End Function
    ''' <summary>
    ''' creates and returns a new explicit transaction and sets the currentTransaction varriable to be the created transaction
    ''' </summary>
    ''' <returns>the transaction obj created</returns>
    ''' <remarks></remarks>
    Public Function getTransaction() As SqlTransaction
        clsGlobalVariable.open_HRS_Main_DBCon()
        Dim trans As SqlTransaction = clsGlobalVariable.HRS_Main_DBCon.BeginTransaction(IsolationLevel.ReadCommitted)
        currentTransaction = trans
        Return trans
    End Function
    ''' <summary>
    ''' ends a transaction by either commiting it to database or rolling back
    ''' </summary>
    ''' <param name="commit">true to signify commit: false to signify rollback</param>
    ''' <param name="trans">the transaction to be effected/rolled back</param>
    ''' <returns>true if the query excecuted successfully, false otherwise</returns>
    ''' <remarks></remarks>

    Public Function commitTransaction(ByVal commit As Boolean, ByVal trans As SqlTransaction) As Boolean

        clsGlobalVariable.open_HRS_Main_DBCon()
        If commit Then
            trans.Commit()
            Return True
        Else
            trans.Rollback()
            Return False
        End If


    End Function
    Public Function updateDownloadItems(ByVal tableName As String, ByVal recordID As String, _
                                        ByVal action As String, Optional ByRef trans As SqlTransaction = Nothing) As Boolean

        Dim sql As String = "INSERT INTO[MHRS_SYS.DownloadItems]([RecordID],[TableName],[Action])" _
                            & " VALUES (?,?,?)"
        Dim cmd As New SqlCommand()
        cmd.CommandText = sql
        cmd.Connection = clsGlobalVariable.HRS_Main_DBCon
        'set this command to be a member of a transaction
        If Not trans Is Nothing Then cmd.Transaction = trans
        clsGlobalVariable.open_HRS_Main_DBCon()
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@[RecordID]", recordID)
        cmd.Parameters.AddWithValue("@[TableName]", tableName)
        cmd.Parameters.AddWithValue("@[Action]", action)

        'cmd.Prepare()
        If cmd.ExecuteNonQuery() > 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function updateIndividualChanges(ByVal tableName As String, ByVal individid As String, _
                                            ByVal colname As String, ByVal oldValue As String, ByVal NewValue As String, _
                                            Optional ByRef trans As SqlTransaction = Nothing) As Boolean

        Dim sql As String = "INSERT INTO[MHRS_SYS.Changes]([recordid],[tablename],[colname],[oldValue],[NewValue])" _
                            & " VALUES (?,?,?,?,?)"
        Dim cmd As New SqlCommand()
        cmd.CommandText = sql
        cmd.Connection = clsGlobalVariable.HRS_Main_DBCon
        'set this command to be a member of a transaction
        If Not trans Is Nothing Then cmd.Transaction = trans
        clsGlobalVariable.open_HRS_Main_DBCon()
        cmd.Parameters.Clear()
        cmd.Parameters.AddWithValue("@[recordid]", individid)
        cmd.Parameters.AddWithValue("@[tablename]", tableName)
        cmd.Parameters.AddWithValue("@[colname]", colname)
        cmd.Parameters.AddWithValue("@[oldValue]", oldValue)
        cmd.Parameters.AddWithValue("@[NewValue]", NewValue)


        ' cmd.Prepare()
        If cmd.ExecuteNonQuery() > 0 Then
            Return True
        Else
            Return False
        End If

    End Function
#End Region
#Region "HRS Static objects"
    Public Function newLocation( ByVal sourceRow As DataRow) As Boolean
        Dim newRecord As DataTable = Me.newRecordValuesTable("[DSS].[location]")
        Me.addColValue(newRecord, "locationid", sourceRow("locationid"))
        Me.addColValue(newRecord, "entry_date", sourceRow("entry_date"))
        Me.addColValue(newRecord, "compoundID", sourceRow("compoundID"))
        Me.addColValue(newRecord, "NoVisited", sourceRow("Novisited"))
        Me.addColValue(newRecord, "Revisit", sourceRow("Revisit"))
        Me.addColValue(newRecord, "fieldworker", sourceRow("fieldworker"))
        Me.addColValue(newRecord, "rec_status", "V")
        If Me.submitNewRecord(newRecord) Then

            Return True
        Else
            Return False
        End If

    End Function
    Public Function updateLocationVisitation(ByVal locationid As String, _
                                             ByVal revisit As String, _
                                             ByVal Novisited As Integer) As Boolean
        Dim wherePart As String = " where  (locationid='" + locationid + "')"

        Dim updateRecord As DataTable = Me.newRecordValuesTable("[DSS].[location]")
        Me.addColValue(updateRecord, "[Revisit]", revisit)
        Me.addColValue(updateRecord, "[Novisited]", Novisited)
        Return Me.updateRecord(updateRecord, wherePart)

    End Function
    Public Function addSocialGroup(ByVal sourceRow As DataRow) As Boolean
        Dim newRecord As DataTable = Me.newRecordValuesTable("[DSS].[socialgroup]")
        Me.addColValue(newRecord, "socialgpid", sourceRow("socialgpid"))
        Me.addColValue(newRecord, "name", sourceRow("name"))
        Me.addColValue(newRecord, "type", sourceRow("Type"))
        Me.addColValue(newRecord, "Entry_date", sourceRow("entry_date"))
        Me.addColValue(newRecord, "location", sourceRow("Location"))
        Me.addColValue(newRecord, "fieldworker", sourceRow("fieldworker"))
        Me.addColValue(newRecord, "rec_status", "V")
        If Me.submitNewRecord(newRecord) Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function addSocialGroupAdmin(ByVal sourceRow As DataRow) As Boolean
        Dim newRecord As DataTable = Me.newRecordValuesTable("[DSS].[SocialGroupadmin]")
        Me.addColValue(newRecord, "socialgpid", sourceRow("socialgpid"))
        Me.addColValue(newRecord, "round", sourceRow("round"))
        Me.addColValue(newRecord, "adminid", sourceRow("adminid"))
        Me.addColValue(newRecord, "fieldworker", sourceRow("fieldworker"))
        Me.addColValue(newRecord, "visitdate", sourceRow("visitdate"))
        Me.addColValue(newRecord, "rec_status", "V")
        Return Me.submitNewRecord(newRecord)
    End Function
    Public Function newIndividual(ByVal sourceRow As DataRow) As Boolean
        Dim newRecord As DataTable = Me.newRecordValuesTable("[DSS].[individual]")
        Me.addColValue(newRecord, "individid", sourceRow("individid"))
        Me.addColValue(newRecord, "fname", sourceRow("fname"))
        Me.addColValue(newRecord, "jname", sourceRow("jname"))
        Me.addColValue(newRecord, "lname", sourceRow("lname"))
        Me.addColValue(newRecord, "famcla", sourceRow("famcla"))
        Me.addColValue(newRecord, "akaname", sourceRow("akaname"))
        Me.addColValue(newRecord, "gender", sourceRow("gender"))
        Me.addColValue(newRecord, "dob", sourceRow("dob"))
        Me.addColValue(newRecord, "arrdate", sourceRow("arrdate"))
        Me.addColValue(newRecord, "motherid", sourceRow("motherid"))
        Me.addColValue(newRecord, "fatherid", sourceRow("fatherid"))
        Me.addColValue(newRecord, "mfname", sourceRow("mfname"))
        Me.addColValue(newRecord, "mjname", sourceRow("mjname"))
        Me.addColValue(newRecord, "mlname", sourceRow("mlname"))
        Me.addColValue(newRecord, "ffname", sourceRow("ffname"))
        Me.addColValue(newRecord, "fjname", sourceRow("fjname"))
        Me.addColValue(newRecord, "flname", sourceRow("flname"))
        Me.addColValue(newRecord, "ethnic", sourceRow("ethnic"))
        Me.addColValue(newRecord, "rec_status", "V")
        If Me.submitNewRecord(newRecord, currentTransaction) Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function deleteIndividual(ByVal sourceRow As DataRow) As Boolean
        Dim record_Todelete As DataTable = Me.newRecordValuesTable("[DSS].[individual]")
        Me.addColValue(record_Todelete, "individid", sourceRow("individid"))
        Me.addColValue(record_Todelete, "fname", sourceRow("fname"))
        Me.addColValue(record_Todelete, "jname", sourceRow("jname"))
        Me.addColValue(record_Todelete, "lname", sourceRow("lname"))
        Me.addColValue(record_Todelete, "famcla", sourceRow("famcla"))
        Me.addColValue(record_Todelete, "akaname", sourceRow("akaname"))
        Me.addColValue(record_Todelete, "gender", sourceRow("gender"))
        Me.addColValue(record_Todelete, "dob", sourceRow("dob"))
        Me.addColValue(record_Todelete, "arrdate", sourceRow("arrdate"))
        Me.addColValue(record_Todelete, "motherid", sourceRow("motherid"))
        Me.addColValue(record_Todelete, "fatherid", sourceRow("fatherid"))
        Me.addColValue(record_Todelete, "mfname", sourceRow("mfname"))
        Me.addColValue(record_Todelete, "mjname", sourceRow("mjname"))
        Me.addColValue(record_Todelete, "mlname", sourceRow("mlname"))
        Me.addColValue(record_Todelete, "ffname", sourceRow("ffname"))
        Me.addColValue(record_Todelete, "fjname", sourceRow("fjname"))
        Me.addColValue(record_Todelete, "flname", sourceRow("flname"))
        Me.addColValue(record_Todelete, "ethnic", sourceRow("ethnic"))
        If Me.DeleteRecord(record_Todelete, currentTransaction) Then
            Return True
        Else
            Return False
        End If

    End Function
    Public Function newCompound(ByVal sourceRow As DataRow) As Boolean
        Dim newRecord As DataTable = Me.newRecordValuesTable("[DSS].[compounds]")
        Me.addColValue(newRecord, "compoundid", sourceRow("compoundid"))
        Me.addColValue(newRecord, "villcode", sourceRow("villcode"))
        Me.addColValue(newRecord, "ccompdesc", sourceRow("ccompdesc"))
        Me.addColValue(newRecord, "headisResident", sourceRow("headisResident"))
        Me.addColValue(newRecord, "chheadid", sourceRow("chheadid"))
        Me.addColValue(newRecord, "cfname", sourceRow("cfname"))
        Me.addColValue(newRecord, "cjname", sourceRow("cjname"))
        Me.addColValue(newRecord, "clname", sourceRow("clname"))
        Me.addColValue(newRecord, "cfcname", sourceRow("cfcname"))
        Me.addColValue(newRecord, "entry_date", sourceRow("entry_date"))
        Me.addColValue(newRecord, "longitude", sourceRow("longitude"))
        Me.addColValue(newRecord, "Latitude", sourceRow("Latitude"))
        Me.addColValue(newRecord, "Altitude", sourceRow("Altitude"))
        Me.addColValue(newRecord, "VisitRank", sourceRow("VisitRank"))
        Me.addColValue(newRecord, "visited", sourceRow("visited"))
        Me.addColValue(newRecord, "fieldworker", sourceRow("fieldworker"))
        Me.addColValue(newRecord, "rec_status", "V")

        If Me.submitNewRecord(newRecord, currentTransaction) Then
            Return True
        Else
            Return False
        End If

    End Function
    Public Function updateIndividual( _
                            ByVal individid As String, _
                            ByVal column_Name As String, _
                            ByVal newValue As String _
                           ) As Boolean

        Dim wherePart As String = " where  (individid='" + individid + "')"

        Dim updateRecord As DataTable = Me.newRecordValuesTable("[DSS].[individual]")
        Me.addColValue(updateRecord, column_Name, newValue)
        Try
            If Me.updateRecord(updateRecord, wherePart) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
       
    End Function
    Public Function updateParentalSurvival( _
                           ByVal psid As String, _
                           ByVal column_Name As String, _
                           ByVal newValue As String _
                          ) As Boolean

        Dim wherePart As String = " where  ([psid]='" + psid + "')"

        Dim updateRecord As DataTable = Me.newRecordValuesTable("[SpecialStudies].[parentsurv]")
        Me.addColValue(updateRecord, column_Name, newValue)
        Try
            If Me.updateRecord(updateRecord, wherePart) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try

    End Function
    Public Function updateResidency( _
                            ByVal residencyid As String, _
                            ByVal column_Name As String, _
                            ByVal newValue As String _
                           ) As Boolean

        Dim wherePart As String = " where  (residencyid='" + residencyid + "')"

        Dim updateRecord As DataTable = Me.newRecordValuesTable("[DSS].[residency]")
        Me.addColValue(updateRecord, column_Name, newValue)
        'Try
        If Me.updateRecord(updateRecord, wherePart) Then
            Return True
        Else
            Return False
        End If
        'Catch ex As Exception
        'Return False
        'End Try
    End Function
    Public Function updateSocialGroup( _
                            ByVal socialgpid As String, _
                            ByVal location As String _
                           ) As Boolean

        Dim wherePart As String = " where  (socialgpid='" + socialgpid + "')"

        Dim updateRecord As DataTable = Me.newRecordValuesTable("[DSS].[socialgroup]")
        Me.addColValue(updateRecord, "location", location)
        If Me.updateRecord(updateRecord, wherePart) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function updateColumn( _
                            ByVal keyColumnValue As String, _
                             ByVal keyColumnName As String, _
                            ByVal table_Name As String, _
                            ByVal column_Name As String, _
                            ByVal newValue As String) As Boolean

        Dim wherePart As String = " where  (" & keyColumnName & "='" + keyColumnValue + "')"

        Dim updateRecord As DataTable = Me.newRecordValuesTable(table_Name)
        Me.addColValue(updateRecord, column_Name, newValue)
        Return Me.updateRecord(updateRecord, wherePart)
    End Function
    Public Function IndividividualExists(ByVal individid As String) As Boolean
        Dim sql As String = "SELECT count(*) FROM [DSS].[individual] where (individid='" + individid + "')"
        Dim cmd As New SqlCommand()
        cmd.Connection = clsGlobalVariable.HRS_Main_DBCon
        clsGlobalVariable.open_HRS_Main_DBCon()

        cmd.CommandText = sql
        If cmd.ExecuteScalar > 0 Then
            Return True
        Else
            Return False
        End If

    End Function
    Public Function addCompAdmin(ByVal sourceRow As DataRow) As Boolean
        Dim newRecord As DataTable = Me.newRecordValuesTable("[DSS].[compadmin]")
        Me.addColValue(newRecord, "coadid", sourceRow("coadid"))
        Me.addColValue(newRecord, "compoundid", sourceRow("compoundid"))
        Me.addColValue(newRecord, "round", sourceRow("round"))
        Me.addColValue(newRecord, "cadminid", sourceRow("cadminid"))
        Me.addColValue(newRecord, "fieldworker", sourceRow("fieldworker"))
        Me.addColValue(newRecord, "visitdate", sourceRow("visitdate"))
        Me.addColValue(newRecord, "rec_status", "V")
        If Me.submitNewRecord(newRecord) Then

            Return True
        Else
            Return False
        End If
    End Function


#End Region
#Region "Residency episode"
    Public Function getLatest_episodeRecord(ByVal episodename As String, ByVal episodeid As String, ByVal idvalue As String) As Data.DataRow
        Dim residency As DataRow = Nothing
        Dim sql As String = "SELECT * FROM " + episodename + " where (" + episodeid + "='" + idvalue + "')"
        Try
            Dim cmd As New SqlCommand()
            Dim table As New DataTable
            cmd.Connection = clsGlobalVariable.HRS_Main_DBCon
            If clsGlobalVariable.open_HRS_Main_DBCon = False Then residency = Nothing
            cmd.CommandText = sql
            Dim readDB As SqlDataReader = cmd.ExecuteReader(CommandBehavior.Default)
            table.Load(readDB)
            If table.Rows.Count > 0 Then
                residency = table.Rows(0)
            Else
                residency = Nothing
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            MsgBox(ex.Message)
            residency = Nothing
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return residency
    End Function


    Public Function startResidency(ByVal sourceRow As DataRow) As Boolean
        If Me.isOpenresidency(getLatestResidency(sourceRow("individid").ToString)) Then
            'MsgBox("The individual has an open residency episode")
            Return False
        Else
            Dim newRecord As DataTable = Me.newRecordValuesTable("[DSS].[residency]")
            Me.addColValue(newRecord, "ResidencyID", sourceRow("ResidencyID"))
            Me.addColValue(newRecord, "individid ", sourceRow("individid"))
            Me.addColValue(newRecord, "locationid", sourceRow("locationid"))
            Me.addColValue(newRecord, "seventtype ", sourceRow("seventtype"))
            Me.addColValue(newRecord, "sdate ", sourceRow("sdate"))
            Me.addColValue(newRecord, "sobserveid ", sourceRow("sobserveid"))
            Me.addColValue(newRecord, "sfieldworker ", sourceRow("sfieldworker"))
            Me.addColValue(newRecord, "rec_status", "V")
            Me.addColValue(newRecord, "StartEntryDate ", sourceRow("StartEntryDate"))
            If Me.submitNewRecord(newRecord, currentTransaction) Then
                Return True
            Else
                Return False
            End If
        End If
    End Function
    Public Function endResidency(ByVal sourceRow As DataRow) As Boolean
        Dim ResidencyRecord As DataRow = Me.getLatest_episodeRecord("[DSS].[residency]", "ResidencyID", sourceRow("ResidencyID").ToString) 'getLatestResidency(individid)
        If Me.isOpenresidency(ResidencyRecord) Then
            Dim updateRecord As DataTable = Me.newRecordValuesTable("[DSS].[residency]")
            Me.addColValue(updateRecord, "eeventtype ", sourceRow("eeventtype"))
            Me.addColValue(updateRecord, "edate ", sourceRow("edate"))
            Me.addColValue(updateRecord, "eobserveid ", sourceRow("eobserveid"))
            Me.addColValue(updateRecord, "efieldworker ", sourceRow("efieldworker"))
            Me.addColValue(updateRecord, "EndEntryDate ", sourceRow("EndEntryDate"))
            If Me.updateRecord(updateRecord, " where ResidencyID='" & ResidencyRecord("ResidencyID").ToString & "'", currentTransaction) Then
                Return True
            Else
                Return False
            End If
        Else
            'MsgBox("The individual has no open episode")
            Return False
        End If
    End Function
    Public Function isOpenresidency(ByVal LatestResidency As DataRow) As Boolean
        If LatestResidency Is Nothing Then
            Return False
        Else

            If ( _
             IsDBNull(LatestResidency("eeventtype")) _
            Or IsDBNull(LatestResidency("eobserveid")) _
            ) _
            Or ((LatestResidency("eeventtype").ToString.Trim = "") _
            Or (LatestResidency("eobserveid").ToString.Trim = "")) _
            Then
                Return True
            Else
                Return False
            End If
        End If
    End Function

    Public Function getLatestResidency(ByVal individid As String) As Data.DataRow
        Dim residency As DataRow = Nothing
        Dim sql As String = "SELECT * FROM [DSS].[residency]where (individid='" + individid + "')and " _
                & "(sdate in (select max(sdate)  FROM [DSS].[residency]where individid='" + individid + "'))"
        Try
            Dim cmd As New SqlCommand()
            Dim table As New DataTable
            cmd.Connection = clsGlobalVariable.HRS_Main_DBCon
            If clsGlobalVariable.open_HRS_Main_DBCon = False Then residency = Nothing
            cmd.CommandText = sql
            Dim readDB As SqlDataReader = cmd.ExecuteReader(CommandBehavior.Default)
            table.Load(readDB)
            If table.Rows.Count > 0 Then
                residency = table.Rows(0)
            Else
                residency = Nothing
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            MsgBox(ex.Message)
            residency = Nothing
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return residency
    End Function

    Public Function getLatestLocationResidency(ByVal individid As String, ByVal locationid As String) As Data.DataRow
        Dim residency As DataRow = Nothing
        Dim sql As String = "SELECT * FROM [DSS.residency]where (individid='" & individid & "')and " _
                & "(sdate in (select max(sdate) FROM [DSS].[residency]where individid='" & individid _
                & "' AND locationid='" & locationid & "'))"
        Try
            Dim cmd As New SqlCommand()
            Dim table As New DataTable
            cmd.Connection = clsGlobalVariable.HRS_Main_DBCon
            If clsGlobalVariable.open_HRS_Main_DBCon = False Then residency = Nothing
            cmd.CommandText = sql
            Dim readDB As SqlDataReader = cmd.ExecuteReader(CommandBehavior.Default)
            table.Load(readDB)
            If table.Rows.Count > 0 Then
                residency = table.Rows(0)
            Else
                residency = Nothing
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            MsgBox(ex.Message)
            residency = Nothing
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return residency
    End Function

#End Region
#Region "Membership episode"
    Public Function startMembership(ByVal sourceRow As DataRow) As Boolean
        Dim newRecord As DataTable = Me.newRecordValuesTable("[DSS].[membership]")
        Me.addColValue(newRecord, "MembershipID", sourceRow("MembershipID"))
        Me.addColValue(newRecord, "individid", sourceRow("individid"))
        Me.addColValue(newRecord, "socialgpid", sourceRow("socialgpid"))
        Me.addColValue(newRecord, "sobserveid", sourceRow("sobserveid"))
        Me.addColValue(newRecord, "sdate", sourceRow("sdate"))
        Me.addColValue(newRecord, "seventtype ", sourceRow("seventtype"))
        Me.addColValue(newRecord, "sfieldworker ", sourceRow("sfieldworker"))
        Me.addColValue(newRecord, "rec_status", "V")
        Me.addColValue(newRecord, "StartEntryDate ", sourceRow("StartEntryDate"))
        If Me.submitNewRecord(newRecord, currentTransaction) Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function endMembership(ByVal sourceRow As DataRow) As Boolean
        Dim Membership As DataRow = Me.getLatest_episodeRecord("[DSS].[membership]", "memberShipID", sourceRow("memberShipID").ToString)
        If Me.isOpenMembership(Membership) Then
            Dim wherePart As String = " where (memberShipID='" + sourceRow("memberShipID").ToString + "') AND (individid='" + sourceRow("individid").ToString + "') "
            Dim updateRecord As DataTable = Me.newRecordValuesTable("[DSS].[membership]")
            Me.addColValue(updateRecord, "eobserveid", sourceRow("eobserveid"))
            Me.addColValue(updateRecord, "edate", sourceRow("edate"))
            Me.addColValue(updateRecord, "eeventtype", sourceRow("eeventtype"))
            Me.addColValue(updateRecord, "efieldworker", sourceRow("efieldworker"))
            Me.addColValue(updateRecord, "EndEntryDate ", sourceRow("EndEntryDate"))
            If Me.updateRecord(updateRecord, wherePart, currentTransaction) Then
                Return True
            Else
                Return False
            End If
        End If

    End Function
    Public Function isOpenMembership(ByVal MembershipRecord As DataRow) As Boolean
        If MembershipRecord Is Nothing Then
            Return False
        Else
            If ( _
             IsDBNull(MembershipRecord("eeventtype")) _
            Or IsDBNull(MembershipRecord("eobserveid")) _
            ) _
            Or ((MembershipRecord("eeventtype").ToString.Trim = "") _
            Or (MembershipRecord("eobserveid").ToString.Trim = "")) _
            Then
                Return True
            Else
                Return False
            End If
        End If
    End Function
    Public Function hasOpenMembership(ByVal individid As String) As Boolean
        Try
            Dim sql As String = "SELECT count(*) FROM [DSS].[membership] where ((len(ltrim(rtrim(eeventtype)))<1) or (eeventtype IS NULL))" _
             & "AND (individid='" + individid + "')"
            Dim cmd As New SqlCommand()
            cmd.Connection = clsGlobalVariable.HRS_Main_DBCon
            clsGlobalVariable.open_HRS_Main_DBCon()
            cmd.CommandText = sql
            If cmd.ExecuteScalar > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            MsgBox(ex.Message)
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
            Return False
        End Try
    End Function
    Public Function getOpenMemberships(ByVal individid As String) As Data.DataTable
        Dim Memberships As New DataTable
        Try
            Dim sql As String = "SELECT * FROM [DSS].[membership] where ((len(ltrim(rtrim(eeventtype)))<1) or (eeventtype IS NULL)) " _
            & "AND (individid='" + individid + "')"
            Dim cmd As New SqlCommand()
            cmd.Connection = clsGlobalVariable.HRS_Main_DBCon
            'If connectToConRef() = False Then Memberships = Nothing
            clsGlobalVariable.open_HRS_Main_DBCon()
            cmd.CommandText = sql
            Dim readDB As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            Memberships.Load(readDB)
            If Memberships.Rows.Count < 1 Then
                Memberships = Nothing
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            MsgBox(ex.Message)
            Memberships = Nothing
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return Memberships
    End Function

#End Region
#Region "Relationship episode"
    '    Public Function startRelationship(ByVal RelationshipID As Guid, _
    '                                      ByVal individid As String, _
    '                                      ByVal individid2 As String, _
    '                                      ByVal type As String, _
    '                                      ByVal sobserveid As String, _
    '                                      ByVal startEvent As String, _
    '                                      ByVal startdate As Date) As Boolean
    '        Dim newRecord As DataTable = Me.newRecordValuesTable("[DSS].[relationship]")
    '        Me.addColValue(newRecord, "RelationshipID", RelationshipID)
    '        Me.addColValue(newRecord, "individid", individid)
    '        Me.addColValue(newRecord, "individid2", individid2)
    '        Me.addColValue(newRecord, "type", type)
    '        Me.addColValue(newRecord, "sdate", startdate)
    '        Me.addColValue(newRecord, "seventtype", startEvent)
    '        Me.addColValue(newRecord, "sobserveid", sobserveid)
    '        If Me.submitNewRecord(newRecord) Then
    '            Return True 'updateDownloadItems("[DSS].[relationship]", RelationshipID.ToString, "INSERT")
    '        Else
    '            Return False
    '        End If

    '    End Function
    '    Public Function endRelationship(ByVal RelationshipID As String, _
    '                                      ByVal individid As String, _
    '                                      ByVal individid2 As String, _
    '                                      ByVal endobserveid As String, _
    '                                      ByVal endEvent As String, _
    '                                      ByVal enddate As Date) As Boolean
    '        Dim RelationshipRecord As DataRow = Me.getLatest_episodeRecord("[DSS].[relationship]", "RelationshipID", RelationshipID) 'getLatestResidency(individid)
    '        If Me.isOpenRelationship(RelationshipRecord) Then
    '            Dim wherePart As String = " where (RelationshipID='" + RelationshipID + "')  "
    '            Dim updateRecord As DataTable = Me.newRecordValuesTable("[DSS].[relationship]")
    '            Me.addColValue(updateRecord, "edate", enddate)
    '            Me.addColValue(updateRecord, "eeventtype", endEvent)
    '            Me.addColValue(updateRecord, "eobserveid", endobserveid)
    '            If Me.updateRecord(updateRecord, wherePart, currentTransaction) Then
    '                Return True 'updateDownloadItems("[DSS].[relationship]", RelationshipID.ToString, "UPDATE", currentTransaction)
    '            Else
    '                Return False
    '            End If
    '        End If
    '    End Function
    '    Public Function isOpenRelationship(ByVal RelationshipRecord As DataRow) As Boolean
    '        If RelationshipRecord Is Nothing Then
    '            Return False
    '        Else
    '            If ( _
    '             IsDBNull(RelationshipRecord("eeventtype")) _
    '            Or IsDBNull(RelationshipRecord("eobserveid")) _
    '            ) _
    '            Or ((RelationshipRecord("eeventtype").ToString.Trim = "") _
    '            Or (RelationshipRecord("eobserveid").ToString.Trim = "")) _
    '            Then
    '                Return True
    '            Else
    '                Return False
    '            End If
    '        End If
    '    End Function
    '    Public Function hasOpenRelationships(ByVal individid As String) As Boolean
    '        If Me.getOpenMemberships(individid) Is Nothing Then
    '            Return False
    '        Else
    '            Return True
    '        End If
    '    End Function
    '    Public Function getOpenRelationships(ByVal individid As String) As Data.DataTable
    '        Dim Relationships As New DataTable
    '        Try
    '            Dim sql As String = "SELECT * FROM [DSS].[relationship] where ((len(ltrim(rtrim(eeventtype)))<1) or (eeventtype IS NULL)) " _
    '            & "AND (individid='" + individid + "')"
    '            Dim cmd As New SqlCommand()
    '            cmd.Connection = clsGlobalVariable.HRS_Main_DBCon
    '            'If connectToConRef() = False Then Memberships = Nothing
    '            clsGlobalVariable.open_HRS_Main_DBCon()
    '            cmd.CommandText = sql
    '            Dim readDB As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
    '            Relationships.Load(readDB)
    '            If Relationships.Rows.Count < 1 Then
    '                Relationships = Nothing
    '            End If
    '        Catch ex As Exception
    '            MsgBox(ex.Message)

    '            Relationships = Nothing
    '        End Try
    '        Return Relationships
    '    End Function
#End Region
#Region "Pregnancy episode"
    Public Function startPregnancy(ByVal sourceRow As DataRow) As Boolean
        Dim newRecord As DataTable = Me.newRecordValuesTable("[DSS].[pregnancy]")
        Me.addColValue(newRecord, "PregnancyID", sourceRow("PregnancyID"))
        Me.addColValue(newRecord, "individid", sourceRow("individid"))
        Me.addColValue(newRecord, "locationid", sourceRow("locationid"))
        Me.addColValue(newRecord, "sobserveid", sourceRow("sobserveid"))
        Me.addColValue(newRecord, "sdate ", sourceRow("sdate"))
        Me.addColValue(newRecord, "seventype", sourceRow("seventype"))
        Me.addColValue(newRecord, "sfieldworker ", sourceRow("sfieldworker"))
        Me.addColValue(newRecord, "rec_status", "V")
        Me.addColValue(newRecord, "edod ", sourceRow("edod"))
        Me.addColValue(newRecord, "everpreg", sourceRow("everpreg"))
        Me.addColValue(newRecord, "StartEntryDate ", sourceRow("StartEntryDate"))
        If Me.submitNewRecord(newRecord, currentTransaction) Then
            Return True 'updateDownloadItems("[DSS].[pregnancy]", PregnancyID.ToString, "INSERT", currentTransaction)
        Else
            Return False
        End If
    End Function
    Public Function endPregnancy(ByVal sourceRow As DataRow) As Boolean
        Dim Pregnancy As DataRow = Me.getLatest_episodeRecord("[DSS].[pregnancy]", "pregnancyID", sourceRow("PregnancyID").ToString)
        If Me.isOpenPregnancy(Pregnancy) Then
            Dim wherePart As String = " where  (individid='" + sourceRow("individid").ToString + "')  AND " _
                                            & "(PregnancyID='" + sourceRow("PregnancyID").ToString + "')"
            Dim updateRecord As DataTable = Me.newRecordValuesTable("[DSS].[pregnancy]")
            Me.addColValue(updateRecord, "eobserveid", sourceRow("eobserveid"))
            Me.addColValue(updateRecord, "edate", sourceRow("edate"))
            Me.addColValue(updateRecord, "eeventtype", sourceRow("eeventtype"))
            Me.addColValue(updateRecord, "efieldworker", sourceRow("efieldworker"))
            Me.addColValue(updateRecord, "EndEntryDate ", sourceRow("EndEntryDate"))
            If Me.updateRecord(updateRecord, wherePart, currentTransaction) Then
                Return True
            Else
                Return False
            End If
        End If
    End Function
    Public Function isOpenPregnancy(ByVal PregnancyRecord As DataRow) As Boolean
        If PregnancyRecord Is Nothing Then
            Return False
        Else
            If ( _
             IsDBNull(PregnancyRecord("eeventtype")) _
            Or IsDBNull(PregnancyRecord("eobserveid")) _
            ) _
            Or ((PregnancyRecord("eeventtype").ToString.Trim = "") _
            Or (PregnancyRecord("eobserveid").ToString.Trim = "")) _
            Then
                Return True
            Else
                Return False
            End If
        End If
    End Function
    Public Function hasOpenPregnancy(ByVal individid As String) As Boolean
        Try
            Dim sql As String = "SELECT count(*) FROM [DSS].[pregnancy] where ((len(ltrim(rtrim(eeventtype)))<1) or (eeventtype IS NULL))" _
             & "AND (individid='" + individid + "')"
            Dim cmd As New SqlCommand()
            cmd.Connection = clsGlobalVariable.HRS_Main_DBCon
            clsGlobalVariable.open_HRS_Main_DBCon()
            cmd.CommandText = sql
            If cmd.ExecuteScalar > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            MsgBox(ex.Message)
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
            Return False
        End Try
    End Function
    Public Function getOpenPregnancy(ByVal individid As String) As Data.DataTable
        Dim openPregnacies As New DataTable
        Try
            Dim sql As String = "SELECT * FROM [DSS].[pregnancy] where ((len(ltrim(rtrim(eeventtype)))<1) or (eeventtype IS NULL)) " _
            & "AND (individid='" + individid + "')"
            Dim cmd As New SqlCommand()
            cmd.Connection = clsGlobalVariable.HRS_Main_DBCon
            'If connectToConRef() = False Then Memberships = Nothing
            clsGlobalVariable.open_HRS_Main_DBCon()
            cmd.CommandText = sql
            Dim readDB As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            openPregnacies.Load(readDB)
            If openPregnacies.Rows.Count < 1 Then
                openPregnacies = Nothing
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            MsgBox(ex.Message)

            openPregnacies = Nothing
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return openPregnacies
    End Function

    Public Function recordIndivStatus(ByVal sourceRow As DataRow) As Boolean
        Dim newRecord As DataTable = Me.newRecordValuesTable("[DSS].[indvstatus]")
        Me.addColValue(newRecord, "eventID", sourceRow("eventID"))
        Me.addColValue(newRecord, "individid", sourceRow("individid"))
        Me.addColValue(newRecord, "observeid", sourceRow("observeid"))
        Me.addColValue(newRecord, "date", sourceRow("date"))
        Me.addColValue(newRecord, "type", sourceRow("type"))
        Me.addColValue(newRecord, "everpreg", sourceRow("everpreg"))
        Me.addColValue(newRecord, "pregnoted", sourceRow("pregnoted"))
        Return Me.submitNewRecord(newRecord, currentTransaction)
    End Function
#End Region
#Region "Events"

    Public Function addBirth(ByVal sourceRow As DataRow) As Boolean
        Dim newRecord As DataTable = Me.newRecordValuesTable("[DSS].[birth]")
        Me.addColValue(newRecord, "eventID", sourceRow("eventID"))
        Me.addColValue(newRecord, "individid", sourceRow("individid"))
        Me.addColValue(newRecord, "motherid", sourceRow("motherid"))
        Me.addColValue(newRecord, "childcried", sourceRow("childcried"))
        Me.addColValue(newRecord, "childbreat", sourceRow("childbreat"))
        Me.addColValue(newRecord, "borndead", sourceRow("borndead"))
        Me.addColValue(newRecord, "rec_status", "V")
        Return Me.submitNewRecord(newRecord, currentTransaction)
    End Function
    Public Function addMigration(ByVal sourceRow As DataRow) As Boolean
        Dim newRecord As DataTable = Me.newRecordValuesTable("[DSS].[Migrations]")
        Me.addColValue(newRecord, "eventID", sourceRow("eventID"))
        Me.addColValue(newRecord, "individid", sourceRow("individid"))
        Me.addColValue(newRecord, "type", sourceRow("Type"))
        Me.addColValue(newRecord, "region_nam", sourceRow("region_nam"))
        Me.addColValue(newRecord, "family_nam", sourceRow("family_nam"))
        Me.addColValue(newRecord, "reason", sourceRow("reason"))
        Me.addColValue(newRecord, "reason_other", sourceRow("reason_other"))
        Me.addColValue(newRecord, "ruralUrban", sourceRow("ruralUrban"))
        Me.addColValue(newRecord, "exemption", sourceRow("exemption"))
        Me.addColValue(newRecord, "rec_status", "V")
        Return Me.submitNewRecord(newRecord, currentTransaction)
    End Function

    Public Function addPregnancyOutcome( ByVal sourceRow As DataRow) As Boolean
        Dim newRecord As DataTable = Me.newRecordValuesTable("[DSS].[pregoutcome]")
        Me.addColValue(newRecord, "eventID", sourceRow("eventID"))
        Me.addColValue(newRecord, "individid", sourceRow("individid"))
        Me.addColValue(newRecord, "type", sourceRow("Type"))
        Me.addColValue(newRecord, "date", sourceRow("date"))
        Me.addColValue(newRecord, "everborn", sourceRow("everborn"))
        Me.addColValue(newRecord, "livebirths", sourceRow("livebirths"))
        Me.addColValue(newRecord, "rec_status", "V")
        Return Me.submitNewRecord(newRecord, currentTransaction)
    End Function
#End Region
#Region "Events_Episodes "
    Public Function newEventEpisode(ByVal sourceRow As DataRow) As Boolean
        Dim newRecord As DataTable = Me.newRecordValuesTable("[DSS].[Events_Episodes]")
        Me.addColValue(newRecord, "EventID", sourceRow("EventID"))
        Me.addColValue(newRecord, "EventType", sourceRow("EventType"))
        Me.addColValue(newRecord, "EpisodeID", sourceRow("EpisodeID"))
        Me.addColValue(newRecord, "EpisodeType", sourceRow("EpisodeType"))
        Me.addColValue(newRecord, "rec_status", "V")
        Return Me.submitNewRecord(newRecord, currentTransaction)
    End Function
    Public Function updateEventEpisode(ByVal EventID As Guid, _
                                ByVal EpisodeID As Guid, _
                               ByVal columnName As String, _
                               ByVal newValue As Object) As Boolean
        Dim wherePart As String = " where  (EventID='" + EventID.ToString + "')  AND " _
                                & "(EpisodeID='" + EpisodeID.ToString + "')"
        Dim updateRecord As DataTable = Me.newRecordValuesTable("[DSS].[Events_Episodes]")
        Me.addColValue(updateRecord, columnName, newValue)
        Return Me.updateRecord(updateRecord, wherePart)
    End Function
#End Region
#Region "Other HRS TABLES"
    Public Function addObservation(ByVal sourceRow As DataRow) As Boolean
        'check first if the observation location & round has been recorded before we cant have observations>1
        If observationExists(sourceRow("locationid").ToString, sourceRow("round").ToString) Then Return False
        Dim newRecord As DataTable = Me.newRecordValuesTable("[DSS].[observation]")
        Me.addColValue(newRecord, "observeid", sourceRow("observeid"))
        Me.addColValue(newRecord, "locationid", sourceRow("locationid"))
        Me.addColValue(newRecord, "round", sourceRow("round"))
        Me.addColValue(newRecord, "date", sourceRow("date"))
        Me.addColValue(newRecord, "fieldworker", sourceRow("fieldworker"))
        Me.addColValue(newRecord, "rec_status", "V")
        Return Me.submitNewRecord(newRecord, currentTransaction)
    End Function
    ''' <summary>
    ''' checks to see whether an observation of the passed location and round has been made
    ''' 
    ''' </summary>
    ''' <param name="locationid"></param>
    ''' <param name="round"></param>
    ''' <returns>true to signify observation has been recorded, false otherwise</returns>
    ''' <remarks></remarks>
    Private Function observationExists(ByVal locationid As String, ByVal round As String) As Boolean
        Dim cmd As New SqlCommand
        cmd.Connection = clsGlobalVariable.HRS_Main_DBCon
        'set this command to be a member of a transaction
        If Not currentTransaction Is Nothing Then cmd.Transaction = currentTransaction
        cmd.CommandText = "Select count(*) from [DSS].[observation] WHERE locationid='" & locationid & "' AND round = '" & round & "'"
        clsGlobalVariable.open_HRS_Main_DBCon()
        Dim count As Integer = cmd.ExecuteScalar()
        clsGlobalVariable.close_HRS_Main_DBCon()
        If count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function addindvstatus(ByVal sourceRow As DataRow) As Boolean
        Dim newRecord As DataTable = Me.newRecordValuesTable("[DSS].[indvstatus]")
        Me.addColValue(newRecord, "eventID", sourceRow("eventID"))
        Me.addColValue(newRecord, "individid", sourceRow("individid"))
        Me.addColValue(newRecord, "observeid", sourceRow("observeid"))
        Me.addColValue(newRecord, "date", sourceRow("date"))
        Me.addColValue(newRecord, "type", sourceRow("type"))
        Me.addColValue(newRecord, "rec_status", "V")
        Return Me.submitNewRecord(newRecord)
    End Function
    Public Function addMarriage(ByVal sourceRow As DataRow) As Boolean
        Dim newRecord As DataTable = Me.newRecordValuesTable("[DSS].[marriage]")
        Me.addColValue(newRecord, "individid", sourceRow("individid"))
        Me.addColValue(newRecord, "episodeid", sourceRow("episodeid"))
        Me.addColValue(newRecord, "observeid", sourceRow("observeid"))
        Me.addColValue(newRecord, "visitdate", sourceRow("visitdate"))
        Me.addColValue(newRecord, "marital", sourceRow("marital"))
        Me.addColValue(newRecord, "firstmar", sourceRow("firstmar"))
        Me.addColValue(newRecord, "marrytyp", sourceRow("marrytyp"))
        Me.addColValue(newRecord, "spouseliv", sourceRow("spouseliv"))
        Me.addColValue(newRecord, "polyspno", sourceRow("polyspno"))
        Me.addColValue(newRecord, "polyspna", sourceRow("polyspna"))
        Me.addColValue(newRecord, "rank", sourceRow("rank"))
        Me.addColValue(newRecord, "rankna", sourceRow("rankna"))
        Me.addColValue(newRecord, "polyliv", sourceRow("polyliv"))
        Me.addColValue(newRecord, "polylivna", sourceRow("polylivna"))
        Me.addColValue(newRecord, "spoushh", sourceRow("spoushh"))
        Me.addColValue(newRecord, "spna", sourceRow("spna"))
        Me.addColValue(newRecord, "compname", sourceRow("compname"))
        Me.addColValue(newRecord, "spfname", sourceRow("spfname"))
        Me.addColValue(newRecord, "spjname", sourceRow("spjname"))
        Me.addColValue(newRecord, "splname", sourceRow("splname"))
        Me.addColValue(newRecord, "spouseid", sourceRow("spouseid"))
        Me.addColValue(newRecord, "spouseidna", sourceRow("spouseidna"))
        Me.addColValue(newRecord, "spnamena", sourceRow("spnamena"))
        Me.addColValue(newRecord, "fieldworker", sourceRow("fieldworker"))
        Me.addColValue(newRecord, "rec_status", "V")
        Return Me.submitNewRecord(newRecord, currentTransaction)
    End Function

    Public Function addMigReconRecord(ByVal eventID As Guid, _
                                     ByVal everReg As String, _
                                     ByVal lreghhldid As String, _
                                    ByVal dob As Date, _
                                     ByVal gender As String, _
                                     ByVal motherid As String, _
                                     ByVal mfname As String, _
                                    ByVal mjname As String, _
                                    ByVal momresid As String, _
                                    ByVal momhhid As String, _
                                    ByVal fatherid As String, _
                                    ByVal ffname As String, _
                                    ByVal fjname As String, _
                                   ByVal dadresid As String, _
                                    ByVal dadhhid As String) As Boolean
        Dim newRecord As DataTable = Me.newRecordValuesTable("[MHRS_SYS].[migrationRecon]")

        Me.addColValue(newRecord, "[eventID]", eventID)
        Me.addColValue(newRecord, "[everReg]", everReg)
        Me.addColValue(newRecord, "[lreghhldid]", lreghhldid)
        Me.addColValue(newRecord, "[dob]", dob)
        Me.addColValue(newRecord, "[gender]", gender)
        Me.addColValue(newRecord, "[motherid]", motherid)
        Me.addColValue(newRecord, "[mfname]", mfname)
        Me.addColValue(newRecord, "[mjname]", mjname)
        Me.addColValue(newRecord, "[momresid]", momresid)
        Me.addColValue(newRecord, "[momhhid]", momhhid)
        Me.addColValue(newRecord, "[fatherid]", fatherid)
        Me.addColValue(newRecord, "[ffname]", ffname)
        Me.addColValue(newRecord, "[fjname]", fjname)
        Me.addColValue(newRecord, "[dadresid]", dadresid)
        Me.addColValue(newRecord, "[dadhhid]", dadhhid)


        Return Me.submitNewRecord(newRecord, currentTransaction)
    End Function

    Public Function addTempMigrations(ByVal ID As Guid, _
                                        ByVal Pid As String, _
                                        ByVal Fname As String, _
                                        ByVal Mname As String, _
                                        ByVal Lname As String, _
                                        ByVal [Date] As Date, _
                                        ByVal [Round] As String, _
                                        ByVal [Type] As String, _
                                        ByVal [Socialgpid] As String, _
                                        ByVal [Details] As String, _
                                        ByVal [Status] As String) As Boolean

        Dim newRecord As DataTable = Me.newRecordValuesTable("[MHRS_SYS].[TempMigrations]")

        Me.addColValue(newRecord, "[ID]", ID)
        Me.addColValue(newRecord, "[Pid]", Pid)
        Me.addColValue(newRecord, "[Fname]", Fname)
        Me.addColValue(newRecord, "[Mname]", Mname)
        Me.addColValue(newRecord, "[Lname]", Lname)
        Me.addColValue(newRecord, "[Date]", [Date])
        Me.addColValue(newRecord, "[Round]", Round)
        Me.addColValue(newRecord, "[Type]", Type)
        Me.addColValue(newRecord, "[Socialgpid]", Socialgpid)
        Me.addColValue(newRecord, "[Details]", Details)
        Me.addColValue(newRecord, "[Status]", Status)

        If Me.submitNewRecord(newRecord, currentTransaction) Then
            Return True 'updateDownloadItems("DSS.membership", ID.ToString, "INSERT", currentTransaction)
        Else
            Return False
        End If

    End Function

    Public Function updateTempMigrations(ByVal ID As Guid, _
                                ByVal status As String) As Boolean

        Dim updateRecord As DataTable = Me.newRecordValuesTable("[MHRS_SYS].[TempMigrations]")
        Me.addColValue(updateRecord, "[Status]", status)

        Return Me.updateRecord(updateRecord, " where ID='" & ID.ToString & "'", currentTransaction)

    End Function
    Public Function updateTempMigrations(ByVal PID As String, _
                                    ByVal sgpid As String, _
                                    ByVal status As String) As Boolean

        Dim updateRecord As DataTable = Me.newRecordValuesTable("[MHRS_SYS].[TempMigrations]")
        Me.addColValue(updateRecord, "[Status]", status)

        Return Me.updateRecord(updateRecord, " where PID='" & PID.ToString & "' AND Socialgpid='" & sgpid & "'", currentTransaction)

    End Function

    Public Function insertTempPID(ByVal tempid As String) As Boolean

        Dim newRecord As DataTable = Me.newRecordValuesTable("[MHRS_SYS].[tempPIDs]")
        Me.addColValue(newRecord, "[tempPID]", tempid)
        Return Me.submitNewRecord(newRecord, currentTransaction)

    End Function
    Private Function insertNextTempPID(ByVal tempid As String) As Boolean

        Dim newRecord As DataTable = Me.newRecordValuesTable("[MHRS_SYS].[nextTempPID]")
        Me.addColValue(newRecord, "[tempPID]", tempid)
        Return Me.submitNewRecord(newRecord, currentTransaction)

    End Function
    Public Function updateNextTempPiD(ByVal oldpid As String, ByVal newpid As String) As Boolean
        'ensure oldpid exists first
        If Not TempPidExists(oldpid) Then insertNextTempPID(oldpid)

        Dim updateRecord As DataTable = Me.newRecordValuesTable("[MHRS_SYS].[nextTempPID]")
        Me.addColValue(updateRecord, "[tempPID]", newpid)

        Return Me.updateRecord(updateRecord, "  WHERE tempPID='" & oldpid & "'", currentTransaction)


    End Function

    Public Function TempPidExists(ByVal tempid As String) As Boolean
        Dim sql As String = "SELECT count(*) FROM [MHRS_SYS].[nextTempPID] where (tempPID='" + tempid + "')"
        Dim cmd As New SqlCommand()
        cmd.Connection = clsGlobalVariable.HRS_Main_DBCon
        clsGlobalVariable.open_HRS_Main_DBCon()
        cmd.CommandText = sql
        If cmd.ExecuteScalar > 0 Then
            Return True
        Else
            Return False
        End If

    End Function
#End Region
#Region "Special Studies Tables"

    'Public Function addParentalSurvival(ByVal sourceRow As DataRow) As Boolean

    '    Dim newRecord As DataTable = Me.newRecordValuesTable("[SpecialStudies].[parentsurv]")
    '    Me.addColValue(newRecord, "psid", sourceRow("psid"))
    '    Me.addColValue(newRecord, "hhid", sourceRow("hhid"))
    '    Me.addColValue(newRecord, "individid", sourceRow("individid"))
    '    Me.addColValue(newRecord, "round", sourceRow("round"))
    '    Me.addColValue(newRecord, "malive", sourceRow("malive"))
    '    Me.addColValue(newRecord, "falive", sourceRow("falive"))
    '    Return Me.submitNewRecord(newRecord, currentTransaction)

    'End Function

    'Public Function addEducation(ByVal [individid] As String, _
    '                            ByVal [observeid] As String, _
    '                            ByVal [vill] As String, _
    '                             ByVal [everenr] As String, _
    '                            ByVal [enrol] As String, _
    '                            ByVal [whyenrol] As String, _
    '                            ByVal [othreas] As String, _
    '                            ByVal [edulevel] As String, _
    '                            ByVal [grade] As Integer, _
    '                            ByVal [eduyrs] As Integer, _
    '                            ByVal [engread] As String, _
    '                            ByVal [engwrite] As String, _
    '                            ByVal [engspk] As String, _
    '                            ByVal [kisread] As String, _
    '                            ByVal [kiswrite] As String, _
    '                            ByVal [kisspk] As String, _
    '                            ByVal round As Integer)

    '    Dim newRecord As DataTable = Me.newRecordValuesTable("[SpecialStudies].[education]")
    '    Me.addColValue(newRecord, "xid", individid & "-" & round)
    '    Me.addColValue(newRecord, "individid", individid)
    '    Me.addColValue(newRecord, "seq", "")
    '    Me.addColValue(newRecord, "[observeid]", [observeid])
    '    Me.addColValue(newRecord, "[vill]", [vill])
    '    Me.addColValue(newRecord, "[date]", Now.Date)
    '    Me.addColValue(newRecord, "[status]", "")
    '    Me.addColValue(newRecord, "[everenr]", [everenr])
    '    Me.addColValue(newRecord, "[enrol]", [enrol])
    '    Me.addColValue(newRecord, "[whyenrol]", [whyenrol])
    '    Me.addColValue(newRecord, "[othreas]", [othreas])
    '    Me.addColValue(newRecord, "[edulevel]", [edulevel])
    '    Me.addColValue(newRecord, "[grade]", [grade])
    '    Me.addColValue(newRecord, "[eduyrs]", [eduyrs])
    '    Me.addColValue(newRecord, "[engread]", [engread])
    '    Me.addColValue(newRecord, "[engwrite]", [engwrite])
    '    Me.addColValue(newRecord, "[engspk]", [engspk])
    '    Me.addColValue(newRecord, "[kisread]", [kisread])
    '    Me.addColValue(newRecord, "[kiswrite]", [kiswrite])
    '    Me.addColValue(newRecord, "[kisspk]", [kisspk])
    '    Me.addColValue(newRecord, "[year]", Today.Year)
    '    Me.addColValue(newRecord, "[glocid]", 0)
    '    Me.addColValue(newRecord, "[stat_dat]", "P")
    '    Return Me.submitNewRecord(newRecord, currentTransaction)
    'End Function

    'Public Function addReligion(ByVal [religionid] As Guid, _
    '                            ByVal [round] As String, _
    '                            ByVal [individid] As String, _
    '                            ByVal [religion] As String, _
    '                            ByVal [othrel] As String, _
    '                            ByVal [observid] As String) As Boolean

    '    Dim newRecord As DataTable = Me.newRecordValuesTable("[SpecialStudies].[religion]")

    '    Me.addColValue(newRecord, "[religionid]", religionid)
    '    Me.addColValue(newRecord, "[round]", round)
    '    Me.addColValue(newRecord, "[individid]", individid)
    '    Me.addColValue(newRecord, "[religion]", religion)
    '    Me.addColValue(newRecord, "[othrel]", othrel)
    '    Me.addColValue(newRecord, "[observid]", observid)
    '    Return Me.submitNewRecord(newRecord, currentTransaction)
    'End Function

    'Public Function addEthnicity(ByVal [ethnicid] As Guid, _
    '                            ByVal [round] As String, _
    '                            ByVal [individid] As String, _
    '                            ByVal [ethnic] As String, _
    '                            ByVal [oth_ethnic] As String, _
    '                            ByVal [observid] As String) As Boolean

    '    Dim newRecord As DataTable = Me.newRecordValuesTable("[SpecialStudies].[Ethnicity]")

    '    Me.addColValue(newRecord, "[ethnicid]", ethnicid)
    '    Me.addColValue(newRecord, "[round]", round)
    '    Me.addColValue(newRecord, "[individid]", individid)
    '    Me.addColValue(newRecord, "[ethnic]", ethnic)
    '    Me.addColValue(newRecord, "[oth_ethnic]", oth_ethnic)
    '    Me.addColValue(newRecord, "[observid]", observid)
    '    Return Me.submitNewRecord(newRecord, currentTransaction)
    'End Function
#End Region

End Class
