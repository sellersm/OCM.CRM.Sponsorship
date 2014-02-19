Public Class RateIncreaseResponsesEditDataFormUIModel

    Private Sub RateIncreaseResponsesEditDataFormUIModel_Loaded(ByVal sender As Object, ByVal e As Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs) Handles Me.Loaded
        'CRMHelper.ShowMessage(Me.RATECHANGEIMPLEMENTATIONDATE.Value.ToString(), UIPromptButtonStyle.Ok, UIPromptImageStyle.Information, Me)
        'Me.RATECHANGEIMPLEMENTATIONDATETEXT.Value = Date.Parse(Date.Now().ToString()) & " - " & Me.RATECHANGEIMPLEMENTATIONDATE.Value.ToString()
        If Date.Parse(Date.Now().ToString()) > Date.Parse(Me.RATECHANGEIMPLEMENTATIONDATE.Value.ToString()) Then
            Me.RESPONSEIFAFTERRATECHANGEIMPLEMENTEDCODEID.Visible = True
        End If
    End Sub


End Class