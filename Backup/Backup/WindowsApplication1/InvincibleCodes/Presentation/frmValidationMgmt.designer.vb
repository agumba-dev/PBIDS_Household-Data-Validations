<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmValidationMgmt
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
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.btnADD = New System.Windows.Forms.Button
        Me.cmb_refColumn = New System.Windows.Forms.ComboBox
        Me.cmb_RefTable = New System.Windows.Forms.ComboBox
        Me.cmb_TableCol = New System.Windows.Forms.ComboBox
        Me.cmb_TableName = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.grid_validRules = New System.Windows.Forms.DataGridView
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.grid_validRules, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.btnADD)
        Me.SplitContainer1.Panel1.Controls.Add(Me.cmb_refColumn)
        Me.SplitContainer1.Panel1.Controls.Add(Me.cmb_RefTable)
        Me.SplitContainer1.Panel1.Controls.Add(Me.cmb_TableCol)
        Me.SplitContainer1.Panel1.Controls.Add(Me.cmb_TableName)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label4)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label3)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.grid_validRules)
        Me.SplitContainer1.Size = New System.Drawing.Size(795, 677)
        Me.SplitContainer1.SplitterDistance = 328
        Me.SplitContainer1.TabIndex = 0
        '
        'btnADD
        '
        Me.btnADD.Location = New System.Drawing.Point(224, 235)
        Me.btnADD.Name = "btnADD"
        Me.btnADD.Size = New System.Drawing.Size(75, 23)
        Me.btnADD.TabIndex = 8
        Me.btnADD.Text = "ADD Validation"
        Me.btnADD.UseVisualStyleBackColor = True
        '
        'cmb_refColumn
        '
        Me.cmb_refColumn.FormattingEnabled = True
        Me.cmb_refColumn.Location = New System.Drawing.Point(135, 165)
        Me.cmb_refColumn.Name = "cmb_refColumn"
        Me.cmb_refColumn.Size = New System.Drawing.Size(327, 21)
        Me.cmb_refColumn.TabIndex = 7
        '
        'cmb_RefTable
        '
        Me.cmb_RefTable.FormattingEnabled = True
        Me.cmb_RefTable.Location = New System.Drawing.Point(131, 104)
        Me.cmb_RefTable.Name = "cmb_RefTable"
        Me.cmb_RefTable.Size = New System.Drawing.Size(331, 21)
        Me.cmb_RefTable.TabIndex = 6
        '
        'cmb_TableCol
        '
        Me.cmb_TableCol.FormattingEnabled = True
        Me.cmb_TableCol.Location = New System.Drawing.Point(131, 58)
        Me.cmb_TableCol.Name = "cmb_TableCol"
        Me.cmb_TableCol.Size = New System.Drawing.Size(331, 21)
        Me.cmb_TableCol.TabIndex = 5
        '
        'cmb_TableName
        '
        Me.cmb_TableName.FormattingEnabled = True
        Me.cmb_TableName.Location = New System.Drawing.Point(131, 13)
        Me.cmb_TableName.Name = "cmb_TableName"
        Me.cmb_TableName.Size = New System.Drawing.Size(331, 21)
        Me.cmb_TableName.TabIndex = 4
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(3, 168)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(126, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Reference Column Name"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 107)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(87, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Reference Table"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 61)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(68, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "table Column"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(65, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Table Name"
        '
        'grid_validRules
        '
        Me.grid_validRules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grid_validRules.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grid_validRules.Location = New System.Drawing.Point(0, 0)
        Me.grid_validRules.Name = "grid_validRules"
        Me.grid_validRules.Size = New System.Drawing.Size(795, 345)
        Me.grid_validRules.TabIndex = 0
        '
        'frmValidationMgmt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(795, 677)
        Me.Controls.Add(Me.SplitContainer1)
        Me.HelpButton = True
        Me.Name = "frmValidationMgmt"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "    Validation Rule Editor"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.grid_validRules, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents grid_validRules As System.Windows.Forms.DataGridView
    Friend WithEvents btnADD As System.Windows.Forms.Button
    Friend WithEvents cmb_refColumn As System.Windows.Forms.ComboBox
    Friend WithEvents cmb_RefTable As System.Windows.Forms.ComboBox
    Friend WithEvents cmb_TableCol As System.Windows.Forms.ComboBox
    Friend WithEvents cmb_TableName As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
