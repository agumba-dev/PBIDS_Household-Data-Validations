Public Class clsformrefrences
    'Do variable declaration here
#Region "Variable Declaration"
    'Friend ObjDbAccess As clsdbAccess = clsdbAccess.getObject
    'Friend objdbMaccess As clsdbAccess = clsdbAccess.getObject("DSSHRS")
    Friend ObjSingleton As New clsSingleton("", "", "", "", "", "")
    Friend strObjMethod As New String("")
    Friend strObjFileName As New String("")
    Private Shared objSingle As clsformrefrences
    Private Shared blCreated As Boolean
#End Region
#Region "Singleton function"
    Private Sub New()
        'Override the default constructor

    End Sub
    Public Shared Function getObject() As clsformrefrences
        If blCreated = False Then
            objSingle = New clsformrefrences
            blCreated = True
            objSingle.initialise()
            Return objSingle
        Else
            Return objSingle
        End If
    End Function
    Private Sub initialise()
        Try
            Dim strWindowsLogin As String = My.User.Name.Substring(1 + My.User.Name.IndexOf("\"), (My.User.Name.Length - (My.User.Name.IndexOf("\") + 1)))
            Me.ObjSingleton = New clsSingleton(strWindowsLogin, "", "", "", "", "")
        Catch ex As Exception

        End Try
    End Sub
#End Region

End Class