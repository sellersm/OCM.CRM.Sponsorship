Imports MoM.Sponsorship.UIModel.AddSponsorshipHelper
Imports MoM.Common

Public Class RecurringGiftExtensionEditDataFormUIModel

#Region "Private Vars"
	'Private Const errorTextCouldNotFindinboundChannelResponseForm As String = "Could not find 'Response Form' Channel code table value."
	Private Const errorTextCouldNotFindCreditCardSchedule As String = "Could not find credit card schedule code table value for - "
	Private Const errorTextCouldNotFindDirectDebitSchedule As String = "Could not find direct debit schedule code table value for - "

	Private Const defaultStartingOnDayOfMonth As Integer = 17				'The day of the month to default the Starting On date to
	Private Const defaultMonthlyChildSponsorshipAmount As Integer = 34		'This should be pulled from the program, but for now just setting to $34

	Private Const frequencyAnnually As Integer = 0
	Private Const frequencySemiAnnually As Integer = 1
	Private Const frequencyQuarterly As Integer = 2
	Private Const frequencyMonthly As Integer = 3

	Private Const paymentMethodeCodeCash As Integer = 0
	Private Const paymentMethodeCodeCheck As Integer = 1
	Private Const paymentMethodeCodeCreditCard As Integer = 2
	Private Const paymentMethodeCodeDirectDebit As Integer = 3

	Private Property model As UIModeling.Core.RootUIModel = Nothing

#End Region

#Region "Properties"
	Property helperMode As AddSponsorshipFormMode		'Used for specific behavior for view form vs edit form

	' Code Table IDs
	Property inboundChannelResponseForm As String = ""

	Property creditCardScheduleMontly1stWeek As String = ""
	Property creditCardScheduleMontly2ndWeek As String = ""
	Property creditCardScheduleMontly3rdWeek As String = ""
	Property creditCardScheduleMontly4thWeek As String = ""

	Property creditCardScheduleQuarterlyMonth1 As String = ""
	Property creditCardScheduleQuarterlyMonth2 As String = ""
	Property creditCardScheduleQuarterlyMonth3 As String = ""

	Property creditCardScheduleAnnuallyJanuary As String = ""
	Property creditCardScheduleAnnuallyFebruary As String = ""
	Property creditCardScheduleAnnuallyMarch As String = ""
	Property creditCardScheduleAnnuallyApril As String = ""
	Property creditCardScheduleAnnuallyMay As String = ""
	Property creditCardScheduleAnnuallyJune As String = ""
	Property creditCardScheduleAnnuallyJuly As String = ""
	Property creditCardScheduleAnnuallyAugust As String = ""
	Property creditCardScheduleAnnuallySeptember As String = ""
	Property creditCardScheduleAnnuallyOctober As String = ""
	Property creditCardScheduleAnnuallyNovember As String = ""
	Property creditCardScheduleAnnuallyDecember As String = ""


	Property directDebitScheduleMonthly1st As String = ""
	Property directDebitScheduleMonthly20th As String = ""
	Property directDebitScheduleMonthly10th As String = ""

	Property directDebitScheduleQuarterlyMonth1_1st As String = ""
	Property directDebitScheduleQuarterlyMonth1_10th As String = ""
	Property directDebitScheduleQuarterlyMonth1_20th As String = ""
	Property directDebitScheduleQuarterlyMonth2_1st As String = ""
	Property directDebitScheduleQuarterlyMonth2_10th As String = ""
	Property directDebitScheduleQuarterlyMonth2_20th As String = ""
	Property directDebitScheduleQuarterlyMonth3_1st As String = ""
	Property directDebitScheduleQuarterlyMonth3_10th As String = ""
	Property directDebitScheduleQuarterlyMonth3_20th As String = ""
