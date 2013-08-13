<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Frm_TheValidationsEditor
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.serversComboBox = New System.Windows.Forms.ComboBox
        Me.findServersButton = New System.Windows.Forms.Button
        Me.databasesComboBox = New System.Windows.Forms.ComboBox
        Me.tableNameListBox = New System.Windows.Forms.ListBox
        Me.TableDetailsListView = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.grpBoxValidValues = New System.Windows.Forms.GroupBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.cmb_valueFunctions = New System.Windows.Forms.ComboBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.btn_save = New System.Windows.Forms.Button
        Me.txt_defaultValue = New System.Windows.Forms.TextBox
        Me.gpb_valueType = New System.Windows.Forms.GroupBox
        Me.rbtn_anyValue = New System.Windows.Forms.RadioButton
        Me.rbtn_singleValue = New System.Windows.Forms.RadioButton
        Me.rbtn_range = New System.Windows.Forms.RadioButton
        Me.pnl_daterange = New System.Windows.Forms.Panel
        Me.dtp_rangeFrom = New System.Windows.Forms.DateTimePicker
        Me.ckbCurrentDate = New System.Windows.Forms.CheckBox
        Me.dtp_rangeTo = New System.Windows.Forms.DateTimePicker
        Me.pnl_range = New System.Windows.Forms.Panel
        Me.Label5 = New System.Windows.Forms.Label
        Me.num_rangeFrom = New System.Windows.Forms.NumericUpDown
        Me.Label6 = New System.Windows.Forms.Label
        Me.num_rangeTo = New System.Windows.Forms.NumericUpDown
        Me.lblDefaultvalue = New System.Windows.Forms.Label
        Me.txt_errorDesc = New System.Windows.Forms.TextBox
        Me.btn_remove = New System.Windows.Forms.Button
        Me.Label12 = New System.Windows.Forms.Label
        Me.btn_add = New System.Windows.Forms.Button
        Me.pnl_singleValue = New System.Windows.Forms.Panel
        Me.dtp_singleValue = New System.Windows.Forms.DateTimePicker
        Me.txt_singleValue = New System.Windows.Forms.TextBox
        Me.lstbox_ValuesAllowed = New System.Windows.Forms.ListBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.GrpBoxSkiplogic = New System.Windows.Forms.GroupBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.cmbValidationStatus = New System.Windows.Forms.ComboBox
        Me.btn_removeClose = New System.Windows.Forms.Button
        Me.btn_insertclose = New System.Windows.Forms.Button
        Me.btn_removeopen = New System.Windows.Forms.Button
        Me.btn_insertopen = New System.Windows.Forms.Button
        Me.Label13 = New System.Windows.Forms.Label
        Me.txt_errordescSkipLogic = New System.Windows.Forms.TextBox
        Me.pnl_skipValues = New System.Windows.Forms.Panel
        Me.Label16 = New System.Windows.Forms.Label
        Me.cmb_skipFunctions = New System.Windows.Forms.ComboBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.rbtn_column = New System.Windows.Forms.RadioButton
        Me.rbtn_value = New System.Windows.Forms.RadioButton
        Me.cmb_booleanExp = New System.Windows.Forms.ComboBox
        Me.cmb_skipValue = New System.Windows.Forms.ComboBox
        Me.cmbColumnName = New System.Windows.Forms.ComboBox
        Me.btn_addSkiplogic = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.rbtn_or = New System.Windows.Forms.RadioButton
        Me.rbtn_and = New System.Windows.Forms.RadioButton
        Me.btn_RemoveSkipLogic = New System.Windows.Forms.Button
        Me.Label9 = New System.Windows.Forms.Label
        Me.gpb_skipCriteria = New System.Windows.Forms.GroupBox
        Me.rbtn_required = New System.Windows.Forms.RadioButton
        Me.rbtn_if = New System.Windows.Forms.RadioButton
        Me.Label8 = New System.Windows.Forms.Label
        Me.lstbox_SkipAdded = New System.Windows.Forms.ListBox
        Me.grpBoxDataGrid = New System.Windows.Forms.GroupBox
        Me.dgvValidations = New System.Windows.Forms.DataGridView
        Me.Label11 = New System.Windows.Forms.Label
        Me.btnconnect = New System.Windows.Forms.Button
        Me.Label10 = New System.Windows.Forms.Label
        Me.lblselectedColumn = New System.Windows.Forms.Label
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.OptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ValidationsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ContextMenuDefaultvalue = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SetAsDefaultValueToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ckb_insertDate = New System.Windows.Forms.CheckBox
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.grpBoxValidValues.SuspendLayout()
        Me.gpb_valueType.SuspendLayout()
        Me.pnl_daterange.SuspendLayout()
        Me.pnl_range.SuspendLayout()
        CType(Me.num_rangeFrom, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.num_rangeTo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl_singleValue.SuspendLayout()
        Me.GrpBoxSkiplogic.SuspendLayout()
        Me.pnl_skipValues.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.gpb_skipCriteria.SuspendLayout()
        Me.grpBoxDataGrid.SuspendLayout()
        CType(Me.dgvValidations, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        Me.ContextMenuDefaultvalue.SuspendLayout()
        Me.SuspendLayout()
        '
        'serversComboBox
        '
        Me.serversComboBox.FormattingEnabled = True
        Me.serversComboBox.Items.AddRange(New Object() {"KISW-DSS-09948"})
        Me.serversComboBox.Location = New System.Drawing.Point(3, 19)
        Me.serversComboBox.Name = "serversComboBox"
        Me.serversComboBox.Size = New System.Drawing.Size(121, 21)
        Me.serversComboBox.TabIndex = 0
        Me.serversComboBox.Visible = False
        '
        'findServersButton
        '
        Me.findServersButton.Location = New System.Drawing.Point(128, 19)
        Me.findServersButton.Name = "findServersButton"
        Me.findServersButton.Size = New System.Drawing.Size(75, 23)
        Me.findServersButton.TabIndex = 1
        Me.findServersButton.Text = "FindServers"
        Me.findServersButton.UseVisualStyleBackColor = True
        Me.findServersButton.Visible = False
        '
        'databasesComboBox
        '
        Me.databasesComboBox.FormattingEnabled = True
        Me.databasesComboBox.Location = New System.Drawing.Point(3, 63)
        Me.databasesComboBox.Name = "databasesComboBox"
        Me.databasesComboBox.Size = New System.Drawing.Size(121, 21)
        Me.databasesComboBox.TabIndex = 2
        Me.databasesComboBox.Visible = False
        '
        'tableNameListBox
        '
        Me.tableNameListBox.FormattingEnabled = True
        Me.tableNameListBox.Location = New System.Drawing.Point(3, 109)
        Me.tableNameListBox.Name = "tableNameListBox"
        Me.tableNameListBox.Size = New System.Drawing.Size(247, 56)
        Me.tableNameListBox.TabIndex = 3
        '
        'TableDetailsListView
        '
        Me.TableDetailsListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3})
        Me.TableDetailsListView.FullRowSelect = True
        Me.TableDetailsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.TableDetailsListView.Location = New System.Drawing.Point(3, 180)
        Me.TableDetailsListView.Name = "TableDetailsListView"
        Me.TableDetailsListView.Size = New System.Drawing.Size(247, 123)
        Me.TableDetailsListView.TabIndex = 6
        Me.TableDetailsListView.UseCompatibleStateImageBehavior = False
        Me.TableDetailsListView.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Column Name"
        Me.ColumnHeader1.Width = 116
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Data Type"
        Me.ColumnHeader2.Width = 133
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Size"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 13)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Choose server"
        Me.Label1.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 47)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(89, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "choose database"
        Me.Label2.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(2, 93)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(138, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "choose table to view details"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(10, 164)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(75, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Column details"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 24)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.grpBoxValidValues)
        Me.SplitContainer1.Panel1.Controls.Add(Me.GrpBoxSkiplogic)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.AutoScroll = True
        Me.SplitContainer1.Panel2.Controls.Add(Me.grpBoxDataGrid)
        Me.SplitContainer1.Panel2.Controls.Add(Me.TableDetailsListView)
        Me.SplitContainer1.Panel2.Controls.Add(Me.tableNameListBox)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label11)
        Me.SplitContainer1.Panel2.Controls.Add(Me.databasesComboBox)
        Me.SplitContainer1.Panel2.Controls.Add(Me.serversComboBox)
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnconnect)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label10)
        Me.SplitContainer1.Panel2.Controls.Add(Me.findServersButton)
        Me.SplitContainer1.Panel2.Controls.Add(Me.lblselectedColumn)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label3)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label4)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label2)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label1)
        Me.SplitContainer1.Size = New System.Drawing.Size(1174, 665)
        Me.SplitContainer1.SplitterDistance = 313
        Me.SplitContainer1.TabIndex = 12
        '
        'grpBoxValidValues
        '
        Me.grpBoxValidValues.Controls.Add(Me.Label15)
        Me.grpBoxValidValues.Controls.Add(Me.cmb_valueFunctions)
        Me.grpBoxValidValues.Controls.Add(Me.Label14)
        Me.grpBoxValidValues.Controls.Add(Me.btn_save)
        Me.grpBoxValidValues.Controls.Add(Me.txt_defaultValue)
        Me.grpBoxValidValues.Controls.Add(Me.gpb_valueType)
        Me.grpBoxValidValues.Controls.Add(Me.pnl_daterange)
        Me.grpBoxValidValues.Controls.Add(Me.pnl_range)
        Me.grpBoxValidValues.Controls.Add(Me.lblDefaultvalue)
        Me.grpBoxValidValues.Controls.Add(Me.txt_errorDesc)
        Me.grpBoxValidValues.Controls.Add(Me.btn_remove)
        Me.grpBoxValidValues.Controls.Add(Me.Label12)
        Me.grpBoxValidValues.Controls.Add(Me.btn_add)
        Me.grpBoxValidValues.Controls.Add(Me.pnl_singleValue)
        Me.grpBoxValidValues.Controls.Add(Me.lstbox_ValuesAllowed)
        Me.grpBoxValidValues.Controls.Add(Me.Label7)
        Me.grpBoxValidValues.Location = New System.Drawing.Point(566, 0)
        Me.grpBoxValidValues.Name = "grpBoxValidValues"
        Me.grpBoxValidValues.Size = New System.Drawing.Size(601, 306)
        Me.grpBoxValidValues.TabIndex = 22
        Me.grpBoxValidValues.TabStop = False
        Me.grpBoxValidValues.Text = "Valid Values"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(23, 54)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(81, 13)
        Me.Label15.TabIndex = 31
        Me.Label15.Text = "Select Function"
        '
        'cmb_valueFunctions
        '
        Me.cmb_valueFunctions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmb_valueFunctions.FormattingEnabled = True
        Me.cmb_valueFunctions.Items.AddRange(New Object() {"", "getAge", "validLocation", "validIndividID", "validObservationID", "validCompoundID", "validVillageID"})
        Me.cmb_valueFunctions.Location = New System.Drawing.Point(108, 50)
        Me.cmb_valueFunctions.Name = "cmb_valueFunctions"
        Me.cmb_valueFunctions.Size = New System.Drawing.Size(157, 21)
        Me.cmb_valueFunctions.TabIndex = 30
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(15, 259)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(74, 13)
        Me.Label14.TabIndex = 23
        Me.Label14.Text = "Default Value:"
        '
        'btn_save
        '
        Me.btn_save.Location = New System.Drawing.Point(520, 277)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(75, 23)
        Me.btn_save.TabIndex = 13
        Me.btn_save.Text = "Save"
        Me.btn_save.UseVisualStyleBackColor = True
        '
        'txt_defaultValue
        '
        Me.txt_defaultValue.Location = New System.Drawing.Point(95, 256)
        Me.txt_defaultValue.Name = "txt_defaultValue"
        Me.txt_defaultValue.Size = New System.Drawing.Size(170, 20)
        Me.txt_defaultValue.TabIndex = 22
        '
        'gpb_valueType
        '
        Me.gpb_valueType.Controls.Add(Me.rbtn_anyValue)
        Me.gpb_valueType.Controls.Add(Me.rbtn_singleValue)
        Me.gpb_valueType.Controls.Add(Me.rbtn_range)
        Me.gpb_valueType.Enabled = False
        Me.gpb_valueType.Location = New System.Drawing.Point(12, 11)
        Me.gpb_valueType.Name = "gpb_valueType"
        Me.gpb_valueType.Size = New System.Drawing.Size(253, 35)
        Me.gpb_valueType.TabIndex = 0
        Me.gpb_valueType.TabStop = False
        Me.gpb_valueType.Text = "Value Type"
        '
        'rbtn_anyValue
        '
        Me.rbtn_anyValue.AutoSize = True
        Me.rbtn_anyValue.Location = New System.Drawing.Point(162, 13)
        Me.rbtn_anyValue.Name = "rbtn_anyValue"
        Me.rbtn_anyValue.Size = New System.Drawing.Size(73, 17)
        Me.rbtn_anyValue.TabIndex = 2
        Me.rbtn_anyValue.TabStop = True
        Me.rbtn_anyValue.Text = "Any Value"
        Me.rbtn_anyValue.UseVisualStyleBackColor = True
        '
        'rbtn_singleValue
        '
        Me.rbtn_singleValue.AutoSize = True
        Me.rbtn_singleValue.Location = New System.Drawing.Point(72, 15)
        Me.rbtn_singleValue.Name = "rbtn_singleValue"
        Me.rbtn_singleValue.Size = New System.Drawing.Size(84, 17)
        Me.rbtn_singleValue.TabIndex = 1
        Me.rbtn_singleValue.TabStop = True
        Me.rbtn_singleValue.Text = "Single Value"
        Me.rbtn_singleValue.UseVisualStyleBackColor = True
        '
        'rbtn_range
        '
        Me.rbtn_range.AutoSize = True
        Me.rbtn_range.Location = New System.Drawing.Point(6, 14)
        Me.rbtn_range.Name = "rbtn_range"
        Me.rbtn_range.Size = New System.Drawing.Size(60, 17)
        Me.rbtn_range.TabIndex = 0
        Me.rbtn_range.TabStop = True
        Me.rbtn_range.Text = "Range "
        Me.rbtn_range.UseVisualStyleBackColor = True
        '
        'pnl_daterange
        '
        Me.pnl_daterange.Controls.Add(Me.dtp_rangeFrom)
        Me.pnl_daterange.Controls.Add(Me.ckbCurrentDate)
        Me.pnl_daterange.Controls.Add(Me.dtp_rangeTo)
        Me.pnl_daterange.Enabled = False
        Me.pnl_daterange.Location = New System.Drawing.Point(10, 128)
        Me.pnl_daterange.Name = "pnl_daterange"
        Me.pnl_daterange.Size = New System.Drawing.Size(255, 45)
        Me.pnl_daterange.TabIndex = 20
        '
        'dtp_rangeFrom
        '
        Me.dtp_rangeFrom.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_rangeFrom.Location = New System.Drawing.Point(6, 3)
        Me.dtp_rangeFrom.Name = "dtp_rangeFrom"
        Me.dtp_rangeFrom.Size = New System.Drawing.Size(112, 20)
        Me.dtp_rangeFrom.TabIndex = 6
        '
        'ckbCurrentDate
        '
        Me.ckbCurrentDate.AutoSize = True
        Me.ckbCurrentDate.Location = New System.Drawing.Point(139, 26)
        Me.ckbCurrentDate.Name = "ckbCurrentDate"
        Me.ckbCurrentDate.Size = New System.Drawing.Size(86, 17)
        Me.ckbCurrentDate.TabIndex = 15
        Me.ckbCurrentDate.Text = "Current Date"
        Me.ckbCurrentDate.UseVisualStyleBackColor = True
        '
        'dtp_rangeTo
        '
        Me.dtp_rangeTo.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_rangeTo.Location = New System.Drawing.Point(139, 3)
        Me.dtp_rangeTo.Name = "dtp_rangeTo"
        Me.dtp_rangeTo.Size = New System.Drawing.Size(112, 20)
        Me.dtp_rangeTo.TabIndex = 8
        '
        'pnl_range
        '
        Me.pnl_range.Controls.Add(Me.Label5)
        Me.pnl_range.Controls.Add(Me.num_rangeFrom)
        Me.pnl_range.Controls.Add(Me.Label6)
        Me.pnl_range.Controls.Add(Me.num_rangeTo)
        Me.pnl_range.Enabled = False
        Me.pnl_range.Location = New System.Drawing.Point(12, 77)
        Me.pnl_range.Name = "pnl_range"
        Me.pnl_range.Size = New System.Drawing.Size(253, 49)
        Me.pnl_range.TabIndex = 16
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(26, 6)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(36, 13)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "From :"
        '
        'num_rangeFrom
        '
        Me.num_rangeFrom.Location = New System.Drawing.Point(3, 25)
        Me.num_rangeFrom.Maximum = New Decimal(New Integer() {10000000, 0, 0, 0})
        Me.num_rangeFrom.Name = "num_rangeFrom"
        Me.num_rangeFrom.Size = New System.Drawing.Size(112, 20)
        Me.num_rangeFrom.TabIndex = 2
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(145, 6)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(26, 13)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "To :"
        '
        'num_rangeTo
        '
        Me.num_rangeTo.Location = New System.Drawing.Point(136, 25)
        Me.num_rangeTo.Maximum = New Decimal(New Integer() {10000000, 0, 0, 0})
        Me.num_rangeTo.Name = "num_rangeTo"
        Me.num_rangeTo.Size = New System.Drawing.Size(110, 20)
        Me.num_rangeTo.TabIndex = 3
        '
        'lblDefaultvalue
        '
        Me.lblDefaultvalue.AutoSize = True
        Me.lblDefaultvalue.Font = New System.Drawing.Font("Arial Black", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDefaultvalue.ForeColor = System.Drawing.Color.DarkOrange
        Me.lblDefaultvalue.Location = New System.Drawing.Point(7, 281)
        Me.lblDefaultvalue.Name = "lblDefaultvalue"
        Me.lblDefaultvalue.Size = New System.Drawing.Size(133, 23)
        Me.lblDefaultvalue.TabIndex = 21
        Me.lblDefaultvalue.Text = "Default Value:"
        '
        'txt_errorDesc
        '
        Me.txt_errorDesc.Location = New System.Drawing.Point(348, 218)
        Me.txt_errorDesc.Multiline = True
        Me.txt_errorDesc.Name = "txt_errorDesc"
        Me.txt_errorDesc.Size = New System.Drawing.Size(247, 40)
        Me.txt_errorDesc.TabIndex = 18
        '
        'btn_remove
        '
        Me.btn_remove.Location = New System.Drawing.Point(271, 179)
        Me.btn_remove.Name = "btn_remove"
        Me.btn_remove.Size = New System.Drawing.Size(73, 30)
        Me.btn_remove.TabIndex = 10
        Me.btn_remove.Text = "<<REMOVE"
        Me.btn_remove.UseVisualStyleBackColor = True
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(345, 202)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(85, 13)
        Me.Label12.TabIndex = 19
        Me.Label12.Text = "Error Description"
        '
        'btn_add
        '
        Me.btn_add.Location = New System.Drawing.Point(271, 123)
        Me.btn_add.Name = "btn_add"
        Me.btn_add.Size = New System.Drawing.Size(73, 33)
        Me.btn_add.TabIndex = 9
        Me.btn_add.Text = "ADD>>"
        Me.btn_add.UseVisualStyleBackColor = True
        '
        'pnl_singleValue
        '
        Me.pnl_singleValue.Controls.Add(Me.ckb_insertDate)
        Me.pnl_singleValue.Controls.Add(Me.dtp_singleValue)
        Me.pnl_singleValue.Controls.Add(Me.txt_singleValue)
        Me.pnl_singleValue.Enabled = False
        Me.pnl_singleValue.Location = New System.Drawing.Point(12, 178)
        Me.pnl_singleValue.Name = "pnl_singleValue"
        Me.pnl_singleValue.Size = New System.Drawing.Size(253, 73)
        Me.pnl_singleValue.TabIndex = 17
        '
        'dtp_singleValue
        '
        Me.dtp_singleValue.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtp_singleValue.Location = New System.Drawing.Point(136, 46)
        Me.dtp_singleValue.Name = "dtp_singleValue"
        Me.dtp_singleValue.Size = New System.Drawing.Size(112, 20)
        Me.dtp_singleValue.TabIndex = 16
        '
        'txt_singleValue
        '
        Me.txt_singleValue.Location = New System.Drawing.Point(4, 3)
        Me.txt_singleValue.Multiline = True
        Me.txt_singleValue.Name = "txt_singleValue"
        Me.txt_singleValue.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txt_singleValue.Size = New System.Drawing.Size(242, 39)
        Me.txt_singleValue.TabIndex = 12
        '
        'lstbox_ValuesAllowed
        '
        Me.lstbox_ValuesAllowed.FormattingEnabled = True
        Me.lstbox_ValuesAllowed.Location = New System.Drawing.Point(348, 42)
        Me.lstbox_ValuesAllowed.Name = "lstbox_ValuesAllowed"
        Me.lstbox_ValuesAllowed.Size = New System.Drawing.Size(247, 160)
        Me.lstbox_ValuesAllowed.TabIndex = 11
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(345, 26)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(79, 13)
        Me.Label7.TabIndex = 13
        Me.Label7.Text = "Values Allowed"
        '
        'GrpBoxSkiplogic
        '
        Me.GrpBoxSkiplogic.Controls.Add(Me.Label17)
        Me.GrpBoxSkiplogic.Controls.Add(Me.cmbValidationStatus)
        Me.GrpBoxSkiplogic.Controls.Add(Me.btn_removeClose)
        Me.GrpBoxSkiplogic.Controls.Add(Me.btn_insertclose)
        Me.GrpBoxSkiplogic.Controls.Add(Me.btn_removeopen)
        Me.GrpBoxSkiplogic.Controls.Add(Me.btn_insertopen)
        Me.GrpBoxSkiplogic.Controls.Add(Me.Label13)
        Me.GrpBoxSkiplogic.Controls.Add(Me.txt_errordescSkipLogic)
        Me.GrpBoxSkiplogic.Controls.Add(Me.pnl_skipValues)
        Me.GrpBoxSkiplogic.Controls.Add(Me.gpb_skipCriteria)
        Me.GrpBoxSkiplogic.Controls.Add(Me.Label8)
        Me.GrpBoxSkiplogic.Controls.Add(Me.lstbox_SkipAdded)
        Me.GrpBoxSkiplogic.Location = New System.Drawing.Point(3, 8)
        Me.GrpBoxSkiplogic.Name = "GrpBoxSkiplogic"
        Me.GrpBoxSkiplogic.Size = New System.Drawing.Size(556, 298)
        Me.GrpBoxSkiplogic.TabIndex = 14
        Me.GrpBoxSkiplogic.TabStop = False
        Me.GrpBoxSkiplogic.Text = "Skip Logic"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(361, 251)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(86, 13)
        Me.Label17.TabIndex = 34
        Me.Label17.Text = "Validation Status"
        '
        'cmbValidationStatus
        '
        Me.cmbValidationStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbValidationStatus.FormattingEnabled = True
        Me.cmbValidationStatus.Items.AddRange(New Object() {"ACTIVE", "INACTIVE"})
        Me.cmbValidationStatus.Location = New System.Drawing.Point(448, 247)
        Me.cmbValidationStatus.Name = "cmbValidationStatus"
        Me.cmbValidationStatus.Size = New System.Drawing.Size(102, 21)
        Me.cmbValidationStatus.TabIndex = 34
        '
        'btn_removeClose
        '
        Me.btn_removeClose.Location = New System.Drawing.Point(496, 114)
        Me.btn_removeClose.Name = "btn_removeClose"
        Me.btn_removeClose.Size = New System.Drawing.Size(52, 23)
        Me.btn_removeClose.TabIndex = 27
        Me.btn_removeClose.Text = "Rem ')'"
        Me.btn_removeClose.UseVisualStyleBackColor = True
        '
        'btn_insertclose
        '
        Me.btn_insertclose.Location = New System.Drawing.Point(496, 80)
        Me.btn_insertclose.Name = "btn_insertclose"
        Me.btn_insertclose.Size = New System.Drawing.Size(52, 23)
        Me.btn_insertclose.TabIndex = 26
        Me.btn_insertclose.Text = "Ins ')'"
        Me.btn_insertclose.UseVisualStyleBackColor = True
        '
        'btn_removeopen
        '
        Me.btn_removeopen.Location = New System.Drawing.Point(496, 46)
        Me.btn_removeopen.Name = "btn_removeopen"
        Me.btn_removeopen.Size = New System.Drawing.Size(52, 23)
        Me.btn_removeopen.TabIndex = 25
        Me.btn_removeopen.Text = "Rem '('"
        Me.btn_removeopen.UseVisualStyleBackColor = True
        '
        'btn_insertopen
        '
        Me.btn_insertopen.Location = New System.Drawing.Point(496, 12)
        Me.btn_insertopen.Name = "btn_insertopen"
        Me.btn_insertopen.Size = New System.Drawing.Size(52, 23)
        Me.btn_insertopen.TabIndex = 24
        Me.btn_insertopen.Text = "Ins '('"
        Me.btn_insertopen.UseVisualStyleBackColor = True
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(343, 135)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(85, 13)
        Me.Label13.TabIndex = 22
        Me.Label13.Text = "Error Description"
        '
        'txt_errordescSkipLogic
        '
        Me.txt_errordescSkipLogic.Location = New System.Drawing.Point(346, 151)
        Me.txt_errordescSkipLogic.Multiline = True
        Me.txt_errordescSkipLogic.Name = "txt_errordescSkipLogic"
        Me.txt_errordescSkipLogic.Size = New System.Drawing.Size(204, 40)
        Me.txt_errordescSkipLogic.TabIndex = 21
        '
        'pnl_skipValues
        '
        Me.pnl_skipValues.Controls.Add(Me.Label16)
        Me.pnl_skipValues.Controls.Add(Me.cmb_skipFunctions)
        Me.pnl_skipValues.Controls.Add(Me.Panel2)
        Me.pnl_skipValues.Controls.Add(Me.cmb_booleanExp)
        Me.pnl_skipValues.Controls.Add(Me.cmb_skipValue)
        Me.pnl_skipValues.Controls.Add(Me.cmbColumnName)
        Me.pnl_skipValues.Controls.Add(Me.btn_addSkiplogic)
        Me.pnl_skipValues.Controls.Add(Me.Panel1)
        Me.pnl_skipValues.Controls.Add(Me.btn_RemoveSkipLogic)
        Me.pnl_skipValues.Controls.Add(Me.Label9)
        Me.pnl_skipValues.Enabled = False
        Me.pnl_skipValues.Location = New System.Drawing.Point(9, 69)
        Me.pnl_skipValues.Name = "pnl_skipValues"
        Me.pnl_skipValues.Size = New System.Drawing.Size(328, 199)
        Me.pnl_skipValues.TabIndex = 23
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(7, 151)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(81, 13)
        Me.Label16.TabIndex = 33
        Me.Label16.Text = "Select Function"
        '
        'cmb_skipFunctions
        '
        Me.cmb_skipFunctions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmb_skipFunctions.FormattingEnabled = True
        Me.cmb_skipFunctions.Items.AddRange(New Object() {"", "getAge", "validLocation", "validIndividID", "validObservationID", "validCompoundID", "validVillageID"})
        Me.cmb_skipFunctions.Location = New System.Drawing.Point(92, 147)
        Me.cmb_skipFunctions.Name = "cmb_skipFunctions"
        Me.cmb_skipFunctions.Size = New System.Drawing.Size(157, 21)
        Me.cmb_skipFunctions.TabIndex = 32
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.rbtn_column)
        Me.Panel2.Controls.Add(Me.rbtn_value)
        Me.Panel2.Location = New System.Drawing.Point(6, 82)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(200, 26)
        Me.Panel2.TabIndex = 31
        '
        'rbtn_column
        '
        Me.rbtn_column.AutoSize = True
        Me.rbtn_column.Location = New System.Drawing.Point(82, 6)
        Me.rbtn_column.Name = "rbtn_column"
        Me.rbtn_column.Size = New System.Drawing.Size(60, 17)
        Me.rbtn_column.TabIndex = 33
        Me.rbtn_column.Text = "Column"
        Me.rbtn_column.UseVisualStyleBackColor = True
        '
        'rbtn_value
        '
        Me.rbtn_value.AutoSize = True
        Me.rbtn_value.Checked = True
        Me.rbtn_value.Location = New System.Drawing.Point(3, 6)
        Me.rbtn_value.Name = "rbtn_value"
        Me.rbtn_value.Size = New System.Drawing.Size(52, 17)
        Me.rbtn_value.TabIndex = 32
        Me.rbtn_value.TabStop = True
        Me.rbtn_value.Text = "Value"
        Me.rbtn_value.UseVisualStyleBackColor = True
        '
        'cmb_booleanExp
        '
        Me.cmb_booleanExp.FormattingEnabled = True
        Me.cmb_booleanExp.Items.AddRange(New Object() {"=", ">=", ">", "<=", "<", "<>"})
        Me.cmb_booleanExp.Location = New System.Drawing.Point(6, 54)
        Me.cmb_booleanExp.Name = "cmb_booleanExp"
        Me.cmb_booleanExp.Size = New System.Drawing.Size(145, 21)
        Me.cmb_booleanExp.TabIndex = 30
        '
        'cmb_skipValue
        '
        Me.cmb_skipValue.FormattingEnabled = True
        Me.cmb_skipValue.Location = New System.Drawing.Point(2, 114)
        Me.cmb_skipValue.Name = "cmb_skipValue"
        Me.cmb_skipValue.Size = New System.Drawing.Size(212, 21)
        Me.cmb_skipValue.TabIndex = 29
        '
        'cmbColumnName
        '
        Me.cmbColumnName.FormattingEnabled = True
        Me.cmbColumnName.Location = New System.Drawing.Point(3, 16)
        Me.cmbColumnName.Name = "cmbColumnName"
        Me.cmbColumnName.Size = New System.Drawing.Size(148, 21)
        Me.cmbColumnName.TabIndex = 19
        '
        'btn_addSkiplogic
        '
        Me.btn_addSkiplogic.Location = New System.Drawing.Point(250, 4)
        Me.btn_addSkiplogic.Name = "btn_addSkiplogic"
        Me.btn_addSkiplogic.Size = New System.Drawing.Size(75, 33)
        Me.btn_addSkiplogic.TabIndex = 11
        Me.btn_addSkiplogic.Text = "ADD>>"
        Me.btn_addSkiplogic.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rbtn_or)
        Me.Panel1.Controls.Add(Me.rbtn_and)
        Me.Panel1.Location = New System.Drawing.Point(155, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(94, 33)
        Me.Panel1.TabIndex = 21
        '
        'rbtn_or
        '
        Me.rbtn_or.AutoSize = True
        Me.rbtn_or.Checked = True
        Me.rbtn_or.Location = New System.Drawing.Point(46, 10)
        Me.rbtn_or.Name = "rbtn_or"
        Me.rbtn_or.Size = New System.Drawing.Size(41, 17)
        Me.rbtn_or.TabIndex = 1
        Me.rbtn_or.TabStop = True
        Me.rbtn_or.Text = "OR"
        Me.rbtn_or.UseVisualStyleBackColor = True
        '
        'rbtn_and
        '
        Me.rbtn_and.AutoSize = True
        Me.rbtn_and.Location = New System.Drawing.Point(3, 9)
        Me.rbtn_and.Name = "rbtn_and"
        Me.rbtn_and.Size = New System.Drawing.Size(48, 17)
        Me.rbtn_and.TabIndex = 0
        Me.rbtn_and.TabStop = True
        Me.rbtn_and.Text = "AND"
        Me.rbtn_and.UseVisualStyleBackColor = True
        '
        'btn_RemoveSkipLogic
        '
        Me.btn_RemoveSkipLogic.Location = New System.Drawing.Point(250, 54)
        Me.btn_RemoveSkipLogic.Name = "btn_RemoveSkipLogic"
        Me.btn_RemoveSkipLogic.Size = New System.Drawing.Size(75, 30)
        Me.btn_RemoveSkipLogic.TabIndex = 12
        Me.btn_RemoveSkipLogic.Text = "<<REMOVE"
        Me.btn_RemoveSkipLogic.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(3, 40)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(100, 13)
        Me.Label9.TabIndex = 20
        Me.Label9.Text = "Boolean Expression"
        '
        'gpb_skipCriteria
        '
        Me.gpb_skipCriteria.Controls.Add(Me.rbtn_required)
        Me.gpb_skipCriteria.Controls.Add(Me.rbtn_if)
        Me.gpb_skipCriteria.Enabled = False
        Me.gpb_skipCriteria.Location = New System.Drawing.Point(9, 14)
        Me.gpb_skipCriteria.Name = "gpb_skipCriteria"
        Me.gpb_skipCriteria.Size = New System.Drawing.Size(273, 39)
        Me.gpb_skipCriteria.TabIndex = 22
        Me.gpb_skipCriteria.TabStop = False
        Me.gpb_skipCriteria.Text = "Skip Criteria (Applicable if..)"
        '
        'rbtn_required
        '
        Me.rbtn_required.AutoSize = True
        Me.rbtn_required.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbtn_required.ForeColor = System.Drawing.Color.DarkOrange
        Me.rbtn_required.Location = New System.Drawing.Point(28, 16)
        Me.rbtn_required.Name = "rbtn_required"
        Me.rbtn_required.Size = New System.Drawing.Size(105, 20)
        Me.rbtn_required.TabIndex = 17
        Me.rbtn_required.TabStop = True
        Me.rbtn_required.Text = "REQUIRED"
        Me.rbtn_required.UseVisualStyleBackColor = True
        '
        'rbtn_if
        '
        Me.rbtn_if.AutoSize = True
        Me.rbtn_if.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbtn_if.ForeColor = System.Drawing.Color.DarkOrange
        Me.rbtn_if.Location = New System.Drawing.Point(139, 16)
        Me.rbtn_if.Name = "rbtn_if"
        Me.rbtn_if.Size = New System.Drawing.Size(39, 20)
        Me.rbtn_if.TabIndex = 18
        Me.rbtn_if.TabStop = True
        Me.rbtn_if.Text = "IF"
        Me.rbtn_if.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(12, 53)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(76, 13)
        Me.Label8.TabIndex = 15
        Me.Label8.Text = "Column Name:"
        '
        'lstbox_SkipAdded
        '
        Me.lstbox_SkipAdded.FormattingEnabled = True
        Me.lstbox_SkipAdded.Location = New System.Drawing.Point(346, 13)
        Me.lstbox_SkipAdded.Name = "lstbox_SkipAdded"
        Me.lstbox_SkipAdded.Size = New System.Drawing.Size(147, 121)
        Me.lstbox_SkipAdded.TabIndex = 15
        '
        'grpBoxDataGrid
        '
        Me.grpBoxDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpBoxDataGrid.Controls.Add(Me.dgvValidations)
        Me.grpBoxDataGrid.Location = New System.Drawing.Point(297, 13)
        Me.grpBoxDataGrid.Name = "grpBoxDataGrid"
        Me.grpBoxDataGrid.Size = New System.Drawing.Size(863, 315)
        Me.grpBoxDataGrid.TabIndex = 18
        Me.grpBoxDataGrid.TabStop = False
        Me.grpBoxDataGrid.Text = "Column Validations"
        '
        'dgvValidations
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvValidations.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.dgvValidations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvValidations.DefaultCellStyle = DataGridViewCellStyle5
        Me.dgvValidations.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvValidations.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgvValidations.Location = New System.Drawing.Point(3, 16)
        Me.dgvValidations.MultiSelect = False
        Me.dgvValidations.Name = "dgvValidations"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvValidations.RowHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.dgvValidations.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvValidations.Size = New System.Drawing.Size(857, 296)
        Me.dgvValidations.TabIndex = 0
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(11, 326)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(57, 13)
        Me.Label11.TabIndex = 17
        Me.Label11.Text = "Data Type"
        '
        'btnconnect
        '
        Me.btnconnect.Location = New System.Drawing.Point(213, 19)
        Me.btnconnect.Name = "btnconnect"
        Me.btnconnect.Size = New System.Drawing.Size(60, 23)
        Me.btnconnect.TabIndex = 14
        Me.btnconnect.Text = "Connect"
        Me.btnconnect.UseVisualStyleBackColor = True
        Me.btnconnect.Visible = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(3, 310)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(87, 13)
        Me.Label10.TabIndex = 16
        Me.Label10.Text = "Selected Column"
        '
        'lblselectedColumn
        '
        Me.lblselectedColumn.AutoSize = True
        Me.lblselectedColumn.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblselectedColumn.ForeColor = System.Drawing.Color.DarkGreen
        Me.lblselectedColumn.Location = New System.Drawing.Point(97, 306)
        Me.lblselectedColumn.Name = "lblselectedColumn"
        Me.lblselectedColumn.Size = New System.Drawing.Size(66, 17)
        Me.lblselectedColumn.TabIndex = 15
        Me.lblselectedColumn.Text = "Label10"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OptionsToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1174, 24)
        Me.MenuStrip1.TabIndex = 14
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'OptionsToolStripMenuItem
        '
        Me.OptionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ValidationsToolStripMenuItem})
        Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
        Me.OptionsToolStripMenuItem.Size = New System.Drawing.Size(56, 20)
        Me.OptionsToolStripMenuItem.Text = "Options"
        '
        'ValidationsToolStripMenuItem
        '
        Me.ValidationsToolStripMenuItem.Name = "ValidationsToolStripMenuItem"
        Me.ValidationsToolStripMenuItem.Size = New System.Drawing.Size(136, 22)
        Me.ValidationsToolStripMenuItem.Text = "Validations"
        '
        'ContextMenuDefaultvalue
        '
        Me.ContextMenuDefaultvalue.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SetAsDefaultValueToolStripMenuItem})
        Me.ContextMenuDefaultvalue.Name = "ContextMenuDefaultvalue"
        Me.ContextMenuDefaultvalue.Size = New System.Drawing.Size(182, 26)
        '
        'SetAsDefaultValueToolStripMenuItem
        '
        Me.SetAsDefaultValueToolStripMenuItem.Name = "SetAsDefaultValueToolStripMenuItem"
        Me.SetAsDefaultValueToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.SetAsDefaultValueToolStripMenuItem.Text = "set as Default Value"
        '
        'ckb_insertDate
        '
        Me.ckb_insertDate.AutoSize = True
        Me.ckb_insertDate.Checked = True
        Me.ckb_insertDate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ckb_insertDate.Location = New System.Drawing.Point(49, 48)
        Me.ckb_insertDate.Name = "ckb_insertDate"
        Me.ckb_insertDate.Size = New System.Drawing.Size(75, 17)
        Me.ckb_insertDate.TabIndex = 17
        Me.ckb_insertDate.Text = "InsertDate"
        Me.ckb_insertDate.UseVisualStyleBackColor = True
        '
        'frmValidator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1174, 689)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmValidator"
        Me.Text = "The Validator"
        Me.TransparencyKey = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        Me.SplitContainer1.ResumeLayout(False)
        Me.grpBoxValidValues.ResumeLayout(False)
        Me.grpBoxValidValues.PerformLayout()
        Me.gpb_valueType.ResumeLayout(False)
        Me.gpb_valueType.PerformLayout()
        Me.pnl_daterange.ResumeLayout(False)
        Me.pnl_daterange.PerformLayout()
        Me.pnl_range.ResumeLayout(False)
        Me.pnl_range.PerformLayout()
        CType(Me.num_rangeFrom, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.num_rangeTo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl_singleValue.ResumeLayout(False)
        Me.pnl_singleValue.PerformLayout()
        Me.GrpBoxSkiplogic.ResumeLayout(False)
        Me.GrpBoxSkiplogic.PerformLayout()
        Me.pnl_skipValues.ResumeLayout(False)
        Me.pnl_skipValues.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.gpb_skipCriteria.ResumeLayout(False)
        Me.gpb_skipCriteria.PerformLayout()
        Me.grpBoxDataGrid.ResumeLayout(False)
        CType(Me.dgvValidations, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ContextMenuDefaultvalue.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents serversComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents findServersButton As System.Windows.Forms.Button
    Friend WithEvents databasesComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents tableNameListBox As System.Windows.Forms.ListBox
    Friend WithEvents TableDetailsListView As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents btnconnect As System.Windows.Forms.Button
    Friend WithEvents gpb_valueType As System.Windows.Forms.GroupBox
    Friend WithEvents rbtn_singleValue As System.Windows.Forms.RadioButton
    Friend WithEvents rbtn_range As System.Windows.Forms.RadioButton
    Friend WithEvents dtp_rangeTo As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtp_rangeFrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents num_rangeTo As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents num_rangeFrom As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lstbox_ValuesAllowed As System.Windows.Forms.ListBox
    Friend WithEvents btn_remove As System.Windows.Forms.Button
    Friend WithEvents btn_add As System.Windows.Forms.Button
    Friend WithEvents txt_singleValue As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents GrpBoxSkiplogic As System.Windows.Forms.GroupBox
    Friend WithEvents lstbox_SkipAdded As System.Windows.Forms.ListBox
    Friend WithEvents btn_RemoveSkipLogic As System.Windows.Forms.Button
    Friend WithEvents btn_addSkiplogic As System.Windows.Forms.Button
    Friend WithEvents rbtn_if As System.Windows.Forms.RadioButton
    Friend WithEvents rbtn_required As System.Windows.Forms.RadioButton
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents rbtn_or As System.Windows.Forms.RadioButton
    Friend WithEvents rbtn_and As System.Windows.Forms.RadioButton
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cmbColumnName As System.Windows.Forms.ComboBox
    Friend WithEvents gpb_skipCriteria As System.Windows.Forms.GroupBox
    Friend WithEvents ckbCurrentDate As System.Windows.Forms.CheckBox
    Friend WithEvents lblselectedColumn As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents pnl_range As System.Windows.Forms.Panel
    Friend WithEvents pnl_singleValue As System.Windows.Forms.Panel
    Friend WithEvents dtp_singleValue As System.Windows.Forms.DateTimePicker
    Friend WithEvents pnl_skipValues As System.Windows.Forms.Panel
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txt_errorDesc As System.Windows.Forms.TextBox
    Friend WithEvents btn_save As System.Windows.Forms.Button
    Friend WithEvents pnl_daterange As System.Windows.Forms.Panel
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents OptionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ValidationsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txt_errordescSkipLogic As System.Windows.Forms.TextBox
    Friend WithEvents btn_removeClose As System.Windows.Forms.Button
    Friend WithEvents btn_insertclose As System.Windows.Forms.Button
    Friend WithEvents btn_removeopen As System.Windows.Forms.Button
    Friend WithEvents btn_insertopen As System.Windows.Forms.Button
    Friend WithEvents lblDefaultvalue As System.Windows.Forms.Label
    Friend WithEvents ContextMenuDefaultvalue As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SetAsDefaultValueToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents rbtn_anyValue As System.Windows.Forms.RadioButton
    Friend WithEvents cmb_skipValue As System.Windows.Forms.ComboBox
    Friend WithEvents grpBoxValidValues As System.Windows.Forms.GroupBox
    Friend WithEvents grpBoxDataGrid As System.Windows.Forms.GroupBox
    Friend WithEvents dgvValidations As System.Windows.Forms.DataGridView
    Friend WithEvents txt_defaultValue As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents cmb_valueFunctions As System.Windows.Forms.ComboBox
    Friend WithEvents cmb_booleanExp As System.Windows.Forms.ComboBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rbtn_column As System.Windows.Forms.RadioButton
    Friend WithEvents rbtn_value As System.Windows.Forms.RadioButton
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents cmb_skipFunctions As System.Windows.Forms.ComboBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents cmbValidationStatus As System.Windows.Forms.ComboBox
    Friend WithEvents ckb_insertDate As System.Windows.Forms.CheckBox


End Class
