
Imports System.Data
Imports System.Data.SqlClient
Public Enum datalevel
    DSSHRS = 0
    TEMP_DSSHRS = 1
End Enum
Public Enum ValidationType
    ''' <summary>
    ''' Used for table level processing
    ''' </summary>
    ''' <remarks></remarks>
    BatchProcessing = 0
    ''' <summary>
    ''' used for row/record level processing
    ''' </summary>
    ''' <remarks></remarks>
    Transactionprocessing = 1
End Enum
Public Class ValidationReturnType
    Friend returnValue As Boolean
    Friend returnmsg As String

End Class

Public Class clsDataTransfer
   
    'Do variable declaration here
#Region "Variable Declaration"
    Private clsGlobalVariable As clsGlobalVariables = clsGlobalVariables.getObject
    Private foreignKeys As clsForeignKeyValidation = clsForeignKeyValidation.getObject
    Private resd As New clsResidency(mhrsSyncValidationTypes.batchprocessing)
    Private resdDSSHRS As New clsDsshrs_Residency_Val(mhrsSyncValidationTypes.batchprocessing)
    Private membDSSHRs As New clsDsshrs_Membership_Val(mhrsSyncValidationTypes.batchprocessing)
    Private pregDSSHRs As New clsDsshrs_Pregnancy_Val(mhrsSyncValidationTypes.batchprocessing)
    Private memb As New clsMembership(mhrsSyncValidationTypes.batchprocessing)
    Private preg As New clsPregnancy(mhrsSyncValidationTypes.batchprocessing)
    'Private userFunctions As New clsUserDefinedFunctions
    Private util As New clsTablesUpdateUtils
    Public da As clsDataAccess = clsDataAccess.getObject
    Public worker As System.ComponentModel.BackgroundWorker
    Private evenEpisod As New clsEventEpisode
    Public objRef As clsformrefrences = clsformrefrences.getObject

    Private Shared objSingle As clsDataTransfer
    Private Shared blCreated As Boolean
#End Region

#Region "Singleton function"
    Private Sub New()
        'Override the default constructor
    End Sub
    Public Shared Function getObject() As clsDataTransfer
        If blCreated = False Then
            objSingle = New clsDataTransfer
            blCreated = True

            Return objSingle
        Else
            Return objSingle
        End If
    End Function
#End Region

#Region "STATIC OBJECTS"
    '    STATIC
    'DSS.Regions
    'DSS.villages
    'DSS.individual
    Private Function transfer_Individual(ByVal sourceConn As SqlClient.SqlConnection, ByVal destinationConn As SqlConnection _
             , ByVal sourceQuery As String) As String
        Dim returnMessage As String = ""
        Dim SourceDad As New SqlClient.SqlDataAdapter
        Dim SourceTable As New DataTable
        SourceDad.SelectCommand = New SqlCommand(sourceQuery, sourceConn)
        SourceDad.Fill(SourceTable)
        Try
            If (SourceTable.Rows.Count > 0) Then
                For Each sourceRow As DataRow In SourceTable.Rows
                    
                    'if submitted delete source
                    If clsUserDefinedFunctions.isValidIndividid("") Then

                        Me.da.saveError(sourceRow("transit_id").ToString, "DSS.individual", "individidid  " + sourceRow("individid").ToString + "  is invalid id", "", Now(), "", da.getrecordsCompound("dss.individual", sourceRow).Trim, da.getrecordsRound("dss.individual", sourceRow).Trim())
                        Me.da.exec_nonqueryInTEMPDB("UPDATE DSS.individual SET [errflag] = 'true' , errdate=getdate() where transit_id =" + sourceRow("transit_id").ToString)
                    Else
                        If Me.util.newIndividual(sourceRow) Then
                            If Me.util.DeleteSpecialStudRecord(sourceRow("transit_id"), "DSS.individual", Me.util.currentTransaction) Then
                                ' If Me.util.generic_delete(sourceRow, SourceTable, "[DSS].[individual]", Me.util.currentTransaction) Then
                                'MsgBox("delete")
                            Else
                                MsgBox("no delet")
                            End If
                        End If
                    End If


                Next
            Else
                returnMessage = "DSS.individual: NO_PDA_RECORDS"
            End If
        Catch ex As Exception

            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            ' MsgBox(ex.Message)
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return returnMessage
    End Function

    'DSS.compounds
    Private Function transfer_compounds(ByVal sourceConn As SqlClient.SqlConnection, ByVal destinationConn As SqlConnection, _
             ByVal sourceQuery As String) As String
        Dim returnMessage As String = ""
        Dim SourceDad As New SqlClient.SqlDataAdapter
        Dim SourceTable As New DataTable
        SourceDad.SelectCommand = New SqlCommand(sourceQuery, sourceConn)
        SourceDad.Fill(SourceTable)
        Try
            If (SourceTable.Rows.Count > 0) Then
                For Each sourceRow As DataRow In SourceTable.Rows
                    'if submitted delete source
                    If Me.util.newCompound(sourceRow) Then
                        'If Me.util.generic_delete(sourceRow, SourceTable, "[DSS].[compounds]", Me.util.currentTransaction) Then
                        If Me.util.DeleteSpecialStudRecord(sourceRow("transit_id"), "DSS.compounds", Me.util.currentTransaction) Then

                        End If
                    End If

                Next
            Else
                returnMessage = "DSS.compounds: NO_PDA_RECORDS"
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            ' MsgBox(ex.Message)
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return returnMessage
    End Function
    'DSS.location
    Private Function transfer_locations(ByVal sourceConn As SqlClient.SqlConnection, ByVal destinationConn As SqlConnection, _
             ByVal sourceQuery As String) As String
        Dim returnMessage As String = ""
        Dim icount As Integer = 0
        Dim SourceDad As New SqlClient.SqlDataAdapter
        Dim SourceTable As New DataTable
        SourceDad.SelectCommand = New SqlCommand(sourceQuery, sourceConn)
        SourceDad.Fill(SourceTable)
        Try
            If (SourceTable.Rows.Count > 0) Then
                For Each sourceRow As DataRow In SourceTable.Rows
                    'if submitted delete source
                    If Me.util.newLocation(sourceRow) Then
                        'If Me.util.generic_delete(sourceRow, SourceTable, "[DSS].[location]", Me.util.currentTransaction) Then
                        If Me.util.DeleteSpecialStudRecord(sourceRow("transit_id"), "DSS.location", Me.util.currentTransaction) Then
                        End If
                    End If
                    icount = icount + 1
                    returnMessage = icount.ToString
                Next
            Else
                returnMessage = "DSS.location: NO_PDA_RECORDS"
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            ' MsgBox(ex.Message)
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return returnMessage
    End Function
    'DSS.socialgroup
    Private Function transfer_socialgroup(ByVal sourceConn As SqlClient.SqlConnection, ByVal destinationConn As SqlConnection, _
            ByVal sourceQuery As String) As String
        Dim returnMessage As String = ""
        Dim SourceDad As New SqlClient.SqlDataAdapter
        Dim SourceTable As New DataTable
        SourceDad.SelectCommand = New SqlCommand(sourceQuery, sourceConn)
        SourceDad.Fill(SourceTable)
        Try
            If (SourceTable.Rows.Count > 0) Then
                For Each sourceRow As DataRow In SourceTable.Rows
                    'if submitted delete source
                    If Me.util.addSocialGroup(sourceRow) Then

                        'If Me.util.generic_delete(sourceRow, SourceTable, "[DSS].[socialgroup]", Me.util.currentTransaction) Then
                        If Me.util.DeleteSpecialStudRecord(sourceRow("transit_id"), "DSS.socialgroup", Me.util.currentTransaction) Then
                        End If
                    End If

                Next
            Else
                returnMessage = "DSS.socialgroup: NO_PDA_RECORDS"
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            ' MsgBox(ex.Message)
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return returnMessage
    End Function
    Private Function transfer_SocialGroupupdates(ByVal sourceConn As SqlClient.SqlConnection, ByVal destinationConn As SqlConnection _
             , ByVal sourceQuery As String) As String
        Dim returnMessage As String = ""
        Dim SourceDad As New SqlClient.SqlDataAdapter
        Dim SourceTable As New DataTable
        SourceDad.SelectCommand = New SqlCommand(sourceQuery, sourceConn)
        SourceDad.Fill(SourceTable)
        Try
            If (SourceTable.Rows.Count > 0) Then
                For Each sourceRow As DataRow In SourceTable.Rows
                    'if submitted delete source
                    If Me.util.updateSocialGroup( _
                                 sourceRow("socialgpid"), _
                                 sourceRow("location")) Then
                        If Me.util.DeleteSpecialStudRecord(sourceRow("transit_id"), "dss.socialgroup", Me.util.currentTransaction) Then

                        Else

                        End If
                    End If

                Next
            Else
                returnMessage = "NO_PDA_RECORDS"
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            ' MsgBox(ex.Message)
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return returnMessage
    End Function
#End Region

#Region "FOR STATIC"
    'DSS.round
    'DSS.observation
    Private Function transfer_observation(ByVal sourceConn As SqlClient.SqlConnection, ByVal destinationConn As SqlConnection, _
            ByVal sourceQuery As String) As String
        Dim returnMessage As String = ""
        Dim SourceDad As New SqlClient.SqlDataAdapter
        Dim SourceTable As New DataTable
        SourceDad.SelectCommand = New SqlCommand(sourceQuery, sourceConn)
        SourceDad.Fill(SourceTable)
        Try
            If (SourceTable.Rows.Count > 0) Then
                For Each sourceRow As DataRow In SourceTable.Rows
                    'if submitted delete source
                    If Me.util.addObservation(sourceRow) Then
                        'If Me.util.generic_delete(sourceRow, SourceTable, "[DSS].[observation]", Me.util.currentTransaction) Then
                        If Me.util.DeleteSpecialStudRecord(sourceRow("transit_id"), "DSS.observation", Me.util.currentTransaction) Then
                        End If
                    End If

                Next
            Else
                returnMessage = "NO_PDA_RECORDS"
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            ' MsgBox(ex.Message)
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return returnMessage
    End Function
    'DSS.SocialGroupadmin
    Private Function transfer_SocialGroupadmin(ByVal sourceConn As SqlClient.SqlConnection, ByVal destinationConn As SqlConnection, _
            ByVal sourceQuery As String) As String
        Dim returnMessage As String = ""
        Dim SourceDad As New SqlClient.SqlDataAdapter
        Dim SourceTable As New DataTable
        SourceDad.SelectCommand = New SqlCommand(sourceQuery, sourceConn)
        SourceDad.Fill(SourceTable)
        Try
            If (SourceTable.Rows.Count > 0) Then
                For Each sourceRow As DataRow In SourceTable.Rows
                    'if submitted delete source
                    If Me.util.addSocialGroupAdmin(sourceRow) Then
                        ' If Me.util.generic_delete(sourceRow, SourceTable, "[DSS].[SocialGroupadmin]", Me.util.currentTransaction) Then
                        If Me.util.DeleteSpecialStudRecord(sourceRow("transit_id"), "DSS.SocialGroupadmin", Me.util.currentTransaction) Then
                        End If
                    End If

                Next
            Else
                returnMessage = "NO_PDA_RECORDS"
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            ' MsgBox(ex.Message)
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return returnMessage
    End Function
    'DSS.compadmin
    Private Function transfer_compadmin(ByVal sourceConn As SqlClient.SqlConnection, ByVal destinationConn As SqlConnection, _
            ByVal sourceQuery As String) As String
        Dim returnMessage As String = ""
        Dim SourceDad As New SqlClient.SqlDataAdapter
        Dim SourceTable As New DataTable
        SourceDad.SelectCommand = New SqlCommand(sourceQuery, sourceConn)
        SourceDad.Fill(SourceTable)
        Try
            If (SourceTable.Rows.Count > 0) Then
                For Each sourceRow As DataRow In SourceTable.Rows
                    'if submitted delete source
                    If Me.util.addCompAdmin(sourceRow) Then
                        'If Me.util.generic_delete(sourceRow, SourceTable, "[DSS].[compadmin]", Me.util.currentTransaction) Then
                        If Me.util.DeleteSpecialStudRecord(sourceRow("transit_id"), "DSS.compadmin", Me.util.currentTransaction) Then
                        End If
                    End If

                Next
            Else
                returnMessage = "NO_PDA_RECORDS"
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            ' MsgBox(ex.Message)
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return returnMessage
    End Function

