Imports System.Data
Imports System.Data.SqlClient

Public Class clsEventEpisode
    Private da As clsDataAccess = clsDataAccess.getObject
    Private Function existsinMain(ByVal EventEpisoderecord As DataRow) As Boolean
        Dim returnValue As Boolean = True
        Dim sql As String = "SELECT count(*) FROM [DSSHRS].[DSS].[Events_Episodes]" _
                            & " where  (EventID='" + EventEpisoderecord("EventID").ToString + "') " _
            & " AND (EpisodeID='" + EventEpisoderecord("EpisodeID").ToString + "')" _
            & "  AND (EpisodeType='" + EventEpisoderecord("EpisodeType").ToString + "')"
       
        If Me.da.executeScalar_INMainDB(sql) > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If
        Return returnValue
    End Function
    Friend Sub validateEventEpisode(ByVal EventEpisoderecords As DataTable, ByVal worker As System.ComponentModel.BackgroundWorker)
        If EventEpisoderecords.Rows.Count > 0 Then
            For Each record As DataRow In EventEpisoderecords.Rows
                If Me.existsinMain(record) Then
                    Me.da.exec_nonqueryInTEMPDB("UPDATE [TEMP_DSSHRS].[DSS].[Events_Episodes] SET [errflag] = 'true', errdate=getdate() , rec_status='XI' where transit_id=" + record("transit_id").ToString)
                    'Me.da.saveError(record("transit_id").ToString.Trim, tablename, "Unknown operation", "", Now(), "", Village, round)
                End If
            Next
        End If
    End Sub
End Class
