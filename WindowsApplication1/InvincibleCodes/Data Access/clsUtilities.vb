
Imports System.IO
Imports System.Net

Public Enum ageLimits

    HEALTH_STS_MIN_AGE = 15 'health status min age
    PSS_MAX_AGE = 18 'parent survival max age
    PRGN_MIN_AGE = 14 'pregancy min age
    IMMU_MAX_AGE = 2 'immunization max age
    EDU_MIN_AGE = 5 'education min age
    EDU_MAX_AGE = 24 'education max age

End Enum


Public Enum idTypes
    VILLAGE = 0
    COMPOUND = 1
    LOCATION_HOUSHOLD = 2
    INDIVIDUAL = 3
End Enum
Public Class clsUtilities

#Region "Variable Declaration"
    Public objRef As clsformrefrences = clsformrefrences.getObject
    Public objVal As clsvalidations = clsvalidations.getObject
    Public authorizedUsers As String() = {"engulukyo", "devengulukyo", "devnaotieno", "naotieno", "vodingo", "adminvodingo", "adminshadrack", "smwuema"}
    Private Shared objSingle As clsUtilities
    Private Shared blCreated As Boolean
#End Region
#Region "Singleton function"
    Private Sub New()
        'Override the default constructor
    End Sub
    Public Shared Function getObject() As clsUtilities
        If blCreated = False Then
            objSingle = New clsUtilities
            blCreated = True

            Return objSingle
        Else
            Return objSingle
        End If
    End Function
#End Region

#Region " procedures"
    ''' <summary>
    ''' use to get the age of an individual when given the persons birthday
    ''' </summary>
    ''' <param name="birthDay"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getAge(ByVal birthDay As Date) As Integer
        Dim Years As Integer
        Dim dtA As Date
        Dim ts As TimeSpan

        dtA = birthDay

        ts = Now.Subtract(dtA)
        Years = Convert.ToInt32(ts.TotalDays) / 365
        Return Years
    End Function

    Public Function setDataView(ByVal bs As BindingSource, ByVal tableName As String, Optional ByVal description As String = "", Optional ByVal enableDeletions As Boolean = False) As Boolean
        HRS_Desktop.dgV_General.DataSource = Nothing
        HRS_Desktop.lblGridName.Text = tableName & " " & description & " Data View"
        HRS_Desktop.dgV_General.DataSource = bs
        HRS_Desktop.pnl_Docking.Height = HRS_Desktop.pnl_Docking.MaximumSize.Height
        HRS_Desktop.enabledelete = enableDeletions
        HRS_Desktop.dataviewTableName = tableName
        HRS_Desktop.btn_newWindow.Enabled = True

        Return True
    End Function
    Public Function isAuthorizedUser() As Boolean
        Dim sarr As New List(Of String)
        sarr.AddRange(authorizedUsers)
        If sarr.Contains(objRef.ObjSingleton.userName.ToLower) Then Return True
    End Function
#End Region

End Class


