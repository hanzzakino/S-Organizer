Public Class frm_LoadingScreen

    Private Sub frm_LoadingScreen_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ''Get version''
        Dim str_vesion As String = FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion
        lbl_VERSION.Text = "" + str_vesion
    End Sub

End Class