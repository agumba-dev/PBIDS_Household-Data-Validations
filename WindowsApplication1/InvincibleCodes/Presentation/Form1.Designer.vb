<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TheInvincibleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditValidationsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DataEditorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditConfigToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RefValidationsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SqlWindowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(990, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TheInvincibleToolStripMenuItem, Me.EditValidationsToolStripMenuItem, Me.DataEditorToolStripMenuItem, Me.EditConfigToolStripMenuItem, Me.RefValidationsToolStripMenuItem, Me.SqlWindowToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'TheInvincibleToolStripMenuItem
        '
        Me.TheInvincibleToolStripMenuItem.Name = "TheInvincibleToolStripMenuItem"
        Me.TheInvincibleToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.TheInvincibleToolStripMenuItem.Text = "TheInvincible"
        '
        'EditValidationsToolStripMenuItem
        '
        Me.EditValidationsToolStripMenuItem.Name = "EditValidationsToolStripMenuItem"
        Me.EditValidationsToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.EditValidationsToolStripMenuItem.Text = "Edit Validations"
        '
        'DataEditorToolStripMenuItem
        '
        Me.DataEditorToolStripMenuItem.Name = "DataEditorToolStripMenuItem"
        Me.DataEditorToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.DataEditorToolStripMenuItem.Text = "Data Editor"
        '
        'EditConfigToolStripMenuItem
        '
        Me.EditConfigToolStripMenuItem.Name = "EditConfigToolStripMenuItem"
        Me.EditConfigToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.EditConfigToolStripMenuItem.Text = "Edit Config"
        '
        'RefValidationsToolStripMenuItem
        '
        Me.RefValidationsToolStripMenuItem.Name = "RefValidationsToolStripMenuItem"
        Me.RefValidationsToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.RefValidationsToolStripMenuItem.Text = "ref validations"
        '
        'SqlWindowToolStripMenuItem
        '
        Me.SqlWindowToolStripMenuItem.Name = "SqlWindowToolStripMenuItem"
        Me.SqlWindowToolStripMenuItem.Size = New System.Drawing.Size(154, 22)
        Me.SqlWindowToolStripMenuItem.Text = "sql Window"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(990, 601)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.ShowIcon = False
        Me.Text = "The Invincible"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TheInvincibleToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditValidationsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DataEditorToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditConfigToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RefValidationsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SqlWindowToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