#End Region
    'MHRS_SYS.Changes
#Region "Changes and correction"
    'MHRS_SYS.Changes
    Private Function transfer_changes(ByVal sourceConn As SqlClient.SqlConnection, ByVal destinationConn As SqlConnection _
             , ByVal sourceQuery As String) As String
        Dim returnMessage As String = ""
        Dim SourceDad As New SqlClient.SqlDataAdapter
        Dim SourceTable As New DataTable
        Dim newdobchanges As New Date()
        SourceDad.SelectCommand = New SqlCommand(sourceQuery, sourceConn)
        SourceDad.Fill(SourceTable)
        Try
            If (SourceTable.Rows.Count > 0) Then
                For Each sourceRow As DataRow In SourceTable.Rows
                    Try

                   
                        'if submitted delete source
                        Select Case sourceRow("tablename").ToString.ToLower.Trim
                            Case "dss.residency".ToLower.Trim


                                If Me.util.updateResidency( _
                                      sourceRow("recordid").ToString, _
                                      sourceRow("colname"), _
                                      sourceRow("NewValue")) Then
                                    If Me.util.DeleteSpecialStudRecord(sourceRow("transit_id"), "MHRS_SYS.Changes", Me.util.currentTransaction) Then
                                        'MsgBox("delete changes")
                                    Else
                                        ' MsgBox("no delet changes")
                                    End If
                                End If
                            Case "dss.individual".ToLower.Trim
                                If sourceRow("colname").ToString.Trim.ToLower.Trim = "dob" Then
                                    'Dim month As String = sourceRow("NewValue").ToString.Trim.ToLower.Split("/")(0)
                                    'Dim day As String = sourceRow("NewValue").ToString.Trim.ToLower.Split("/")(1)
                                    'Dim year As String = sourceRow("NewValue").ToString.Trim.ToLower.Split("/")(2)
                                    'If year.Length < 3 Then
                                    '    If CInt(year) < 12 Then
                                    '        year = "20" + year
                                    '    Else
                                    '        year = "19" + year
                                    '    End If
                                    'End If



                                    newdobchanges = DateTime.Parse(sourceRow("NewValue").ToString.Trim.ToLower) ', "dd-MMM-yyyy", Globalization.DateTimeStyles.None) ' New Date(CInt(year), CInt(month), CInt(day))
                                    If Me.util.updateIndividual( _
                                                                                                     sourceRow("recordid"), _
                                                                                                     sourceRow("colname"), _
                                                                                                     newdobchanges) Then
                                        If Me.util.DeleteSpecialStudRecord(sourceRow("transit_id"), "MHRS_SYS.Changes", Me.util.currentTransaction) Then
                                            'MsgBox("delete changes")
                                        Else
                                            'MsgBox("no delet changes")
                                        End If
                                    End If
                                Else
                                    If Me.util.updateIndividual( _
                                                                     sourceRow("recordid"), _
                                                                     sourceRow("colname"), _
                                                                     sourceRow("NewValue")) Then
                                        If Me.util.DeleteSpecialStudRecord(sourceRow("transit_id"), "MHRS_SYS.Changes", Me.util.currentTransaction) Then
                                            'MsgBox("delete changes")
                                        Else
                                            'MsgBox("no delet changes")
                                        End If
                                    End If
                                End If
                            Case "specialstudies.parentsurv".ToLower.Trim
                                If Me.util.updateParentalSurvival( _
                                                                                                     sourceRow("recordid"), _
                                                                                                     sourceRow("colname"), _
                                                                                                     sourceRow("NewValue")) Then
                                    If Me.util.DeleteSpecialStudRecord(sourceRow("transit_id"), "MHRS_SYS.Changes", Me.util.currentTransaction) Then
                                        'MsgBox("delete changes")
                                    Else
                                        'MsgBox("no delet changes")
                                    End If
                                End If
                        End Select
                    Catch ex As Exception
                        returnMessage = returnMessage + vbCrLf + ex.Message
                    End Try
                Next
            Else
                returnMessage = "NO_PDA_RECORDS"
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()
            'MsgBox(ex.Message)
            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            ' MsgBox(ex.Message)
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return returnMessage
    End Function
#End Region
    'EPISODES INSERTS
#Region "EPISODES INSERTS "
    'DSS.residency
    Private Function transfer_residency_NewEpisodes(ByVal sourceConn As SqlClient.SqlConnection, ByVal destinationConn As SqlConnection, _
            ByVal sourceQuery As String) As String
        Dim returnMessage As String = ""
        Dim SourceDad As New SqlClient.SqlDataAdapter
        Dim SourceTable As New DataTable
        SourceDad.SelectCommand = New SqlCommand(sourceQuery, sourceConn)
        SourceDad.Fill(SourceTable)
        Try
            If (SourceTable.Rows.Count > 0) Then
                For Each sourceRow As DataRow In SourceTable.Rows
                    'if submitted delete source
                    If Me.util.startResidency(sourceRow) Then
                        'If Me.util.generic_delete(sourceRow, SourceTable, "[DSS].[residency]", Me.util.currentTransaction) Then

                        'End If
                        If Me.util.DeleteSpecialStudRecord(sourceRow("transit_id"), " [DSS].[residency]") Then
                            'MsgBox("delete")
                        Else
                            'MsgBox("no delet")
                        End If
                    End If
                Next
            Else
                returnMessage = "NO_PDA_RECORDS"
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            ' MsgBox(ex.Message)
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return returnMessage
    End Function
    'DSS.pregnancy
    Private Function transfer_pregnancy_NewEpisodes(ByVal sourceConn As SqlClient.SqlConnection, ByVal destinationConn As SqlConnection, _
               ByVal sourceQuery As String) As String
        Dim returnMessage As String = ""
        Dim SourceDad As New SqlClient.SqlDataAdapter
        Dim SourceTable As New DataTable
        SourceDad.SelectCommand = New SqlCommand(sourceQuery, sourceConn)
        SourceDad.Fill(SourceTable)
        Try
            If (SourceTable.Rows.Count > 0) Then
                For Each sourceRow As DataRow In SourceTable.Rows
                    'if submitted delete source
                    If Me.util.startPregnancy(sourceRow) Then
                        'If Me.util.generic_delete(sourceRow, SourceTable, "[DSS].[pregnancy]", Me.util.currentTransaction) Then

                        'End If
                        If Me.util.DeleteSpecialStudRecord(sourceRow("transit_id"), " [DSS].[pregnancy]") Then
                            'MsgBox("delete")
                        Else
                            'MsgBox("no delet")
                        End If
                    End If

                Next
            Else
                returnMessage = "NO_PDA_RECORDS"
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            ' MsgBox(ex.Message)
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return returnMessage
    End Function
    'DSS.membership
    Private Function transfer_membership_NewEpisodes(ByVal sourceConn As SqlClient.SqlConnection, ByVal destinationConn As SqlConnection, _
                  ByVal sourceQuery As String) As String
        Dim returnMessage As String = ""
        Dim SourceDad As New SqlClient.SqlDataAdapter
        Dim SourceTable As New DataTable
        SourceDad.SelectCommand = New SqlCommand(sourceQuery, sourceConn)
        SourceDad.Fill(SourceTable)
        Try
            If (SourceTable.Rows.Count > 0) Then
                For Each sourceRow As DataRow In SourceTable.Rows
                    'if submitted delete source
                    If Me.util.startMembership(sourceRow) Then
                        'If Me.util.generic_delete(sourceRow, SourceTable, "[DSS].[membership]", Me.util.currentTransaction) Then

                        'End If
                        If Me.util.DeleteSpecialStudRecord(sourceRow("transit_id"), " [DSS].[membership]") Then
                            'MsgBox("delete")
                        Else
                            'MsgBox("no delet")
                        End If
                    End If

                Next
            Else
                returnMessage = "NO_PDA_RECORDS"
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            ' MsgBox(ex.Message)
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return returnMessage
    End Function

#End Region
    'EPISODES UPDATES
#Region "EPISODES UPDATES "
    'DSS.residency
    'DSS.pregnancy
    'DSS.membership

    'DSS.residency
    Private Function transfer_residency_UPDATES(ByVal sourceConn As SqlClient.SqlConnection, ByVal destinationConn As SqlConnection, _
            ByVal sourceQuery As String) As String
        Dim returnMessage As String = ""
        Dim SourceDad As New SqlClient.SqlDataAdapter
        Dim SourceTable As New DataTable
        SourceDad.SelectCommand = New SqlCommand(sourceQuery, sourceConn)
        SourceDad.Fill(SourceTable)
        Try
            If (SourceTable.Rows.Count > 0) Then
                For Each sourceRow As DataRow In SourceTable.Rows
                    'get the lastest epi
                    If Me.util.endResidency(sourceRow) Then
                        'if submitted delete source
                        'If Me.util.generic_delete(sourceRow, SourceTable, "[DSS].[residency]", Me.util.currentTransaction) Then

                        'End If
                        If Me.util.DeleteSpecialStudRecord(sourceRow("transit_id"), " [DSS].[residency]") Then
                            'MsgBox("delete")
                        Else
                            'MsgBox("no delet")
                        End If
                    End If
                Next
            Else
                returnMessage = "NO_PDA_RECORDS"
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            ' MsgBox(ex.Message)
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return returnMessage
    End Function
    'DSS.pregnancy
    Private Function transfer_pregnancy_UPDATES(ByVal sourceConn As SqlClient.SqlConnection, ByVal destinationConn As SqlConnection, _
               ByVal sourceQuery As String) As String
        Dim returnMessage As String = ""
        Dim SourceDad As New SqlClient.SqlDataAdapter
        Dim SourceTable As New DataTable
        SourceDad.SelectCommand = New SqlCommand(sourceQuery, sourceConn)
        SourceDad.Fill(SourceTable)
        Try
            If (SourceTable.Rows.Count > 0) Then
                For Each sourceRow As DataRow In SourceTable.Rows
                    'if submitted delete source
                    If Me.util.endPregnancy(sourceRow) Then
                        'If Me.util.generic_delete(sourceRow, SourceTable, "[DSS].[pregnancy]", Me.util.currentTransaction) Then

                        'End If
                        If Me.util.DeleteSpecialStudRecord(sourceRow("transit_id"), " [DSS].[pregnancy]") Then
                            'MsgBox("delete")
                        Else
                            'MsgBox("no delet")
                        End If
                    End If

                Next
            Else
                returnMessage = "NO_PDA_RECORDS"
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            ' MsgBox(ex.Message)
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return returnMessage
    End Function
    'DSS.membership
    Private Function transfer_membership_UPDATES(ByVal sourceConn As SqlClient.SqlConnection, ByVal destinationConn As SqlConnection, _
                  ByVal sourceQuery As String) As String
        Dim returnMessage As String = ""
        Dim SourceDad As New SqlClient.SqlDataAdapter
        Dim SourceTable As New DataTable
        SourceDad.SelectCommand = New SqlCommand(sourceQuery, sourceConn)
        SourceDad.Fill(SourceTable)
        Try
            If (SourceTable.Rows.Count > 0) Then
                For Each sourceRow As DataRow In SourceTable.Rows
                    'if submitted delete source
                    If Me.util.endMembership(sourceRow) Then
                        'If Me.util.generic_delete(sourceRow, SourceTable, "[DSS].[membership]", Me.util.currentTransaction) Then

                        'End If
                        If Me.util.DeleteSpecialStudRecord(sourceRow("transit_id"), " [DSS].[membership]") Then
                            'MsgBox("delete")
                        Else
                            'MsgBox("no delet")
                        End If
                    End If

                Next
            Else
                returnMessage = "NO_PDA_RECORDS"
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            ' MsgBox(ex.Message)
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return returnMessage
    End Function
