Public Class clsvalidations

#Region "Variable Declaration"
    Public objRef As clsformrefrences = clsformrefrences.getObject
    Private Shared objSingle As clsvalidations
    Private Shared blCreated As Boolean
#End Region
#Region "Singleton function"
    Private Sub New()
        'Override the default constructor
    End Sub
    Public Shared Function getObject() As clsvalidations
        If blCreated = False Then
            objSingle = New clsvalidations
            blCreated = True

            Return objSingle
        Else
            Return objSingle
        End If
    End Function
#End Region
#Region "procedured"
    Public Function getIDSubstring(ByVal delimiteredString As String, ByVal idType As idTypes) As String
        Dim arry As String() = delimiteredString.Split("-"c)
        Dim returnString As String = ""
        If arry.Length > idType Then
            For id As Integer = 0 To idType
                returnString = returnString & arry(id) & "-"
            Next
            Return returnString.TrimEnd("-"c)
        Else
            Return "Q"
        End If
    End Function
#End Region

End Class

