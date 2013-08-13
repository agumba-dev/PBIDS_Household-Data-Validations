<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_ToValidateForm
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frm_ToValidateForm))
        Me.gpbox_results = New System.Windows.Forms.GroupBox()
        Me.dgr_results = New System.Windows.Forms.DataGridView()
        Me.bndNvgerrors = New System.Windows.Forms.BindingNavigator(Me.components)
        Me.BindingNavigatorCountItem = New System.Windows.Forms.ToolStripLabel()
        Me.BindingNavigatorMoveFirstItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorMovePreviousItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.BindingNavigatorPositionItem = New System.Windows.Forms.ToolStripTextBox()
        Me.BindingNavigatorSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.BindingNavigatorMoveNextItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorMoveLastItem = New System.Windows.Forms.ToolStripButton()
        Me.BindingNavigatorSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripButtonPrint = New System.Windows.Forms.ToolStripButton()
        Me.lstbox_tables = New System.Windows.Forms.ListBox()
        Me.cmb_selectOption = New System.Windows.Forms.ComboBox()
        Me.btn_Go = New System.Windows.Forms.Button()
        Me.grpbox_tables = New System.Windows.Forms.GroupBox()
        Me.ckb_upload = New System.Windows.Forms.CheckBox()
        Me.btn_deSelectAll = New System.Windows.Forms.Button()
        Me.btn_selectAll = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.grpbox_erroroptions = New System.Windows.Forms.GroupBox()
        Me.rbtn_perCompound = New System.Windows.Forms.RadioButton()
        Me.rbtn_pervillperTable = New System.Windows.Forms.RadioButton()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cmb_errorView = New System.Windows.Forms.ComboBox()
        Me.rbtn_perVillage = New System.Windows.Forms.RadioButton()
        Me.rbtn_errorCountperRecord = New System.Windows.Forms.RadioButton()
        Me.rbtn_errorCounterrtype = New System.Windows.Forms.RadioButton()
        Me.rbtn_errorCountPerTble = New System.Windows.Forms.RadioButton()
        Me.rbtn_allDetails = New System.Windows.Forms.RadioButton()
        Me.cms_ShowRecord = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ShowRecordsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetErrorStatusToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CleanedToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PendingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ProgramFalseAlarmToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CommentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.InsertQuerryRefToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.PBarlabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.PBar = New System.Windows.Forms.ToolStripProgressBar()
        Me.LabelProgress = New System.Windows.Forms.ToolStripStatusLabel()
        Me.BackgroundWorkerValidate = New System.ComponentModel.BackgroundWorker()
        Me.tb_log = New System.Windows.Forms.TextBox()
        Me.gpbox_results.SuspendLayout()
        CType(Me.dgr_results, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bndNvgerrors, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.bndNvgerrors.SuspendLayout()
        Me.grpbox_tables.SuspendLayout()
        Me.grpbox_erroroptions.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.cms_ShowRecord.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'gpbox_results
        '
        Me.gpbox_results.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gpbox_results.Controls.Add(Me.dgr_results)
        Me.gpbox_results.Controls.Add(Me.bndNvgerrors)
        Me.gpbox_results.Location = New System.Drawing.Point(14, 367)
        Me.gpbox_results.Name = "gpbox_results"
        Me.gpbox_results.Size = New System.Drawing.Size(1050, 295)
        Me.gpbox_results.TabIndex = 0
        Me.gpbox_results.TabStop = False
        Me.gpbox_results.Text = "ERRORS"
        '
        'dgr_results
        '
        Me.dgr_results.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.dgr_results.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgr_results.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgr_results.Location = New System.Drawing.Point(3, 17)
        Me.dgr_results.Name = "dgr_results"
        Me.dgr_results.Size = New System.Drawing.Size(1044, 250)
        Me.dgr_results.TabIndex = 0
        '
        'bndNvgerrors
        '
        Me.bndNvgerrors.AddNewItem = Nothing
        Me.bndNvgerrors.CountItem = Me.BindingNavigatorCountItem
        Me.bndNvgerrors.DeleteItem = Nothing
        Me.bndNvgerrors.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.bndNvgerrors.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BindingNavigatorMoveFirstItem, Me.BindingNavigatorMovePreviousItem, Me.BindingNavigatorSeparator, Me.BindingNavigatorPositionItem, Me.BindingNavigatorCountItem, Me.BindingNavigatorSeparator1, Me.BindingNavigatorMoveNextItem, Me.BindingNavigatorMoveLastItem, Me.BindingNavigatorSeparator2, Me.ToolStripButtonPrint})
        Me.bndNvgerrors.Location = New System.Drawing.Point(3, 267)
        Me.bndNvgerrors.MoveFirstItem = Me.BindingNavigatorMoveFirstItem
        Me.bndNvgerrors.MoveLastItem = Me.BindingNavigatorMoveLastItem
        Me.bndNvgerrors.MoveNextItem = Me.BindingNavigatorMoveNextItem
        Me.bndNvgerrors.MovePreviousItem = Me.BindingNavigatorMovePreviousItem
        Me.bndNvgerrors.Name = "bndNvgerrors"
        Me.bndNvgerrors.PositionItem = Me.BindingNavigatorPositionItem
        Me.bndNvgerrors.Size = New System.Drawing.Size(1044, 25)
        Me.bndNvgerrors.TabIndex = 1
        Me.bndNvgerrors.Text = "BindingNavigator1"
        '
        'BindingNavigatorCountItem
        '
        Me.BindingNavigatorCountItem.Name = "BindingNavigatorCountItem"
        Me.BindingNavigatorCountItem.Size = New System.Drawing.Size(36, 22)
        Me.BindingNavigatorCountItem.Text = "of {0}"
        Me.BindingNavigatorCountItem.ToolTipText = "Total number of items"
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
        Me.BindingNavigatorPositionItem.Size = New System.Drawing.Size(58, 21)
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
        'ToolStripButtonPrint
        '
        Me.ToolStripButtonPrint.Image = CType(resources.GetObject("ToolStripButtonPrint.Image"), System.Drawing.Image)
        Me.ToolStripButtonPrint.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButtonPrint.Name = "ToolStripButtonPrint"
        Me.ToolStripButtonPrint.Size = New System.Drawing.Size(49, 22)
        Me.ToolStripButtonPrint.Text = "Print"
        '
        'lstbox_tables
        '
        Me.lstbox_tables.FormattingEnabled = True
        Me.lstbox_tables.Location = New System.Drawing.Point(7, 19)
        Me.lstbox_tables.Name = "lstbox_tables"
        Me.lstbox_tables.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstbox_tables.Size = New System.Drawing.Size(403, 238)
        Me.lstbox_tables.TabIndex = 2
        '
        'cmb_selectOption
        '
        Me.cmb_selectOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmb_selectOption.FormattingEnabled = True
        Me.cmb_selectOption.Items.AddRange(New Object() {"Validate tempdb", "Validate maindb", "View Errors", "SyncErrorTab", "UpdateCompoundField", "remove duplicates", "undo all records marked for deletion"})
        Me.cmb_selectOption.Location = New System.Drawing.Point(6, 322)
        Me.cmb_selectOption.Name = "cmb_selectOption"
        Me.cmb_selectOption.Size = New System.Drawing.Size(230, 21)
        Me.cmb_selectOption.TabIndex = 3
        '
        'btn_Go
        '
        Me.btn_Go.Location = New System.Drawing.Point(272, 322)
        Me.btn_Go.Name = "btn_Go"
        Me.btn_Go.Size = New System.Drawing.Size(114, 21)
        Me.btn_Go.TabIndex = 4
        Me.btn_Go.Text = "GO!!"
        Me.btn_Go.UseVisualStyleBackColor = True
        '
        'grpbox_tables
        '
        Me.grpbox_tables.Controls.Add(Me.ckb_upload)
        Me.grpbox_tables.Controls.Add(Me.btn_deSelectAll)
        Me.grpbox_tables.Controls.Add(Me.btn_selectAll)
        Me.grpbox_tables.Controls.Add(Me.Label1)
        Me.grpbox_tables.Controls.Add(Me.lstbox_tables)
        Me.grpbox_tables.Controls.Add(Me.btn_Go)
        Me.grpbox_tables.Controls.Add(Me.cmb_selectOption)
        Me.grpbox_tables.Location = New System.Drawing.Point(14, 12)
        Me.grpbox_tables.Name = "grpbox_tables"
        Me.grpbox_tables.Size = New System.Drawing.Size(436, 349)
        Me.grpbox_tables.TabIndex = 5
        Me.grpbox_tables.TabStop = False
        Me.grpbox_tables.Text = "Database Tables"
        '
        'ckb_upload
        '
        Me.ckb_upload.AutoSize = True
        Me.ckb_upload.Location = New System.Drawing.Point(201, 268)
        Me.ckb_upload.Name = "ckb_upload"
        Me.ckb_upload.Size = New System.Drawing.Size(137, 17)
        Me.ckb_upload.TabIndex = 8
        Me.ckb_upload.Text = "Upload after validations"
        Me.ckb_upload.UseVisualStyleBackColor = True
        '
        'btn_deSelectAll
        '
        Me.btn_deSelectAll.Location = New System.Drawing.Point(99, 263)
        Me.btn_deSelectAll.Name = "btn_deSelectAll"
        Me.btn_deSelectAll.Size = New System.Drawing.Size(89, 23)
        Me.btn_deSelectAll.TabIndex = 7
        Me.btn_deSelectAll.Text = "Un Select All"
        Me.btn_deSelectAll.UseVisualStyleBackColor = True
        '
        'btn_selectAll
        '
        Me.btn_selectAll.Location = New System.Drawing.Point(9, 263)
        Me.btn_selectAll.Name = "btn_selectAll"
        Me.btn_selectAll.Size = New System.Drawing.Size(75, 23)
        Me.btn_selectAll.TabIndex = 6
        Me.btn_selectAll.Text = "Select All"
        Me.btn_selectAll.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 306)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(87, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Select Option "
        '
        'grpbox_erroroptions
        '
        Me.grpbox_erroroptions.Controls.Add(Me.rbtn_perCompound)
        Me.grpbox_erroroptions.Controls.Add(Me.rbtn_pervillperTable)
        Me.grpbox_erroroptions.Controls.Add(Me.GroupBox1)
        Me.grpbox_erroroptions.Controls.Add(Me.rbtn_perVillage)
        Me.grpbox_erroroptions.Controls.Add(Me.rbtn_errorCountperRecord)
        Me.grpbox_erroroptions.Controls.Add(Me.rbtn_errorCounterrtype)
        Me.grpbox_erroroptions.Controls.Add(Me.rbtn_errorCountPerTble)
        Me.grpbox_erroroptions.Controls.Add(Me.rbtn_allDetails)
        Me.grpbox_erroroptions.Location = New System.Drawing.Point(456, 12)
        Me.grpbox_erroroptions.Name = "grpbox_erroroptions"
        Me.grpbox_erroroptions.Size = New System.Drawing.Size(297, 349)
        Me.grpbox_erroroptions.TabIndex = 6
        Me.grpbox_erroroptions.TabStop = False
        Me.grpbox_erroroptions.Text = "Errors Options"
        '
        'rbtn_perCompound
        '
        Me.rbtn_perCompound.AutoSize = True
        Me.rbtn_perCompound.Location = New System.Drawing.Point(6, 269)
        Me.rbtn_perCompound.Name = "rbtn_perCompound"
        Me.rbtn_perCompound.Size = New System.Drawing.Size(186, 17)
        Me.rbtn_perCompound.TabIndex = 8
        Me.rbtn_perCompound.Text = "Show error count ( per compound)"
        Me.rbtn_perCompound.UseVisualStyleBackColor = True
        '
        'rbtn_pervillperTable
        '
        Me.rbtn_pervillperTable.AutoSize = True
        Me.rbtn_pervillperTable.Location = New System.Drawing.Point(6, 245)
        Me.rbtn_pervillperTable.Name = "rbtn_pervillperTable"
        Me.rbtn_pervillperTable.Size = New System.Drawing.Size(214, 17)
        Me.rbtn_pervillperTable.TabIndex = 5
        Me.rbtn_pervillperTable.Text = "Show error count ( per village per Table)"
        Me.rbtn_pervillperTable.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmb_errorView)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 20)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(283, 59)
        Me.GroupBox1.TabIndex = 7
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Error View Options"
        '
        'cmb_errorView
        '
        Me.cmb_errorView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmb_errorView.FormattingEnabled = True
        Me.cmb_errorView.Items.AddRange(New Object() {"PENDING", "ALL ERRORS", "CLEANED ONLY", "With Comments"})
        Me.cmb_errorView.Location = New System.Drawing.Point(6, 20)
        Me.cmb_errorView.Name = "cmb_errorView"
        Me.cmb_errorView.Size = New System.Drawing.Size(199, 21)
        Me.cmb_errorView.TabIndex = 0
        '
        'rbtn_perVillage
        '
        Me.rbtn_perVillage.AutoSize = True
        Me.rbtn_perVillage.Location = New System.Drawing.Point(6, 219)
        Me.rbtn_perVillage.Name = "rbtn_perVillage"
        Me.rbtn_perVillage.Size = New System.Drawing.Size(166, 17)
        Me.rbtn_perVillage.TabIndex = 4
        Me.rbtn_perVillage.Text = "Show error count ( per village)"
        Me.rbtn_perVillage.UseVisualStyleBackColor = True
        '
        'rbtn_errorCountperRecord
        '
        Me.rbtn_errorCountperRecord.AutoSize = True
        Me.rbtn_errorCountperRecord.Location = New System.Drawing.Point(6, 193)
        Me.rbtn_errorCountperRecord.Name = "rbtn_errorCountperRecord"
        Me.rbtn_errorCountperRecord.Size = New System.Drawing.Size(163, 17)
        Me.rbtn_errorCountperRecord.TabIndex = 3
        Me.rbtn_errorCountperRecord.Text = "Show error count (per record)"
        Me.rbtn_errorCountperRecord.UseVisualStyleBackColor = True
        '
        'rbtn_errorCounterrtype
        '
        Me.rbtn_errorCounterrtype.AutoSize = True
        Me.rbtn_errorCounterrtype.Location = New System.Drawing.Point(6, 167)
        Me.rbtn_errorCounterrtype.Name = "rbtn_errorCounterrtype"
        Me.rbtn_errorCounterrtype.Size = New System.Drawing.Size(221, 17)
        Me.rbtn_errorCounterrtype.TabIndex = 2
        Me.rbtn_errorCounterrtype.Text = "Show error count( per table per error type)"
        Me.rbtn_errorCounterrtype.UseVisualStyleBackColor = True
        '
        'rbtn_errorCountPerTble
        '
        Me.rbtn_errorCountPerTble.AutoSize = True
        Me.rbtn_errorCountPerTble.Location = New System.Drawing.Point(6, 141)
        Me.rbtn_errorCountPerTble.Name = "rbtn_errorCountPerTble"
        Me.rbtn_errorCountPerTble.Size = New System.Drawing.Size(156, 17)
        Me.rbtn_errorCountPerTble.TabIndex = 1
        Me.rbtn_errorCountPerTble.Text = "Show error count (per table)"
        Me.rbtn_errorCountPerTble.UseVisualStyleBackColor = True
        '
        'rbtn_allDetails
        '
        Me.rbtn_allDetails.AutoSize = True
        Me.rbtn_allDetails.Checked = True
        Me.rbtn_allDetails.Location = New System.Drawing.Point(6, 115)
        Me.rbtn_allDetails.Name = "rbtn_allDetails"
        Me.rbtn_allDetails.Size = New System.Drawing.Size(100, 17)
        Me.rbtn_allDetails.TabIndex = 0
        Me.rbtn_allDetails.TabStop = True
        Me.rbtn_allDetails.Text = "Show all Details"
        Me.rbtn_allDetails.UseVisualStyleBackColor = True
        '
        'cms_ShowRecord
        '
        Me.cms_ShowRecord.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ShowRecordsToolStripMenuItem, Me.SetErrorStatusToolStripMenuItem, Me.CommentToolStripMenuItem, Me.InsertQuerryRefToolStripMenuItem})
        Me.cms_ShowRecord.Name = "cms_ShowRecord"
        Me.cms_ShowRecord.Size = New System.Drawing.Size(161, 92)
        '
        'ShowRecordsToolStripMenuItem
        '
        Me.ShowRecordsToolStripMenuItem.Name = "ShowRecordsToolStripMenuItem"
        Me.ShowRecordsToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.ShowRecordsToolStripMenuItem.Text = "Show Record(s)"
        '
        'SetErrorStatusToolStripMenuItem
        '
        Me.SetErrorStatusToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CleanedToolStripMenuItem, Me.PendingToolStripMenuItem, Me.ProgramFalseAlarmToolStripMenuItem})
        Me.SetErrorStatusToolStripMenuItem.Name = "SetErrorStatusToolStripMenuItem"
        Me.SetErrorStatusToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.SetErrorStatusToolStripMenuItem.Text = "Set Error Status"
        '
        'CleanedToolStripMenuItem
        '
        Me.CleanedToolStripMenuItem.Name = "CleanedToolStripMenuItem"
        Me.CleanedToolStripMenuItem.Size = New System.Drawing.Size(173, 22)
        Me.CleanedToolStripMenuItem.Text = "Cleaned"
        '
        'PendingToolStripMenuItem
        '
        Me.PendingToolStripMenuItem.Name = "PendingToolStripMenuItem"
        Me.PendingToolStripMenuItem.Size = New System.Drawing.Size(173, 22)
        Me.PendingToolStripMenuItem.Text = "Pending"
        '
        'ProgramFalseAlarmToolStripMenuItem
        '
        Me.ProgramFalseAlarmToolStripMenuItem.Name = "ProgramFalseAlarmToolStripMenuItem"
        Me.ProgramFalseAlarmToolStripMenuItem.Size = New System.Drawing.Size(173, 22)
        Me.ProgramFalseAlarmToolStripMenuItem.Text = "Program/False Alarm"
        '
        'CommentToolStripMenuItem
        '
        Me.CommentToolStripMenuItem.Name = "CommentToolStripMenuItem"
        Me.CommentToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.CommentToolStripMenuItem.Text = "Comment"
        '
        'InsertQuerryRefToolStripMenuItem
        '
        Me.InsertQuerryRefToolStripMenuItem.Name = "InsertQuerryRefToolStripMenuItem"
        Me.InsertQuerryRefToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.InsertQuerryRefToolStripMenuItem.Text = "Insert Querry Ref"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PBarlabel, Me.PBar, Me.LabelProgress})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 665)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1071, 24)
        Me.StatusStrip1.TabIndex = 44
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'PBarlabel
        '
        Me.PBarlabel.Name = "PBarlabel"
        Me.PBarlabel.Size = New System.Drawing.Size(38, 19)
        Me.PBarlabel.Text = "Ready"
        '
        'PBar
        '
        Me.PBar.Enabled = False
        Me.PBar.Name = "PBar"
        Me.PBar.Size = New System.Drawing.Size(100, 18)
        '
        'LabelProgress
        '
        Me.LabelProgress.AutoSize = False
        Me.LabelProgress.Name = "LabelProgress"
        Me.LabelProgress.Size = New System.Drawing.Size(600, 19)
        Me.LabelProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'BackgroundWorkerValidate
        '
        Me.BackgroundWorkerValidate.WorkerReportsProgress = True
        '
        'tb_log
        '
        Me.tb_log.Location = New System.Drawing.Point(759, 20)
        Me.tb_log.Multiline = True
        Me.tb_log.Name = "tb_log"
        Me.tb_log.ReadOnly = True
        Me.tb_log.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tb_log.Size = New System.Drawing.Size(305, 341)
        Me.tb_log.TabIndex = 45
        Me.tb_log.WordWrap = False
        '
        'frm_ToValidateForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(1071, 689)
        Me.Controls.Add(Me.tb_log)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.grpbox_tables)
        Me.Controls.Add(Me.grpbox_erroroptions)
        Me.Controls.Add(Me.gpbox_results)
        Me.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frm_ToValidateForm"
        Me.ShowIcon = False
        Me.Text = "Validations"
        Me.gpbox_results.ResumeLayout(False)
        Me.gpbox_results.PerformLayout()
        CType(Me.dgr_results, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bndNvgerrors, System.ComponentModel.ISupportInitialize).EndInit()
        Me.bndNvgerrors.ResumeLayout(False)
        Me.bndNvgerrors.PerformLayout()
        Me.grpbox_tables.ResumeLayout(False)
        Me.grpbox_tables.PerformLayout()
        Me.grpbox_erroroptions.ResumeLayout(False)
        Me.grpbox_erroroptions.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.cms_ShowRecord.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents gpbox_results As System.Windows.Forms.GroupBox
    Friend WithEvents dgr_results As System.Windows.Forms.DataGridView
    Friend WithEvents lstbox_tables As System.Windows.Forms.ListBox
    Friend WithEvents cmb_selectOption As System.Windows.Forms.ComboBox
    Friend WithEvents btn_Go As System.Windows.Forms.Button
    Friend WithEvents grpbox_tables As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents grpbox_erroroptions As System.Windows.Forms.GroupBox
    Friend WithEvents rbtn_errorCountperRecord As System.Windows.Forms.RadioButton
    Friend WithEvents rbtn_errorCounterrtype As System.Windows.Forms.RadioButton
    Friend WithEvents rbtn_errorCountPerTble As System.Windows.Forms.RadioButton
    Friend WithEvents rbtn_allDetails As System.Windows.Forms.RadioButton
    Friend WithEvents bndNvgerrors As System.Windows.Forms.BindingNavigator
    Friend WithEvents BindingNavigatorCountItem As System.Windows.Forms.ToolStripLabel
    Friend WithEvents BindingNavigatorMoveFirstItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorMovePreviousItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BindingNavigatorPositionItem As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents BindingNavigatorSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BindingNavigatorMoveNextItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorMoveLastItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cms_ShowRecord As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ShowRecordsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents rbtn_perVillage As System.Windows.Forms.RadioButton
    Friend WithEvents rbtn_pervillperTable As System.Windows.Forms.RadioButton
    Friend WithEvents SetErrorStatusToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CleanedToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cmb_errorView As System.Windows.Forms.ComboBox
    Friend WithEvents btn_selectAll As System.Windows.Forms.Button
    Friend WithEvents btn_deSelectAll As System.Windows.Forms.Button
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents PBarlabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents PBar As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents LabelProgress As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents BackgroundWorkerValidate As System.ComponentModel.BackgroundWorker
    Friend WithEvents tb_log As System.Windows.Forms.TextBox
    Friend WithEvents ToolStripButtonPrint As System.Windows.Forms.ToolStripButton
    Friend WithEvents ckb_upload As System.Windows.Forms.CheckBox
    Friend WithEvents rbtn_perCompound As System.Windows.Forms.RadioButton
    Friend WithEvents PendingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ProgramFalseAlarmToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CommentToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InsertQuerryRefToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    'Friend WithEvents CachedcompletnessReport1 As Households_Registration_System.CachedcompletnessReport
End Class
