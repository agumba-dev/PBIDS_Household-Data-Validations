Public Class clsSingleton
    'private static Singleton instance;
    Private Shared m_instance As clsSingleton
    Private Shared m_instanceUtil As clsSingleton
    Private strUsn As String ' the windows loggin username
    Private strAuth As String
    Private strPwd As String
    Private strUser As String
    Private strRound As String
    Private strSelQry As String
    Private strRegion As String
    '// Note: Constructor is 'protected' 
    Public Sub New(ByVal strU As String, ByVal strA As String, ByVal strP As String, ByVal strUs As String, ByVal strRnd As String, ByVal strRgn As String)
        strUsn = strU
        strAuth = strA
        strPwd = strP
        strUser = strUs
        strRound = strRnd
        strRegion = strRgn
    End Sub

    Public Shared Function SingletonInstance(ByVal strU As String, ByVal strA As String, ByVal strP As String, ByVal strUs As String, ByVal strRnd As String, ByVal strRgn As String) As clsSingleton
        '// Use 'Lazy initialization' 

        If m_instance Is Nothing Then
            m_instance = New clsSingleton(strU, strA, strP, strUs, strRnd, strRgn)
        End If

        Return m_instance
    End Function
    Public Shared Sub SingletonDestructor()
        '// Use 'Lazy initialization' 
        m_instance = Nothing
        'm_instanceUtil = Nothing
    End Sub
    Public Function userName() As String
        Return strUsn
        '//set { username = value; }
    End Function

    Public Function authLevel() As String
        Return strAuth
        '//set { userRights = value; }
    End Function

    Public Function passWord() As String
        Return strPwd
        '//set { userRights = value; }
    End Function
    Public Function user() As String
        Return strUser
        '//set { userRights = value; }
    End Function

    Public Function round() As String
        Return strRound
        '//set { userRights = value; }
    End Function
    Public Function Region() As String
        Return strRegion
        '//set { username = value; }
    End Function
End Class