#End Region

	' Translation = "Credit card"})
	' PAYMENTMETHODCODES) With {.Value = PAYMENTMETHODCODES.[DirectDebit], .Translation = "Direct debit"

	Private Sub RecurringGiftExtensionEditDataFormUIModel_Loaded(ByVal sender As Object, ByVal e As Blackbaud.AppFx.UIModeling.Core.LoadedEventArgs) Handles Me.Loaded
		'turn on the correct field based on paymentmethod text
		Select Case Me.PAYMENTMETHODTEXT.Value.ToString().ToLower()
			Case "credit card"
				Me.DIRECTDEBITSCHEDULECODEID.Visible = False
				Me.DIRECTDEBITSCHEDULECODEID.Enabled = False
				Me.CREDITCARDSCHEDULECODEID.Visible = True
				Me.CREDITCARDSCHEDULECODEID.Enabled = True

			Case "direct debit"
				Me.DIRECTDEBITSCHEDULECODEID.Visible = True
				Me.DIRECTDEBITSCHEDULECODEID.Enabled = True
				Me.CREDITCARDSCHEDULECODEID.Visible = False
				Me.CREDITCARDSCHEDULECODEID.Enabled = False

			Case Else
				'leave them turned off
				Me.DIRECTDEBITSCHEDULECODEID.Visible = False
				Me.DIRECTDEBITSCHEDULECODEID.Enabled = False
				Me.CREDITCARDSCHEDULECODEID.Visible = False
				Me.CREDITCARDSCHEDULECODEID.Enabled = False
		End Select

		AddHandler Me.FREQUENCYCODE.ValueChanged, AddressOf FrequencyCode_ValueChanged
		'AddHandler Me.AUTOPAY.ValueChanged, AddressOf Autopay_ValueChanged
		'AddHandler Me.PAYMENTMETHODCODE.ValueChanged, AddressOf PaymentMethodCode_ValueChanged
		AddHandler Me.CREDITCARDSCHEDULECODEID.ValueChanged, AddressOf CreditCardScheduleCodeID_ValueChanged
		AddHandler Me.DIRECTDEBITSCHEDULECODEID.ValueChanged, AddressOf DirectDebitScheduleCodeID_ValueChanged

		InitializeCodeTableVars()

		Me.AMOUNT.DBReadOnly = False
		Me.AMOUNT.Enabled = False

	End Sub

	' Set Frequcney and Starting On Date based on Credit Card schedule selected
	Private Sub CreditCardScheduleCodeID_ValueChanged(ByVal sender As Object, ByVal e As Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs)
		Select Case Me.CREDITCARDSCHEDULECODEID.Value.ToString()
			Case creditCardScheduleMontly1stWeek
				Me.REVENUESCHEDULESTARTDATE.Value = GetStartingOnDateByDay(1)
				Me.FREQUENCYCODE.Value = frequencyMonthly
			Case creditCardScheduleMontly2ndWeek
				Me.REVENUESCHEDULESTARTDATE.Value = GetStartingOnDateByDay(8)
				Me.FREQUENCYCODE.Value = frequencyMonthly
			Case creditCardScheduleMontly3rdWeek
				Me.REVENUESCHEDULESTARTDATE.Value = GetStartingOnDateByDay(15)
				Me.FREQUENCYCODE.Value = frequencyMonthly
			Case creditCardScheduleMontly4thWeek
				Me.REVENUESCHEDULESTARTDATE.Value = GetStartingOnDateByDay(22)
				Me.FREQUENCYCODE.Value = frequencyMonthly

			Case creditCardScheduleQuarterlyMonth1, creditCardScheduleQuarterlyMonth2, creditCardScheduleQuarterlyMonth3
				Me.REVENUESCHEDULESTARTDATE.Value = GetStartingOnDateByDay(defaultStartingOnDayOfMonth)
				Me.FREQUENCYCODE.Value = frequencyQuarterly

			Case creditCardScheduleAnnuallyJanuary, creditCardScheduleAnnuallyFebruary, creditCardScheduleAnnuallyMarch,
			  creditCardScheduleAnnuallyApril, creditCardScheduleAnnuallyMay, creditCardScheduleAnnuallyJune,
			  creditCardScheduleAnnuallyJuly, creditCardScheduleAnnuallyAugust, creditCardScheduleAnnuallySeptember,
			  creditCardScheduleAnnuallyOctober, creditCardScheduleAnnuallyNovember, creditCardScheduleAnnuallyDecember

				Me.REVENUESCHEDULESTARTDATE.Value = GetStartingOnDateByDay(defaultStartingOnDayOfMonth)
				Me.FREQUENCYCODE.Value = frequencyAnnually

		End Select

		ShowMessage("Be sure to check the dates of this recurring gift.", UIPromptButtonStyle.Ok, UIPromptImageStyle.Information)

	End Sub


	' Set Frequcney and Starting On Date based on Direct Debit schedule selected
	Private Sub DirectDebitScheduleCodeID_ValueChanged(ByVal sender As Object, ByVal e As Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs)
		Select Case Me.DIRECTDEBITSCHEDULECODEID.Value.ToString
			Case directDebitScheduleMonthly1st, directDebitScheduleQuarterlyMonth1_1st, directDebitScheduleQuarterlyMonth2_1st, directDebitScheduleQuarterlyMonth3_1st
				Me.REVENUESCHEDULESTARTDATE.Value = GetStartingOnDateByDay(1)

			Case directDebitScheduleMonthly10th, directDebitScheduleQuarterlyMonth1_10th, directDebitScheduleQuarterlyMonth2_10th, directDebitScheduleQuarterlyMonth3_10th
				Me.REVENUESCHEDULESTARTDATE.Value = GetStartingOnDateByDay(10)

			Case directDebitScheduleMonthly20th, directDebitScheduleQuarterlyMonth1_20th, directDebitScheduleQuarterlyMonth2_20th, directDebitScheduleQuarterlyMonth3_20th
				Me.REVENUESCHEDULESTARTDATE.Value = GetStartingOnDateByDay(20)
		End Select

		Select Case Me.DIRECTDEBITSCHEDULECODEID.Value.ToString

			Case directDebitScheduleMonthly1st, directDebitScheduleMonthly10th, directDebitScheduleMonthly20th
				Me.FREQUENCYCODE.Value = frequencyMonthly

			Case directDebitScheduleQuarterlyMonth1_1st, directDebitScheduleQuarterlyMonth1_10th, directDebitScheduleQuarterlyMonth1_20th, _
			 directDebitScheduleQuarterlyMonth2_1st, directDebitScheduleQuarterlyMonth2_10th, directDebitScheduleQuarterlyMonth2_20th, _
			 directDebitScheduleQuarterlyMonth3_1st, directDebitScheduleQuarterlyMonth3_10th, directDebitScheduleQuarterlyMonth3_20th
				Me.FREQUENCYCODE.Value = frequencyQuarterly
		End Select

		ShowMessage("Be sure to check the dates of this recurring gift.", UIPromptButtonStyle.Ok, UIPromptImageStyle.Information)

	End Sub

	' Determine the starting on date based on the day passed in and the current date
	' If the day is greater than or equal to today move it to the next month
	Private Function GetStartingOnDateByDay(ByVal Day As Integer) As Date
		Dim startingOnDate As Date

		If Today.Day >= Day Then
			startingOnDate = DateAdd(DateInterval.Month, 1, New Date(Today.Year, Today.Month, Day))
		Else
			startingOnDate = New Date(Today.Year, Today.Month, Day)
		End If

		Return startingOnDate
	End Function

	Public Sub InitializeCodeTableVars()
		'Initialize Code Table IDs 
		Using crmSQLConnection = GetRequestContext().OpenAppDBConnection()
			'inboundChannelResponseForm = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CHANNEL_RESPONSEFORM, True, errorTextCouldNotFindinboundChannelResponseForm)

			creditCardScheduleMontly1stWeek = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_MONTHLY1STWEEK, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_MONTHLY1STWEEK)
			creditCardScheduleMontly2ndWeek = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_MONTHLY2NDWEEK, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_MONTHLY2NDWEEK)
			creditCardScheduleMontly3rdWeek = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_MONTHLY3RDWEEK, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_MONTHLY3RDWEEK)
			creditCardScheduleMontly4thWeek = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_MONTHLY4THWEEK, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_MONTHLY4THWEEK)

			creditCardScheduleQuarterlyMonth1 = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_QUARTERLYMONTH1JANAPRJULOCT, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_QUARTERLYMONTH1JANAPRJULOCT)
			creditCardScheduleQuarterlyMonth2 = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_QUARTERLYMONTH2FEBMAYAUGNOV, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_QUARTERLYMONTH2FEBMAYAUGNOV)
			creditCardScheduleQuarterlyMonth3 = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_QUARTERLYMONTH3MARJUNSEPDEC, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_QUARTERLYMONTH3MARJUNSEPDEC)

			creditCardScheduleAnnuallyJanuary = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYJANUARY, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYJANUARY)
			creditCardScheduleAnnuallyFebruary = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYFEBRUARY, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYFEBRUARY)
			creditCardScheduleAnnuallyMarch = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYMARCH, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYMARCH)
			creditCardScheduleAnnuallyApril = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYAPRIL, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYAPRIL)
			creditCardScheduleAnnuallyMay = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYMAY, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYMAY)
			creditCardScheduleAnnuallyJune = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYJUNE, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYJUNE)
			creditCardScheduleAnnuallyJuly = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYJULY, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYJULY)
			creditCardScheduleAnnuallyAugust = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYAUGUST, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYAUGUST)
			creditCardScheduleAnnuallySeptember = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYSEPTEMBER, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYSEPTEMBER)
			creditCardScheduleAnnuallyOctober = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYOCTOBER, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYOCTOBER)
			creditCardScheduleAnnuallyNovember = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYNOVEMBER, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYNOVEMBER)
			creditCardScheduleAnnuallyDecember = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYDECEMBER, True, errorTextCouldNotFindCreditCardSchedule & CodeTableFields.CREDITCARDSCHEDULE_ANNUALLYDECEMBER)


			directDebitScheduleMonthly1st = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.DIRECTDEBITSCHEDULE_MONTHLY1ST, True, errorTextCouldNotFindDirectDebitSchedule & CodeTableFields.DIRECTDEBITSCHEDULE_MONTHLY1ST)
			directDebitScheduleMonthly10th = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.DIRECTDEBITSCHEDULE_MONTHLY10TH, True, errorTextCouldNotFindDirectDebitSchedule & CodeTableFields.DIRECTDEBITSCHEDULE_MONTHLY10TH)
			directDebitScheduleMonthly20th = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.DIRECTDEBITSCHEDULE_MONTHLY20TH, True, errorTextCouldNotFindDirectDebitSchedule & CodeTableFields.DIRECTDEBITSCHEDULE_MONTHLY20TH)

			directDebitScheduleQuarterlyMonth1_1st = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH1JANAPRJULOCT1ST, True, errorTextCouldNotFindDirectDebitSchedule & CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH1JANAPRJULOCT1ST)
			directDebitScheduleQuarterlyMonth1_10th = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH1JANAPRJULOCT10TH, True, errorTextCouldNotFindDirectDebitSchedule & CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH1JANAPRJULOCT10TH)
			directDebitScheduleQuarterlyMonth1_20th = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH1JANAPRJULOCT20TH, True, errorTextCouldNotFindDirectDebitSchedule & CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH1JANAPRJULOCT20TH)
			directDebitScheduleQuarterlyMonth2_1st = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH2FEBMAYAUGNOV1ST, True, errorTextCouldNotFindDirectDebitSchedule & CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH2FEBMAYAUGNOV1ST)
			directDebitScheduleQuarterlyMonth2_10th = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH2FEBMAYAUGNOV10TH, True, errorTextCouldNotFindDirectDebitSchedule & CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH2FEBMAYAUGNOV10TH)
			directDebitScheduleQuarterlyMonth2_20th = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH2FEBMAYAUGNOV20TH, True, errorTextCouldNotFindDirectDebitSchedule & CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH2FEBMAYAUGNOV20TH)
			directDebitScheduleQuarterlyMonth3_1st = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH3MARJUNSEPDEC1ST, True, errorTextCouldNotFindDirectDebitSchedule & CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH3MARJUNSEPDEC1ST)
			directDebitScheduleQuarterlyMonth3_10th = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH3MARJUNSEPDEC10TH, True, errorTextCouldNotFindDirectDebitSchedule & CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH3MARJUNSEPDEC10TH)
			directDebitScheduleQuarterlyMonth3_20th = CRMHelper.GetCodeTableItemID(crmSQLConnection, CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH3MARJUNSEPDEC20TH, True, errorTextCouldNotFindDirectDebitSchedule & CodeTableFields.DIRECTDEBITSCHEDULE_QUARTERLYMONTH3MARJUNSEPDEC20TH)

		End Using
	End Sub

	Private Sub FrequencyCode_ValueChanged(ByVal sender As Object, ByVal e As Blackbaud.AppFx.UIModeling.Core.ValueChangedEventArgs)
		' Only update the amount based on the frequency if it is a child sponsorship		
		Select Case e.NewValue
			Case frequencyAnnually
				Me.AMOUNT.Value = defaultMonthlyChildSponsorshipAmount * 12

			Case frequencySemiAnnually
				Me.AMOUNT.Value = defaultMonthlyChildSponsorshipAmount * 6

			Case frequencyQuarterly
				Me.AMOUNT.Value = defaultMonthlyChildSponsorshipAmount * 3

			Case frequencyMonthly
				Me.AMOUNT.Value = defaultMonthlyChildSponsorshipAmount
		End Select
	End Sub

	' CAN NOT get these events to fire: moved it up to the changed event of the debit and credit codes!
	'Private Sub RecurringGiftExtensionEditDataFormUIModel_Saved(ByVal sender As Object, ByVal e As Blackbaud.AppFx.UIModeling.Core.SavedEventArgs) Handles Me.Saved
	'	ShowMessage("Be sure to check the dates of this recurring gift.", UIPromptButtonStyle.Ok, UIPromptImageStyle.Exclamation)
	'End Sub

	'Private Sub RecurringGiftExtensionEditDataFormUIModel_Saving(ByVal sender As Object, ByVal e As Blackbaud.AppFx.UIModeling.Core.SavingEventArgs) Handles Me.Saving
	'	' prompt user to remind them to check the starting date of the 
	'	ShowMessage("Be sure to check the dates of this recurring gift.", UIPromptButtonStyle.Ok, UIPromptImageStyle.Warning)
	'	'Me.Prompts.Add(New UIPrompt() With { _
	'	'  .Text = "Be sure to check the Dates of this recurring gift.", _
	'	'  .ButtonStyle = UIPromptButtonStyle.Ok})
	'End Sub

	Private Sub ShowMessage(ByVal message As String, ByVal buttonStyle As UIPromptButtonStyle, ByVal imageStyle As UIPromptImageStyle)
		Dim prompt As New UIPrompt
		prompt.Text = message
		prompt.ImageStyle = imageStyle
		prompt.ButtonStyle = buttonStyle
		Me.Prompts.Add(prompt)
	End Sub
End Class