#End Region
    'EVENT
#Region "EVENT"
    'DSS.Migrations
    Private Function transfer_Migrations(ByVal sourceConn As SqlClient.SqlConnection, ByVal destinationConn As SqlConnection, _
                  ByVal sourceQuery As String) As String
        Dim returnMessage As String = ""
        Dim SourceDad As New SqlClient.SqlDataAdapter
        Dim SourceTable As New DataTable
        SourceDad.SelectCommand = New SqlCommand(sourceQuery, sourceConn)
        SourceDad.Fill(SourceTable)
        Try
            If (SourceTable.Rows.Count > 0) Then
                For Each sourceRow As DataRow In SourceTable.Rows
                    'if submitted delete source
                    If Me.util.addMigration(sourceRow) Then
                        If Me.util.generic_delete(sourceRow, SourceTable, "[DSS].[Migrations]", Me.util.currentTransaction) Then

                        End If
                    End If

                Next
            Else
                returnMessage = "NO_PDA_RECORDS"
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            ' MsgBox(ex.Message)
            ''emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return returnMessage
    End Function
    'DSS.birth
    Private Function transfer_birth(ByVal sourceConn As SqlClient.SqlConnection, ByVal destinationConn As SqlConnection, _
                  ByVal sourceQuery As String) As String
        Dim returnMessage As String = ""
        Dim SourceDad As New SqlClient.SqlDataAdapter
        Dim SourceTable As New DataTable
        SourceDad.SelectCommand = New SqlCommand(sourceQuery, sourceConn)
        SourceDad.Fill(SourceTable)
        Try
            If (SourceTable.Rows.Count > 0) Then
                For Each sourceRow As DataRow In SourceTable.Rows
                    'if submitted delete source
                    If Me.util.addBirth(sourceRow) Then
                        If Me.util.generic_delete(sourceRow, SourceTable, "[DSS].[birth]", Me.util.currentTransaction) Then

                        End If
                    End If

                Next
            Else
                returnMessage = "NO_PDA_RECORDS"
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            ' MsgBox(ex.Message)
            ''emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return returnMessage
    End Function
    'DSS.pregoutcome
    Private Function transfer_pregoutcome(ByVal sourceConn As SqlClient.SqlConnection, ByVal destinationConn As SqlConnection, _
                     ByVal sourceQuery As String) As String
        Dim returnMessage As String = ""
        Dim SourceDad As New SqlClient.SqlDataAdapter
        Dim SourceTable As New DataTable
        SourceDad.SelectCommand = New SqlCommand(sourceQuery, sourceConn)
        SourceDad.Fill(SourceTable)
        Try
            If (SourceTable.Rows.Count > 0) Then
                For Each sourceRow As DataRow In SourceTable.Rows
                    'if submitted delete source
                    If Me.util.addPregnancyOutcome(sourceRow) Then
                        If Me.util.generic_delete(sourceRow, SourceTable, "[DSS].[pregoutcome]", Me.util.currentTransaction) Then

                        End If
                    End If

                Next
            Else
                returnMessage = "NO_PDA_RECORDS"
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            ' MsgBox(ex.Message)
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return returnMessage
    End Function
    'DSS.indvstatus
    Private Function transfer_indvstatus(ByVal sourceConn As SqlClient.SqlConnection, ByVal destinationConn As SqlConnection, _
                     ByVal sourceQuery As String) As String
        Dim returnMessage As String = ""
        Dim SourceDad As New SqlClient.SqlDataAdapter
        Dim SourceTable As New DataTable
        SourceDad.SelectCommand = New SqlCommand(sourceQuery, sourceConn)
        SourceDad.Fill(SourceTable)
        Try
            If (SourceTable.Rows.Count > 0) Then
                For Each sourceRow As DataRow In SourceTable.Rows
                    'if submitted delete source
                    If Me.util.addindvstatus(sourceRow) Then
                        If Me.util.generic_delete(sourceRow, SourceTable, "[DSS].[indvstatus]", Me.util.currentTransaction) Then

                        End If
                    End If

                Next
            Else
                returnMessage = "NO_PDA_RECORDS"
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            ' MsgBox(ex.Message)
            ''emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return returnMessage
    End Function
    'DSS.Events_Episodes
    Private Function transfer_Events_Episodes(ByVal sourceConn As SqlClient.SqlConnection, ByVal destinationConn As SqlConnection, _
                    ByVal sourceQuery As String) As String
        Dim returnMessage As String = ""
        Dim SourceDad As New SqlClient.SqlDataAdapter
        Dim SourceTable As New DataTable
        SourceDad.SelectCommand = New SqlCommand(sourceQuery, sourceConn)
        SourceDad.Fill(SourceTable)
        Try
            If (SourceTable.Rows.Count > 0) Then
                For Each sourceRow As DataRow In SourceTable.Rows

                    'if submitted delete source
                    If Me.util.newEventEpisode(sourceRow) Then
                        If Me.util.generic_delete(sourceRow, SourceTable, "[DSS].[Events_Episodes]", Me.util.currentTransaction) Then

                        End If
                    End If

                Next
            Else
                returnMessage = "NO_PDA_RECORDS"
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            ' MsgBox(ex.Message)
            ''emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return returnMessage
    End Function
    'DSS.marriage
    Private Function transfer_marriage(ByVal sourceConn As SqlClient.SqlConnection, ByVal destinationConn As SqlConnection, _
                     ByVal sourceQuery As String) As String
        Dim returnMessage As String = ""
        Dim SourceDad As New SqlClient.SqlDataAdapter
        Dim SourceTable As New DataTable
        SourceDad.SelectCommand = New SqlCommand(sourceQuery, sourceConn)
        SourceDad.Fill(SourceTable)
        Try
            If (SourceTable.Rows.Count > 0) Then
                For Each sourceRow As DataRow In SourceTable.Rows
                    'if submitted delete source
                    If Me.util.addMarriage(sourceRow) Then
                        If Me.util.generic_delete(sourceRow, SourceTable, "[DSS].[marriage]", Me.util.currentTransaction) Then

                        End If
                    End If

                Next
            Else
                returnMessage = "NO_PDA_RECORDS"
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            ' MsgBox(ex.Message)
            ''emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        Return returnMessage
    End Function
#End Region
    'SPECIAL STUDIES
#Region "SPECIAL STUDIES"
    'DSS.visitation
    'SpecialStudies.health
    'SpecialStudies.GPSDATA
    'SpecialStudies.hsedetails
    'SpecialStudies.HUAS_LITE
    'SpecialStudies.immunize
    'SpecialStudies.ses
    'SpecialStudies.education
    'SpecialStudies.Ethnicity
    'SpecialStudies.itn
    'SpecialStudies.morbidity
    'SpecialStudies.parentsurv
    'SpecialStudies.Religion
    '            "SpecialStudies.immunize", _
    Private specialStudies As String() = _
            { _
            "DSS.visitation", _
            "SpecialStudies.health", _
            "SpecialStudies.GPSDATA", _
            "SpecialStudies.hsedetails", _
            "SpecialStudies.HUAS_LITE", _
            "SpecialStudies.ses", _
            "SpecialStudies.education", _
            "SpecialStudies.itn", _
            "SpecialStudies.morbidity", _
            "SpecialStudies.parentsurv", _
            "SpecialStudies.circumcision", _
            "specialstudies.druguse", _
            "specialstudies.contraception", _
            "specialstudies.dmicampaign", _
            "specialstudies.Reprodactivehealth", _
            "specialstudies.mobilephoneusage", _
            "specialstudies.stove", _
            "SpecialStudies.Religion", _
            "ghi.Toilet", _
            "ghi.pregnancy_and_Birth", _
            "ghi.House_sanitation", _
            "ghi.Child_health", _
            "ghi.ghi_itn", _
            "ghi.family_planning", _
            "ghi.vct_hiv", _
            "ghi.Relationships", _
            "ghi.Diarrhea_treat", _
            "ghi.fp_current_use", _
            "ghi.Fever_treatment", _
            "ghi.fever_other", _
            "ghi.Fever_drug", _
            "ghi.baby_drink", _
            "ghi.anc_place", _
            "ghi.afterdelivery_service", _
            "ghi.Treat_water", _
            "specialstudies.bednet", _
            "specialstudies.bednet_individual_netUse", _
            "specialstudies.bednet_netinfo", _
            "specialStudies.BreastFeedingKnowledge", _
            "specialStudies.LiveStock", _
            "specialStudies.Cropgrown", _
            "specialstudies.Crop_Live_production", _
            "specialStudies.HHD_Waterusage", _
            "specialStudies.FetchWater_Member", _
            "specialStudies.WaterAccess", _
            "specialStudies.WaterAccess_Activities", _
            "specialStudies.CleanWater_Methods", _
            "specialStudies.Income", _
            "specialStudies.FoodFreq", _
            "specialStudies.FoodSecurity", _
            "MS.WASHSCHOOL", _
            "MS.WASHLatrine", _
            "MS.WASHContainers", _
            "MS.WASHLatrineHome", _
            "MS.WASHContainersHome", _
            "MS.PEDSQL23", _
            "MS.PEDSQL7", _
            "MS.WASHHOME", _
            "MS.ENROLLMENT", _
            "MS.CLIPBOARD", _
            "MS.CONSENT", _
            "MS.ENROLLMENT", _
            "MS.MS_MEMBERSHIP", _
            "MS.WASHFollowUp", _
            "SpecialStudies.EVP", _
            "SpecialStudies.EVP_Anthropometric", _
            "SpecialStudies.EVPinterviewOutcome", _
            "SpecialStudies.immunize", _
            "SpecialStudies.SocialGroupSurvey", _
            "BH.BIRTHHISTORY", _
            "BH.CHILDREN", _
            "PBR.ANC", _
            "PBR.Birth_Delivery", _
            "PBR.afterdelivery_service", _
            "PBR.anc", _
            "PBR.anc_place"}

    Private Function transfer_specialStudies() As String
        Dim str As String = vbCrLf
        For Each obj As String In Me.specialStudies
            Try
               
                worker.ReportProgress(Nothing, " starting  validating and uploading " & obj.Trim & " " & Now.ToString())
                Me.foreignKeys.ValidateforeignKey_Table_inTEMP_DSSHRS(obj.Trim, "not(rec_status like '%x%')")
                If obj.ToLower.Trim = "SpecialStudies.immunize".ToLower.Trim Then
                    Me.da.exec_nonqueryInTEMPDB("exec [TEMP_DSSHRS].[dbo].[re_flag_immunizedRecs]")
                End If
                str = str & vbCrLf & Me.CopyTempDB_To_MAINDB(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, obj.Trim, obj.Trim, _
                           "select * from " + obj + " where errflag=0 and not(rec_status like '%x%')")
                worker.ReportProgress(Nothing, " finished  validating and uploading " & obj.Trim & " " & Now.ToString())
            Catch ex As Exception
                objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

                'Throw (ex)
                'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

                ' MsgBox(ex.Message)
                'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
            End Try
        Next
        Return str
    End Function
    Private eventsStudies As String() = _
           {"DSS.Migrations", _
           "DSS.birth", _
           "DSS.pregoutcome", _
           "DSS.indvstatus", _
           "DSS.Events_Episodes", _
           "DSS.marriage", _
           "DSS.NationalID"}
    Private Function transfer_events() As String
        Dim str As String = vbCrLf
        For Each obj As String In Me.eventsStudies
            Try
                worker.ReportProgress(Nothing, " starting  validating and uploading " & obj.Trim & " " & Now.ToString())
                Me.foreignKeys.ValidateforeignKey_Table_inTEMP_DSSHRS(obj.Trim, "not(rec_status like '%x%')")
                str = str & vbCrLf & Me.CopyTempDB_To_MAINDB(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, obj.Trim, obj.Trim, _
                           "select * from " + obj + " where errflag=0 and not(rec_status like '%x%')")

                worker.ReportProgress(Nothing, " finished  validating and uploading " & obj.Trim & " " & Now.ToString())

            Catch ex As Exception
                objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

                'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

                ' MsgBox(ex.Message)
                'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
            End Try
        Next
        Return str
    End Function
    Private Function CopyTempDB_To_MAINDB(ByVal sourceConn As SqlClient.SqlConnection, ByVal destinationConn As SqlConnection, ByVal sourceTableName As String, ByVal destTableName As String _
       , ByVal sourceQuery As String) As String
        Dim returnMessage As String = ""
        Dim destinationDad As New SqlClient.SqlDataAdapter
        Dim SourceDad As New SqlClient.SqlDataAdapter
        Dim destinationTable As New DataTable
        Dim SourceTable As New DataTable
        Dim destCol As DataColumn
        Dim recaffected As Integer = 0
        Dim sourceCmb As SqlCommandBuilder = New SqlCommandBuilder(SourceDad)
        Dim destinationCmb As SqlCommandBuilder = New SqlCommandBuilder(destinationDad)
        SourceDad.SelectCommand = New SqlCommand(sourceQuery, sourceConn)
        SourceDad.Fill(SourceTable)
        destinationDad.SelectCommand = New SqlCommand("SELECT TOP 1 * FROM " + destTableName + "", destinationConn)
        Try
            destinationDad.Fill(destinationTable)
            If (SourceTable.Rows.Count > 0) Then
                For Each sourceRow As DataRow In SourceTable.Rows
                    Try
                        destinationTable.RejectChanges()
                        Dim destinationRow As DataRow = destinationTable.NewRow()
                        ' Dim dataItem, dataItemCopied As String
                        For Each destCol In destinationTable.Columns
                            If destCol.ColumnName.ToLower = "rec_status" Then
                                destinationRow(destCol.ColumnName) = "V"
                            Else
                                destinationRow(destCol.ColumnName) = sourceRow(destCol.ColumnName)
                            End If
                        Next
                        destinationTable.Rows.Add(destinationRow)
                        If destinationDad.Update(destinationTable) > 0 Then
                            recaffected = recaffected + 1
                            'sourceRow.Delete()
                            'SourceDad.Update(SourceTable)
                            If Me.util.DeleteSpecialStudRecord(sourceRow("transit_id"), sourceTableName) Then
                                'MsgBox("delete")
                            Else
                                'MsgBox("no delet")
                            End If
                        Else
                            destinationTable.RejectChanges()
                        End If
                    Catch ex As Exception
                        objRef.strObjMethod = New Diagnostics.StackTrace().ToString()
                        returnMessage = returnMessage + vbCrLf + sourceTableName + vbCrLf + vbCrLf + ex.Message
                    End Try

                Next
                returnMessage = recaffected
            Else
                returnMessage = destTableName & ": NO_PDA_RECORDS"
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            ' MsgBox(ex.Message)
            sourceConn.Close()
            returnMessage = returnMessage + vbCrLf + sourceTableName + vbCrLf + ex.Message
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        sourceConn.Close()
        destinationConn.Close()
        Return destTableName + ": " + returnMessage
    End Function
