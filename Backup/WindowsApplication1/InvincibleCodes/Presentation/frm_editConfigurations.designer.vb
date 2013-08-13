<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_EditConfigurations
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
        Me.cmb_chooseTable = New System.Windows.Forms.ComboBox
        Me.serversComboBox = New System.Windows.Forms.ComboBox
        Me.btnconnect = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.databasesComboBox = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.findServersButton = New System.Windows.Forms.Button
        Me.grpBoxDataGrid = New System.Windows.Forms.GroupBox
        Me.dgv_records = New System.Windows.Forms.DataGridView
        Me.clmnName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.clmnreadOnly = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.dgv_tables = New System.Windows.Forms.DataGridView
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewCheckBoxColumn1 = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.grpBoxDataGrid.SuspendLayout()
        CType(Me.dgv_records, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.dgv_tables, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmb_chooseTable
        '
        Me.cmb_chooseTable.FormattingEnabled = True
        Me.cmb_chooseTable.Location = New System.Drawing.Point(163, 76)
        Me.cmb_chooseTable.Name = "cmb_chooseTable"
        Me.cmb_chooseTable.Size = New System.Drawing.Size(148, 21)
        Me.cmb_chooseTable.TabIndex = 30
        Me.cmb_chooseTable.Visible = False
        '
        'serversComboBox
        '
        Me.serversComboBox.FormattingEnabled = True
        Me.serversComboBox.Items.AddRange(New Object() {"KISW-DSS-09948"})
        Me.serversComboBox.Location = New System.Drawing.Point(163, 12)
        Me.serversComboBox.Name = "serversComboBox"
        Me.serversComboBox.Size = New System.Drawing.Size(148, 21)
        Me.serversComboBox.TabIndex = 23
        '
        'btnconnect
        '
        Me.btnconnect.Location = New System.Drawing.Point(317, 12)
        Me.btnconnect.Name = "btnconnect"
        Me.btnconnect.Size = New System.Drawing.Size(60, 23)
        Me.btnconnect.TabIndex = 29
        Me.btnconnect.Text = "Connect"
        Me.btnconnect.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(72, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 13)
        Me.Label1.TabIndex = 26
        Me.Label1.Text = "Choose server"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(52, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(89, 13)
        Me.Label2.TabIndex = 27
        Me.Label2.Text = "choose database"
        '
        'databasesComboBox
        '
        Me.databasesComboBox.FormattingEnabled = True
        Me.databasesComboBox.Location = New System.Drawing.Point(163, 45)
        Me.databasesComboBox.Name = "databasesComboBox"
        Me.databasesComboBox.Size = New System.Drawing.Size(148, 21)
        Me.databasesComboBox.TabIndex = 25
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(19, 82)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(138, 13)
        Me.Label3.TabIndex = 28
        Me.Label3.Text = "choose table to view details"
        '
        'findServersButton
        '
        Me.findServersButton.Location = New System.Drawing.Point(383, 12)
        Me.findServersButton.Name = "findServersButton"
        Me.findServersButton.Size = New System.Drawing.Size(75, 23)
        Me.findServersButton.TabIndex = 24
        Me.findServersButton.Text = "FindServers"
        Me.findServersButton.UseVisualStyleBackColor = True
        '
        'grpBoxDataGrid
        '
        Me.grpBoxDataGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpBoxDataGrid.Controls.Add(Me.dgv_records)
        Me.grpBoxDataGrid.Location = New System.Drawing.Point(12, 329)
        Me.grpBoxDataGrid.Name = "grpBoxDataGrid"
        Me.grpBoxDataGrid.Size = New System.Drawing.Size(599, 281)
        Me.grpBoxDataGrid.TabIndex = 31
        Me.grpBoxDataGrid.TabStop = False
        '
        'dgv_records
        '
        Me.dgv_records.AllowUserToAddRows = False
        Me.dgv_records.AllowUserToDeleteRows = False
        Me.dgv_records.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.dgv_records.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_records.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.clmnName, Me.clmnreadOnly})
        Me.dgv_records.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgv_records.Location = New System.Drawing.Point(3, 16)
        Me.dgv_records.Name = "dgv_records"
        Me.dgv_records.Size = New System.Drawing.Size(593, 262)
        Me.dgv_records.TabIndex = 0
        '
        'clmnName
        '
        Me.clmnName.HeaderText = "Column Name"
        Me.clmnName.Name = "clmnName"
        Me.clmnName.Width = 200
        '
        'clmnreadOnly
        '
        Me.clmnreadOnly.HeaderText = "ReadOnly"
        Me.clmnreadOnly.Name = "clmnreadOnly"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(163, 625)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(83, 41)
        Me.Button1.TabIndex = 32
        Me.Button1.Text = "SAVE"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(375, 625)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(83, 41)
        Me.Button2.TabIndex = 33
        Me.Button2.Text = "CLOSE"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.dgv_tables)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 103)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(599, 220)
        Me.GroupBox1.TabIndex = 32
        Me.GroupBox1.TabStop = False
        '
        'dgv_tables
        '
        Me.dgv_tables.AllowUserToAddRows = False
        Me.dgv_tables.AllowUserToDeleteRows = False
        Me.dgv_tables.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.dgv_tables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv_tables.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewCheckBoxColumn1})
        Me.dgv_tables.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgv_tables.Location = New System.Drawing.Point(3, 16)
        Me.dgv_tables.Name = "dgv_tables"
        Me.dgv_tables.Size = New System.Drawing.Size(593, 201)
        Me.dgv_tables.TabIndex = 0
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "Table Name"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.Width = 200
        '
        'DataGridViewCheckBoxColumn1
        '
        Me.DataGridViewCheckBoxColumn1.HeaderText = "Allow Edit"
        Me.DataGridViewCheckBoxColumn1.Name = "DataGridViewCheckBoxColumn1"
        '
        'frm_EditConfigurations
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(623, 692)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.grpBoxDataGrid)
        Me.Controls.Add(Me.cmb_chooseTable)
        Me.Controls.Add(Me.serversComboBox)
        Me.Controls.Add(Me.btnconnect)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.databasesComboBox)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.findServersButton)
        Me.Name = "frm_EditConfigurations"
        Me.Text = "Edit Configurations"
        Me.grpBoxDataGrid.ResumeLayout(False)
        CType(Me.dgv_records, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.dgv_tables, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmb_chooseTable As System.Windows.Forms.ComboBox
    Friend WithEvents serversComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents btnconnect As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents databasesComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents findServersButton As System.Windows.Forms.Button
    Friend WithEvents grpBoxDataGrid As System.Windows.Forms.GroupBox
    Friend WithEvents dgv_records As System.Windows.Forms.DataGridView
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents clmnName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents clmnreadOnly As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents dgv_tables As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewCheckBoxColumn1 As System.Windows.Forms.DataGridViewCheckBoxColumn


End Class
