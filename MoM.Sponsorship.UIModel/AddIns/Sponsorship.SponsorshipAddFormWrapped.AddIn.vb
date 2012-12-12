
Partial Public NotInheritable Class SponsorshipAddIn

	Dim myAddSponsorshipHelper As AddSponsorshipHelper

	Private Sub OnInit()
		'This method is called when the UI model is created to allow any initialization to be performed.

		myAddSponsorshipHelper = New AddSponsorshipHelper(MODEL, AddSponsorshipHelper.AddSponsorshipFormMode.ChildSponsorship)
		myAddSponsorshipHelper.InitializeCodeTableVars()
		myAddSponsorshipHelper.InitializeForm()


	End Sub


	Private Sub ModelValidated(ByVal Sender As Object, ByVal e As Blackbaud.AppFx.UIModeling.Core.ValidatedEventArgs) Handles MODEL.Validated
		myAddSponsorshipHelper.ValidateModel(Sender, e)
	End Sub
End Class