#End Region
    'the ui
#Region "Laptop to server codes"
    Public Sub copyDataFromNetBook(ByVal lbLogWindow As ListBox, ByVal dsssercon As SqlConnection)
        Dim rs As String = ""
        Dim sql As String = "SELECT distinct DOWNLOADTABLE FROM [TEMP_DSSHRS].[dbo].[DownloadActions]" _
                                   & "where  ([ACTION] in ('DClear','DItems','DOWNLOAD')) OR" _
                                   & "(DOWNLOADTABLE  IN ('DSS.SocialGroupadmin','DSS.marriage','DSS.observation'))"
        Dim currentDownloadTableSQL As String = ""
        Dim tablename As String = ""
        'Using dsssercon As New SqlConnection
        'dsssercon.ConnectionString = "Data Source= DSS-KEK2; initial catalog=TEMP_DSSHRS; integrated security=true"
        Dim allDownloadTables As DataTable = Me.getTableData(sql)
        For Each DownloadTable As DataRow In allDownloadTables.Rows
            tablename = DownloadTable("DownloadTable")
            currentDownloadTableSQL = "select * from " + tablename + " where rec_status in('I','U')"
            'add statements to transfter data
            Try
                Me.writeLog(tablename + " : " + Me.CopyNetBook_To_TempDB(clsGlobalVariable.HRS_Temp_DBCon, dsssercon, tablename, tablename, currentDownloadTableSQL), lbLogWindow)
            Catch ex As Exception
                objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

                'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

                ' MsgBox(ex.Message)
                'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
            End Try
        Next
        'End Using

    End Sub
    Private Function CopyNetBook_To_TempDB(ByVal Netbook_Conn As SqlClient.SqlConnection, ByVal DssServer_Conn As SqlConnection, ByVal sourceTableName As String, ByVal destTableName As String _
       , ByVal sourceQuery As String) As String
        Dim returnMessage As String = ""
        Dim DssServer_Dad As New SqlClient.SqlDataAdapter
        Dim Netbook_Dad As New SqlClient.SqlDataAdapter
        Dim DssServer_Table As New DataTable
        Dim Netbook_Table As New DataTable
        ' Dim DssServer_Col As DataColumn
        Dim recaffected As Integer = 0
        Dim sourceCmb As SqlCommandBuilder = New SqlCommandBuilder(Netbook_Dad)
        Dim destinationCmb As SqlCommandBuilder = New SqlCommandBuilder(DssServer_Dad)
        Netbook_Dad.SelectCommand = New SqlCommand(sourceQuery, Netbook_Conn)
        Netbook_Dad.Fill(Netbook_Table)
        DssServer_Dad.SelectCommand = New SqlCommand("SELECT TOP 0 * FROM " + destTableName + "", DssServer_Conn)
        Try
            DssServer_Dad.Fill(DssServer_Table)
            If (Netbook_Table.Rows.Count > 0) Then
                For Each Netbook_Row As DataRow In Netbook_Table.Rows
                    Dim DssServer_Row As DataRow = DssServer_Table.NewRow()
                    ' Dim dataItem, dataItemCopied As String
                    If (Netbook_Row("rec_status").ToString.ToLower = "i") Or (Netbook_Row("rec_status").ToString.ToLower = "u") Then
                        For Each DssServer_Col As DataColumn In DssServer_Table.Columns
                            If DssServer_Col.ColumnName.ToLower <> "transit_id" Then
                                DssServer_Row(DssServer_Col.ColumnName) = Netbook_Row(DssServer_Col.ColumnName)
                            End If
                        Next
                        DssServer_Table.Rows.Add(DssServer_Row)
                        recaffected = recaffected + 1
                        Netbook_Row("rec_status") = "D" + Netbook_Row("rec_status").ToString
                    End If
                Next
                If DssServer_Dad.Update(DssServer_Table) > 0 Then
                    '''''write code here to update laptop
                    If Netbook_Dad.Update(Netbook_Table) > 0 Then
                        'MsgBox("Netbook updated")
                        returnMessage = recaffected.ToString + ": Netbook updated"
                    Else
                        returnMessage = recaffected.ToString
                    End If

                Else
                    returnMessage = "NO_PDA_RECORDS"
                End If
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            ' MsgBox(ex.Message)
            'Netbook_Conn.Close()
            returnMessage = ex.Message
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        'Netbook_Conn.Close()
        'DssServer_Conn.Close()
        Return returnMessage
    End Function
    'validation codes

    Private Function CopyMDF_To_MDF(ByVal sourceConn As SqlClient.SqlConnection, ByVal destinationConn As SqlConnection, ByVal sourceTableName As String, ByVal destTableName As String _
          , ByVal sourceQuery As String) As String
        Dim returnMessage As String = ""
        Dim destinationDad As New SqlClient.SqlDataAdapter
        Dim SourceDad As New SqlClient.SqlDataAdapter
        Dim destinationTable As New DataTable
        Dim SourceTable As New DataTable
        Dim destCol As DataColumn
        Dim recaffected As Integer = 0
        Dim sourceCmb As SqlCommandBuilder = New SqlCommandBuilder(SourceDad)
        Dim destinationCmb As SqlCommandBuilder = New SqlCommandBuilder(destinationDad)
        SourceDad.SelectCommand = New SqlCommand(sourceQuery, sourceConn)
        SourceDad.Fill(SourceTable)
        destinationDad.SelectCommand = New SqlCommand("SELECT TOP 0 * FROM " + destTableName + "", destinationConn)
        Try
            destinationDad.Fill(destinationTable)
            If (SourceTable.Rows.Count > 0) Then
                For Each sourceRow As DataRow In SourceTable.Rows
                    destinationTable.RejectChanges()
                    Dim destinationRow As DataRow = destinationTable.NewRow()
                    ' Dim dataItem, dataItemCopied As String
                    For Each destCol In destinationTable.Columns
                        destinationRow(destCol.ColumnName) = sourceRow(destCol.ColumnName)
                    Next
                    destinationTable.Rows.Add(destinationRow)
                Next
                returnMessage = destinationDad.Update(destinationTable)
            Else
                returnMessage = "NO_PDA_RECORDS"
            End If
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            ' MsgBox(ex.Message)
            sourceConn.Close()
            returnMessage = ex.Message
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        sourceConn.Close()
        destinationConn.Close()
        Return returnMessage
    End Function

    Friend Function getTableData(ByVal query As String) As DataTable
        Dim table As New DataTable
        Dim readb As Data.SqlClient.SqlDataReader
        Dim cmd As New SqlCommand
        cmd.Connection = clsGlobalVariable.HRS_Temp_DBCon
        cmd.CommandText = query
        Try
            clsGlobalVariable.open_HRS_TEMP_DBCon()
            readb = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            table.Load(readb)
        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            ' MsgBox(ex.Message)
            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
        If clsGlobalVariable.HRS_Temp_DBCon.State <> ConnectionState.Closed Then
            clsGlobalVariable.HRS_Temp_DBCon.Close()
        End If
        Return table
    End Function
    Private Sub writeLog(ByVal message As String, ByVal lbLogWindow As ListBox)
        Try
            lbLogWindow.Items.Add(message)
            lbLogWindow.SelectedItem = lbLogWindow.Items.Item(lbLogWindow.Items.Count - 1)

        Catch ex As Exception
            objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

            'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

            'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
        End Try
    End Sub
#End Region
    'Main data transfer codes
#Region "Data transfer for TEMP_DSSHRS"
    Public Sub configureTrigger(ByVal dblevel As datalevel, ByVal toenable As Boolean)
        Dim table As New DataTable
        Select Case dblevel
            Case datalevel.DSSHRS
                table = Me.da.getTableDataFromMAINDB("SELECT [disable_script],[enable_script]FROM [DSSHRS].[dbo].[TriggerControl]")
            Case datalevel.TEMP_DSSHRS
                table = Me.da.getTableDataFromMAINDB("SELECT [disable_script],[enable_script]FROM [TEMP_DSSHRS].[dbo].[TriggerControl]")
        End Select
        For Each obj As DataRow In table.Rows
            If toenable Then
                If dblevel = datalevel.DSSHRS Then
                    Me.da.exec_nonqueryMain(obj("enable_script").ToString)
                Else
                    Me.da.exec_nonqueryInTEMPDB(obj("enable_script").ToString)
                End If

            Else
                If dblevel = datalevel.DSSHRS Then
                    Me.da.exec_nonqueryMain(obj("disable_script").ToString)
                Else
                    Me.da.exec_nonqueryInTEMPDB(obj("disable_script").ToString)
                End If
            End If
        Next
    End Sub
    Private Function updateNullEpisodesQuery(ByVal episodetablename As String, ByVal episodeidName As String) As String
        Dim sql As String = "update [TEMP_DSSHRS].[DSS].[" & episodetablename & "] set rec_status='UX'" _
        & " where rec_status='u' and (edate is null  or eobserveid is null or eeventtype is null)" _
        & " and " + episodeidName + "  in (select a." + episodeidName + " from DSSHRS.DSS." & episodetablename & " as a)"
        Return sql
    End Function
    Private Function updateRectatusEpisodesQuery(ByVal episodetablename As String, ByVal episodeidName As String) As String
        Dim sql As String = "update [TEMP_DSSHRS].[DSS].[" & episodetablename & "] set rec_status='I'" _
        & " where rec_status='u' and (edate is null  or eobserveid is null or eeventtype is null)" _
        & " and " + episodeidName + "  not in (select a." + episodeidName + " from DSSHRS.DSS." & episodetablename & " as a)"
        Return sql
    End Function
    Friend Sub uploadDatatoMainDb()
        'clear error tab
        Dim returnVal As String = ""
        foreignKeys.worker = worker

        'Static objects
        worker.ReportProgress(Nothing, "starting validating and uploading DSS.individual " & " " & Now.ToString())
        returnVal = returnVal & vbCrLf & Me.foreignKeys.ValidateforeignKey_Table_inTEMP_DSSHRS("DSS.individual", "not(rec_status like '%x%')")
        returnVal = returnVal & vbCrLf & Me.transfer_Individual(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, "SELECT * FROM [TEMP_DSSHRS].[DSS].[individual]where [errflag]=0 and  not(rec_status like '%x%')")
        worker.ReportProgress(Nothing, " finished validating and uploading DSS.individual " & " " & Now.ToString())

        worker.ReportProgress(Nothing, " starting  validating and uploading DSS.compounds " & " " & Now.ToString())
        returnVal = returnVal & vbCrLf & Me.foreignKeys.ValidateforeignKey_Table_inTEMP_DSSHRS("DSS.compounds", "not(rec_status like '%x%')")
        returnVal = returnVal & vbCrLf & Me.transfer_compounds(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, "SELECT * FROM [DSS].[compounds]where [errflag]=0 and  not(rec_status like '%x%')")

        worker.ReportProgress(Nothing, " finished validating and uploading DSS.compounds " & " " & Now.ToString())


        worker.ReportProgress(Nothing, " starting  validating and uploading DSS.location " & " " & Now.ToString())
        returnVal = returnVal & vbCrLf & Me.foreignKeys.ValidateforeignKey_Table_inTEMP_DSSHRS("DSS.location", "not(rec_status like '%x%')")
        returnVal = returnVal & vbCrLf & Me.transfer_locations(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, "SELECT * FROM [DSS].[location]where [errflag]=0 and  not(rec_status like '%x%')")

        worker.ReportProgress(Nothing, " finished validating and uploading DSS.location " & " " & Now.ToString())

        worker.ReportProgress(Nothing, " starting  validating and uploading DSS.socialgrop " & " " & Now.ToString())
        'socialgroup updates
        returnVal = returnVal & vbCrLf & Me.transfer_SocialGroupupdates(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, "SELECT * FROM dbo.SocialGroupUpdates ")


        returnVal = returnVal & vbCrLf & Me.foreignKeys.ValidateforeignKey_Table_inTEMP_DSSHRS("DSS.socialgroup", "not(rec_status like '%x%')")
        returnVal = returnVal & vbCrLf & Me.transfer_socialgroup(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, "SELECT * FROM [DSS].[socialgroup]where ([errflag]=0) and (not(rec_status like '%x%'))")

        worker.ReportProgress(Nothing, " finished validating and uploading DSS.socialgroup " & " " & Now.ToString())

        'FOR STATIC
        worker.ReportProgress(Nothing, " starting  validating and uploading DSS.observation " & " " & Now.ToString())

        returnVal = returnVal & vbCrLf & Me.foreignKeys.ValidateforeignKey_Table_inTEMP_DSSHRS("DSS.observation", "not(rec_status like '%x%')")
        returnVal = returnVal & vbCrLf & Me.transfer_observation(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, "SELECT * FROM [DSS].[observation]where [errflag]=0 and  not(rec_status like '%x%')")

        worker.ReportProgress(Nothing, " finished validating and uploading DSS.observation " & " " & Now.ToString())

        worker.ReportProgress(Nothing, " starting  validating and uploading DSS.socialGroupadmin " & " " & Now.ToString())

        returnVal = returnVal & vbCrLf & Me.foreignKeys.ValidateforeignKey_Table_inTEMP_DSSHRS("DSS.SocialGroupadmin", "not(rec_status like '%x%')")
        returnVal = returnVal & vbCrLf & Me.transfer_SocialGroupadmin(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, "SELECT * FROM [DSS].[SocialGroupadmin]where [errflag]=0 and  not(rec_status like '%x%')")

        worker.ReportProgress(Nothing, " finished validating and uploading DSS.socialgroupadmin " & " " & Now.ToString())


        worker.ReportProgress(Nothing, " starting  validating and uploading DSS.compadmin " & " " & Now.ToString())

        returnVal = returnVal & vbCrLf & Me.foreignKeys.ValidateforeignKey_Table_inTEMP_DSSHRS("DSS.compadmin", "not(rec_status like '%x%')")
        returnVal = returnVal & vbCrLf & Me.transfer_compadmin(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, "SELECT * FROM [DSS].[compadmin]where [errflag]=0 and  not(rec_status like '%x%')")

        worker.ReportProgress(Nothing, " finished validating and uploading DSS.compadmin " & " " & Now.ToString())

        'MHRS_SYS.Changes()
        worker.ReportProgress(Nothing, " starting  validating and uploading  changes " & " " & Now.ToString())

        Me.foreignKeys.validateIndividualChanges_Table_InTEMP_DSSHRS("select * from [MHRS_SYS].[Changes] where tablename='dss.individual' and  not(rec_status like '%x%')")
        Me.foreignKeys.validateResidencyChanges_table_InTEMP_DSSHRS("select * from [MHRS_SYS].[Changes] where tablename='dss.residency' and  not(rec_status like '%x%')")
        returnVal = returnVal & vbCrLf & Me.transfer_changes(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, "SELECT * FROM [MHRS_SYS].[Changes] where ([errflag]=0) and (tablename in ('dss.individual','dss.residency')) and  not(rec_status like '%x%')")

        worker.ReportProgress(Nothing, " finished validating and uploading IndividualChanges " & " " & Now.ToString())



        'EPISODES()
        ' Residency()

        worker.ReportProgress(Nothing, " starting  validating and uploading DSS.residency " & " " & Now.ToString())


        returnVal = returnVal & vbCrLf & Me.foreignKeys.ValidateforeignKey_Table_inTEMP_DSSHRS("DSS.residency", "not(rec_status like '%x%')")

        resd.validateResidency(Me.da.getTableDataFromTempDB("select * from dss.residency where rec_status in('U','DU','TU','MU') order by edate asc,sdate asc "), "dss.residency", worker)
       
        returnVal = returnVal & vbCrLf & Me.transfer_residency_UPDATES(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, _
                               "SELECT * FROM [DSS].[residency] where (rec_status in('U','DU','TU','MU')) and ([errflag]=0)")

        resd.validateResidency(Me.da.getTableDataFromTempDB("select * from dss.residency where rec_status in('DI','I','TI','MI')  order by sdate asc,edate asc "), "dss.residency", worker)
        returnVal = returnVal & vbCrLf & Me.transfer_residency_NewEpisodes(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, _
                    "SELECT * FROM [DSS].[residency] where  (rec_status in('I','DI','TI','MI')) and ([errflag]=0)")

        worker.ReportProgress(Nothing, " finished validating and uploading DSS.residency " & " " & Now.ToString())

        'Memberships()

        worker.ReportProgress(Nothing, " starting  validating and uploading DSS.membership " & " " & Now.ToString())


        returnVal = returnVal & vbCrLf & Me.foreignKeys.ValidateforeignKey_Table_inTEMP_DSSHRS("DSS.Membership", "not(rec_status like '%x%')")

        Me.memb.validateMembership(Me.da.getTableDataFromTempDB("select * from dss.Membership  where rec_status in('U','DU','TU','MU') order by edate asc,sdate asc "), "dss.Membership", worker)
        returnVal = returnVal & vbCrLf & Me.transfer_membership_UPDATES(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, _
                             "SELECT * FROM [DSS].[membership] where (rec_status in('U','DU','TU','MU')) and ([errflag]=0)")

        Me.memb.validateMembership(Me.da.getTableDataFromTempDB("select * from dss.Membership  where rec_status in('DI','I','TI','MI') order by sdate asc,edate asc "), "dss.Membership", worker)
        returnVal = returnVal & vbCrLf & Me.transfer_membership_NewEpisodes(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, _
                                "SELECT * FROM [DSS].[Membership] where   (rec_status in('I','DI','TI','TI','MI')) and ([errflag]=0)")


        worker.ReportProgress(Nothing, " finished validating and uploading DSS.memberships " & " " & Now.ToString())

        'Pregnacy()

        worker.ReportProgress(Nothing, " starting  validating and uploading DSS.pregnancy " & " " & Now.ToString())


        returnVal = returnVal & vbCrLf & Me.foreignKeys.ValidateforeignKey_Table_inTEMP_DSSHRS("DSS.Pregnancy", "not(rec_status like '%x%')")

        Me.preg.validatePregnancy(Me.da.getTableDataFromTempDB("select * from dss.Pregnancy  where rec_status in('U','DU','TU','MU') order by edate asc,sdate asc"), "dss.Pregnancy", worker)
        returnVal = returnVal & vbCrLf & Me.transfer_pregnancy_UPDATES(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, _
                                      "SELECT * FROM [DSS].[pregnancy] where (rec_status in('U','DU','TU','MU')) and ([errflag]=0)")

        Me.preg.validatePregnancy(Me.da.getTableDataFromTempDB("select * from dss.Pregnancy  where rec_status in('DI','I','TI','MI') order by sdate asc,edate asc "), "dss.Pregnancy", worker)
        returnVal = returnVal & vbCrLf & Me.transfer_pregnancy_NewEpisodes(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, _
                               "SELECT * FROM [DSS].[Pregnancy] where  (rec_status in('I','DI','TI','MI')) and ([errflag]=0)")


        worker.ReportProgress(Nothing, " finished validating and uploading DSS.pregnancy " & " " & Now.ToString())






        'Validate event episode table
        worker.ReportProgress(Nothing, " starting  validating and uploading Events_Episodes " & " " & Now.ToString())
        evenEpisod.validateEventEpisode(Me.da.getTableDataFromTempDB("select * from [TEMP_DSSHRS].[DSS].[Events_Episodes]  where not(rec_status like '%x%')"), worker)
        worker.ReportProgress(Nothing, " finished validating and uploading Events_Episodes " & " " & Now.ToString())



        'Update events data transfer 
        'This ensures that events are transfered only when all related episodes have been transfered.
        worker.ReportProgress(Nothing, " starting  Data transfer validation " & " " & Now.ToString())
        Me.da.exec_nonquery("exec [TEMP_DSSHRS].[DSS].[Transfer_Updates_EventEpisode_A]")
        Me.da.exec_nonquery("exec [TEMP_DSSHRS].[DSS].[Transfer_Updates_EventEpisode_B]")

        'Update events for data transfer
        Me.da.exec_nonquery("exec [TEMP_DSSHRS].[DSS].[Transfer_Updates_Births]")
        Me.da.exec_nonquery("exec [TEMP_DSSHRS].[DSS].[Transfer_Updates_Migrations]")
        Me.da.exec_nonquery("exec [TEMP_DSSHRS].[DSS].[Transfer_Updates_Pregoutcome]")
        worker.ReportProgress(Nothing, " Finished Data transfer validation " & " " & Now.ToString())


        ' ''EVENT
        returnVal = returnVal & vbCrLf & Me.transfer_events()
        ' ''SPECIES
        Try
            returnVal = returnVal & vbCrLf & Me.transfer_specialStudies()
            worker.ReportProgress(Nothing, " " & returnVal & " " & Now.ToString())
        Catch ex As Exception
            worker.ReportProgress(Nothing, " " & ex.Message & " " & Now.ToString())
        End Try






    End Sub
    Public Function getTEMPRowValidations(ByVal schemaName As String, ByVal tablename As String, ByVal record As DataRow, _
    Optional ByVal clmName As DataColumn = Nothing, Optional ByVal displayerrormessage As Boolean = False) As Boolean
        'clear error tab
        Dim returnVal As Boolean = True
        Dim strReturnValue As String = ""
        Dim transit_id As String = record("transit_id").ToString
        Dim rec_status As String = record("rec_status").ToString.ToLower.Trim
        Dim fulltablename As String = schemaName.ToLower.Trim + "." + tablename.ToLower.Trim
        foreignKeys.worker = worker
        Select Case fulltablename.ToLower.Trim
            Case "dss.individual"
                If Not Me.foreignKeys.ValidateforeignKey_Row_INTEMP_DSSHRS("DSS.individual", record) Then
                    returnVal = False
                End If

                If Me.da.hasSmallAgediffwithfather(record, 13) Then
                    Me.da.saveError(record("transit_id").ToString.Trim, "DSS.individual", "The father is too young", "", Now(), "", da.getrecordsCompound("dss.individual", record).Trim, da.getrecordsRound("dss.individual", record).Trim())
                    Me.da.exec_nonqueryInTEMPDB("UPDATE [dss].[individual] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + record("transit_id").ToString.Trim)
                    returnVal = False
                End If
                If Me.da.hasSmallAgediffwithMother(record, 13) Then
                    Me.da.saveError(record("transit_id").ToString.Trim, "DSS.individual", "The mother is too young", "", Now(), "", da.getrecordsCompound("dss.individual", record).Trim, da.getrecordsRound("dss.individual", record).Trim())
                    Me.da.exec_nonqueryInTEMPDB("UPDATE [dss].[individual] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + record("transit_id").ToString.Trim)
                    returnVal = False
                End If
            Case "dss.compounds"
                If Not Me.foreignKeys.ValidateforeignKey_Row_INTEMP_DSSHRS("DSS.compounds", record) Then
                    returnVal = False
                End If
            Case "dss.location"
                If Not Me.foreignKeys.ValidateforeignKey_Row_INTEMP_DSSHRS("DSS.location", record) Then
                    returnVal = False
                End If
            Case "dss.socialgroup"
                If Not Me.foreignKeys.ValidateforeignKey_Row_INTEMP_DSSHRS("DSS.socialgroup", record) Then
                    returnVal = False
                End If
            Case "dss.observation"
                If Not Me.foreignKeys.ValidateforeignKey_Row_INTEMP_DSSHRS("DSS.observation", record) Then
                    returnVal = False
                End If
            Case "dss.socialgroupadmin"
                If Not Me.foreignKeys.ValidateforeignKey_Row_INTEMP_DSSHRS("DSS.SocialGroupadmin", record) Then
                    returnVal = False
                End If

            Case "dss.compadmin"
                If Not Me.foreignKeys.ValidateforeignKey_Row_INTEMP_DSSHRS("DSS.compadmin", record) Then
                    returnVal = False
                End If

            Case "mhrs_sys.changes"
                Select Case record("tablename").ToString.ToLower.Trim
                    Case "dss.individual"
                        If Not Me.foreignKeys.validateIndividualChanges_Row_inTEMP_DSSHRS(record) Then
                            returnVal = False
                        End If

                    Case "dss.residency"
                        If Not Me.foreignKeys.validateResidencyChanges_row_InTEMP_DSSHRS(record) Then
                            returnVal = False
                        End If
                End Select
            Case "dss.residency"
                If Not Me.foreignKeys.ValidateforeignKey_Row_INTEMP_DSSHRS("DSS.residency", record) Then
                    returnVal = False
                End If
                If Not resd.validaterec(record, fulltablename) Then
                    returnVal = False
                End If
            Case "dss.membership"
                'Memberships

                If Not Me.foreignKeys.ValidateforeignKey_Row_INTEMP_DSSHRS("DSS.Membership", record) Then
                    returnVal = False
                End If
                If Not Me.memb.validaterec(record, fulltablename) Then
                    returnVal = False
                End If
            Case "dss.pregnancy"
                If Not Me.foreignKeys.ValidateforeignKey_Row_INTEMP_DSSHRS("DSS.Pregnancy", record) Then
                    returnVal = False
                End If
                If Not Me.preg.validaterec(record, fulltablename) Then
                    returnVal = False
                End If
                'events
            Case "dss.migrations", _
             "dss.birth", _
             "dss.pregoutcome", _
             "dss.indvstatus", _
             "dss.events_episodes", _
             "dss.marriage"

                If fulltablename.ToLower.Trim = "dss.marriage" Then
                    If Me.da.individualtooYounfForMarriage(record("individid").ToString, 13) Then
                        Me.da.saveError(record("transit_id").ToString.Trim, "dss.marriage", " individual too young for marriage ", "", Now(), "", da.getrecordsCompound("dss.marriage", record).Trim, da.getrecordsRound("dss.marriage", record).Trim())
                        Me.da.exec_nonqueryInTEMPDB("UPDATE [dss].[marriage] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + record("transit_id").ToString.Trim)
                        returnVal = False
                    End If

                    'Ensure that the opposite gender 
                    If Me.da.CheckSexforSpouse(record("individid").ToString, record("episodeid").ToString) Then
                        Me.da.saveError(record("transit_id").ToString.Trim, "dss.marriage", "Marriage to same sex not allowed", "", Now(), "", da.getrecordsCompound("dss.marriage", record).Trim, da.getrecordsRound("dss.marriage", record).Trim())
                        Me.da.exec_nonqueryInTEMPDB("UPDATE [dss].[marriage] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + record("transit_id").ToString.Trim)
                        returnVal = False
                    End If

                    If (Not record("spouseid").ToString.Trim.Equals("")) AndAlso Me.da.spousetooYounfForMarriage(record("spouseid").ToString, 13) Then
                        Me.da.saveError(record("transit_id").ToString.Trim, "dss.marriage", "Spouse too young for marriage", "", Now(), "", da.getrecordsCompound("dss.marriage", record).Trim, da.getrecordsRound("dss.marriage", record).Trim())
                        Me.da.exec_nonqueryInTEMPDB("UPDATE [dss].[marriage] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + record("transit_id").ToString.Trim)
                        returnVal = False
                    End If
                End If
                If fulltablename.Trim.ToLower = "dss.pregoutcome" Then
                    If Me.da.has_closePregnacy_Outcome(record("individid").ToString, record("date")) Then
                        Me.da.saveError(record("transit_id").ToString.Trim, "dss.pregoutcome", " individual had another preg recently ", "", Now(), "", da.getrecordsCompound("dss.pregoutcome", record).Trim, da.getrecordsRound("dss.pregoutcome", record).Trim())
                        Me.da.exec_nonqueryInTEMPDB("UPDATE [dss].[pregoutcome] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + record("transit_id").ToString.Trim)
                        returnVal = False
                    End If
                End If


                Try
                    If Not Me.foreignKeys.ValidateforeignKey_Row_INTEMP_DSSHRS(fulltablename.Trim, record) Then
                        returnVal = False
                    End If
                Catch ex As Exception
                    objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

                    'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

                    ' MsgBox(ex.Message)
                    'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
                End Try

                'special studies
            Case "dss.visitation", _
            "specialstudies.health", _
            "specialstudies.gpsdata", _
            "specialstudies.hsedetails", _
            "specialstudies.huas_lite", _
            "specialstudies.immunize", _
            "specialstudies.ses", _
            "specialstudies.education", _
            "specialstudies.itn", _
            "specialstudies.morbidity", _
            "specialstudies.parentsurv", _
            "specialstudies.druguse", _
            "specialstudies.contraception", _
            "specialstudies.dmicampaign", _
            "specialstudies.Reprodactivehealth", _
            "specialstudies.mobilephoneusage", _
            "specialstudies.stove", _
            "specialstudies.religion", _
            "ghi.Toilet", _
            "ghi.Diarrhea_treat", _
            "ghi.fp_current_use", _
            "ghi.Fever_treatment", _
            "ghi.fever_other", _
            "ghi.Fever_drug", _
            "ghi.baby_drink", _
            "ghi.anc_place", _
            "ghi.afterdelivery_service", _
            "ghi.Relationships", _
            "ghi.pregnancy_and_Birth", _
            "ghi.House_sanitation", _
            "ghi.ghi_itn", _
            "ghi.family_planning", _
            "ghi.vct_hiv", _
            "ghi.Treat_water", _
            "SpecialStudies.bednet".ToLower, _
            "SpecialStudies.bednet_individual_netUse".ToLower, _
            "SpecialStudies.bednet_netinfo".ToLower, _
            "SpecialStudies.BreastFeedingKnowledge".ToLower, _
            "MS.WASHSCHOOL".ToLower, _
            "MS.WASHLatrine".ToLower, _
            "MS.WASHContainers".ToLower, _
            "SpecialStudies.EVP".ToLower, _
            "specialStudies.EVP_Anthropometric".ToLower, _
            "specialStudies.EVPinterviewOutcome".ToLower, _
            "specialStudies.FetchWater_Member".ToLower.Trim, _
             "PBR.ANC".ToLower.Trim, _
             "PBR.ANC_PLACE".ToLower.Trim, _
             "PBR.afterdelivery_service".ToLower.Trim, _
             "PBR.CHILDREN".ToLower.Trim

                Try

                    If fulltablename.Trim.ToLower = "dss.visitation" Then
                        If Me.da.has_Completed_Visitation(record("locationid").ToString.Trim, record("round").ToString.Trim) Then
                            'Validation to check for revisits. If the corresponding record that is completed has been uploaded to man datase,
                            'the revisits for that round should also be able to go thru.
                            Dim sql As String = "SELECT [TEMP_DSSHRS].[dbo].[getRevisitsWithCompStatus] ('" & record("round").ToString.Trim & "','" & record("locationid").ToString.Trim & "')"
                            If record("Outcome").ToString.ToUpper.Trim.Equals("REVISIT") AndAlso Me.da.executeScalar_INMainDB(sql) > 0 Then
                                'Do nothing
                            Else
                                Me.da.saveError(record("transit_id").ToString.Trim, "dss.visitation", " Location has a completed visitation In MainDB ", "", Now(), "", da.getrecordsCompound("dss.visitation", record).Trim, da.getrecordsRound("dss.visitation", record).Trim())
                                Me.da.exec_nonqueryInTEMPDB("UPDATE [dss].[visitation] SET [errflag] = 'true' , errdate=getdate() where transit_id=" + record("transit_id").ToString.Trim)
                                returnVal = False
                            End If

                        End If
                    End If
                    If fulltablename.Trim.ToLower = "specialstudies.fetchwater_member" Then
                        'validate it should be 2 years and above
                        If Me.da.getIndivdualsAge(Nothing, record("Entry_date"), record("individid")) < 2.0 Then
                            Me.da.saveError(record("transit_id").ToString.Trim, "specialStudies.FetchWater_Member", "The child is below 2 Years", "", Now(), "", _
                                da.getrecordsCompound("specialStudies.FetchWater_Member", record).Trim, da.getrecordsRound("specialStudies.FetchWater_Member", record).Trim())
                            Me.da.exec_nonqueryInTEMPDB("UPDATE specialStudies.FetchWater_Member SET [errflag] = 'true' , errdate=getdate() where transit_id=" + record("transit_id").ToString.Trim)
                            returnVal = False
                        End If

                    End If

                    If fulltablename.Trim.ToLower = "specialStudies.EVP_Anthropometric".ToLower Then
                        'validate it should be 2 years and above
                        Dim iresAge As Double = Me.da.getIndivdualsAge(Nothing, record("WAS_ANT_TIME"), record("WAS_ANT_C1_ID"))
                        If Not (iresAge >= 0.5 And iresAge <= 6) Then
                            Me.da.saveError(record("transit_id").ToString.Trim, "specialStudies.EVP_Anthropometric", "The child should be between 0.5 and 6 years of age", "", Now(), "", _
                                da.getrecordsCompound("specialStudies.EVP_Anthropometric", record).Trim, da.getrecordsRound("specialStudies.EVP_Anthropometric", record).Trim())
                            Me.da.exec_nonqueryInTEMPDB("UPDATE specialStudies.EVP_Anthropometric SET [errflag] = 'true' , errdate=getdate() where transit_id=" + record("transit_id").ToString.Trim)
                            returnVal = False
                        End If

                    End If

                    If fulltablename.Trim.ToLower = "specialStudies.EVP".ToLower Then
                        'validate it should be 2 years and above
                        Dim iresAge As Double = Me.da.getIndivdualsAge(Nothing, record("WAS_ANT_TIME"), record("WAS_ANT_C1_ID"))
                        If Not (iresAge >= 18 And iresAge <= 50) Then
                            Me.da.saveError(record("transit_id").ToString.Trim, "specialStudies.EVP", "The mother should be between 18 and 49 years of age", "", Now(), "", _
                                da.getrecordsCompound("specialStudies.EVP", record).Trim, da.getrecordsRound("specialStudies.EVP", record).Trim())
                            Me.da.exec_nonqueryInTEMPDB("UPDATE specialStudies.EVP SET [errflag] = 'true' , errdate=getdate() where transit_id=" + record("transit_id").ToString.Trim)
                            returnVal = False
                        End If

                    End If

                    If fulltablename.Trim.ToLower = "specialStudies.FoodFreq".ToLower Then
                        'validate it should be 2 years and above
                        Dim iresAge As Double = Me.da.getIndivdualsAge(Nothing, record("entry_date"), record("individid"))
                        If Not (iresAge >= 0.5 And iresAge <= 10) Then
                            Me.da.saveError(record("transit_id").ToString.Trim, "specialStudies.FoodFreq", "The mother should be between 18 and 49 years of age", "", Now(), "", _
                                da.getrecordsCompound("specialStudies.FoodFreq", record).Trim, da.getrecordsRound("specialStudies.FoodFreq", record).Trim())
                            Me.da.exec_nonqueryInTEMPDB("UPDATE specialStudies.FoodFreq SET [errflag] = 'true' , errdate=getdate() where transit_id=" + record("transit_id").ToString.Trim)
                            returnVal = False
                        End If

                    End If

                    If Not Me.foreignKeys.ValidateforeignKey_Row_INTEMP_DSSHRS(fulltablename.Trim, record) Then
                        returnVal = False
                    End If
                Catch ex As Exception
                    objRef.strObjMethod = New Diagnostics.StackTrace().ToString()

                    'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

                    ' MsgBox(ex.Message)
                    'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
                End Try
            Case Else

        End Select
        'xxxxx
        Return returnVal
    End Function
#End Region

#Region "Data transfer for DSSHRS"
    Public Function getRowValidations(ByVal db As datalevel, ByVal schemaName As String, ByVal tablename As String, ByVal record As DataRow, _
    Optional ByVal clmName As DataColumn = Nothing, Optional ByVal displayerrormessage As Boolean = False) As Boolean

        If displayerrormessage Then
            Me.da.validationtype = mhrsSyncValidationTypes.userpplication
        Else
            Me.da.validationtype = mhrsSyncValidationTypes.batchprocessing
        End If

        Select Case db
            Case datalevel.DSSHRS
                Return Me.getMAINRowValidations(schemaName, tablename, record, clmName, displayerrormessage)
            Case datalevel.TEMP_DSSHRS
                Return Me.getTEMPRowValidations(schemaName, tablename, record, clmName, displayerrormessage)
        End Select
    End Function
    Public Function getMAINRowValidations(ByVal schemaName As String, ByVal tablename As String, ByVal record As DataRow _
, Optional ByVal clmName As DataColumn = Nothing, Optional ByVal displayerrormessage As Boolean = False) As Boolean
       
        Dim returnVal As Boolean = True
        Dim fulltablename As String = schemaName + "." + tablename
        foreignKeys.worker = worker

        Select Case fulltablename.ToLower.Trim
            Case "", "dss.individual", "dss.compounds", "dss.location", "dss.socialgroup", _
            "dss.observation", "dss.socialgroupadmin", "dss.compadmin", "mhrs_sys.changes", _
            "dss.visitation", _
            "SpecialStudies.health", _
            "SpecialStudies.GPSDATA".ToLower, _
            "SpecialStudies.hsedetails", _
            "SpecialStudies.HUAS_LITE".ToLower, _
            "SpecialStudies.immunize", _
            "SpecialStudies.ses", _
            "SpecialStudies.education", _
            "SpecialStudies.itn", _
            "SpecialStudies.morbidity", _
            "SpecialStudies.parentsurv", _
            "SpecialStudies.Religion".ToLower, _
            "dss.migrations", _
            "dss.birth", _
            "dss.pregoutcome", _
            "dss.indvstatus", _
            "dss.events_episodes", _
            "dss.marriage", _
            "ghi.ghi_itn".ToLower.Trim, _
            "ghi.family_planning".ToLower.Trim, _
            "ghi.vct_hiv".ToLower.Trim, _
            "ghi.Treat_water".ToLower.Trim, _
            "ghi.Toilet".ToLower.Trim, _
            "ghi.Diarrhea_treat".ToLower.Trim, _
            "ghi.fp_current_use".ToLower.Trim, _
            "ghi.Fever_treatment".ToLower.Trim, _
            "ghi.fever_other".ToLower.Trim, _
            "ghi.Fever_drug".ToLower.Trim, _
            "ghi.baby_drink".ToLower.Trim, _
            "ghi.anc_place".ToLower.Trim, _
            "ghi.afterdelivery_service".ToLower.Trim, _
            "SpecialStudies.bednet".ToLower.Trim, _
            "specialStudies.bednet_individual_netUse".ToLower.Trim, _
            "specialStudies.bednet_netinfo".ToLower.Trim
                Try
                    If Not Me.foreignKeys.ValidateforeignKey_DSSHRS(fulltablename.ToLower.Trim, record) Then
                        returnVal = False
                    End If
                Catch ex As Exception
                    objRef.strObjMethod = New Diagnostics.StackTrace().ToString()
                    returnVal = False
                    'strObjFileName = strObjFileName.Substring(strObjFileName.LastIndexOf("\") + 1)

                    ' MsgBox(ex.Message)
                    'emailErrors("--- Exception StackTrace. Please Follow  ---" & vbCrLf & vbCrLf & objRef.strObjMethod )
                End Try
            Case "dss.residency"
                If Not Me.foreignKeys.ValidateforeignKey_DSSHRS(fulltablename.ToLower.Trim, record) Then
                    returnVal = False
                End If
                If Not resdDSSHRS.validaterec(record, fulltablename.ToLower.Trim) Then
                    returnVal = False
                End If
            Case "dss.membership"
                If Not Me.foreignKeys.ValidateforeignKey_DSSHRS(fulltablename.ToLower.Trim, record) Then
                    returnVal = False
                End If
                If Not membDSSHRs.validaterec(record, fulltablename.ToLower.Trim) Then
                    returnVal = False
                End If
            Case "dss.pregnancy"
                If Not Me.foreignKeys.ValidateforeignKey_DSSHRS(fulltablename.ToLower.Trim, record) Then
                    returnVal = False
                End If
                If Not pregDSSHRs.validaterec(record, fulltablename.ToLower.Trim) Then
                    returnVal = False
                End If
            Case Else
        End Select
        'xxxxx
        Return returnVal
    End Function
#End Region

#Region "transfer temp data row"
    Public Function transferTEMPRowValidations(ByVal schemaName As String, ByVal tablename As String, ByVal record As DataRow) As Boolean
        'clear error tab
        Dim returnVal As Boolean = True
        Dim strReturnValue As String = ""
        Dim transit_id As String = record("transit_id").ToString
        Dim rec_status As String = record("rec_status").ToString.ToLower.Trim
        If rec_status.Contains("x") Then
            MsgBox("record was marked for deletion", MsgBoxStyle.Critical)
            Return False
        End If
        Dim fulltablename As String = schemaName.ToLower.Trim + "." + tablename.ToLower.Trim
        foreignKeys.worker = worker
        Select Case fulltablename.ToLower.Trim
            Case "dss.individual"
                strReturnValue = strReturnValue & vbCrLf & Me.transfer_Individual(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, "SELECT * FROM [TEMP_DSSHRS].[DSS].[individual]where (transit_id=" + transit_id + ") and ([errflag]=0)")
            Case "dss.compounds"
                strReturnValue = strReturnValue & vbCrLf & Me.transfer_compounds(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, "SELECT * FROM [DSS].[compounds]where (transit_id=" + transit_id + ") and ([errflag]=0)")
            Case "dss.location"
                strReturnValue = strReturnValue & vbCrLf & Me.transfer_locations(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, "SELECT * FROM [DSS].[location]where (transit_id=" + transit_id + ") and ([errflag]=0)")
            Case "dss.socialgroup"
                strReturnValue = strReturnValue & vbCrLf & Me.transfer_socialgroup(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, "SELECT * FROM [DSS].[socialgroup]where (transit_id=" + transit_id + ") and ([errflag]=0)")
            Case "dss.observation"
                strReturnValue = strReturnValue & vbCrLf & Me.transfer_observation(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, "SELECT * FROM [DSS].[observation]where (transit_id=" + transit_id + ") and ([errflag]=0)")
            Case "dss.socialgroupadmin"
                strReturnValue = strReturnValue & vbCrLf & Me.transfer_SocialGroupadmin(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, "SELECT * FROM [DSS].[SocialGroupadmin]where (transit_id=" + transit_id + ") and ([errflag]=0)")
            Case "dss.compadmin"
                strReturnValue = strReturnValue & vbCrLf & Me.transfer_compadmin(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, "SELECT * FROM [DSS].[compadmin]where (transit_id=" + transit_id + ") and ([errflag]=0)")
            Case "mhrs_sys.changes"
                strReturnValue = strReturnValue & vbCrLf & Me.transfer_changes(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, "SELECT * FROM [MHRS_SYS].[Changes] where (transit_id=" + transit_id + ") and ([errflag]=0)")
            Case "dss.residency"
                Select Case rec_status.ToLower.Trim
                    Case "u", "du", "tu", "mu"
                        strReturnValue = strReturnValue & vbCrLf & Me.transfer_residency_UPDATES(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, _
                                               "SELECT * FROM [DSS].[residency] where (transit_id=" + transit_id + ") and ([errflag]=0)")

                    Case "i", "di", "ti", "mi"
                        strReturnValue = strReturnValue & vbCrLf & Me.transfer_residency_NewEpisodes(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, _
                                                    "SELECT * FROM [DSS].[residency] where  (transit_id=" + transit_id + ") and ([errflag]=0)")
                End Select

            Case "dss.membership"
                'Memberships

                Select Case rec_status.ToLower.Trim
                    Case "u", "du", "tu", "mu"
                        strReturnValue = strReturnValue & vbCrLf & Me.transfer_membership_UPDATES(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, _
                                                           "SELECT * FROM [DSS].[membership] where (transit_id=" + transit_id + ") and ([errflag]=0)")
                    Case "i", "di", "ti", "mi"
                        strReturnValue = strReturnValue & vbCrLf & Me.transfer_membership_NewEpisodes(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, _
                                       "SELECT * FROM [DSS].[Membership] where   (transit_id=" + transit_id + ") and ([errflag]=0)")
                End Select

            Case "dss.pregnancy"

                Select Case rec_status.ToLower.Trim
                    Case "u", "du", "tu", "mu"
                        strReturnValue = strReturnValue & vbCrLf & Me.transfer_pregnancy_UPDATES(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, _
                                                                    "SELECT * FROM [DSS].[pregnancy] where (transit_id=" + transit_id + ") and ([errflag]=0)")

                    Case "i", "di", "ti", "mi"

                        strReturnValue = strReturnValue & vbCrLf & Me.transfer_pregnancy_NewEpisodes(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, _
                                       "SELECT * FROM [DSS].[Pregnancy] where  (transit_id=" + transit_id + ") and ([errflag]=0)")
                End Select

                'events
            Case "dss.migrations", _
             "dss.birth", _
             "dss.pregoutcome", _
             "dss.indvstatus", _
             "dss.events_episodes", _
             "dss.marriage"

                Try

                    Select Case fulltablename.ToLower.Trim
                        Case "dss.events_episodes"
                            evenEpisod.validateEventEpisode(Me.da.getTableDataFromTempDB("select * from [TEMP_DSSHRS].[DSS].[Events_Episodes]  where transit_id=" + transit_id), worker)
                            Me.da.exec_nonquery("exec [TEMP_DSSHRS].[DSS].[Transfer_Updates_EventEpisode_A_B] @record_key=" + transit_id)
                            Me.da.exec_nonquery("exec [TEMP_DSSHRS].[DSS].[Transfer_Updates_EventEpisode_B_B] @record_key=" + transit_id)

                        Case "dss.migrations"
                            Me.da.exec_nonquery("exec [TEMP_DSSHRS].[DSS].[Transfer_Updates_Migrations_B]@record_key=" + transit_id)
                        Case "dss.birth"
                            Me.da.exec_nonquery("exec [TEMP_DSSHRS].[DSS].[Transfer_Updates_Births_B]@record_key=" + transit_id)
                        Case "dss.pregoutcome"
                            Me.da.exec_nonquery("exec [TEMP_DSSHRS].[DSS].[Transfer_Updates_Pregoutcome_B]@record_key=" + transit_id)
                    End Select
                    strReturnValue = strReturnValue & vbCrLf & Me.CopyTempDB_To_MAINDB(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, fulltablename.Trim, fulltablename.Trim, _
                               "select * from " + fulltablename + " where (transit_id=" + transit_id + ") and ([errflag]=0)")
                Catch ex As Exception
                    objRef.strObjMethod = New Diagnostics.StackTrace().ToString()
                End Try
                'special studies
            Case "dss.visitation", _
            "specialstudies.health", _
            "specialstudies.gpsdata", _
            "specialstudies.hsedetails", _
            "specialstudies.huas_lite", _
            "specialstudies.immunize", _
            "specialstudies.ses", _
            "specialstudies.education", _
            "specialstudies.circumcision", _
            "specialstudies.itn", _
            "specialstudies.morbidity", _
            "specialstudies.parentsurv", _
            "specialstudies.druguse", _
            "specialstudies.contraception", _
            "specialstudies.dmicampaign", _
            "specialstudies.Reprodactivehealth".ToLower.Trim, _
            "specialstudies.mobilephoneusage", _
            "specialstudies.stove", _
            "specialstudies.religion", _
            "ghi.Child_health".ToLower.Trim, _
            "ghi.Relationships".ToLower.Trim, _
            "ghi.pregnancy_and_Birth".ToLower.Trim, _
            "ghi.House_sanitation".ToLower.Trim, _
            "ghi.ghi_itn".ToLower.Trim, _
            "ghi.family_planning".ToLower.Trim, _
            "ghi.vct_hiv".ToLower.Trim, _
            "ghi.Treat_water".ToLower.Trim, _
            "ghi.Toilet".ToLower.Trim, _
            "ghi.Diarrhea_treat".ToLower.Trim, _
            "ghi.fp_current_use".ToLower.Trim, _
            "ghi.Fever_treatment".ToLower.Trim, _
            "ghi.fever_other".ToLower.Trim, _
            "ghi.Fever_drug".ToLower.Trim, _
            "ghi.baby_drink".ToLower.Trim, _
            "ghi.anc_place".ToLower.Trim, _
            "ghi.afterdelivery_service".ToLower.Trim, _
            "SpecialStudies.bednet".ToLower.Trim, _
            "specialStudies.bednet_individual_netUse".ToLower.Trim, _
            "specialStudies.bednet_netinfo".ToLower.Trim, _
            "specialStudies.BreastFeedingKnowledge".ToLower.Trim, _
            "specialStudies.LiveStock".ToLower.Trim, _
            "specialStudies.Cropgrown".ToLower.Trim, _
            "specialstudies.Crop_Live_production".ToLower.Trim, _
            "specialStudies.HHD_Waterusage".ToLower.Trim, _
            "specialStudies.FetchWater_Member".ToLower.Trim, _
            "specialStudies.WaterAccess".ToLower.Trim, _
            "specialStudies.WaterAccess_Activities".ToLower.Trim, _
            "specialStudies.CleanWater_Methods".ToLower.Trim, _
            "specialStudies.Income".ToLower.Trim, _
            "specialStudies.FoodFreq".ToLower.Trim, _
            "specialStudies.FoodSecurity".ToLower.Trim, _
            "MS.WASHLATRINE".ToLower, _
            "MS.WASHContainers".ToLower.Trim, _
            "MS.WASHSCHOOL".ToLower.Trim

                Try


                    Dim strParentTbl, strPK_Key As String
                    Select Case fulltablename.ToLower.Trim
                        Case "specialStudies.bednet_individual_netUse".ToLower.Trim, "specialStudies.bednet_netinfo".ToLower.Trim
                            strParentTbl = "specialstudies.bednet"
                            strPK_Key = "id"
                        Case "specialStudies.HHD_Waterusage".ToLower.Trim, "specialStudies.FetchWater_Member".ToLower.Trim _
                           , "specialStudies.CleanWater_Methods".ToLower.Trim, "specialStudies.WaterAccess_Activities".ToLower.Trim
                            strParentTbl = "specialstudies.WaterAccess"
                            strPK_Key = "wateraccessID"
                        Case "specialStudies.LiveStock".ToLower.Trim, "specialStudies.Cropgrown".ToLower.Trim
                            strParentTbl = "specialstudies.Crop_Live_production"
                            strPK_Key = "Crop_Live_ProductionID"
                        Case "MS.WASHLatrine".ToLower.Trim, "MS.WASHContainers".ToLower.Trim
                            strParentTbl = "MS.WASHSCHOOL"
                            strPK_Key = "wschid"
                    End Select

                    'check to ensure that the  parent record have all gone to the main databse
                    If fulltablename.ToLower.Trim.Equals("specialStudies.bednet_individual_netUse".ToLower.Trim) Or _
                        fulltablename.ToLower.Trim.Equals("specialStudies.bednet_netinfo".ToLower.Trim) Or _
                        fulltablename.ToLower.Trim.Equals("SpecialStudies.WaterAccess_Activities".ToLower.Trim) Or _
                        fulltablename.ToLower.Trim.Equals("specialStudies.HHD_Waterusage".ToLower.Trim) Or _
                        fulltablename.ToLower.Trim.Equals("specialStudies.FetchWater_Member".ToLower.Trim) Or _
                        fulltablename.ToLower.Trim.Equals("specialStudies.CleanWater_Methods".ToLower.Trim) Or _
                        fulltablename.ToLower.Trim.Equals("MS.WASHLatrine".ToLower.Trim) Or _
                        fulltablename.ToLower.Trim.Equals("MS.WASHContainers".ToLower.Trim) Then


                        'check to see if the recorded has a parent 
                        Dim cmd As New SqlCommand
                        'cmd.CommandType = CommandType.StoredProcedure
                        cmd.Connection = Me.clsGlobalVariable.HRS_Temp_DBCon
                        If Not Me.clsGlobalVariable.HRS_Temp_DBCon.State = ConnectionState.Open Then Me.clsGlobalVariable.HRS_Temp_DBCon.Open()
                        cmd.CommandText = "[DSSHRS].[dbo].[getParentRecords]  @tblParent,@tblChild,@tblTransit_id,@strPrimaryKey"
                        cmd.Parameters.Clear()
                        cmd.Parameters.AddWithValue("@tblParent", strParentTbl)
                        cmd.Parameters.AddWithValue("@tblChild", fulltablename.ToLower)
                        cmd.Parameters.AddWithValue("@tblTransit_id", transit_id)
                        cmd.Parameters.AddWithValue("strPrimaryKey", strPK_Key)

                        Me.clsGlobalVariable.open_HRS_TEMP_DBCon()

                        Dim newValue As Integer
                        newValue = cmd.ExecuteScalar()

                        If newValue = 0 Then
                            Return False
                        End If
                        Me.clsGlobalVariable.HRS_Temp_DBCon.Close()
                    End If

                   


                    strReturnValue = strReturnValue & vbCrLf & Me.CopyTempDB_To_MAINDB(clsGlobalVariable.HRS_Temp_DBCon, clsGlobalVariable.HRS_Main_DBCon, fulltablename.Trim, fulltablename.Trim, _
                               "select * from " + fulltablename.Trim + " where (transit_id=" + transit_id + ") and ([errflag]=0)")
                Catch ex As Exception
                    objRef.strObjMethod = New Diagnostics.StackTrace().ToString()
                    returnVal = False
                Finally
                    clsGlobalVariable.close_HRS_TEMP_DBCon()

                End Try
            Case Else

        End Select
        'xxxxx
        Return returnVal
    End Function
#End Region

End Class
