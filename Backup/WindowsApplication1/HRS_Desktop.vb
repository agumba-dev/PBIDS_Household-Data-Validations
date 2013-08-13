Imports System.Windows.Forms
Imports DgvFilterPopup
Public Class HRS_Desktop
    Public enabledelete As Boolean = False
    Public dataviewTableName As String
    Private globalvariables As clsGlobalVariables = clsGlobalVariables.getObject

#Region " code to enable keyboard shortcuts i.e. ctrl+x,ctrl+c ctrl+v etc"


    Private Declare Auto Function SendMessage Lib "user32" (ByVal hwnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr

    'Then overide ProcessCmdKey in the MDI parent:

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean

        SendMessage(Me.ActiveMdiChild.Handle, msg.Msg, msg.WParam, msg.LParam)

        'Return MyBase.ProcessCmdKey(msg, keyData)

    End Function
#End Region
    Private Sub ShowNewForm(ByVal sender As Object, ByVal e As EventArgs) Handles NewToolStripMenuItem.Click, NewToolStripButton.Click, NewWindowToolStripMenuItem.Click
        ' Create a new instance of the child form.
        Dim ChildForm As New System.Windows.Forms.Form
        ' Make it a child of this MDI form before showing it.
        ChildForm.MdiParent = Me

        m_ChildFormNumber += 1
        ChildForm.Text = "Window " & m_ChildFormNumber

        ChildForm.Show()
    End Sub

    Private Sub OpenFile(ByVal sender As Object, ByVal e As EventArgs) Handles OpenToolStripMenuItem.Click, OpenToolStripButton.Click
        Dim OpenFileDialog As New OpenFileDialog
        OpenFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        OpenFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
        If (OpenFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            Dim FileName As String = OpenFileDialog.FileName
            ' TODO: Add code here to open the file.
        End If
    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SaveAsToolStripMenuItem.Click
        Dim SaveFileDialog As New SaveFileDialog
        SaveFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        SaveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"

        If (SaveFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            Dim FileName As String = SaveFileDialog.FileName
            ' TODO: Add code here to save the current contents of the form to a file.
        End If
    End Sub


    Private Sub ExitToolsStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ExitToolStripMenuItem.Click
        Global.System.Windows.Forms.Application.Exit()
    End Sub

    Private Sub CutToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CutToolStripMenuItem.Click
        ' Use My.Computer.Clipboard to insert the selected text or images into the clipboard
    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CopyToolStripMenuItem.Click
        ' Use My.Computer.Clipboard to insert the selected text or images into the clipboard
    End Sub

    Private Sub PasteToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles PasteToolStripMenuItem.Click
        'Use My.Computer.Clipboard.GetText() or My.Computer.Clipboard.GetData to retrieve information from the clipboard.
    End Sub

    Private Sub ToolBarToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ToolBarToolStripMenuItem.Click
        Me.ToolStrip.Visible = Me.ToolBarToolStripMenuItem.Checked
    End Sub

    Private Sub StatusBarToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles StatusBarToolStripMenuItem.Click
        Me.StatusStrip.Visible = Me.StatusBarToolStripMenuItem.Checked
    End Sub

    Private Sub CascadeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CascadeToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub

    Private Sub TileVerticleToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileVerticalToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub

    Private Sub TileHorizontalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileHorizontalToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub

    Private Sub ArrangeIconsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ArrangeIconsToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.ArrangeIcons)
    End Sub

    Private Sub CloseAllToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CloseAllToolStripMenuItem.Click
        ' Close all child forms of the parent.
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next
    End Sub

    'data entry menu forms begins
    Private m_ChildFormNumber As Integer = 0
    Private Sub BaselineToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BaselineToolStripMenuItem.Click
        ' Create a new instance of the child form.
        'Dim baselineForm As New frmBaseline
        ' Make it a child of this MDI form before showing it.
        'baselineForm.MdiParent = Me
        ''show the form
        'baselineForm.Show()
    End Sub

    Private Sub LocationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LocationToolStripMenuItem.Click
        'Dim locationForm As New frmLocations
        'locationForm.MdiParent = Me
        'locationForm.Show()
    End Sub

    Private Sub IndividualsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IndividualsToolStripMenuItem.Click
        'Dim indiForm As New individual
        'indiForm.MdiParent = Me
        'indiForm.Show()
    End Sub
    Private Sub SocialGroupToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SocialGroupToolStripMenuItem.Click
        'Dim arr As New ArrayList
        'Dim socGrpForm As New frmSocialGroup(arr)
        'socGrpForm.MdiParent = Me
        'socGrpForm.Show()
    End Sub
    Private Sub LocationObservationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LocationObservationToolStripMenuItem.Click
        'Dim locObsrForm As New frmObservation
        'locObsrForm.MdiParent = Me
        'locObsrForm.Show()
    End Sub
    Private Sub PregnancyOutcomeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PregnancyOutcomeToolStripMenuItem.Click
        'Dim prgOutForm As New frmPregOutcome
        'prgOutForm.MdiParent = Me
        'prgOutForm.Show()
    End Sub

    Private Sub BirthToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BirthToolStripMenuItem.Click
        'Dim birthForm As New frmBirth
        'birthForm.MdiParent = Me
        'birthForm.Show()
    End Sub
    Private Sub DeathToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeathToolStripMenuItem.Click
        'Dim deathform As New frmDeath
        'deathform.MdiParent = Me
        'deathform.Show()
    End Sub

    Private Sub MigrationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MigrationToolStripMenuItem.Click
        'Dim migrationForm As New frmMigrations("", "")
        'migrationForm.MdiParent = Me
        'migrationForm.Show()
    End Sub

    Private Sub PreganancyObservationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PreganancyObservationToolStripMenuItem.Click
        'Dim prgObsForm As New frmPregnancyObservation
        'prgObsForm.MdiParent = Me
        'prgObsForm.Show()
    End Sub

    Private Sub MaritalStatusChangeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MaritalStatusChangeToolStripMenuItem.Click
        'Dim relationshipForm As New frmRelationship
        'relationshipForm.MdiParent = Me
        'relationshipForm.Show()
    End Sub


    Private Sub ChangMembershipStatusToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChangMembershipStatusToolStripMenuItem.Click
        'Dim membershpForm As New frmMembership
        'membershpForm.MdiParent = Me
        'membershpForm.Show()
    End Sub

    'Private Sub EditResidencyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditResidencyToolStripMenuItem.Click
    '    Dim residencyForm As New frmPregnancy
    '    residencyForm.MdiParent = Me
    '    residencyForm.Show()
    'End Sub

    'Private Sub ViewResidencyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewResidencyToolStripMenuItem.Click
    '    Dim viewResidencyForm As New frmViewResidency
    '    viewResidencyForm.MdiParent = Me
    '    viewResidencyForm.Show()
    'End Sub

    'Private Sub GeneralUpdatesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GeneralUpdatesToolStripMenuItem.Click
    '    Dim updatesForm As New frmUpdates
    '    updatesForm.MdiParent = Me
    '    updatesForm.Show()
    'End Sub
    ''end of data entry forms
    ''reports menu forms begins
    'Private Sub DemographicRatesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DemographicRatesToolStripMenuItem.Click
    '    Dim ratesForm As New frmRates
    '    ratesForm.MdiParent = Me
    '    ratesForm.Show()
    'End Sub

    'Private Sub LifeTablesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LifeTablesToolStripMenuItem.Click
    '    Dim lifeTabForm As New frmLifetab
    '    lifeTabForm.MdiParent = Me
    '    lifeTabForm.Show()
    'End Sub

    'Private Sub DataEntrySummaryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataEntrySummaryToolStripMenuItem.Click
    '    Dim dereportForm As New frmDeReport
    '    dereportForm.MdiParent = Me
    '    dereportForm.Show()
    'End Sub
    ''end of report generation forms
    ''Utilities menu forms begins
    'Private Sub ValidateDataToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ValidateDataToolStripMenuItem.Click
    '    Dim validata As New frmValidateData
    '    validata.MdiParent = Me
    '    validata.Show()
    'End Sub

    'Private Sub RegistrationBooksToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RegistrationBooksToolStripMenuItem.Click
    '    Dim registerForm As New frmRegister
    '    registerForm.MdiParent = Me
    '    registerForm.Show()
    'End Sub

    'Private Sub UserAccessToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UserAccessToolStripMenuItem.Click
    '    Dim usraccess As New frmAdmin
    '    usraccess.MdiParent = Me
    '    usraccess.Show()
    'End Sub

    'Private Sub EnterRoundsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EnterRoundsToolStripMenuItem.Click

    'End Sub

    'Private Sub MigrationReconToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim migrecForm As New frmRecn
    '    migrecForm.MdiParent = Me
    '    migrecForm.Show()
    'End Sub

    'Private Sub PregObsReconToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PregObsReconToolStripMenuItem.Click
    '    Dim prgForm As New frmRecn
    '    prgForm.MdiParent = Me
    '    prgForm.Show()
    'End Sub

    Private Sub HRS_Desktop_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'If Not System.IO.File.Exists("serverpath") Then
        '    Dim DBcof As New configureServer
        '    DBcof.ShowDialog()
        'End If
        'globalvariables.initialiseGlobalVariables()
        'Dim fm As New DgvFilterManager(dgV_General)

        'If getrunSetup().Trim.ToLower = "on" Then
        '    Dim frmVal As New frm_ToValidateForm
        '    frmVal.MdiParent = Me
        '    frmVal.Show()
        'Else
        '    Dim gui_Login As New frmLogin()
        '    gui_Login.MdiParent = Me
        '    gui_Login.Show()

        '    updateObservationTable()

        '    Dim verDesployed As System.Version
        '    Dim strVerDeployed As String

        '    If (Deployment.Application.ApplicationDeployment.IsNetworkDeployed) Then
        '        verDesployed = Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion '.Application.Deployment.CurrentDeployment
        '        strVerDeployed = verDesployed.ToString
        '        Me.tsmi_Version.Text = "VERSION: " & strVerDeployed 'show the current version

        '    End If
        'End If
    End Sub
    Private Sub updateObservationTable()

        'Dim s As String = "INSERT INTO [TEMP_DSSHRS].[DSS].[observation] " _
        '& "([observeid],[locationid],[round],[date],[fieldworker],[rec_status]) " _
        '& "select locationid +'-'+ [round] as observeid,locationid,round,date " _
        '& ",fieldworker,rec_status from [dss].[visitation] vist where outcome='completed' " _
        '& "and not exists (select * from [dss].[observation] obs where " _
        '& "obs.observeid= vist.locationid +'-'+ vist.[round] ) "
        ''completnessCon = ObjDbAccess.getConnectionToServer("SupervisoryTables")
        'Dim con As SqlClient.SqlConnection = ObjDbAccess.getConnectionToServer("TEMP_DSSHRS")
        'Dim cmd As New SqlClient.SqlCommand(s, con)
        'Dim c As Integer = cmd.ExecuteNonQuery()

    End Sub
    Private Sub SocialAndEconomicStudiesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SocialAndEconomicStudiesToolStripMenuItem.Click
        'Dim sesForm As New frmHholdSocioEco
        'sesForm.MdiParent = Me
        'sesForm.Show()
    End Sub

    'Private Sub InfectiousTreatedNetsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InfectiousTreatedNetsToolStripMenuItem.Click
    '    Dim itnForm As New frmITN("")
    '    itnForm.MdiParent = Me
    '    itnForm.Show()
    'End Sub

    Private Sub tsbtn_Baseline_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtn_Baseline.Click
        ' Create a new instance of the child form.
        'Dim baselineForm As New frmBaseline
        '' Make it a child of this MDI form before showing it.
        'baselineForm.MdiParent = Me
        ''show the form
        'baselineForm.Show()
    End Sub

    'Private Sub tsbtn_Location_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtn_Location.Click
    '    Dim locationForm As New frmLocations
    '    locationForm.MdiParent = Me
    '    locationForm.Show()
    'End Sub

    'Private Sub tsbtn_Birth_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtn_Birth.Click
    '    Dim indiForm As New individual
    '    indiForm.MdiParent = Me
    '    indiForm.Show()
    'End Sub

    'Private Sub tsbtn_Individual_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtn_Individual.Click
    '    Dim indiForm As New individual
    '    indiForm.MdiParent = Me
    '    indiForm.Show()
    'End Sub

    'Private Sub tsbtn_EditResidency_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtn_EditResidency.Click
    '    Dim residencyForm As New frmPregnancy
    '    residencyForm.MdiParent = Me
    '    residencyForm.Show()
    'End Sub

    'Private Sub tsbtn_ViewResidency_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtn_ViewResidency.Click
    '    Dim viewResidencyForm As New frmViewResidency
    '    viewResidencyForm.MdiParent = Me
    '    viewResidencyForm.Show()
    'End Sub

    'Private Sub tsbtn_Death_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbtn_Death.Click
    '    Dim deathform As New frmDeath
    '    deathform.MdiParent = Me
    '    deathform.Show()
    'End Sub

    'Private Sub CompletnessCheckToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CompletnessCheckToolStripMenuItem.Click
    '    Dim completnessForm As New FormCompleteness
    '    completnessForm.MdiParent = Me
    '    completnessForm.Show()
    'End Sub

    'Private Sub ConfigureServerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConfigureServerToolStripMenuItem.Click
    '    Dim configureServerForm As New configureServer
    '    configureServerForm.MdiParent = Me
    '    configureServerForm.Show()
    'End Sub

    'Private Sub RecordedEventsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RecordedEventsToolStripMenuItem.Click
    '    Dim frmFieldEvents As New FormFieldEvents
    '    frmFieldEvents.MdiParent = Me
    '    frmFieldEvents.Show()
    'End Sub

    'Private Sub VisitInfoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VisitInfoToolStripMenuItem.Click
    '    Dim frmvsts As New FormLocationVisitation
    '    frmvsts.MdiParent = Me
    '    frmvsts.Show()
    'End Sub

    'Private Sub WorkProgressToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WorkProgressToolStripMenuItem.Click
    '    Dim frmprgrpt As New FormProgressReport
    '    frmprgrpt.MdiParent = Me
    '    frmprgrpt.Show()
    'End Sub

    'Private Sub MigrationReconcilliationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MigrationReconcilliationToolStripMenuItem.Click
    '    Dim frmMigRecon As New FormMigrationReconcilliation
    '    frmMigRecon.MdiParent = Me
    '    frmMigRecon.Show()
    'End Sub

    'Private Sub LoggOffToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoggOffToolStripMenuItem.Click
    '    Singleton.SingletonDestructor()
    '    frmLogin.Show()
    '    Me.MenuStrip.Enabled = False
    'End Sub

    'Private Sub QuerriesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles QuerriesToolStripMenuItem.Click
    '    Dim frmSup As New frmsupervisor
    '    frmSup.MdiParent = Me
    '    frmSup.Show()
    'End Sub

    Private Sub ViewErrorsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewErrorsToolStripMenuItem.Click
        Dim frmMainVal As New Frm_TheValidationsEditor
        frmMainVal.MdiParent = Me
        frmMainVal.Show()
    End Sub

    Private Sub AdministorErrorsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AdministorErrorsToolStripMenuItem.Click
        Dim frmVal As New frm_ToValidateForm
        frmVal.MdiParent = Me
        frmVal.Show()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmi_Version.Click
        ''My.Resources.Version()
        ''Dim vrs As Version = New Version(Environment.Version.)
        'Dim verDesployed As System.Version
        'Dim strVerDeployed As String

        'If (Deployment.Application.ApplicationDeployment.IsNetworkDeployed) Then
        '    verDesployed = Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion '.Application.Deployment.CurrentDeployment
        '    strVerDeployed = verDesployed.ToString
        '    MessageBox.Show("VERSION: " & strVerDeployed)

        'End If
        ''MessageBox.Show("Major: " & Environment.Version.Major & ":" & Environment.Version.Minor & ":" & Environment.Version.Build & ":" & Environment.Version.Revision)
        ''MessageBox.Show("VERSION: " & My.Application .Info.Version.ToString, "Version Control", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    'Private Sub SearchEngineToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchEngineToolStripMenuItem.Click
    '    frm_SearchDSS_HRS.MdiParent = Me
    '    frm_SearchDSS_HRS.Show()
    'End Sub

    'Private Sub btn_clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_clear.Click
    '    dgV_General.DataSource = Nothing
    '    btn_newWindow.Enabled = False
    'End Sub


    'Private Sub btn_newWindow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_newWindow.Click
    '    Dim bs As New BindingSource()
    '    bs.DataSource = dgV_General.DataSource
    '    Dim frmrec As New frm_errorRecord
    '    frmrec.Text = lblGridName.Text
    '    frmrec.bnv_errorRecords.BindingSource = bs
    '    frmrec.dgv_errorRecords.DataSource = bs
    '    frmrec.MdiParent = Me
    '    frmrec.Show()
    'End Sub

    Private Sub DataTransferToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataTransferToolStripMenuItem.Click
        Dim frm As New frmDataTransfer
        frm.MdiParent = Me
        frm.Show()
    End Sub

    Private Sub VAlMgtToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VAlMgtToolStripMenuItem.Click
        Dim frm As New frmValidationMgmt
        frm.MdiParent = Me
        frm.Show()
    End Sub

    'Private Sub ProblemLoggerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProblemLoggerToolStripMenuItem.Click
    '    Dim frm As New frmProblemLogger
    '    frm.MdiParent = Me
    '    frm.Show()
    'End Sub

    'Private Sub btn_DeleteRecord_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_DeleteRecord.Click


    '    If MsgBox("this will delete the current selected record only, are you sure?", MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

    '        Dim recordTempid As String = dgV_General.CurrentRow.Cells("transit_id").Value.ToString.Trim
    '        deleterecord(dataviewTableName, recordTempid)

    '        Dim bs As BindingSource = dgV_General.DataSource
    '        bs.ResetBindings(False)
    '    End If
    'End Sub

    Private Sub dgV_General_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgV_General.CellContentClick

    End Sub

    Private Sub dgV_General_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgV_General.SelectionChanged
        btn_DeleteRecord.Enabled = False
        If dgV_General.SelectedRows.Count <> 0 Then
            '    If dataviewTableName.Trim.ToUpper = "DSS.MIGRATIONS" Then Return
            btn_DeleteRecord.Enabled = enabledelete
        End If
    End Sub

    Private Sub DatabaseSpyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DatabaseSpyToolStripMenuItem.Click
        Dim frm As New frmDBEditor

        frm.MdiParent = Me
        frm.Show()
        frm.serversComboBox.Text = "DSS-KEK3"
        frm.connectToServer("DSS-KEK3")
    End Sub

    Private Sub TableEditConfigurationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TableEditConfigurationToolStripMenuItem.Click
        Dim frm As New frm_EditConfigurations
        frm.MdiParent = Me
        frm.Show()
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class
