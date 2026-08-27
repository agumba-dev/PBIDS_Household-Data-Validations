<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDBEditor
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDBEditor))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.serversComboBox = New System.Windows.Forms.ComboBox
        Me.findServersButton = New System.Windows.Forms.Button
        Me.databasesComboBox = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.cbx_upload = New System.Windows.Forms.CheckBox
        Me.lbl_datasetState = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.ckb_editmode = New System.Windows.Forms.CheckBox
        Me.cmb_chooseTable = New System.Windows.Forms.ComboBox
        Me.GroupBoxSearchFields = New System.Windows.Forms.GroupBox
        Me.PanelFilterCriteria = New System.Windows.Forms.Panel
        Me.Label36 = New System.Windows.Forms.Label
        Me.rbOR = New System.Windows.Forms.RadioButton
        Me.rbAnd = New System.Windows.Forms.RadioButton
        Me.TbFieldDesc2 = New System.Windows.Forms.TextBox
        Me.TbFieldDesc1 = New System.Windows.Forms.TextBox
        Me.CBFiledDesc1 = New System.Windows.Forms.ComboBox
        Me.CBFiledDesc2 = New System.Windows.Forms.ComboBox
        Me.Label41 = New System.Windows.Forms.Label
        Me.Label42 = New System.Windows.Forms.Label
        Me.btnQuerry = New System.Windows.Forms.Button
        Me.btnconnect = New System.Windows.Forms.Button
        Me.bn_mainDB = New System.Windows.Forms.BindingNavigator(Me.components)
        Me.BindingNavigatorCountItem = New System.Windows.Forms.ToolStripLabel
        Me.BindingNavigatorDeleteItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorMoveFirstItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorMovePreviousItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorSeparator = New System.Windows.Forms.ToolStripSeparator
        Me.BindingNavigatorPositionItem = New System.Windows.Forms.ToolStripTextBox
        Me.BindingNavigatorSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.BindingNavigatorMoveNextItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorMoveLastItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.NewToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.OpenToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.SaveToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.PrintToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.toolStripSeparator = New System.Windows.Forms.ToolStripSeparator
        Me.CutToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.CopyToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.PasteToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.toolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.HelpToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.tsp_validate = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.tsp_Cancelchanges = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.grpBoxDataGrid = New System.Windows.Forms.GroupBox
        Me.dgv_records = New System.Windows.Forms.DataGridView
        Me.cms_EditorsMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator
        Me.tsmn_ValidateRecord = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator
        Me.tsmn_CancelChange = New System.Windows.Forms.ToolStripMenuItem
        Me.tsp_RetrieveRecords = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.BindingSource1 = New System.Windows.Forms.BindingSource(Me.components)
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.GroupBoxSearchFields.SuspendLayout()
        Me.PanelFilterCriteria.SuspendLayout()
        CType(Me.bn_mainDB, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.bn_mainDB.SuspendLayout()
        Me.grpBoxDataGrid.SuspendLayout()
        CType(Me.dgv_records, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cms_EditorsMenu.SuspendLayout()
        CType(Me.BindingSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'serversComboBox
        '
        Me.serversComboBox.FormattingEnabled = True
        Me.serversComboBox.Items.AddRange(New Object() {"KISW-DSS-09948"})
        Me.serversComboBox.Location = New System.Drawing.Point(164, 15)
        Me.serversComboBox.Name = "serversComboBox"
        Me.serversComboBox.Size = New System.Drawing.Size(148, 21)
        Me.serversComboBox.TabIndex = 0
        '
        'findServersButton
        '
        Me.findServersButton.Location = New System.Drawing.Point(384, 15)
        Me.findServersButton.Name = "findServersButton"
        Me.findServersButton.Size = New System.Drawing.Size(75, 23)
        Me.findServersButton.TabIndex = 1
        Me.findServersButton.Text = "FindServers"
        Me.findServersButton.UseVisualStyleBackColor = True
        '
        'databasesComboBox
        '
        Me.databasesComboBox.FormattingEnabled = True
        Me.databasesComboBox.Location = New System.Drawing.Point(164, 48)
        Me.databasesComboBox.Name = "databasesComboBox"
        Me.databasesComboBox.Size = New System.Drawing.Size(214, 21)
        Me.databasesComboBox.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Enabled = False
        Me.Label1.Location = New System.Drawing.Point(73, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 13)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Choose server"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(53, 51)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(89, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "choose database"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(20, 85)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(138, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "choose table to view details"
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
        Me.SplitContainer1.Panel1.Controls.Add(Me.cbx_upload)
        Me.SplitContainer1.Panel1.Controls.Add(Me.lbl_datasetState)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label4)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ckb_editmode)
        Me.SplitContainer1.Panel1.Controls.Add(Me.cmb_chooseTable)
        Me.SplitContainer1.Panel1.Controls.Add(Me.GroupBoxSearchFields)
        Me.SplitContainer1.Panel1.Controls.Add(Me.btnQuerry)
        Me.SplitContainer1.Panel1.Controls.Add(Me.serversComboBox)
        Me.SplitContainer1.Panel1.Controls.Add(Me.btnconnect)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.databasesComboBox)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label3)
        Me.SplitContainer1.Panel1.Controls.Add(Me.findServersButton)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.AutoScroll = True
        Me.SplitContainer1.Panel2.Controls.Add(Me.bn_mainDB)
        Me.SplitContainer1.Panel2.Controls.Add(Me.grpBoxDataGrid)
        Me.SplitContainer1.Size = New System.Drawing.Size(1028, 709)
        Me.SplitContainer1.SplitterDistance = 221
        Me.SplitContainer1.TabIndex = 12
        '
        'cbx_upload
        '
        Me.cbx_upload.AutoSize = True
        Me.cbx_upload.Enabled = False
        Me.cbx_upload.Location = New System.Drawing.Point(164, 138)
        Me.cbx_upload.Name = "cbx_upload"
        Me.cbx_upload.Size = New System.Drawing.Size(238, 17)
        Me.cbx_upload.TabIndex = 26
        Me.cbx_upload.Text = "IF TEMP DB UPLOAD AFTER VALIDATION"
        Me.cbx_upload.UseVisualStyleBackColor = True
        '
        'lbl_datasetState
        '
        Me.lbl_datasetState.AutoSize = True
        Me.lbl_datasetState.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_datasetState.ForeColor = System.Drawing.Color.Red
        Me.lbl_datasetState.Location = New System.Drawing.Point(164, 168)
        Me.lbl_datasetState.Name = "lbl_datasetState"
        Me.lbl_datasetState.Size = New System.Drawing.Size(57, 13)
        Me.lbl_datasetState.TabIndex = 25
        Me.lbl_datasetState.Text = "Read Only"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(83, 168)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(75, 13)
        Me.Label4.TabIndex = 24
        Me.Label4.Text = "DataSet state:"
        '
        'ckb_editmode
        '
        Me.ckb_editmode.AutoSize = True
        Me.ckb_editmode.Enabled = False
        Me.ckb_editmode.Location = New System.Drawing.Point(164, 115)
        Me.ckb_editmode.Name = "ckb_editmode"
        Me.ckb_editmode.Size = New System.Drawing.Size(74, 17)
        Me.ckb_editmode.TabIndex = 23
        Me.ckb_editmode.Text = "Edit Mode"
        Me.ckb_editmode.UseVisualStyleBackColor = True
        '
        'cmb_chooseTable
        '
        Me.cmb_chooseTable.FormattingEnabled = True
        Me.cmb_chooseTable.Location = New System.Drawing.Point(164, 79)
        Me.cmb_chooseTable.Name = "cmb_chooseTable"
        Me.cmb_chooseTable.Size = New System.Drawing.Size(214, 21)
        Me.cmb_chooseTable.TabIndex = 22
        '
        'GroupBoxSearchFields
        '
        Me.GroupBoxSearchFields.Controls.Add(Me.PanelFilterCriteria)
        Me.GroupBoxSearchFields.Controls.Add(Me.Label41)
        Me.GroupBoxSearchFields.Controls.Add(Me.Label42)
        Me.GroupBoxSearchFields.Location = New System.Drawing.Point(518, 15)
        Me.GroupBoxSearchFields.Name = "GroupBoxSearchFields"
        Me.GroupBoxSearchFields.Size = New System.Drawing.Size(318, 157)
        Me.GroupBoxSearchFields.TabIndex = 21
        Me.GroupBoxSearchFields.TabStop = False
        Me.GroupBoxSearchFields.Text = "Search Fields"
        '
        'PanelFilterCriteria
        '
        Me.PanelFilterCriteria.Controls.Add(Me.Label36)
        Me.PanelFilterCriteria.Controls.Add(Me.rbOR)
        Me.PanelFilterCriteria.Controls.Add(Me.rbAnd)
        Me.PanelFilterCriteria.Controls.Add(Me.TbFieldDesc2)
        Me.PanelFilterCriteria.Controls.Add(Me.TbFieldDesc1)
        Me.PanelFilterCriteria.Controls.Add(Me.CBFiledDesc1)
        Me.PanelFilterCriteria.Controls.Add(Me.CBFiledDesc2)
        Me.PanelFilterCriteria.Location = New System.Drawing.Point(6, 35)
        Me.PanelFilterCriteria.Name = "PanelFilterCriteria"
        Me.PanelFilterCriteria.Size = New System.Drawing.Size(306, 103)
        Me.PanelFilterCriteria.TabIndex = 9
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.Location = New System.Drawing.Point(13, 37)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(61, 13)
        Me.Label36.TabIndex = 16
        Me.Label36.Text = "FilterCriteria"
        '
        'rbOR
        '
        Me.rbOR.AutoSize = True
        Me.rbOR.Location = New System.Drawing.Point(138, 35)
        Me.rbOR.Name = "rbOR"
        Me.rbOR.Size = New System.Drawing.Size(41, 17)
        Me.rbOR.TabIndex = 15
        Me.rbOR.Text = "OR"
        Me.rbOR.UseVisualStyleBackColor = True
        '
        'rbAnd
        '
        Me.rbAnd.AutoSize = True
        Me.rbAnd.Checked = True
        Me.rbAnd.Location = New System.Drawing.Point(76, 35)
        Me.rbAnd.Name = "rbAnd"
        Me.rbAnd.Size = New System.Drawing.Size(48, 17)
        Me.rbAnd.TabIndex = 14
        Me.rbAnd.TabStop = True
        Me.rbAnd.Text = "AND"
        Me.rbAnd.UseVisualStyleBackColor = True
        '
        'TbFieldDesc2
        '
        Me.TbFieldDesc2.Location = New System.Drawing.Point(164, 66)
        Me.TbFieldDesc2.Name = "TbFieldDesc2"
        Me.TbFieldDesc2.Size = New System.Drawing.Size(136, 20)
        Me.TbFieldDesc2.TabIndex = 13
        '
        'TbFieldDesc1
        '
        Me.TbFieldDesc1.Location = New System.Drawing.Point(164, 9)
        Me.TbFieldDesc1.Name = "TbFieldDesc1"
        Me.TbFieldDesc1.Size = New System.Drawing.Size(136, 20)
        Me.TbFieldDesc1.TabIndex = 12
        '
        'CBFiledDesc1
        '
        Me.CBFiledDesc1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CBFiledDesc1.FormattingEnabled = True
        Me.CBFiledDesc1.Location = New System.Drawing.Point(6, 7)
        Me.CBFiledDesc1.Name = "CBFiledDesc1"
        Me.CBFiledDesc1.Size = New System.Drawing.Size(142, 21)
        Me.CBFiledDesc1.TabIndex = 10
        '
        'CBFiledDesc2
        '
        Me.CBFiledDesc2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CBFiledDesc2.FormattingEnabled = True
        Me.CBFiledDesc2.Location = New System.Drawing.Point(6, 65)
        Me.CBFiledDesc2.Name = "CBFiledDesc2"
        Me.CBFiledDesc2.Size = New System.Drawing.Size(142, 21)
        Me.CBFiledDesc2.TabIndex = 11
        '
        'Label41
        '
        Me.Label41.BackColor = System.Drawing.Color.Gainsboro
        Me.Label41.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label41.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label41.Location = New System.Drawing.Point(201, 16)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(100, 16)
        Me.Label41.TabIndex = 8
        Me.Label41.Text = "FIELD VALUE"
        '
        'Label42
        '
        Me.Label42.BackColor = System.Drawing.Color.Gainsboro
        Me.Label42.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label42.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label42.Location = New System.Drawing.Point(6, 16)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(134, 16)
        Me.Label42.TabIndex = 7
        Me.Label42.Text = "FIELD DESCRIPTION"
        '
        'btnQuerry
        '
        Me.btnQuerry.Location = New System.Drawing.Point(706, 175)
        Me.btnQuerry.Name = "btnQuerry"
        Me.btnQuerry.Size = New System.Drawing.Size(128, 26)
        Me.btnQuerry.TabIndex = 20
        Me.btnQuerry.Text = "RETRIVE RECORDS"
        Me.btnQuerry.UseVisualStyleBackColor = True
        '
        'btnconnect
        '
        Me.btnconnect.Location = New System.Drawing.Point(318, 15)
        Me.btnconnect.Name = "btnconnect"
        Me.btnconnect.Size = New System.Drawing.Size(60, 23)
        Me.btnconnect.TabIndex = 14
        Me.btnconnect.Text = "Connect"
        Me.btnconnect.UseVisualStyleBackColor = True
        '
        'bn_mainDB
        '
        Me.bn_mainDB.AddNewItem = Nothing
        Me.bn_mainDB.CountItem = Me.BindingNavigatorCountItem
        Me.bn_mainDB.DeleteItem = Me.BindingNavigatorDeleteItem
        Me.bn_mainDB.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.bn_mainDB.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BindingNavigatorMoveFirstItem, Me.BindingNavigatorMovePreviousItem, Me.BindingNavigatorSeparator, Me.BindingNavigatorPositionItem, Me.BindingNavigatorCountItem, Me.BindingNavigatorSeparator1, Me.BindingNavigatorMoveNextItem, Me.BindingNavigatorMoveLastItem, Me.BindingNavigatorSeparator2, Me.BindingNavigatorDeleteItem, Me.NewToolStripButton, Me.OpenToolStripButton, Me.SaveToolStripButton, Me.PrintToolStripButton, Me.toolStripSeparator, Me.CutToolStripButton, Me.CopyToolStripButton, Me.PasteToolStripButton, Me.toolStripSeparator1, Me.HelpToolStripButton, Me.ToolStripSeparator2, Me.tsp_validate, Me.ToolStripSeparator3, Me.tsp_Cancelchanges, Me.ToolStripSeparator4})
        Me.bn_mainDB.Location = New System.Drawing.Point(0, 455)
        Me.bn_mainDB.MoveFirstItem = Me.BindingNavigatorMoveFirstItem
        Me.bn_mainDB.MoveLastItem = Me.BindingNavigatorMoveLastItem
        Me.bn_mainDB.MoveNextItem = Me.BindingNavigatorMoveNextItem
        Me.bn_mainDB.MovePreviousItem = Me.BindingNavigatorMovePreviousItem
        Me.bn_mainDB.Name = "bn_mainDB"
        Me.bn_mainDB.PositionItem = Me.BindingNavigatorPositionItem
        Me.bn_mainDB.Size = New System.Drawing.Size(1024, 25)
        Me.bn_mainDB.TabIndex = 19
        Me.bn_mainDB.Text = "BindingNavigator1"
        '
        'BindingNavigatorCountItem
        '
        Me.BindingNavigatorCountItem.Name = "BindingNavigatorCountItem"
        Me.BindingNavigatorCountItem.Size = New System.Drawing.Size(36, 22)
        Me.BindingNavigatorCountItem.Text = "of {0}"
        Me.BindingNavigatorCountItem.ToolTipText = "Total number of items"
        '
        'BindingNavigatorDeleteItem
        '
        Me.BindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorDeleteItem.Enabled = False
        Me.BindingNavigatorDeleteItem.Image = CType(resources.GetObject("BindingNavigatorDeleteItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorDeleteItem.Name = "BindingNavigatorDeleteItem"
        Me.BindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorDeleteItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorDeleteItem.Text = "Delete"
        '
        'BindingNavigatorMoveFirstItem
        '
        Me.BindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveFirstItem.Image = CType(resources.GetObject("BindingNavigatorMoveFirstItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveFirstItem.Name = "BindingNavigatorMoveFirstItem"
        Me.BindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveFirstItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMoveFirstItem.Text = "Move first"
        '
        'BindingNavigatorMovePreviousItem
        '
        Me.BindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMovePreviousItem.Image = CType(resources.GetObject("BindingNavigatorMovePreviousItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMovePreviousItem.Name = "BindingNavigatorMovePreviousItem"
        Me.BindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMovePreviousItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMovePreviousItem.Text = "Move previous"
        '
        'BindingNavigatorSeparator
        '
        Me.BindingNavigatorSeparator.Name = "BindingNavigatorSeparator"
        Me.BindingNavigatorSeparator.Size = New System.Drawing.Size(6, 25)
        '
        'BindingNavigatorPositionItem
        '
        Me.BindingNavigatorPositionItem.AccessibleName = "Position"
        Me.BindingNavigatorPositionItem.AutoSize = False
        Me.BindingNavigatorPositionItem.Name = "BindingNavigatorPositionItem"
        Me.BindingNavigatorPositionItem.Size = New System.Drawing.Size(50, 21)
        Me.BindingNavigatorPositionItem.Text = "0"
        Me.BindingNavigatorPositionItem.ToolTipText = "Current position"
        '
        'BindingNavigatorSeparator1
        '
        Me.BindingNavigatorSeparator1.Name = "BindingNavigatorSeparator1"
        Me.BindingNavigatorSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'BindingNavigatorMoveNextItem
        '
        Me.BindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveNextItem.Image = CType(resources.GetObject("BindingNavigatorMoveNextItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveNextItem.Name = "BindingNavigatorMoveNextItem"
        Me.BindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveNextItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMoveNextItem.Text = "Move next"
        '
        'BindingNavigatorMoveLastItem
        '
        Me.BindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveLastItem.Image = CType(resources.GetObject("BindingNavigatorMoveLastItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveLastItem.Name = "BindingNavigatorMoveLastItem"
        Me.BindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveLastItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMoveLastItem.Text = "Move last"
        '
        'BindingNavigatorSeparator2
        '
        Me.BindingNavigatorSeparator2.Name = "BindingNavigatorSeparator2"
        Me.BindingNavigatorSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'NewToolStripButton
        '
        Me.NewToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.NewToolStripButton.Image = CType(resources.GetObject("NewToolStripButton.Image"), System.Drawing.Image)
        Me.NewToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.NewToolStripButton.Name = "NewToolStripButton"
        Me.NewToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.NewToolStripButton.Text = "&New"
        '
        'OpenToolStripButton
        '
        Me.OpenToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.OpenToolStripButton.Image = CType(resources.GetObject("OpenToolStripButton.Image"), System.Drawing.Image)
        Me.OpenToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.OpenToolStripButton.Name = "OpenToolStripButton"
        Me.OpenToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.OpenToolStripButton.Text = "&Open"
        '
        'SaveToolStripButton
        '
        Me.SaveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.SaveToolStripButton.Enabled = False
        Me.SaveToolStripButton.Image = CType(resources.GetObject("SaveToolStripButton.Image"), System.Drawing.Image)
        Me.SaveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.SaveToolStripButton.Name = "SaveToolStripButton"
        Me.SaveToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.SaveToolStripButton.Text = "&Save"
        '
        'PrintToolStripButton
        '
        Me.PrintToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.PrintToolStripButton.Image = CType(resources.GetObject("PrintToolStripButton.Image"), System.Drawing.Image)
        Me.PrintToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.PrintToolStripButton.Name = "PrintToolStripButton"
        Me.PrintToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.PrintToolStripButton.Text = "&Print"
        '
        'toolStripSeparator
        '
        Me.toolStripSeparator.Name = "toolStripSeparator"
        Me.toolStripSeparator.Size = New System.Drawing.Size(6, 25)
        '
        'CutToolStripButton
        '
        Me.CutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.CutToolStripButton.Image = CType(resources.GetObject("CutToolStripButton.Image"), System.Drawing.Image)
        Me.CutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CutToolStripButton.Name = "CutToolStripButton"
        Me.CutToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.CutToolStripButton.Text = "C&ut"
        '
        'CopyToolStripButton
        '
        Me.CopyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.CopyToolStripButton.Image = CType(resources.GetObject("CopyToolStripButton.Image"), System.Drawing.Image)
        Me.CopyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.CopyToolStripButton.Name = "CopyToolStripButton"
        Me.CopyToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.CopyToolStripButton.Text = "&Copy"
        '
        'PasteToolStripButton
        '
        Me.PasteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.PasteToolStripButton.Image = CType(resources.GetObject("PasteToolStripButton.Image"), System.Drawing.Image)
        Me.PasteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.PasteToolStripButton.Name = "PasteToolStripButton"
        Me.PasteToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.PasteToolStripButton.Text = "&Paste"
        '
        'toolStripSeparator1
        '
        Me.toolStripSeparator1.Name = "toolStripSeparator1"
        Me.toolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'HelpToolStripButton
        '
        Me.HelpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.HelpToolStripButton.Image = CType(resources.GetObject("HelpToolStripButton.Image"), System.Drawing.Image)
        Me.HelpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.HelpToolStripButton.Name = "HelpToolStripButton"
        Me.HelpToolStripButton.Size = New System.Drawing.Size(23, 22)
        Me.HelpToolStripButton.Text = "He&lp"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'tsp_validate
        '
        Me.tsp_validate.Image = CType(resources.GetObject("tsp_validate.Image"), System.Drawing.Image)
        Me.tsp_validate.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsp_validate.Name = "tsp_validate"
        Me.tsp_validate.Size = New System.Drawing.Size(139, 22)
        Me.tsp_validate.Text = "Validate Current record"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 25)
        '
        'tsp_Cancelchanges
        '
        Me.tsp_Cancelchanges.Image = CType(resources.GetObject("tsp_Cancelchanges.Image"), System.Drawing.Image)
        Me.tsp_Cancelchanges.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsp_Cancelchanges.Name = "tsp_Cancelchanges"
        Me.tsp_Cancelchanges.Size = New System.Drawing.Size(102, 22)
        Me.tsp_Cancelchanges.Text = "Cancel changes"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 25)
        '
        'grpBoxDataGrid
        '
        Me.grpBoxDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpBoxDataGrid.Controls.Add(Me.dgv_records)
        Me.grpBoxDataGrid.Location = New System.Drawing.Point(3, 13)
        Me.grpBoxDataGrid.Name = "grpBoxDataGrid"
        Me.grpBoxDataGrid.Size = New System.Drawing.Size(1011, 439)
        Me.grpBoxDataGrid.TabIndex = 18
        Me.grpBoxDataGrid.TabStop = False
        Me.grpBoxDataGrid.Text = "Table Data"
        '
        'dgv_records
        '
        Me.dgv_records.AllowUserToAddRows = False
        Me.dgv_records.AllowUserToDeleteRows = False
        Me.dgv_records.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.dgv_records.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_records.ContextMenuStrip = Me.cms_EditorsMenu
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Info
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgv_records.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgv_records.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgv_records.Location = New System.Drawing.Point(3, 16)
        Me.dgv_records.MultiSelect = False
        Me.dgv_records.Name = "dgv_records"
        Me.dgv_records.Size = New System.Drawing.Size(1005, 420)
        Me.dgv_records.TabIndex = 0
        '
        'cms_EditorsMenu
        '
        Me.cms_EditorsMenu.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.cms_EditorsMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripSeparator5, Me.tsmn_ValidateRecord, Me.ToolStripSeparator6, Me.tsmn_CancelChange, Me.tsp_RetrieveRecords})
        Me.cms_EditorsMenu.Name = "cms_EditorsMenu"
        Me.cms_EditorsMenu.Size = New System.Drawing.Size(187, 82)
        Me.cms_EditorsMenu.Text = "Editor' Menu"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(183, 6)
        '
        'tsmn_ValidateRecord
        '
        Me.tsmn_ValidateRecord.Name = "tsmn_ValidateRecord"
        Me.tsmn_ValidateRecord.Size = New System.Drawing.Size(186, 22)
        Me.tsmn_ValidateRecord.Text = "Validate Current record"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(183, 6)
        '
        'tsmn_CancelChange
        '
        Me.tsmn_CancelChange.Name = "tsmn_CancelChange"
        Me.tsmn_CancelChange.Size = New System.Drawing.Size(186, 22)
        Me.tsmn_CancelChange.Text = "Cancel Changes"
        '
        'tsp_RetrieveRecords
        '
        Me.tsp_RetrieveRecords.Name = "tsp_RetrieveRecords"
        Me.tsp_RetrieveRecords.Size = New System.Drawing.Size(186, 22)
        Me.tsp_RetrieveRecords.Text = "Retrieve Records"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1028, 24)
        Me.MenuStrip1.TabIndex = 14
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'frmDBEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 733)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmDBEditor"
        Me.ShowIcon = False
        Me.Text = "Database Editor"
        Me.TransparencyKey = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        Me.SplitContainer1.ResumeLayout(False)
        Me.GroupBoxSearchFields.ResumeLayout(False)
        Me.PanelFilterCriteria.ResumeLayout(False)
        Me.PanelFilterCriteria.PerformLayout()
        CType(Me.bn_mainDB, System.ComponentModel.ISupportInitialize).EndInit()
        Me.bn_mainDB.ResumeLayout(False)
        Me.bn_mainDB.PerformLayout()
        Me.grpBoxDataGrid.ResumeLayout(False)
        CType(Me.dgv_records, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cms_EditorsMenu.ResumeLayout(False)
        CType(Me.BindingSource1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents serversComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents findServersButton As System.Windows.Forms.Button
    Friend WithEvents databasesComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents btnconnect As System.Windows.Forms.Button
    Friend WithEvents grpBoxDataGrid As System.Windows.Forms.GroupBox
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents GroupBoxSearchFields As System.Windows.Forms.GroupBox
    Friend WithEvents PanelFilterCriteria As System.Windows.Forms.Panel
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents rbOR As System.Windows.Forms.RadioButton
    Friend WithEvents rbAnd As System.Windows.Forms.RadioButton
    Friend WithEvents TbFieldDesc2 As System.Windows.Forms.TextBox
    Friend WithEvents TbFieldDesc1 As System.Windows.Forms.TextBox
    Friend WithEvents CBFiledDesc1 As System.Windows.Forms.ComboBox
    Friend WithEvents CBFiledDesc2 As System.Windows.Forms.ComboBox
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents btnQuerry As System.Windows.Forms.Button
    Friend WithEvents bn_mainDB As System.Windows.Forms.BindingNavigator
    Friend WithEvents BindingNavigatorCountItem As System.Windows.Forms.ToolStripLabel
    Friend WithEvents BindingNavigatorDeleteItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorMoveFirstItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorMovePreviousItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BindingNavigatorPositionItem As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents BindingNavigatorSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BindingNavigatorMoveNextItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorMoveLastItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents dgv_records As System.Windows.Forms.DataGridView
    Friend WithEvents cmb_chooseTable As System.Windows.Forms.ComboBox
    Friend WithEvents NewToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents OpenToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents SaveToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents PrintToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents toolStripSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents CutToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents CopyToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents PasteToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents toolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents HelpToolStripButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingSource1 As System.Windows.Forms.BindingSource
    Friend WithEvents ckb_editmode As System.Windows.Forms.CheckBox
    Friend WithEvents lbl_datasetState As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsp_validate As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsp_Cancelchanges As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cbx_upload As System.Windows.Forms.CheckBox
    Friend WithEvents cms_EditorsMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsmn_ValidateRecord As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsmn_CancelChange As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsp_RetrieveRecords As System.Windows.Forms.ToolStripMenuItem


End Class
