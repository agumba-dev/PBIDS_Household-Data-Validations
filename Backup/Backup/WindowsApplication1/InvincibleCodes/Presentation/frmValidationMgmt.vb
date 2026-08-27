Public Class frmValidationMgmt
    Private validation As clsForeignKeyValidation = clsForeignKeyValidation.getObject
    Private da As clsDataAccess = clsDataAccess.getObject
    Private globalvariables As clsGlobalVariables = clsGlobalVariables.getObject
    Public objRef As clsformrefrences = clsformrefrences.getObject
   
    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Try
            ' Add any initialization after the InitializeComponent() call.
            ' initialiseGlobalVariables()
            Me.bindCombo(Me.cmb_TableName, Me.da.getTableDataFromTempDB("select table_schema+'.'+table_name from information_schema.tables where table_schema in ('DSS','SpecialStudies','dbo','ghi') order by table_schema+'.'+table_name"))
            Me.bindCombo(Me.cmb_RefTable, Me.da.getTableDataFromTempDB("select table_schema+'.'+table_name from information_schema.tables where table_schema in ('DSS','SpecialStudies','dbo','ghi') order by table_schema+'.'+table_name"))

        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            MsgBox(ex.Message)

        End Try
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
            MsgBox(ex.Message)

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
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & strObjMethod)
        End Try
        Return server
    End Function
    Private Sub cmb_TableName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmb_TableName.SelectedIndexChanged
        Try
            Me.cmb_TableCol.SelectedIndex = -1
            Me.cmb_TableCol.Items.Clear()
            Me.cmb_RefTable.SelectedIndex = -1
            Me.cmb_refColumn.SelectedIndex = -1
            Me.cmb_refColumn.Items.Clear()
            Me.bindCombo(Me.cmb_TableCol, Me.da.getTableDataFromTempDB("select column_name from information_schema.columns where table_schema+'.'+table_name='" + Me.cmb_TableName.SelectedItem.ToString.Trim + "'"))
            Me.grid_validRules.DataSource = Me.da.getTableDataFromTempDB("SELECT * FROM [TEMP_DSSHRS].[dbo].[validationitems] where [table_name]='" + Me.cmb_TableName.SelectedItem.ToString.Trim + "'")
        Catch ex As Exception
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & strObjMethod)
        End Try
    End Sub
    Private Sub bindCombo(ByVal cmb As ComboBox, ByVal table As DataTable)
        cmb.Items.Clear()
        For Each item As DataRow In table.Rows
            cmb.Items.Add(item(0).ToString)
        Next
    End Sub

    Private Sub cmb_RefTable_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmb_RefTable.SelectedIndexChanged
        Try
            Me.cmb_refColumn.SelectedIndex = -1
            Me.cmb_refColumn.Items.Clear()
            Me.bindCombo(Me.cmb_refColumn, Me.da.getTableDataFromTempDB("select column_name from information_schema.columns where table_schema+'.'+table_name='" + Me.cmb_RefTable.SelectedItem.ToString.Trim + "'"))
        Catch ex As Exception
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & strObjMethod)
        End Try
    End Sub

    Private Sub btnADD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnADD.Click
        Try
            Dim tblname As String = Me.cmb_TableName.SelectedItem.ToString.Trim
            Dim colName As String = Me.cmb_TableCol.SelectedItem.ToString.Trim
            Dim reftbl As String = Me.cmb_RefTable.SelectedItem.ToString.Trim
            Dim refcol As String = Me.cmb_refColumn.SelectedItem.ToString.Trim
            If Me.da.AddValidationRule(tblname, colName, reftbl, refcol) Then
                Me.grid_validRules.DataSource = Me.da.getTableDataFromTempDB("SELECT * FROM [TEMP_DSSHRS].[dbo].[validationitems] where [table_name]='" + Me.cmb_TableName.SelectedItem.ToString.Trim + "'")
            Else
                MsgBox("fsdfkdsf")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & strObjMethod)
        End Try
    End Sub

  
End Class